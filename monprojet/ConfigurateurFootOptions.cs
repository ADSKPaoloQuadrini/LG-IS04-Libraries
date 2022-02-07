using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using static PTLGClassLibrary.GeneralClass;

namespace PTLGClassLibrary
{
    class ConfigurateurFootOptions
    {
        public static void FootOptionsGen(ConfigurateurGenForm f, Element e, Document doc)
        {
            #region footButton
            using (PiedForm g = new PiedForm())
            {
                f.footButton.Click += (sendor, o) =>
                {
                    Foot(g, e, doc);
                    g.ShowDialog();
                };
                #endregion footButton

                #region optionsButton
                //optionsButton
                using (optionsForm op = new optionsForm())
                {
                    f.optionsButton.Click += (sendur, u) =>
                    {
                        Options(op, e, doc);
                        op.ShowDialog();
                    };

                    f.ShowDialog();
                }
            }
            #endregion optionsButton
        }

        public static void FootOptionsMan(ConfigurateurManForm f, Element e, Document doc)
        {
            #region footButton
            using (PiedForm g = new PiedForm())
            {
                f.footButton.Click += (sendor, o) =>
                {
                    Foot(g, e, doc);
                    g.ShowDialog();
                };
                #endregion footButton

                #region optionsButton
                //optionsButton
                using (optionsForm op = new optionsForm())
                {
                    f.optionsButton.Click += (sendur, u) =>
                    {
                        Options(op, e, doc);
                        op.ShowDialog();
                    };

                    f.ShowDialog();
                }
            }
            #endregion optionsButton
        }

