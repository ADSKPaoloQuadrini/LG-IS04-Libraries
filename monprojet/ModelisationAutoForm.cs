using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PTLGClassLibrary
{
    public partial class ModelisationAutoForm : Form
    {
        public ModelisationAutoForm()
        {
            InitializeComponent();
        }

        private void bancheComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (eventCheckBox.Checked == true)
                eventCheckBox.Checked = false;
            if (eventCheckBox.Checked == false)
                eventCheckBox.Checked = true;
        }

        private void angleDroitCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (eventCheckBox.Checked == true)
                eventCheckBox.Checked = false;
            if (eventCheckBox.Checked == false)
                eventCheckBox.Checked = true;
        }
        private void angleGaucheCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (eventCheckBox.Checked == true)
                eventCheckBox.Checked = false;
            if (eventCheckBox.Checked == false)
                eventCheckBox.Checked = true;
        }

        private void eventCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void option1CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (eventCheckBox.Checked == true)
                eventCheckBox.Checked = false;
            if (eventCheckBox.Checked == false)
                eventCheckBox.Checked = true;
        }
        private void option2CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (eventCheckBox.Checked == true)
                eventCheckBox.Checked = false;
            if (eventCheckBox.Checked == false)
                eventCheckBox.Checked = true;
        }
        private void option3CheckBox_CheckedChanged_1(object sender, EventArgs e)
        {
            if (eventCheckBox.Checked == true)
                eventCheckBox.Checked = false;
            if (eventCheckBox.Checked == false)
                eventCheckBox.Checked = true;
        }

        private void option1button_Click(object sender, EventArgs e)
        {
            option1CheckBox.Checked = true;
            option2CheckBox.Checked = false;
            option3CheckBox.Checked = false;
        }
        private void option2button_Click(object sender, EventArgs e)
        {
            option1CheckBox.Checked = false;
            option2CheckBox.Checked = true;
            option3CheckBox.Checked = false;
        }
        private void option3button_Click(object sender, EventArgs e)
        {
            option1CheckBox.Checked = false;
            option2CheckBox.Checked = false;
            option3CheckBox.Checked = true;
        }

        private void applyButton_Click(object sender, EventArgs e)
        {

        }

        




        
    }
}
