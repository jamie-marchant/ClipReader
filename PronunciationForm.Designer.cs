/***
    * Copyright (c) 2012, James Marchant
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
namespace ClipPlay
{
    partial class PronunciationsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PronunciationsForm));
            this._txtIniText = new System.Windows.Forms.TextBox();
            this._btnOk = new System.Windows.Forms.Button();
            this._btnHelp = new System.Windows.Forms.Button();
            this._txtSelectMe = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // _txtIniText
            // 
            this._txtIniText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._txtIniText.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.82857F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._txtIniText.Location = new System.Drawing.Point(11, 10);
            this._txtIniText.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this._txtIniText.Multiline = true;
            this._txtIniText.Name = "_txtIniText";
            this._txtIniText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._txtIniText.Size = new System.Drawing.Size(835, 440);
            this._txtIniText.TabIndex = 0;
            // 
            // _btnOk
            // 
            this._btnOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._btnOk.Location = new System.Drawing.Point(342, 453);
            this._btnOk.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this._btnOk.Name = "_btnOk";
            this._btnOk.Size = new System.Drawing.Size(83, 29);
            this._btnOk.TabIndex = 1;
            this._btnOk.Text = "Ok";
            this._btnOk.UseVisualStyleBackColor = true;
            this._btnOk.Click += new System.EventHandler(this._btnOk_Click);
            // 
            // _btnHelp
            // 
            this._btnHelp.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._btnHelp.Location = new System.Drawing.Point(466, 452);
            this._btnHelp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this._btnHelp.Name = "_btnHelp";
            this._btnHelp.Size = new System.Drawing.Size(73, 30);
            this._btnHelp.TabIndex = 2;
            this._btnHelp.Text = "Help";
            this._btnHelp.UseVisualStyleBackColor = true;
            this._btnHelp.Click += new System.EventHandler(this._btnHelp_Click);
            // 
            // _txtSelectMe
            // 
            this._txtSelectMe.Location = new System.Drawing.Point(119, 35);
            this._txtSelectMe.Name = "_txtSelectMe";
            this._txtSelectMe.Size = new System.Drawing.Size(100, 22);
            this._txtSelectMe.TabIndex = 3;
            this._txtSelectMe.Visible = false;
            // 
            // PronunciationsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(848, 483);
            this.Controls.Add(this._txtSelectMe);
            this.Controls.Add(this._btnHelp);
            this.Controls.Add(this._btnOk);
            this.Controls.Add(this._txtIniText);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "PronunciationsForm";
            this.Text = "Custom Pronunciations";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PronunciationsForm_FormClosing);
            this.Load += new System.EventHandler(this.PronunciationForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _txtIniText;
        private System.Windows.Forms.Button _btnOk;
        private System.Windows.Forms.Button _btnHelp;
        private System.Windows.Forms.TextBox _txtSelectMe;
    }
}