        private static void Foot(PiedForm g, Element e, Document doc)
        {
            #region initialisation des pieds
            double Rallonge105cm = 300;
            while (Rallonge105cm < 440)
            {
                g.rallongeGaucheComboBox.Items.Add(Rallonge105cm);
                g.rallongeDroiteComboBox.Items.Add(Rallonge105cm);
                Rallonge105cm += 7.5;
            }
            double Rallonge180cm = 375;
            while (Rallonge180cm < 515)
            {
                g.rallonge180GaucheComboBox.Items.Add(Rallonge180cm);
                g.rallonge180DroiteComboBox.Items.Add(Rallonge180cm);
                Rallonge180cm += 7.5;
            }
            g.rallongeGaucheComboBox.SelectedIndex = 0;
            g.rallongeDroiteComboBox.SelectedIndex = 0;
            g.rallonge180GaucheComboBox.SelectedIndex = 0;
            g.rallonge180DroiteComboBox.SelectedIndex = 0;

            #region pied gauche
            //initialisation pied gauche
            if (e.LookupParameter("Pied vertical gauche").AsInteger() == vrai)
            {
                g.piedGaucheVerticalRadioButton.Checked = true;
                g.rallongeGauche105CheckBox.Visible = true;
                g.rallongeGauche180CheckBox.Visible = true;
                //Initialisation de longueur pied vertical gauche 
                //si la lg du pied n'est pas dans la liste des données acceptables, on positionne par défaut sur la plus petite longueur
                if (g.piedGaucheVerticalComboBox.Items.Contains((GeneralClass.RetournePiedEnCM((e.LookupParameter("Longueur pied vertical gauche").AsDouble()))).ToString()))
                {
                    g.piedGaucheVerticalComboBox.SelectedIndex = g.piedGaucheVerticalComboBox.FindStringExact
                         ((GeneralClass.RetournePiedEnCM((e.LookupParameter("Longueur pied vertical gauche").AsDouble()))).ToString());
                }
                else
                {
                    g.piedGaucheVerticalComboBox.SelectedIndex = 0;
                }
                //initialisation rallonge de pied gauche 105
                if (e.LookupParameter("Rallonge de pied gauche").AsInteger() == vrai)
                {
                    g.rallongeGauche105CheckBox.Checked = true;
                    g.rallongeGauche180CheckBox.Checked = false;
                    g.rallongeGauchePictureBox.Visible = true;
                    g.rallongeGaucheComboBox.Visible = true;
                    g.rallonge180GaucheComboBox.Visible = false;
                    g.piedGaucheVerticalComboBox.Visible = false;
                    try
                    {
                        g.rallongeGaucheComboBox.SelectedIndex = g.rallongeGaucheComboBox.FindStringExact(GeneralClass.RetournePiedEnCM((e.LookupParameter("Longueur pied vertical gauche").AsDouble())).ToString());
                    }
                    catch { }
                }
                else
                {
                    //initialisation rallonge de pied gauche 180
                    if (e.LookupParameter("LG_MET_PTLG_Rallonge180G").AsInteger() == vrai)
                    {
                        g.rallongeGauche105CheckBox.Checked = false;
                        g.rallongeGauche180CheckBox.Checked = true;
                        g.rallongeGauchePictureBox.Visible = true;
                        g.rallongeGaucheComboBox.Visible = false;
                        g.rallonge180GaucheComboBox.Visible = true;
                        g.piedGaucheVerticalComboBox.Visible = false;
                        try
                        {
                            g.rallonge180GaucheComboBox.SelectedIndex = g.rallonge180GaucheComboBox.FindStringExact((GeneralClass.RetournePiedEnCM(e.LookupParameter("Longueur pied vertical gauche").AsDouble())).ToString());
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        g.rallongeGauche105CheckBox.Checked = false;
                        g.rallongeGauche180CheckBox.Checked = false;
                        g.rallongeGauchePictureBox.Visible = false;
                        g.rallongeGaucheComboBox.Visible = false;
                        g.rallonge180GaucheComboBox.Visible = false;
                        g.piedGaucheVerticalComboBox.Visible = true;
                    }
                }
            }
            else
            {
                g.rallongeGauche105CheckBox.Visible = false;
                g.rallongeGauche180CheckBox.Visible = false;
            }
            if (e.LookupParameter("Pied longitudinal gauche").AsInteger() == vrai)
            {
                g.piedGaucheLongitudinalRadioButton.Checked = true;
                //Initialisation de longueur pied longitudinal gauche
                if (g.piedGaucheLongitudinalComboBox.Items.Contains(e.LookupParameter("Longueur pied longitudinal gauche").AsValueString()))
                {
                    g.piedGaucheLongitudinalComboBox.SelectedIndex = g.piedGaucheLongitudinalComboBox.FindStringExact
                        (e.LookupParameter("Longueur pied longitudinal gauche").AsValueString());
                }
                else
                {
                    g.piedGaucheLongitudinalComboBox.SelectedIndex = 0;
                }
            }
            if (e.LookupParameter("Pied en profondeur gauche").AsInteger() == vrai)
            {
                g.piedGaucheTransversalRadioButton.Checked = true;
                //Initialisation de longueur pied transversal gauche
                if (g.piedGaucheTransversalComboBox.Items.Contains(((e.LookupParameter("Longueur pied transversal gauche").AsDouble()) * 30.48).ToString()))
                {

                    g.piedGaucheTransversalComboBox.SelectedIndex = g.piedGaucheTransversalComboBox.FindStringExact
                        (((e.LookupParameter("Longueur pied transversal gauche").AsDouble()) * 30.48).ToString());
                }
                else
                {
                    g.piedGaucheTransversalComboBox.SelectedIndex = 0;
                }
            }
            #endregion Pied gauche

            #region pied droit
            //initialisation pied droit
            if (e.LookupParameter("Pied vertical droit").AsInteger() == vrai)
            {
                g.piedDroitVerticalRadioButton.Checked = true;
                g.rallongeDroit105CheckBox.Visible = true;
                g.rallongeDroit180CheckBox.Visible = true;
                //Initialisation de longueur pied vertical droit 
                //si la lg du pied n'est pas dans la liste des données acceptables, on positionne par défaut sur la plus petite longueur
                if (g.piedDroitVerticalComboBox.Items.Contains((GeneralClass.RetournePiedEnCM(e.LookupParameter("Longueur pied vertical droit").AsDouble())).ToString()))
                {
                    g.piedDroitVerticalComboBox.SelectedIndex = g.piedDroitVerticalComboBox.FindStringExact
                         (GeneralClass.RetournePiedEnCM(e.LookupParameter("Longueur pied vertical droit").AsDouble()).ToString());
                }
                else
                {
                    g.piedDroitVerticalComboBox.SelectedIndex = 0;
                }
                //initialisation rallonge de pied droit 105
                if (e.LookupParameter("Rallonge de pied droit").AsInteger() == vrai)
                {
                    g.rallongeDroit105CheckBox.Checked = true;
                    g.rallongeDroit180CheckBox.Checked = false;
                    g.rallongeDroitePictureBox.Visible = true;
                    g.rallongeDroiteComboBox.Visible = true;
                    g.rallonge180DroiteComboBox.Visible = false;
                    g.piedDroitVerticalComboBox.Visible = false;
                    try
                    {
                        g.rallongeDroiteComboBox.SelectedIndex = g.rallongeDroiteComboBox.FindStringExact(GeneralClass.RetournePiedEnCM(e.LookupParameter("Longueur pied vertical droit").AsDouble()).ToString());
                    }
                    catch { }
                }
                else
                {
                    if (e.LookupParameter("LG_MET_PTLG_Rallonge180D").AsInteger() == vrai)
                    {
                        g.rallongeDroit105CheckBox.Checked = false;
                        g.rallongeDroit180CheckBox.Checked = true;
                        g.rallongeDroitePictureBox.Visible = true;
                        g.rallongeDroiteComboBox.Visible = false;
                        g.rallonge180DroiteComboBox.Visible = true;
                        g.piedDroitVerticalComboBox.Visible = false;
                        try
                        {
                            ;
                            g.rallonge180DroiteComboBox.SelectedIndex = g.rallonge180DroiteComboBox.FindStringExact(GeneralClass.RetournePiedEnCM(e.LookupParameter("Longueur pied vertical droit").AsDouble()).ToString());
                        }
                        catch { }
                    }
                    else
                    {
                        g.rallongeDroit105CheckBox.Checked = false;
                        g.rallongeDroit180CheckBox.Checked = false;
                        g.rallongeDroitePictureBox.Visible = false;
                        g.rallongeDroiteComboBox.Visible = false;
                        g.rallonge180DroiteComboBox.Visible = false;
                        g.piedDroitVerticalComboBox.Visible = true;
                    }
                }
            }
            else
            {
                g.rallongeDroit105CheckBox.Visible = false;
            }
            if (e.LookupParameter("Pied longitudinal droit").AsInteger() == vrai)
            {
                g.piedDroitLongitudinalRadioButton.Checked = true;
                //Initialisation de longueur pied longitudinal droit
                if (g.piedDroitLongitudinalComboBox.Items.Contains(e.LookupParameter("Longueur pied longitudinal droit").AsValueString()))
                {
                    g.piedDroitLongitudinalComboBox.SelectedIndex = g.piedDroitLongitudinalComboBox.FindStringExact
                        (e.LookupParameter("Longueur pied longitudinal droit").AsValueString());
                }
                else
                {
                    g.piedDroitLongitudinalComboBox.SelectedIndex = 0;
                }
            }
            if (e.LookupParameter("Pied en profondeur droit").AsInteger() == vrai)
            {
                g.piedDroitTransversalRadioButton.Checked = true;
                //Initialisation de longueur pied transversal droit
                if (g.piedDroitTransversalComboBox.Items.Contains(((e.LookupParameter("Longueur pied transversal droit").AsDouble()) * 30.48).ToString()))
                {

                    g.piedDroitTransversalComboBox.SelectedIndex = g.piedDroitTransversalComboBox.FindStringExact
                        (((e.LookupParameter("Longueur pied transversal droit").AsDouble()) * 30.48).ToString());
                }
                else
                {
                    g.piedDroitTransversalComboBox.SelectedIndex = 0;
                }
            }
            #endregion Pied droit

            #endregion initialisation des pieds

            #region Maj rallonge
            g.rallongeGauche105CheckBox.CheckedChanged += (sander, a) =>
            {
                if (g.rallongeGauche105CheckBox.Checked == true)
                {
                    g.rallongeGauche180CheckBox.Checked = false;
                    g.rallonge180GaucheComboBox.Visible = false;
                    g.rallongeGaucheComboBox.Visible = true;
                    g.rallongeGauchePictureBox.Visible = true;
                    g.piedGaucheVerticalComboBox.Visible = false;
                }
                else
                {
                    g.rallongeGaucheComboBox.Visible = false;
                    if (g.rallongeGauche180CheckBox.Checked == false)
                    {
                        g.rallongeGauchePictureBox.Visible = false;
                        g.piedGaucheVerticalComboBox.Visible = true;
                    }
                }
            };

            g.rallongeGauche180CheckBox.CheckedChanged += (sander, a) =>
            {
                if (g.rallongeGauche180CheckBox.Checked == true)
                {
                    g.rallongeGauche105CheckBox.Checked = false;
                    g.rallongeGaucheComboBox.Visible = false;
                    g.rallonge180GaucheComboBox.Visible = true;
                    g.rallongeGauchePictureBox.Visible = true;
                    g.piedGaucheVerticalComboBox.Visible = false;
                }
                else
                {
                    g.rallonge180GaucheComboBox.Visible = false;
                    if (g.rallongeGauche105CheckBox.Checked == false)
                    {
                        g.rallongeGauchePictureBox.Visible = false;
                        g.piedGaucheVerticalComboBox.Visible = true;
                    }
                }
            };

            g.rallongeDroit105CheckBox.CheckedChanged += (sander, a) =>
            {
                if (g.rallongeDroit105CheckBox.Checked == true)
                {
                    g.rallongeDroit180CheckBox.Checked = false;
                    g.rallonge180DroiteComboBox.Visible = false;
                    g.rallongeDroiteComboBox.Visible = true;
                    g.rallongeDroitePictureBox.Visible = true;
                    g.piedDroitVerticalComboBox.Visible = false;
                }
                else
                {
                    g.rallongeDroiteComboBox.Visible = false;
                    if (g.rallongeDroit180CheckBox.Checked == false)
                    {
                        g.rallongeDroitePictureBox.Visible = false;
                        g.piedDroitVerticalComboBox.Visible = true;
                    }
                }
            };

            g.rallongeDroit180CheckBox.CheckedChanged += (sander, a) =>
            {
                if (g.rallongeDroit180CheckBox.Checked == true)
                {
                    g.rallongeDroit105CheckBox.Checked = false;
                    g.rallongeDroiteComboBox.Visible = false;
                    g.rallonge180DroiteComboBox.Visible = true;
                    g.rallongeDroitePictureBox.Visible = true;
                    g.piedDroitVerticalComboBox.Visible = false;
                }
                else
                {
                    g.rallonge180DroiteComboBox.Visible = false;
                    if (g.rallongeDroit105CheckBox.Checked == false)
                    {
                        g.rallongeDroitePictureBox.Visible = false;
                        g.piedDroitVerticalComboBox.Visible = true;
                    }
                }
            };
            g.piedDroitVerticalRadioButton.CheckedChanged += (sander, a) =>
            {
                if (g.piedDroitVerticalRadioButton.Checked == false)
                {
                    g.piedDroitVerticalComboBox.Visible = false;
                    g.piedDroitVerticalComboBox.Visible = false;
                    g.rallongeDroit105CheckBox.Visible = false;
                    g.rallongeDroit105CheckBox.Checked = false;
                    g.rallonge180DroiteComboBox.Visible = false;
                    g.rallongeDroit180CheckBox.Checked = false;
                    g.rallongeDroit180CheckBox.Visible = false;
                }
                else
                {
                    if (g.rallongeDroit105CheckBox.Checked == false && g.rallongeDroit180CheckBox.Checked == false)
                    {
                        g.piedDroitVerticalComboBox.Visible = true;
                        g.piedDroitVerticalComboBox.Visible = true;
                    }
                    g.rallongeDroit105CheckBox.Visible = true;
                    g.rallongeDroit180CheckBox.Visible = true;
                    g.rallongeDroit180CheckBox.Checked = false;
                    g.rallongeDroit105CheckBox.Checked = false;
                }
            };
            g.piedGaucheVerticalRadioButton.CheckedChanged += (sander, a) =>
            {
                if (g.piedGaucheVerticalRadioButton.Checked == false)
                {
                    g.piedGaucheVerticalComboBox.Visible = false;
                    g.piedGaucheVerticalComboBox.Visible = false;
                    g.rallongeGauche105CheckBox.Visible = false;
                    g.rallongeGauche105CheckBox.Checked = false;
                    g.rallonge180GaucheComboBox.Visible = false;
                    g.rallongeGauche180CheckBox.Checked = false;
                    g.rallongeGauche180CheckBox.Visible = false;
                }
                else
                {
                    if (g.rallongeGauche105CheckBox.Checked == false && g.rallongeGauche180CheckBox.Checked == false)
                    {
                        g.piedGaucheVerticalComboBox.Visible = true;
                        g.piedGaucheVerticalComboBox.Visible = true;
                    }
                    g.rallongeGauche105CheckBox.Visible = true;
                    g.rallongeGauche180CheckBox.Visible = true;
                    g.rallongeGauche105CheckBox.Checked = false;
                    g.rallongeGauche180CheckBox.Checked = false;
                }
            };

            #endregion Maj rallonge

            #region applyFootButton
            g.applyFootButton.Click += (sander, a) =>
            {
                Transaction trPied = new Transaction(doc, "Configuration Pied");
                {
                    #region transaction pied
                    trPied.Start();
                    //réglage pied gauche
                    if (g.piedGaucheVerticalRadioButton.Checked == true)
                    {
                        e.LookupParameter("Pied vertical gauche").Set(vrai);
                        e.LookupParameter("Pied longitudinal gauche").Set(faux);
                        e.LookupParameter("Pied en profondeur gauche").Set(faux);
                        if (g.rallongeGauche105CheckBox.Checked == true)
                        {
                            e.LookupParameter("Rallonge de pied gauche").Set(vrai);
                            e.LookupParameter("Longueur pied vertical gauche").SetValueString(g.rallongeGaucheComboBox.SelectedItem.ToString());
                            e.LookupParameter("Bracon gauche").Set(vrai);
                            e.LookupParameter("LG_MET_PTLG_Rallonge180G").Set(faux);
                        }

                        else
                        {
                            if (g.rallongeGauche180CheckBox.Checked == true)
                            {
                                e.LookupParameter("Bracon gauche").Set(vrai);
                                e.LookupParameter("LG_MET_PTLG_Rallonge180G").Set(vrai);
                                e.LookupParameter("Longueur pied vertical gauche").SetValueString(g.rallonge180GaucheComboBox.SelectedItem.ToString());

                            }
                            else
                            {
                                e.LookupParameter("Longueur pied vertical gauche").SetValueString(g.piedGaucheVerticalComboBox.SelectedItem.ToString());

                                e.LookupParameter("LG_MET_PTLG_Rallonge180G").Set(faux);
                                e.LookupParameter("Bracon gauche").Set(faux);
                            }
                            e.LookupParameter("Rallonge de pied gauche").Set(faux);
                        }
                    }
                    else if (g.piedGaucheLongitudinalRadioButton.Checked == true)
                    {
                        e.LookupParameter("Pied longitudinal gauche").Set(vrai);
                        e.LookupParameter("Longueur pied longitudinal gauche").SetValueString(g.piedGaucheLongitudinalComboBox.SelectedItem.ToString());
                        e.LookupParameter("Pied vertical gauche").Set(faux);
                        e.LookupParameter("LG_MET_PTLG_Rallonge180G").Set(faux);
                        e.LookupParameter("Pied en profondeur gauche").Set(faux);
                        e.LookupParameter("Rallonge de pied gauche").Set(faux);
                        e.LookupParameter("Bracon gauche").Set(faux);
                    }
                    else if (g.piedGaucheTransversalRadioButton.Checked == true)
                    {
                        e.LookupParameter("Longueur pied transversal gauche").SetValueString(g.piedGaucheTransversalComboBox.SelectedItem.ToString());
                        e.LookupParameter("Pied en profondeur gauche").Set(vrai);
                        e.LookupParameter("Pied vertical gauche").Set(faux);
                        e.LookupParameter("Pied longitudinal gauche").Set(faux);
                        e.LookupParameter("Rallonge de pied gauche").Set(faux);
                        e.LookupParameter("LG_MET_PTLG_Rallonge180G").Set(faux);
                        e.LookupParameter("Bracon gauche").Set(faux);
                    }
                    else
                    {
                        e.LookupParameter("Pied vertical gauche").Set(vrai);
                        e.LookupParameter("Pied longitudinal gauche").Set(faux);
                        e.LookupParameter("Pied en profondeur gauche").Set(faux);
                        e.LookupParameter("Longueur pied vertical gauche").SetValueString("230");
                        e.LookupParameter("Rallonge de pied gauche").Set(faux);
                        e.LookupParameter("Bracon gauche").Set(faux);
                        e.LookupParameter("LG_MET_PTLG_Rallonge180G").Set(faux);
                    }
                    //réglage pied droit
                    if (g.piedDroitVerticalRadioButton.Checked == true)
                    {
                        e.LookupParameter("Pied vertical droit").Set(vrai);
                        e.LookupParameter("Pied longitudinal droit").Set(faux);
                        e.LookupParameter("Pied en profondeur droit").Set(faux);
                        if (g.rallongeDroit105CheckBox.Checked == true)
                        {
                            e.LookupParameter("Rallonge de pied droit").Set(vrai);
                            e.LookupParameter("LG_MET_PTLG_Rallonge180D").Set(faux);
                            e.LookupParameter("Bracon").Set(vrai);
                            e.LookupParameter("Longueur pied vertical droit").SetValueString(g.rallongeDroiteComboBox.SelectedItem.ToString());
                        }
                        else
                        {
                            if (g.rallongeDroit180CheckBox.Checked == true)
                            {
                                e.LookupParameter("Bracon").Set(vrai);
                                e.LookupParameter("LG_MET_PTLG_Rallonge180D").Set(vrai);
                                e.LookupParameter("Longueur pied vertical droit").SetValueString(g.rallonge180DroiteComboBox.SelectedItem.ToString());
                            }
                            else
                            {
                                e.LookupParameter("LG_MET_PTLG_Rallonge180D").Set(faux);
                                e.LookupParameter("Bracon").Set(faux);
                                e.LookupParameter("Longueur pied vertical droit").SetValueString(g.piedDroitVerticalComboBox.SelectedItem.ToString());
                            }
                            e.LookupParameter("Rallonge de pied droit").Set(faux);
                        }
                    }
                    else if (g.piedDroitLongitudinalRadioButton.Checked == true)
                    {
                        e.LookupParameter("Longueur pied longitudinal droit").SetValueString(g.piedDroitLongitudinalComboBox.SelectedItem.ToString());
                        e.LookupParameter("Pied longitudinal droit").Set(vrai);
                        e.LookupParameter("Pied vertical droit").Set(faux);
                        e.LookupParameter("Pied en profondeur droit").Set(faux);
                        e.LookupParameter("Rallonge de pied droit").Set(faux);
                        e.LookupParameter("LG_MET_PTLG_Rallonge180D").Set(faux);
                        e.LookupParameter("Bracon").Set(faux);
                    }
                    else if (g.piedDroitTransversalRadioButton.Checked == true)
                    {
                        e.LookupParameter("Longueur pied transversal droit").SetValueString(g.piedDroitTransversalComboBox.SelectedItem.ToString());
                        e.LookupParameter("Pied en profondeur droit").Set(vrai);
                        e.LookupParameter("Pied vertical droit").Set(faux);
                        e.LookupParameter("Pied longitudinal droit").Set(faux);
                        e.LookupParameter("Rallonge de pied droit").Set(faux);
                        e.LookupParameter("LG_MET_PTLG_Rallonge180D").Set(faux);
                        e.LookupParameter("Bracon").Set(faux);
                    }
                    else
                    {
                        e.LookupParameter("Longueur pied vertical droit").SetValueString("230");
                        e.LookupParameter("Pied vertical droit").Set(vrai);
                        e.LookupParameter("Pied longitudinal droit").Set(faux);
                        e.LookupParameter("Pied en profondeur droit").Set(faux);
                        e.LookupParameter("Rallonge de pied droit").Set(faux);
                        e.LookupParameter("LG_MET_PTLG_Rallonge180D").Set(faux);
                        e.LookupParameter("Bracon").Set(faux);
                    }
                    trPied.Commit();
                    #endregion transaction pied
                };//transaction
            };//applyFootButton
            #endregion ApplyFootButton
        }

        private static void Options(optionsForm op, Element e, Document doc)
        {
            #region Initialisation

            #region  Maximum des scrollbar retrait auvent en fonction de la longueur de la passerelle
            op.RetraitDScrollbar.Maximum = Convert.ToInt32(GeneralClass.RetournePiedEnCM(e.LookupParameter("Longueur extension droite").AsDouble()));
            op.RetraitGScrollbar.Maximum = Convert.ToInt32(GeneralClass.RetournePiedEnCM(e.LookupParameter("Longueur extension gauche").AsDouble()));
            #endregion

            #region Retrait auvent gauche
            try
            {
                op.RetraitGScrollbar.Value = Convert.ToInt32(GeneralClass.RetournePiedEnCM(e.LookupParameter("Retrait gauche").AsDouble()));
                op.RetraitGTextbox.Text = op.RetraitGScrollbar.Value.ToString();
            }
            catch
            {
                TaskDialog.Show("Erreur", " La valeur de retrait du auvent gauche est incorrect, la valeur va passer à 0 par défaut");
                op.RetraitGScrollbar.Value = 0;
                op.RetraitGTextbox.Text = op.RetraitGScrollbar.Value.ToString();
            }
            #endregion

            #region Retrait auvent droite
            try
            {
                op.RetraitDScrollbar.Value = Convert.ToInt32(GeneralClass.RetournePiedEnCM(e.LookupParameter("Retrait droit").AsDouble()));
                op.RetraitDTextbox.Text = op.RetraitDScrollbar.Value.ToString();
            }
            catch
            {
                TaskDialog.Show("Erreur", " La valeur de retrait du auvent droit est incorrect, la valeur va passer à 0 par défaut");
                op.RetraitGScrollbar.Value = 0;
                op.RetraitDTextbox.Text = op.RetraitGScrollbar.Value.ToString();
            }
            #endregion

            #region Retrait GC Gauche
            try
            {
                op.GCGScrollBar.Value = Convert.ToInt32(GeneralClass.RetournePiedEnCM(e.LookupParameter("Retrait GC gauche").AsDouble()));
                op.GCGtextBox.Text = op.GCGScrollBar.Value.ToString();
            }
            catch
            {
                TaskDialog.Show("Erreur", " La valeur de retrait du GC gauche est incorrect, la valeur va passer à 0 par défaut");
                op.GCGScrollBar.Value = 0;
                op.GCGtextBox.Text = op.GCGScrollBar.Value.ToString();
            }
            #endregion

            #region Retrait GC droite
            try
            {
                op.GCDScrollBar.Value = Convert.ToInt32(GeneralClass.RetournePiedEnCM(e.LookupParameter("Retrait GC droit").AsDouble()));
                op.GCDtextBox.Text = op.GCDScrollBar.Value.ToString();
            }
            catch
            {
                TaskDialog.Show("Erreur", " La valeur de retrait du GC droit est incorrect, la valeur va passer à 0 par défaut");
                op.GCDScrollBar.Value = 0;
                op.GCDtextBox.Text = op.GCDScrollBar.Value.ToString();
            }
            #endregion

            #region Decoupe Gauche
            op.DecoupeGDisttextBox.Text = GeneralClass.RetournePiedEnCM(e.LookupParameter("LG_MET_PTLG_Decoupe 1 distance au bord").AsDouble()).ToString();
            op.DecoupeGLrgtextBox.Text = GeneralClass.RetournePiedEnCM(e.LookupParameter("LG_MET_PTLG_Decoupe 1 Largeur").AsDouble()).ToString();
            op.DecoupeGProftextBox.Text = GeneralClass.RetournePiedEnCM(e.LookupParameter("LG_MET_PTLG_Decoupe 1 profondeur").AsDouble()).ToString();

            if (e.LookupParameter("LG_MET_PTLG_Decoupe gauche").AsInteger() == 1)
            {
                op.DecoupeGcheckBox.Checked = true;
                op.DecoupeGDisttextBox.Enabled = true;
                op.DecoupeGLrgtextBox.Enabled = true;
                op.DecoupeGProftextBox.Enabled = true;
            }
            else
            {
                op.DecoupeGcheckBox.Checked = false;
                op.DecoupeGDisttextBox.Enabled = false;
                op.DecoupeGLrgtextBox.Enabled = false;
                op.DecoupeGProftextBox.Enabled = false;
            }
            #endregion

            #region Decoupe droite
            op.DecoupeDDisttextBox.Text = GeneralClass.RetournePiedEnCM(e.LookupParameter("LG_MET_PTLG_Decoupe 2 distance au bord").AsDouble()).ToString();
            op.DecoupeDLrgtextBox.Text = GeneralClass.RetournePiedEnCM(e.LookupParameter("LG_MET_PTLG_Decoupe 2 Largeur").AsDouble()).ToString();
            op.DecoupeDProftextBox.Text = GeneralClass.RetournePiedEnCM(e.LookupParameter("LG_MET_PTLG_Decoupe 2 Profondeur").AsDouble()).ToString();
            if (e.LookupParameter("LG_MET_PTLG_Decoupe droite").AsInteger() == 1)
            {
                op.DecoupeDcheckBox.Checked = true;
                op.DecoupeDDisttextBox.Enabled = true;
                op.DecoupeDLrgtextBox.Enabled = true;
                op.DecoupeDProftextBox.Enabled = true;
            }
            else
            {
                op.DecoupeDcheckBox.Checked = false;
                op.DecoupeDDisttextBox.Enabled = false;
                op.DecoupeDLrgtextBox.Enabled = false;
                op.DecoupeDProftextBox.Enabled = false;
            }
            #endregion

            #region Comportement Checkbox decoupe gauche
            op.DecoupeGcheckBox.CheckedChanged += (sander, a) =>
            {
                if (op.DecoupeGcheckBox.Checked == true)
                {
                    op.DecoupeGDisttextBox.Enabled = true;
                    op.DecoupeGLrgtextBox.Enabled = true;
                    op.DecoupeGProftextBox.Enabled = true;
                }
                else
                {
                    op.DecoupeGDisttextBox.Enabled = false;
                    op.DecoupeGLrgtextBox.Enabled = false;
                    op.DecoupeGProftextBox.Enabled = false;
                }
            };
            #endregion

            #region Comportement CheckBox decoupe droite
            op.DecoupeDcheckBox.CheckedChanged += (sander, a) =>
            {
                if (op.DecoupeDcheckBox.Checked == true)
                {
                    op.DecoupeDDisttextBox.Enabled = true;
                    op.DecoupeDLrgtextBox.Enabled = true;
                    op.DecoupeDProftextBox.Enabled = true;
                }
                else
                {
                    op.DecoupeDDisttextBox.Enabled = false;
                    op.DecoupeDLrgtextBox.Enabled = false;
                    op.DecoupeDProftextBox.Enabled = false;
                }
            };
            #endregion

            #region U Bas
            if (e.GetParameters("U bas droit")[0].AsInteger() == 1 ||
            e.GetParameters("U bas gauche")[0].AsInteger() == 1)
                op.uBasCheckBox.Checked = true;
            else
                op.uBasCheckBox.Checked = false;
            #endregion

            #region Bracon
            if (
                e.GetParameters("Bracon")[0].AsInteger() == 1 ||
                e.GetParameters("Bracon gauche")[0].AsInteger() == 1
                )
                op.braconCheckBox.Checked = true;

            else
                op.braconCheckBox.Checked = false;
            #endregion

            #region Comportement des Scrollbars
            op.RetraitDScrollbar.Scroll += (sander, a) =>
            {
                op.RetraitDTextbox.Text = op.RetraitDScrollbar.Value.ToString();
            };
            op.RetraitGScrollbar.Scroll += (sander, a) =>
            {
                op.RetraitGTextbox.Text = op.RetraitGScrollbar.Value.ToString();
            };
            op.RetraitGTextbox.TextChanged += (sander, a) =>
            {
                op.RetraitGScrollbar.Value = Convert.ToInt32(op.RetraitGTextbox.Text);
            };
            op.RetraitDTextbox.TextChanged += (sander, a) =>
            {
                op.RetraitDScrollbar.Value = Convert.ToInt32(op.RetraitDTextbox.Text);
            };

            op.GCDScrollBar.Scroll += (sander, a) =>
            {
                op.GCDtextBox.Text = op.GCDScrollBar.Value.ToString();
            };
            op.GCGScrollBar.Scroll += (sander, a) =>
            {
                op.GCGtextBox.Text = op.GCGScrollBar.Value.ToString();
            };
            op.GCGtextBox.TextChanged += (sander, a) =>
            {
                op.GCGScrollBar.Value = Convert.ToInt32(op.GCGtextBox.Text);
            };
            op.GCDtextBox.TextChanged += (sander, a) =>
            {
                op.GCDScrollBar.Value = Convert.ToInt32(op.GCDtextBox.Text);
            };
            #endregion Cmportement des scrollbar

            #endregion Initialisation

            #region Apply Button Option
            op.applyOptionsButton.Click += (sander, a) =>
            {

                Transaction trOptions = new Transaction(doc, "Configuration options");
                trOptions.Start();
                e.LookupParameter("Retrait gauche").Set(GeneralClass.RetourneCmEnPieds(op.RetraitGScrollbar.Value));
                e.LookupParameter("Retrait droit").Set(GeneralClass.RetourneCmEnPieds(op.RetraitDScrollbar.Value));
                e.LookupParameter("Retrait GC gauche").Set(GeneralClass.RetourneCmEnPieds(op.GCGScrollBar.Value));
                e.LookupParameter("Retrait GC droit").Set(GeneralClass.RetourneCmEnPieds(op.GCDScrollBar.Value));


                if (op.uBasCheckBox.Checked == true)
                {
                    e.LookupParameter("U bas droit").Set(1);
                    e.LookupParameter("U bas gauche").Set(1);
                }
                else
                {
                    e.LookupParameter("U bas droit").Set(0);
                    e.LookupParameter("U bas gauche").Set(0);
                }

                if (op.braconCheckBox.Checked == true)
                {
                    e.LookupParameter("Bracon").Set(1);
                    e.LookupParameter("Bracon gauche").Set(1);
                }
                else
                {
                    e.LookupParameter("Bracon").Set(0);
                    e.LookupParameter("Bracon gauche").Set(0);
                }

                if (op.DecoupeGcheckBox.Checked == true)
                {
                    try
                    {
                        e.LookupParameter("LG_MET_PTLG_Decoupe gauche").Set(vrai);
                        e.LookupParameter("LG_MET_PTLG_Decoupe 1 distance au bord").Set(GeneralClass.RetourneCmEnPieds(Convert.ToDouble(op.DecoupeGDisttextBox.Text)));
                        e.LookupParameter("LG_MET_PTLG_Decoupe 1 Largeur").Set(GeneralClass.RetourneCmEnPieds(Convert.ToDouble(op.DecoupeGLrgtextBox.Text)));
                        e.LookupParameter("LG_MET_PTLG_Decoupe 1 profondeur").Set(GeneralClass.RetourneCmEnPieds(Convert.ToDouble(op.DecoupeGProftextBox.Text)));
                    }
                    catch
                    {
                        TaskDialog.Show("Erreur", "Une valeur nom numérique a été saisie pour la découpe gauche, la modification ne sera pas prise en compte");
                    }
                }
                else
                {
                    e.LookupParameter("LG_MET_PTLG_Decoupe gauche").Set(faux);
                }

                if (op.DecoupeDcheckBox.Checked == true)
                {
                    try
                    {
                        e.LookupParameter("LG_MET_PTLG_Decoupe droite").Set(vrai);
                        e.LookupParameter("LG_MET_PTLG_Decoupe 2 distance au bord").Set(GeneralClass.RetourneCmEnPieds(Convert.ToDouble(op.DecoupeDDisttextBox.Text)));
                        e.LookupParameter("LG_MET_PTLG_Decoupe 2 Largeur").Set(GeneralClass.RetourneCmEnPieds(Convert.ToDouble(op.DecoupeDLrgtextBox.Text)));
                        e.LookupParameter("LG_MET_PTLG_Decoupe 2 Profondeur").Set(GeneralClass.RetourneCmEnPieds(Convert.ToDouble(op.DecoupeDProftextBox.Text)));
                    }
                    catch
                    {
                        TaskDialog.Show("Erreur", "Une valeur nom numérique a été saisie pour la découpe gauche, la modification ne sera pas prise en compte");
                    }
                }
                else
                {
                    e.LookupParameter("LG_MET_PTLG_Decoupe droite").Set(faux);
                }

                trOptions.Commit();
            };
            #endregion Apply Button
        }
    }
}
