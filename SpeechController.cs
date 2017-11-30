/***
    * Copyright (c) 2015, James Marchant
      All rights reserved.

      Redistribution and use in source and binary forms, with or without
      modification, are permitted provided that the following conditions are met: 

       1. Redistributions of source code must retain the above copyright notice, this
          list of conditions and the following disclaimer. 
       2. Redistributions in binary form must reproduce the above copyright notice,
          this list of conditions and the following disclaimer in the documentation
          and/or other materials provided with the distribution. 

          THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
          ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
          WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
          DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
          ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
          (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
          LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
          ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
          (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
           SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

           The views and conclusions contained in the software and documentation are those
           of the authors and should not be interpreted as representing official policies, 
           either expressed or implied, of the FreeBSD Project.
    * 
    * */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Speech.Synthesis;
using System.IO;
using System.Collections.ObjectModel;

namespace ClipPlay
{
    /// <summary>
    /// This class controls the TTS(Text-To-Speech). Inherits from NativeWindow to use WndProc. 
    /// </summary>
    /// <remarks>
    /// In the main program, it is used with the SetClipboardViewer external method
    /// to take clipboard messages. 
    /// </remarks>
    class SpeechController : NativeWindow, IDisposable
    {
       // private Boolean canSpeak = true; 
        private String _lastRead = "";
        private Prompt _prompt;
        private static bool S_isReading = false; 
        private bool _copyMessage = false; 
        private bool _paused = false; 
        private bool _startingUp = true;
        // Several warnings are only displayed once per program run.
        private bool _userWarnedOfMissingFile = false; 
        private bool _userNotified = false; 
        // to avoid reading what was on the clipboard last 
        private static SpeechSynthesizer S_ss;// This needs to be static to stop the synthesizer from 
       
        // reading over itself.
        const int WM_DRAWCLIPBOARD = 0x0308;
        private String iniFileLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ClipReader\\pronunciation.ini";
        /***
         * Now I realize that I could have events for this, but I think this is simpler 
         * and creates less overhead. 
         * */ 
        private Program _gram;
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_DRAWCLIPBOARD:
                    read();
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }
        static SpeechController()
        {
            S_ss = new SpeechSynthesizer();
            S_ss.Volume = 100;
            S_ss.SpeakStarted += S_ss_SpeakStarted;
            S_ss.SpeakCompleted += S_ss_SpeakCompleted;
        }

