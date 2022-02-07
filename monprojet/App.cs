using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace PTLGClassLibrary
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class App : IExternalApplication
    {
        static AddInId m_appId = new AddInId(new Guid("356CDA5A-E6C7-4c2f-A9EF-B3222116C9D9"));

        // get the absolute path of this assembly
        static string ExecutingAssemblyPath = Assembly.GetExecutingAssembly().Location;

        public Result OnStartup(UIControlledApplication application)
        {
            #region Autodesk PQ - IS04
            ///
            /// UNCOMMENT BEFORE DELIVER
            /// 

            string domainString = System.DirectoryServices.ActiveDirectory.Domain.GetComputerDomain().ToString();
            if (domainString == "LEON-GROSSE.FR")
            {
                // Call this method explicitly in App.cs when Revit starts up because 
                // in .Net 4, the static variables will not be initialized until use them,*/

                AddMenu(application);
                AddAppDocEvents(application.ControlledApplication);
                return Result.Succeeded;
            }
            else
            {
                TaskDialog.Show("Erreur", "Non autorisé");
                return Result.Failed;
            }
            //return Autodesk.Revit.UI.Result.Failed;




            ///
            /// COMMENT BEFORE DELIVER
            /// 

            //AddMenu(application);
            //AddAppDocEvents(application.ControlledApplication);

            //return Result.Succeeded;
            #endregion
        }
        public Result OnShutdown(UIControlledApplication application)
        {
            RemoveAppDocEvents();

            return Result.Succeeded;
        }
        private void AddMenu(UIControlledApplication app)
        {
            // Creation de la Tab LEON GROSSE si ca n'existe pas
            string tabName = "LEON GROSSE";
            // On essaie de créer le Tab. Si ca ne fonctionne pas ca veut dire qu'il est déjà créé
            try { app.CreateRibbonTab(tabName); }
            catch { };
            
            //Creation du ribbonpanel
            RibbonPanel rvtRibbonPanel = null;
            try { rvtRibbonPanel = app.CreateRibbonPanel(tabName, "Méthodes - Sécurité"); }
            catch
            {
                foreach (RibbonPanel rbbpanel in app.GetRibbonPanels("LEON GROSSE"))
                {
                    if (rbbpanel.Name == "Méthodes - Sécurité") rvtRibbonPanel = rbbpanel;
                }
            };

            #region Autodesk PQ - IS04
            /// Bouton Configurateur ORIGINAL ===> supprimer/cacher
            //PushButton configurateurBtnORIG = rvtRibbonPanel.AddItem(new PushButtonData(
            //    "configurationPTLG", "Configurateur" + "\r\n" + "général orig", ExecutingAssemblyPath, "PTLGClassLibrary.ConfigurateurClass")) as PushButton;
            
            //BitmapImage largeImageORIG = new BitmapImage(new Uri("pack://application:,,,/PTLGClassLibrary;component/RessourcesApp/PTLG_3D.png"));
            //configurateurBtnORIG.LargeImage = largeImageORIG;
            //configurateurBtnORIG.ToolTip = "Ouvre le Configurateur de passerelle PTLG";


            /// Bouton Configurateur GENERAL
            PushButton configurateurGenBtn = rvtRibbonPanel.AddItem(new PushButtonData(
                "configurationGenPTLG", "Configurateur" + "\r\n" + "général", ExecutingAssemblyPath, "PTLGClassLibrary.ConfigurateurGenClass")) as PushButton;

            BitmapImage largeImageGen = new BitmapImage(new Uri("pack://application:,,,/PTLGClassLibrary;component/RessourcesApp/PTLG_3D.png"));
            configurateurGenBtn.LargeImage = largeImageGen;
            configurateurGenBtn.ToolTip = "Ouvre le Configurateur de passerelle PTLG";


            /// Bouton Configurateur MANUEL
            PushButton configurateurManBtn = rvtRibbonPanel.AddItem(new PushButtonData(
                "configurationManPTLG", "Configurateur" + "\r\n" + "manuel", ExecutingAssemblyPath, "PTLGClassLibrary.ConfigurateurManClass")) as PushButton;

            BitmapImage largeImageMan = new BitmapImage(new Uri("pack://application:,,,/PTLGClassLibrary;component/RessourcesApp/PTLG_3D_Man.png"));
            configurateurManBtn.LargeImage = largeImageMan;
            configurateurManBtn.ToolTip = "Ouvre le Configurateur manuel de passerelle PTLG";


            /// Bouton Configurateur ALTITUDE
            PushButton configurateurAltBtn = rvtRibbonPanel.AddItem(new PushButtonData(
                "configurationManPTLGE", "Altitude des" + "\r\n" + "passerelles", ExecutingAssemblyPath, "PTLGClassLibrary.Elevations")) as PushButton;

            BitmapImage largeImageAlt = new BitmapImage(new Uri("pack://application:,,,/PTLGClassLibrary;component/RessourcesApp/PTLG_3D_As.png"));
            configurateurAltBtn.LargeImage = largeImageAlt;
            configurateurAltBtn.ToolTip = "Met à jour le paramètre d'altitude de toutes les passerelle PTLG";
            #endregion



            /// Bouton Configurateur PIEDS
            PushButton piedBtn = rvtRibbonPanel.AddItem(new PushButtonData(
               "configurationPied", "Configurateur de" + "\r\n" + "Pied", ExecutingAssemblyPath, "PTLGClassLibrary.PiedClass")) as PushButton;
            piedBtn.ToolTip = "Ouvre le Configurateur de pied des passerelles PTLG";
            
            BitmapImage configpiedLogo = new BitmapImage(new Uri("pack://application:,,,/PTLGClassLibrary;component/RessourcesApp/Pied_ptlg.png"));         
            piedBtn.LargeImage = configpiedLogo;
             
            
            /*
            // ajout du bouton pour la modelisation automatique
            PushButton modelisationBtn = rvtRibbonPanel.AddItem(new PushButtonData(
              "modelisationauto", "Calepinage" + "\r\n" + "automatique", ExecutingAssemblyPath, "PTLGClassLibrary.ModelisationAuto")) as PushButton;
            
            BitmapImage modelisationautoLogo = new System.Windows.Media.Imaging.BitmapImage(new Uri(
                "pack://application:,,,/PTLGClassLibrary;component/RessourcesApp/Calepinage_Auto_PTLG.png"));

            modelisationBtn.LargeImage = modelisationautoLogo;
            modelisationBtn.ToolTip = "Créer un calepinage automatique de PTLG. Pour cela, selectionner une ligne puis cliquer sur le plugin. Vous serez alors guider pour la création du calepinage.";
            */


            
        
        }
        private void AddAppDocEvents(Autodesk.Revit.ApplicationServices.ControlledApplication app)
        {

        }

        private void RemoveAppDocEvents()
        {

        }

    }
}
