using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit;
using Autodesk.Revit.Attributes;

namespace PTLGClassLibrary
{
    public partial class ConfigurateurForm : System.Windows.Forms.Form
    {
        public ConfigurateurForm()
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
            if (eventcheckBoxext.Checked == true)
                eventcheckBoxext.Checked = false;
            if (eventcheckBoxext.Checked == false)
                eventcheckBoxext.Checked = true;
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

            if (eventcheckBoxext.Checked == true)
                eventcheckBoxext.Checked = false;
            if (eventcheckBoxext.Checked == false)
                eventcheckBoxext.Checked = true;
        }
        private void stabilisationComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (eventcheckBoxext.Checked == true)
                eventcheckBoxext.Checked = false;
            if (eventcheckBoxext.Checked == false)
                eventcheckBoxext.Checked = true;
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
            if (eventcheckBoxext.Checked == true)
                eventcheckBoxext.Checked = false;
            if (eventcheckBoxext.Checked == false)
                eventcheckBoxext.Checked = true;
        }
        private void extensionGaucheIntComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (eventcheckBoxext.Checked == true)
                eventcheckBoxext.Checked = false;
            if (eventcheckBoxext.Checked == false)
                eventcheckBoxext.Checked = true;
        }
        private void extensionDroiteComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (eventcheckBoxext.Checked == true)
                eventcheckBoxext.Checked = false;
            if (eventcheckBoxext.Checked == false)
                eventcheckBoxext.Checked = true;
        }
        private void extensionDroiteIntComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (eventcheckBoxext.Checked == true)
                eventcheckBoxext.Checked = false;
            if (eventcheckBoxext.Checked == false)
                eventcheckBoxext.Checked = true;
        }

        //Attaches
        private void attacheGaucheTrackbar_Scroll(object sender, EventArgs e)
        {
            attacheGaucheTrackbarTextBox.Text =((-attacheGaucheTrackbar.Value).ToString());
            if (eventcheckBoxext.Checked == true)
                eventcheckBoxext.Checked = false;
            if (eventcheckBoxext.Checked == false)
                eventcheckBoxext.Checked = true;
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
            if (eventcheckBoxext.Checked == true)
                eventcheckBoxext.Checked = false;
            if (eventcheckBoxext.Checked == false)
                eventcheckBoxext.Checked = true;
        }
        private void attacheDroiteTrackbar_Scroll(object sender, EventArgs e)
        {
            attacheDroiteTrackbarTextBox.Text = ((attacheDroiteTrackbar.Value).ToString());
            if (eventcheckBoxext.Checked == true)
                eventcheckBoxext.Checked = false;
            if (eventcheckBoxext.Checked == false)
                eventcheckBoxext.Checked = true;
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
            if (eventcheckBoxext.Checked == true)
                eventcheckBoxext.Checked = false;
            if (eventcheckBoxext.Checked == false)
                eventcheckBoxext.Checked = true;
        }

        //Troisieme attache
        private void troisiemeAttacheCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (troisiemeAttacheCheckBox.Checked == true)
            {
                attacheCentreComboBox.Visible = true;
                attacheCentreTrackbar.Visible = true;
                dLabel.Visible = false;
                dMaxiLabel.Visible = false;
                d1Label.Visible = true;
                d2Label.Visible = true;
                d1MaxiLabel.Visible = true;
                d2MaxiLabel.Visible = true;
                this.dMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
                GeneralClass.erreur10 = "";
                int distanceg = 15;
                int distanced = 15;
                if(attacheDroiteComboBox.SelectedIndex==2)
                {
                    distanced = 12;
                }
                if (attacheGaucheComboBox.SelectedIndex == 2)
                {
                    distanceg = 12;
                }
                if (Math.Abs(Convert.ToInt32(this.cGaucheMaxiLabel.Text))>distanceg)
                {
                    this.cGaucheMaxiLabel.BackColor= System.Drawing.Color.LightGreen;
                    GeneralClass.erreur6 = "";
                }
                if (Math.Abs(Convert.ToInt32(this.cDroiteMaxiLabel.Text)) > distanced)
                {
                    this.cDroiteMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
                    GeneralClass.erreur7 = "";
                }
            }
            else
            {
                attacheCentreComboBox.Visible = false;
                attacheCentreTrackbar.Visible = false;
                dLabel.Visible = true;
                dMaxiLabel.Visible = true;
                d1Label.Visible = false;
                d2Label.Visible = false;
                d1MaxiLabel.Visible = false;
                d2MaxiLabel.Visible = false;
            }
            if (eventcheckBoxext.Checked == true)
                eventcheckBoxext.Checked = false;
            if (eventcheckBoxext.Checked == false)
                eventcheckBoxext.Checked = true;
        }
        private void attacheCentreComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (eventcheckBoxext.Checked == true)
                eventcheckBoxext.Checked = false;
            if (eventcheckBoxext.Checked == false)
                eventcheckBoxext.Checked = true;
        }
        private void attacheCentreTrackbar_Scroll(object sender, EventArgs e)
        {
            if (eventcheckBoxext.Checked == true)
                eventcheckBoxext.Checked = false;
            if (eventcheckBoxext.Checked == false)
                eventcheckBoxext.Checked = true;
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
            if (eventcheckBoxext.Checked == true)
                eventcheckBoxext.Checked = false;
            if (eventcheckBoxext.Checked == false)
                eventcheckBoxext.Checked = true;
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
            if (eventcheckBoxext.Checked == true)
                eventcheckBoxext.Checked = false;
            if (eventcheckBoxext.Checked == false)
                eventcheckBoxext.Checked = true;
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
            if (eventcheckBoxext.Checked == true)
                eventcheckBoxext.Checked = false;
            if (eventcheckBoxext.Checked == false)
                eventcheckBoxext.Checked = true;
        }
        private void angleBiaisDroitCheckBox_CheckedChanged_1(object sender, EventArgs e)
        {
            if (angleBiaisDroitCheckBox.Checked == true)
            {
                extensionDroiteIntComboBox.Visible = true;
                angleDroitCheckBox.Checked = false;
            }
            else extensionDroiteIntComboBox.Visible = false;
            if (eventcheckBoxext.Checked == true)
                eventcheckBoxext.Checked = false;
            if (eventcheckBoxext.Checked == false)
                eventcheckBoxext.Checked = true;
        }
        
        //Angle intérieur
        private void angleGaucheInterieurCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (angleGaucheInterieurCheckBox.Checked == true)
            {
                angleGaucheCheckBox.Checked = false;
                sabotGauchePictureBox.Visible = true;
                sabotGaucheTextBox2.Visible = true;
                sabotGaucheTrackBar.Visible = true;

            }
            else
            {
                sabotGauchePictureBox.Visible = false;
                sabotGaucheTextBox2.Visible = false;
                sabotGaucheTrackBar.Visible = false;
            }

            if (eventcheckBoxext.Checked == true)
                eventcheckBoxext.Checked = false;
            if (eventcheckBoxext.Checked == false)
                eventcheckBoxext.Checked = true;
        }
        private void angleDroitInterieurCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (angleDroitInterieurCheckBox.Checked == true)
            {
                angleDroitCheckBox.Checked = false;
                sabotDroitPictureBox.Visible = true;
                sabotDroitTextBox2.Visible = true;
                sabotDroitTrackBar.Visible = true;
            }
            else
            {
                sabotDroitPictureBox.Visible = false;
                sabotDroitTextBox2.Visible = false;
                sabotDroitTrackBar.Visible = false;
            }
            if (eventcheckBoxext.Checked == true)
                eventcheckBoxext.Checked = false;
            if (eventcheckBoxext.Checked == false)
                eventcheckBoxext.Checked = true;
        }

        //Sabots
        private void sabotGaucheTrackBar_Scroll(object sender, EventArgs e)
        {
            sabotGaucheTextBox2.Text = ((-sabotGaucheTrackBar.Value).ToString());
        }

        /*private void sabotGaucheTextBox2_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(sabotGaucheTextBox2.Text) || sabotGaucheTextBox2.Text == "-")
            {
                sabotGaucheTextBoxFantome2.Text = "-105";
            }
            else
            {
                sabotGaucheTextBoxFantome2.Text = (Convert.ToInt32(sabotGaucheTextBox2.Text) - 105).ToString();
            }
            if (eventcheckBoxext.Checked == true)
                eventcheckBoxext.Checked = false;
            if (eventcheckBoxext.Checked == false)
                eventcheckBoxext.Checked = true;
        }*/

        /*private void sabotGaucheTextBoxFantome_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(sabotGaucheTextBoxFantome2.Text) || sabotGaucheTextBoxFantome2.Text == "-")
            {
                sabotGaucheTrackBar.Value = 105;
            }
            else if (Convert.ToInt32(sabotGaucheTextBoxFantome2.Text) > 70)
            {
                sabotGaucheTextBoxFantome2.Text = "70";
                sabotGaucheTrackBar.Value = 175;
            }
            else if (Convert.ToInt32(sabotGaucheTextBoxFantome2.Text) < -70)
            {
                sabotGaucheTextBoxFantome2.Text = "-70";
                sabotGaucheTrackBar.Value = 35;
            }
            else sabotGaucheTrackBar.Value = Convert.ToInt32(sabotGaucheTextBoxFantome2.Text)+105;

            if (eventcheckBoxext.Checked == true)
                eventcheckBoxext.Checked = false;
            if (eventcheckBoxext.Checked == false)
                eventcheckBoxext.Checked = true;
        }*/
        private void sabotDroitTrackBar_Scroll(object sender, EventArgs e)
        {
            sabotDroitTextBox2.Text = ((-sabotDroitTrackBar.Value).ToString());
        }
       private void sabotDroitTextBox2_TextChanged(object sender, EventArgs e)
        {
            /*if (string.IsNullOrEmpty(sabotDroitTextBox2.Text) || sabotDroitTextBox2.Text == "-")
            {
                sabotDroitTextBoxFantome2.Text = "-105";
            }
            else
            {
                sabotDroitTextBoxFantome2.Text = (Convert.ToInt32(sabotDroitTextBox2.Text) - 105).ToString();
            }*/
            if (eventcheckBoxext.Checked == true)
                eventcheckBoxext.Checked = false;
            if (eventcheckBoxext.Checked == false)
                eventcheckBoxext.Checked = true;
        }
        /*private void sabotDroitTextBoxFantome_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(sabotDroitTextBoxFantome2.Text) || sabotDroitTextBoxFantome2.Text == "-")
            {
                sabotDroitTrackBar.Value = 105;
            }
            else if (Convert.ToInt32(sabotDroitTextBoxFantome2.Text) > 70)
            {
                sabotDroitTextBoxFantome2.Text = "70";
                sabotDroitTrackBar.Value = 175;
            }
            else if (Convert.ToInt32(sabotDroitTextBoxFantome2.Text) < -70)
            {
                sabotDroitTextBoxFantome2.Text = "-70";
                sabotDroitTrackBar.Value = 35;
            }
            else sabotDroitTrackBar.Value = Convert.ToInt32(sabotDroitTextBoxFantome2.Text) + 105;

            if (eventcheckBoxext.Checked == true)
                eventcheckBoxext.Checked = false;
            if (eventcheckBoxext.Checked == false)
                eventcheckBoxext.Checked = true;
        }*/

        private void ConfigurateurForm_Load(object sender, EventArgs e)
        {

        }

        private void extArriereCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (eventcheckBoxext.Checked == true)
                eventcheckBoxext.Checked = false;
            if (eventcheckBoxext.Checked == false)
                eventcheckBoxext.Checked = true;
        }

        private void sabotDroitPictureBox_Click(object sender, EventArgs e)
        {

        }

        private void sabotDroitTextBox2_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void pafGaucheMaxiLabel_Click(object sender, EventArgs e)
        {

        }

        private void attacheGaucheComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void commentaireLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
