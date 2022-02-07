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
using System.IO;

namespace PTLGClassLibrary
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(RegenerationOption.Manual)]
    public class ConfigurateurClass : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
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
            // Si il y a plus d'un objet selectionne : erreur
            if (selectionPasserelleIdList.Count == 0)
                {
                    TaskDialog.Show("ERREUR", "Selectionner une passerelle");
                    return Result.Cancelled;
                }

                if (selectionPasserelleIdList.Count > 1)
                {
                    TaskDialog.Show("ERREUR", "Selectionner une seule passerelle");
                    return Result.Cancelled;
                }
            #endregion Erreur : plusieurs objets selectionnes
            Element passerelleSelectionneeElement = selectionPasserelleList[0];

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
            string type = "";
            if (passerelleSelectionneeElement.Name.Contains("M1"))
                type = "M1";
            if (passerelleSelectionneeElement.Name.Contains("M2"))
                type = "M2";
            if (passerelleSelectionneeElement.Name.Contains("M3"))
                type = "M3";
            #endregion Association du type
            #region liste de tous les parametres de la passerelle selectionnee
            List<Autodesk.Revit.DB.Parameter> parameterList = new List<Autodesk.Revit.DB.Parameter>();
            foreach (Autodesk.Revit.DB.Parameter para in passerelleSelectionneeElement.Parameters)
            {
                //On crée la liste de tous les parametres de la passerelle selectionnee
                parameterList.Add(para);
            }
            #endregion liste de tous les parametres de la passerelle selectionnee
            List<int> ListExtLaterale = new List<int>();
            int nbre = 0;
            if (type == "M1")
            {
                for (int i = -1; i < 12; i++)
                    ListExtLaterale.Add((i + 4) * 5);
            }
            else if (type == "M2")
                for (int i = 0; i < 16; i++)
                    ListExtLaterale.Add((i + 4) * 5);
            else if (type == "M3")
            {
                for (int i = 0; i < 29; i++)
                    ListExtLaterale.Add((i + 4) * 5);
            }

            #region Initialisation diverse

            //Initialisation du numéro de passerelle.

            ConfigurateurForm f = new ConfigurateurForm();
            f.numeroPasserelleString = passerelleSelectionneeElement.GetParameters("LG_MET_PTLG_Designation")[0].AsString();

            //initialisation stabilisation
            foreach (string str in f.stabilisationComboBox.Items)
            {
                if (passerelleSelectionneeElement.GetParameters(str)[0].AsInteger() == vrai)
                {
                    f.stabilisationComboBox.SelectedIndex = f.stabilisationComboBox.FindStringExact(str);
                }
            }

            // Initialisation de la banche
            foreach (string str in f.bancheComboBox.Items)
            {
                if (passerelleSelectionneeElement.GetParameters(str)[0].AsInteger() == vrai)
                {
                    f.bancheComboBox.SelectedIndex = f.bancheComboBox.FindStringExact(str);
                }
            }

            if (passerelleSelectionneeElement.Name.Contains("M3") && (f.bancheComboBox.SelectedIndex > 3))
            {
                f.troisiemeAttacheCheckBox.Visible = true;
                if (f.bancheComboBox.SelectedIndex > 6)
                {
                    f.troisiemeAttacheCheckBox.Checked = true;
                    f.troisiemeAttacheCheckBox.Enabled = false;
                }
                
            }
            else
            {
                f.troisiemeAttacheCheckBox.Visible = false;
            }

            //initialisation de l'angle droit
            if (passerelleSelectionneeElement.Name.Contains("M2")
                || passerelleSelectionneeElement.Name.Contains("M3"))
            {
                //gauche
                if (passerelleSelectionneeElement.GetParameters("Gauche - Angle droit")[0].AsInteger() == vrai)
                    f.angleGaucheCheckBox.Checked = true;
                else f.angleGaucheCheckBox.Checked = false;
                //droit
                if (passerelleSelectionneeElement.GetParameters("Droite - Angle droit")[0].AsInteger() == vrai)
                    f.angleDroitCheckBox.Checked = true;
                else f.angleDroitCheckBox.Checked = false;
            }
            else
            {
                f.angleGaucheCheckBox.Visible = false;
                f.angleGaucheCheckBox.Checked = false;
                f.angleDroitCheckBox.Visible = false;
                f.angleDroitCheckBox.Checked = false;
            }
            //initialisation GC droit
            if (passerelleSelectionneeElement.GetParameters("GC about droit")[0].AsInteger() == vrai)
            {
                f.gcDroiteCheckBox.Checked = true;
            }
            else f.gcDroiteCheckBox.Checked = false;

            //initialisation GC droit
            if (passerelleSelectionneeElement.GetParameters("GC about gauche")[0].AsInteger() == vrai)
            {
                f.gcGaucheCheckBox.Checked = true;
            }
            else f.gcGaucheCheckBox.Checked = false;

            #endregion


            //actualisation

            f.bancheComboBox.SelectedIndexChanged += (send, a) =>
            {
                 if (passerelleSelectionneeElement.Name.Contains("M3") && (f.bancheComboBox.SelectedIndex > 3))
                 {
                     f.troisiemeAttacheCheckBox.Visible = true;
                     if (f.bancheComboBox.SelectedIndex > 6)
                     {
                         f.troisiemeAttacheCheckBox.Checked = true;
                         f.troisiemeAttacheCheckBox.Enabled = false;
                     }
                     
                 }
                 else
                 {
                     f.troisiemeAttacheCheckBox.Checked = false;
                     f.troisiemeAttacheCheckBox.Visible=false;
                 }
            };

            #region actualisation des valeurs maxi des qu'on change soit Hbanche soit Angles droit/gauche
            f.eventcheckBoxext.CheckedChanged += (send, a) =>
            {
            //on lit et stocke les valeurs maxi
            #region lecture et stockage des valeurs maxi
            if (passerelleSelectionneeElement.Name.Contains("M3"))
                {
                    GeneralClass.leMaxAngle = 120;
                    GeneralClass.lgFixe = 330;
                    f.attacheGaucheTrackbar.Maximum = 160;
                    f.attacheGaucheTrackbar.Minimum = -160;
                    f.attacheDroiteTrackbar.Maximum = 160;
                    f.attacheDroiteTrackbar.Minimum = -160;
                    GeneralClass.pafMax = GeneralClass.lgTotale / 3;

                    if (f.extArriereCheckBox.Checked == true) //2.5m
                {
                        if ((f.bancheComboBox.SelectedIndex > 2) && (f.troisiemeAttacheCheckBox.Checked == true)) // 3e attache cochée
                    {
                               
                            GeneralClass.lgMax = GeneralClass.LectureCSVint("DonneesBancheM3bis_extarr", f.bancheComboBox.SelectedIndex + 1, 3);
                            GeneralClass.leMax = GeneralClass.LectureCSVint("DonneesBancheM3bis_extarr", f.bancheComboBox.SelectedIndex + 1, 4);
                            GeneralClass.cMax = GeneralClass.LectureCSVint("DonneesBancheM3bis_extarr", f.bancheComboBox.SelectedIndex + 1, 1);
                            GeneralClass.dMax = GeneralClass.LectureCSVint("DonneesBancheM3bis_extarr", f.bancheComboBox.SelectedIndex + 1, 5);
                               
                        }
                        else //2 attaches
                    {
                            GeneralClass.lgMax = GeneralClass.LectureCSVint("DonneesBancheM3_extarr", f.bancheComboBox.SelectedIndex + 1, 3);
                            GeneralClass.leMax = GeneralClass.LectureCSVint("DonneesBancheM3_extarr", f.bancheComboBox.SelectedIndex + 1, 4);
                            GeneralClass.cMax = GeneralClass.LectureCSVint("DonneesBancheM3_extarr", f.bancheComboBox.SelectedIndex + 1, 1);
                            GeneralClass.dMax = GeneralClass.LectureCSVint("DonneesBancheM3_extarr", f.bancheComboBox.SelectedIndex + 1, 5);
                        }
                    }
                    else //2m
                {
                        if ((f.bancheComboBox.SelectedIndex > 3) && (f.troisiemeAttacheCheckBox.Checked == true)) //&& 3e attache cochée
                    {
                            GeneralClass.lgMax = GeneralClass.LectureCSVint("DonneesBancheM3bis", f.bancheComboBox.SelectedIndex + 1, 3);
                            GeneralClass.leMax = GeneralClass.LectureCSVint("DonneesBancheM3bis", f.bancheComboBox.SelectedIndex + 1, 4);
                            GeneralClass.cMax = GeneralClass.LectureCSVint("DonneesBancheM3bis", f.bancheComboBox.SelectedIndex + 1, 1);
                            GeneralClass.dMax = GeneralClass.LectureCSVint("DonneesBancheM3bis", f.bancheComboBox.SelectedIndex + 1, 5);
                        }
                        else //2attaches
                    {
                            GeneralClass.lgMax = GeneralClass.LectureCSVint("DonneesBancheM3", f.bancheComboBox.SelectedIndex + 1, 3);
                            GeneralClass.leMax = GeneralClass.LectureCSVint("DonneesBancheM3", f.bancheComboBox.SelectedIndex + 1, 4);
                            GeneralClass.cMax = GeneralClass.LectureCSVint("DonneesBancheM3", f.bancheComboBox.SelectedIndex + 1, 1);
                            GeneralClass.dMax = GeneralClass.LectureCSVint("DonneesBancheM3", f.bancheComboBox.SelectedIndex + 1, 5);
                    }
                    }
                }
                if (passerelleSelectionneeElement.Name.Contains("M2"))
                {
                    GeneralClass.leMaxAngle = 55;
                    GeneralClass.lgFixe = 180;
                    f.attacheGaucheTrackbar.Maximum = 95;
                    f.attacheGaucheTrackbar.Minimum = -95;
                    f.attacheDroiteTrackbar.Maximum = 95;
                    f.attacheDroiteTrackbar.Minimum = -95;
                    GeneralClass.pafMax = GeneralClass.lgTotale / 3;
                    if (f.extArriereCheckBox.Checked == true)
                    {
                        GeneralClass.lgMax = GeneralClass.LectureCSVint("DonneesBancheM2_extarr", f.bancheComboBox.SelectedIndex + 1, 3);
                        GeneralClass.leMax = GeneralClass.LectureCSVint("DonneesBancheM2_extarr", f.bancheComboBox.SelectedIndex + 1, 4);
                        GeneralClass.cMax = GeneralClass.LectureCSVint("DonneesBancheM2_extarr", f.bancheComboBox.SelectedIndex + 1, 1);
                        GeneralClass.dMax = GeneralClass.LectureCSVint("DonneesBancheM2_extarr", f.bancheComboBox.SelectedIndex + 1, 5);

                    }
                    else //2m
                {
                        GeneralClass.lgMax = GeneralClass.LectureCSVint("DonneesBancheM2", f.bancheComboBox.SelectedIndex + 1, 3);
                        GeneralClass.leMax = GeneralClass.LectureCSVint("DonneesBancheM2", f.bancheComboBox.SelectedIndex + 1, 4);
                        GeneralClass.cMax = GeneralClass.LectureCSVint("DonneesBancheM2", f.bancheComboBox.SelectedIndex + 1, 1);
                        GeneralClass.dMax = GeneralClass.LectureCSVint("DonneesBancheM2", f.bancheComboBox.SelectedIndex + 1, 5);
                    }
                }
                if (passerelleSelectionneeElement.Name.Contains("M1"))
                {

                    GeneralClass.lgFixe = 70;
                    f.attacheGaucheTrackbar.Maximum = 75;
                    f.attacheGaucheTrackbar.Minimum = -75;
                    f.attacheDroiteTrackbar.Maximum = 75;
                    f.attacheDroiteTrackbar.Minimum = -75;
                    GeneralClass.pafMax = GeneralClass.lgTotale / 4;
                    if (f.extArriereCheckBox.Checked == true) //2.5m
                {
                        GeneralClass.lgMax = GeneralClass.LectureCSVint("DonneesBancheM1_extarr", f.bancheComboBox.SelectedIndex + 1, 3);
                        GeneralClass.leMax = GeneralClass.LectureCSVint("DonneesBancheM1_extarr", f.bancheComboBox.SelectedIndex + 1, 4);
                        GeneralClass.cMax = GeneralClass.LectureCSVint("DonneesBancheM1_extarr", f.bancheComboBox.SelectedIndex + 1, 1);
                        GeneralClass.dMax = GeneralClass.LectureCSVint("DonneesBancheM1_extarr", f.bancheComboBox.SelectedIndex + 1, 5);

                    }
                    else //2m
                {
                        GeneralClass.lgMax = GeneralClass.LectureCSVint("DonneesBancheM1", f.bancheComboBox.SelectedIndex + 1, 3);
                        GeneralClass.leMax = GeneralClass.LectureCSVint("DonneesBancheM1", f.bancheComboBox.SelectedIndex + 1, 4);
                        GeneralClass.cMax = GeneralClass.LectureCSVint("DonneesBancheM1", f.bancheComboBox.SelectedIndex + 1, 1);
                        GeneralClass.dMax = GeneralClass.LectureCSVint("DonneesBancheM1", f.bancheComboBox.SelectedIndex + 1, 5);
                    }
                    GeneralClass.leMaxAngle = GeneralClass.leMax;
                }

                #endregion lecture et stockage des valeurs maxi

                if (GeneralClass.lgTotale < 300)
                {
                    GeneralClass.pafMaxAngle = 29;
                }
                else if (GeneralClass.lgTotale <= 610)
                {
                    GeneralClass.pafMaxAngle = GeneralClass.LectureCSVint("PAFmaxi", Convert.ToInt32(GeneralClass.lgTotale / 10) - 28 -1, 1);//-1 à voir
                }
                        
            #region affichage des valeurs maxi
            if (f.angleGaucheCheckBox.Checked == false)
                {
                    f.leGaucheMaxiLabel.Text = GeneralClass.leMax.ToString();
                    f.pafGaucheMaxiLabel.Text = GeneralClass.pafMax.ToString();
                }
                else
                {
                    f.leGaucheMaxiLabel.Text = Math.Min(GeneralClass.leMaxAngle, GeneralClass.leMax).ToString();
                    f.pafGaucheMaxiLabel.Text = GeneralClass.pafMaxAngle.ToString();
                }
                if (f.angleDroitCheckBox.Checked == false)
                {
                    f.leDroitMaxiLabel.Text = GeneralClass.leMax.ToString();
                    f.pafDroitMaxiLabel.Text = GeneralClass.pafMax.ToString();
                }
                else
                {
                    f.leDroitMaxiLabel.Text = Math.Min(GeneralClass.leMaxAngle, GeneralClass.leMax).ToString();
                    f.pafDroitMaxiLabel.Text = GeneralClass.pafMaxAngle.ToString();
                }
            //GeneralClass.lgFixe
            f.lgFixeLabel.Text = GeneralClass.lgFixe.ToString();
            //GeneralClass.lgMax
            f.lgTotaleMaxiLabel.Text = GeneralClass.lgMax.ToString();
            //cMax
            f.cGaucheMaxiLabel.Text = GeneralClass.cMax.ToString();
                f.cDroiteMaxiLabel.Text = GeneralClass.cMax.ToString();
            //dmax
            f.dMaxiLabel.Text = GeneralClass.dMax.ToString();
                f.d1MaxiLabel.Text = GeneralClass.dMax.ToString();
                f.d2MaxiLabel.Text = GeneralClass.dMax.ToString();
            #endregion affichage des valeurs maxi
            #region affichage des valeurs maxi
            if (f.angleGaucheCheckBox.Checked == false)
                {
                    f.leGaucheMaxiLabel.Text = GeneralClass.leMax.ToString();
                    f.pafGaucheMaxiLabel.Text = GeneralClass.pafMax.ToString();
                }
                else
                {
                    f.leGaucheMaxiLabel.Text = Math.Min(GeneralClass.leMaxAngle, GeneralClass.leMax).ToString();
                    f.pafGaucheMaxiLabel.Text = GeneralClass.pafMaxAngle.ToString();
                }
                if (f.angleDroitCheckBox.Checked == false)
                {
                    f.leDroitMaxiLabel.Text = GeneralClass.leMax.ToString();
                    f.pafDroitMaxiLabel.Text = GeneralClass.pafMax.ToString();
                }
                else
                {
                    f.leDroitMaxiLabel.Text = Math.Min(GeneralClass.leMaxAngle, GeneralClass.leMax).ToString();
                    f.pafDroitMaxiLabel.Text = GeneralClass.pafMaxAngle.ToString();
                }
            //GeneralClass.lgFixe
            f.lgFixeLabel.Text = GeneralClass.lgFixe.ToString();
            //GeneralClass.lgMax
            f.dMaxiLabel.Text = GeneralClass.lgMax.ToString();
            //GeneralClass.cMax
            f.cGaucheMaxiLabel.Text = GeneralClass.cMax.ToString();
                f.cDroiteMaxiLabel.Text = GeneralClass.cMax.ToString();
            //dmax
            f.dMaxiLabel.Text = GeneralClass.dMax.ToString();
                f.d1MaxiLabel.Text = GeneralClass.dMax.ToString();
                f.d2MaxiLabel.Text = GeneralClass.dMax.ToString();
                #endregion affichage des valeurs maxi


                #region affichage des valeurs courantes
                //on regarde si on est en angle biais et on prend le cas le plus défavorable
                if (f.angleBiaisGaucheCheckBox.Checked == true)
                {
                    GeneralClass.lecturegauche = Math.Max(Convert.ToInt32(f.extensionGaucheComboBox.SelectedItem), Convert.ToInt32(f.extensionGaucheIntComboBox.SelectedItem));
                }
                else GeneralClass.lecturegauche = Convert.ToInt32(f.extensionGaucheComboBox.SelectedItem);

                if (f.angleBiaisDroitCheckBox.Checked == true)
                {
                    GeneralClass.lecturedroite = Math.Max(Convert.ToInt32(f.extensionDroiteComboBox.SelectedItem), Convert.ToInt32(f.extensionDroiteIntComboBox.SelectedItem));
                }
                else GeneralClass.lecturedroite = Convert.ToInt32(f.extensionDroiteComboBox.SelectedItem);
            //lg totale
            GeneralClass.lgTotale = GeneralClass.lecturegauche + GeneralClass.lecturedroite + GeneralClass.lgFixe;
                f.lgTotaleLabel.Text = GeneralClass.lgTotale.ToString();
            //GeneralClass.cGauche   
            GeneralClass.cGauche = -f.attacheGaucheTrackbar.Value;
                GeneralClass.distanceAttacheGauche = (GeneralClass.cGauche + GeneralClass.lgFixe / 2);
            //GeneralClass.cDroite
            GeneralClass.cDroite = f.attacheDroiteTrackbar.Value;
                GeneralClass.distanceAttacheDroite = (GeneralClass.cDroite + GeneralClass.lgFixe / 2);
            //paf
            GeneralClass.pafDroit = GeneralClass.lecturedroite - GeneralClass.cDroite;
                GeneralClass.pafGauche = GeneralClass.lecturegauche - GeneralClass.cGauche;
                f.pafGaucheLabel.Text = GeneralClass.pafGauche.ToString();
                f.pafDroitLabel.Text = GeneralClass.pafDroit.ToString();
            //d
            GeneralClass.d = GeneralClass.distanceAttacheDroite + GeneralClass.distanceAttacheGauche;
                f.dLabel.Text = GeneralClass.d.ToString();

                if (passerelleSelectionneeElement.Name.Contains("M3") && (f.bancheComboBox.SelectedIndex > 2))
                {
                    GeneralClass.d1 = GeneralClass.lgFixe / 2 + GeneralClass.cGauche- (-f.attacheCentreTrackbar.Value) ;
                    GeneralClass.d2 = GeneralClass.lgFixe / 2 + GeneralClass.cDroite- (+f.attacheCentreTrackbar.Value);
                    f.d1Label.Text = GeneralClass.d1.ToString();
                    f.d2Label.Text = GeneralClass.d2.ToString();
                    GeneralClass.distanceAttacheCentre = GeneralClass.lgFixe/2+ f.attacheCentreTrackbar.Value;
                }
            #endregion affichage des valeurs courantes
            #region test des valeurs
            //on teste les GeneralClass.leMax
            if (GeneralClass.lecturegauche > Convert.ToInt32(f.leGaucheMaxiLabel.Text))
                {
                    f.leGaucheMaxiLabel.BackColor = System.Drawing.Color.HotPink;
                    GeneralClass.erreur1 = "Extension gauche trop grande" + "\n";
                }
                else
                {
                    f.leGaucheMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
                    GeneralClass.erreur1 = "";
                }

            //on teste si passerelle biaisée droite, que Ld >100
            if (f.angleBiaisDroitCheckBox.Checked == true)
                {
                    if (Math.Abs(Convert.ToInt32(f.extensionDroiteComboBox.SelectedItem) - Convert.ToInt32(f.extensionDroiteIntComboBox.SelectedItem)) > 100)
                    {
                        f.angleBiaisDroitCheckBox.BackColor = System.Drawing.Color.HotPink;
                        f.fondExtensionExtDroite.BackColor = System.Drawing.Color.HotPink;
                        f.fondExtensionIntDroite.BackColor = System.Drawing.Color.HotPink;
                        GeneralClass.erreur2 = "Réduire le delta des extensions int et ext à droite" + "\n";
                    }
                    else
                    {
                        f.fondExtensionExtDroite.BackColor = System.Drawing.Color.Transparent;
                        f.fondExtensionIntDroite.BackColor = System.Drawing.Color.Transparent;
                        f.angleBiaisDroitCheckBox.BackColor = System.Drawing.Color.LightGreen;
                        GeneralClass.erreur2 = "";
                    }
                }
                else
                {
                    f.fondExtensionExtDroite.BackColor = System.Drawing.Color.Transparent;
                    f.fondExtensionIntDroite.BackColor = System.Drawing.Color.Transparent;
                    f.angleBiaisDroitCheckBox.BackColor = System.Drawing.Color.Transparent;
                    GeneralClass.erreur2 = "";
                }
            //on teste si passerelle biaisée gauche, que Ld <100
            if (f.angleBiaisGaucheCheckBox.Checked == true)
                {
                    if (Math.Abs(Convert.ToInt32(f.extensionGaucheComboBox.SelectedItem) - Convert.ToInt32(f.extensionGaucheIntComboBox.SelectedItem)) > 100)
                    {

                        f.angleBiaisGaucheCheckBox.BackColor = System.Drawing.Color.HotPink;
                        f.fondExtensionIntGauche.BackColor = System.Drawing.Color.HotPink;
                        f.fondExtensionExtGauche.BackColor = System.Drawing.Color.HotPink;
                        GeneralClass.erreur3 = "Réduire le delta des extensions int et ext à gauche" + "\n";
                    }
                    else
                    {
                        f.fondExtensionExtGauche.BackColor = System.Drawing.Color.Transparent;
                        f.fondExtensionIntGauche.BackColor = System.Drawing.Color.Transparent;
                        f.angleBiaisGaucheCheckBox.BackColor = System.Drawing.Color.LightGreen;
                        GeneralClass.erreur3 = "";
                    }
                }
                else
                {
                    f.fondExtensionExtGauche.BackColor = System.Drawing.Color.Transparent;
                    f.fondExtensionIntGauche.BackColor = System.Drawing.Color.Transparent;
                    f.angleBiaisGaucheCheckBox.BackColor = System.Drawing.Color.Transparent;
                    GeneralClass.erreur3 = "";
                }
            //lgextdroite
            if (GeneralClass.lecturedroite > Convert.ToInt32(f.leDroitMaxiLabel.Text))
                {
                    f.leDroitMaxiLabel.BackColor = System.Drawing.Color.HotPink;
                    GeneralClass.erreur4 = "Extension droite trop grande" + "\n";
                }
                else
                {
                    f.leDroitMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
                    GeneralClass.erreur4 = "";
                }

            //on teste GeneralClass.lgMax
            if (GeneralClass.lgTotale > GeneralClass.lgMax)
                {
                    f.lgTotaleMaxiLabel.BackColor = System.Drawing.Color.HotPink;
                    GeneralClass.erreur5 = "Longueur totale de la passerelle trop grande" + "\n";
                }
                else
                {
                    f.lgTotaleMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
                    GeneralClass.erreur5 = "";
                }

                //on test GeneralClass.cMax
                if (GeneralClass.cGauche > GeneralClass.cMax)
                {
                    if (f.troisiemeAttacheCheckBox.Checked == false)
                    {
                        f.cGaucheMaxiLabel.BackColor = System.Drawing.Color.HotPink;
                        GeneralClass.erreur6 = "Distance attache-ferme gauche trop grande" + "\n";
                    }
                }
                else if (Math.Abs(GeneralClass.cGauche) < 15)
                {

                    if (f.attacheGaucheComboBox.SelectedIndex == 2)
                    {
                        if (Math.Abs(GeneralClass.cGauche) < 12)
                        {
                            f.cGaucheMaxiLabel.BackColor = System.Drawing.Color.HotPink;
                            GeneralClass.erreur6 = "Distance attache-ferme gauche inférieure à 12" + "\n";
                        }
                        else
                        {
                            f.cGaucheMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
                            GeneralClass.erreur6 = "";
                        }
                    }
                    else
                    {
                        f.cGaucheMaxiLabel.BackColor = System.Drawing.Color.HotPink;
                        GeneralClass.erreur6 = "Distance attache-ferme gauche inférieure à 15" + "\n";
                    }
                }
                else
                {
                    f.cGaucheMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
                    GeneralClass.erreur6 = "";
                }

                if (GeneralClass.cDroite > GeneralClass.cMax)
                {
                    if (f.troisiemeAttacheCheckBox.Checked == false)
                    {
                        f.cDroiteMaxiLabel.BackColor = System.Drawing.Color.HotPink;
                        GeneralClass.erreur7 = "Distance attache-ferme droite trop grande" + "\n";
                    }
                }
                else if (Math.Abs(GeneralClass.cDroite) < 15)
                {
                    if (f.attacheDroiteComboBox.SelectedIndex == 2)
                    {
                        if (Math.Abs(GeneralClass.cDroite) < 12)
                        {
                            f.cDroiteMaxiLabel.BackColor = System.Drawing.Color.HotPink;
                            GeneralClass.erreur7 = "Distance attache-ferme droite inférieure à 12" + "\n";
                        }
                        else
                        {
                            f.cDroiteMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
                            GeneralClass.erreur7 = "";
                        }
                    }
                    else
                    {
                        f.cDroiteMaxiLabel.BackColor = System.Drawing.Color.HotPink;
                        GeneralClass.erreur7 = "Distance attache-ferme droite inférieure à 15" + "\n";
                    }
                }
                else
                {
                    f.cDroiteMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
                    GeneralClass.erreur7 = "";
                }
                //paf gauche
                if (Convert.ToInt32(f.pafGaucheLabel.Text) > Convert.ToInt32(f.pafGaucheMaxiLabel.Text))
                {
                    f.pafGaucheMaxiLabel.BackColor = System.Drawing.Color.HotPink;
                    GeneralClass.erreur8 = "Porte à faux gauche trop grand" + "\n";
                }
                else if (Convert.ToInt32(f.pafGaucheLabel.Text) < 0)
                {
                    f.pafGaucheMaxiLabel.BackColor = System.Drawing.Color.HotPink;
                    GeneralClass.erreur8 = "Attache gauche en dehors de la passerelle" + "\n";
                }
                else if (Convert.ToInt32(f.pafGaucheLabel.Text) < 20)
                {
                    f.pafGaucheMaxiLabel.BackColor = System.Drawing.Color.HotPink;
                    GeneralClass.erreur8 = "Distance attache-rive gauche extension inférieure à 20" + "\n";
                }
                else
                {
                    f.pafGaucheMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
                    GeneralClass.erreur8 = "";
                }

            //paf droit
            if (Convert.ToInt32(f.pafDroitLabel.Text) > Convert.ToInt32(f.pafDroitMaxiLabel.Text))
                {
                    f.pafDroitMaxiLabel.BackColor = System.Drawing.Color.HotPink;
                    GeneralClass.erreur9 = "Porte à faux droit trop grand" + "\n";
                }
                else if (Convert.ToInt32(f.pafDroitLabel.Text) < 0)
                {
                    f.pafDroitMaxiLabel.BackColor = System.Drawing.Color.HotPink;
                    GeneralClass.erreur9 = "Attache droite en dehors de la passerelle" + "\n";
                }
                else if (Convert.ToInt32(f.pafDroitLabel.Text) < 20)
                {
                    f.pafDroitMaxiLabel.BackColor = System.Drawing.Color.HotPink;
                    GeneralClass.erreur9 = "Distance attache-rive droite extension inférieure à 20" + "\n";
                }
                else
                {
                    f.pafDroitMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
                    GeneralClass.erreur9 = "";
                }
            //D
            if (passerelleSelectionneeElement.Name.Contains("M3") && (f.troisiemeAttacheCheckBox.Checked == true))
                {
                    if (Convert.ToInt32(f.d1Label.Text) > Convert.ToInt32(f.dMaxiLabel.Text))
                    {
                        f.d1MaxiLabel.BackColor = System.Drawing.Color.HotPink;
                        GeneralClass.erreur13 = "Distance entre attache gauche et troisieme attache trop grande" + "\n";
                    }
                    else
                    {
                        f.d1MaxiLabel.BackColor = System.Drawing.Color.LightGreen;
                        GeneralClass.erreur13 = "";
                    }
                    if (Convert.ToInt32(f.d2Label.Text) > Convert.ToInt32(f.dMaxiLabel.Text))
                    {
                        f.d2MaxiLabel.BackColor = System.Drawing.Color.HotPink;
                        GeneralClass.erreur14 = "Distance entre attache droite et troisieme attache trop grande" + "\n";
                    }
                    else
                    {
                        f.d2MaxiLabel.BackColor = System.Drawing.Color.LightGreen;
                        GeneralClass.erreur14 = "";
                    }

                }
                else if (Convert.ToInt32(f.dLabel.Text) > Convert.ToInt32(f.dMaxiLabel.Text))
                {
                    if (f.troisiemeAttacheCheckBox.Checked == false)
                    {
                        f.dMaxiLabel.BackColor = System.Drawing.Color.HotPink;
                        GeneralClass.erreur10 = "Distance entre attaches trop grande" + "\n";
                    }
                }
                else
                {
                    f.dMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
                    GeneralClass.erreur10 = "";
                }
            //banche selectionnée
            if (f.bancheComboBox.SelectedItem.ToString() == "Pas de banche")
                {
                    GeneralClass.erreur11 = "";
                }
                else
                {
                    if (f.stabilisationComboBox.SelectedItem == null)
                    {
                        GeneralClass.erreur11 = "Sélectionner une stabilisation de banche" + "\n";
                        f.stabilisationLabel.BackColor = System.Drawing.Color.HotPink;
                    }
                    else
                    {
                        f.stabilisationLabel.BackColor = System.Drawing.Color.Transparent;
                        GeneralClass.erreur11 = "";
                    }
                }
            //numéro passerelle
            if (string.IsNullOrEmpty(f.numeroTextBox.Text) || f.numeroTextBox.Text == "-")
                {
                    f.numeroTextBox.BackColor = System.Drawing.Color.HotPink;
                    GeneralClass.erreur12 = "Donner un numéro de passerelle" + "\n";
                }
                else
                {
                    f.numeroTextBox.BackColor = System.Drawing.Color.White;
                    GeneralClass.erreur12 = "";
                }
            #endregion test des valeurs
            
            #region affichage de l'erreur
            f.commentaireLabel.Text = GeneralClass.erreur4 + GeneralClass.erreur5 + GeneralClass.erreur3 + GeneralClass.erreur2 + GeneralClass.erreur6 + GeneralClass.erreur7 + GeneralClass.erreur8 + GeneralClass.erreur9 + GeneralClass.erreur10 + GeneralClass.erreur11 + GeneralClass.erreur12 + GeneralClass.erreur13 + GeneralClass.erreur14;
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
                #region Autodesk PQ - IS04
                //int IndexTest = (f.attacheDroiteComboBox.SelectedIndex + 1) * (f.attacheGaucheComboBox.SelectedIndex + 1) * (f.attacheCentreComboBox.SelectedIndex + 1) * (f.bancheComboBox.SelectedIndex + 1) * (f.extensionDroiteComboBox.SelectedIndex + 1) * (f.extensionGaucheComboBox.SelectedIndex + 1);
                //if (IndexTest == 0)
                //    f.applyButton.Enabled = false;
                //else
                //    f.applyButton.Enabled = true;
                //GeneralClass.TestIndexComboBox(f);
                int IndexTest = (f.attacheDroiteComboBox.SelectedIndex + 1) * (f.attacheGaucheComboBox.SelectedIndex + 1) * (f.bancheComboBox.SelectedIndex + 1) * (f.extensionDroiteComboBox.SelectedIndex + 1) * (f.extensionGaucheComboBox.SelectedIndex + 1);
                if (f.troisiemeAttacheCheckBox.Checked == true)
                    IndexTest *= (f.attacheCentreComboBox.SelectedIndex + 1);

                if (IndexTest == 0)
                    f.applyButton.Enabled = false;
                else
                    f.applyButton.Enabled = true;
                #endregion
            };
            #endregion actualisation des valeurs maxi et courantes des qu'on change soit Hbanche soit Angles droit/gauche
            #region initialisation extensions/ sabots d'appuis
            #region ajout des valeurs des extensions suivant passerelle selectionnee
            
            
            foreach (int i in ListExtLaterale)
            {
                f.extensionDroiteComboBox.Items.Add(i.ToString());
                f.extensionGaucheComboBox.Items.Add(i.ToString());
                f.extensionDroiteIntComboBox.Items.Add(i.ToString());
                f.extensionGaucheIntComboBox.Items.Add(i.ToString());
            }
            #endregion

            #region initialisation extensions
            if (passerelleSelectionneeElement.LookupParameter("Extension arriere").AsInteger() == vrai)
            {
                f.extArriereCheckBox.Checked = true;
            }
            else
            {
                f.extArriereCheckBox.Checked = false;
            }
            //Initialisation de l extension gauche
            try
            {
                f.extensionGaucheComboBox.SelectedIndex = f.extensionGaucheComboBox.FindStringExact
                        (GeneralClass.RetournePiedEnCM( 
                            passerelleSelectionneeElement.LookupParameter("Longueur extension gauche")
                            .AsDouble()).ToString());
            }
            catch
            {
            }

            try
            {
                f.extensionGaucheIntComboBox.SelectedIndex = f.extensionGaucheIntComboBox.FindStringExact
                        (GeneralClass.RetournePiedEnCM(passerelleSelectionneeElement.LookupParameter("Longueur extension gauche interieure").AsDouble()).ToString());
            }
            catch { }
            try
            {
                if (f.extensionGaucheIntComboBox.SelectedItem.ToString() == f.extensionGaucheComboBox.SelectedItem.ToString())
                {
                    f.angleBiaisGaucheCheckBox.Checked = false;
                    f.extensionGaucheIntComboBox.Visible = false;
                }
                else
                {
                    f.angleBiaisGaucheCheckBox.Checked = true;
                    f.extensionGaucheIntComboBox.Visible = true;
                }
            }
            catch { }
            //Initialisation de l'extension droite
            try
            {
                f.extensionDroiteComboBox.SelectedIndex = f.extensionDroiteComboBox.FindStringExact
                        (GeneralClass.RetournePiedEnCM(passerelleSelectionneeElement.LookupParameter("Longueur extension droite").AsDouble()).ToString());
            }
            catch { }
            try
            {
                f.extensionDroiteIntComboBox.SelectedIndex = f.extensionGaucheIntComboBox.FindStringExact
                    (GeneralClass.RetournePiedEnCM(passerelleSelectionneeElement.LookupParameter("Longueur extension droite interieure").AsDouble()).ToString());
            }
            catch {  }
            try
            {
                if (f.extensionDroiteIntComboBox.SelectedItem.ToString() == f.extensionDroiteComboBox.SelectedItem.ToString())
                {
                    f.angleBiaisDroitCheckBox.Checked = false;
                    f.extensionDroiteIntComboBox.Visible = false;
                }
                else
                {
                    f.angleBiaisDroitCheckBox.Checked = true;
                    f.extensionDroiteIntComboBox.Visible = true;
                }
            }
            catch { }
            #endregion initialisation extensions

            #region sabots d'appuis
            //initialisation des sabots d'appuis
            if (passerelleSelectionneeElement.LookupParameter("Sabot droit").AsInteger() == vrai)
            {
                f.angleDroitInterieurCheckBox.Checked = true;
                f.sabotDroitPictureBox.Visible = true;
                f.sabotDroitTextBox2.Visible = true;
                f.sabotDroitTrackBar.Visible = true;
                Int32 distanceSabotDroit = -Convert.ToInt32(passerelleSelectionneeElement.LookupParameter("Distance mur-sabot droit").AsValueString());
                f.sabotDroitTrackBar.Value = (distanceSabotDroit);
            }
            else
            {
                f.angleDroitInterieurCheckBox.Checked = false;
                f.sabotDroitPictureBox.Visible = false;
                f.sabotDroitTextBox2.Visible = false;
                f.sabotDroitTrackBar.Visible = false;
            }

            if (passerelleSelectionneeElement.LookupParameter("Sabot gauche").AsInteger() == vrai)
            {
                f.angleGaucheInterieurCheckBox.Checked = true;
                f.sabotGauchePictureBox.Visible = true;
                f.sabotGaucheTextBox2.Visible = true;
                f.sabotGaucheTrackBar.Visible = true;
                Int32 distanceSabotGauche = -Convert.ToInt32(passerelleSelectionneeElement.LookupParameter("Distance mur-sabot gauche").AsValueString());
                f.sabotGaucheTrackBar.Value = (distanceSabotGauche);
            }
            else
            {
                f.angleGaucheInterieurCheckBox.Checked = false;
                f.sabotGauchePictureBox.Visible = false;
                f.sabotGaucheTextBox2.Visible = false;
                f.sabotGaucheTrackBar.Visible = false;
            }
            //initialisation des distances des sabots
            f.sabotDroitTextBox2.Text = (-f.sabotDroitTrackBar.Value).ToString();
            f.sabotGaucheTextBox2.Text = (-f.sabotGaucheTrackBar.Value).ToString();
            //f.sabotDroitTextBox2.Text = GeneralClass.RetournePiedEnCM( passerelleSelectionneeElement.GetParameters("Distance mur-sabot gauche")[0].AsDouble()).ToString();
            //f.sabotGaucheTextBox2.Text = GeneralClass.RetournePiedEnCM(passerelleSelectionneeElement.GetParameters("Distance mur-sabot gauche")[0].AsDouble()).ToString();
            //f.sabotGaucheTextBoxFantome2.Text = (GeneralClass.RetournePiedEnCM(passerelleSelectionneeElement.LookupParameter("Distance mur-sabot gauche").AsDouble()) - 105).ToString();
            //f.sabotDroitTextBoxFantome2.Text = (GeneralClass.RetournePiedEnCM(passerelleSelectionneeElement.LookupParameter("Distance mur-sabot droit").AsDouble()) - 105).ToString();



            #endregion sabbot d'appuis
            #endregion initialisation extensions/ sabots d'appuis

            #region initialisation lgTot,attaches,c,paf,d
            //initialisation de GeneralClass.lgTotale
            if (f.angleBiaisGaucheCheckBox.Checked == true)
            {
                GeneralClass.lecturegauche1 = Math.Max(Convert.ToInt32(f.extensionGaucheComboBox.SelectedItem), Convert.ToInt32(f.extensionGaucheIntComboBox.SelectedItem));
            }
            else GeneralClass.lecturegauche1 = Convert.ToInt32(f.extensionGaucheComboBox.SelectedItem);

            if (f.angleBiaisDroitCheckBox.Checked == true)
            {
                GeneralClass.lecturedroite1 = Math.Max(Convert.ToInt32(f.extensionDroiteComboBox.SelectedItem), Convert.ToInt32(f.extensionDroiteIntComboBox.SelectedItem));
}
            else GeneralClass.lecturedroite1 = Convert.ToInt32(f.extensionDroiteComboBox.SelectedItem);
            GeneralClass.lgTotale = GeneralClass.lecturegauche1 + GeneralClass.lecturedroite1 + GeneralClass.lgFixe;
            f.lgTotaleLabel.Text = GeneralClass.lgTotale.ToString();

            //initialisation du type d'attache droite
            foreach (string str in f.attacheDroiteComboBox.Items)
            {
                if (passerelleSelectionneeElement.LookupParameter(str).AsInteger() == vrai)
                    f.attacheDroiteComboBox.SelectedIndex = f.attacheDroiteComboBox.FindStringExact(str);
            }
            //initialisation du type d'attache gauche
            foreach (string str in f.attacheGaucheComboBox.Items)
            {
                if (passerelleSelectionneeElement.LookupParameter("Gauche - " + str).AsInteger() == vrai)
                    f.attacheGaucheComboBox.SelectedIndex = f.attacheGaucheComboBox.FindStringExact(str);
            }
            //Initialisation GeneralClass.cGauche
            GeneralClass.distanceAttacheGauche = Convert.ToInt32(GeneralClass.RetournePiedEnCM(passerelleSelectionneeElement.LookupParameter("Distance attache gauche").AsDouble()));
            GeneralClass.cGauche = GeneralClass.distanceAttacheGauche - GeneralClass.lgFixe / 2;
            f.attacheGaucheTrackbarTextBox.Text = (GeneralClass.cGauche).ToString();

            //initialisation GeneralClass.cDroite
            GeneralClass.distanceAttacheDroite = Convert.ToInt32(GeneralClass.RetournePiedEnCM(passerelleSelectionneeElement.LookupParameter("Distance attache droite").AsDouble()));
            GeneralClass.cDroite = GeneralClass.distanceAttacheDroite - GeneralClass.lgFixe / 2;
            f.attacheDroiteTrackbarTextBox.Text = (GeneralClass.cDroite).ToString();
            //initialisation du type d'attache centre
            if (passerelleSelectionneeElement.Name.Contains("M3") && (f.bancheComboBox.SelectedIndex > 2))
            {
                foreach (string str in f.attacheCentreComboBox.Items)
                {
                    if (passerelleSelectionneeElement.LookupParameter("Centre - " + str).AsInteger() == vrai)
                    {
                        f.attacheCentreComboBox.SelectedIndex = f.attacheCentreComboBox.FindStringExact(str);
                        f.troisiemeAttacheCheckBox.Checked = true;
                        f.attacheCentreComboBox.Visible = true;
                        f.attacheCentreTrackbar.Visible = true;
                        f.dLabel.Visible = false;
                        f.dMaxiLabel.Visible = false;
                        f.d1Label.Visible = true;
                        f.d2Label.Visible = true;
                        f.d1MaxiLabel.Visible = true;
                        f.d2MaxiLabel.Visible = true;
                    }
                }
                GeneralClass.d1 = GeneralClass.lgFixe / 2 + /*GeneralClass.cGauche*/-(-f.attacheCentreTrackbar.Value);
                GeneralClass.d2 = GeneralClass.lgFixe / 2 + /*GeneralClass.cDroite*/-(+f.attacheCentreTrackbar.Value);
                f.d1Label.Text = GeneralClass.d1.ToString();
                f.d2Label.Text = GeneralClass.d2.ToString();
                GeneralClass.distanceAttacheCentre = GeneralClass.lgFixe / 2 + f.attacheCentreTrackbar.Value;
                try { f.attacheCentreTrackbar.Value = -(GeneralClass.lgTotale / 2 + GeneralClass.cGauche - GeneralClass.d1); }  //GeneralClass.distanceAttacheCentre - GeneralClass.lgFixe/2; // 2;*/  
                catch { };
                }
            else
            {
                f.dLabel.Visible = true;
                f.dMaxiLabel.Visible = true;
                f.d1Label.Visible = false;
                f.d2Label.Visible = false;
                f.d1MaxiLabel.Visible = false;
                f.d2MaxiLabel.Visible = false;
                f.attacheCentreComboBox.Visible = false;
                f.attacheCentreTrackbar.Visible = false;
            }

            //initialisation des paf
            GeneralClass.pafDroit = GeneralClass.lecturedroite1 - GeneralClass.cDroite;
            GeneralClass.pafGauche = GeneralClass.lecturegauche1 - GeneralClass.cGauche;
            f.pafGaucheLabel.Text = GeneralClass.pafGauche.ToString();
            f.pafDroitLabel.Text = GeneralClass.pafDroit.ToString();

            //d
            GeneralClass.d = GeneralClass.distanceAttacheDroite + GeneralClass.distanceAttacheGauche;
            f.dLabel.Text = GeneralClass.d.ToString();

            #endregion initialisation lgTot,attaches,c,paf,d



                #region buttons appliquer, reglage pied, options
                #region applybutton
                //applyButton

                f.attacheDroiteComboBox.SelectedValueChanged += (sender, e) =>
            {
                if (GeneralClass.cDroite > GeneralClass.cMax)
                {
                    if (f.troisiemeAttacheCheckBox.Checked == false)
                    {
                        f.cDroiteMaxiLabel.BackColor = System.Drawing.Color.HotPink;
                        GeneralClass.erreur7 = "Distance attache-ferme droite trop grande" + "\n";
                    }
                }
                else if (Math.Abs(GeneralClass.cDroite) < 15)
                {
                    if (f.attacheDroiteComboBox.SelectedIndex == 2)
                    {
                        if (Math.Abs(GeneralClass.cDroite) < 12)
                        {
                            f.cDroiteMaxiLabel.BackColor = System.Drawing.Color.HotPink;
                            GeneralClass.erreur7 = "Distance attache-ferme droite inférieure à 12" + "\n";
                        }
                        else
                        {
                            f.cDroiteMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
                            GeneralClass.erreur7 = "";
                        }
                    }
                    else
                    {
                        f.cDroiteMaxiLabel.BackColor = System.Drawing.Color.HotPink;
                        GeneralClass.erreur7 = "Distance attache-ferme droite inférieure à 15" + "\n";
                    }
                }
                else
                {
                    f.cDroiteMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
                    GeneralClass.erreur7 = "";
                }
                int IndexTest = (f.attacheDroiteComboBox.SelectedIndex + 1) * (f.attacheGaucheComboBox.SelectedIndex + 1) * (f.attacheCentreComboBox.SelectedIndex + 1) * (f.bancheComboBox.SelectedIndex + 1) * (f.extensionDroiteComboBox.SelectedIndex + 1) * (f.extensionGaucheComboBox.SelectedIndex + 1);
                if (IndexTest == 0)
                    f.applyButton.Enabled = false;
                else
                    f.applyButton.Enabled = true;
            };

            f.attacheGaucheComboBox.SelectedValueChanged += (sender, e) =>
            {
                if (GeneralClass.cGauche > GeneralClass.cMax)
                {
                    if (f.troisiemeAttacheCheckBox.Checked == false)
                    {
                        f.cGaucheMaxiLabel.BackColor = System.Drawing.Color.HotPink;
                        GeneralClass.erreur6 = "Distance attache-ferme gauche trop grande" + "\n";
                    }
                }
                else if (Math.Abs(GeneralClass.cGauche) < 15)
                {

                    if (f.attacheDroiteComboBox.SelectedIndex == 2)
                    {
                        if (Math.Abs(GeneralClass.cGauche) < 12)
                        {
                            f.cGaucheMaxiLabel.BackColor = System.Drawing.Color.HotPink;
                            GeneralClass.erreur6 = "Distance attache-ferme gauche inférieure à 12" + "\n";
                        }
                        else
                        {
                            f.cGaucheMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
                            GeneralClass.erreur6 = "";
                        }
                    }
                    else
                    {
                        f.cGaucheMaxiLabel.BackColor = System.Drawing.Color.HotPink;
                        GeneralClass.erreur6 = "Distance attache-ferme gauche inférieure à 15" + "\n";
                    }
                }
                else
                {
                    f.cGaucheMaxiLabel.BackColor = System.Drawing.Color.LightGreen;
                    GeneralClass.erreur6 = "";
                }
            };
            f.attacheCentreTrackbar.Value += 1;
            f.attacheCentreTrackbar.Value -= 1;
            GeneralClass.d1 = GeneralClass.lgFixe / 2 + GeneralClass.cGauche - (-f.attacheCentreTrackbar.Value);
            GeneralClass.d2 = GeneralClass.lgFixe / 2 + GeneralClass.cDroite - (+f.attacheCentreTrackbar.Value);
            f.d1Label.Text = GeneralClass.d1.ToString();
            f.d2Label.Text = GeneralClass.d2.ToString();
            GeneralClass.distanceAttacheCentre = GeneralClass.lgFixe / 2 + f.attacheCentreTrackbar.Value;

            f.applyButton.Click += (sender, e) =>
            {
                Transaction trButton = new Transaction(doc, "Configuration PTLG");
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
                        passerelleSelectionneeElement.LookupParameter("Distance mur-sabot gauche").SetValueString(f.sabotGaucheTextBox2.Text);
                    }
                    else passerelleSelectionneeElement.LookupParameter("Sabot gauche").Set(faux);
                    //reglage angle interieur droit
                    if (f.angleDroitInterieurCheckBox.Checked == true)
                    {
                        passerelleSelectionneeElement.LookupParameter("Sabot droit").Set(vrai);
                        passerelleSelectionneeElement.LookupParameter("Distance mur-sabot droit").SetValueString(f.sabotDroitTextBox2.Text);
                    }
                    else passerelleSelectionneeElement.LookupParameter("Sabot droit").Set(faux);

                    #endregion angle interieur
                    
                    #region extensions
                    passerelleSelectionneeElement.LookupParameter("Longueur extension droite").SetValueString(f.extensionDroiteComboBox.SelectedItem.ToString());
                    passerelleSelectionneeElement.LookupParameter("Longueur extension gauche").SetValueString(f.extensionGaucheComboBox.SelectedItem.ToString() );
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
                    //reglage attache centre
                    if (passerelleSelectionneeElement.Name.Contains("M3"))
                    {
                        if (f.troisiemeAttacheCheckBox.Checked == true)
                        {
                            passerelleSelectionneeElement.LookupParameter("3ème attache").Set(vrai);
                            foreach (string str in f.attacheCentreComboBox.Items)
                            {
                                if (f.attacheCentreComboBox.SelectedIndex == f.attacheCentreComboBox.FindStringExact(str))
                                {
                                    passerelleSelectionneeElement.LookupParameter("Centre - " + str).Set(vrai);
                                    passerelleSelectionneeElement.LookupParameter("Type d'attache 3").Set(str);
                                }
                                else passerelleSelectionneeElement.LookupParameter("Centre - " + str).Set(faux);
                            }
                            passerelleSelectionneeElement.LookupParameter("Distance attache centre").SetValueString((GeneralClass.distanceAttacheCentre + GeneralClass.cGauche + GeneralClass.pafGauche).ToString());
                        }
                        else
                        {
                            passerelleSelectionneeElement.LookupParameter("3ème attache").Set(faux);
                            foreach (string str in f.attacheCentreComboBox.Items)
                            {
                                passerelleSelectionneeElement.LookupParameter("Centre - " + str).Set(faux);
                                passerelleSelectionneeElement.LookupParameter("Type d'attache 3").Set("");
                            }
                        }
                        
                    }
                    #endregion attaches
                    #region distances
                    passerelleSelectionneeElement.LookupParameter("Distance attache gauche").SetValueString(GeneralClass.distanceAttacheGauche.ToString());
                    passerelleSelectionneeElement.LookupParameter("Distance attache droite").SetValueString(GeneralClass.distanceAttacheDroite.ToString());

                    #endregion distances
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
                    if ((f.bancheComboBox.SelectedIndex > 0) && (f.stabilisationComboBox.SelectedItem == null))
                    {
                        TaskDialog.Show("Action utilisateur", "Sélectionner une stabilisation puis appuyer sur le bouton \"Appliquer\" pour calculer les réactions");
                    }
                    else
                    {
                        #region REACTIONS
                        
                        int ligneReaction = f.stabilisationComboBox.SelectedIndex * 30 +f.attacheDroiteComboBox.SelectedIndex*5+ f.bancheComboBox.SelectedIndex - 1;
                        passerelleSelectionneeElement.LookupParameter("RH").Set(GeneralClass.LectureCSVstr("Reactions", ligneReaction, 3));
                        passerelleSelectionneeElement.LookupParameter("RV").Set(GeneralClass.LectureCSVstr("Reactions", ligneReaction, 4));
                        passerelleSelectionneeElement.LookupParameter("RU").Set(GeneralClass.LectureCSVstr("Reactions", ligneReaction, 5));
                        passerelleSelectionneeElement.LookupParameter("RA").Set(GeneralClass.LectureCSVstr("Reactions", ligneReaction, 6));
                        passerelleSelectionneeElement.LookupParameter("RB").Set(GeneralClass.LectureCSVstr("Reactions", ligneReaction, 7));
                        passerelleSelectionneeElement.LookupParameter("RC").Set(GeneralClass.LectureCSVstr("Reactions", ligneReaction, 8));
                        passerelleSelectionneeElement.LookupParameter("R2").Set(GeneralClass.LectureCSVstr("Reactions", ligneReaction, 9));
                        passerelleSelectionneeElement.LookupParameter("R1").Set(GeneralClass.LectureCSVstr("Reactions", ligneReaction, 10));
                        passerelleSelectionneeElement.LookupParameter("RH'").Set(GeneralClass.LectureCSVstr("Reactions", ligneReaction, 11));
                        
                        #endregion REACTIONS
                    }
                    //passerelle rouge
                    if ((f.commentaireLabel.Text != "") || (((f.bancheComboBox.SelectedIndex > 0) && (f.stabilisationComboBox.SelectedItem == null))))
                    {
                        passerelleSelectionneeElement.LookupParameter("Passerelle incorrecte").Set(vrai);
                    }
                    else passerelleSelectionneeElement.LookupParameter("Passerelle incorrecte").Set(faux);
                    //fin de la transaction
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

                    if (f.angleGaucheCheckBox.Checked==true && f.extArriereCheckBox.Checked==true)
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


                    trButton.Commit();
                }
            };

            #endregion applybutton
            /////////
            #region footButton
            using (PiedForm g = new PiedForm())
            {
                f.footButton.Click += (sendor, o) =>
                {
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
                                 ((GeneralClass.RetournePiedEnCM((passerelleSelectionneeElement.LookupParameter("Longueur pied vertical gauche").AsDouble()) )).ToString());
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
                                try {
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
                        if (g.piedDroitVerticalComboBox.Items.Contains((GeneralClass.RetournePiedEnCM(passerelleSelectionneeElement.LookupParameter("Longueur pied vertical droit").AsDouble()) ).ToString()))
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
                                {;
                                    g.rallonge180DroiteComboBox.SelectedIndex = g.rallonge180DroiteComboBox.FindStringExact(GeneralClass.RetournePiedEnCM(passerelleSelectionneeElement.LookupParameter("Longueur pied vertical droit").AsDouble()).ToString());
                                }
                                catch { }
                            }
                            else {
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
                            if (g.rallongeGauche105CheckBox.Checked == false && g.rallongeGauche180CheckBox.Checked==false)
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
                };


                    #endregion footButton
                    #region optionsButton
                    //optionsButton
                    using (optionsForm op = new optionsForm())
                    {
                        f.optionsButton.Click += (sendur, u) =>
                        {
                            #region Initialisation
                            #region  Maximum des scrollbar retrait auvent en fonction de la longueur de la passerelle
                            op.RetraitDScrollbar.Maximum = Convert.ToInt32(GeneralClass.RetournePiedEnCM(passerelleSelectionneeElement.LookupParameter("Longueur extension droite").AsDouble()));
                            op.RetraitGScrollbar.Maximum = Convert.ToInt32(GeneralClass.RetournePiedEnCM(passerelleSelectionneeElement.LookupParameter("Longueur extension gauche").AsDouble()));
                            #endregion

                            #region Retrait auvent gauche
                            try
                            {
                                op.RetraitGScrollbar.Value = Convert.ToInt32(GeneralClass.RetournePiedEnCM(passerelleSelectionneeElement.LookupParameter("Retrait gauche").AsDouble()));
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
                                op.RetraitDScrollbar.Value = Convert.ToInt32(GeneralClass.RetournePiedEnCM(passerelleSelectionneeElement.LookupParameter("Retrait droit").AsDouble()));
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
                            op.GCGScrollBar.Value = Convert.ToInt32(GeneralClass.RetournePiedEnCM(passerelleSelectionneeElement.LookupParameter("Retrait GC gauche").AsDouble()));
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
                                op.GCDScrollBar.Value = Convert.ToInt32(GeneralClass.RetournePiedEnCM(passerelleSelectionneeElement.LookupParameter("Retrait GC droit").AsDouble()));
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
                            op.DecoupeGDisttextBox.Text = GeneralClass.RetournePiedEnCM(passerelleSelectionneeElement.LookupParameter("LG_MET_PTLG_Decoupe 1 distance au bord").AsDouble()).ToString();
                            op.DecoupeGLrgtextBox.Text = GeneralClass.RetournePiedEnCM(passerelleSelectionneeElement.LookupParameter("LG_MET_PTLG_Decoupe 1 Largeur").AsDouble()).ToString();
                            op.DecoupeGProftextBox.Text = GeneralClass.RetournePiedEnCM(passerelleSelectionneeElement.LookupParameter("LG_MET_PTLG_Decoupe 1 profondeur").AsDouble()).ToString();
                            
                            if (passerelleSelectionneeElement.LookupParameter("LG_MET_PTLG_Decoupe gauche").AsInteger() == 1)
                            {
                                op.DecoupeGcheckBox.Checked = true;
                                op.DecoupeGDisttextBox.Enabled = true;
                                op.DecoupeGLrgtextBox.Enabled = true;
                                op.DecoupeGProftextBox.Enabled = true;
                            }
                            else
                            {
                                op.DecoupeGcheckBox.Checked= false;
                                op.DecoupeGDisttextBox.Enabled = false;
                                op.DecoupeGLrgtextBox.Enabled = false;
                                op.DecoupeGProftextBox.Enabled = false;
                            }
                            #endregion

                            #region Decoupe droite
                            op.DecoupeDDisttextBox.Text = GeneralClass.RetournePiedEnCM(passerelleSelectionneeElement.LookupParameter("LG_MET_PTLG_Decoupe 2 distance au bord").AsDouble()).ToString();
                            op.DecoupeDLrgtextBox.Text = GeneralClass.RetournePiedEnCM(passerelleSelectionneeElement.LookupParameter("LG_MET_PTLG_Decoupe 2 Largeur").AsDouble()).ToString();
                            op.DecoupeDProftextBox.Text = GeneralClass.RetournePiedEnCM(passerelleSelectionneeElement.LookupParameter("LG_MET_PTLG_Decoupe 2 Profondeur").AsDouble()).ToString();
                            if (passerelleSelectionneeElement.LookupParameter("LG_MET_PTLG_Decoupe droite").AsInteger() == 1)
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
                            if (passerelleSelectionneeElement.GetParameters("U bas droit")[0].AsInteger() == 1||
                            passerelleSelectionneeElement.GetParameters("U bas gauche")[0].AsInteger() == 1)
                                op.uBasCheckBox.Checked = true;
                            else
                                op.uBasCheckBox.Checked = false;
                            #endregion

                            #region Bracon
                            if (
                                passerelleSelectionneeElement.GetParameters("Bracon")[0].AsInteger() == 1 ||
                                passerelleSelectionneeElement.GetParameters("Bracon gauche")[0].AsInteger() == 1
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
                                    passerelleSelectionneeElement.LookupParameter("Retrait gauche").Set(GeneralClass.RetourneCmEnPieds(op.RetraitGScrollbar.Value));
                                    passerelleSelectionneeElement.LookupParameter("Retrait droit").Set(GeneralClass.RetourneCmEnPieds(op.RetraitDScrollbar.Value));
                                    passerelleSelectionneeElement.LookupParameter("Retrait GC gauche").Set(GeneralClass.RetourneCmEnPieds(op.GCGScrollBar.Value));
                                    passerelleSelectionneeElement.LookupParameter("Retrait GC droit").Set(GeneralClass.RetourneCmEnPieds(op.GCDScrollBar.Value));


                                    if (op.uBasCheckBox.Checked == true)
                                    {
                                        passerelleSelectionneeElement.LookupParameter("U bas droit").Set(1);
                                        passerelleSelectionneeElement.LookupParameter("U bas gauche").Set(1);
                                    }
                                    else
                                    {
                                        passerelleSelectionneeElement.LookupParameter("U bas droit").Set(0);
                                        passerelleSelectionneeElement.LookupParameter("U bas gauche").Set(0);
                                    }

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

                                    if (op.DecoupeGcheckBox.Checked == true)
                                    {
                                        try
                                        {
                                            passerelleSelectionneeElement.LookupParameter("LG_MET_PTLG_Decoupe gauche").Set(vrai);
                                            passerelleSelectionneeElement.LookupParameter("LG_MET_PTLG_Decoupe 1 distance au bord").Set(GeneralClass.RetourneCmEnPieds(Convert.ToDouble(op.DecoupeGDisttextBox.Text)));
                                            passerelleSelectionneeElement.LookupParameter("LG_MET_PTLG_Decoupe 1 Largeur").Set(GeneralClass.RetourneCmEnPieds(Convert.ToDouble(op.DecoupeGLrgtextBox.Text)));
                                            passerelleSelectionneeElement.LookupParameter("LG_MET_PTLG_Decoupe 1 profondeur").Set(GeneralClass.RetourneCmEnPieds(Convert.ToDouble(op.DecoupeGProftextBox.Text)));
                                        }
                                        catch
                                        {
                                            TaskDialog.Show("Erreur", "Une valeur nom numérique a été saisie pour la découpe gauche, la modification ne sera pas prise en compte");
                                        }
                                        }
                                    else
                                    {
                                        passerelleSelectionneeElement.LookupParameter("LG_MET_PTLG_Decoupe gauche").Set(faux);
                                    }

                                    if (op.DecoupeDcheckBox.Checked == true)
                                    {
                                        try
                                        {
                                            passerelleSelectionneeElement.LookupParameter("LG_MET_PTLG_Decoupe droite").Set(vrai);
                                            passerelleSelectionneeElement.LookupParameter("LG_MET_PTLG_Decoupe 2 distance au bord").Set(GeneralClass.RetourneCmEnPieds(Convert.ToDouble(op.DecoupeDDisttextBox.Text)));
                                            passerelleSelectionneeElement.LookupParameter("LG_MET_PTLG_Decoupe 2 Largeur").Set(GeneralClass.RetourneCmEnPieds(Convert.ToDouble(op.DecoupeDLrgtextBox.Text)));
                                            passerelleSelectionneeElement.LookupParameter("LG_MET_PTLG_Decoupe 2 Profondeur").Set(GeneralClass.RetourneCmEnPieds(Convert.ToDouble(op.DecoupeDProftextBox.Text)));
                                        }
                                        catch
                                        {
                                            TaskDialog.Show("Erreur", "Une valeur nom numérique a été saisie pour la découpe gauche, la modification ne sera pas prise en compte");
                                        }
                                    }
                                    else
                                    {
                                        passerelleSelectionneeElement.LookupParameter("LG_MET_PTLG_Decoupe droite").Set(faux);
                                    }

                                    trOptions.Commit();
                                };
                            #endregion Apply Button Foot

                            op.ShowDialog();
                        };

                        #endregion optionsButton
                        #endregion buttons appliquer, reglage pied, options
                  

                    f.ShowDialog();
            }

            }

                #region Affichage de l'erreur si la passerelle est incorrecte
                if (passerelleSelectionneeElement.LookupParameter("Passerelle incorrecte").AsInteger() == vrai)
                {
                    TaskDialog.Show("Information importante", "Attention la passerelle n'est pas conforme");
                }
            #endregion Affichage de l'erreur si la passerelle est incorrecte
            /*
                        #region Réinitialisation
                        //création des valeurs maxi
                    public static int lgMax = new int();
                    public static int leMax = new int();
                    public static int cMax = new int();
                    public static int dMax = new int();
                    public static int leMaxAngle = new int();
                    public static int pafMax = new int();
                    public static int pafMaxAngle = new int();

                    //création des autres variables
                    public static int pafDroit = new int();
                    public static int pafGauche = new int();
                    public static int cGauche = new int();
                    public static int cDroite = new int();
                    public static int distanceAttacheDroite = new int();
                    public static int distanceAttacheGauche = new int();
                    public static int distanceAttacheCentre = new int();
                    public static int lgFixe = new int();
                    public static int lgTotale = new int();
                    public static int d = new int();
                    public static int d1 = new int();
                    public static int d2 = new int();
                    public static int lecturegauche = new int();
                    public static int lecturedroite = new int();
                    public static int lecturegauche1 = new int();
                    public static int lecturedroite1 = new int();

                    //création des variables pour les réactions
                    public static string stabilisation = null;
                    public static string hauteurBanche = null;
                    public static string attache = null;
                    #endregion reinitialisation
                */
            //TaskDialog.Show("test", "supprimer");
            return Result.Succeeded;

        }//namespace
    }
}