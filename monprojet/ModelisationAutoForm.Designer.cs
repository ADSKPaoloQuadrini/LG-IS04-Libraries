namespace PTLGClassLibrary
{
    partial class ModelisationAutoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModelisationAutoForm));
            this.bancheComboBox = new System.Windows.Forms.ComboBox();
            this.bancheLabel = new System.Windows.Forms.Label();
            this.applyButton = new System.Windows.Forms.Button();
            this.angleGaucheCheckBox = new System.Windows.Forms.CheckBox();
            this.angleDroitCheckBox = new System.Windows.Forms.CheckBox();
            this.option1button = new System.Windows.Forms.Button();
            this.option2button = new System.Windows.Forms.Button();
            this.eventCheckBox = new System.Windows.Forms.CheckBox();
            this.option1CheckBox = new System.Windows.Forms.CheckBox();
            this.option2CheckBox = new System.Windows.Forms.CheckBox();
            this.option3button = new System.Windows.Forms.Button();
            this.option3CheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // bancheComboBox
            // 
            this.bancheComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bancheComboBox.FormattingEnabled = true;
            this.bancheComboBox.Items.AddRange(new object[] {
            "Pas de banche",
            "H Banche_280",
            "H Banche_330",
            "H Banche_380",
            "H Banche_430",
            "H Banche_480"});
            this.bancheComboBox.Location = new System.Drawing.Point(224, 38);
            this.bancheComboBox.Name = "bancheComboBox";
            this.bancheComboBox.Size = new System.Drawing.Size(121, 24);
            this.bancheComboBox.TabIndex = 0;
            this.bancheComboBox.SelectedIndexChanged += new System.EventHandler(this.bancheComboBox_SelectedIndexChanged);
            // 
            // bancheLabel
            // 
            this.bancheLabel.AutoSize = true;
            this.bancheLabel.Location = new System.Drawing.Point(64, 44);
            this.bancheLabel.Name = "bancheLabel";
            this.bancheLabel.Size = new System.Drawing.Size(138, 17);
            this.bancheLabel.TabIndex = 1;
            this.bancheLabel.Text = "Hauteur de banche :";
            // 
            // applyButton
            // 
            this.applyButton.Location = new System.Drawing.Point(429, 288);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(180, 29);
            this.applyButton.TabIndex = 2;
            this.applyButton.Text = "Lancer le calepinage";
            this.applyButton.UseVisualStyleBackColor = true;
            this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
            // 
            // angleGaucheCheckBox
            // 
            this.angleGaucheCheckBox.AutoSize = true;
            this.angleGaucheCheckBox.Location = new System.Drawing.Point(67, 166);
            this.angleGaucheCheckBox.Name = "angleGaucheCheckBox";
            this.angleGaucheCheckBox.Size = new System.Drawing.Size(100, 21);
            this.angleGaucheCheckBox.TabIndex = 5;
            this.angleGaucheCheckBox.Text = "Angle Droit";
            this.angleGaucheCheckBox.UseVisualStyleBackColor = true;
            this.angleGaucheCheckBox.CheckedChanged += new System.EventHandler(this.angleGaucheCheckBox_CheckedChanged);
            // 
            // angleDroitCheckBox
            // 
            this.angleDroitCheckBox.AutoSize = true;
            this.angleDroitCheckBox.Location = new System.Drawing.Point(470, 166);
            this.angleDroitCheckBox.Name = "angleDroitCheckBox";
            this.angleDroitCheckBox.Size = new System.Drawing.Size(100, 21);
            this.angleDroitCheckBox.TabIndex = 6;
            this.angleDroitCheckBox.Text = "Angle Droit";
            this.angleDroitCheckBox.UseVisualStyleBackColor = true;
            this.angleDroitCheckBox.CheckedChanged += new System.EventHandler(this.angleDroitCheckBox_CheckedChanged);
            // 
            // option1button
            // 
            this.option1button.Location = new System.Drawing.Point(43, 229);
            this.option1button.Name = "option1button";
            this.option1button.Size = new System.Drawing.Size(129, 51);
            this.option1button.TabIndex = 7;
            this.option1button.Text = "option1";
            this.option1button.UseVisualStyleBackColor = true;
            this.option1button.Click += new System.EventHandler(this.option1button_Click);
            // 
            // option2button
            // 
            this.option2button.Location = new System.Drawing.Point(196, 229);
            this.option2button.Name = "option2button";
            this.option2button.Size = new System.Drawing.Size(129, 51);
            this.option2button.TabIndex = 8;
            this.option2button.Text = "option2";
            this.option2button.UseVisualStyleBackColor = true;
            this.option2button.Click += new System.EventHandler(this.option2button_Click);
            // 
            // eventCheckBox
            // 
            this.eventCheckBox.AutoSize = true;
            this.eventCheckBox.Location = new System.Drawing.Point(471, 229);
            this.eventCheckBox.Name = "eventCheckBox";
            this.eventCheckBox.Size = new System.Drawing.Size(98, 21);
            this.eventCheckBox.TabIndex = 9;
            this.eventCheckBox.Text = "checkBox1";
            this.eventCheckBox.UseVisualStyleBackColor = true;
            this.eventCheckBox.Visible = false;
            this.eventCheckBox.CheckedChanged += new System.EventHandler(this.eventCheckBox_CheckedChanged);
            // 
            // option1CheckBox
            // 
            this.option1CheckBox.AutoSize = true;
            this.option1CheckBox.Location = new System.Drawing.Point(43, 288);
            this.option1CheckBox.Name = "option1CheckBox";
            this.option1CheckBox.Size = new System.Drawing.Size(77, 21);
            this.option1CheckBox.TabIndex = 10;
            this.option1CheckBox.Text = "option1";
            this.option1CheckBox.UseVisualStyleBackColor = true;
            this.option1CheckBox.Visible = false;
            this.option1CheckBox.CheckedChanged += new System.EventHandler(this.option1CheckBox_CheckedChanged);
            // 
            // option2CheckBox
            // 
            this.option2CheckBox.AutoSize = true;
            this.option2CheckBox.Location = new System.Drawing.Point(196, 286);
            this.option2CheckBox.Name = "option2CheckBox";
            this.option2CheckBox.Size = new System.Drawing.Size(77, 21);
            this.option2CheckBox.TabIndex = 11;
            this.option2CheckBox.Text = "option2";
            this.option2CheckBox.UseVisualStyleBackColor = true;
            this.option2CheckBox.Visible = false;
            this.option2CheckBox.CheckedChanged += new System.EventHandler(this.option2CheckBox_CheckedChanged);
            // 
            // option3button
            // 
            this.option3button.Location = new System.Drawing.Point(352, 229);
            this.option3button.Name = "option3button";
            this.option3button.Size = new System.Drawing.Size(129, 51);
            this.option3button.TabIndex = 12;
            this.option3button.Text = "option3";
            this.option3button.UseVisualStyleBackColor = true;
            this.option3button.Click += new System.EventHandler(this.option3button_Click);
            // 
            // option3CheckBox
            // 
            this.option3CheckBox.AutoSize = true;
            this.option3CheckBox.Location = new System.Drawing.Point(352, 286);
            this.option3CheckBox.Name = "option3CheckBox";
            this.option3CheckBox.Size = new System.Drawing.Size(77, 21);
            this.option3CheckBox.TabIndex = 13;
            this.option3CheckBox.Text = "option3";
            this.option3CheckBox.UseVisualStyleBackColor = true;
            this.option3CheckBox.Visible = false;
            this.option3CheckBox.CheckedChanged += new System.EventHandler(this.option3CheckBox_CheckedChanged_1);
            // 
            // ModelisationAutoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(633, 329);
            this.Controls.Add(this.option3CheckBox);
            this.Controls.Add(this.option3button);
            this.Controls.Add(this.option2CheckBox);
            this.Controls.Add(this.option1CheckBox);
            this.Controls.Add(this.eventCheckBox);
            this.Controls.Add(this.option2button);
            this.Controls.Add(this.option1button);
            this.Controls.Add(this.angleDroitCheckBox);
            this.Controls.Add(this.angleGaucheCheckBox);
            this.Controls.Add(this.applyButton);
            this.Controls.Add(this.bancheLabel);
            this.Controls.Add(this.bancheComboBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ModelisationAutoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Proposition de calepinage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label bancheLabel;
        public System.Windows.Forms.ComboBox bancheComboBox;
        public System.Windows.Forms.Button applyButton;
        public System.Windows.Forms.CheckBox angleGaucheCheckBox;
        public System.Windows.Forms.CheckBox angleDroitCheckBox;
        public System.Windows.Forms.Button option1button;
        public System.Windows.Forms.Button option2button;
        public System.Windows.Forms.CheckBox eventCheckBox;
        public System.Windows.Forms.CheckBox option1CheckBox;
        public System.Windows.Forms.CheckBox option2CheckBox;
        public System.Windows.Forms.Button option3button;
        public System.Windows.Forms.CheckBox option3CheckBox;
    }
}