#region Autodesk PQ - IS04
using System;

namespace PTLGClassLibrary
{
    public partial class ConfigurateurManForm : System.Windows.Forms.Form
    {
        public ConfigurateurManForm()
        {
            InitializeComponent();
        }

        //Numéro Passerelle
        public string numeroPasserelleString
        {
            get
            {
                return numeroTextBox.Text;
            }
            set
            {
                numeroTextBox.Text = value;
            }
        }
        private void numeroTextBox_TextChanged(object sender, EventArgs e)
        {
            if (eventcheckBoxext.Checked) eventcheckBoxext.Checked = false;
            if (!eventcheckBoxext.Checked) eventcheckBoxext.Checked = true;
        }
        //Banche + Stabilisation
        public void banchecomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (bancheComboBox.SelectedItem.ToString() == "Pas de banche")
            {
                stabilisationComboBox.Items.Insert(0, "Pas de banche");
                stabilisationComboBox.SelectedIndex = stabilisationComboBox.FindStringExact("Pas de banche");
                stabilisationComboBox.Visible = false;
                stabilisationLabel.Visible = false;
            }
            else
            {
                stabilisationComboBox.Visible = true;
                stabilisationComboBox.Items.Remove("Pas de banche");
                stabilisationLabel.Visible = true;
            }

