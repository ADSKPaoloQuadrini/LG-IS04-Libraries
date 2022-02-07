#region Autodesk PQ - IS04
using System;
using static PTLGClassLibrary.GeneralClass;

namespace PTLGClassLibrary.ConfGen
{
	class G_LabelsBackColors
	{
        public static void ColoringMaxValuesErrors(ConfigurateurGenForm f)
        {
            #region 1 Extension gauche
            if (lecturegauche > Convert.ToInt32(f.leGaucheMaxiLabel.Text))
            {
                f.leGaucheMaxiLabel.BackColor = System.Drawing.Color.HotPink;
                erreur1 = "Extension gauche trop grande" + "\n";
            }
            else
            {
                f.leGaucheMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
                erreur1 = "";
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
                    erreur2 = "Réduire le delta des extensions int et ext à droite" + "\n";
                }
                else
                {
                    f.fondExtensionExtDroite.BackColor = System.Drawing.Color.Transparent;
                    f.fondExtensionIntDroite.BackColor = System.Drawing.Color.Transparent;
                    f.angleBiaisDroitCheckBox.BackColor = System.Drawing.Color.LightGreen;
                    erreur2 = "";
                }
            }
            else
            {
                f.fondExtensionExtDroite.BackColor = System.Drawing.Color.Transparent;
                f.fondExtensionIntDroite.BackColor = System.Drawing.Color.Transparent;
                f.angleBiaisDroitCheckBox.BackColor = System.Drawing.Color.Transparent;
                erreur2 = "";
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
                    erreur3 = "Réduire le delta des extensions int et ext à gauche" + "\n";
                }
                else
                {
                    f.fondExtensionExtGauche.BackColor = System.Drawing.Color.Transparent;
                    f.fondExtensionIntGauche.BackColor = System.Drawing.Color.Transparent;
                    f.angleBiaisGaucheCheckBox.BackColor = System.Drawing.Color.LightGreen;
                    erreur3 = "";
                }
            }
            else
            {
                f.fondExtensionExtGauche.BackColor = System.Drawing.Color.Transparent;
                f.fondExtensionIntGauche.BackColor = System.Drawing.Color.Transparent;
                f.angleBiaisGaucheCheckBox.BackColor = System.Drawing.Color.Transparent;
                erreur3 = "";
            }
            #endregion

            #region 4 Extension droite
            if (lecturedroite > Convert.ToInt32(f.leDroitMaxiLabel.Text))
            {
                f.leDroitMaxiLabel.BackColor = System.Drawing.Color.HotPink;
                erreur4 = "Extension droite trop grande" + "\n";
            }
            else
            {
                f.leDroitMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
                erreur4 = "";
            }
            #endregion

            #region 5 Longueur totale de la passerelle
            if (lgTotale > lgMax)
            {
                f.lgTotaleMaxiLabel.BackColor = System.Drawing.Color.HotPink;
                erreur5 = "Longueur totale de la passerelle trop grande" + "\n";
            }
            else
            {
                f.lgTotaleMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
                erreur5 = "";
            }
            #endregion lgMax

            #region 6 attache-ferme gauche
            if (cGauche > cMax)
            {
                if (f.troisiemeAttacheCheckBox.Checked == false)
                {
                    f.cGaucheMaxiLabel.BackColor = System.Drawing.Color.HotPink;
                    GeneralClass.erreur6 = "Distance attache-ferme gauche trop grande" + "\n";
                }
            }
            else if (Math.Abs(cGauche) < 15)
            {
                if (f.attacheGaucheComboBox.SelectedIndex == 2)
                {
                    if (Math.Abs(cGauche) < 12)
                    {
                        f.cGaucheMaxiLabel.BackColor = System.Drawing.Color.HotPink;
                        erreur6 = "Distance attache-ferme gauche inférieure à 12" + "\n";
                    }
                    else
                    {
                        f.cGaucheMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
                        erreur6 = "";
                    }
                }
                else
                {
                    f.cGaucheMaxiLabel.BackColor = System.Drawing.Color.HotPink;
                    erreur6 = "Distance attache-ferme gauche inférieure à 15" + "\n";
                }
            }
            else
            {
                f.cGaucheMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
                erreur6 = "";
            }
            #endregion

