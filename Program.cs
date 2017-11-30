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
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using System.Threading;
namespace ClipPlay
{
   

    /***
     * This program uses Java style method cases (which begin with a lower case). The exception is event handlers. 
     * */ 
    class Program : ApplicationContext
    {
        private const string APPNAME = "ClipRead";
        private string _lastMessage;
        private NotifyIcon _ni;
        private static SpeechController S_win;
        private static ToolStripMenuItem S_voiceMenu; 
        private bool isEnabled = false; 
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThreadAttribute]
        static void Main()
        {
            
            bool created;
            Mutex mutex = new Mutex(false, APPNAME, out created);
            if (!created)
            {
                MessageBox.Show("You can (and should) only run one 'copy' of this program at a time.", 
                                 "You are already running a copy of this!", 
                                 MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Environment.Exit(1);
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Program gram = new Program();
            gram.createPronunciations(); 
            PronunciationsForm instance = new PronunciationsForm();
            instance.Visible = false;
            gram._ni = new NotifyIcon();
            gram._ni.Visible = true;
            gram._ni.ContextMenuStrip = new ContextMenuStrip();
            gram._ni.MouseClick += new MouseEventHandler(gram.ico_MouseClick);
            gram._ni.ContextMenuStrip.Items.Add("Re-Read", new Bitmap("No Image.png"));
            gram._ni.ContextMenuStrip.Items.Add("Pronunciations", new Bitmap("No image.png"));
            gram._ni.ContextMenuStrip.Items.Add("Pause", new Bitmap("No image.png"));
            gram._ni.ContextMenuStrip.Items.Add("Stop", new Bitmap("No image.png"));
            gram._ni.ContextMenuStrip.Items.Add("Voice", new Bitmap("No image.png")); 
            gram._ni.ContextMenuStrip.Items.Add("Exit", new Bitmap("No image.png"));
            
         
            gram._ni.ContextMenuStrip.Items[0].MouseUp += new MouseEventHandler(gram.ReRead_MouseUp);
            gram._ni.ContextMenuStrip.Items[0].Enabled = false;
            gram._ni.ContextMenuStrip.Items[1].MouseUp += new MouseEventHandler(gram.pro_MouseUp);
            gram._ni.ContextMenuStrip.Items[2].MouseUp += new MouseEventHandler(gram.play_MouseUp);
            gram._ni.ContextMenuStrip.Items[4].MouseUp += new MouseEventHandler(gram.voice_MouseUp); 
            gram._ni.ContextMenuStrip.Items[3].MouseUp += new MouseEventHandler(gram.stop_MouseUp);
            gram._ni.ContextMenuStrip.Items[5].MouseUp += new MouseEventHandler(gram.exit_MouseUp);
            gram._ni.BalloonTipClicked += gram.booleanTipClicked; ; 
            // sub menu 
            string[] voiceNames = SpeechController.getInstalledVoiceNames();
            foreach (string voiceName in voiceNames)
            {
                (gram._ni.ContextMenuStrip.Items[4] as ToolStripMenuItem).DropDownItems.
                 Add(voiceName);
            }
            S_voiceMenu = gram._ni.ContextMenuStrip.Items[4] as ToolStripMenuItem; 
            ToolStripItemCollection tsic = (gram._ni.ContextMenuStrip.Items[4] as ToolStripMenuItem).DropDownItems;
            for (int counter = 0; counter < tsic.Count; counter++)
            {
                tsic[counter].MouseUp += new MouseEventHandler(gram.voiceName_MouseUp);
            }

            gram._ni.Icon = new Icon("cr.ico");
            S_win = new SpeechController(gram);
            S_win.CreateHandle(new CreateParams()); 
            SetClipboardViewer(S_win.Handle);
            // first run. 
            if (String.IsNullOrEmpty(Properties.Voice.Default.VoiceHeard))
            {
                ToolStripMenuItem tsmi = gram._ni.ContextMenuStrip.Items[4] as ToolStripMenuItem;
                Debug.Write("First run.");
                setToDefaultVoice(tsmi);
            }
            else
            {
                String voice = Properties.Voice.Default.VoiceHeard;
                S_win.changeVoice(voice);
                ToolStripItem tsi = gram._ni.ContextMenuStrip.Items[4];
                ToolStripMenuItem tsmi = gram._ni.ContextMenuStrip.Items[4] as ToolStripMenuItem;
                bool voiceFound = setVoiceTick(voice);
                if (!voiceFound)
                {
                    Debug.Write("No voice found, first run?");
                    setToDefaultVoice(tsmi);
                }
            }
            gram.notifyUser("ClipReader is ready to read to you!", "Your reader is ready!", 
                            ToolTipIcon.Info); 
            Application.Run(gram);
        }

        private static bool setVoiceTick(String voice)
        {
            bool voiceFound = false;
            foreach (ToolStripItem item in S_voiceMenu.DropDownItems)
            {
                String asS = item.ToString(); 
                if (asS.Equals(voice))
                {
                    item.Image = new Bitmap("tick.png");
                    voiceFound = true;
                }
                else
                {
                    item.Image = null;// to make all images blank
                }
            }
            return voiceFound;
        }

        private static void setToDefaultVoice(ToolStripMenuItem tsmi)
        {
            String defaultVoice = S_win.getCurrentVoice(); // When no voice is explicitly set, the default is used.
            Properties.Voice.Default.VoiceHeard = defaultVoice;
            Properties.Voice.Default.Save();
            setVoiceTick(defaultVoice);
        }
        /// <summary>
        /// Creates the pronunciation file if it does not already exist. 
        /// </summary>
        private void createPronunciations()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + 
             "\\ClipReader\\pronunciation.ini";
            try
            {
                
                if (File.Exists(path) == false)
                {
                    if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ClipReader\\") == false)
                    {
                        Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ClipReader\\");
                        FileStream fs = File.Create(path);
                        writeToFile(fs); 
                    }
                    else
                    {
                       FileStream fs = File.Create(path);
                       writeToFile(fs); 
                    }
                }
            }
            catch (IOException error)
            {
                MessageBox.Show("Can you write files to" + path + "?", "Could not create 'custom pronunciations' file; feature will be disabled", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Debug.WriteLine(error.StackTrace); // like print stack trace in Java. 
                S_win.doNotWarnUserOfMissingFile(); 
                _ni.ContextMenuStrip.Items[1].Enabled = false; 
            }
        }

        private void writeToFile(FileStream fs)
        {
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("[pronunciation]");
            sw.Dispose(); 
        }
        private void exit_MouseUp(object sender, MouseEventArgs e)
        {
            this.Dispose(); 
            Application.Exit();
        }
        private void pro_MouseUp(object sender, MouseEventArgs e)
        {
            PronunciationsForm pf = new PronunciationsForm();
            pf.Show(); 
        }
        private void ico_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _ni.ContextMenuStrip.Show();
            }
            // this version does not support left clicking.
        }
        private void play_MouseUp(object sender, EventArgs e)
        {
            if (S_win.paused)
            {
                S_win.resume();
                changePausePlayText("Pause"); 
            }
            else
            {
                S_win.pause();
                changePausePlayText("Play"); 
            }
        }
        private void stop_MouseUp(object send, EventArgs e)
        {
            S_win.resume();
            changePausePlayText("Pause"); 
            S_win.stop();
        }
        private void voice_MouseUp(object sender, EventArgs e)
        {
            // not used anymore
        }
        public void voiceName_MouseUp(object sender, EventArgs e)
        {
            ToolStripItem itm = (ToolStripItem) sender;
            S_win.changeVoice(itm.Text);
            Properties.Voice.Default.VoiceHeard = itm.Text;
            Properties.Voice.Default.Save();
            setVoiceTick(itm.Text);
        }
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);
        /// <summary>
        /// New implementation of the Dispose method. Calls the base method. 
        /// </summary>
        public new void Dispose()
        {
            _ni.Dispose();
            S_win.Dispose();
            base.Dispose(); 
        }
        /// <summary>
        /// Display a tooltip above the icon for a short period of time. 
        /// </summary>
        /// <param name="message">The message body.</param>
        /// <param name="title">The title </param>
        /// <param name="icon">The icon</param>
        public void notifyUser(String message, String title, ToolTipIcon icon)
        {
            notifyUser(1000, message, title, icon); 
        }
        /// <summary>
        ///  Display a tooltip above the icon for a  short period of time. 
        /// </summary>
        /// <param name="timeout">The period of time to display the message for</param>
        /// <param name="message">The message to be displayed</param>
        /// <param name="title">The title.</param>
        /// <param name="icon">The icon to use.</param>
        public void notifyUser(int timeout, String message, String title, ToolTipIcon icon)
        {
            _ni.ShowBalloonTip(timeout, title, message, icon);
            _lastMessage = message;
        }

        private void booleanTipClicked(object sender, EventArgs e)
        {
            S_win.read(_lastMessage);
        }
        /// <summary>
        /// Changes the pause play text. 
        /// </summary>
        /// <remarks>
        /// To be used to change the text from pause to play and back again. 
        /// </remarks>
        /// <param name="text">The next text.</param>
        private void changePausePlayText(string text)
        {
            _ni.ContextMenuStrip.Items[2].Text = text; 
        }
        private void ReRead_MouseUp(object sender, EventArgs e)
        {
                S_win.reRead();
        }
        /// <summary>
        /// enables "re-read" after something has been read
        /// </summary>
        public void enableLastRead()
        {
            if (isEnabled == false)
            {
                _ni.ContextMenuStrip.Items[0].Enabled = true;
                _ni.ContextMenuStrip.Items[4].Enabled = true; 
                isEnabled = true;
            }// Make sure you only do this once.
        }
     
    }
}