        private static void S_ss_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            S_isReading = false; 
        }

        private static void S_ss_SpeakStarted(object sender, SpeakStartedEventArgs e)
        {
            S_isReading = true;
        }
        /// <summary>
        /// Gets a string[] of the voices installed on your machine. 
        /// </summary>
        /// <remarks>
        /// Static for there is only one set of voices for the OS. 
        /// </remarks>
        /// <returns>A string[] of the voices installed on your machine. </returns>
        public static string[] getInstalledVoiceNames()
        {
           ReadOnlyCollection<InstalledVoice> ivc = S_ss.GetInstalledVoices();
           string[] returnMe = new string[ivc.Count];
           for (int counter = 0; counter < ivc.Count; counter++)
           {
               returnMe[counter] = ivc[counter].VoiceInfo.Name; 
           }
           return returnMe;
        }

        public SpeechController(Program gram)
        {
            this._gram = gram;
        }
        public void stop()
        {
            S_ss.SpeakAsyncCancelAll();
        }
        public void read()
        {
            read(false, Clipboard.GetText());
        }
        public void read(bool reReadAnyways)
        {
            read(reReadAnyways, Clipboard.GetText());
        }
        public void read(string messageToRead)
        {
            read(false, messageToRead);
        }
        /// <summary>
        /// Plays the conents of the clipboard. 
        /// </summary>
        /// <param name="reReadAnyways">True if you want to read something regradless of weather it's on the clipboard or not.</param>
        private void read(bool reReadAnyways, string messageToRead)
        {
            if (_paused && _userNotified == false && reReadAnyways == false)
            {
                _gram.notifyUser("When paused , any text copied to the clipboard is ignored!",
                                 "That won't be read!", ToolTipIcon.Info);
                _userNotified = true; 
                // only occurs once per program run 
            }
            if (_paused)
            {
                Debug.WriteLine("Reading is off, so text is not read.");
                return; 
            }
            if (_startingUp)
            {
                _startingUp = false;
                return; 
            }
            if (Clipboard.ContainsText() == false)
            {
                Debug.WriteLine("The clipboard does not contain text");
            }
            else
            {
                String withPronunciations = getPronunciations(messageToRead);
                if (_lastRead == withPronunciations && reReadAnyways == false)
                {
                    Debug.WriteLine("Data already on the clipboard; will not be read!"); 
                    if (_copyMessage == false)
                    {
                        _gram.notifyUser(0, "This program will not read the same message from the clipboard twice." +
                                         " This is because some programs like Acrobat Reader will 'abuse' the clipboard. Use the 're-read' menu button to re-read text.",
                                         "Will not read twice. ", ToolTipIcon.Warning);
                        _copyMessage = true; 
                    }
                    return; // Don't read again. 
                }
                else
                {
                    _lastRead = withPronunciations; 
                }
                if (_prompt != null)
                {
                    if (_prompt.IsCompleted == false)
                    {
                        // If prompt is not complete, override the speech. 
                        S_ss.SpeakAsyncCancel(_prompt); 
                    }
                }
                _prompt = S_ss.SpeakAsync(withPronunciations);

                _gram.enableLastRead(); 

            }
        }

        private string getPronunciations(string clipBoardText)
        {
            if (File.Exists(iniFileLocation) == false)
            {
                if (_userWarnedOfMissingFile == false)
                {
                    _gram.notifyUser("'Pronunciation.ini' file is missing, so pronunciations will not be used.", "No pronunciations", ToolTipIcon.Error);
                    _userWarnedOfMissingFile = true; 
                }
                return clipBoardText;
            }
            String[] allLines = File.ReadAllLines(iniFileLocation); 
            int pronunciationSectionIndex = 0; 
            foreach(String line in allLines){
                // 1. find the pronunciation section 
                if (line.Equals("[pronunciation]"))
                { 
                    break; 
                }
                pronunciationSectionIndex++; 
            }
            char[] spaceSep = { ' ' };
            String[] words = clipBoardText.Split(spaceSep);
            for (int loopCounter = pronunciationSectionIndex; loopCounter < allLines.Length; loopCounter++)
            {
                if(allLines[loopCounter].Equals("[pronunciation]")){
                    // Skip this one.
                    continue; 
                }
                if (allLines[loopCounter].Contains("[") || allLines[loopCounter].Contains("]"))
                {
                    break; // You have found another section. 
                }
                // Otherwise, replace all the words of a type with what's in the file.
                char[] separators = { '=' };
                String oldValue = allLines[loopCounter].Split(separators)[0];
                String newValue = allLines[loopCounter].Split(separators)[1];
                // replace the words, String.Replace does not do this, 
                // It uses letters. I want words.
                int pos = 0; 
                foreach(String word in words){
                    if (word.Trim() == oldValue.Trim())
                    {
                        words[pos] = newValue.Trim(); 
                    }
                    pos++; 
                }
            }
            // now rebuild the string 
            StringBuilder sb = new StringBuilder(); 
            foreach(String word in words){
                sb.Append(word + " "); 
            }
            return sb.ToString();
        }
        /// <summary>
        /// Implementation of IDisposable <see cref="IDisposable"/>
        /// </summary>
        public void Dispose()
        {
            S_ss.Dispose(); 
        }
        /// <summary>
        /// Pauses reading from the clipboard. Copying is ignored while this is done. 
        /// </summary>
        public void pause()
        {
            _paused = true; 
            S_ss.Pause(); 
            
        }
        /// <summary>
        /// Resumes play of speech and resumes receiving of copy commands. 
        /// </summary>
        public void resume()
        {
            _paused = false;
            S_ss.Resume(); 
        }
        /// <summary>
        /// Property for getting whether or not the speech is paused.
        /// </summary>
        public bool paused
        {
            get
            {
                return _paused;
            }
        }
        /// <summary>
        /// Re-reads the last thing in the buffer.
        /// </summary>
        public void reRead()
        {
            if (_paused)
            {
                _gram.notifyUser("When paused , any text copied to the clipboard is ignored!",
                                "That won't be read!", ToolTipIcon.Info);
                _userNotified = true; 
            }
            _prompt = S_ss.SpeakAsync(_lastRead);
            if (S_isReading)
            {
                this.stop();
                this.read(true);
            }
        }
        /// <summary>
        /// Gets the text that was read last.
        /// </summary>
        public string lastRead
        {
            get
            {
                return _lastRead; 
            }
        }
        /// <summary>
        /// This method is called when the user was warned of the missing file on
        /// start up. 
        /// </summary>
        public void doNotWarnUserOfMissingFile()
        {
            _userWarnedOfMissingFile = true; 
        }
        public void changeVoice(String voiceName)
        {
            if (S_isReading)
            {
                this.stop();
                S_ss.SelectVoice(voiceName);
                this.reRead();
            }
            else
            {
                S_ss.SelectVoice(voiceName);
            }
        }

        internal string getCurrentVoice()
        {
            return S_ss.Voice.Name;
        }
    }
}