            #region 7 attache-ferme droite
            if (cDroite > cMax)
            {
                if (f.troisiemeAttacheCheckBox.Checked == false)
                {
                    f.cDroiteMaxiLabel.BackColor = System.Drawing.Color.HotPink;
                    erreur7 = "Distance attache-ferme droite trop grande" + "\n";
                }
            }
            else if (Math.Abs(cDroite) < 15)
            {
                if (f.attacheDroiteComboBox.SelectedIndex == 2)
                {
                    if (Math.Abs(cDroite) < 12)
                    {
                        f.cDroiteMaxiLabel.BackColor = System.Drawing.Color.HotPink;
                        erreur7 = "Distance attache-ferme droite inférieure à 12" + "\n";
                    }
                    else
                    {
                        f.cDroiteMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
                        erreur7 = "";
                    }
                }
                else
                {
                    f.cDroiteMaxiLabel.BackColor = System.Drawing.Color.HotPink;
                    erreur7 = "Distance attache-ferme droite inférieure à 15" + "\n";
                }
            }
            else
            {
                f.cDroiteMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
                erreur7 = "";
            }
            #endregion

            #region NON - Labels pour 3ème attache
            //if (cCentre > cMax)
            //{
            //    f.cCentreMaxiLabel.BackColor = System.Drawing.Color.HotPink;
            //}
            //else
            //if (Math.Abs(cCentre) < 15)
            //{
            //    if (f.attacheDroiteComboBox.SelectedIndex == 2)
            //    {
            //        if (Math.Abs(cCentre) < 12)
            //        {
            //            f.cCentreMaxiLabel.BackColor = System.Drawing.Color.HotPink;
            //        }
            //        else
            //        {
            //            f.cCentreMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
            //        }
            //    }
            //    else
            //    {
            //        f.cCentreMaxiLabel.BackColor = System.Drawing.Color.HotPink;
            //    }
            //}
            //else
            //{
            //    f.cCentreMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
            //}

            //if (cCentreDroite > cMax)
            //{
            //    f.cCentreDroiteMaxiLabel.BackColor = System.Drawing.Color.HotPink;
            //}
            //else
            //if (Math.Abs(cCentreDroite) < 15)
            //{
            //    if (f.attacheDroiteComboBox.SelectedIndex == 2)
            //    {
            //        if (Math.Abs(cCentreDroite) < 12)
            //        {
            //            f.cCentreDroiteMaxiLabel.BackColor = System.Drawing.Color.HotPink;
            //        }
            //        else
            //        {
            //            f.cCentreDroiteMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
            //        }
            //    }
            //    else
            //    {
            //        f.cCentreDroiteMaxiLabel.BackColor = System.Drawing.Color.HotPink;
            //    }
            //}
            //else
            //{
            //    f.cCentreDroiteMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
            //}
            #endregion NON

            #region 8 pafGauche
            if (Convert.ToInt32(f.pafGaucheLabel.Text) > Convert.ToInt32(f.pafGaucheMaxiLabel.Text))
            {
                f.pafGaucheMaxiLabel.BackColor = System.Drawing.Color.HotPink;
                erreur8 = "Porte à faux gauche trop grand" + "\n";
            }
            else if (Convert.ToInt32(f.pafGaucheLabel.Text) < 0)
            {
                f.pafGaucheMaxiLabel.BackColor = System.Drawing.Color.HotPink;
                erreur8 = "Attache gauche en dehors de la passerelle" + "\n";
            }
            else if (Convert.ToInt32(f.pafGaucheLabel.Text) < 20)
            {
                f.pafGaucheMaxiLabel.BackColor = System.Drawing.Color.HotPink;
                erreur8 = "Distance attache-rive gauche extension inférieure à 20" + "\n";
            }
            else
            {
                f.pafGaucheMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
                erreur8 = "";
            }
            #endregion pafGauche

            #region 9 pafDroite
            if (Convert.ToInt32(f.pafDroitLabel.Text) > Convert.ToInt32(f.pafDroitMaxiLabel.Text))
            {
                f.pafDroitMaxiLabel.BackColor = System.Drawing.Color.HotPink;
                erreur9 = "Porte à faux droit trop grand" + "\n";
            }
            else if (Convert.ToInt32(f.pafDroitLabel.Text) < 0)
            {
                f.pafDroitMaxiLabel.BackColor = System.Drawing.Color.HotPink;
                erreur9 = "Attache droite en dehors de la passerelle" + "\n";
            }
            else if (Convert.ToInt32(f.pafDroitLabel.Text) < 20)
            {
                f.pafDroitMaxiLabel.BackColor = System.Drawing.Color.HotPink;
                erreur9 = "Distance attache-rive droite extension inférieure à 20" + "\n";
            }
            else
            {
                f.pafDroitMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
                erreur9 = "";
            }
            #endregion pafDroite

            #region 10, 13, 14 D
            //int distanceg = 15;
            //int distanced = 15;
            //if (f.attacheDroiteComboBox.SelectedIndex == 2) distanced = 12;
            //if (f.attacheGaucheComboBox.SelectedIndex == 2) distanceg = 12;

