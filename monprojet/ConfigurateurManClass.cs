#region Autodesk PQ - IS04
using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using static PTLGClassLibrary.GeneralClass;

namespace PTLGClassLibrary
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class ConfigurateurManClass : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            #region Initialisation generale

            #region parametres pour code
            //Get application and document objects
            UIApplication uiApp = commandData.Application;
            UIDocument uidoc = uiApp.ActiveUIDocument;
            Document doc = uiApp.ActiveUIDocument.Document;
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
            // Si il y a plus d'un objet selectionne : en sélectionner un
            Element passerelleSelectionneeElement;

            if (selectionPasserelleIdList.Count != 1)
            {
                string n = "";
                if (selectionPasserelleIdList.Count > 1) n = " seule";
                TaskDialog.Show("ERREUR", "Sélectionner une" + n + " passerelle");

                passerelleSelectionneeElement = Helpers.Revit.PickWalkway(uidoc);
            }
            else
            {
                passerelleSelectionneeElement = selectionPasserelleList[0];
            }
            #endregion Erreur : plusieurs objets selectionnes

            #region Erreur : objet selectionné n'est pas une passerelle
            // Si le nom de l'objet selectionné ne contient pas LG_M
            if (!(passerelleSelectionneeElement.Name.Contains("M1")
                || passerelleSelectionneeElement.Name.Contains("M2")
                || passerelleSelectionneeElement.Name.Contains("M3")))
            {
                TaskDialog.Show("Erreur", "L'objet selectionné n'est pas une passerelle");
                return Result.Cancelled;
            }
            #endregion

            #region Association du type
            //string type = "";
            if (passerelleSelectionneeElement.Name.Contains("M1"))
                type = "M1";
            if (passerelleSelectionneeElement.Name.Contains("M2"))
                type = "M2";
            if (passerelleSelectionneeElement.Name.Contains("M3"))
                type = "M3";
            #endregion Association du type

            #region liste de tous les parametres de la passerelle selectionnee
            List<Parameter> parameterList = new List<Parameter>();
            foreach (Parameter para in passerelleSelectionneeElement.Parameters)
            {
                //On crée la liste de tous les parametres de la passerelle selectionnee
                parameterList.Add(para);
            }
            #endregion liste de tous les parametres de la passerelle selectionnee

            #region ListExtLaterale
            if (type == "M1")
            {
                for (int i = -1; i < 12; i++)
                    ListExtLaterale.Add((i + 4) * 5);
            }
            else if (type == "M2")
            {
                for (int i = 0; i < 16; i++)
                    ListExtLaterale.Add((i + 4) * 5);
            }
            else if (type == "M3")
            {
                for (int i = 0; i < 29; i++)
                    ListExtLaterale.Add((i + 4) * 5);
            }
            #endregion

            #endregion Initialisation generale


            #region Initialisation - GET DATA from Model

            Helpers.Revit.UpdateGeneralClassDataFromElem(passerelleSelectionneeElement);

            ConfigurateurManForm f = new ConfigurateurManForm();

            ConfMan.M_DataFromGUI.DataInitialisation(passerelleSelectionneeElement, f);
            ConfMan.M_DataFromGUI.MaxiValues(f);
            ConfMan.M_DataFromGUI.ThirdFourthSupports(f);
            ConfMan.M_LabelsBackColors.ColoringMaxValues(f);

            #endregion Initialisation - GET DATA from Model


            #region EVENT CHECKBOX
            f.eventcheckBoxext.CheckedChanged += (send, a) =>
            {
                ConfMan.M_DataFromGUI.MaxiValues(f);
                ConfMan.M_DataFromGUI.DataUpdate(f);
                ConfMan.M_DataFromGUI.ThirdFourthSupports(f);
                ConfMan.M_LabelsBackColors.ColoringMaxValues(f);

                ConfMan.M_DataFromGUI.TestIndexComboBox(f);
            };
            #endregion EVENT CHECKBOX


            #region Buttons appliquer, reglage pied, options, elevation
            
            #region applybutton
            f.applyButton.Click += (sender, e) =>
            {
                Transaction trButton = new Transaction(doc, "Configuration PTLG Manuel");
                {
                    trButton.Start();

                    //reglage numéro passerelle
                    passerelleSelectionneeElement.LookupParameter("Identifiant passerelle").Set(f.numeroPasserelleString);
                    #region banche
                    //reglage banche

                    foreach (string str in f.bancheComboBox.Items)
                    {
                        if (f.bancheComboBox.SelectedIndex == f.bancheComboBox.FindStringExact(str))
                            passerelleSelectionneeElement.LookupParameter(str).Set(vrai);
                        else passerelleSelectionneeElement.LookupParameter(str).Set(faux);
                    }

                    //stabilisation
                    foreach (string str in f.stabilisationComboBox.Items)
                    {
                        if (f.stabilisationComboBox.SelectedIndex == f.stabilisationComboBox.FindStringExact(str))
                            passerelleSelectionneeElement.LookupParameter(str).Set(vrai);
                        else passerelleSelectionneeElement.LookupParameter(str).Set(faux);
                    }
                    if (f.stabilisationComboBox.SelectedIndex == f.stabilisationComboBox.FindStringExact("Stabilisation_Passerelle"))
                    {
                        passerelleSelectionneeElement.LookupParameter("Glissiere de reprise").Set(vrai);
                    }
                    else
                    {
                        passerelleSelectionneeElement.LookupParameter("Glissiere de reprise").Set(faux);
                    }
                    #endregion banche

                    #region angles
                    if (passerelleSelectionneeElement.Name.Contains("M2")
                    || passerelleSelectionneeElement.Name.Contains("M3"))
                    {
                        //reglage angle gauche
                        if (f.angleGaucheCheckBox.Checked == true)
                            passerelleSelectionneeElement.LookupParameter("Gauche - Angle droit").Set(vrai);
                        else
                            passerelleSelectionneeElement.LookupParameter("Gauche - Angle droit").Set(faux);

                        //reglage angle droit
                        if (f.angleDroitCheckBox.Checked == true)
                            passerelleSelectionneeElement.LookupParameter("Droite - Angle droit").Set(vrai);
                        else
                            passerelleSelectionneeElement.LookupParameter("Droite - Angle droit").Set(faux);
                    }
                    #endregion angles

                    #region angle interieur
                    //reglage angle interieur gauche
                    if (f.angleGaucheInterieurCheckBox.Checked == true)
                    {
                        passerelleSelectionneeElement.LookupParameter("Sabot gauche").Set(vrai);
                        passerelleSelectionneeElement.LookupParameter("Distance mur-sabot gauche").SetValueString(f.sabotGaucheTextBox1.Text);
                    }
                    else passerelleSelectionneeElement.LookupParameter("Sabot gauche").Set(faux);

                    if (f.angleGaucheInterieurCheckBox2.Checked == true)
                    {
                        passerelleSelectionneeElement.LookupParameter("LG_MET_PTLG_Sabot gauche 2").Set(vrai);
                        passerelleSelectionneeElement.LookupParameter("Distance mur-sabot gauche 2").SetValueString(f.sabotGaucheTextBox2.Text);
                    }
                    else passerelleSelectionneeElement.LookupParameter("LG_MET_PTLG_Sabot gauche 2").Set(faux);
                    //reglage angle interieur droit
                    if (f.angleDroitInterieurCheckBox.Checked == true)
                    {
                        passerelleSelectionneeElement.LookupParameter("Sabot droit").Set(vrai);
                        passerelleSelectionneeElement.LookupParameter("Distance mur-sabot droit").SetValueString(f.sabotDroitTextBox1.Text);
                    }
                    else passerelleSelectionneeElement.LookupParameter("Sabot droit").Set(faux);
                    if (f.angleDroitInterieurCheckBox2.Checked == true)
                    {
                        passerelleSelectionneeElement.LookupParameter("LG_MET_PTLG_Sabot droit 2").Set(vrai);
                        passerelleSelectionneeElement.LookupParameter("Distance mur-sabot droit 2").SetValueString(f.sabotDroitTextBox2.Text);
                    }
                    else passerelleSelectionneeElement.LookupParameter("LG_MET_PTLG_Sabot droit 2").Set(faux);
                    #endregion angle interieur

                    #region extensions
                    passerelleSelectionneeElement.LookupParameter("Longueur extension droite").SetValueString(f.extensionDroiteComboBox.SelectedItem.ToString());
                    passerelleSelectionneeElement.LookupParameter("Longueur extension gauche").SetValueString(f.extensionGaucheComboBox.SelectedItem.ToString());
                    if (f.angleBiaisDroitCheckBox.Checked == true)
                        passerelleSelectionneeElement.LookupParameter("Longueur extension droite interieure").SetValueString(f.extensionDroiteIntComboBox.SelectedItem.ToString());
                    else passerelleSelectionneeElement.LookupParameter("Longueur extension droite interieure").SetValueString(f.extensionDroiteComboBox.SelectedItem.ToString());

                    if (f.angleBiaisGaucheCheckBox.Checked == true)
                        passerelleSelectionneeElement.LookupParameter("Longueur extension gauche interieure").SetValueString(f.extensionGaucheIntComboBox.SelectedItem.ToString());
                    else passerelleSelectionneeElement.LookupParameter("Longueur extension gauche interieure").SetValueString(f.extensionGaucheComboBox.SelectedItem.ToString());
                    #endregion extensions

                    #region GC
                    //reglage gc droit
                    if (f.gcDroiteCheckBox.Checked == true)
                    {
                        passerelleSelectionneeElement.LookupParameter("GC about droit").Set(vrai);
                    }
                    else
                    {
                        passerelleSelectionneeElement.LookupParameter("GC about droit").Set(faux);
                    }
                    //reglage gc gauche
                    if (f.gcGaucheCheckBox.Checked == true)
                    {
                        passerelleSelectionneeElement.LookupParameter("GC about gauche").Set(vrai);
                    }
                    else
                    {
                        passerelleSelectionneeElement.LookupParameter("GC about gauche").Set(faux);
                    }
                    #endregion GC

                    #region attaches
                    //reglage attache droite
                    foreach (string str in f.attacheDroiteComboBox.Items)
                    {
                        if (f.attacheDroiteComboBox.SelectedIndex == f.attacheDroiteComboBox.FindStringExact(str))
                        {
                            passerelleSelectionneeElement.LookupParameter(str).Set(vrai);
                            passerelleSelectionneeElement.LookupParameter("Type d'attache 2").Set(str);
                        }
                        else passerelleSelectionneeElement.LookupParameter(str).Set(faux);
                    }
                    passerelleSelectionneeElement.LookupParameter("Distance attache droite").SetValueString(distanceAttacheDroite.ToString());

                    //reglage attache gauche
                    foreach (string str in f.attacheGaucheComboBox.Items)
                    {
                        if (f.attacheGaucheComboBox.SelectedIndex == f.attacheGaucheComboBox.FindStringExact(str))
                        {

                            passerelleSelectionneeElement.LookupParameter("Gauche - " + str).Set(vrai);
                            passerelleSelectionneeElement.LookupParameter("Type d'attache 1").Set(str);
                        }
                        else passerelleSelectionneeElement.LookupParameter("Gauche - " + str).Set(faux);
                    }
                    passerelleSelectionneeElement.LookupParameter("Distance attache gauche").SetValueString(distanceAttacheGauche.ToString());
                    
                    //reglage attache centre
                    //if (passerelleSelectionneeElement.Name.Contains("M3"))
                    //{
                    if (f.troisiemeAttacheCheckBox.Checked == true && f.quatriemeAttacheCheckBox.Checked == false)
                    {
                        passerelleSelectionneeElement.LookupParameter("3ème attache").Set(vrai);
                        passerelleSelectionneeElement.LookupParameter("4ème attache").Set(faux);
                        foreach (string str in f.attacheCentreComboBox.Items)
                        {
                            if (f.attacheCentreComboBox.SelectedIndex == f.attacheCentreComboBox.FindStringExact(str))
                            {
                                passerelleSelectionneeElement.LookupParameter("Centre - " + str).Set(vrai);
                                passerelleSelectionneeElement.LookupParameter("Type d'attache 3").Set(str);
                            }
                            else passerelleSelectionneeElement.LookupParameter("Centre - " + str).Set(faux);
                        }
                        foreach (string str in f.attacheCentreDroitComboBox.Items)
                        {
                            passerelleSelectionneeElement.LookupParameter("Centre droit - " + str).Set(faux);
                        }
                        passerelleSelectionneeElement.LookupParameter("Type d'attache 4").Set("");
                        passerelleSelectionneeElement.LookupParameter("Distance attache centre").SetValueString(distanceAttacheCentre.ToString());
                    }
                    else if (f.troisiemeAttacheCheckBox.Checked == true && f.quatriemeAttacheCheckBox.Checked == true)
                    {
                        passerelleSelectionneeElement.LookupParameter("3ème attache").Set(vrai);
                        passerelleSelectionneeElement.LookupParameter("4ème attache").Set(vrai);
                        foreach (string str in f.attacheCentreComboBox.Items)
                        {
                            if (f.attacheCentreComboBox.SelectedIndex == f.attacheCentreComboBox.FindStringExact(str))
                            {
                                passerelleSelectionneeElement.LookupParameter("Centre - " + str).Set(vrai);
                                passerelleSelectionneeElement.LookupParameter("Type d'attache 3").Set(str);
                            }
                            else passerelleSelectionneeElement.LookupParameter("Centre - " + str).Set(faux);
                        }
                        foreach (string str in f.attacheCentreDroitComboBox.Items)
                        {
                            if (f.attacheCentreDroitComboBox.SelectedIndex == f.attacheCentreDroitComboBox.FindStringExact(str))
                            {
                                passerelleSelectionneeElement.LookupParameter("Centre droit - " + str).Set(vrai);
                                passerelleSelectionneeElement.LookupParameter("Type d'attache 4").Set(str);
                            }
                            else passerelleSelectionneeElement.LookupParameter("Centre droit - " + str).Set(faux);
                        }
                        passerelleSelectionneeElement.LookupParameter("Distance attache centre").SetValueString(distanceAttacheCentre.ToString());
                        passerelleSelectionneeElement.LookupParameter("Distance attache centre droit").SetValueString(distanceAttacheCentreDroit.ToString());
                    }
                    else
                    {
                        passerelleSelectionneeElement.LookupParameter("3ème attache").Set(faux);
                        foreach (string str in f.attacheCentreComboBox.Items)
                        {
                            passerelleSelectionneeElement.LookupParameter("Centre - " + str).Set(faux);
                            passerelleSelectionneeElement.LookupParameter("Type d'attache 3").Set("");
                        }
                        passerelleSelectionneeElement.LookupParameter("4ème attache").Set(faux);
                        foreach (string str in f.attacheCentreDroitComboBox.Items)
                        {
                            passerelleSelectionneeElement.LookupParameter("Centre droit - " + str).Set(faux);
                            passerelleSelectionneeElement.LookupParameter("Type d'attache 4").Set("");
                        }
                    }

                    //}
                    #endregion attaches

                    #region extension arriere
                    if (f.extArriereCheckBox.Checked == true)
                    {
                        passerelleSelectionneeElement.LookupParameter("Extension arriere").Set(vrai);
                    }
                    else
                    {
                        passerelleSelectionneeElement.LookupParameter("Extension arriere").Set(faux);
                    }
                    #endregion extension arriere

                    passerelleSelectionneeElement.LookupParameter("Passerelle incorrecte").Set(faux);

                    #region Désignation
                    passerelleSelectionneeElement.LookupParameter("LG_MET_PTLG_Designation").Set(f.numeroTextBox.Text);
                    #endregion

                    #region Code bible
                    if (f.numeroTextBox.Text != "" && f.numeroTextBox.Text != "XX")
                    {
                        try
                        {
                            passerelleSelectionneeElement.LookupParameter("LG_EDP_Code bible").Set("PTLG_" + type);
                        }
                        catch
                        {
                            TaskDialog.Show("Erreur", "Merci d'ajouter le parametre code bible");
                        }
                    }
                    else
                    {
                        passerelleSelectionneeElement.LookupParameter("LG_EDP_Code bible").Set("");
                    }
                    #endregion

                    #region Auvent
                    if (f.angleGaucheCheckBox.Checked == true && f.extArriereCheckBox.Checked == true)
                    {
                        passerelleSelectionneeElement.LookupParameter("Auvent arriere gauche").Set(vrai);
                    }
                    else
                    {
                        passerelleSelectionneeElement.LookupParameter("Auvent arriere gauche").Set(faux);
                    }
                    if (f.angleDroitCheckBox.Checked == true && f.extArriereCheckBox.Checked == true)
                    {
                        passerelleSelectionneeElement.LookupParameter("Auvent arriere droit").Set(vrai);
                    }
                    else
                    {
                        passerelleSelectionneeElement.LookupParameter("Auvent arriere droit").Set(faux);
                    }
                    #endregion Auvent

                    trButton.Commit();
                }
            };

            #endregion applybutton

            #region reglage pied, options
            ConfigurateurFootOptions.FootOptionsMan(f, passerelleSelectionneeElement, doc);
            #endregion reglage pied, options

            #region elevation passerelle
            double h = RetournePiedEnCM((passerelleSelectionneeElement.Location as LocationPoint).Point.Z);
            Transaction trElevPass = new Transaction(doc, "Configuration PTLG - Elevation passerelle");
            {
                trElevPass.Start();
                try
                {
                    passerelleSelectionneeElement.LookupParameter("LG_MET_PTLG_Elevation passerelle").SetValueString(h.ToString());
                    trElevPass.Commit();
                }
                catch
                {
                    TaskDialog.Show("Warning", "L'élément n'a le paramètre 'LG_MET_PTLG_Elevation passerelle'");
                    trElevPass.RollBack();
                }
            }
            #endregion elevation passerelle

            #endregion Buttons appliquer, reglage pied, options, elevation


            return Result.Succeeded;

        }
    }
}
#endregion