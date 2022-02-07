using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;
using Autodesk.Revit;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Input;
using System.Windows.Forms.DataVisualization;
using System.Data;
using DataAccess;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;
using static PTLGClassLibrary.GeneralClass;


namespace PTLGClassLibrary
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(RegenerationOption.Manual)]
    public class ModelisationAuto : IExternalCommand
    {

        public Result Execute(
            ExternalCommandData commandData,
            ref string message,
            ElementSet elements)
        {

            #region Commands on document
            //Get application and document objects
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiApp.ActiveUIDocument.Document;

            //variables definies pour tout le code
            //int vrai = 1;
            //int faux = 0;

            #endregion
            using (ModelisationAutoForm h = new ModelisationAutoForm())
            {

                #region déclaration des variables

                Double lgMurCurve = new Double();
                int lgMurInt = new int();

                //nombre de passerelles
                //int nM1 = new int();
                int nM2 = new int();
                int nM3 = new int();

                //int nM1bis = new int();
                int nM2bis = new int();
                int nM3bis = new int();

                int nM3ter = new int();

                //les longueurs maxi
                int lM1 = new int();
                int lM2 = new int();
                int lM3 = new int();

                //int lM1bis = new int();
                int lM2bis = new int();
                int lM3bis = new int();

                int lM3ter = new int();

                //les restes
                int r1 = new int();
                int r2 = new int();

                //variables maxi
                // pour la M3
                int lgMaxM3 = new int();
                int leMaxM3 = new int();
                int cMaxM3 = new int();
                int dMaxM3 = new int();
                int lgFixeM3 = new int();
                // pour la M2
                int lgMaxM2 = new int();
                int leMaxM2 = new int();
                int cMaxM2 = new int();
                int dMaxM2 = new int();
                int lgFixeM2 = new int();
                // pour la M1
                int lgMaxM1 = new int();
                int leMaxM1 = new int();
                int cMaxM1 = new int();
                int dMaxM1 = new int();
                int lgFixeM1 = new int();


                //première et dernière passerelles
                int pR = new int();
                int dR = new int();
                int prG = new int();
                int prC = new int();
                int prD = new int();
                int drG = new int();
                int drC = new int();
                int drD = new int();

                FamilySymbol prfs = null;
                FamilySymbol drfs = null;

                #endregion déclaration des variables
                #region Family Collector des passerelles
                // Get an available passerelle famille from document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilySymbol));
                collector.OfCategory(BuiltInCategory.OST_GenericModel);
                List<Element> collectorList = new List<Element>();

                foreach (Element xx in collector)
                {
                    if (xx.Name.Contains("LG_M1")
                        || xx.Name.Contains("LG_M2")
                        || xx.Name.Contains("LG_M3"))
                    {
                        collectorList.Add(xx);
                    }
                };

                if (collectorList.Count < 3)
                {
                    TaskDialog.Show("Erreur", "Veuillez charger les familles de passerelles M1/M2/M3 avant de lancer le plugin");
                }
                
                else
                {
                FamilySymbol m1FS = null;
                FamilySymbol m2FS = null;
                FamilySymbol m3FS = null;

                foreach (Element yy in collectorList)
                {
                    if (yy.Name.Contains("LG_M1"))
                    {
                        m1FS = yy as FamilySymbol;
                    }
                    else if (yy.Name.Contains("LG_M2"))
                    {
                        m2FS = yy as FamilySymbol;
                    }
                    else if (yy.Name.Contains("LG_M3"))
                    {
                        m3FS = yy as FamilySymbol;
                    }
                }
                #endregion Family Collector
                #region Tableau Valeurs maxi C,D,Le,L= f(banche)
                //création du tableau des valeurs maxi, tableau à 5 lignes et 15 colonnes
                int[,] arraymaxi = new int[6, 15]
                        {
                        {//Ligne 00 pas de banche
                            //M1
                            80,2,220,75,160,
                            //M2
                            80,2,370,95,300,
                            //M3
                            80,2,650,160,420
                        },
                        {//Ligne 0 banche de 0 à 280
                            //M1
                            80,2,220,75,160,
                            //M2
                            80,2,370,95,300,
                            //M3
                            80,2,650,160,420
                        },
                        {//Ligne 1 banche de 280 à 330
                            //M1
                            70,2,220,75,160,
                            //M2
                            70,2,370,95,300,
                            //M3
                            70,2,650,160,420
                        },
                        {//Ligne 2 banche de 330 à 380
                            //M1
                            65,2,220,75,150,
                            //M2
                            65,2,370,95,300,
                            //M3
                            65,2,650,160,370
                        },
                        {//Ligne 3 banche de 380 à 430
                            //M1
                            60,2,220,75,150,
                            //M2
                            60,2,370,95,300,
                            //M3
                            60,2,580,125,350
                        },
                        {//Ligne 4 banche de 430 à 480
                            //M1
                            55,2,220,75,150,
                            //M2
                            55,2,370,95,250,
                            //M3
                            55,2,550,110,350
                        }
                        };
                #endregion tableau

                #region initialisation comboboxbanche
                //initialisation comboxbox selectionne et hauteurbancheindex, par défaut pas de banche
                h.bancheComboBox.SelectedIndex = 0;
                #endregion initialisation combobox banche
                #region initialisation lecture et stockage des valeurs maxi si on a encore la valeur par défaut
                lgMaxM3 = arraymaxi[h.bancheComboBox.SelectedIndex, 12];
                leMaxM3 = arraymaxi[h.bancheComboBox.SelectedIndex, 13];
                cMaxM3 = arraymaxi[h.bancheComboBox.SelectedIndex, 10];
                dMaxM3 = arraymaxi[h.bancheComboBox.SelectedIndex, 14];
                lgFixeM3 = 330;

                lgMaxM2 = arraymaxi[h.bancheComboBox.SelectedIndex, 7];
                leMaxM2 = arraymaxi[h.bancheComboBox.SelectedIndex, 8];
                cMaxM2 = arraymaxi[h.bancheComboBox.SelectedIndex, 5];
                dMaxM2 = arraymaxi[h.bancheComboBox.SelectedIndex, 9];
                lgFixeM2 = 180;

                lgMaxM1 = arraymaxi[h.bancheComboBox.SelectedIndex, 2];
                leMaxM1 = arraymaxi[h.bancheComboBox.SelectedIndex, 3];
                cMaxM1 = arraymaxi[h.bancheComboBox.SelectedIndex, 0];
                dMaxM1 = arraymaxi[h.bancheComboBox.SelectedIndex, 4];
                lgFixeM1 = 70;
                #endregion lecture et stockage des valeurs maxi

                #region parameter Insert and rotate and parametres
                //récupération de la ligne selectionnée
                Selection selection = uiDoc.Selection;
                ICollection<ElementId> selectedIds = uiDoc.Selection.GetElementIds();
                List<ElementId> selectionIdlist = selection.GetElementIds().ToList();

                int abc = new int();
                abc = 0;

                foreach (ElementId eltId in selectionIdlist)
                {

                    Element selectionelement = doc.GetElement(selectionIdlist[abc]);
                    #region Erreur : objet selectionné n'est pas une ligne
                    // Si le nom de l'objet selectionné ne contient pas LG_M
                    if (!(selectionelement.Category.Name == "Lignes"))
                    {
                        TaskDialog.Show("Erreur", "L'objet numéro " + abc.ToString() + " selectionné n'est pas une ligne");
                    }


                    #endregion

                    //transformation en curve
                    Double feet = 30.48;
                    LocationCurve lc = selectionelement.Location as LocationCurve;
                    Curve c = lc.Curve;

                    XYZ pointDebut = c.GetEndPoint(0);
                    XYZ pointFin = c.GetEndPoint(1);
                    lgMurCurve = c.Length * feet;
                    lgMurInt = (int)(lgMurCurve / 5) * 5;


                #endregion parameter of Insert and rotate

                    #region initialisation
                    #region Cas 1 //Rien
                    if ((0 <= lgMurInt) && (lgMurInt < 100)) //passerelle impossible
                    {
                        h.option1button.Visible = false;
                        h.option2button.Visible = false;
                        h.option3button.Visible = false;
                        TaskDialog.Show("Attention", "Le voile est trop court pour installer une passerelle");
                    }
                    #endregion Cas 1
                    #region Cas 2 //M1
                    else if ((100 <= lgMurInt) && (lgMurInt <= 200)) //place pour un M1 seulement
                    {
                        h.option1button.Visible = false;
                        h.option2button.Visible = false;
                        h.option3button.Visible = false;
                        TaskDialog.Show("Information", "Seule une M1 est possible");

                        pR = 1;
                        prfs = m1FS;
                        prC = 70;
                        if (lgMurInt % 10 == 0)
                        {
                            prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                            prD = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                        }
                        else
                        {
                            prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                            prD = ((int)((lgMurInt - prC) / 2) / 5) * 5 + 5;
                        }

                    }
                    #endregion Cas 2
                    #region Cas 3 //M1 //M1+M1
                    else if ((205 <= lgMurInt) && (lgMurInt <= 215))
                    {
                        h.option1button.Visible = true;
                        h.option2button.Visible = true;
                        h.option3button.Visible = false;
                        h.option1button.Text = "M1";
                        h.option2button.Text = "M1+M1";
                        if (h.option1CheckBox.Checked == true)
                        {
                            pR = 1;
                            prfs = m1FS;
                            prC = 70;
                            if ((lgMurInt - prC) % 10 == 0)
                            {
                                prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                prD = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                            }
                            else
                            {
                                prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                prD = ((int)((lgMurInt - prC) / 2) / 5) * 5 + 5;
                            }
                        }


                        else if (h.option2CheckBox.Checked == true)
                        {
                            pR = 1;
                            prfs = m1FS;
                            prC = 70;
                            dR = 1;
                            drfs = m1FS;
                            drC = 70;
                            if ((lgMurInt - (prC + drC + 5)) % 20 == 0)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 15)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 10)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 5)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                        }//option 2

                    }
                    #endregion Cas 3
                    #region Cas 4 //M1 //M1+M1 //M2
                    else if (lgMurInt == 220)
                    {
                        h.option1button.Visible = true;
                        h.option2button.Visible = true;
                        h.option3button.Visible = true;
                        h.option1button.Text = "M1";
                        h.option2button.Text = "M1+M1";
                        h.option3button.Text = "M2";
                        if (h.option1CheckBox.Checked == true)
                        {
                            pR = 1;
                            prfs = m1FS;
                            prC = 70;

                            if ((lgMurInt - prC) % 10 == 00)
                            {
                                prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                prD = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                            }
                            else
                            {
                                prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                prD = ((int)((lgMurInt - prC) / 2) / 5) * 5 + 5;
                            }
                        }//option 1
                        else if (h.option2CheckBox.Checked == true)
                        {
                            pR = 1;
                            prfs = m1FS;
                            prC = 70;
                            dR = 1;
                            drfs = m1FS;
                            drC = 70;
                            if ((lgMurInt - (prC + drC + 5)) % 20 == 0)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 15)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 10)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 5)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                        }//option 2
                        if (h.option3CheckBox.Checked == true)
                        {
                            pR = 1;
                            prfs = m2FS;
                            prC = 180;
                            if ((lgMurInt - prC) % 10 == 00)
                            {
                                prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                prD = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                            }
                            else
                            {
                                prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                prD = ((int)((lgMurInt - prC) / 2) / 5) * 5 + 5;
                            }
                        }//option 3
                    }
                    #endregion Cas 4
                    #region Cas 5 //M2 //M1+M1
                    else if ((225 <= lgMurInt) && (lgMurInt <= 300))
                    {
                        h.option1button.Visible = true;
                        h.option2button.Visible = true;
                        h.option3button.Visible = false;
                        h.option1button.Text = "M2";
                        h.option2button.Text = "M1+M1";
                        if (h.option1CheckBox.Checked == true)
                        {
                            pR = 1;
                            prfs = m2FS;
                            prC = 180;
                            if ((lgMurInt - prC) % 10 == 00)
                            {
                                prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                prD = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                            }
                            else
                            {
                                prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                prD = ((int)((lgMurInt - prC) / 2) / 5) * 5 + 5;
                            }
                        }//option 1
                        else if (h.option2CheckBox.Checked == true)
                        {
                            pR = 1;
                            prfs = m1FS;
                            prC = 70;
                            dR = 1;
                            drfs = m1FS;
                            drC = 70;
                            if ((lgMurInt - (prC + drC + 5)) % 20 == 0)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 15)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 10)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 5)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                        }//option 2

                    }
                    #endregion Cas 5
                    #region Cas 6 //M2 //M1+M1 //M1+M2
                    else if ((305 <= lgMurInt) && (lgMurInt <= 370))
                    {
                        h.option1button.Visible = true;
                        h.option2button.Visible = true;
                        h.option3button.Visible = true;
                        h.option1button.Text = "M2";
                        h.option2button.Text = "M1 + M1";
                        h.option3button.Text = "M1+M2";
                        if (h.option1CheckBox.Checked == true)
                        {
                            pR = 1;
                            prfs = m2FS;
                            prC = 180;
                            if ((lgMurInt - prC) % 10 == 00)
                            {
                                prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                prD = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                            }
                            else
                            {
                                prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                prD = ((int)((lgMurInt - prC) / 2) / 5) * 5 + 5;
                            }
                        }//option 1
                        else if (h.option2CheckBox.Checked == true)
                        {
                            pR = 1;
                            prfs = m1FS;
                            prC = 70;
                            dR = 1;
                            drfs = m1FS;
                            drC = 70;
                            if ((lgMurInt - (prC + drC + 5)) % 20 == 0)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 15)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 10)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 5)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                        }//option 2

                        else if (h.option3CheckBox.Checked == true)
                        {

                            pR = 1;
                            prfs = m1FS;
                            prC = 70;
                            dR = 1;
                            drfs = m2FS;
                            drC = 180;
                            if ((lgMurInt - (prC + drC + 5)) % 20 == 0)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 15)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 10)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 5)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                        }//option 3
                    }
                    #endregion Cas 6
                    #region Cas 7 //M2 //M3
                    else if (lgMurInt == 370)
                    {
                        h.option1button.Visible = true;
                        h.option2button.Visible = true;
                        h.option3button.Visible = false;
                        h.option1button.Text = "M2";
                        h.option2button.Text = "M3";
                        if (h.option1CheckBox.Checked == true)
                        {
                            pR = 1;
                            prfs = m2FS;
                            prC = 180;
                            if ((lgMurInt - prC) % 10 == 00)
                            {
                                prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                prD = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                            }
                            else
                            {
                                prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                prD = ((int)((lgMurInt - prC) / 2) / 5) * 5 + 5;
                            }
                        }//option 1
                        if (h.option2CheckBox.Checked == true)
                        {
                            pR = 1;
                            prfs = m3FS;
                            prC = 330;
                            if ((lgMurInt - prC) % 10 == 00)
                            {
                                prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                prD = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                            }
                            else
                            {
                                prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                prD = ((int)((lgMurInt - prC) / 2) / 5) * 5 + 5;
                            }
                        }//option 2
                    }
                    #endregion Cas 7
                    #region Cas 8 //M3  //M1+M2
                    else if ((370 < lgMurInt) && (lgMurInt < 445))
                    {
                        h.option1button.Visible = true;
                        h.option2button.Visible = true;
                        h.option3button.Visible = false;
                        h.option1button.Text = "M3";
                        h.option2button.Text = "M1 + M2";
                        if (h.option1CheckBox.Checked == true)
                        {
                            pR = 1;
                            prfs = m3FS;
                            prC = 330;
                            if ((lgMurInt - prC) % 10 == 00)
                            {
                                prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                prD = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                            }
                            else
                            {
                                prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                prD = ((int)((lgMurInt - prC) / 2) / 5) * 5 + 5;
                            }
                        }//option 1
                        else if (h.option2CheckBox.Checked == true)
                        {
                            pR = 1;
                            prfs = m1FS;
                            prC = 70;
                            dR = 1;
                            drfs = m2FS;
                            drC = 180;
                            if ((lgMurInt - (prC + drC + 5)) % 20 == 0)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 15)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 10)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 5)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                        }//option 2

                    }
                    #endregion Cas 8
                    #region Cas 9 //M3 //M2+M2
                    else if (lgMurInt == 445)
                    {
                        h.option1button.Visible = true;
                        h.option2button.Visible = true;
                        h.option3button.Visible = false;
                        h.option1button.Text = "M3";
                        h.option2button.Text = "M2 + M2";
                        if (h.option1CheckBox.Checked == true)
                        {
                            pR = 1;
                            prfs = m3FS;
                            prC = 330;
                            if ((lgMurInt - prC) % 10 == 00)
                            {
                                prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                prD = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                            }
                            else
                            {
                                prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                prD = ((int)((lgMurInt - prC) / 2) / 5) * 5 + 5;
                            }
                        }//option 1
                        else if (h.option2CheckBox.Checked == true)
                        {
                            pR = 1;
                            prfs = m2FS;
                            prC = 180;
                            dR = 1;
                            drfs = m2FS;
                            drC = 180;
                            if ((lgMurInt - (prC + drC + 5)) % 20 == 0)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 15)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 10)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 5)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                        }//option 2

                    }
                    #endregion Cas 9
                    #region Cas 10 //M3 //M2+M2
                    else if ((445 < lgMurInt) && (lgMurInt < 475))
                    {
                        h.option1button.Visible = true;
                        h.option2button.Visible = true;
                        h.option3button.Visible = false;
                        h.option1button.Text = "M3";
                        h.option2button.Text = "M2 + M2";
                        if (h.option1CheckBox.Checked == true)
                        {
                            pR = 1;
                            prfs = m3FS;
                            prC = 330;
                            if ((lgMurInt - prC) % 10 == 00)
                            {
                                prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                prD = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                            }
                            else
                            {
                                prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                prD = ((int)((lgMurInt - prC) / 2) / 5) * 5 + 5;
                            }
                        }//option 1
                        else if (h.option2CheckBox.Checked == true)
                        {
                            pR = 1;
                            prfs = m2FS;
                            prC = 180;
                            dR = 1;
                            drfs = m2FS;
                            drC = 180;
                            if ((lgMurInt - (prC + drC + 5)) % 20 == 0)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 15)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 10)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 5)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                        }//option 2
                    }
                    #endregion Cas 10
                    #region Cas 11 // M3 //M2+M2 ou //M2+M2
                    else if ((475 <= lgMurInt) && (lgMurInt <= 590))
                    {
                        if (lgMaxM3 >= lgMurInt)
                        {
                            h.option1button.Visible = true;
                            h.option2button.Visible = true;
                            h.option3button.Visible = false;
                            h.option1button.Text = "M3";
                            h.option2button.Text = "M2 + M2";
                            if (h.option1CheckBox.Checked == true)
                            {
                                pR = 1;
                                prfs = m3FS;
                                prC = 330;
                                if ((lgMurInt - prC) % 10 == 00)
                                {
                                    prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                    prD = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                }
                                else
                                {
                                    prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                    prD = ((int)((lgMurInt - prC) / 2) / 5) * 5 + 5;
                                }
                            }//option 1
                            else if (h.option2CheckBox.Checked == true)
                            {
                                pR = 1;
                                prfs = m2FS;
                                prC = 180;
                                dR = 1;
                                drfs = m2FS;
                                drC = 180;
                                if ((lgMurInt - (prC + drC + 5)) % 20 == 0)
                                {
                                    prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                }
                                else if ((lgMurInt - (prC + drC + 5)) % 20 == 15)
                                {
                                    prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                    drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                    drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                }
                                else if ((lgMurInt - (prC + drC + 5)) % 20 == 10)
                                {
                                    prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                    drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                }
                                else if ((lgMurInt - (prC + drC + 5)) % 20 == 5)
                                {
                                    prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                }
                            }//option 2

                        }
                        else
                        {
                            h.option1button.Visible = true;
                            h.option2button.Visible = false;
                            h.option3button.Visible = false;
                            h.option1button.Text = "M2+M2";

                            pR = 1;
                            prfs = m2FS;
                            prC = 180;
                            dR = 1;
                            drfs = m2FS;
                            drC = 180;
                            if ((lgMurInt - (prC + drC + 5)) % 20 == 0)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 15)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 10)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 5)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                        }
                    }
                    #endregion Cas 11
                    #region Cas 12 //M3 //M2+M2 ou //M2
                    else if ((590 < lgMurInt) && (lgMurInt < 595))
                    {
                        if (lgMaxM3 >= lgMurInt)
                        {
                            h.option1button.Visible = true;
                            h.option2button.Visible = true;
                            h.option3button.Visible = false;
                            h.option1button.Text = "M3";
                            h.option2button.Text = "M2 + M2";
                            if (h.option1CheckBox.Checked == true)
                            {
                                pR = 1;
                                prfs = m3FS;
                                prC = 330;
                                if ((lgMurInt - prC) % 10 == 00)
                                {
                                    prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                    prD = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                }
                                else
                                {
                                    prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                    prD = ((int)((lgMurInt - prC) / 2) / 5) * 5 + 5;
                                }
                            }//option 1
                            else if (h.option2CheckBox.Checked == true)
                            {
                                pR = 1;
                                prfs = m2FS;
                                prC = 180;
                                dR = 1;
                                drfs = m2FS;
                                drC = 180;
                                if ((lgMurInt - (prC + drC + 5)) % 20 == 0)
                                {
                                    prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                }
                                else if ((lgMurInt - (prC + drC + 5)) % 20 == 15)
                                {
                                    prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                    drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                    drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                }
                                else if ((lgMurInt - (prC + drC + 5)) % 20 == 10)
                                {
                                    prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                    drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                }
                                else if ((lgMurInt - (prC + drC + 5)) % 20 == 5)
                                {
                                    prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                }
                            }//option 2
                        }
                        else
                        {

                            h.option1button.Visible = true;
                            h.option2button.Visible = false;
                            h.option3button.Visible = false;
                            h.option1button.Text = "M2";

                            pR = 1;
                            prfs = m2FS;
                            prC = 180;
                            if ((lgMurInt - prC) % 10 == 00)
                            {
                                prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                prD = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                            }
                            else
                            {
                                prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                prD = ((int)((lgMurInt - prC) / 2) / 5) * 5 + 5;
                            }
                        }
                    }
                    #endregion Cas 12
                    #region Cas 13 //M3 //M2+M2 ou //M2+M2 // M3+M2
                    else if ((595 <= lgMurInt) && (lgMurInt <= 650))
                    {
                        if (lgMaxM3 >= lgMurInt)
                        {
                            h.option1button.Visible = true;
                            h.option2button.Visible = true;
                            h.option3button.Visible = false;
                            h.option1button.Text = "M3";
                            h.option2button.Text = "M2 + M2";
                            if (h.option1CheckBox.Checked == true)
                            {
                                pR = 1;
                                prfs = m3FS;
                                prC = 330;
                                if ((lgMurInt - prC) % 10 == 00)
                                {
                                    prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                    prD = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                }
                                else
                                {
                                    prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                    prD = ((int)((lgMurInt - prC) / 2) / 5) * 5 + 5;
                                }
                            }//option 1
                            else if (h.option2CheckBox.Checked == true)
                            {
                                pR = 1;
                                prfs = m2FS;
                                prC = 180;
                                dR = 1;
                                drfs = m2FS;
                                drC = 180;
                                if ((lgMurInt - (prC + drC + 5)) % 20 == 0)
                                {
                                    prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                }
                                else if ((lgMurInt - (prC + drC + 5)) % 20 == 15)
                                {
                                    prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                    drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                    drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                }
                                else if ((lgMurInt - (prC + drC + 5)) % 20 == 10)
                                {
                                    prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                    drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                }
                                else if ((lgMurInt - (prC + drC + 5)) % 20 == 5)
                                {
                                    prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                }
                            }//option 2
                        }
                        else
                        {
                            h.option1button.Visible = true;
                            h.option2button.Visible = true;
                            h.option3button.Visible = false;
                            h.option1button.Text = "M2 + M2";
                            h.option2button.Text = "M3 + M2";
                            if (h.option1CheckBox.Checked == true)
                            {
                                pR = 1;
                                prfs = m2FS;
                                prC = 180;
                                dR = 1;
                                drfs = m2FS;
                                drC = 180;
                                if ((lgMurInt - (prC + drC + 5)) % 20 == 0)
                                {
                                    prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                }
                                else if ((lgMurInt - (prC + drC + 5)) % 20 == 15)
                                {
                                    prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                    drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                    drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                }
                                else if ((lgMurInt - (prC + drC + 5)) % 20 == 10)
                                {
                                    prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                    drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                }
                                else if ((lgMurInt - (prC + drC + 5)) % 20 == 5)
                                {
                                    prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                }
                            }//option 1
                            if (h.option2CheckBox.Checked == true)
                            {
                                pR = 1;
                                prfs = m3FS;
                                prC = 330;
                                dR = 1;
                                drfs = m2FS;
                                drC = 180;
                                if ((lgMurInt - (prC + drC + 5)) % 20 == 0)
                                {
                                    prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                }
                                else if ((lgMurInt - (prC + drC + 5)) % 20 == 15)
                                {
                                    prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                    drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                    drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                }
                                else if ((lgMurInt - (prC + drC + 5)) % 20 == 10)
                                {
                                    prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                    drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                }
                                else if ((lgMurInt - (prC + drC + 5)) % 20 == 5)
                                {
                                    prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                }
                            }//option 2
                        }
                    }
                    #endregion Cas 13
                    #region Cas 14 //M2+M2 // M3+M2
                    else if ((655 <= lgMurInt) && (lgMurInt <= 740))
                    {
                        h.option1button.Visible = true;
                        h.option2button.Visible = true;
                        h.option3button.Visible = false;
                        h.option1button.Text = "M2 + M2";
                        h.option2button.Text = "M3 + M2";
                        if (h.option1CheckBox.Checked == true)
                        {
                            pR = 1;
                            prfs = m2FS;
                            prC = 180;
                            dR = 1;
                            drfs = m2FS;
                            drC = 180;
                            if ((lgMurInt - (prC + drC + 5)) % 20 == 0)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 15)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 10)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 5)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                        }//option 1
                        if (h.option2CheckBox.Checked == true)
                        {
                            pR = 1;
                            prfs = m3FS;
                            prC = 330;
                            dR = 1;
                            drfs = m2FS;
                            drC = 180;
                            if ((lgMurInt - (prC + drC + 5)) % 20 == 0)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 15)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 10)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 5)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                        }//option 2
                    }
                    #endregion Cas 14
                    #region Cas 15 //M2+M2 //M3+M2 //M3+M3
                    else if (lgMurInt == 745)
                    {
                        h.option1button.Visible = true;
                        h.option2button.Visible = true;
                        h.option3button.Visible = true;
                        h.option1button.Text = "M2 + M2";
                        h.option2button.Text = "M3 + M2";
                        h.option3button.Text = "M3 + M3";
                        if (h.option1CheckBox.Checked == true)
                        {
                            pR = 1;
                            prfs = m2FS;
                            prC = 180;
                            dR = 1;
                            drfs = m2FS;
                            drC = 180;
                            if ((lgMurInt - (prC + drC + 5)) % 20 == 0)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 15)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 10)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 5)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                        }//option 1
                        else if (h.option2CheckBox.Checked == true)
                        {
                            pR = 1;
                            prfs = m3FS;
                            prC = 330;
                            dR = 1;
                            drfs = m2FS;
                            drC = 180;
                            if ((lgMurInt - (prC + drC + 5)) % 20 == 0)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 15)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 10)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 5)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                        }//option 2
                        else if (h.option3CheckBox.Checked == true)
                        {
                            pR = 1;
                            prfs = m3FS;
                            prC = 330;
                            dR = 1;
                            drfs = m3FS;
                            drC = 330;
                            if ((lgMurInt - (prC + drC + 5)) % 20 == 0)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 15)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 10)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 5)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                        }//option 3
                    }
                    #endregion Cas 15
                    #region Cas 16 //M2+M2 //M3+M3
                    else if ((750 <= lgMurInt) && (lgMurInt <= lgMaxM3 + 220 + 5))
                    {
                        h.option1button.Visible = true;
                        h.option2button.Visible = true;
                        h.option3button.Visible = false;
                        h.option1button.Text = "M3 + M2";
                        h.option2button.Text = "M3 + M3";
                        if (h.option1CheckBox.Checked == true)
                        {
                            pR = 1;
                            prfs = m3FS;
                            prC = 330;
                            dR = 1;
                            drfs = m2FS;
                            drC = 180;
                            if ((lgMurInt - (prC + drC + 5)) % 20 == 0)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 15)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 10)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 5)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                        }//option 1
                        else if (h.option2CheckBox.Checked == true)
                        {
                            pR = 1;
                            prfs = m3FS;
                            prC = 330;
                            dR = 1;
                            drfs = m3FS;
                            drC = 330;
                            if ((lgMurInt - (prC + drC + 5)) % 20 == 0)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 15)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 10)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                            else if ((lgMurInt - (prC + drC + 5)) % 20 == 5)
                            {
                                prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            }
                        }//option 2
                    }
                    #endregion Cas 16
                    #region Cas 17 //M3+M3
                    else if ((lgMaxM3 + 370 + 5 < lgMurInt) && (lgMurInt <= 2 * lgMaxM3 + 5))
                    {
                        h.option1button.Visible = true;
                        h.option2button.Visible = false;
                        h.option3button.Visible = false;
                        h.option1button.Text = "M3 + M3";
                        pR = 1;
                        prfs = m3FS;
                        prC = 330;
                        dR = 1;
                        drfs = m3FS;
                        drC = 330;
                        if ((lgMurInt - (prC + drC + 5)) % 20 == 0)
                        {
                            prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                            prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                            drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                            drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                        }
                        else if ((lgMurInt - (prC + drC + 5)) % 20 == 15)
                        {
                            prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                            prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                        }
                        else if ((lgMurInt - (prC + drC + 5)) % 20 == 10)
                        {
                            prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                            prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                            drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                            drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                        }
                        else if ((lgMurInt - (prC + drC + 5)) % 20 == 5)
                        {
                            prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                            prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                            drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                            drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                        }
                    }
                    #endregion Cas 17
                    #region Cas 18 Reste du monde
                    else
                    {
                        h.option1button.Visible = false;
                        h.option2button.Visible = false;
                        h.option3button.Visible = false;
                        //Première boucle
                        //on regarde combien de passerelles M3max-40 on peut insérer, exclues les passerelles de départ et d'arrivée
                        //nM3 = ((int)(lgMurInt - ((prG + prC + prD) + (drG + drC + drD)))) / (lgMaxM3 - 40);
                        //r1 = ((int)(lgMurInt - ((prG + prC + prD) + (drG + drC + drD)))) % (lgMaxM3 - 40) - (nM3 + pR + dR) * 5;
                        nM3 = ((int)(lgMurInt)) / (lgMaxM3 - 40);
                        r1 = ((int)(lgMurInt)) % (lgMaxM3 - 40) - (nM3) * 5;

                        //on regarde ce qu'on peut mettre à la fin
                        if (r1 > (lgMaxM2))
                        {
                            nM3bis = 1;
                            lM3bis = (int)(r1 / 5) * 5;
                            //insert une M3 de longueur r1
                        }
                        else if (r1 > lgMaxM1 & r1 < lgMaxM2)
                        {
                            nM2 = 1;
                            lM2 = (int)(r1 / 5) * 5;
                            //insert M2 longueur r1
                        }
                        else
                        {
                            //on enleve une passerelle M3 et on a un nouveau reste
                            r2 = (lgMaxM3 - 40) + r1 - 370;
                            nM3 = nM3 - 1;

                            nM3bis = 1;
                            lM3bis = 370;
                            //insert M3min puis complete

                            if (r2 > lgMaxM1 & r2 < lgMaxM2)
                            {
                                nM2bis = 1;
                                lM2bis = (int)(r2 / 5) * 5;
                                //insert M2 de longueur r2

                            }
                            else
                            {
                                nM3ter = 1;
                                lM3ter = (int)(r2 / 5) * 5;
                                //insert M3 de longueur r2
                            }
                        }
                    }
                    #endregion Reste du monde
                    #endregion initialisation
                    #region processus
                    h.eventCheckBox.CheckedChanged += (sender, e) =>
                    {
                        nM3 = 0;
                        pR = 0;
                        dR = 0;
                        prfs = null;
                        drfs = null;
                        prG = 0;
                        prC = 0;
                        prD = 0;
                        drG = 0;
                        drC = 0;
                        drD = 0;
                        //on actualise et sauvergarde les valeurs longueurs maxi en fonction de la banche selectionnee
                        #region lecture et stockage des valeurs maxi
                        lgMaxM3 = arraymaxi[h.bancheComboBox.SelectedIndex, 12];
                        leMaxM3 = arraymaxi[h.bancheComboBox.SelectedIndex, 13];
                        cMaxM3 = arraymaxi[h.bancheComboBox.SelectedIndex, 10];
                        dMaxM3 = arraymaxi[h.bancheComboBox.SelectedIndex, 14];
                        lgFixeM3 = 330;


                        lgMaxM2 = arraymaxi[h.bancheComboBox.SelectedIndex, 7];
                        leMaxM2 = arraymaxi[h.bancheComboBox.SelectedIndex, 8];
                        cMaxM2 = arraymaxi[h.bancheComboBox.SelectedIndex, 5];
                        dMaxM2 = arraymaxi[h.bancheComboBox.SelectedIndex, 9];
                        lgFixeM2 = 180;

                        lgMaxM1 = arraymaxi[h.bancheComboBox.SelectedIndex, 2];
                        leMaxM1 = arraymaxi[h.bancheComboBox.SelectedIndex, 3];
                        cMaxM1 = arraymaxi[h.bancheComboBox.SelectedIndex, 0];
                        dMaxM1 = arraymaxi[h.bancheComboBox.SelectedIndex, 4];
                        lgFixeM1 = 70;

                        #endregion lecture et stockage des valeurs maxi

                        #region zéro angles
                        if ((h.angleGaucheCheckBox.Checked == false) && (h.angleDroitCheckBox.Checked == false))
                        {
                            #region Cas 1 //Rien
                            if ((0 <= lgMurInt) && (lgMurInt < 100)) //passerelle impossible
                            {
                                h.option1button.Visible = false;
                                h.option2button.Visible = false;
                                h.option3button.Visible = false;
                                TaskDialog.Show("Attention", "Le voile est trop court pour installer une passerelle");
                            }
                            #endregion Cas 1
                            #region Cas 2 //M1
                            else if ((100 <= lgMurInt) && (lgMurInt <= 200)) //place pour un M1 seulement
                            {
                                h.option1button.Visible = false;
                                h.option2button.Visible = false;
                                h.option3button.Visible = false;
                                TaskDialog.Show("Information", "Seule une M1 est possible");

                                pR = 1;
                                prfs = m1FS;
                                prC = 70;
 
                                if (lgMurInt % 10== 0)
                                {
                                    prG = ((int)((lgMurInt - prC) / 2)/5)*5;
                                    prD = ((int)((lgMurInt - prC) / 2)/5)*5;
                                }
                                else
                                {
                                    prG = ((int)((lgMurInt - prC) / 2)/5)*5;
                                    prD = ((int)((lgMurInt - prC) / 2)/5)*5 + 5;
                                }

                            }
                            #endregion Cas 2
                            #region Cas 3 //M1 //M1+M1
                            else if ((205 <= lgMurInt) && (lgMurInt <= 215))
                            {
                                h.option1button.Visible = true;
                                h.option2button.Visible = true;
                                h.option3button.Visible = false;
                                h.option1button.Text = "M1";
                                h.option2button.Text = "M1+M1";
                                if (h.option1CheckBox.Checked == true)
                                {
                                    pR = 1;
                                    prfs = m1FS;
                                    prC = 70;
                                    if ((lgMurInt - prC) % 10 == 0)
                                    {
                                        prG = ((int)((lgMurInt - prC) / 2)/5)*5;
                                        prD = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                    }
                                    else
                                    {
                                        prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                        prD = ((int)((lgMurInt - prC) / 2) / 5) * 5 + 5;
                                    }
                                }


                                else if (h.option2CheckBox.Checked == true)
                                {
                                    pR = 1;
                                    prfs = m1FS;
                                    prC = 70;
                                    dR = 1;
                                    drfs = m1FS;
                                    drC = 70;
                                    if ((lgMurInt - (prC + drC+5)) % 20 == 0)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 15)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 10)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 5)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                }//option 2

                            }
                            #endregion Cas 3
                            #region Cas 4 //M1 //M1+M1 //M2
                            else if (lgMurInt == 220)
                            {
                                h.option1button.Visible = true;
                                h.option2button.Visible = true;
                                h.option3button.Visible = true;
                                h.option1button.Text = "M1";
                                h.option2button.Text = "M1+M1";
                                h.option3button.Text = "M2";
                                if (h.option1CheckBox.Checked == true)
                                {
                                    pR = 1;
                                    prfs = m1FS;
                                    prC = 70;

                                    if ((lgMurInt - prC) % 10 == 00)
                                    {
                                        prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                        prD = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                    }
                                    else
                                    {
                                        prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                        prD = ((int)((lgMurInt - prC) / 2) / 5) * 5 + 5;
                                    }
                                }//option 1
                                else if (h.option2CheckBox.Checked == true)
                                {
                                    pR = 1;
                                    prfs = m1FS;
                                    prC = 70;
                                    dR = 1;
                                    drfs = m1FS;
                                    drC = 70;
                                    if ((lgMurInt - (prC + drC+5)) % 20 == 0)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 15)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 10)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 5)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                }//option 2
                                if (h.option3CheckBox.Checked == true)
                                {
                                    pR = 1;
                                    prfs = m2FS;
                                    prC = 180;
                                    if ((lgMurInt - prC) % 10 == 00)
                                    {
                                        prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                        prD = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                    }
                                    else
                                    {
                                        prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                        prD = ((int)((lgMurInt - prC) / 2) / 5) * 5 + 5;
                                    }
                                }//option 3
                            }
                            #endregion Cas 4
                            #region Cas 5 //M2 //M1+M1
                            else if ((225 <= lgMurInt) && (lgMurInt <= 300))
                            {
                                h.option1button.Visible = true;
                                h.option2button.Visible = true;
                                h.option3button.Visible = false;
                                h.option1button.Text = "M2";
                                h.option2button.Text = "M1+M1";
                                if (h.option1CheckBox.Checked == true)
                                {
                                    pR = 1;
                                    prfs = m2FS;
                                    prC = 180;
                                    if ((lgMurInt - prC) % 10 == 00)
                                    {
                                        prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                        prD = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                    }
                                    else
                                    {
                                        prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                        prD = ((int)((lgMurInt - prC) / 2) / 5) * 5 + 5;
                                    }
                                }//option 1
                                else if (h.option2CheckBox.Checked == true)
                                {
                                    pR = 1;
                                    prfs = m1FS;
                                    prC = 70;
                                    dR = 1;
                                    drfs = m1FS;
                                    drC = 70;
                                    if ((lgMurInt - (prC + drC+5)) % 20 == 0)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 15)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 10)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 5)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                }//option 2

                            }
                            #endregion Cas 5
                            #region Cas 6 //M2 //M1+M1 //M1+M2
                            else if ((305 <= lgMurInt) && (lgMurInt <= 370))
                            {
                                h.option1button.Visible = true;
                                h.option2button.Visible = true;
                                h.option3button.Visible = true;
                                h.option1button.Text = "M2";
                                h.option2button.Text = "M1 + M1";
                                h.option3button.Text = "M1+M2";
                                if (h.option1CheckBox.Checked == true)
                                {
                                    pR = 1;
                                    prfs = m2FS;
                                    prC = 180;
                                    if ((lgMurInt - prC) % 10 == 00)
                                    {
                                        prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                        prD = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                    }
                                    else
                                    {
                                        prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                        prD = ((int)((lgMurInt - prC) / 2) / 5) * 5 + 5;
                                    }
                                }//option 1
                                else if (h.option2CheckBox.Checked == true)
                                {
                                    pR = 1;
                                    prfs = m1FS;
                                    prC = 70;
                                    dR = 1;
                                    drfs = m1FS;
                                    drC = 70;
                                    if ((lgMurInt - (prC + drC+5)) % 20 == 0)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 15)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 10)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 5)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                }//option 2

                                else if (h.option3CheckBox.Checked == true)
                                {
                                    
                                    pR = 1;
                                    prfs = m1FS;
                                    prC = 70;
                                    dR = 1;
                                    drfs = m2FS;
                                    drC = 180;
                                    if ((lgMurInt - (prC + drC+5)) % 20 == 0)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 15)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 10)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 5)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                }//option 3
                            }
                            #endregion Cas 6
                            #region Cas 7 //M2 //M3
                            else if (lgMurInt == 370)
                            {
                                h.option1button.Visible = true;
                                h.option2button.Visible = true;
                                h.option3button.Visible = false;
                                h.option1button.Text = "M2";
                                h.option2button.Text = "M3";
                                if (h.option1CheckBox.Checked == true)
                                {
                                    pR = 1;
                                    prfs = m2FS;
                                    prC = 180;
                                    if ((lgMurInt - prC) % 10 == 00)
                                    {
                                        prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                        prD = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                    }
                                    else
                                    {
                                        prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                        prD = ((int)((lgMurInt - prC) / 2) / 5) * 5 + 5;
                                    }
                                }//option 1
                                if (h.option2CheckBox.Checked == true)
                                {
                                    pR = 1;
                                    prfs = m3FS;
                                    prC = 330;
                                    if ((lgMurInt - prC) % 10 == 00)
                                    {
                                        prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                        prD = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                    }
                                    else
                                    {
                                        prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                        prD = ((int)((lgMurInt - prC) / 2) / 5) * 5 + 5;
                                    }
                                }//option 2
                            }
                            #endregion Cas 7
                            #region Cas 8 //M3  //M1+M2
                            else if ((370 < lgMurInt) && (lgMurInt < 445))
                            {
                                h.option1button.Visible = true;
                                h.option2button.Visible = true;
                                h.option3button.Visible = false;
                                h.option1button.Text = "M3";
                                h.option2button.Text = "M1 + M2";
                                if (h.option1CheckBox.Checked == true)
                                {
                                    pR = 1;
                                    prfs = m3FS;
                                    prC = 330;
                                    if ((lgMurInt - prC) % 10 == 00)
                                    {
                                        prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                        prD = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                    }
                                    else
                                    {
                                        prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                        prD = ((int)((lgMurInt - prC) / 2) / 5) * 5 + 5;
                                    }
                                }//option 1
                                else if (h.option2CheckBox.Checked == true)
                                {
                                    pR = 1;
                                    prfs = m1FS;
                                    prC = 70;
                                    dR = 1;
                                    drfs = m2FS;
                                    drC = 180;
                                    if ((lgMurInt - (prC + drC+5)) % 20 == 0)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 15)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 10)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 5)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                }//option 2

                            }
                            #endregion Cas 8
                            #region Cas 9 //M3 //M2+M2
                            else if (lgMurInt == 445)
                            {
                                h.option1button.Visible = true;
                                h.option2button.Visible = true;
                                h.option3button.Visible = false;
                                h.option1button.Text = "M3";
                                h.option2button.Text = "M2 + M2";
                                if (h.option1CheckBox.Checked == true)
                                {
                                    pR = 1;
                                    prfs = m3FS;
                                    prC = 330;
                                    if ((lgMurInt - prC) % 10 == 00)
                                    {
                                        prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                        prD = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                    }
                                    else
                                    {
                                        prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                        prD = ((int)((lgMurInt - prC) / 2) / 5) * 5 + 5;
                                    }
                                }//option 1
                                else if (h.option2CheckBox.Checked == true)
                                {
                                    pR = 1;
                                    prfs = m2FS;
                                    prC = 180;
                                    dR = 1;
                                    drfs = m2FS;
                                    drC = 180;
                                    if ((lgMurInt - (prC + drC+5)) % 20 == 0)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 15)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 10)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 5)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                }//option 2

                            }
                            #endregion Cas 9
                            #region Cas 10 //M3 //M2+M2
                            else if ((445 < lgMurInt) && (lgMurInt < 475))
                            {
                                h.option1button.Visible = true;
                                h.option2button.Visible = true;
                                h.option3button.Visible = false;
                                h.option1button.Text = "M3";
                                h.option2button.Text = "M2 + M2";
                                if (h.option1CheckBox.Checked == true)
                                {
                                    pR = 1;
                                    prfs = m3FS;
                                    prC = 330;
                                    if ((lgMurInt - prC) % 10 == 00)
                                    {
                                        prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                        prD = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                    }
                                    else
                                    {
                                        prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                        prD = ((int)((lgMurInt - prC) / 2) / 5) * 5 + 5;
                                    }
                                }//option 1
                                else if (h.option2CheckBox.Checked == true)
                                {
                                    pR = 1;
                                    prfs = m2FS;
                                    prC = 180;
                                    dR = 1;
                                    drfs = m2FS;
                                    drC = 180;
                                    if ((lgMurInt - (prC + drC+5)) % 20 == 0)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 15)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 10)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 5)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                }//option 2
                            }
                            #endregion Cas 10
                            #region Cas 11 // M3 //M2+M2 ou //M2+M2
                            else if ((475 <= lgMurInt) && (lgMurInt <= 590))
                            {
                                if (lgMaxM3 >= lgMurInt)
                                {
                                    h.option1button.Visible = true;
                                    h.option2button.Visible = true;
                                    h.option3button.Visible = false;
                                    h.option1button.Text = "M3";
                                    h.option2button.Text = "M2 + M2";
                                    if (h.option1CheckBox.Checked == true)
                                    {
                                        pR = 1;
                                        prfs = m3FS;
                                        prC = 330;
                                        if ((lgMurInt - prC) % 10 == 00)
                                        {
                                            prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                            prD = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                        }
                                        else
                                        {
                                            prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                            prD = ((int)((lgMurInt - prC) / 2) / 5) * 5 + 5;
                                        }
                                    }//option 1
                                    else if (h.option2CheckBox.Checked == true)
                                    {
                                        pR = 1;
                                        prfs = m2FS;
                                        prC = 180;
                                        dR = 1;
                                        drfs = m2FS;
                                        drC = 180;
                                        if ((lgMurInt - (prC + drC+5)) % 20 == 0)
                                        {
                                            prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        }
                                        else if ((lgMurInt - (prC + drC+5)) % 20 == 15)
                                        {
                                            prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                            drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                            drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        }
                                        else if ((lgMurInt - (prC + drC+5)) % 20 == 10)
                                        {
                                            prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                            drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        }
                                        else if ((lgMurInt - (prC + drC+5)) % 20 == 5)
                                        {
                                            prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        }
                                    }//option 2

                                }
                                else
                                {
                                    h.option1button.Visible = true;
                                    h.option2button.Visible = false;
                                    h.option3button.Visible = false;
                                    h.option1button.Text = "M2+M2";

                                    pR = 1;
                                    prfs = m2FS;
                                    prC = 180;
                                    dR = 1;
                                    drfs = m2FS;
                                    drC = 180;
                                    if ((lgMurInt - (prC + drC+5)) % 20 == 0)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 15)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 10)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 5)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                }
                            }
                            #endregion Cas 11
                            #region Cas 12 //M3 //M2+M2 ou //M2
                            else if ((590 < lgMurInt) && (lgMurInt < 595))
                            {
                                if (lgMaxM3 >= lgMurInt)
                                {
                                    h.option1button.Visible = true;
                                    h.option2button.Visible = true;
                                    h.option3button.Visible = false;
                                    h.option1button.Text = "M3";
                                    h.option2button.Text = "M2 + M2";
                                    if (h.option1CheckBox.Checked == true)
                                    {
                                        pR = 1;
                                        prfs = m3FS;
                                        prC = 330;
                                        if ((lgMurInt - prC) % 10 == 00)
                                        {
                                            prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                            prD = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                        }
                                        else
                                        {
                                            prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                            prD = ((int)((lgMurInt - prC) / 2) / 5) * 5 + 5;
                                        }
                                    }//option 1
                                    else if (h.option2CheckBox.Checked == true)
                                    {
                                        pR = 1;
                                        prfs = m2FS;
                                        prC = 180;
                                        dR = 1;
                                        drfs = m2FS;
                                        drC = 180;
                                        if ((lgMurInt - (prC + drC+5)) % 20 == 0)
                                        {
                                            prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        }
                                        else if ((lgMurInt - (prC + drC+5)) % 20 == 15)
                                        {
                                            prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                            drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                            drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        }
                                        else if ((lgMurInt - (prC + drC+5)) % 20 == 10)
                                        {
                                            prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                            drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        }
                                        else if ((lgMurInt - (prC + drC+5)) % 20 == 5)
                                        {
                                            prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        }
                                    }//option 2
                                }
                                else
                                {

                                    h.option1button.Visible = true;
                                    h.option2button.Visible = false;
                                    h.option3button.Visible = false;
                                    h.option1button.Text = "M2";

                                    pR = 1;
                                    prfs = m2FS;
                                    prC = 180;
                                    if ((lgMurInt - prC) % 10 == 00)
                                    {
                                        prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                        prD = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                    }
                                    else
                                    {
                                        prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                        prD = ((int)((lgMurInt - prC) / 2) / 5) * 5 + 5;
                                    }
                                }
                            }
                            #endregion Cas 12
                            #region Cas 13 //M3 //M2+M2 ou //M2+M2 // M3+M2
                            else if ((595 <= lgMurInt) && (lgMurInt <= 650))
                            {
                                if (lgMaxM3 >= lgMurInt)
                                {
                                    h.option1button.Visible = true;
                                    h.option2button.Visible = true;
                                    h.option3button.Visible = false;
                                    h.option1button.Text = "M3";
                                    h.option2button.Text = "M2 + M2";
                                    if (h.option1CheckBox.Checked == true)
                                    {
                                        pR = 1;
                                        prfs = m3FS;
                                        prC = 330;
                                        if ((lgMurInt - prC) % 10 == 00)
                                        {
                                            prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                            prD = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                        }
                                        else
                                        {
                                            prG = ((int)((lgMurInt - prC) / 2) / 5) * 5;
                                            prD = ((int)((lgMurInt - prC) / 2) / 5) * 5 + 5;
                                        }
                                    }//option 1
                                    else if (h.option2CheckBox.Checked == true)
                                    {
                                        pR = 1;
                                        prfs = m2FS;
                                        prC = 180;
                                        dR = 1;
                                        drfs = m2FS;
                                        drC = 180;
                                        if ((lgMurInt - (prC + drC+5)) % 20 == 0)
                                        {
                                            prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        }
                                        else if ((lgMurInt - (prC + drC+5)) % 20 == 15)
                                        {
                                            prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                            drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                            drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        }
                                        else if ((lgMurInt - (prC + drC+5)) % 20 == 10)
                                        {
                                            prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                            drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        }
                                        else if ((lgMurInt - (prC + drC+5)) % 20 == 5)
                                        {
                                            prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        }
                                    }//option 2
                                }
                                else
                                {
                                    h.option1button.Visible = true;
                                    h.option2button.Visible = true;
                                    h.option3button.Visible = false;
                                    h.option1button.Text = "M2 + M2";
                                    h.option2button.Text = "M3 + M2";
                                    if (h.option1CheckBox.Checked == true)
                                    {
                                        pR = 1;
                                        prfs = m2FS;
                                        prC = 180;
                                        dR = 1;
                                        drfs = m2FS;
                                        drC = 180;
                                        if ((lgMurInt - (prC + drC+5)) % 20 == 0)
                                        {
                                            prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        }
                                        else if ((lgMurInt - (prC + drC+5)) % 20 == 15)
                                        {
                                            prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                            drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                            drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        }
                                        else if ((lgMurInt - (prC + drC+5)) % 20 == 10)
                                        {
                                            prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                            drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        }
                                        else if ((lgMurInt - (prC + drC+5)) % 20 == 5)
                                        {
                                            prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        }
                                    }//option 1
                                    if (h.option2CheckBox.Checked == true)
                                    {
                                        pR = 1;
                                        prfs = m3FS;
                                        prC = 330;
                                        dR = 1;
                                        drfs = m2FS;
                                        drC = 180;
                                        if ((lgMurInt - (prC + drC+5)) % 20 == 0)
                                        {
                                            prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        }
                                        else if ((lgMurInt - (prC + drC+5)) % 20 == 15)
                                        {
                                            prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                            drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                            drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        }
                                        else if ((lgMurInt - (prC + drC+5)) % 20 == 10)
                                        {
                                            prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                            drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        }
                                        else if ((lgMurInt - (prC + drC+5)) % 20 == 5)
                                        {
                                            prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                            drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        }
                                    }//option 2
                                }
                            }
                            #endregion Cas 13
                            #region Cas 14 //M2+M2 // M3+M2
                            else if ((655 <= lgMurInt) && (lgMurInt <= 740))
                            {
                                h.option1button.Visible = true;
                                h.option2button.Visible = true;
                                h.option3button.Visible = false;
                                h.option1button.Text = "M2 + M2";
                                h.option2button.Text = "M3 + M2";
                                if (h.option1CheckBox.Checked == true)
                                {
                                    pR = 1;
                                    prfs = m2FS;
                                    prC = 180;
                                    dR = 1;
                                    drfs = m2FS;
                                    drC = 180;
                                    if ((lgMurInt - (prC + drC+5)) % 20 == 0)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 15)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 10)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 5)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                }//option 1
                                if (h.option2CheckBox.Checked == true)
                                {
                                    pR = 1;
                                    prfs = m3FS;
                                    prC = 330;
                                    dR = 1;
                                    drfs = m2FS;
                                    drC = 180;
                                    if ((lgMurInt - (prC + drC+5)) % 20 == 0)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 15)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 10)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 5)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                }//option 2
                            }
                            #endregion Cas 14
                            #region Cas 15 //M2+M2 //M3+M2 //M3+M3
                            else if (lgMurInt == 745)
                            {
                                h.option1button.Visible = true;
                                h.option2button.Visible = true;
                                h.option3button.Visible = true;
                                h.option1button.Text = "M2 + M2";
                                h.option2button.Text = "M3 + M2";
                                h.option3button.Text = "M3 + M3";
                                if (h.option1CheckBox.Checked == true)
                                {
                                    pR = 1;
                                    prfs = m2FS;
                                    prC = 180;
                                    dR = 1;
                                    drfs = m2FS;
                                    drC = 180;
                                    if ((lgMurInt - (prC + drC+5)) % 20 == 0)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 15)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 10)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 5)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                }//option 1
                                else if (h.option2CheckBox.Checked == true)
                                {
                                    pR = 1;
                                    prfs = m3FS;
                                    prC = 330;
                                    dR = 1;
                                    drfs = m2FS;
                                    drC = 180;
                                    if ((lgMurInt - (prC + drC+5)) % 20 == 0)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 15)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 10)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 5)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                }//option 2
                                else if (h.option3CheckBox.Checked == true)
                                {
                                    pR = 1;
                                    prfs = m3FS;
                                    prC = 330;
                                    dR = 1;
                                    drfs = m3FS;
                                    drC = 330;
                                    if ((lgMurInt - (prC + drC+5)) % 20 == 0)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 15)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 10)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 5)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                }//option 3
                            }
                            #endregion Cas 15
                            #region Cas 16 //M2+M2 //M3+M3
                            else if ((750 <= lgMurInt) && (lgMurInt <= lgMaxM3 + 220 + 5))
                            {
                                h.option1button.Visible = true;
                                h.option2button.Visible = true;
                                h.option3button.Visible = false;
                                h.option1button.Text = "M3 + M2";
                                h.option2button.Text = "M3 + M3";
                                if (h.option1CheckBox.Checked == true)
                                {
                                    pR = 1;
                                    prfs = m3FS;
                                    prC = 330;
                                    dR = 1;
                                    drfs = m2FS;
                                    drC = 180;
                                    if ((lgMurInt - (prC + drC+5)) % 20 == 0)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 15)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 10)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 5)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                }//option 1
                                else if (h.option2CheckBox.Checked == true)
                                {
                                    pR = 1;
                                    prfs = m3FS;
                                    prC = 330;
                                    dR = 1;
                                    drfs = m3FS;
                                    drC = 330;
                                    if ((lgMurInt - (prC + drC+5)) % 20 == 0)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 15)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 10)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                    else if ((lgMurInt - (prC + drC+5)) % 20 == 5)
                                    {
                                        prG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        prD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drG = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5;
                                        drD = ((int)((lgMurInt - (prC + drC+5)) / 4) / 5) * 5 + 5;
                                    }
                                }//option 2
                            }
                            #endregion Cas 16
                            #region Cas 17 //M3+M3
                            else if ((lgMaxM3 + 370 + 5 < lgMurInt) && (lgMurInt <= 2 * lgMaxM3 + 5))
                            {
                                h.option1button.Visible = true;
                                h.option2button.Visible = false;
                                h.option3button.Visible = false;
                                h.option1button.Text = "M3 + M3";
                                pR = 1;
                                prfs = m3FS;
                                prC = 330;
                                dR = 1;
                                drfs = m3FS;
                                drC = 330;
                                if ((lgMurInt - (prC + drC + 5)) % 20 == 0)
                                {
                                    prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                }
                                else if ((lgMurInt - (prC + drC + 5)) % 20 == 15)
                                {
                                    prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                    drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                    drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                }
                                else if ((lgMurInt - (prC + drC + 5)) % 20 == 10)
                                {
                                    prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                    drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                }
                                else if ((lgMurInt - (prC + drC + 5)) % 20 == 5)
                                {
                                    prG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    prD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    drG = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5;
                                    drD = ((int)((lgMurInt - (prC + drC + 5)) / 4) / 5) * 5 + 5;
                                }
                            }
                            #endregion Cas 17
                            #region Cas 18 Reste du monde
                            else
                            {
                                h.option1button.Visible = false;
                                h.option2button.Visible = false;
                                h.option3button.Visible = false;
                                //Première boucle
                                //on regarde combien de passerelles M3max-40 on peut insérer, exclues les passerelles de départ et d'arrivée
                                //nM3 = ((int)(lgMurInt - ((prG + prC + prD) + (drG + drC + drD)))) / (lgMaxM3 - 40);
                                //r1 = ((int)(lgMurInt - ((prG + prC + prD) + (drG + drC + drD)))) % (lgMaxM3 - 40) - (nM3 + pR + dR) * 5;
                                nM3 = ((int)(lgMurInt)) / (lgMaxM3 - 40);
                                r1 = ((int)(lgMurInt)) % (lgMaxM3 - 40) - (nM3) * 5;

                                //on regarde ce qu'on peut mettre à la fin
                                if (r1 > (lgMaxM2))
                                {
                                    nM3bis = 1;
                                    lM3bis = (int)(r1 / 5) * 5;
                                    //insert une M3 de longueur r1
                                }
                                else if (r1 > lgMaxM1 & r1 < lgMaxM2)
                                {
                                    nM2 = 1;
                                    lM2 = (int)(r1 / 5) * 5;
                                    //insert M2 longueur r1
                                }
                                else
                                {
                                    //on enleve une passerelle M3 et on a un nouveau reste
                                    r2 = (lgMaxM3-40) + r1 - 370;
                                    nM3 = nM3 - 1;

                                    nM3bis = 1;
                                    lM3bis = 370;
                                    //insert M3min puis complete

                                    if (r2 > lgMaxM1 & r2 < lgMaxM2)
                                    {
                                        nM2bis = 1;
                                        lM2bis = (int)(r2 / 5) * 5;
                                        //insert M2 de longueur r2

                                    }
                                    else
                                    {
                                        nM3ter = 1;
                                        lM3ter = (int)(r2 / 5) * 5;
                                        //insert M3 de longueur r2
                                    }
                                }
                            }
                            #endregion Reste du monde
                        }//if pas d'angles
                        #endregion pas d'angles
                        #region deux angles
                        else if ((h.angleGaucheCheckBox.Checked == true) && (h.angleDroitCheckBox.Checked == true))//les deux angles sont cochés
                        {
                            if ((0 <= lgMurInt) && (lgMurInt < 220))
                            {
                                h.option1button.Visible = false;
                                h.option2button.Visible = false;
                                h.option3button.Visible = false;
                                TaskDialog.Show("Information", "Le voile est trop court, impossible de placer une passerelle avec un angle");
                            }
                            else if ((220 <= lgMurInt) && (lgMurInt < 370))
                            {
                                h.option1button.Visible = false;
                                h.option2button.Visible = false;
                                h.option3button.Visible = false;
                                TaskDialog.Show("Attention", "Consulter SATECO, informations à donner :" + "\n" + "Cas d'une passerelle M2 avec deux retours d'angle");
                            }
                            else
                            {
                                h.option1button.Visible = false;
                                h.option2button.Visible = false;
                                h.option3button.Visible = false;
                            }

                        }//deux angles cochés
                        #endregion deux angles
                        #region un des deux angles est coché
                        else // un des deux angles est coché
                        {
                            if ((0 <= lgMurInt) && (lgMurInt < 220))
                            {
                                h.option1button.Visible = false;
                                h.option2button.Visible = false;
                                h.option3button.Visible = false;
                                TaskDialog.Show("Information", "Le voile est trop court, impossible de placer une passerelle avec un angle");
                            }
                            else if ((220 <= lgMurInt) && (lgMurInt < 300))
                            {
                                h.option1button.Visible = false;
                                h.option2button.Visible = false;
                                h.option3button.Visible = false;
                                TaskDialog.Show("Attention", "Consulter SATECO, informations à donner :" + "\n" + "Cas d'une passerelle M2 avec un retour d'angle et une longueur de plateforme inférieure à 300cm");
                            }
                            else if ((300 <= lgMurInt) && (lgMurInt <= 330)) //M2 avec un angle
                            {
                                h.option1button.Visible = false;
                                h.option2button.Visible = false;
                                h.option3button.Visible = false;

                            }
                            else if ((330 < lgMurInt) && (lgMurInt < 370))
                            {
                                h.option1button.Visible = false;
                                h.option2button.Visible = false;
                                h.option3button.Visible = false;
                                TaskDialog.Show("Attention", "Consulter SATECO et la documentation :" + "\n" + "Montage d'une M2 avec des traverses extensibles de M3");
                            }
                            else if ((370 <= lgMurInt) && (lgMurInt < 475))//475=M3min+M1min+5
                            {
                                //M3seule
                                h.option1button.Visible = false;
                                h.option2button.Visible = false;
                                h.option3button.Visible = false;
                            }
                            else if ((475 <= lgMurInt) && (lgMurInt < leMaxM3 + 330 + 110))//475=M3min+M1min+5
                            {
                                if (leMaxM3 + 330 + 110 < 595)//595=M3min+M2min+5
                                {
                                    //M3 seule ou M3+M1
                                    h.option1button.Text = "M3";
                                    h.option2button.Text = "M3 + M1";
                                    h.option1button.Visible = true;
                                    h.option2button.Visible = true;
                                    h.option3button.Visible = false;
                                }
                                if (leMaxM3 + 330 + 110 >= 595)
                                {
                                    //M3seule ou M3+M1 ou M3+M2
                                    h.option1button.Text = "M3";
                                    h.option2button.Text = "M3 + M1";
                                    h.option3button.Text = "M3 + M2";
                                    h.option1button.Visible = true;
                                    h.option2button.Visible = true;
                                    h.option3button.Visible = true;
                                }
                            }
                            else if ((600 <= lgMurInt) && (lgMurCurve <= leMaxM3 + 330 + 110 + 220 + 5)) //600=M3plus grand max
                            {
                                //M3+M1 ou M3+M2
                                h.option1button.Text = "M3 + M1";
                                h.option2button.Text = "M3 + M2";
                                h.option1button.Visible = true;
                                h.option2button.Visible = true;
                                h.option3button.Visible = false;
                            }
                            else if ((leMaxM3 + 330 + 110 + 220 + 5 <= lgMurInt) && (lgMurCurve <= leMaxM3 + 330 + 110 + 370 + 5)) //entre M3max+M2min et M3max+M2min
                            {
                                //M3+M2
                                h.option1button.Visible = false;
                                h.option2button.Visible = false;
                                h.option3button.Visible = false;
                            }
                        }//un angle coché
                        #endregion un des deux angles est coché

                    };
                    #endregion processus

                    #region Excel
                    /*
                        h.applyButton.Click += (sender, e) =>
                        {
                            //premiere boucle
                            //on regarde les longueurs maxi autorisées et on les garde en mémoire
                            lM1 = (int)xlRange.Cells[hauteurBancheIndex, 4].Value();
                            lM2 = (int)xlRange.Cells[hauteurBancheIndex, 9].Value();
                            lM3 = (int)xlRange.Cells[hauteurBancheIndex, 14].Value();

                            //on regarde combien de passerelles M3max-40 on peut insérer, exclues les passerelles de départ et d'arrivée
                            //les passerelles de départ et d'arrivée font ext 100 + partie fixe 330 + ext (longeur max - partie fixe)/2
                            nM3 = ((int)(lgMurCurve - 2 * (100 + 330 + (lM3 - 330) / 2))) / lM3;
                            r1 = ((int)(lgMurCurve - 2 * (100 + 330 + (lM3 - 330) / 2)) % lM3) - (nM3 + 2) * 5;
                            //nM3+2 car il y a les deux passerelles de départ et d'arrivée


                            //on regarde ce qu'on peut mettre à la fin
                            if (r1 > lM2)
                            {
                                nM3bis = 1;
                                lM3bis = (int)(r1 / 5) * 5;

                                //insert une M3 de longueur r1
                            }
                            else if (r1 > lM1 & r1 < lM2)
                            {
                                nM2 = 1;
                                lM2 = (int)(r1 / 5) * 5;
                                //insert M2 longueur r1
                            }

                            /*
                             finalement on ne met pas de M1
                             else if (r1 < lM1 & r1 > 100)
                            {
                                nM1 = 1;
                                lM1 = (r1 / 5) * 5;
                                ///insert M1 longueur r1
                            }
                                */
                    /*
                    else
                    {
                        //on enleve une passerelle M3 et on a un nouveau reste
                        r2 = lM3 + r1 - 370;
                        nM3 = nM3 - 1;

                        nM3bis = 1;
                        lM3bis = 370;
                        //insert M3min puis complete

                        /*
                         toujours pas de M1
                         if (r2 < lM1 & r2 > 100)
                         {
                             nM1bis = 1;
                             lM1bis = (r2 / 5) * 5;
                             //insert M1 de longueur r2
                         }
                             */
                    /*
                            if (r2 > lM1 & r2 < lM2)
                            {
                                nM2bis = 1;
                                lM2bis = (int)(r2 / 5) * 5;
                                //insert M2 de longueur r2

                            }
                            else
                            {
                                nM3ter = 1;
                                lM3ter = (int)(r2 / 5) * 5;
                                //insert M3 de longueur r2
                            }
                        };

                        h.Close();
                    };
                */
                    #endregion //excel

                    h.applyButton.Click += (sender, e) =>
                        {
                            #region initialisation insert
                            //on ne sait pas trop à quoi ça sert mais il le faut apparemment
                            FamilyInstance placement = null;
                            Autodesk.Revit.DB.Structure.StructuralType nonStructural = Autodesk.Revit.DB.Structure.StructuralType.NonStructural;
                            #endregion initialisation insert
                            #region angle de rotation+ angle trigo
                            //cela sert à dire à revit d'utiliser le bon angle 
                            //angle de rotation
                            Double angleTrigo = new double();

                            if (pointDebut.X < pointFin.X)
                            {
                                angleTrigo = Math.Atan((pointFin.Y - pointDebut.Y) / (pointFin.X - pointDebut.X));
                            }
                            if (pointDebut.X > pointFin.X)
                            {
                                angleTrigo = Math.Atan((pointFin.Y - pointDebut.Y) / (pointFin.X - pointDebut.X)) + Math.PI;
                            }
                            if (pointDebut.X == pointFin.X)
                            {
                                if (pointDebut.Y < pointFin.Y)
                                {
                                    angleTrigo = 3 * Math.PI / 2;
                                }
                                else angleTrigo = Math.PI / 2;
                            }
                            Double anglerotation = new Double();
                            if (pointFin.Y > pointDebut.Y)
                            {
                                if (pointFin.X > pointDebut.X) //cadran 1
                                {
                                    anglerotation = Math.Atan((pointFin.Y - pointDebut.Y) / (pointFin.X - pointDebut.X));
                                }
                                else //cadran 2
                                {
                                    anglerotation = Math.PI + Math.Atan((pointFin.Y - pointDebut.Y) / (pointFin.X - pointDebut.X));
                                }
                            }
                            else
                            {
                                if (pointFin.X < pointDebut.X) //cadran 3
                                {
                                    anglerotation = Math.PI + Math.Atan((pointFin.Y - pointDebut.Y) / (pointFin.X - pointDebut.X));
                                }
                                else //cadran 4
                                {
                                    anglerotation = Math.Atan((pointFin.Y - pointDebut.Y) / (pointFin.X - pointDebut.X));
                                }
                            }


                            #endregion Rotate
                            #region initialisation pointInsertionPass
                            //on va initialiser le premier point d'insertion de la passerelle de départ
                            //abscisse curviligne
                            Double l = new Double();
                            Double l2 = new Double();
                            if (pR == 1)
                            {
                                l = (prG + prC / 2) / feet;
                                l2 = 0;
                            }
                            else
                            {
                                //à vérifier
                                l = 0;
                                l2 = 0;
                            }

                            XYZ pointInsertionPass = null;
                            //abscisse
                            Double x = new Double();
                            x = pointDebut.X + l * Math.Cos(angleTrigo);

                            //ordonnée
                            Double y = new Double();
                            y = pointDebut.Y + l * Math.Sin(angleTrigo);

                            //altitude
                            Double z = new Double();
                            z = pointDebut.Z;

                            pointInsertionPass = new XYZ(x, y, z);
                            #endregion initialisation
                            #region on insere la passerelle de départ
                            if (pR==1)
                            {

                                #region Transaction Placement
                                Transaction placementTransaction = new Transaction(doc, "transaction placement");
                                {
                                    placementTransaction.Start();

                                    placement = doc.Create.NewFamilyInstance(pointInsertionPass, prfs, nonStructural);

                                    placementTransaction.Commit();
                                }
                                #endregion transaction placement

                                #region axe de rotation
                                //axe de rotation
                                XYZ a1 = (placement.Location as LocationPoint).Point;
                                XYZ a2 = new XYZ(a1.X, a1.Y, a1.Z + 10);
                                Autodesk.Revit.DB.Line axis = Autodesk.Revit.DB.Line.CreateBound(a1, a2);
                                #endregion axe rotation

                                #region Transaction rotate
                                Transaction rotationtransaction = new Transaction(doc, "RotationPasserelle");
                                {
                                    rotationtransaction.Start();
                                    ElementTransformUtils.RotateElement(doc, placement.Id, axis, anglerotation);

                                    rotationtransaction.Commit();
                                }
                                #endregion transaction rotate

                                #region Transaction parametres
                                Transaction parametresTransaction = new Transaction(doc, "parametresTransaction");
                                {
                                    parametresTransaction.Start();
                                    placement.LookupParameter("Longueur extension gauche interieure").SetValueString((prG).ToString());
                                    placement.LookupParameter("Longueur extension gauche").SetValueString((prG).ToString());
                                    placement.LookupParameter("Longueur extension droite interieure").SetValueString((prD).ToString());
                                    placement.LookupParameter("Longueur extension droite").SetValueString((prD).ToString());
                                    placement.LookupParameter(h.bancheComboBox.SelectedItem.ToString()).Set(vrai);
                                    placement.LookupParameter("Passerelle incorrecte").Set(vrai);
                                    parametresTransaction.Commit();
                                }
                                #endregion transaction parametres

                                //on actualise pour la suivante
                                l2 = (prG + prC / 2 + 5) / feet;
                            }
                            #endregion on insere la passerelle de départ
                            #region Première boucle centrale
                            #region boucle M3max
                            //boucle
                            int i = 1;
                            while (i <= nM3)
                            {
                                l = l + l2 + ((lgMaxM3-40) / 2) / feet;
                                x = pointDebut.X + l * Math.Cos(angleTrigo);
                                y = pointDebut.Y + l * Math.Sin(angleTrigo);
                                pointInsertionPass = new XYZ(x, y, z);

                                #region Transaction Placement
                                Transaction placementTransaction = new Transaction(doc, "transaction placement");
                                {
                                    placementTransaction.Start();
                                    placement = doc.Create.NewFamilyInstance(pointInsertionPass, m3FS, nonStructural);
                                    placementTransaction.Commit();
                                }
                                #endregion transaction placement
                                #region axe de rotation
                                //axe de rotation
                                XYZ a1 = (placement.Location as LocationPoint).Point;
                                XYZ a2 = new XYZ(a1.X, a1.Y, a1.Z + 10);
                                Autodesk.Revit.DB.Line axis = Autodesk.Revit.DB.Line.CreateBound(a1, a2);
                                #endregion axe rotation
                                #region Transaction rotate
                                Transaction rotationtransaction = new Transaction(doc, "RotationPasserelle");
                                {
                                    rotationtransaction.Start();
                                    ElementTransformUtils.RotateElement(doc, placement.Id, axis, anglerotation);
                                    rotationtransaction.Commit();
                                }

                                #endregion transaction rotate
                                #region Transaction parametres
                                Transaction parametresTransaction = new Transaction(doc, "parametresTransaction");
                                {
                                    parametresTransaction.Start();
                                    placement.LookupParameter("Longueur extension gauche").SetValueString(((lgMaxM3-40 - 330) / 2).ToString());
                                    placement.LookupParameter("Longueur extension gauche interieure").SetValueString(((lgMaxM3-40 - 330) / 2).ToString());
                                    placement.LookupParameter("Longueur extension droite").SetValueString(((lgMaxM3-40 - 330) / 2).ToString());
                                    placement.LookupParameter("Longueur extension droite interieure").SetValueString(((lgMaxM3-40 - 330) / 2).ToString());
                                    placement.LookupParameter(h.bancheComboBox.SelectedItem.ToString()).Set(vrai);
                                    placement.LookupParameter("Passerelle incorrecte").Set(vrai);
                                    parametresTransaction.Commit();
                                }
                                #endregion transaction parametres

                                l2 = ((lgMaxM3-40) / 2 + 5) / feet;
                                i = i + 1;
                            }
                            #endregion boucle M3Max
                            #region Boucle si nM3bis
                            if (nM3bis != 0)
                            {
                                l = l + l2 + ((int)(((lM3bis - 330) / 2) / 5) * 5) / feet + (330 / 2) / feet;
                                x = pointDebut.X + l * Math.Cos(angleTrigo);
                                y = pointDebut.Y + l * Math.Sin(angleTrigo);
                                pointInsertionPass = new XYZ(x, y, z);

                                #region Transaction Placement M3bis
                                Transaction placementTransactionM3bis = new Transaction(doc, "transaction placement");
                                {
                                    placementTransactionM3bis.Start();
                                    placement = doc.Create.NewFamilyInstance(pointInsertionPass, m3FS, nonStructural);
                                    placementTransactionM3bis.Commit();
                                }
                                #endregion transaction placement
                                #region axe de rotation M3bis
                                //axe de rotation
                                XYZ a1 = (placement.Location as LocationPoint).Point;
                                XYZ a2 = new XYZ(a1.X, a1.Y, a1.Z + 10);
                                Autodesk.Revit.DB.Line axis = Autodesk.Revit.DB.Line.CreateBound(a1, a2);
                                #endregion axe rotation
                                #region Transaction rotate M3bis
                                Transaction rotationtransactionM3bis = new Transaction(doc, "RotationPasserelle");
                                {
                                    rotationtransactionM3bis.Start();
                                    ElementTransformUtils.RotateElement(doc, placement.Id, axis, anglerotation);
                                    rotationtransactionM3bis.Commit();
                                }

                                #endregion transaction rotate M3bis
                                #region Transaction parametres M3bis
                                Transaction parametresTransactionM3bis = new Transaction(doc, "parametresTransaction");
                                {
                                    parametresTransactionM3bis.Start();
                                    placement.LookupParameter("Longueur extension gauche").SetValueString(((int)(((lM3bis - 330) / 2) / 5) * 5).ToString());
                                    placement.LookupParameter("Longueur extension gauche interieure").SetValueString(((int)(((lM3bis - 330) / 2) / 5) * 5).ToString());
                                    if (lM3bis % 10 == 0)
                                    {
                                        placement.LookupParameter("Longueur extension droite").SetValueString(((int)(((lM3bis - 330) / 2) / 5) * 5).ToString());
                                        placement.LookupParameter("Longueur extension droite interieure").SetValueString(((int)(((lM3bis - 330) / 2) / 5) * 5).ToString());
                                    }
                                    else
                                    {
                                        placement.LookupParameter("Longueur extension droite").SetValueString(((int)(((lM3bis - 330) / 2) / 5) * 5 + 5).ToString());
                                        placement.LookupParameter("Longueur extension droite interieure").SetValueString(((int)(((lM3bis - 330) / 2) / 5) * 5 + 5).ToString());
                                    }
                                    placement.LookupParameter(h.bancheComboBox.SelectedItem.ToString()).Set(vrai);
                                    placement.LookupParameter("Passerelle incorrecte").Set(vrai);
                                    parametresTransactionM3bis.Commit();
                                }
                                #endregion transaction parametres

                                l2 = ((int)(((lM3bis - 330) / 2) / 5) * 5 + 5 + 330 / 2) / feet;
                            }
                            #endregion nM3bis
                            #region Boucle si nM2
                            if (nM2 != 0)
                            {
                                l = l + l2 + ((int)(((lM2 - 180) / 2) / 5) * 5) / feet + (180 / 2) / feet;
                                x = pointDebut.X + l * Math.Cos(angleTrigo);
                                y = pointDebut.Y + l * Math.Sin(angleTrigo);
                                pointInsertionPass = new XYZ(x, y, z);

                                #region Transaction Placement M2
                                Transaction placementTransactionM2 = new Transaction(doc, "transaction placement");
                                {
                                    placementTransactionM2.Start();
                                    placement = doc.Create.NewFamilyInstance(pointInsertionPass, m2FS, nonStructural);
                                    placementTransactionM2.Commit();
                                }
                                #endregion transaction placement M2
                                #region axe de rotation M2
                                //axe de rotation
                                XYZ a1 = (placement.Location as LocationPoint).Point;
                                XYZ a2 = new XYZ(a1.X, a1.Y, a1.Z + 10);
                                Autodesk.Revit.DB.Line axis = Autodesk.Revit.DB.Line.CreateBound(a1, a2);
                                #endregion axe rotation M2
                                #region Transaction rotate M2
                                Transaction rotationtransactionM2 = new Transaction(doc, "RotationPasserelle");
                                {
                                    rotationtransactionM2.Start();
                                    ElementTransformUtils.RotateElement(doc, placement.Id, axis, anglerotation);
                                    rotationtransactionM2.Commit();
                                }

                                #endregion transaction rotate M2
                                #region Transaction parametres M2
                                Transaction parametresTransactionM2 = new Transaction(doc, "parametresTransaction");
                                {
                                    parametresTransactionM2.Start();
                                    placement.LookupParameter("Longueur extension gauche").SetValueString(((int)(((lM2 - 180) / 2) / 5) * 5).ToString());
                                    placement.LookupParameter("Longueur extension gauche interieure").SetValueString(((int)(((lM2 - 180) / 2) / 5) * 5).ToString());
                                    if (lM2 % 10 == 0)
                                    {
                                        placement.LookupParameter("Longueur extension droite").SetValueString(((int)(((lM2 - 180) / 2) / 5) * 5).ToString());
                                        placement.LookupParameter("Longueur extension droite interieure").SetValueString(((int)(((lM2 - 180) / 2) / 5) * 5).ToString());
                                    }
                                    else
                                    {
                                        placement.LookupParameter("Longueur extension droite").SetValueString(((int)(((lM2 - 180) / 2) / 5) * 5 + 5).ToString());
                                        placement.LookupParameter("Longueur extension droite interieure").SetValueString(((int)(((lM2 - 180) / 2) / 5) * 5 + 5).ToString());
                                    }
                                    placement.LookupParameter(h.bancheComboBox.SelectedItem.ToString()).Set(vrai);
                                    placement.LookupParameter("Passerelle incorrecte").Set(vrai);
                                    parametresTransactionM2.Commit();
                                }
                                #endregion transaction parametres M2

                                if (lM2 % 10 == 0)
                                {
                                    l2 = ((int)(((lM2 - 180) / 2) / 5) * 5 + 180 / 2) / feet;
                                }
                                else { l2 = ((int)(((lM2 - 180) / 2) / 5) * 5 + 5 + 180 / 2) / feet; }
                            }
                            #endregion M2
                            #endregion Première boucle centrale
                            #region Deuxième boucle
                            #region Boucle si nM2bis exite
                            if (nM2bis != 0)
                            {
                                l = l + l2 + ((((lM2bis - 180) / 2) / 5) * 5) / feet + (180 / 2) / feet;
                                x = pointDebut.X + l * Math.Cos(angleTrigo);
                                y = pointDebut.Y + l * Math.Sin(angleTrigo);
                                pointInsertionPass = new XYZ(x, y, z);

                                #region Transaction Placement M2bis
                                Transaction placementTransactionM2bis = new Transaction(doc, "transaction placement");
                                {
                                    placementTransactionM2bis.Start();
                                    placement = doc.Create.NewFamilyInstance(pointInsertionPass, m2FS, nonStructural);
                                    placementTransactionM2bis.Commit();
                                }
                                #endregion transaction placement M2bis
                                #region axe de rotation M2bis
                                //axe de rotation
                                XYZ a1 = (placement.Location as LocationPoint).Point;
                                XYZ a2 = new XYZ(a1.X, a1.Y, a1.Z + 10);
                                Autodesk.Revit.DB.Line axis = Autodesk.Revit.DB.Line.CreateBound(a1, a2);
                                #endregion axe rotation M2bis
                                #region Transaction rotate M2bis
                                Transaction rotationtransactionM2bis = new Transaction(doc, "RotationPasserelle");
                                {
                                    rotationtransactionM2bis.Start();
                                    ElementTransformUtils.RotateElement(doc, placement.Id, axis, anglerotation);
                                    rotationtransactionM2bis.Commit();
                                }
                                #endregion transaction rotate M2bis
                                #region Transaction parametres M2bis
                                Transaction parametresTransactionM2bis = new Transaction(doc, "parametresTransaction");
                                {
                                    parametresTransactionM2bis.Start();
                                    placement.LookupParameter("Longueur extension gauche").SetValueString(((int)(((lM2bis - 180) / 2) / 5) * 5).ToString());
                                    placement.LookupParameter("Longueur extension gauche interieure").SetValueString(((int)(((lM2bis - 180) / 2) / 5) * 5).ToString());
                                    if (lM2bis % 10 == 0)
                                    {
                                        placement.LookupParameter("Longueur extension droite").SetValueString(((int)(((lM2bis - 180) / 2) / 5) * 5).ToString());
                                        placement.LookupParameter("Longueur extension droite interieure").SetValueString(((int)(((lM2bis - 180) / 2) / 5) * 5).ToString());
                                    }
                                    else
                                    {
                                        placement.LookupParameter("Longueur extension droite").SetValueString(((int)(((lM2bis - 180) / 2) / 5) * 5 + 5).ToString());
                                        placement.LookupParameter("Longueur extension droite interieure").SetValueString(((int)(((lM2bis - 180) / 2) / 5) * 5 + 5).ToString());
                                    }
                                    placement.LookupParameter(h.bancheComboBox.SelectedItem.ToString()).Set(vrai);
                                    placement.LookupParameter("Passerelle incorrecte").Set(vrai);
                                    parametresTransactionM2bis.Commit();
                                }
                                #endregion transaction parametres M2bis
                                if (lM2bis % 10 == 0)
                                {
                                    l2 = ((int)(((lM2bis - 180) / 2) / 5) * 5 + 180 / 2) / feet;
                                }
                                else { l2 = ((int)(((lM2bis - 180) / 2) / 5) * 5 + 5 + 180 / 2) / feet; }
                            }
                            #endregion M2bis
                            #region Boucle si nM3ter existe
                            if (nM3ter != 0)
                            {
                                l = l + l2 + ((((lM3ter - 330) / 2) / 5) * 5) / feet + (330 / 2) / feet;
                                x = pointDebut.X + l * Math.Cos(angleTrigo);
                                y = pointDebut.Y + l * Math.Sin(angleTrigo);
                                pointInsertionPass = new XYZ(x, y, z);

                                #region Transaction Placement M3ter
                                Transaction placementTransactionnM3ter = new Transaction(doc, "transaction placement");
                                {
                                    placementTransactionnM3ter.Start();
                                    placement = doc.Create.NewFamilyInstance(pointInsertionPass, m3FS, nonStructural);
                                    placementTransactionnM3ter.Commit();
                                }
                                #endregion transaction placement M3ter
                                #region axe de rotation M3ter
                                //axe de rotation
                                XYZ a1 = (placement.Location as LocationPoint).Point;
                                XYZ a2 = new XYZ(a1.X, a1.Y, a1.Z + 10);
                                Autodesk.Revit.DB.Line axis = Autodesk.Revit.DB.Line.CreateBound(a1, a2);
                                #endregion axe rotation M1bis
                                #region Transaction rotate M3ter
                                Transaction rotationtransactionM3ter = new Transaction(doc, "RotationPasserelle");
                                {
                                    rotationtransactionM3ter.Start();
                                    ElementTransformUtils.RotateElement(doc, placement.Id, axis, anglerotation);
                                    rotationtransactionM3ter.Commit();
                                }
                                #endregion transaction rotate M3ter
                                #region Transaction parametres M3ter
                                Transaction parametresTransactionM3ter = new Transaction(doc, "parametresTransaction");
                                {
                                    parametresTransactionM3ter.Start();
                                    placement.LookupParameter("Longueur extension gauche").SetValueString(((int)(((lM3ter - 330) / 2) / 5) * 5).ToString());
                                    placement.LookupParameter("Longueur extension gauche interieure").SetValueString(((int)(((lM3ter - 330) / 2) / 5) * 5).ToString());
                                    if (lM3ter % 10 == 0)
                                    {
                                        placement.LookupParameter("Longueur extension droite").SetValueString(((int)(((lM3ter - 330) / 2) / 5) * 5).ToString());
                                        placement.LookupParameter("Longueur extension droite interieure").SetValueString(((int)(((lM3ter - 330) / 2) / 5) * 5).ToString());
                                    }
                                    else
                                    {
                                        placement.LookupParameter("Longueur extension droite").SetValueString(((int)(((lM3ter - 330) / 2) / 5) * 5 + 5).ToString());
                                        placement.LookupParameter("Longueur extension droite interieure").SetValueString(((int)(((lM3ter - 330) / 2) / 5) * 5 + 5).ToString());
                                    }
                                    placement.LookupParameter(h.bancheComboBox.SelectedItem.ToString()).Set(vrai);
                                    placement.LookupParameter("Passerelle incorrecte").Set(vrai);
                                    parametresTransactionM3ter.Commit();
                                }

                                #endregion transaction parametres M3ter
                                if (lM3ter % 10 == 0)
                                {
                                    l2 = ((int)(((lM3ter - 330) / 2) / 5) * 5 + 330 / 2) / feet;
                                }
                                else { l2 = ((int)(((lM3ter - 330) / 2) / 5) * 5 + 5 + 330 / 2) / feet; }
                            }
                            #endregion M3ter
                            #endregion deuxieme boucle
                            #region on insere la dernière passerelle
                            if (dR==1)
                            {
                                l = (lgMurInt - (drD + drC / 2))/feet;
                                x = pointDebut.X + l * Math.Cos(angleTrigo);
                                y = pointDebut.Y + l * Math.Sin(angleTrigo);
                                pointInsertionPass = new XYZ(x, y, z);


                                #region Transaction Placement
                                Transaction placementTransactionFin = new Transaction(doc, "transaction placement");
                                {
                                    placementTransactionFin.Start();

                                    placement = doc.Create.NewFamilyInstance(pointInsertionPass, drfs, nonStructural);

                                    placementTransactionFin.Commit();
                                }
                                #endregion transaction placement
                                #region axe de rotation
                                //axe de rotation
                                XYZ a1 = (placement.Location as LocationPoint).Point;
                                XYZ a2 = new XYZ(a1.X, a1.Y, a1.Z + 10);
                                Autodesk.Revit.DB.Line axis = Autodesk.Revit.DB.Line.CreateBound(a1, a2);
                                #endregion axe rotation
                                #region Transaction rotate
                                Transaction rotationtransactionFin = new Transaction(doc, "RotationPasserelle");
                                {
                                    rotationtransactionFin.Start();
                                    ElementTransformUtils.RotateElement(doc, placement.Id, axis, anglerotation);

                                    rotationtransactionFin.Commit();
                                }
                                #endregion transaction rotate
                                #region Transaction parametres
                                Transaction parametresTransactionFin = new Transaction(doc, "parametresTransaction");
                                {
                                    parametresTransactionFin.Start();
                                    placement.LookupParameter("Longueur extension gauche").SetValueString((drG).ToString());
                                    placement.LookupParameter("Longueur extension gauche interieure").SetValueString((drG).ToString());
                                    placement.LookupParameter("Longueur extension droite").SetValueString((drD).ToString());
                                    placement.LookupParameter("Longueur extension droite interieure").SetValueString((drD).ToString());
                                    placement.LookupParameter(h.bancheComboBox.SelectedItem.ToString()).Set(vrai);
                                    placement.LookupParameter("Passerelle incorrecte").Set(vrai);
                                    parametresTransactionFin.Commit();
                                }
                                #endregion transaction parametres
                            }
                        #endregion Dernière passerelle
                            h.Close();
                        };//applybutton
                    h.ShowDialog();
                    #region masqué
                    #region Premiere Boucle
                    
                    #region boucle M3max
                     /*
                    //boucle
                    int i = 1;
                    while (i <= nM3)
                    {
                        l = l + l2 + (lM3 / 2) / feet;
                        x = pointDebut.X + l * Math.Cos(angleTrigo);
                        y = pointDebut.Y + l * Math.Sin(angleTrigo);
                        pointInsertionPass = new XYZ(x, y, z);

                        #region Transaction Placement
                        Transaction placementTransaction = new Transaction(doc, "transaction placement");
                        {
                            placementTransaction.Start();

                            placement = doc.Create.NewFamilyInstance(pointInsertionPass, m3FS, nonStructural);



                            placementTransaction.Commit();
                        }
                        #endregion transaction placement

                        #region axe de rotation
                        //axe de rotation
                        XYZ a1 = (placement.Location as LocationPoint).Point;
                        XYZ a2 = new XYZ(a1.X, a1.Y, a1.Z + 10);
                        Autodesk.Revit.DB.Line axis = Autodesk.Revit.DB.Line.CreateBound(a1, a2);
                        #endregion axe rotation

                        #region Transaction rotate
                        Transaction rotationtransaction = new Transaction(doc, "RotationPasserelle");
                        {
                            rotationtransaction.Start();
                            ElementTransformUtils.RotateElement(doc, placement.Id, axis, anglerotation);

                            rotationtransaction.Commit();
                        }

                        #endregion transaction rotate

                        #region Transaction parametres
                        Transaction parametresTransaction = new Transaction(doc, "parametresTransaction");
                        {
                            parametresTransaction.Start();
                            placement.LookupParameter("Longueur extension gauche").SetValueString(((lM3 - 330) / 2).ToString());
                            placement.LookupParameter("Longueur extension gauche interieure").SetValueString(((lM3 - 330) / 2).ToString());
                            placement.LookupParameter("Longueur extension droite").SetValueString(((lM3 - 330) / 2).ToString());
                            placement.LookupParameter("Longueur extension droite interieure").SetValueString(((lM3 - 330) / 2).ToString());
                            placement.LookupParameter(h.bancheComboBox.SelectedItem.ToString()).Set(vrai);
                            parametresTransaction.Commit();
                        }

                        #endregion transaction parametres

                        l2 = (lM3 / 2 + 5) / feet;
                        i = i + 1;

                    }

                    #endregion boucle M3Max

                    #region Boucle si nM3bis
                    if (nM3bis != 0)
                    {

                        l = l + l2 + ((int)(((lM3bis - 330) / 2) / 5) * 5) / feet + (330 / 2) / feet;
                        x = pointDebut.X + l * Math.Cos(angleTrigo);
                        y = pointDebut.Y + l * Math.Sin(angleTrigo);
                        pointInsertionPass = new XYZ(x, y, z);

                        #region Transaction Placement M3bis
                        Transaction placementTransactionM3bis = new Transaction(doc, "transaction placement");
                        {
                            placementTransactionM3bis.Start();

                            placement = doc.Create.NewFamilyInstance(pointInsertionPass, m3FS, nonStructural);



                            placementTransactionM3bis.Commit();
                        }
                        #endregion transaction placement

                        #region axe de rotation M3bis
                        //axe de rotation
                        XYZ a1 = (placement.Location as LocationPoint).Point;
                        XYZ a2 = new XYZ(a1.X, a1.Y, a1.Z + 10);
                        Autodesk.Revit.DB.Line axis = Autodesk.Revit.DB.Line.CreateBound(a1, a2);
                        #endregion axe rotation

                        #region Transaction rotate M3bis
                        Transaction rotationtransactionM3bis = new Transaction(doc, "RotationPasserelle");
                        {
                            rotationtransactionM3bis.Start();
                            ElementTransformUtils.RotateElement(doc, placement.Id, axis, anglerotation);

                            rotationtransactionM3bis.Commit();
                        }

                        #endregion transaction rotate M3bis

                        #region Transaction parametres M3bis
                        Transaction parametresTransactionM3bis = new Transaction(doc, "parametresTransaction");
                        {
                            parametresTransactionM3bis.Start();
                            placement.LookupParameter("Longueur extension gauche").SetValueString(((int)(((lM3bis - 330) / 2) / 5) * 5).ToString());
                            placement.LookupParameter("Longueur extension gauche interieure").SetValueString(((int)(((lM3bis - 330) / 2) / 5) * 5).ToString());
                            if (lM3bis % 10 == 0)
                            {
                                placement.LookupParameter("Longueur extension droite").SetValueString(((int)(((lM3bis - 330) / 2) / 5) * 5).ToString());
                                placement.LookupParameter("Longueur extension droite interieure").SetValueString(((int)(((lM3bis - 330) / 2) / 5) * 5).ToString());
                            }
                            else
                            {
                                placement.LookupParameter("Longueur extension droite").SetValueString(((int)(((lM3bis - 330) / 2) / 5) * 5 + 5).ToString());
                                placement.LookupParameter("Longueur extension droite interieure").SetValueString(((int)(((lM3bis - 330) / 2) / 5) * 5 + 5).ToString());
                            }
                            placement.LookupParameter(h.bancheComboBox.SelectedItem.ToString()).Set(vrai);

                            parametresTransactionM3bis.Commit();
                        }

                        #endregion transaction parametres

                        l2 = ((int)(((lM3bis - 330) / 2) / 5) * 5 + 5 + 330 / 2) / feet;


                    }

                    #endregion nM3bis

                    #region Boucle si nM2
                    if (nM2 != 0)
                    {
                        l = l + l2 + ((int)(((lM2 - 180) / 2) / 5) * 5) / feet + (180 / 2) / feet;
                        x = pointDebut.X + l * Math.Cos(angleTrigo);
                        y = pointDebut.Y + l * Math.Sin(angleTrigo);
                        pointInsertionPass = new XYZ(x, y, z);


                        #region Transaction Placement M2
                        Transaction placementTransactionM2 = new Transaction(doc, "transaction placement");
                        {
                            placementTransactionM2.Start();

                            placement = doc.Create.NewFamilyInstance(pointInsertionPass, m2FS, nonStructural);

                            placementTransactionM2.Commit();
                        }
                        #endregion transaction placement M2

                        #region axe de rotation M2
                        //axe de rotation
                        XYZ a1 = (placement.Location as LocationPoint).Point;
                        XYZ a2 = new XYZ(a1.X, a1.Y, a1.Z + 10);
                        Autodesk.Revit.DB.Line axis = Autodesk.Revit.DB.Line.CreateBound(a1, a2);
                        #endregion axe rotation M2

                        #region Transaction rotate M2
                        Transaction rotationtransactionM2 = new Transaction(doc, "RotationPasserelle");
                        {
                            rotationtransactionM2.Start();
                            ElementTransformUtils.RotateElement(doc, placement.Id, axis, anglerotation);

                            rotationtransactionM2.Commit();
                        }

                        #endregion transaction rotate M2

                        #region Transaction parametres M2
                        Transaction parametresTransactionM2 = new Transaction(doc, "parametresTransaction");
                        {
                            parametresTransactionM2.Start();
                            placement.LookupParameter("Longueur extension gauche").SetValueString(((int)(((lM2 - 180) / 2) / 5) * 5).ToString());
                            placement.LookupParameter("Longueur extension gauche interieure").SetValueString(((int)(((lM2 - 180) / 2) / 5) * 5).ToString());


                            placement.LookupParameter(h.bancheComboBox.SelectedItem.ToString()).Set(vrai);

                            parametresTransactionM2.Commit();
                        }

                        #endregion transaction parametres M2

                        if (lM2 % 10 == 0)
                        {
                            l2 = ((int)(((lM2 - 180) / 2) / 5) * 5 + 180 / 2) / feet;
                        }
                        else { l2 = ((int)(((lM2 - 180) / 2) / 5) * 5 + 5 + 180 / 2) / feet; }
                    }

                    #endregion M2

                    //exit
                    #region Boucle si nM1
                    /*
            if (nM1 != 0)
            {
                l = l + (lM1 / 2) / feet - (lM3 / 2) / feet;
                x = pointDebut.X + l * Math.Cos(angleTrigo);
                y = pointDebut.Y + l * Math.Sin(angleTrigo);
                pointInsertionPass = new XYZ(x, y, z);


                #region Transaction Placement M1
                Transaction placementTransactionM1 = new Transaction(doc, "transaction placement");
                {
                    placementTransactionM1.Start();

                    placement = doc.Create.NewFamilyInstance(pointInsertionPass, m1FS, nonStructural);

                    placementTransactionM1.Commit();
                }
                #endregion transaction placement M1

                #region axe de rotation M1
                //axe de rotation
                XYZ a1 = (placement.Location as LocationPoint).Point;
                XYZ a2 = new XYZ(a1.X, a1.Y, a1.Z + 10);
                Autodesk.Revit.DB.Line axis = Autodesk.Revit.DB.Line.CreateBound(a1, a2);
                #endregion axe rotation M1

                #region Transaction rotate M1
                Transaction rotationtransactionM1 = new Transaction(doc, "RotationPasserelle");
                {
                    rotationtransactionM1.Start();
                    ElementTransformUtils.RotateElement(doc, placement.Id, axis, anglerotation);

                    rotationtransactionM1.Commit();
                }

                #endregion transaction rotate M1

                #region Transaction parametres M1
                Transaction parametresTransactionM1 = new Transaction(doc, "parametresTransaction");
                {
                    parametresTransactionM1.Start();
                    placement.LookupParameter("Longueur extension gauche").SetValueString(((lM1 - 70) / 2).ToString());
                    placement.LookupParameter("Longueur extension droite").SetValueString(((lM1 - 70) / 2).ToString());
                    placement.LookupParameter("GC about droit").Set(vrai);
                    placement.LookupParameter(bancheComboBox.SelectedItem.ToString()).Set(vrai);
                    parametresTransactionM1.Commit();
                }

                #endregion transaction parametres M1
            }
            */
                    
                    #endregion M1
                    
                    #endregion Premiere boucle
                    /*
                    #region Deuxieme boucle avec M3 min inséré
                    #region Boucle si nM2bis exite
                    if (nM2bis != 0)
                    {
                        l = l + l2 + ((((lM2bis - 180) / 2) / 5) * 5) / feet + (180 / 2) / feet;
                        x = pointDebut.X + l * Math.Cos(angleTrigo);
                        y = pointDebut.Y + l * Math.Sin(angleTrigo);
                        pointInsertionPass = new XYZ(x, y, z);


                        #region Transaction Placement M2bis
                        Transaction placementTransactionM2bis = new Transaction(doc, "transaction placement");
                        {
                            placementTransactionM2bis.Start();

                            placement = doc.Create.NewFamilyInstance(pointInsertionPass, m2FS, nonStructural);

                            placementTransactionM2bis.Commit();
                        }
                        #endregion transaction placement M2bis

                        #region axe de rotation M2bis
                        //axe de rotation
                        XYZ a1 = (placement.Location as LocationPoint).Point;
                        XYZ a2 = new XYZ(a1.X, a1.Y, a1.Z + 10);
                        Autodesk.Revit.DB.Line axis = Autodesk.Revit.DB.Line.CreateBound(a1, a2);
                        #endregion axe rotation M2bis

                        #region Transaction rotate M2bis
                        Transaction rotationtransactionM2bis = new Transaction(doc, "RotationPasserelle");
                        {
                            rotationtransactionM2bis.Start();
                            ElementTransformUtils.RotateElement(doc, placement.Id, axis, anglerotation);

                            rotationtransactionM2bis.Commit();
                        }

                        #endregion transaction rotate M2bis

                        #region Transaction parametres M2bis
                        Transaction parametresTransactionM2bis = new Transaction(doc, "parametresTransaction");
                        {
                            parametresTransactionM2bis.Start();
                            placement.LookupParameter("Longueur extension gauche").SetValueString(((int)(((lM2bis - 180) / 2) / 5) * 5).ToString());
                            placement.LookupParameter("Longueur extension gauche interieure").SetValueString(((int)(((lM2bis - 180) / 2) / 5) * 5).ToString());
                            if (lM2bis % 10 == 0)
                            {
                                placement.LookupParameter("Longueur extension droite").SetValueString(((int)(((lM2bis - 180) / 2) / 5) * 5).ToString());
                                placement.LookupParameter("Longueur extension droite interieure").SetValueString(((int)(((lM2bis - 180) / 2) / 5) * 5).ToString());
                            }
                            else
                            {
                                placement.LookupParameter("Longueur extension droite").SetValueString(((int)(((lM2bis - 180) / 2) / 5) * 5 + 5).ToString());
                                placement.LookupParameter("Longueur extension droite interieure").SetValueString(((int)(((lM2bis - 180) / 2) / 5) * 5 + 5).ToString());
                            }
                            placement.LookupParameter(h.bancheComboBox.SelectedItem.ToString()).Set(vrai);
                            parametresTransactionM2bis.Commit();
                        }

                        #endregion transaction parametres M2bis
                        if (lM2bis % 10 == 0)
                        {
                            l2 = ((int)(((lM2bis - 180) / 2) / 5) * 5 + 180 / 2) / feet;
                        }
                        else { l2 = ((int)(((lM2bis - 180) / 2) / 5) * 5 + 5 + 180 / 2) / feet; }
                    }

                    #endregion M2bis

                    //exit
                    #region Boucle si nM1bis existe
                    /*
        if (nM1bis != 0)
        {
            l = l + (lM1bis / 2) / feet + (lM3bis/2+5) / feet;
            x = pointDebut.X + l * Math.Cos(angleTrigo);
            y = pointDebut.Y + l * Math.Sin(angleTrigo);
            pointInsertionPass = new XYZ(x, y, z);


            #region Transaction Placement M1bis
            Transaction placementTransactionM1bis = new Transaction(doc, "transaction placement");
            {
                placementTransactionM1bis.Start();

                placement = doc.Create.NewFamilyInstance(pointInsertionPass, m1FS, nonStructural);

                placementTransactionM1bis.Commit();
            }
            #endregion transaction placement M1bis

            #region axe de rotation M1bis
            //axe de rotation
            XYZ a1 = (placement.Location as LocationPoint).Point;
            XYZ a2 = new XYZ(a1.X, a1.Y, a1.Z + 10);
            Autodesk.Revit.DB.Line axis = Autodesk.Revit.DB.Line.CreateBound(a1, a2);
            #endregion axe rotation M1bis

            #region Transaction rotate M1bis
            Transaction rotationtransactionM1bis = new Transaction(doc, "RotationPasserelle");
            {
                rotationtransactionM1bis.Start();
                ElementTransformUtils.RotateElement(doc, placement.Id, axis, anglerotation);

                rotationtransactionM1bis.Commit();
            }

            #endregion transaction rotate M1bis

            #region Transaction parametres M1bis
            Transaction parametresTransactionM1bis = new Transaction(doc, "parametresTransaction");
            {
                parametresTransactionM1bis.Start();
                placement.LookupParameter("Longueur extension gauche").SetValueString(((lM1bis - 70) / 2).ToString());
                placement.LookupParameter("Longueur extension droite").SetValueString(((lM1bis - 70) / 2).ToString());
                placement.LookupParameter("GC about droit").Set(vrai);
                placement.LookupParameter(bancheComboBox.SelectedItem.ToString()).Set(vrai);
                parametresTransactionM1bis.Commit();
            }

            #endregion transaction parametres M1bis
        }
        */
                    /*
                    #endregion M1bis

                    #region Boucle si nM3ter existe
                    if (nM3ter != 0)
                    {
                        l = l + l2 + ((((lM3ter - 330) / 2) / 5) * 5) / feet + (330 / 2) / feet;
                        x = pointDebut.X + l * Math.Cos(angleTrigo);
                        y = pointDebut.Y + l * Math.Sin(angleTrigo);
                        pointInsertionPass = new XYZ(x, y, z);


                        #region Transaction Placement M3ter
                        Transaction placementTransactionnM3ter = new Transaction(doc, "transaction placement");
                        {
                            placementTransactionnM3ter.Start();

                            placement = doc.Create.NewFamilyInstance(pointInsertionPass, m3FS, nonStructural);

                            placementTransactionnM3ter.Commit();
                        }
                        #endregion transaction placement M3ter

                        #region axe de rotation M3ter
                        //axe de rotation
                        XYZ a1 = (placement.Location as LocationPoint).Point;
                        XYZ a2 = new XYZ(a1.X, a1.Y, a1.Z + 10);
                        Autodesk.Revit.DB.Line axis = Autodesk.Revit.DB.Line.CreateBound(a1, a2);
                        #endregion axe rotation M1bis

                        #region Transaction rotate M3ter
                        Transaction rotationtransactionM3ter = new Transaction(doc, "RotationPasserelle");
                        {
                            rotationtransactionM3ter.Start();
                            ElementTransformUtils.RotateElement(doc, placement.Id, axis, anglerotation);

                            rotationtransactionM3ter.Commit();
                        }

                        #endregion transaction rotate M3ter

                        #region Transaction parametres M3ter
                        Transaction parametresTransactionM3ter = new Transaction(doc, "parametresTransaction");
                        {
                            parametresTransactionM3ter.Start();
                            placement.LookupParameter("Longueur extension gauche").SetValueString(((int)(((lM3ter - 330) / 2) / 5) * 5).ToString());
                            placement.LookupParameter("Longueur extension gauche interieure").SetValueString(((int)(((lM3ter - 330) / 2) / 5) * 5).ToString());
                            if (lM3ter % 10 == 0)
                            {
                                placement.LookupParameter("Longueur extension droite").SetValueString(((int)(((lM3ter - 330) / 2) / 5) * 5).ToString());
                                placement.LookupParameter("Longueur extension droite interieure").SetValueString(((int)(((lM3ter - 330) / 2) / 5) * 5).ToString());
                            }
                            else
                            {
                                placement.LookupParameter("Longueur extension droite").SetValueString(((int)(((lM3ter - 330) / 2) / 5) * 5 + 5).ToString());
                                placement.LookupParameter("Longueur extension droite interieure").SetValueString(((int)(((lM3ter - 330) / 2) / 5) * 5 + 5).ToString());
                            }
                            placement.LookupParameter(h.bancheComboBox.SelectedItem.ToString()).Set(vrai);
                            parametresTransactionM3ter.Commit();
                        }

                        #endregion transaction parametres M3ter
                        if (lM3ter % 10 == 0)
                        {
                            l2 = ((int)(((lM3ter - 330) / 2) / 5) * 5 + 330 / 2) / feet;
                        }
                        else { l2 = ((int)(((lM3ter - 330) / 2) / 5) * 5 + 5 + 330 / 2) / feet; }
                    }

                    #endregion M3ter
                    #endregion deuxieme boucle
                    
                    
                    abc = abc + 1;)
                    */
                    #endregion masqué
                }

            }//using h
            return Result.Succeeded;
            }
        }//elementSet
    }//public class
}//namespace
