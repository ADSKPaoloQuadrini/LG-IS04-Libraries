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
    public partial class PiedForm : System.Windows.Forms.Form
    {
        public PiedForm()
        {
            InitializeComponent();
        }
        //Pied Gauche
        private void piedGaucheVerticalRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (piedGaucheVerticalRadioButton.Checked == true)
            {
                rallongeGauche105CheckBox.Visible = true;
                labelinfo1.Visible = true;
                piedGaucheVerticalComboBox.Visible = true;
                piedGaucheLongitudinalComboBox.Visible = false;
                piedGaucheTransversalComboBox.Visible=false;
                if (piedGaucheVerticalComboBox.SelectedItem == null)
                {
                    piedGaucheVerticalComboBox.SelectedIndex = 0;
                }
            }
        }
        private void piedGaucheLongitudinalRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (piedGaucheLongitudinalRadioButton.Checked == true)
            {
                rallongeGauche105CheckBox.Visible = false;
                rallongeGauche105CheckBox.Checked = false;
                rallongeGaucheComboBox.Visible = false;
                rallongeGauchePictureBox.Visible = false;
                labelinfo1.Visible = true;
                piedGaucheVerticalComboBox.Visible = false;
                piedGaucheLongitudinalComboBox.Visible = true;
                piedGaucheTransversalComboBox.Visible = false;
                if (piedGaucheLongitudinalComboBox.SelectedItem == null)
                {
                    piedGaucheLongitudinalComboBox.SelectedIndex = 0;
                }

            }
        }
        private void piedGaucheTransversalRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (piedGaucheTransversalRadioButton.Checked == true)
            {
                rallongeGauche105CheckBox.Visible = false;
                rallongeGauche105CheckBox.Checked = false;
                rallongeGaucheComboBox.Visible = false;
                rallongeGauchePictureBox.Visible = false;
                labelinfo1.Visible = false;
                piedGaucheVerticalComboBox.Visible = false;
                piedGaucheLongitudinalComboBox.Visible = false;
                piedGaucheTransversalComboBox.Visible = true;
                if (piedGaucheTransversalComboBox.SelectedItem == null)
                {
                    piedGaucheTransversalComboBox.SelectedIndex = 0;
                }
                
            }
        }

        //Pied Droit
        private void piedDroitVerticalRadioButon_CheckedChanged(object sender, EventArgs e)
        {
            if (piedDroitVerticalRadioButton.Checked == true)
            {
                labelinfo2.Visible = true;
                rallongeDroit105CheckBox.Visible = true;
                piedDroitVerticalComboBox.Visible = true;
                piedDroitLongitudinalComboBox.Visible = false;
                piedDroitTransversalComboBox.Visible = false;
                if (piedDroitVerticalComboBox.SelectedItem == null)
                {
                    piedDroitVerticalComboBox.SelectedIndex = 0;
                }
            }
        }       
        private void piedDroitLongitudinalRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (piedDroitLongitudinalRadioButton.Checked == true)
            {
                rallongeDroit105CheckBox.Visible = false;
                rallongeDroit105CheckBox.Checked = false;
                rallongeDroiteComboBox.Visible = false;
                rallongeDroitePictureBox.Visible = false;
                labelinfo2.Visible = true;
                piedDroitVerticalComboBox.Visible = false;
                piedDroitLongitudinalComboBox.Visible = true;
                piedDroitTransversalComboBox.Visible = false;
                if (piedDroitLongitudinalComboBox.SelectedItem == null)
                {
                    piedDroitLongitudinalComboBox.SelectedIndex = 0;
                }
               
            }
        }
        private void piedDroitTransversalRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (piedDroitTransversalRadioButton.Checked == true)
            {
                rallongeDroit105CheckBox.Visible = false;
                rallongeDroit105CheckBox.Checked = false;
                rallongeDroiteComboBox.Visible = false;
                rallongeDroitePictureBox.Visible = false;
                labelinfo2.Visible = false;
                piedDroitVerticalComboBox.Visible = false;
                piedDroitLongitudinalComboBox.Visible = false;
                piedDroitTransversalComboBox.Visible = true;
                if (piedDroitTransversalComboBox.SelectedItem == null)
                {
                    piedDroitTransversalComboBox.SelectedIndex = 0;
                }
       
            }
        }

        //Buttons
        private void applyFootButton_Click(object sender, EventArgs e)
        {

        }
        private void quiteFootButton_Click(object sender, EventArgs e)
        {

        }

        private void rallongeDroit105CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (rallongeDroit105CheckBox.Checked==true)
            {
                rallongeDroitePictureBox.Visible = true;
                rallongeDroiteComboBox.Visible=true;
                piedDroitVerticalComboBox.Visible = false;
            }
            else
            {
                rallongeDroitePictureBox.Visible = false;
                rallongeDroiteComboBox.Visible=false;
                piedDroitVerticalComboBox.Visible = true;
            }

        }

        private void rallongeGauche105CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (rallongeGauche105CheckBox.Checked == true)
            {
                rallongeGauchePictureBox.Visible = true;
                rallongeGaucheComboBox.Visible = true;
                piedGaucheVerticalComboBox.Visible = false;
            }
            else
            {
                rallongeGauchePictureBox.Visible = false;
                rallongeGaucheComboBox.Visible = false;
                piedGaucheVerticalComboBox.Visible = true;

            }
        }

        private void rallongeDroiteComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            piedDroitVerticalComboBox.SelectedIndex = piedDroitVerticalComboBox.FindStringExact((Convert.ToDecimal(rallongeDroiteComboBox.SelectedItem.ToString()) - 75).ToString());
        }

        private void rallongeGaucheComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            piedGaucheVerticalComboBox.SelectedIndex = piedGaucheVerticalComboBox.FindStringExact((Convert.ToDecimal(rallongeGaucheComboBox.SelectedItem.ToString()) - 75).ToString());
        }

        private void PiedForm_Load(object sender, EventArgs e)
        {

        }

        private void piedGaucheVerticalComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void piedDroitLongitudinalComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