            if (type == "M3" && f.troisiemeAttacheCheckBox.Checked) // && !f.quatriemeAttacheCheckBox.Checked)
            {
                if (Convert.ToInt32(f.d1Label.Text) > Convert.ToInt32(f.dMaxiLabel.Text) || Convert.ToInt32(f.d1Label.Text) < 15)
                {
                    f.d1MaxiLabel.BackColor = System.Drawing.Color.HotPink;
                    erreur13 = "Distance entre attache gauche et troisieme attache trop grande" + "\n";
                }
                else
                {
                    f.d1MaxiLabel.BackColor = System.Drawing.Color.LightGreen;
                    erreur13 = "";
                }
                if (Convert.ToInt32(f.d2Label.Text) > Convert.ToInt32(f.dMaxiLabel.Text) || Convert.ToInt32(f.d2Label.Text) < 15)
                {
                    f.d2MaxiLabel.BackColor = System.Drawing.Color.HotPink;
                    erreur14 = "Distance entre attache droite et troisieme attache trop grande" + "\n";
                }
                else
                {
                    f.d2MaxiLabel.BackColor = System.Drawing.Color.LightGreen;
                    erreur14 = "";
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
            //else if (f.troisiemeAttacheCheckBox.Checked && f.quatriemeAttacheCheckBox.Checked)
            //{
            //    if (Convert.ToInt32(f.dLabel.Text) > Convert.ToInt32(f.dMaxiLabel.Text) || Convert.ToInt32(f.dLabel.Text) < 15)
            //    {
            //        f.dMaxiLabel.BackColor = System.Drawing.Color.HotPink;
            //    }
            //    else
            //    {
            //        f.dMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
            //    }
            //    if (Convert.ToInt32(f.d1Label.Text) > Convert.ToInt32(f.dMaxiLabel.Text) || Convert.ToInt32(f.d1Label.Text) < 15)
            //    {
            //        f.d1MaxiLabel.BackColor = System.Drawing.Color.HotPink;
            //    }
            //    else
            //    {
            //        f.d1MaxiLabel.BackColor = System.Drawing.Color.LightGreen;
            //    }
            //    if (Convert.ToInt32(f.d2Label.Text) > Convert.ToInt32(f.dMaxiLabel.Text) || Convert.ToInt32(f.d2Label.Text) < 15)
            //    {
            //        f.d2MaxiLabel.BackColor = System.Drawing.Color.HotPink;
            //    }
            //    else
            //    {
            //        f.d2MaxiLabel.BackColor = System.Drawing.Color.LightGreen;
            //    }
            //}
            else if (Convert.ToInt32(f.dLabel.Text) > Convert.ToInt32(f.dMaxiLabel.Text))
            {
                if (f.troisiemeAttacheCheckBox.Checked == false)
                {
                    f.dMaxiLabel.BackColor = System.Drawing.Color.HotPink;
                    erreur10 = "Distance entre attaches trop grande" + "\n";
                }
            }
            else
            {
                f.dMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
                erreur10 = "";
            }
            #endregion D

            #region 11 banche selectionnée
            if (f.bancheComboBox.SelectedItem.ToString() == "Pas de banche")
            {
                erreur11 = "";
            }
            else
            {
                if (f.stabilisationComboBox.SelectedItem == null)
                {
                    f.stabilisationLabel.BackColor = System.Drawing.Color.HotPink;
                    erreur11 = "Sélectionner une stabilisation de banche" + "\n";
                }
                else
                {
                    f.stabilisationLabel.BackColor = System.Drawing.Color.Transparent;
                    erreur11 = "";
                }
            }
            #endregion banche selectionnée

            #region 12 numéro passerelle
            if (string.IsNullOrEmpty(f.numeroTextBox.Text) || f.numeroTextBox.Text == "-")
            {
                f.numeroTextBox.BackColor = System.Drawing.Color.HotPink;
                erreur12 = "Donner un numéro de passerelle" + "\n";
            }
            else
            {
                f.numeroTextBox.BackColor = System.Drawing.Color.White;
                erreur12 = "";
            }
            #endregion


            #region Affichage de l'erreur
            f.commentaireLabel.Text = erreur4 + erreur5 + erreur3 + erreur2 + erreur6 + erreur7 + erreur8 + erreur9 + erreur10 + erreur11 + erreur12 + erreur13 + erreur14;
            if (f.commentaireLabel.Text == "")
            {
                f.errorPictureBox.Visible = false;
                f.commentaireLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            }
            else
            {
                f.errorPictureBox.Visible = true;
                f.commentaireLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            }
            #endregion affichage de l'erreur
        }
    }
}
#endregion