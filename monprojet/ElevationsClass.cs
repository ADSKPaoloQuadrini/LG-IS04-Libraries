#region Autodesk PQ - IS04
using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace PTLGClassLibrary
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class ElevationsClass : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            #region parametres pour code
            //Get application and document objects
            UIApplication uiApp = commandData.Application;
            UIDocument uidoc = uiApp.ActiveUIDocument;
            Document doc = uiApp.ActiveUIDocument.Document;

            #endregion parametres pour code

            #region liste des passerelles
            //creation de la liste de toutes les passerelles
            List<Element> allGenMod = Helpers.Revit.GetAllGenModFamilies(doc);
            List<Element> allPasserelles = new List<Element>();
            foreach(Element e in allGenMod)
            {
                if (e.Name.Contains("M1") || e.Name.Contains("M2") || e.Name.Contains("M3")) allPasserelles.Add(e);
            }
            #endregion liste des passerelles


            #region elevation passerelle

            string erreur = "";

            foreach (Element e in allPasserelles)
            {
                double h = GeneralClass.RetournePiedEnCM((e.Location as LocationPoint).Point.Z);
                Transaction trElevPass = new Transaction(doc, "Configuration PTLG - Elevation passerelle");
                {
                    trElevPass.Start();
                    try
                    {
                        e.LookupParameter("LG_MET_PTLG_Elevation passerelle").SetValueString(h.ToString());
                        trElevPass.Commit();
                    }
                    catch
                    {
                        erreur = erreur + e.Id.ToString() + "; ";
                        trElevPass.RollBack();
                    }
                }
            }

            if (erreur != "")
            {
                TaskDialog.Show("Warning", "Les éléments suivantes n'ont pas le paramètre 'LG_MET_PTLG_Elevation passerelle' :\n" + erreur);
            }

            #endregion elevation passerelle

            return Result.Succeeded;

        }
    }
}
#endregion