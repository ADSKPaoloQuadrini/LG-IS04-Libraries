using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Autodesk.Revit.DB;

namespace PTLGClassLibrary
{
	class GeneralClass
	{
		#region Global Parameters
		#region Autodesk PQ - IS04
		public static int vrai = 1;
		public static int faux = 0;

		public static string type = "";

		public static List<int> ListExtLaterale = new List<int>();

		public static int HalfLgFixe = new int();
		public static int QuarterLgFixe = new int();
        #endregion

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
		#region Autodesk PQ - IS04
		public static int cCentre = new int();
		public static int cCentreDroite = new int();
        #endregion
        public static int distanceAttacheDroite = new int();
		public static int distanceAttacheGauche = new int();
		public static int distanceAttacheCentre = new int();
		#region Autodesk PQ - IS04
		public static int distanceAttacheCentreDroit = new int();
        #endregion
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

		//création des variables erreurs
		public static string erreur1 = null;
		public static string erreur2 = null;
		public static string erreur3 = null;
		public static string erreur4 = null;
		public static string erreur5 = null;
		public static string erreur6 = null;
		public static string erreur7 = null;
		public static string erreur8 = null;
		public static string erreur9 = null;
		public static string erreur10 = null;
		public static string erreur11 = null;
		public static string erreur12 = null;
		public static string erreur13 = null;
		public static string erreur14 = null;
		#endregion

		#region Autodesk PQ - IS04
		//public static void TestIndexComboBox(ConfigurateurForm f)
		//{
		//	int IndexTest = (f.attacheDroiteComboBox.SelectedIndex + 1) * (f.attacheGaucheComboBox.SelectedIndex + 1) * (f.bancheComboBox.SelectedIndex + 1) * (f.extensionDroiteComboBox.SelectedIndex + 1) * (f.extensionGaucheComboBox.SelectedIndex + 1);
		//	if (f.troisiemeAttacheCheckBox.Checked == true)
		//		IndexTest *= (f.attacheCentreComboBox.SelectedIndex + 1);

		//	if (IndexTest == 0)
		//		f.applyButton.Enabled = false;
		//	else
		//		f.applyButton.Enabled = true;
		//}
		#endregion

		public static int LectureCSVint(string name, int ligne, int colonne)
		{
			string ssPath = Assembly.GetExecutingAssembly().Location.Replace("bin\\Debug\\PTLGClassLibrary.dll", "Resources");
			//A modifier//
			//String ssPath = "C:\\Users\\asabatier\\Documents\\CodeSource\\revit-lg-ptlg\\monprojet\\Resources";

			StreamReader reader = new StreamReader(File.OpenRead(ssPath + "\\" + name + ".csv"));

			for (int i = 0; i < ligne; i++)
			{
				reader.ReadLine();
			}
			int result = Convert.ToInt32(Convert.ToDouble(reader.ReadLine().Split(';')[colonne]));
			return result;
		}

		public static string LectureCSVstr(string name, int ligne, int colonne)
		{
			string ssPath = Assembly.GetExecutingAssembly().Location.Replace("bin\\Debug\\PTLGClassLibrary.dll", "Resources");
			//A modifier//
			//String ssPath = "C:\\Users\\asabatier\\Documents\\CodeSource\\revit-lg-ptlg\\monprojet\\Resources";
			StreamReader reader = new StreamReader(File.OpenRead(ssPath + "\\" + name + ".csv"));
			for (int i = 0; i < ligne; i++)
			{
				reader.ReadLine();
			}
			return reader.ReadLine().Split(';')[colonne];
		}

		public static double RetournePiedEnCM(double nomduparametre)
		{
			double dble = Convert.ToDouble(UnitUtils.ConvertFromInternalUnits(nomduparametre, DisplayUnitType.DUT_METERS) * 100);
			return dble;
		}

		public static double RetourneCmEnPieds(double Longueur)
		{
			double Lgr = Convert.ToDouble(UnitUtils.ConvertToInternalUnits(Longueur / 100, DisplayUnitType.DUT_METERS));
			return Lgr;
		}
	}
}
