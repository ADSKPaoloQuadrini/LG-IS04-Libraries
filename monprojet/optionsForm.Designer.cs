namespace PTLGClassLibrary
{
    partial class optionsForm
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
            this.applyOptionsButton = new System.Windows.Forms.Button();
            this.quiteOptionsButton = new System.Windows.Forms.Button();
            this.braconCheckBox = new System.Windows.Forms.CheckBox();
            this.uBasCheckBox = new System.Windows.Forms.CheckBox();
            this.RetraitDScrollbar = new System.Windows.Forms.HScrollBar();
            this.RetraitGScrollbar = new System.Windows.Forms.HScrollBar();
            this.RetraitGTextbox = new System.Windows.Forms.TextBox();
            this.RetraitDTextbox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.GCGScrollBar = new System.Windows.Forms.VScrollBar();
            this.GCDScrollBar = new System.Windows.Forms.VScrollBar();
            this.RetraitGCGlabel = new System.Windows.Forms.Label();
            this.GCGtextBox = new System.Windows.Forms.TextBox();
            this.RetraitGCDlabel = new System.Windows.Forms.Label();
            this.GCDtextBox = new System.Windows.Forms.TextBox();
            this.DecoupeGcheckBox = new System.Windows.Forms.CheckBox();
            this.DecoupeGDisttextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.DecoupeGLrgtextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.DecoupeGProftextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.DecoupeDProftextBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.DecoupeDLrgtextBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.DecoupeDDisttextBox = new System.Windows.Forms.TextBox();
            this.DecoupeDcheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // applyOptionsButton
            // 
            this.applyOptionsButton.Location = new System.Drawing.Point(288, 504);
            this.applyOptionsButton.Name = "applyOptionsButton";
            this.applyOptionsButton.Size = new System.Drawing.Size(95, 36);
            this.applyOptionsButton.TabIndex = 0;
            this.applyOptionsButton.Text = "Appliquer";
            this.applyOptionsButton.UseVisualStyleBackColor = true;
            this.applyOptionsButton.Click += new System.EventHandler(this.applyOptionsButton_Click);
            // 
            // quiteOptionsButton
            // 
            this.quiteOptionsButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.quiteOptionsButton.Location = new System.Drawing.Point(402, 504);
            this.quiteOptionsButton.Name = "quiteOptionsButton";
            this.quiteOptionsButton.Size = new System.Drawing.Size(95, 36);
            this.quiteOptionsButton.TabIndex = 1;
            this.quiteOptionsButton.Text = "Quitter";
            this.quiteOptionsButton.UseVisualStyleBackColor = true;
            this.quiteOptionsButton.Click += new System.EventHandler(this.quiteOptionsButton_Click);
            // 
            // braconCheckBox
            // 
            this.braconCheckBox.AutoSize = true;
            this.braconCheckBox.Location = new System.Drawing.Point(84, 519);
            this.braconCheckBox.Name = "braconCheckBox";
            this.braconCheckBox.Size = new System.Drawing.Size(75, 21);
            this.braconCheckBox.TabIndex = 3;
            this.braconCheckBox.Text = "Bracon";
            this.braconCheckBox.UseVisualStyleBackColor = true;
            // 
            // uBasCheckBox
            // 
            this.uBasCheckBox.AutoSize = true;
            this.uBasCheckBox.Location = new System.Drawing.Point(84, 483);
            this.uBasCheckBox.Name = "uBasCheckBox";
            this.uBasCheckBox.Size = new System.Drawing.Size(68, 21);
            this.uBasCheckBox.TabIndex = 5;
            this.uBasCheckBox.Text = "U Bas";
            this.uBasCheckBox.UseVisualStyleBackColor = true;
            this.uBasCheckBox.CheckedChanged += new System.EventHandler(this.glissiereCheckBox_CheckedChanged);
            // 
            // RetraitDScrollbar
            // 
            this.RetraitDScrollbar.LargeChange = 1;
            this.RetraitDScrollbar.Location = new System.Drawing.Point(301, 75);
            this.RetraitDScrollbar.Name = "RetraitDScrollbar";
            this.RetraitDScrollbar.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RetraitDScrollbar.Size = new System.Drawing.Size(200, 21);
            this.RetraitDScrollbar.TabIndex = 6;
            this.RetraitDScrollbar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.RetraitDScrollbar_Scroll);
            // 
            // RetraitGScrollbar
            // 
            this.RetraitGScrollbar.LargeChange = 1;
            this.RetraitGScrollbar.Location = new System.Drawing.Point(27, 75);
            this.RetraitGScrollbar.Name = "RetraitGScrollbar";
            this.RetraitGScrollbar.Size = new System.Drawing.Size(200, 21);
            this.RetraitGScrollbar.TabIndex = 7;
            this.RetraitGScrollbar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar2_Scroll);
            // 
            // RetraitGTextbox
            // 
            this.RetraitGTextbox.Location = new System.Drawing.Point(97, 50);
            this.RetraitGTextbox.Name = "RetraitGTextbox";
            this.RetraitGTextbox.Size = new System.Drawing.Size(55, 22);
            this.RetraitGTextbox.TabIndex = 8;
            this.RetraitGTextbox.Text = "100";
            this.RetraitGTextbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.RetraitGTextbox.TextChanged += new System.EventHandler(this.RetraitGTextbox_TextChanged);
            // 
            // RetraitDTextbox
            // 
            this.RetraitDTextbox.Location = new System.Drawing.Point(376, 50);
            this.RetraitDTextbox.Name = "RetraitDTextbox";
            this.RetraitDTextbox.Size = new System.Drawing.Size(55, 22);
            this.RetraitDTextbox.TabIndex = 9;
            this.RetraitDTextbox.Text = "100";
            this.RetraitDTextbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(168, 17);
            this.label1.TabIndex = 10;
            this.label1.Text = "Retrait du auvent gauche";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(328, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(149, 17);
            this.label2.TabIndex = 11;
            this.label2.Text = "Retrait du auvent droit";
            // 
            // GCGScrollBar
            // 
            this.GCGScrollBar.Location = new System.Drawing.Point(68, 153);
            this.GCGScrollBar.Maximum = 42;
            this.GCGScrollBar.Name = "GCGScrollBar";
            this.GCGScrollBar.Size = new System.Drawing.Size(21, 80);
            this.GCGScrollBar.TabIndex = 12;
            // 
            // GCDScrollBar
            // 
            this.GCDScrollBar.Location = new System.Drawing.Point(439, 153);
            this.GCDScrollBar.Maximum = 42;
            this.GCDScrollBar.Name = "GCDScrollBar";
            this.GCDScrollBar.Size = new System.Drawing.Size(21, 80);
            this.GCDScrollBar.TabIndex = 13;
            // 
            // RetraitGCGlabel
            // 
            this.RetraitGCGlabel.AutoSize = true;
            this.RetraitGCGlabel.Location = new System.Drawing.Point(112, 153);
            this.RetraitGCGlabel.Name = "RetraitGCGlabel";
            this.RetraitGCGlabel.Size = new System.Drawing.Size(79, 34);
            this.RetraitGCGlabel.TabIndex = 15;
            this.RetraitGCGlabel.Text = "Retrait du \r\nGC gauche";
            this.RetraitGCGlabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GCGtextBox
            // 
            this.GCGtextBox.Location = new System.Drawing.Point(124, 200);
            this.GCGtextBox.Name = "GCGtextBox";
            this.GCGtextBox.Size = new System.Drawing.Size(55, 22);
            this.GCGtextBox.TabIndex = 14;
            this.GCGtextBox.Text = "10";
            this.GCGtextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // RetraitGCDlabel
            // 
            this.RetraitGCDlabel.AutoSize = true;
            this.RetraitGCDlabel.Location = new System.Drawing.Point(344, 153);
            this.RetraitGCDlabel.Name = "RetraitGCDlabel";
            this.RetraitGCDlabel.Size = new System.Drawing.Size(74, 34);
            this.RetraitGCDlabel.TabIndex = 17;
            this.RetraitGCDlabel.Text = "Retrait du \r\nGC droit";
            this.RetraitGCDlabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GCDtextBox
            // 
            this.GCDtextBox.Location = new System.Drawing.Point(355, 200);
            this.GCDtextBox.Name = "GCDtextBox";
            this.GCDtextBox.Size = new System.Drawing.Size(55, 22);
            this.GCDtextBox.TabIndex = 16;
            this.GCDtextBox.Text = "10";
            this.GCDtextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // DecoupeGcheckBox
            // 
            this.DecoupeGcheckBox.AutoSize = true;
            this.DecoupeGcheckBox.Location = new System.Drawing.Point(50, 279);
            this.DecoupeGcheckBox.Name = "DecoupeGcheckBox";
            this.DecoupeGcheckBox.Size = new System.Drawing.Size(147, 38);
            this.DecoupeGcheckBox.TabIndex = 18;
            this.DecoupeGcheckBox.Text = "Découpe dans \r\nl\'extension gauche";
            this.DecoupeGcheckBox.UseVisualStyleBackColor = true;
            // 
            // DecoupeGDisttextBox
            // 
            this.DecoupeGDisttextBox.Location = new System.Drawing.Point(152, 328);
            this.DecoupeGDisttextBox.Name = "DecoupeGDisttextBox";
            this.DecoupeGDisttextBox.Size = new System.Drawing.Size(55, 22);
            this.DecoupeGDisttextBox.TabIndex = 19;
            this.DecoupeGDisttextBox.Text = "10";
            this.DecoupeGDisttextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 328);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(124, 17);
            this.label5.TabIndex = 20;
            this.label5.Text = "Distance au bord :";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(24, 359);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 17);
            this.label6.TabIndex = 22;
            this.label6.Text = "Largeur :";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DecoupeGLrgtextBox
            // 
            this.DecoupeGLrgtextBox.Location = new System.Drawing.Point(152, 359);
            this.DecoupeGLrgtextBox.Name = "DecoupeGLrgtextBox";
            this.DecoupeGLrgtextBox.Size = new System.Drawing.Size(55, 22);
            this.DecoupeGLrgtextBox.TabIndex = 21;
            this.DecoupeGLrgtextBox.Text = "10";
            this.DecoupeGLrgtextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(24, 389);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(87, 17);
            this.label7.TabIndex = 24;
            this.label7.Text = "Profondeur :";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DecoupeGProftextBox
            // 
            this.DecoupeGProftextBox.Location = new System.Drawing.Point(152, 389);
            this.DecoupeGProftextBox.Name = "DecoupeGProftextBox";
            this.DecoupeGProftextBox.Size = new System.Drawing.Size(55, 22);
            this.DecoupeGProftextBox.TabIndex = 23;
            this.DecoupeGProftextBox.Text = "10";
            this.DecoupeGProftextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(305, 389);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(87, 17);
            this.label8.TabIndex = 31;
            this.label8.Text = "Profondeur :";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DecoupeDProftextBox
            // 
            this.DecoupeDProftextBox.Location = new System.Drawing.Point(433, 389);
            this.DecoupeDProftextBox.Name = "DecoupeDProftextBox";
            this.DecoupeDProftextBox.Size = new System.Drawing.Size(55, 22);
            this.DecoupeDProftextBox.TabIndex = 30;
            this.DecoupeDProftextBox.Text = "10";
            this.DecoupeDProftextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(305, 359);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(66, 17);
            this.label9.TabIndex = 29;
            this.label9.Text = "Largeur :";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DecoupeDLrgtextBox
            // 
            this.DecoupeDLrgtextBox.Location = new System.Drawing.Point(433, 359);
            this.DecoupeDLrgtextBox.Name = "DecoupeDLrgtextBox";
            this.DecoupeDLrgtextBox.Size = new System.Drawing.Size(55, 22);
            this.DecoupeDLrgtextBox.TabIndex = 28;
            this.DecoupeDLrgtextBox.Text = "10";
            this.DecoupeDLrgtextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(305, 328);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(124, 17);
            this.label10.TabIndex = 27;
            this.label10.Text = "Distance au bord :";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DecoupeDDisttextBox
            // 
            this.DecoupeDDisttextBox.Location = new System.Drawing.Point(433, 328);
            this.DecoupeDDisttextBox.Name = "DecoupeDDisttextBox";
            this.DecoupeDDisttextBox.Size = new System.Drawing.Size(55, 22);
            this.DecoupeDDisttextBox.TabIndex = 26;
            this.DecoupeDDisttextBox.Text = "10";
            this.DecoupeDDisttextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // DecoupeDcheckBox
            // 
            this.DecoupeDcheckBox.AutoSize = true;
            this.DecoupeDcheckBox.Location = new System.Drawing.Point(331, 279);
            this.DecoupeDcheckBox.Name = "DecoupeDcheckBox";
            this.DecoupeDcheckBox.Size = new System.Drawing.Size(136, 38);
            this.DecoupeDcheckBox.TabIndex = 25;
            this.DecoupeDcheckBox.Text = "Découpe dans \r\nl\'extension droite";
            this.DecoupeDcheckBox.UseVisualStyleBackColor = true;
            // 
            // optionsForm
            // 
            this.AcceptButton = this.applyOptionsButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.quiteOptionsButton;
            this.ClientSize = new System.Drawing.Size(530, 560);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.DecoupeDProftextBox);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.DecoupeDLrgtextBox);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.DecoupeDDisttextBox);
            this.Controls.Add(this.DecoupeDcheckBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.DecoupeGProftextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.DecoupeGLrgtextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.DecoupeGDisttextBox);
            this.Controls.Add(this.DecoupeGcheckBox);
            this.Controls.Add(this.RetraitGCDlabel);
            this.Controls.Add(this.GCDtextBox);
            this.Controls.Add(this.RetraitGCGlabel);
            this.Controls.Add(this.GCGtextBox);
            this.Controls.Add(this.GCDScrollBar);
            this.Controls.Add(this.GCGScrollBar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RetraitDTextbox);
            this.Controls.Add(this.RetraitGTextbox);
            this.Controls.Add(this.RetraitGScrollbar);
            this.Controls.Add(this.RetraitDScrollbar);
            this.Controls.Add(this.uBasCheckBox);
            this.Controls.Add(this.braconCheckBox);
            this.Controls.Add(this.quiteOptionsButton);
            this.Controls.Add(this.applyOptionsButton);
            this.Name = "optionsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options Complémentaires";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button applyOptionsButton;
        public System.Windows.Forms.Button quiteOptionsButton;
        public System.Windows.Forms.CheckBox braconCheckBox;
        public System.Windows.Forms.CheckBox uBasCheckBox;
        public System.Windows.Forms.HScrollBar RetraitDScrollbar;
        public System.Windows.Forms.HScrollBar RetraitGScrollbar;
        public System.Windows.Forms.TextBox RetraitGTextbox;
        public System.Windows.Forms.TextBox RetraitDTextbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label RetraitGCGlabel;
        public System.Windows.Forms.TextBox GCGtextBox;
        private System.Windows.Forms.Label RetraitGCDlabel;
        public System.Windows.Forms.TextBox GCDtextBox;
        public System.Windows.Forms.VScrollBar GCGScrollBar;
        public System.Windows.Forms.VScrollBar GCDScrollBar;
        public System.Windows.Forms.TextBox DecoupeGDisttextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.TextBox DecoupeGLrgtextBox;
        private System.Windows.Forms.Label label7;
        public System.Windows.Forms.TextBox DecoupeGProftextBox;
        private System.Windows.Forms.Label label8;
        public System.Windows.Forms.TextBox DecoupeDProftextBox;
        private System.Windows.Forms.Label label9;
        public System.Windows.Forms.TextBox DecoupeDLrgtextBox;
        private System.Windows.Forms.Label label10;
        public System.Windows.Forms.TextBox DecoupeDDisttextBox;
        public System.Windows.Forms.CheckBox DecoupeGcheckBox;
        public System.Windows.Forms.CheckBox DecoupeDcheckBox;
    }
}