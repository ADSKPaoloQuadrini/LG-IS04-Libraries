#region Autodesk PQ - IS04
using System;
using static PTLGClassLibrary.GeneralClass;

namespace PTLGClassLibrary.ConfMan
{
    class M_LabelsBackColors
    {
		public static void ColoringMaxValues(ConfigurateurManForm f)
		{
			#region 1 Extension gauche
			if (lecturegauche > Convert.ToInt32(f.leGaucheMaxiLabel.Text))
			{
				f.leGaucheMaxiLabel.BackColor = System.Drawing.Color.HotPink;
			}
			else
			{
				f.leGaucheMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
			}
			#endregion test leMax

			#region 2 Passerelle biaisée droite, Ld >100
			if (f.angleBiaisDroitCheckBox.Checked == true)
			{
				if (Math.Abs(Convert.ToInt32(f.extensionDroiteComboBox.SelectedItem) - Convert.ToInt32(f.extensionDroiteIntComboBox.SelectedItem)) > 100)
				{
					f.angleBiaisDroitCheckBox.BackColor = System.Drawing.Color.HotPink;
					f.fondExtensionExtDroite.BackColor = System.Drawing.Color.HotPink;
					f.fondExtensionIntDroite.BackColor = System.Drawing.Color.HotPink;
				}
				else
				{
					f.fondExtensionExtDroite.BackColor = System.Drawing.Color.Transparent;
					f.fondExtensionIntDroite.BackColor = System.Drawing.Color.Transparent;
					f.angleBiaisDroitCheckBox.BackColor = System.Drawing.Color.LightGreen;
				}
			}
			else
			{
				f.fondExtensionExtDroite.BackColor = System.Drawing.Color.Transparent;
				f.fondExtensionIntDroite.BackColor = System.Drawing.Color.Transparent;
				f.angleBiaisDroitCheckBox.BackColor = System.Drawing.Color.Transparent;
			}
			#endregion

			#region 3 Passerelle biaisée gauche, Ld <100
			if (f.angleBiaisGaucheCheckBox.Checked == true)
			{
				if (Math.Abs(Convert.ToInt32(f.extensionGaucheComboBox.SelectedItem) - Convert.ToInt32(f.extensionGaucheIntComboBox.SelectedItem)) > 100)
				{
					f.angleBiaisGaucheCheckBox.BackColor = System.Drawing.Color.HotPink;
					f.fondExtensionIntGauche.BackColor = System.Drawing.Color.HotPink;
					f.fondExtensionExtGauche.BackColor = System.Drawing.Color.HotPink;
				}
				else
				{
					f.fondExtensionExtGauche.BackColor = System.Drawing.Color.Transparent;
					f.fondExtensionIntGauche.BackColor = System.Drawing.Color.Transparent;
					f.angleBiaisGaucheCheckBox.BackColor = System.Drawing.Color.LightGreen;
				}
			}
			else
			{
				f.fondExtensionExtGauche.BackColor = System.Drawing.Color.Transparent;
				f.fondExtensionIntGauche.BackColor = System.Drawing.Color.Transparent;
				f.angleBiaisGaucheCheckBox.BackColor = System.Drawing.Color.Transparent;
			}
			#endregion

			#region 4 Extension droite
			if (lecturedroite > Convert.ToInt32(f.leDroitMaxiLabel.Text))
			{
				f.leDroitMaxiLabel.BackColor = System.Drawing.Color.HotPink;
			}
			else
			{
				f.leDroitMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
			}
			#endregion

			#region 5 Longueur totale de la passerelle
			if (lgTotale > lgMax)
			{
				f.lgTotaleMaxiLabel.BackColor = System.Drawing.Color.HotPink;
			}
			else
			{
				f.lgTotaleMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
			}
			#endregion lgMax

			#region 6 attache-ferme gauche
			if (cGauche > cMax)
			{
				if (f.troisiemeAttacheCheckBox.Checked == false)
				{
					f.cGaucheMaxiLabel.BackColor = System.Drawing.Color.HotPink;
				}
			}
			else if (Math.Abs(cGauche) < 15)
			{
				if (f.attacheGaucheComboBox.SelectedIndex == 2)
				{
					if (Math.Abs(cGauche) < 12)
					{
						f.cGaucheMaxiLabel.BackColor = System.Drawing.Color.HotPink;
					}
					else
					{
						f.cGaucheMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
					}
				}
				else
				{
					f.cGaucheMaxiLabel.BackColor = System.Drawing.Color.HotPink;
				}
			}
			else
			{
				f.cGaucheMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
			}
			#endregion

			#region 7 attache-ferme droite
			if (cDroite > cMax)
			{
				if (f.troisiemeAttacheCheckBox.Checked == false)
				{
					f.cDroiteMaxiLabel.BackColor = System.Drawing.Color.HotPink;
				}
			}
			else if (Math.Abs(cDroite) < 15)
			{
				if (f.attacheDroiteComboBox.SelectedIndex == 2)
				{
					if (Math.Abs(cDroite) < 12)
					{
						f.cDroiteMaxiLabel.BackColor = System.Drawing.Color.HotPink;
					}
					else
					{
						f.cDroiteMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
					}
				}
				else
				{
					f.cDroiteMaxiLabel.BackColor = System.Drawing.Color.HotPink;
				}
			}
			else
			{
				f.cDroiteMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
			}
			#endregion

			#region Labels pour 3ème/4ème attache
			//if (cCentre > cMax)
			//{
			//	f.cCentreMaxiLabel.BackColor = System.Drawing.Color.HotPink;
			//}
			//else 
			if (Math.Abs(cCentre) < 15)
			{
				if (f.attacheDroiteComboBox.SelectedIndex == 2)
				{
					if (Math.Abs(cCentre) < 12)
					{
						f.cCentreMaxiLabel.BackColor = System.Drawing.Color.HotPink;
					}
					else
					{
						f.cCentreMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
					}
				}
				else
				{
					f.cCentreMaxiLabel.BackColor = System.Drawing.Color.HotPink;
				}
			}
			else
			{
				f.cCentreMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
			}

			//if (cCentreDroite > cMax)
			//{
			//	f.cCentreDroiteMaxiLabel.BackColor = System.Drawing.Color.HotPink;
			//}
			//else
			if (Math.Abs(cCentreDroite) < 15)
			{
				if (f.attacheDroiteComboBox.SelectedIndex == 2)
				{
					if (Math.Abs(cCentreDroite) < 12)
					{
						f.cCentreDroiteMaxiLabel.BackColor = System.Drawing.Color.HotPink;
					}
					else
					{
						f.cCentreDroiteMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
					}
				}
				else
				{
					f.cCentreDroiteMaxiLabel.BackColor = System.Drawing.Color.HotPink;
				}
			}
			else
			{
				f.cCentreDroiteMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
			}
			#endregion

			#region 8 pafGauche
			if (Convert.ToInt32(f.pafGaucheLabel.Text) > Convert.ToInt32(f.pafGaucheMaxiLabel.Text))
			{
				f.pafGaucheMaxiLabel.BackColor = System.Drawing.Color.HotPink;
			}
			else if (Convert.ToInt32(f.pafGaucheLabel.Text) < 0)
			{
				f.pafGaucheMaxiLabel.BackColor = System.Drawing.Color.HotPink;
			}
			else if (Convert.ToInt32(f.pafGaucheLabel.Text) < 20)
			{
				f.pafGaucheMaxiLabel.BackColor = System.Drawing.Color.HotPink;
			}
			else
			{
				f.pafGaucheMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
			}
			#endregion pafGauche

			#region 9 pafDroite
			if (Convert.ToInt32(f.pafDroitLabel.Text) > Convert.ToInt32(f.pafDroitMaxiLabel.Text))
			{
				f.pafDroitMaxiLabel.BackColor = System.Drawing.Color.HotPink;
			}
			else if (Convert.ToInt32(f.pafDroitLabel.Text) < 0)
			{
				f.pafDroitMaxiLabel.BackColor = System.Drawing.Color.HotPink;
			}
			else if (Convert.ToInt32(f.pafDroitLabel.Text) < 20)
			{
				f.pafDroitMaxiLabel.BackColor = System.Drawing.Color.HotPink;
			}
			else
			{
				f.pafDroitMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
			}
			#endregion pafDroite

			#region 10, 13, 14 D
			//int distanceg = 15;
			//int distanced = 15;
			//if (f.attacheDroiteComboBox.SelectedIndex == 2) distanced = 12;
			//if (f.attacheGaucheComboBox.SelectedIndex == 2) distanceg = 12;

			if (f.troisiemeAttacheCheckBox.Checked && !f.quatriemeAttacheCheckBox.Checked)
			{
				if (Convert.ToInt32(f.d1Label.Text) > Convert.ToInt32(f.dMaxiLabel.Text) || Convert.ToInt32(f.d1Label.Text) < 15)
				{
					f.d1MaxiLabel.BackColor = System.Drawing.Color.HotPink;
				}
				else
				{
					f.d1MaxiLabel.BackColor = System.Drawing.Color.LightGreen;
				}
				if (Convert.ToInt32(f.d2Label.Text) > Convert.ToInt32(f.dMaxiLabel.Text) || Convert.ToInt32(f.d2Label.Text) < 15)
				{
					f.d2MaxiLabel.BackColor = System.Drawing.Color.HotPink;
				}
				else
				{
					f.d2MaxiLabel.BackColor = System.Drawing.Color.LightGreen;
				}

				

				//if (Math.Abs(Convert.ToInt32(f.cGaucheMaxiLabel.Text)) > distanceg)
				//{
				//	f.cGaucheMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
				//}
				//if (Math.Abs(Convert.ToInt32(f.cDroiteMaxiLabel.Text)) > distanced)
				//{
				//	f.cDroiteMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
				//}
				//if (Math.Abs(Convert.ToInt32(f.cCentreMaxiLabel.Text)) > distanceg)
				//{
				//	f.cCentreMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
				//}
				//if (Math.Abs(Convert.ToInt32(f.cCentreDroiteMaxiLabel.Text)) > distanced)
				//{
				//	f.cCentreDroiteMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
				//}
			}
			else if (f.troisiemeAttacheCheckBox.Checked && f.quatriemeAttacheCheckBox.Checked)
			{
				if (Convert.ToInt32(f.dLabel.Text) > Convert.ToInt32(f.dMaxiLabel.Text) || Convert.ToInt32(f.dLabel.Text) < 15)
				{
					f.dMaxiLabel.BackColor = System.Drawing.Color.HotPink;
				}
				else
				{
					f.dMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
				}
				if (Convert.ToInt32(f.d1Label.Text) > Convert.ToInt32(f.dMaxiLabel.Text) ||	Convert.ToInt32(f.d1Label.Text) < 15)
				{
					f.d1MaxiLabel.BackColor = System.Drawing.Color.HotPink;
				}
				else
				{
					f.d1MaxiLabel.BackColor = System.Drawing.Color.LightGreen;
				}
				if (Convert.ToInt32(f.d2Label.Text) > Convert.ToInt32(f.dMaxiLabel.Text) || Convert.ToInt32(f.d2Label.Text) < 15)
				{
					f.d2MaxiLabel.BackColor = System.Drawing.Color.HotPink;
				}
				else
				{
					f.d2MaxiLabel.BackColor = System.Drawing.Color.LightGreen;
				}
			}
			else if (Convert.ToInt32(f.dLabel.Text) > Convert.ToInt32(f.dMaxiLabel.Text))
			{
				f.dMaxiLabel.BackColor = System.Drawing.Color.HotPink;
			}
			else
			{
				f.dMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
			}
			#endregion D

			#region 11 banche selectionnée
			if (f.bancheComboBox.SelectedItem.ToString() != "Pas de banche")
			{
				if (f.stabilisationComboBox.SelectedItem == null)
				{
					f.stabilisationLabel.BackColor = System.Drawing.Color.HotPink;
				}
				else
				{
					f.stabilisationLabel.BackColor = System.Drawing.Color.Transparent;
				}
			}
			#endregion banche selectionnée

			#region 12 numéro passerelle
			if (string.IsNullOrEmpty(f.numeroTextBox.Text) || f.numeroTextBox.Text == "-")
			{
				f.numeroTextBox.BackColor = System.Drawing.Color.HotPink;
			}
			else
			{
				f.numeroTextBox.BackColor = System.Drawing.Color.White;
			}
			#endregion

		}
	}
}
#endregion