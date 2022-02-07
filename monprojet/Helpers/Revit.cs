#region Autodesk PQ - IS04
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using static PTLGClassLibrary.GeneralClass;

namespace PTLGClassLibrary.Helpers
{
    public static class Revit
    {
		public static void UpdateGeneralClassDataFromElem(Element e)
		{
			#region get data
			//variables definies pour tout le code
			int vrai = 1;
			int faux = 0;

			ListExtLaterale = new List<int>();

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

			//int extArrier = e.LookupParameter("Extension arriere").AsInteger();
			int troisAttache = e.GetParameters("3ème attache")[0].AsInteger();
			int quatrAttache = e.GetParameters("4ème attache")[0].AsInteger();
			double extensionGauche = RetournePiedEnCM(e.LookupParameter("Longueur extension gauche").AsDouble());
			double extensionGaucheInt = RetournePiedEnCM(e.LookupParameter("Longueur extension gauche interieure").AsDouble());
			double extensionDroite = RetournePiedEnCM(e.LookupParameter("Longueur extension droite").AsDouble());
			double extensionDroiteInt = RetournePiedEnCM(e.LookupParameter("Longueur extension droite interieure").AsDouble());

			#endregion get data

			#region lgFixe
			if (type == "M1")
				lgFixe = 70;
			if (type == "M2")
				lgFixe = 180;
			if (type == "M3")
				lgFixe = 330;

			HalfLgFixe = (int)Math.Round((double)lgFixe / 2, MidpointRounding.AwayFromZero);
			QuarterLgFixe = (int)Math.Round((double)lgFixe / 4, MidpointRounding.AwayFromZero);
			#endregion

			#region initialisation lgTot,attaches,c,paf,d
			if (Math.Abs(extensionGauche - extensionGaucheInt) < 0.01) // sont egales
			{
				lecturegauche = Convert.ToInt32(extensionGauche);
			}
			else lecturegauche = Math.Max(Convert.ToInt32(extensionGauche), Convert.ToInt32(extensionGaucheInt));

			if (Math.Abs(extensionDroite - extensionDroiteInt) < 0.01)
			{
				lecturedroite = Convert.ToInt32(extensionDroite);
			}
			else lecturedroite = Math.Max(Convert.ToInt32(extensionDroite), Convert.ToInt32(extensionDroiteInt));

			lgTotale = lecturegauche + lecturedroite + lgFixe;

			//Initialisation GeneralClass.cGauche
			distanceAttacheGauche = Convert.ToInt32(RetournePiedEnCM(e.LookupParameter("Distance attache gauche").AsDouble()));
			cGauche = distanceAttacheGauche - HalfLgFixe;

			//initialisation GeneralClass.cDroite
			distanceAttacheDroite = Convert.ToInt32(RetournePiedEnCM(e.LookupParameter("Distance attache droite").AsDouble()));
			cDroite = distanceAttacheDroite - HalfLgFixe;

			//initialisation des paf
			pafDroit = lecturedroite - cDroite;
			pafGauche = lecturegauche - cGauche;

			//initialisation AttacheCentre & AttacheCentreDroit
			distanceAttacheCentre = Convert.ToInt32(RetournePiedEnCM(e.GetParameters("Distance attache centre")[0].AsDouble()));
			distanceAttacheCentreDroit = Convert.ToInt32(RetournePiedEnCM(e.GetParameters("Distance attache centre droit")[0].AsDouble()));

			//initialisation GeneralClass.cCentre
			cCentre = distanceAttacheCentre - cGauche - pafGauche;

			//initialisation GeneralClass.cCentreDroite
			if (troisAttache == vrai && quatrAttache == faux)
				cCentreDroite = cDroite + pafDroit + distanceAttacheCentre;
			else if (quatrAttache == vrai)
				cCentreDroite = cDroite + pafDroit + distanceAttacheCentreDroit;

			//d
			d = distanceAttacheDroite + distanceAttacheGauche;
			#endregion

			#region 3 et 4 attache
			if (troisAttache == vrai && quatrAttache == faux)
			{
				d1 = distanceAttacheCentre - pafGauche;
				d2 = lgTotale - pafDroit - distanceAttacheCentre;
			}
			else if (quatrAttache == vrai)
			{
				d1 = distanceAttacheCentre - pafGauche;
				d2 = distanceAttacheCentreDroit - pafDroit;
				d = lgFixe + pafGauche + pafDroit - d1 - d2;
			}
			#endregion 3 et 4 attache

		}

		public static Element PickWalkway(UIDocument uidoc)
        {
            ISelectionFilter selFilter = new GenModSelectionFilter();
            Selection sel = uidoc.Selection;
            Reference pickedref = sel.PickObject(ObjectType.Element, selFilter, "Sélectionner une passerelle");
            Element elem = uidoc.Document.GetElement(pickedref);
            string NameCei = elem.Name.ToString();
            return elem;
        }

        public static List<Element> GetAllGenModFamilies(Document doc)
        {
            List<Element> outFam = new List<Element>();
            FilteredElementCollector collectorI = new FilteredElementCollector(doc);
            ICollection<Element> elemsI = collectorI.OfClass(typeof(FamilyInstance)).ToElements();
            foreach (Element e in elemsI)
            {
                if (e.Category.Id.IntegerValue == (int)BuiltInCategory.OST_GenericModel)
                    outFam.Add(e);
            }
            return outFam;
        }

        public static List<Family> GetAllGenModFamiliesOrig(Document doc)
        {
            List<Family> outFam = new List<Family>();
            FilteredElementCollector collectorI = new FilteredElementCollector(doc);
            ICollection<Element> elemsI = collectorI.OfClass(typeof(Family)).ToElements();
            foreach (Element e in elemsI)
            {
                Family f = e as Family;
                if (f.FamilyCategoryId.IntegerValue == (int)BuiltInCategory.OST_GenericModel)
                {
                    outFam.Add(f);
                }
            }
            return outFam;
        }


        private class GenModSelectionFilter : ISelectionFilter
        {
            public bool AllowElement(Element e)
            {
                return (e.Category.Id.IntegerValue.Equals((int)BuiltInCategory.OST_GenericModel));
            }
            public bool AllowReference(Reference r, XYZ p)
            {
                return false;
            }
        }

        public static int GetWindowsScaling()
        {
            return (int)(Screen.PrimaryScreen.Bounds.Width / SystemParameters.PrimaryScreenWidth);
        }
    }
}
#endregion