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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Diagnostics;

namespace ClipPlay
{
    public partial class PronunciationsForm : Form
    {
        // You can't have methods in a constant. :( 
        private String iniFileLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ClipReader\\pronunciation.ini";
        public PronunciationsForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// From load event.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">EventArgs <see cref="Event Args"/></param>
        private void PronunciationForm_Load(object sender, EventArgs e)
        {
            FileStream fs = File.OpenRead(iniFileLocation);
            StreamReader sr = new StreamReader(fs);
            _txtIniText.Text = sr.ReadToEnd();
            sr.Close();
            _txtSelectMe.Select();
            // ^ _txtSelectMe is a work around so that the text is not selected.
            if (Properties.PronunciationForm.Default.maximized)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            this.Height = Properties.PronunciationForm.Default.height;
            this.Width = Properties.PronunciationForm.Default.width;

            this.Location = Properties.PronunciationForm.Default.location;

        }

        private void _btnHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Type pronunciation in the form <word>=<correct pronunciation> \n with out the '<' and '>'. \n Push enter between items. \n Leave the \"[pronunciation]\" at the top!"); 
        }

        private void _btnOk_Click(object sender, EventArgs e)
        {
            if (validateINI())
            {
                try
                {
                    StreamWriter sw = new StreamWriter(new FileStream(iniFileLocation, FileMode.Create));
                    sw.Write(_txtIniText.Text);
                    sw.Close();
                    sw = null;
                    this.Close();
                }
                catch (UnauthorizedAccessException error)
                {
                    Debug.WriteLine(error.Message);
                    MessageBox.Show("Your pronunciation file(pronunciation.ini) is not writable, so your settings were not saved.\n" +
                                    " Please fix this and try again!", "File write error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close(); 
                }
            }
        }

        private bool validateINI()
        {
            String[] lines = _txtIniText.Text.Split('\n');
            // search for a pronunciation section [
            bool found = false;
            bool valid = true;
            //Valid is true until we find something to switch it.
            // It is used later on. 
            int posOfPron = 0;
            foreach(String line in lines){
                if (line.Equals("[pronunciation]\r") || line.Equals("[pronunciation]"))
                {
                    found = true;
                    break; 
                }
                posOfPron++; 
            }
            if (found == false)
            {
                MessageBox.Show("No \"[pronunciation]\" section, please add \"[pronunciation]\"" +
                "to the top of the dialog text. This is in case the ini file generated contains other 'sections'.", "Validation error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else
            {
                for (int counter = posOfPron; counter < lines.Length; counter++)
                {
                    if (lines[counter].StartsWith("[") && lines[counter].EndsWith("]\r"))
                    {
                        continue;
                    }
                    if (lines[counter].StartsWith("[") && lines[counter].EndsWith("]"))
                    {
                        continue; //anouther possiblity
                    }
                    if (lines[counter] == "") {
                        continue; 
                        // skip it
                    }

                    // This parser does not care about "[" in the middle of key/value 
                    // there should be 1 and only one = 
                    int equalsSignCount = 0;
                    foreach (char c in lines[counter])
                    {
                        if (c == '=')
                        {
                            equalsSignCount++; 
                        }
                    }
                    if (equalsSignCount == 0)
                    {
                        MessageBox.Show("You forgot a '='!", "You forgot something", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                    else if (equalsSignCount > 1)
                    {
                        MessageBox.Show("Only one '=' is allowed per line!", "Error with data.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    // I don't care if a word is pronounced ' ' after parsing. 
                }
            }
            return valid; // replace 
        }

        private void PronunciationsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.PronunciationForm.Default.location = this.Location;
           
            if(this.WindowState == FormWindowState.Maximized){// we will not start minimized even if that was the last window state, that is confusing. 
                Properties.PronunciationForm.Default.maximized = true;
            }
            else
            {
                Properties.PronunciationForm.Default.width = this.Width;
                Properties.PronunciationForm.Default.height = this.Height; 
            }
            Properties.PronunciationForm.Default.Save();
        }



    }
}