            if (eventcheckBoxext.Checked) eventcheckBoxext.Checked = false;
            if (!eventcheckBoxext.Checked) eventcheckBoxext.Checked = true;
        }
        private void stabilisationComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (eventcheckBoxext.Checked) eventcheckBoxext.Checked = false;
            if (!eventcheckBoxext.Checked) eventcheckBoxext.Checked = true;
        }

        //Buttons
        private void applyButton_Click(object sender, EventArgs e)
        {

        }
        private void optionsButton_Click(object sender, EventArgs e)
        {

        }
        private void quiteButton_Click(object sender, EventArgs e)
        {

        }
        private void footButton_Click(object sender, EventArgs e)
        {

        }

        //Extensions
        private void extensionGaucheComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (eventcheckBoxext.Checked) eventcheckBoxext.Checked = false;
            if (!eventcheckBoxext.Checked) eventcheckBoxext.Checked = true;
        }
        private void extensionGaucheIntComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (eventcheckBoxext.Checked) eventcheckBoxext.Checked = false;
            if (!eventcheckBoxext.Checked) eventcheckBoxext.Checked = true;
        }
        private void extensionDroiteComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (eventcheckBoxext.Checked) eventcheckBoxext.Checked = false;
            if (!eventcheckBoxext.Checked) eventcheckBoxext.Checked = true;
        }
        private void extensionDroiteIntComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (eventcheckBoxext.Checked) eventcheckBoxext.Checked = false;
            if (!eventcheckBoxext.Checked) eventcheckBoxext.Checked = true;
        }

        //Attaches
        private void attacheGaucheTrackbar_Scroll(object sender, EventArgs e)
        {
            attacheGaucheTrackbarTextBox.Text = ((-attacheGaucheTrackbar.Value).ToString());

            if (eventcheckBoxext.Checked) eventcheckBoxext.Checked = false;
            if (!eventcheckBoxext.Checked) eventcheckBoxext.Checked = true;
        }
        private void attacheGaucheTrackbarTextBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(attacheGaucheTrackbarTextBox.Text) || attacheGaucheTrackbarTextBox.Text == "-")
            {
                attacheGaucheTrackbar.Value = 0;
            }
            else if (Convert.ToInt32(attacheGaucheTrackbarTextBox.Text) > attacheGaucheTrackbar.Maximum)
            {
                attacheGaucheTrackbarTextBox.Text = attacheGaucheTrackbar.Maximum.ToString();
            }
            else if (Convert.ToInt32(attacheGaucheTrackbarTextBox.Text) < attacheGaucheTrackbar.Minimum)
            {
                attacheGaucheTrackbarTextBox.Text = attacheGaucheTrackbar.Minimum.ToString();
            }
            else attacheGaucheTrackbar.Value = -Convert.ToInt32(attacheGaucheTrackbarTextBox.Text);

            if (eventcheckBoxext.Checked) eventcheckBoxext.Checked = false;
            if (!eventcheckBoxext.Checked) eventcheckBoxext.Checked = true;
        }
        private void attacheDroiteTrackbar_Scroll(object sender, EventArgs e)
        {
            attacheDroiteTrackbarTextBox.Text = ((attacheDroiteTrackbar.Value).ToString());

            if (eventcheckBoxext.Checked) eventcheckBoxext.Checked = false;
            if (!eventcheckBoxext.Checked) eventcheckBoxext.Checked = true;
        }
        private void attacheDroiteTrackbarTextBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(attacheDroiteTrackbarTextBox.Text) || attacheDroiteTrackbarTextBox.Text == "-")
            {
                attacheDroiteTrackbar.Value = 0;
            }
            else if (Convert.ToInt32(attacheDroiteTrackbarTextBox.Text) > attacheDroiteTrackbar.Maximum)
            {
                attacheDroiteTrackbarTextBox.Text = attacheDroiteTrackbar.Maximum.ToString();
            }
            else if (Convert.ToInt32(attacheDroiteTrackbarTextBox.Text) < attacheDroiteTrackbar.Minimum)
            {
                attacheDroiteTrackbarTextBox.Text = attacheDroiteTrackbar.Minimum.ToString();
            }
            else attacheDroiteTrackbar.Value = Convert.ToInt32(attacheDroiteTrackbarTextBox.Text);

            if (eventcheckBoxext.Checked) eventcheckBoxext.Checked = false;
            if (!eventcheckBoxext.Checked) eventcheckBoxext.Checked = true;
        }

        /// Troisieme attache
        private void troisiemeAttacheCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (eventcheckBoxext.Checked) eventcheckBoxext.Checked = false;
            if (!eventcheckBoxext.Checked) eventcheckBoxext.Checked = true;
        }
        private void attacheCentreComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (eventcheckBoxext.Checked) eventcheckBoxext.Checked = false;
            if (!eventcheckBoxext.Checked) eventcheckBoxext.Checked = true;
        }
        private void attacheCentreTrackbar_Scroll(object sender, EventArgs e)
        {
            if (troisiemeAttacheCheckBox.Checked && !quatriemeAttacheCheckBox.Checked)
            {
                attacheCentreGTrackbarTextBox.Text = (GeneralClass.lgFixe / 2 + attacheCentreTrackbar.Value).ToString();
                attacheCentreDTrackbarTextBox.Text = (GeneralClass.lgFixe / 2 - attacheCentreTrackbar.Value).ToString();
            }
            else if (troisiemeAttacheCheckBox.Checked && quatriemeAttacheCheckBox.Checked)
            {
                attacheCentreGTrackbarTextBox.Text = (GeneralClass.lgFixe / 4 + attacheCentreTrackbar.Value).ToString();
                //attacheCentreDTrackbarTextBox.Text = (GeneralClass.lgFixe / 4 - attacheCentreDroitTrackbar.Value).ToString();
            }

            if (eventcheckBoxext.Checked) eventcheckBoxext.Checked = false;
            if (!eventcheckBoxext.Checked) eventcheckBoxext.Checked = true;
        }

        private void cCenterMaxiLabel_Click(object sender, EventArgs e)
        {

        }

        /// Quatrieme attache
        private void quatriemeAttacheCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (eventcheckBoxext.Checked) eventcheckBoxext.Checked = false;
            if (!eventcheckBoxext.Checked) eventcheckBoxext.Checked = true;
        }

        private void attacheCentreDroitComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (eventcheckBoxext.Checked) eventcheckBoxext.Checked = false;
            if (!eventcheckBoxext.Checked) eventcheckBoxext.Checked = true;
        }

        private void attacheCentreDroitTrackbar_Scroll(object sender, EventArgs e)
        {
            attacheCentreDTrackbarTextBox.Text = (GeneralClass.lgFixe / 4 - attacheCentreDroitTrackbar.Value).ToString();

            if (eventcheckBoxext.Checked) eventcheckBoxext.Checked = false;
            if (!eventcheckBoxext.Checked) eventcheckBoxext.Checked = true;
        }

        private void cCenterDroitMaxiLabel_Click(object sender, EventArgs e)
        {

        }

        //Angle droit
        private void angleGaucheCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (angleGaucheCheckBox.Checked == true)
            {
                carreBlancGauche.Visible = false;
                angleBiaisGaucheCheckBox.Checked = false;
                angleGaucheInterieurCheckBox.Checked = false;
            }
            else
            {
                carreBlancGauche.Visible = true;
            }

            if (eventcheckBoxext.Checked) eventcheckBoxext.Checked = false;
            if (!eventcheckBoxext.Checked) eventcheckBoxext.Checked = true;
        }
        private void angleDroitCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (angleDroitCheckBox.Checked == true)
            {
                carreBlancDroit.Visible = false;
                angleBiaisDroitCheckBox.Checked = false;
                angleDroitInterieurCheckBox.Checked = false;
            }
            else
            {
                carreBlancDroit.Visible = true;
            }

            if (eventcheckBoxext.Checked) eventcheckBoxext.Checked = false;
            if (!eventcheckBoxext.Checked) eventcheckBoxext.Checked = true;
        }

        //Angle biais
        private void angleBiaisGaucheCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (angleBiaisGaucheCheckBox.Checked == true)
            {
                extensionGaucheIntComboBox.Visible = true;
                angleGaucheCheckBox.Checked = false;
            }
            else extensionGaucheIntComboBox.Visible = false;

            if (eventcheckBoxext.Checked) eventcheckBoxext.Checked = false;
            if (!eventcheckBoxext.Checked) eventcheckBoxext.Checked = true;
        }
        private void angleBiaisDroitCheckBox_CheckedChanged_1(object sender, EventArgs e)
        {
            if (angleBiaisDroitCheckBox.Checked == true)
            {
                extensionDroiteIntComboBox.Visible = true;
                angleDroitCheckBox.Checked = false;
            }
            else extensionDroiteIntComboBox.Visible = false;

            if (eventcheckBoxext.Checked) eventcheckBoxext.Checked = false;
            if (!eventcheckBoxext.Checked) eventcheckBoxext.Checked = true;
        }

        //Angle intérieur
        private void angleGaucheInterieurCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (angleGaucheInterieurCheckBox.Checked)
            {
                angleGaucheCheckBox.Checked = false;
                sabotGauchePictureBox.Visible = true;
                sabotGaucheTextBox1.Visible = true;
                sabotGaucheTrackBar.Visible = true;
            }
            else
            {
                sabotGauchePictureBox.Visible = false;
                sabotGaucheTextBox1.Visible = false;
                sabotGaucheTrackBar.Visible = false;

                angleGaucheInterieurCheckBox2.Checked = false;
            }

            if (eventcheckBoxext.Checked) eventcheckBoxext.Checked = false;
            if (!eventcheckBoxext.Checked) eventcheckBoxext.Checked = true;
        }
        private void angleGaucheInterieurCheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (angleGaucheInterieurCheckBox2.Checked)
            {
                angleGaucheCheckBox.Checked = false;
                sabotGauchePictureBox2.Visible = true;
                sabotGaucheTextBox2.Visible = true;
                sabotGaucheTrackBar2.Visible = true;

                angleGaucheInterieurCheckBox.Checked = true;
            }
            else
            {
                sabotGauchePictureBox2.Visible = false;
                sabotGaucheTextBox2.Visible = false;
                sabotGaucheTrackBar2.Visible = false;
            }

            if (eventcheckBoxext.Checked) eventcheckBoxext.Checked = false;
            if (!eventcheckBoxext.Checked) eventcheckBoxext.Checked = true;
        }

        private void angleDroitInterieurCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (angleDroitInterieurCheckBox.Checked)
            {
                angleDroitCheckBox.Checked = false;
                sabotDroitPictureBox.Visible = true;
                sabotDroitTextBox1.Visible = true;
                sabotDroitTrackBar.Visible = true;
            }
            else
            {
                sabotDroitPictureBox.Visible = false;
                sabotDroitTextBox1.Visible = false;
                sabotDroitTrackBar.Visible = false;

                angleDroitInterieurCheckBox2.Checked = false;
            }

            if (eventcheckBoxext.Checked) eventcheckBoxext.Checked = false;
            if (!eventcheckBoxext.Checked) eventcheckBoxext.Checked = true;
        }
        private void angleDroitInterieurCheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (angleDroitInterieurCheckBox2.Checked)
            {
                angleDroitCheckBox.Checked = false;
                sabotDroitPictureBox2.Visible = true;
                sabotDroitTextBox2.Visible = true;
                sabotDroitTrackBar2.Visible = true;

                angleDroitInterieurCheckBox.Checked = true;
            }
            else
            {
                sabotDroitPictureBox2.Visible = false;
                sabotDroitTextBox2.Visible = false;
                sabotDroitTrackBar2.Visible = false;
            }

            if (eventcheckBoxext.Checked) eventcheckBoxext.Checked = false;
            if (!eventcheckBoxext.Checked) eventcheckBoxext.Checked = true;
        }

        #region Sabots
        /// TrackBars
        private void sabotGaucheTrackBar_Scroll(object sender, EventArgs e)
        {
            sabotGaucheTextBox1.Text = sabotGaucheTrackBar.Value.ToString();

            if (eventcheckBoxext.Checked) eventcheckBoxext.Checked = false;
            if (!eventcheckBoxext.Checked) eventcheckBoxext.Checked = true;
        }
        private void sabotGaucheTrackBar2_Scroll(object sender, EventArgs e)
        {
            sabotGaucheTextBox2.Text = sabotGaucheTrackBar2.Value.ToString();

            if (eventcheckBoxext.Checked) eventcheckBoxext.Checked = false;
            if (!eventcheckBoxext.Checked) eventcheckBoxext.Checked = true;
        }
        private void sabotDroitTrackBar_Scroll(object sender, EventArgs e)
        {
            sabotDroitTextBox1.Text = sabotDroitTrackBar.Value.ToString();

            if (eventcheckBoxext.Checked) eventcheckBoxext.Checked = false;
            if (!eventcheckBoxext.Checked) eventcheckBoxext.Checked = true;
        }
        private void sabotDroitTrackBar2_Scroll(object sender, EventArgs e)
        {
            sabotDroitTextBox2.Text = sabotDroitTrackBar2.Value.ToString();

            if (eventcheckBoxext.Checked) eventcheckBoxext.Checked = false;
            if (!eventcheckBoxext.Checked) eventcheckBoxext.Checked = true;
        }

        /// TextBoxes
        private void sabotGaucheTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(sabotGaucheTextBox1.Text) || sabotGaucheTextBox1.Text == "-")
            {
                sabotGaucheTrackBar.Value = sabotGaucheTrackBar.Minimum;
            }
            else if (sabotGaucheTextBox1.Text.Length > 2)
            {
                if (Convert.ToInt32(sabotGaucheTextBox1.Text) > sabotGaucheTrackBar.Maximum)
                {
                    sabotGaucheTextBox1.Text = sabotGaucheTrackBar.Maximum.ToString();
                }
                else sabotGaucheTrackBar.Value = Convert.ToInt32(sabotGaucheTextBox1.Text);
            }

            if (eventcheckBoxext.Checked) eventcheckBoxext.Checked = false;
            if (!eventcheckBoxext.Checked) eventcheckBoxext.Checked = true;
        }
        private void sabotGaucheTextBox1_Leave(object sender, EventArgs e)
        {
            if (Convert.ToInt32(sabotGaucheTextBox1.Text) < sabotGaucheTrackBar.Minimum)
            {
                sabotGaucheTextBox1.Text = sabotGaucheTrackBar.Minimum.ToString();
            }
            sabotGaucheTrackBar.Value = Convert.ToInt32(sabotGaucheTextBox1.Text);
        }

        private void sabotGaucheTextBox2_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(sabotGaucheTextBox2.Text) || sabotGaucheTextBox2.Text == "-")
            {
                sabotGaucheTrackBar2.Value = sabotGaucheTrackBar2.Minimum;
            }
            else if (sabotGaucheTextBox2.Text.Length > 2)
            {
                if (Convert.ToInt32(sabotGaucheTextBox2.Text) > sabotGaucheTrackBar2.Maximum)
                {
                    sabotGaucheTextBox2.Text = sabotGaucheTrackBar2.Maximum.ToString();
                }
                else sabotGaucheTrackBar2.Value = Convert.ToInt32(sabotGaucheTextBox2.Text);
            }
        }
        private void sabotGaucheTextBox2_Leave(object sender, EventArgs e)
        {
            if (Convert.ToInt32(sabotGaucheTextBox2.Text) < sabotGaucheTrackBar2.Minimum)
            {
                sabotGaucheTextBox2.Text = sabotGaucheTrackBar2.Minimum.ToString();
            }
            sabotGaucheTrackBar2.Value = Convert.ToInt32(sabotGaucheTextBox2.Text);
        }
        
        private void sabotDroitTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(sabotDroitTextBox1.Text) || sabotDroitTextBox1.Text == "-")
            {
                sabotDroitTrackBar.Value = sabotDroitTrackBar.Minimum;
            }
            else if (sabotDroitTextBox1.Text.Length > 2)
            {
                if (Convert.ToInt32(sabotDroitTextBox1.Text) > sabotDroitTrackBar.Maximum)
                {
                    sabotDroitTextBox1.Text = sabotDroitTrackBar.Maximum.ToString();
                }
                else sabotDroitTrackBar.Value = Convert.ToInt32(sabotDroitTextBox1.Text);
            }

            if (eventcheckBoxext.Checked) eventcheckBoxext.Checked = false;
            if (!eventcheckBoxext.Checked) eventcheckBoxext.Checked = true;
        }
        private void sabotDroitTextBox1_Leave(object sender, EventArgs e)
        {
            if (Convert.ToInt32(sabotDroitTextBox1.Text) < sabotDroitTrackBar.Minimum)
            {
                sabotDroitTextBox1.Text = sabotDroitTrackBar.Minimum.ToString();
            }
            sabotDroitTrackBar.Value = Convert.ToInt32(sabotDroitTextBox1.Text);
        }

        private void sabotDroitTextBox2_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(sabotDroitTextBox2.Text) || sabotDroitTextBox2.Text == "-")
            {
                sabotDroitTrackBar2.Value = sabotDroitTrackBar2.Minimum;
            }
            else if (sabotDroitTextBox2.Text.Length > 2)
            {
                if (Convert.ToInt32(sabotDroitTextBox2.Text) > sabotDroitTrackBar2.Maximum)
                {
                    sabotDroitTextBox2.Text = sabotDroitTrackBar2.Maximum.ToString();
                }
                else sabotDroitTrackBar2.Value = Convert.ToInt32(sabotDroitTextBox2.Text);
            }
        }
        private void sabotDroitTextBox2_Leave(object sender, EventArgs e)
        {
            if (Convert.ToInt32(sabotDroitTextBox2.Text) < sabotDroitTrackBar2.Minimum)
            {
                sabotDroitTextBox2.Text = sabotDroitTrackBar2.Minimum.ToString();
            }
            sabotDroitTrackBar2.Value = Convert.ToInt32(sabotDroitTextBox2.Text);
        }
        #endregion Sabot

        private void ConfigurateurForm_Load(object sender, EventArgs e)
        {

        }

        private void extArriereCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (eventcheckBoxext.Checked) eventcheckBoxext.Checked = false;
            if (!eventcheckBoxext.Checked) eventcheckBoxext.Checked = true;
        }


        private void pafGaucheMaxiLabel_Click(object sender, EventArgs e)
        {

        }

        private void attacheGaucheComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (eventcheckBoxext.Checked) eventcheckBoxext.Checked = false;
            if (!eventcheckBoxext.Checked) eventcheckBoxext.Checked = true;
        }

        private void attacheDroiteComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (eventcheckBoxext.Checked) eventcheckBoxext.Checked = false;
            if (!eventcheckBoxext.Checked) eventcheckBoxext.Checked = true;
        }


        private void eventcheckBoxext_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void d1MaxiLabel_Click(object sender, EventArgs e)
        {

        }

        private void d2Label_Click(object sender, EventArgs e)
        {

        }

        private void attacheCentreGTrackbarTextBox_TextChanged(object sender, EventArgs e)
        {
            if (troisiemeAttacheCheckBox.Checked && !quatriemeAttacheCheckBox.Checked)
            {
                attacheCentreDTrackbarTextBox.Text = (GeneralClass.lgFixe - Convert.ToInt32(attacheCentreGTrackbarTextBox.Text)).ToString();
            }

            if (eventcheckBoxext.Checked) eventcheckBoxext.Checked = false;
            if (!eventcheckBoxext.Checked) eventcheckBoxext.Checked = true;
        }

        private void attacheCentreDTrackbarTextBox_TextChanged(object sender, EventArgs e)
        {
            if (troisiemeAttacheCheckBox.Checked && !quatriemeAttacheCheckBox.Checked)
            {
                attacheCentreGTrackbarTextBox.Text = (GeneralClass.lgFixe - Convert.ToInt32(attacheCentreDTrackbarTextBox.Text)).ToString();
            }

            if (eventcheckBoxext.Checked) eventcheckBoxext.Checked = false;
            if (!eventcheckBoxext.Checked) eventcheckBoxext.Checked = true;
        }

        private void sabotGauchePictureBox_Click(object sender, EventArgs e)
        {

        }
        private void sabotGauchePictureBox2_Click(object sender, EventArgs e)
        {

        }
        private void sabotDroitPictureBox_Click(object sender, EventArgs e)
        {

        }
        private void sabotDroitPictureBox2_Click(object sender, EventArgs e)
        {

        }
    }
}
#endregion