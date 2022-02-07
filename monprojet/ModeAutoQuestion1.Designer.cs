namespace PTLGClassLibrary
{
    partial class ModeAutoQuestion1
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
            this.questionoption1button = new System.Windows.Forms.Button();
            this.questionoption2button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // questionoption1button
            // 
            this.questionoption1button.Location = new System.Drawing.Point(90, 116);
            this.questionoption1button.Name = "questionoption1button";
            this.questionoption1button.Size = new System.Drawing.Size(98, 39);
            this.questionoption1button.TabIndex = 0;
            this.questionoption1button.Text = "option1";
            this.questionoption1button.UseVisualStyleBackColor = true;
            this.questionoption1button.Click += new System.EventHandler(this.questionoption1button_Click);
            // 
            // questionoption2button
            // 
            this.questionoption2button.Location = new System.Drawing.Point(276, 116);
            this.questionoption2button.Name = "questionoption2button";
            this.questionoption2button.Size = new System.Drawing.Size(98, 39);
            this.questionoption2button.TabIndex = 1;
            this.questionoption2button.Text = "option2";
            this.questionoption2button.UseVisualStyleBackColor = true;
            this.questionoption2button.Click += new System.EventHandler(this.questionoption2button_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(157, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(166, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "Choix utilisateur : ";
            // 
            // ModeAutoQuestion1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(491, 183);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.questionoption2button);
            this.Controls.Add(this.questionoption1button);
            this.Name = "ModeAutoQuestion1";
            this.Text = "Action utilisateur";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button questionoption1button;
        public System.Windows.Forms.Button questionoption2button;
        private System.Windows.Forms.Label label1;
    }
}