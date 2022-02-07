///Cette class n'est plus utilisée.
///Toute cette classe est dans la configurateurClass

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
    public class optionsClass : IExternalCommand
    {
        public string rallongeString= "De 325 à 500 maxi (pas=7.5)" + "\n" + "Bracon supplémentaire obligatoire";
        public string ubasString= "b";
        public string glissiereString= "Ancrage sur glissière si stabilisation sur passerelle" + "\n" + "Bracon supplémentaire si plate-forme de 2.5m" + "\n" + "Un ancrage U bas par ferme";
        public string braconString= "d";        

        public Result Execute(
           ExternalCommandData commandData,
           ref string message,
           ElementSet elements)
        {
            #region parametres pour code
            //Get application and document objects
            UIApplication uiApp = commandData.Application;
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
            {
             
                if (selectionPasserelleIdList.Count == 0)
                    TaskDialog.Show("ERREUR", "Selectionner une passerelle");

                if (selectionPasserelleIdList.Count > 1)
                    TaskDialog.Show("ERREUR", "Selectionner une seule passerelle");
            }
            #endregion erreur plusieurs objetcs selectionnes

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
                #region on commence la configuration des options
                {
                    #region liste de tous les parametres de la passerelle selectionnee
                    List<Autodesk.Revit.DB.Parameter> parameterList = new List<Autodesk.Revit.DB.Parameter>();
                    foreach (Autodesk.Revit.DB.Parameter para in passerelleSelectionneeElement.Parameters)
                    {
                        //On crée la liste de tous les parametres de la passerelle selectionnee
                        parameterList.Add(para);
                    }
                    #endregion liste de tous les parametres de la passerelle selectionnee                   
                    optionsForm op = new optionsForm();

                    #region initialisation de la form
                    //Initialisation retrait du auvent gauche
                    op.RetraitGTextbox.Text = passerelleSelectionneeElement.GetParameters("Retrait gauche")[0].AsValueString();

                    //Initialisation retrait du auvent droit
                    op.RetraitDTextbox.Text = passerelleSelectionneeElement.GetParameters("Retrait droit")[0].AsValueString();

                    //Initialisation retrait du GC Gauche
                    op.GCGtextBox.Text = passerelleSelectionneeElement.GetParameters("Retrait GC gauche")[0].AsValueString();

                    //Initialisation retrait du GC Droit
                    op.GCDtextBox.Text = passerelleSelectionneeElement.GetParameters("Retrait GC droite")[0].AsValueString();

                    //Initialisation découpe dans extension gauche
                    if (passerelleSelectionneeElement.GetParameters("LG_MET_PTLG_Decoupe gauche")[0].AsInteger() == 1)
                    {
                        op.DecoupeGcheckBox.Checked = true;
                        op.DecoupeGDisttextBox.Text = passerelleSelectionneeElement.GetParameters("LG_MET_PTLG_Decoupe 1 distance au bord")[0].AsValueString();
                        op.DecoupeGLrgtextBox.Text = passerelleSelectionneeElement.GetParameters("LG_MET_PTLG_Decoupe 1 Largeur")[0].AsValueString();
                        op.DecoupeGProftextBox.Text = passerelleSelectionneeElement.GetParameters("LG_MET_PTLG_Decoupe 1 profondeur")[0].AsValueString();
                    }
                    else
                    {
                        op.DecoupeGDisttextBox.Text = passerelleSelectionneeElement.GetParameters("LG_MET_PTLG_Decoupe 1 distance au bord")[0].AsValueString();
                        op.DecoupeGLrgtextBox.Text = passerelleSelectionneeElement.GetParameters("LG_MET_PTLG_Decoupe 1 Largeur")[0].AsValueString();
                        op.DecoupeGProftextBox.Text = passerelleSelectionneeElement.GetParameters("LG_MET_PTLG_Decoupe 1 profondeur")[0].AsValueString();
                        op.DecoupeGcheckBox.Checked = false;
                    }
                    //Initialisation découpe dans extension droite
                    if (passerelleSelectionneeElement.GetParameters("LG_MET_PTLG_Decoupe droite")[0].AsInteger() == 1)
                    {
                        op.DecoupeDcheckBox.Checked = true;
                        op.DecoupeDDisttextBox.Text = passerelleSelectionneeElement.GetParameters("LG_MET_PTLG_Decoupe 2 distance au bord")[0].AsValueString();
                        op.DecoupeDLrgtextBox.Text = passerelleSelectionneeElement.GetParameters("LG_MET_PTLG_Decoupe 2 Largeur")[0].AsValueString();
                        op.DecoupeDProftextBox.Text = passerelleSelectionneeElement.GetParameters("LG_MET_PTLG_Decoupe 2 profondeur")[0].AsValueString();
                    }
                    else
                    {
                        op.DecoupeDDisttextBox.Text = passerelleSelectionneeElement.GetParameters("LG_MET_PTLG_Decoupe 2 distance au bord")[0].AsValueString();
                        op.DecoupeDLrgtextBox.Text = passerelleSelectionneeElement.GetParameters("LG_MET_PTLG_Decoupe 2 Largeur")[0].AsValueString();
                        op.DecoupeDProftextBox.Text = passerelleSelectionneeElement.GetParameters("LG_MET_PTLG_Decoupe 2 profondeur")[0].AsValueString();
                        op.DecoupeDcheckBox.Checked = false;
                    }

                    //Initialisation glissière de reprise
                    if (passerelleSelectionneeElement.GetParameters("Glissiere de reprise")[0].AsInteger()==1)
                        op.uBasCheckBox.Checked=true;
                    else
                        op.uBasCheckBox.Checked = false;

                    //Initialisation bracon
                    if (
                        passerelleSelectionneeElement.GetParameters("Bracon")[0].AsInteger() == 1||
                        passerelleSelectionneeElement.GetParameters("Bracon gauche")[0].AsInteger() == 1
                        )
                        op.braconCheckBox.Checked = true;
                    else
                        op.braconCheckBox.Checked = false;

                    #endregion

                    #region bouton appliquer
                    op.applyOptionsButton.Click += (sander, a) =>
                    {
                        Transaction trOptions = new Transaction(doc, "Configuration options");
                        trOptions.Start();
                        if (op.uBasCheckBox.Checked == true)
                            passerelleSelectionneeElement.LookupParameter("Glissiere de reprise").Set(1);
                        else
                            passerelleSelectionneeElement.LookupParameter("Glissiere de reprise").Set(0);
                        if (op.braconCheckBox.Checked == true)
                        {
                            passerelleSelectionneeElement.LookupParameter("Bracon").Set(1);
                            passerelleSelectionneeElement.LookupParameter("Bracon gauche").Set(1);
                        }
                        else
                        {
                            passerelleSelectionneeElement.LookupParameter("Bracon").Set(0);
                            passerelleSelectionneeElement.LookupParameter("Bracon gauche").Set(0);
                        }
                        trOptions.Commit();
                    };
                    op.ShowDialog();
                    #endregion

                }//else
                #endregion configuration des options
            }// if un seul objet selectionne
            return Result.Succeeded;
        }//elementSet
    }
}

