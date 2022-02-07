using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit;
using Autodesk.Revit.Attributes;

namespace PTLGClassLibrary
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(RegenerationOption.Manual)]
    public class PiedClass : IExternalCommand
    {
        public Result Execute(
           ExternalCommandData commandData,
           ref string message,
           ElementSet elements)
        {
            #region parametres pour code
            //Get application and document objects
            UIApplication uiApp = commandData.Application;
            Document doc = uiApp.ActiveUIDocument.Document;

            //variables definies pour tout le code
            int vrai = 1;
            int faux = 0;
            #endregion parametres pour code
            #region liste des passerelles
            //creation de la liste des id des passerelles et la liste des passerelles
            List<ElementId> selectionPasserelleIdList = uiApp.ActiveUIDocument.Selection.GetElementIds().ToList();
            List<Element> selectionPasserelleList = new List<Element>();

            //remplissage de la liste des passerelles
            foreach (ElementId eid in selectionPasserelleIdList)
            {
                selectionPasserelleList.Add(doc.GetElement(eid));
            }
            #endregion liste des passerelles
            #region Erreur : plusieurs objets selectionnes
            {               
                // Si il y a plus d'un objet selectionne : erreur
                if (selectionPasserelleIdList.Count == 0)
                    TaskDialog.Show("ERREUR", "Selectionner une passerelle");

                if (selectionPasserelleIdList.Count > 1)
                    TaskDialog.Show("ERREUR", "Selectionner une seule passerelle");
            }
            #endregion erreur plusieurs objets selectionnes

            if (selectionPasserelleIdList.Count == 1)
            {
                Element passerelleSelectionneeElement = selectionPasserelleList[0];
                #region Erreur : objet selectionné n'est pas une passerelle
                // Si le nom de l'objet selectionné ne contient pas LG_M
                if (!(passerelleSelectionneeElement.Name.Contains("M1")
                    || passerelleSelectionneeElement.Name.Contains("M2")
                    || passerelleSelectionneeElement.Name.Contains("M3")))
                {
                    TaskDialog.Show("Erreur", "L'objet selectionné n'est pas une passerelle");
                }


                #endregion
                else
                #region on commence la configuration
                {
                    

                    #region liste de tous les parametres de la passerelle selectionnee
                        List<Autodesk.Revit.DB.Parameter> parameterList = new List<Autodesk.Revit.DB.Parameter>();
                    foreach (Autodesk.Revit.DB.Parameter para in passerelleSelectionneeElement.Parameters)
                    {
                        //On crée la liste de tous les parametres de la passerelle selectionnee
                        parameterList.Add(para);
                    }
                    #endregion liste de tous les parametres de la passerelle selectionnee

                    using (PiedForm g = new PiedForm())
                    {

                        //Modif des possibilités du pied longitudinal pour la M1
                        if (passerelleSelectionneeElement.Name.Contains("LG_M1"))
                        {
                            for (int i = 0; i < 21; i++)
                            {
                                g.piedDroitLongitudinalComboBox.Items.RemoveAt(0);
                                g.piedGaucheLongitudinalComboBox.Items.RemoveAt(0);

                            }
                        }

                        #region copier
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

                        if (passerelleSelectionneeElement.LookupParameter("Pied vertical gauche").AsInteger() == vrai)
                        {
                            g.piedGaucheVerticalRadioButton.Checked = true;
                            g.rallongeGauche105CheckBox.Visible = true;
                            g.rallongeGauche180CheckBox.Visible = true;
                            //Initialisation de longueur pied vertical gauche 
                            //si la lg du pied n'est pas dans la liste des données acceptables, on positionne par défaut sur la plus petite longueur
                            if (g.piedGaucheVerticalComboBox.Items.Contains((GeneralClass.RetournePiedEnCM((passerelleSelectionneeElement.LookupParameter("Longueur pied vertical gauche").AsDouble()))).ToString()))
                            {
                                g.piedGaucheVerticalComboBox.SelectedIndex = g.piedGaucheVerticalComboBox.FindStringExact
                                     ((GeneralClass.RetournePiedEnCM((passerelleSelectionneeElement.LookupParameter("Longueur pied vertical gauche").AsDouble()))).ToString());
                            }
                            else
                            {
                                g.piedGaucheVerticalComboBox.SelectedIndex = 0;
                            }
                            //initialisation rallonge de pied gauche 105
                            if (passerelleSelectionneeElement.LookupParameter("Rallonge de pied gauche").AsInteger() == vrai)
                            {
                                g.rallongeGauche105CheckBox.Checked = true;
                                g.rallongeGauche180CheckBox.Checked = false;
                                g.rallongeGauchePictureBox.Visible = true;
                                g.rallongeGaucheComboBox.Visible = true;
                                g.rallonge180GaucheComboBox.Visible = false;
                                g.piedGaucheVerticalComboBox.Visible = false;
                                try
                                {
                                    g.rallongeGaucheComboBox.SelectedIndex = g.rallongeGaucheComboBox.FindStringExact(GeneralClass.RetournePiedEnCM((passerelleSelectionneeElement.LookupParameter("Longueur pied vertical gauche").AsDouble())).ToString());
                                }
                                catch { }
                            }
                            else
                            {
                                //initialisation rallonge de pied gauche 180
                                if (passerelleSelectionneeElement.LookupParameter("LG_MET_PTLG_Rallonge180G").AsInteger() == vrai)
                                {
                                    g.rallongeGauche105CheckBox.Checked = false;
                                    g.rallongeGauche180CheckBox.Checked = true;
                                    g.rallongeGauchePictureBox.Visible = true;
                                    g.rallongeGaucheComboBox.Visible = false;
                                    g.rallonge180GaucheComboBox.Visible = true;
                                    g.piedGaucheVerticalComboBox.Visible = false;
                                    try
                                    {
                                        g.rallonge180GaucheComboBox.SelectedIndex = g.rallonge180GaucheComboBox.FindStringExact((GeneralClass.RetournePiedEnCM(passerelleSelectionneeElement.LookupParameter("Longueur pied vertical gauche").AsDouble())).ToString());
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
                        if (passerelleSelectionneeElement.LookupParameter("Pied longitudinal gauche").AsInteger() == vrai)
                        {
                            g.piedGaucheLongitudinalRadioButton.Checked = true;
                            //Initialisation de longueur pied longitudinal gauche
                            if (g.piedGaucheLongitudinalComboBox.Items.Contains(passerelleSelectionneeElement.LookupParameter("Longueur pied longitudinal gauche").AsValueString()))
                            {
                                g.piedGaucheLongitudinalComboBox.SelectedIndex = g.piedGaucheLongitudinalComboBox.FindStringExact
                                    (passerelleSelectionneeElement.LookupParameter("Longueur pied longitudinal gauche").AsValueString());
                            }
                            else
                            {
                                g.piedGaucheLongitudinalComboBox.SelectedIndex = 0;
                            }
                        }
                        if (passerelleSelectionneeElement.LookupParameter("Pied en profondeur gauche").AsInteger() == vrai)
                        {
                            g.piedGaucheTransversalRadioButton.Checked = true;
                            //Initialisation de longueur pied transversal gauche
                            if (g.piedGaucheTransversalComboBox.Items.Contains(((passerelleSelectionneeElement.LookupParameter("Longueur pied transversal gauche").AsDouble()) * 30.48).ToString()))
                            {

                                g.piedGaucheTransversalComboBox.SelectedIndex = g.piedGaucheTransversalComboBox.FindStringExact
                                    (((passerelleSelectionneeElement.LookupParameter("Longueur pied transversal gauche").AsDouble()) * 30.48).ToString());
                            }
                            else
                            {
                                g.piedGaucheTransversalComboBox.SelectedIndex = 0;
                            }
                        }
                        #endregion Pied gauche

                        #region pied droit
                        //initialisation pied droit
                        if (passerelleSelectionneeElement.LookupParameter("Pied vertical droit").AsInteger() == vrai)
                        {
                            g.piedDroitVerticalRadioButton.Checked = true;
                            g.rallongeDroit105CheckBox.Visible = true;
                            g.rallongeDroit180CheckBox.Visible = true;
                            //Initialisation de longueur pied vertical droit 
                            //si la lg du pied n'est pas dans la liste des données acceptables, on positionne par défaut sur la plus petite longueur
                            if (g.piedDroitVerticalComboBox.Items.Contains((GeneralClass.RetournePiedEnCM(passerelleSelectionneeElement.LookupParameter("Longueur pied vertical droit").AsDouble())).ToString()))
                            {
                                g.piedDroitVerticalComboBox.SelectedIndex = g.piedDroitVerticalComboBox.FindStringExact
                                     (GeneralClass.RetournePiedEnCM(passerelleSelectionneeElement.LookupParameter("Longueur pied vertical droit").AsDouble()).ToString());
                            }
                            else
                            {
                                g.piedDroitVerticalComboBox.SelectedIndex = 0;
                            }
                            //initialisation rallonge de pied droit 105
                            if (passerelleSelectionneeElement.LookupParameter("Rallonge de pied droit").AsInteger() == vrai)
                            {
                                g.rallongeDroit105CheckBox.Checked = true;
                                g.rallongeDroit180CheckBox.Checked = false;
                                g.rallongeDroitePictureBox.Visible = true;
                                g.rallongeDroiteComboBox.Visible = true;
                                g.rallonge180DroiteComboBox.Visible = false;
                                g.piedDroitVerticalComboBox.Visible = false;
                                try
                                {
                                    g.rallongeDroiteComboBox.SelectedIndex = g.rallongeDroiteComboBox.FindStringExact(GeneralClass.RetournePiedEnCM(passerelleSelectionneeElement.LookupParameter("Longueur pied vertical droit").AsDouble()).ToString());
                                }
                                catch { }
                            }
                            else
                            {
                                if (passerelleSelectionneeElement.LookupParameter("LG_MET_PTLG_Rallonge180D").AsInteger() == vrai)
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
                                        g.rallonge180DroiteComboBox.SelectedIndex = g.rallonge180DroiteComboBox.FindStringExact(GeneralClass.RetournePiedEnCM(passerelleSelectionneeElement.LookupParameter("Longueur pied vertical droit").AsDouble()).ToString());
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
                        if (passerelleSelectionneeElement.LookupParameter("Pied longitudinal droit").AsInteger() == vrai)
                        {
                            g.piedDroitLongitudinalRadioButton.Checked = true;
                            //Initialisation de longueur pied longitudinal droit
                            if (g.piedDroitLongitudinalComboBox.Items.Contains(passerelleSelectionneeElement.LookupParameter("Longueur pied longitudinal droit").AsValueString()))
                            {
                                g.piedDroitLongitudinalComboBox.SelectedIndex = g.piedDroitLongitudinalComboBox.FindStringExact
                                    (passerelleSelectionneeElement.LookupParameter("Longueur pied longitudinal droit").AsValueString());
                            }
                            else
                            {
                                g.piedDroitLongitudinalComboBox.SelectedIndex = 0;
                            }
                        }
                        if (passerelleSelectionneeElement.LookupParameter("Pied en profondeur droit").AsInteger() == vrai)
                        {
                            g.piedDroitTransversalRadioButton.Checked = true;
                            //Initialisation de longueur pied transversal droit
                            if (g.piedDroitTransversalComboBox.Items.Contains(((passerelleSelectionneeElement.LookupParameter("Longueur pied transversal droit").AsDouble()) * 30.48).ToString()))
                            {

                                g.piedDroitTransversalComboBox.SelectedIndex = g.piedDroitTransversalComboBox.FindStringExact
                                    (((passerelleSelectionneeElement.LookupParameter("Longueur pied transversal droit").AsDouble()) * 30.48).ToString());
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
                                    passerelleSelectionneeElement.LookupParameter("Pied vertical gauche").Set(vrai);
                                    passerelleSelectionneeElement.LookupParameter("Pied longitudinal gauche").Set(faux);
                                    passerelleSelectionneeElement.LookupParameter("Pied en profondeur gauche").Set(faux);
                                    if (g.rallongeGauche105CheckBox.Checked == true)
                                    {
                                        passerelleSelectionneeElement.LookupParameter("Rallonge de pied gauche").Set(vrai);
                                        passerelleSelectionneeElement.LookupParameter("Longueur pied vertical gauche").SetValueString(g.rallongeGaucheComboBox.SelectedItem.ToString());
                                        passerelleSelectionneeElement.LookupParameter("Bracon gauche").Set(vrai);
                                        passerelleSelectionneeElement.LookupParameter("LG_MET_PTLG_Rallonge180G").Set(faux);
                                    }

                                    else
                                    {
                                        if (g.rallongeGauche180CheckBox.Checked == true)
                                        {
                                            passerelleSelectionneeElement.LookupParameter("Bracon gauche").Set(vrai);
                                            passerelleSelectionneeElement.LookupParameter("LG_MET_PTLG_Rallonge180G").Set(vrai);
                                            passerelleSelectionneeElement.LookupParameter("Longueur pied vertical gauche").SetValueString(g.rallonge180GaucheComboBox.SelectedItem.ToString());

                                        }
                                        else
                                        {
                                            passerelleSelectionneeElement.LookupParameter("Longueur pied vertical gauche").SetValueString(g.piedGaucheVerticalComboBox.SelectedItem.ToString());

                                            passerelleSelectionneeElement.LookupParameter("LG_MET_PTLG_Rallonge180G").Set(faux);
                                            passerelleSelectionneeElement.LookupParameter("Bracon gauche").Set(faux);
                                        }
                                        passerelleSelectionneeElement.LookupParameter("Rallonge de pied gauche").Set(faux);
                                    }
                                }
                                else if (g.piedGaucheLongitudinalRadioButton.Checked == true)
                                {
                                    passerelleSelectionneeElement.LookupParameter("Pied longitudinal gauche").Set(vrai);
                                    passerelleSelectionneeElement.LookupParameter("Longueur pied longitudinal gauche").SetValueString(g.piedGaucheLongitudinalComboBox.SelectedItem.ToString());
                                    passerelleSelectionneeElement.LookupParameter("Pied vertical gauche").Set(faux);
                                    passerelleSelectionneeElement.LookupParameter("LG_MET_PTLG_Rallonge180G").Set(faux);
                                    passerelleSelectionneeElement.LookupParameter("Pied en profondeur gauche").Set(faux);
                                    passerelleSelectionneeElement.LookupParameter("Rallonge de pied gauche").Set(faux);
                                    passerelleSelectionneeElement.LookupParameter("Bracon gauche").Set(faux);
                                }
                                else if (g.piedGaucheTransversalRadioButton.Checked == true)
                                {
                                    passerelleSelectionneeElement.LookupParameter("Longueur pied transversal gauche").SetValueString(g.piedGaucheTransversalComboBox.SelectedItem.ToString());
                                    passerelleSelectionneeElement.LookupParameter("Pied en profondeur gauche").Set(vrai);
                                    passerelleSelectionneeElement.LookupParameter("Pied vertical gauche").Set(faux);
                                    passerelleSelectionneeElement.LookupParameter("Pied longitudinal gauche").Set(faux);
                                    passerelleSelectionneeElement.LookupParameter("Rallonge de pied gauche").Set(faux);
                                    passerelleSelectionneeElement.LookupParameter("LG_MET_PTLG_Rallonge180G").Set(faux);
                                    passerelleSelectionneeElement.LookupParameter("Bracon gauche").Set(faux);
                                }
                                else
                                {
                                    passerelleSelectionneeElement.LookupParameter("Pied vertical gauche").Set(vrai);
                                    passerelleSelectionneeElement.LookupParameter("Pied longitudinal gauche").Set(faux);
                                    passerelleSelectionneeElement.LookupParameter("Pied en profondeur gauche").Set(faux);
                                    passerelleSelectionneeElement.LookupParameter("Longueur pied vertical gauche").SetValueString("230");
                                    passerelleSelectionneeElement.LookupParameter("Rallonge de pied gauche").Set(faux);
                                    passerelleSelectionneeElement.LookupParameter("Bracon gauche").Set(faux);
                                    passerelleSelectionneeElement.LookupParameter("LG_MET_PTLG_Rallonge180G").Set(faux);
                                }
                                //réglage pied droit
                                if (g.piedDroitVerticalRadioButton.Checked == true)
                                {
                                    passerelleSelectionneeElement.LookupParameter("Pied vertical droit").Set(vrai);
                                    passerelleSelectionneeElement.LookupParameter("Pied longitudinal droit").Set(faux);
                                    passerelleSelectionneeElement.LookupParameter("Pied en profondeur droit").Set(faux);
                                    if (g.rallongeDroit105CheckBox.Checked == true)
                                    {
                                        passerelleSelectionneeElement.LookupParameter("Rallonge de pied droit").Set(vrai);
                                        passerelleSelectionneeElement.LookupParameter("LG_MET_PTLG_Rallonge180D").Set(faux);
                                        passerelleSelectionneeElement.LookupParameter("Bracon").Set(vrai);
                                        passerelleSelectionneeElement.LookupParameter("Longueur pied vertical droit").SetValueString(g.rallongeDroiteComboBox.SelectedItem.ToString());
                                    }
                                    else
                                    {
                                        if (g.rallongeDroit180CheckBox.Checked == true)
                                        {
                                            passerelleSelectionneeElement.LookupParameter("Bracon").Set(vrai);
                                            passerelleSelectionneeElement.LookupParameter("LG_MET_PTLG_Rallonge180D").Set(vrai);
                                            passerelleSelectionneeElement.LookupParameter("Longueur pied vertical droit").SetValueString(g.rallonge180DroiteComboBox.SelectedItem.ToString());
                                        }
                                        else
                                        {
                                            passerelleSelectionneeElement.LookupParameter("LG_MET_PTLG_Rallonge180D").Set(faux);
                                            passerelleSelectionneeElement.LookupParameter("Bracon").Set(faux);
                                            passerelleSelectionneeElement.LookupParameter("Longueur pied vertical droit").SetValueString(g.piedDroitVerticalComboBox.SelectedItem.ToString());
                                        }
                                        passerelleSelectionneeElement.LookupParameter("Rallonge de pied droit").Set(faux);
                                    }
                                }
                                else if (g.piedDroitLongitudinalRadioButton.Checked == true)
                                {
                                    passerelleSelectionneeElement.LookupParameter("Longueur pied longitudinal droit").SetValueString(g.piedDroitLongitudinalComboBox.SelectedItem.ToString());
                                    passerelleSelectionneeElement.LookupParameter("Pied longitudinal droit").Set(vrai);
                                    passerelleSelectionneeElement.LookupParameter("Pied vertical droit").Set(faux);
                                    passerelleSelectionneeElement.LookupParameter("Pied en profondeur droit").Set(faux);
                                    passerelleSelectionneeElement.LookupParameter("Rallonge de pied droit").Set(faux);
                                    passerelleSelectionneeElement.LookupParameter("LG_MET_PTLG_Rallonge180D").Set(faux);
                                    passerelleSelectionneeElement.LookupParameter("Bracon").Set(faux);
                                }
                                else if (g.piedDroitTransversalRadioButton.Checked == true)
                                {
                                    passerelleSelectionneeElement.LookupParameter("Longueur pied transversal droit").SetValueString(g.piedDroitTransversalComboBox.SelectedItem.ToString());
                                    passerelleSelectionneeElement.LookupParameter("Pied en profondeur droit").Set(vrai);
                                    passerelleSelectionneeElement.LookupParameter("Pied vertical droit").Set(faux);
                                    passerelleSelectionneeElement.LookupParameter("Pied longitudinal droit").Set(faux);
                                    passerelleSelectionneeElement.LookupParameter("Rallonge de pied droit").Set(faux);
                                    passerelleSelectionneeElement.LookupParameter("LG_MET_PTLG_Rallonge180D").Set(faux);
                                    passerelleSelectionneeElement.LookupParameter("Bracon").Set(faux);
                                }
                                else
                                {
                                    passerelleSelectionneeElement.LookupParameter("Longueur pied vertical droit").SetValueString("230");
                                    passerelleSelectionneeElement.LookupParameter("Pied vertical droit").Set(vrai);
                                    passerelleSelectionneeElement.LookupParameter("Pied longitudinal droit").Set(faux);
                                    passerelleSelectionneeElement.LookupParameter("Pied en profondeur droit").Set(faux);
                                    passerelleSelectionneeElement.LookupParameter("Rallonge de pied droit").Set(faux);
                                    passerelleSelectionneeElement.LookupParameter("LG_MET_PTLG_Rallonge180D").Set(faux);
                                    passerelleSelectionneeElement.LookupParameter("Bracon").Set(faux);
                                }
                                trPied.Commit();
                                #endregion transaction pied
                            };//transaction
                        };//applyFootButton
                        #endregion ApplyFootButton
                        #endregion copier
                        g.ShowDialog();
                                        
                        };//using g

                   

                }//else 
                #endregion on commence la modelisation
            }//un seul objet selectionne
            return Result.Succeeded;
            }//elementSet
            }//public class
        }//namespace

