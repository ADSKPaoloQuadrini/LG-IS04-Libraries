#region Autodesk PQ - IS04
using Autodesk.Revit.DB;
using System;
using System.Windows;
using System.Windows.Forms;
using static PTLGClassLibrary.GeneralClass;

namespace PTLGClassLibrary.ConfMan
{
	class M_DataFromGUI
	{
		public static void DataInitialisation(Element e, ConfigurateurManForm f)
		{
			//Initialisation du numéro de passerelle.
			f.numeroPasserelleString = e.GetParameters("LG_MET_PTLG_Designation")[0].AsString();

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
			if (e.LookupParameter("Extension arriere").AsInteger() == vrai)
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
						(RetournePiedEnCM(
							e.LookupParameter("Longueur extension gauche")
							.AsDouble()).ToString());
			}
			catch { }

			try
			{
				f.extensionGaucheIntComboBox.SelectedIndex = f.extensionGaucheIntComboBox.FindStringExact
						(RetournePiedEnCM(e.LookupParameter("Longueur extension gauche interieure").AsDouble()).ToString());
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
						(RetournePiedEnCM(e.LookupParameter("Longueur extension droite").AsDouble()).ToString());
			}
			catch { }
			try
			{
				f.extensionDroiteIntComboBox.SelectedIndex = f.extensionGaucheIntComboBox.FindStringExact
					(RetournePiedEnCM(e.LookupParameter("Longueur extension droite interieure").AsDouble()).ToString());
			}
			catch { }
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

			//initialisation des paf
			f.pafGaucheLabel.Text = pafGauche.ToString();
			f.pafDroitLabel.Text = pafDroit.ToString();

			//d
			f.dLabel.Text = d.ToString();

			#region sabots d'appuis
			//initialisation des sabots d'appuis
			SabotMaxi(f);

			if (e.LookupParameter("Sabot droit").AsInteger() == vrai)
			{
				f.angleDroitInterieurCheckBox.Checked = true;
				f.sabotDroitPictureBox.Visible = true;
				f.sabotDroitTextBox1.Visible = true;
				f.sabotDroitTrackBar.Visible = true;
				int distanceSabotDroit = Convert.ToInt32(e.LookupParameter("Distance mur-sabot droit").AsValueString());
				f.sabotDroitTrackBar.Value = (distanceSabotDroit);
			}
			else
			{
				f.angleDroitInterieurCheckBox.Checked = false;
				f.sabotDroitPictureBox.Visible = false;
				f.sabotDroitTextBox1.Visible = false;
				f.sabotDroitTrackBar.Visible = false;
			}

			if (e.LookupParameter("LG_MET_PTLG_Sabot droit 2").AsInteger() == vrai)
			{
				f.angleDroitInterieurCheckBox2.Checked = true;
				f.sabotDroitPictureBox2.Visible = true;
				f.sabotDroitTextBox2.Visible = true;
				f.sabotDroitTrackBar2.Visible = true;
				int distanceSabotDroit2 = Convert.ToInt32(e.LookupParameter("Distance mur-sabot droit 2").AsValueString());
				f.sabotDroitTrackBar2.Value = (distanceSabotDroit2);
			}
			else
			{
				f.angleDroitInterieurCheckBox2.Checked = false;
				f.sabotDroitPictureBox2.Visible = false;
				f.sabotDroitTextBox2.Visible = false;
				f.sabotDroitTrackBar2.Visible = false;
			}

			if (e.LookupParameter("Sabot gauche").AsInteger() == vrai)
			{
				f.angleGaucheInterieurCheckBox.Checked = true;
				f.sabotGauchePictureBox.Visible = true;
				f.sabotGaucheTextBox1.Visible = true;
				f.sabotGaucheTrackBar.Visible = true;
				int distanceSabotGauche = Convert.ToInt32(e.LookupParameter("Distance mur-sabot gauche").AsValueString());
				f.sabotGaucheTrackBar.Value = (distanceSabotGauche);
			}
			else
			{
				f.angleGaucheInterieurCheckBox.Checked = false;
				f.sabotGauchePictureBox.Visible = false;
				f.sabotGaucheTextBox1.Visible = false;
				f.sabotGaucheTrackBar.Visible = false;
			}

			if (e.LookupParameter("LG_MET_PTLG_Sabot gauche 2").AsInteger() == vrai)
			{
				f.angleGaucheInterieurCheckBox2.Checked = true;
				f.sabotGauchePictureBox2.Visible = true;
				f.sabotGaucheTextBox2.Visible = true;
				f.sabotGaucheTrackBar2.Visible = true;
				int distanceSabotGauche2 = Convert.ToInt32(e.LookupParameter("Distance mur-sabot gauche 2").AsValueString());
				f.sabotGaucheTrackBar2.Value = (distanceSabotGauche2);
			}
			else
			{
				f.angleGaucheInterieurCheckBox2.Checked = false;
				f.sabotGauchePictureBox2.Visible = false;
				f.sabotGaucheTextBox2.Visible = false;
				f.sabotGaucheTrackBar2.Visible = false;
			}
			//initialisation des distances des sabots
			f.sabotDroitTextBox1.Text = (f.sabotDroitTrackBar.Value).ToString();
			f.sabotDroitTextBox2.Text = (f.sabotDroitTrackBar2.Value).ToString();
			f.sabotGaucheTextBox1.Text = (f.sabotGaucheTrackBar.Value).ToString();
			f.sabotGaucheTextBox2.Text = (f.sabotGaucheTrackBar2.Value).ToString();
			//f.sabotDroitTextBox2.Text = GeneralClass.RetournePiedEnCM( passerelleSelectionneeElement.GetParameters("Distance mur-sabot gauche")[0].AsDouble()).ToString();
			//f.sabotGaucheTextBox2.Text = GeneralClass.RetournePiedEnCM(passerelleSelectionneeElement.GetParameters("Distance mur-sabot gauche")[0].AsDouble()).ToString();
			//f.sabotGaucheTextBoxFantome2.Text = (GeneralClass.RetournePiedEnCM(passerelleSelectionneeElement.LookupParameter("Distance mur-sabot gauche").AsDouble()) - 105).ToString();
			//f.sabotDroitTextBoxFantome2.Text = (GeneralClass.RetournePiedEnCM(passerelleSelectionneeElement.LookupParameter("Distance mur-sabot droit").AsDouble()) - 105).ToString();
			#endregion sabbot d'appuis

			#endregion initialisation extensions/ sabots d'appuis

			#region initialisation des attaches (3ème et 4ème)
			f.troisiemeAttacheCheckBox.Visible = true;
			f.quatriemeAttacheCheckBox.Visible = true;
			if (e.GetParameters("3ème attache")[0].AsInteger() == vrai &&
				e.GetParameters("4ème attache")[0].AsInteger() == faux)
			{
				f.troisiemeAttacheCheckBox.Checked = true;
				f.attacheCentreComboBox.Visible = true;
				f.attacheCentreTrackbar.Visible = true;
				string str = e.GetParameters("Type d'attache 3")[0].AsString();
				f.attacheCentreComboBox.SelectedIndex = f.attacheCentreComboBox.FindStringExact(str);

				int TrackBarMax = (int)Math.Round((double)lgFixe / 2, MidpointRounding.AwayFromZero);
				f.attacheCentreTrackbar.Maximum = TrackBarMax;
				f.attacheCentreTrackbar.Minimum = -TrackBarMax;

				f.quatriemeAttacheCheckBox.Checked = false;

				f.dLabel.Visible = false;
				f.dMaxiLabel.Visible = false;
				f.d1Label.Visible = true;
				f.d2Label.Visible = true;
				f.d1MaxiLabel.Visible = true;
				f.d2MaxiLabel.Visible = true;
				f.lblDi.Visible = true;
				f.lblD1i.Visible = false;
				f.lblD2i.Visible = false;

				f.d1Label.Text = d1.ToString();
				f.d2Label.Text = d2.ToString();

				f.attacheCentreTrackbar.Value = distanceAttacheCentre - lgTotale / 2;
				f.attacheCentreGTrackbarTextBox.Text = (lgFixe / 2 + f.attacheCentreTrackbar.Value).ToString();
				f.attacheCentreDTrackbarTextBox.Text = (lgFixe / 2 - f.attacheCentreTrackbar.Value).ToString();
			}
			else if (e.GetParameters("4ème attache")[0].AsInteger() == vrai)
			{
				f.troisiemeAttacheCheckBox.Checked = true;
				f.attacheCentreComboBox.Visible = true;
				f.attacheCentreTrackbar.Visible = true;
				string strT = e.GetParameters("Type d'attache 3")[0].AsString();
				f.attacheCentreComboBox.SelectedIndex = f.attacheCentreComboBox.FindStringExact(strT);

				f.quatriemeAttacheCheckBox.Checked = true;
				f.attacheCentreDroitComboBox.Visible = true;
				f.attacheCentreDroitTrackbar.Visible = true;
				string strQ = e.GetParameters("Type d'attache 4")[0].AsString();
				f.attacheCentreDroitComboBox.SelectedIndex = f.attacheCentreDroitComboBox.FindStringExact(strQ);

				int TrackBarMax = (int)Math.Round((double)lgFixe / 4, MidpointRounding.AwayFromZero);
				f.attacheCentreTrackbar.Maximum = TrackBarMax;
				f.attacheCentreTrackbar.Minimum = -TrackBarMax;
				f.attacheCentreDroitTrackbar.Maximum = TrackBarMax;
				f.attacheCentreDroitTrackbar.Minimum = -TrackBarMax;

				f.dLabel.Visible = true;
				f.dMaxiLabel.Visible = true;
				f.d1Label.Visible = true;
				f.d2Label.Visible = true;
				f.d1MaxiLabel.Visible = true;
				f.d2MaxiLabel.Visible = true;
				f.lblDi.Visible = false;
				f.lblD1i.Visible = true;
				f.lblD2i.Visible = true;

				f.d1Label.Text = d1.ToString();
				f.d2Label.Text = d2.ToString();
				f.dLabel.Text = d.ToString();

				f.attacheCentreTrackbar.Value = d1 - cGauche - lgFixe / 4;
				f.attacheCentreDroitTrackbar.Value = -(d2 - cDroite - lgFixe / 4);
				f.attacheCentreGTrackbarTextBox.Text = (lgFixe / 4 + f.attacheCentreTrackbar.Value).ToString();
				f.attacheCentreDTrackbarTextBox.Text = (lgFixe / 4 - f.attacheCentreDroitTrackbar.Value).ToString();
			}
			else
			{
				f.troisiemeAttacheCheckBox.Checked = false;
				f.quatriemeAttacheCheckBox.Checked = false;

				f.dLabel.Visible = true;
				f.dMaxiLabel.Visible = true;
				f.d1Label.Visible = false;
				f.d2Label.Visible = false;
				f.d1MaxiLabel.Visible = false;
				f.d2MaxiLabel.Visible = false;
				f.lblDi.Visible = false;
				f.lblD1i.Visible = false;
				f.lblD2i.Visible = false;
			}
			#endregion initialisation des attaches (3ème et 4ème)

			#region banche et stabilisation
			//initialisation stabilisation
			foreach (string str in f.stabilisationComboBox.Items)
			{
				if (e.GetParameters(str)[0].AsInteger() == vrai)
				{
					f.stabilisationComboBox.SelectedIndex = f.stabilisationComboBox.FindStringExact(str);
				}
			}

			// Initialisation de la banche
			foreach (string str in f.bancheComboBox.Items)
			{
				if (e.GetParameters(str)[0].AsInteger() == vrai)
				{
					f.bancheComboBox.SelectedIndex = f.bancheComboBox.FindStringExact(str);
				}
			}
			#endregion

			#region attache gauche et droite MAX MIN
			if (type == "M3")
			{
				f.attacheGaucheTrackbar.Maximum = 160;
				f.attacheGaucheTrackbar.Minimum = -160;
				f.attacheDroiteTrackbar.Maximum = 160;
				f.attacheDroiteTrackbar.Minimum = -160;
			}
			if (type == "M2")
			{
				f.attacheGaucheTrackbar.Maximum = 95;
				f.attacheGaucheTrackbar.Minimum = -95;
				f.attacheDroiteTrackbar.Maximum = 95;
				f.attacheDroiteTrackbar.Minimum = -95;
			}
			if (type == "M1")
			{
				f.attacheGaucheTrackbar.Maximum = 75;
				f.attacheGaucheTrackbar.Minimum = -75;
				f.attacheDroiteTrackbar.Maximum = 75;
				f.attacheDroiteTrackbar.Minimum = -75;
			}
			#endregion attache gauche et droite MAX MIN

			#region initialisation de l'angle droit et GC
			if (type == "M2" || type == "M3")
			{
				//gauche
				if (e.GetParameters("Gauche - Angle droit")[0].AsInteger() == vrai)
					f.angleGaucheCheckBox.Checked = true;
				else f.angleGaucheCheckBox.Checked = false;
				//droit
				if (e.GetParameters("Droite - Angle droit")[0].AsInteger() == vrai)
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
			if (e.GetParameters("GC about droit")[0].AsInteger() == vrai)
			{
				f.gcDroiteCheckBox.Checked = true;
			}
			else f.gcDroiteCheckBox.Checked = false;

			//initialisation GC droit
			if (e.GetParameters("GC about gauche")[0].AsInteger() == vrai)
			{
				f.gcGaucheCheckBox.Checked = true;
			}
			else f.gcGaucheCheckBox.Checked = false;
			#endregion

			#region initialisation lgTot,attaches,c,paf,d
			f.lgTotaleLabel.Text = lgTotale.ToString();

			//initialisation du type d'attache droite
			foreach (string str in f.attacheDroiteComboBox.Items)
			{
				if (e.LookupParameter(str).AsInteger() == vrai)
					f.attacheDroiteComboBox.SelectedIndex = f.attacheDroiteComboBox.FindStringExact(str);
			}
			//initialisation du type d'attache gauche
			foreach (string str in f.attacheGaucheComboBox.Items)
			{
				if (e.LookupParameter("Gauche - " + str).AsInteger() == vrai)
					f.attacheGaucheComboBox.SelectedIndex = f.attacheGaucheComboBox.FindStringExact(str);
			}
			//Initialisation GeneralClass.cGauche
			f.attacheGaucheTrackbarTextBox.Text = (cGauche).ToString();

			//initialisation GeneralClass.cDroite
			f.attacheDroiteTrackbarTextBox.Text = (cDroite).ToString();

			#endregion initialisation lgTot,attaches,c,paf,d
		}

		public static void MaxiValues(ConfigurateurManForm f)
        {
			#region lecture et stockage des valeurs maxi
			int bancheSelInd = f.bancheComboBox.SelectedIndex;

			if (type == "M1")
			{
				pafMax = lgTotale / 4;
				if (f.extArriereCheckBox.Checked) //2.5m
				{
					lgMax = LectureCSVint("DonneesBancheM1_extarr", bancheSelInd + 1, 3);
					leMax = LectureCSVint("DonneesBancheM1_extarr", bancheSelInd + 1, 4);
					cMax = LectureCSVint("DonneesBancheM1_extarr", bancheSelInd + 1, 1);
					dMax = LectureCSVint("DonneesBancheM1_extarr", bancheSelInd + 1, 5);

				}
				else //2m
				{
					lgMax = LectureCSVint("DonneesBancheM1", bancheSelInd + 1, 3);
					leMax = LectureCSVint("DonneesBancheM1", bancheSelInd + 1, 4);
					cMax = LectureCSVint("DonneesBancheM1", bancheSelInd + 1, 1);
					dMax = LectureCSVint("DonneesBancheM1", bancheSelInd + 1, 5);
				}
				leMaxAngle = leMax;
			}
			if (type == "M2")
			{
				leMaxAngle = 55;
				pafMax = lgTotale / 3;
				if (f.extArriereCheckBox.Checked)
				{
					lgMax = LectureCSVint("DonneesBancheM2_extarr", bancheSelInd + 1, 3);
					leMax = LectureCSVint("DonneesBancheM2_extarr", bancheSelInd + 1, 4);
					cMax = LectureCSVint("DonneesBancheM2_extarr", bancheSelInd + 1, 1);
					dMax = LectureCSVint("DonneesBancheM2_extarr", bancheSelInd + 1, 5);

				}
				else //2m
				{
					lgMax = LectureCSVint("DonneesBancheM2", bancheSelInd + 1, 3);
					leMax = LectureCSVint("DonneesBancheM2", bancheSelInd + 1, 4);
					cMax = LectureCSVint("DonneesBancheM2", bancheSelInd + 1, 1);
					dMax = LectureCSVint("DonneesBancheM2", bancheSelInd + 1, 5);
				}
			}
			if (type == "M3")
			{
				leMaxAngle = 120;
				pafMax = lgTotale / 3;
				if (f.extArriereCheckBox.Checked) //2.5m
				{
					if (f.troisiemeAttacheCheckBox.Checked) // 3e attache cochée
					{
						lgMax = LectureCSVint("DonneesBancheM3bis_extarr", bancheSelInd + 1, 3);
						leMax = LectureCSVint("DonneesBancheM3bis_extarr", bancheSelInd + 1, 4);
						cMax = LectureCSVint("DonneesBancheM3bis_extarr", bancheSelInd + 1, 1);
						dMax = LectureCSVint("DonneesBancheM3bis_extarr", bancheSelInd + 1, 5);

					}
					else //2 attaches
					{
						lgMax = LectureCSVint("DonneesBancheM3_extarr", bancheSelInd + 1, 3);
						leMax = LectureCSVint("DonneesBancheM3_extarr", bancheSelInd + 1, 4);
						cMax = LectureCSVint("DonneesBancheM3_extarr", bancheSelInd + 1, 1);
						dMax = LectureCSVint("DonneesBancheM3_extarr", bancheSelInd + 1, 5);
					}
				}
				else //2m
				{
					if (f.troisiemeAttacheCheckBox.Checked) // 3e attache cochée
					{
						lgMax = LectureCSVint("DonneesBancheM3bis", bancheSelInd + 1, 3);
						leMax = LectureCSVint("DonneesBancheM3bis", bancheSelInd + 1, 4);
						cMax = LectureCSVint("DonneesBancheM3bis", bancheSelInd + 1, 1);
						dMax = LectureCSVint("DonneesBancheM3bis", bancheSelInd + 1, 5);
					}
					else //2attaches
					{
						lgMax = LectureCSVint("DonneesBancheM3", bancheSelInd + 1, 3);
						leMax = LectureCSVint("DonneesBancheM3", bancheSelInd + 1, 4);
						cMax = LectureCSVint("DonneesBancheM3", bancheSelInd + 1, 1);
						dMax = LectureCSVint("DonneesBancheM3", bancheSelInd + 1, 5);
					}
				}
			}

			/// Sabot
			SabotMaxi(f);
			if (Convert.ToInt32(f.sabotGaucheTextBox1.Text) > f.sabotGaucheTrackBar.Value)
				f.sabotGaucheTextBox1.Text = f.sabotGaucheTrackBar.Value.ToString();
			if (Convert.ToInt32(f.sabotGaucheTextBox2.Text) > f.sabotGaucheTrackBar2.Value)
				f.sabotGaucheTextBox2.Text = f.sabotGaucheTrackBar2.Value.ToString();
			if (Convert.ToInt32(f.sabotDroitTextBox1.Text) > f.sabotDroitTrackBar.Value)
				f.sabotDroitTextBox1.Text = f.sabotDroitTrackBar.Value.ToString();
			if (Convert.ToInt32(f.sabotDroitTextBox2.Text) > f.sabotDroitTrackBar2.Value)
				f.sabotDroitTextBox2.Text = f.sabotDroitTrackBar2.Value.ToString();

			/// lgTotale
			if (lgTotale < 300)
			{
				pafMaxAngle = 29;
			}
			else if (lgTotale <= 610)
			{
				pafMaxAngle = LectureCSVint("PAFmaxi", Convert.ToInt32(lgTotale / 10) - 28 - 1, 1);//-1 à voir
			}
			#endregion

			#region affichage des valeurs maxi
			if (f.angleGaucheCheckBox.Checked == false)
			{
				f.leGaucheMaxiLabel.Text = leMax.ToString();
				f.pafGaucheMaxiLabel.Text = pafMax.ToString();
			}
			else
			{
				f.leGaucheMaxiLabel.Text = Math.Min(leMaxAngle, leMax).ToString();
				f.pafGaucheMaxiLabel.Text = pafMaxAngle.ToString();
			}
			if (f.angleDroitCheckBox.Checked == false)
			{
				f.leDroitMaxiLabel.Text = leMax.ToString();
				f.pafDroitMaxiLabel.Text = pafMax.ToString();
			}
			else
			{
				f.leDroitMaxiLabel.Text = Math.Min(leMaxAngle, leMax).ToString();
				f.pafDroitMaxiLabel.Text = pafMaxAngle.ToString();
			}
			//GeneralClass.lgFixe
			f.lgFixeLabel.Text = lgFixe.ToString();
			//GeneralClass.lgMax
			f.lgTotaleMaxiLabel.Text = lgMax.ToString();
			//cMax
			f.cGaucheMaxiLabel.Text = cMax.ToString();
			f.cDroiteMaxiLabel.Text = cMax.ToString();
			f.cCentreMaxiLabel.Text = cMax.ToString();
			f.cCentreDroiteMaxiLabel.Text = cMax.ToString();
			//dmax
			f.dMaxiLabel.Text = dMax.ToString();
			f.d1MaxiLabel.Text = dMax.ToString();
			f.d2MaxiLabel.Text = dMax.ToString();
			#endregion affichage des valeurs maxi
		}

		private static void SabotMaxi(ConfigurateurManForm f)
        {
			int l1 = 175;
			int l2 = 233;
			if (f.extArriereCheckBox.Checked)
			{
				f.sabotGaucheTrackBar.Maximum = l2;
				f.sabotGaucheTrackBar2.Maximum = l2;
				f.sabotDroitTrackBar.Maximum = l2;
				f.sabotDroitTrackBar2.Maximum = l2;
			}
			else
			{
				f.sabotGaucheTrackBar.Maximum = l1;
				f.sabotGaucheTrackBar2.Maximum = l1;
				f.sabotDroitTrackBar.Maximum = l1;
				f.sabotDroitTrackBar2.Maximum = l1;
			}
		}

		public static void ThirdFourthSupports(ConfigurateurManForm f)
        {
			if (f.troisiemeAttacheCheckBox.Checked)
			{
				f.attacheCentreComboBox.Visible = true;
				f.attacheCentreTrackbar.Visible = true;
				f.attacheCentreGTrackbarTextBox.Visible = true;
				f.attacheCentreDTrackbarTextBox.Visible = true;
				f.d1Label.Visible = true;
				f.d2Label.Visible = true;
				f.d1MaxiLabel.Visible = true;
				f.d2MaxiLabel.Visible = true;
				f.cCentreMaxiLabel.Visible = true;
				f.cCentreDroiteMaxiLabel.Visible = true;

				if (f.quatriemeAttacheCheckBox.Checked)
				{
					// visibility
					f.attacheCentreDroitComboBox.Visible = true;
					f.attacheCentreDroitTrackbar.Visible = true;
					f.attacheCentreTrackbar.Width = 90 * Helpers.Revit.GetWindowsScaling();
					f.dLabel.Visible = true;
					f.dMaxiLabel.Visible = true;
					f.lblDi.Visible = false;
					f.lblD1i.Visible = true;
					f.lblD2i.Visible = true;

					// centreTrackbar, centreDroitTrackbar, d, d1, d2
					if (f.attacheCentreTrackbar.Value > QuarterLgFixe) f.attacheCentreTrackbar.Value = QuarterLgFixe;
					if (f.attacheCentreTrackbar.Value < -QuarterLgFixe) f.attacheCentreTrackbar.Value = -QuarterLgFixe;
					f.attacheCentreTrackbar.Maximum = QuarterLgFixe;
					f.attacheCentreTrackbar.Minimum = -QuarterLgFixe;
					f.attacheCentreDroitTrackbar.Maximum = QuarterLgFixe;
					f.attacheCentreDroitTrackbar.Minimum = -QuarterLgFixe;
					d1 = lgFixe / 4 + cGauche - (-f.attacheCentreTrackbar.Value);
					d2 = lgFixe / 4 + cDroite - (+f.attacheCentreDroitTrackbar.Value);
					d = lgFixe + cGauche + cDroite - d1 - d2;
					f.d1Label.Text = d1.ToString();
					f.d2Label.Text = d2.ToString();
					f.dLabel.Text = d.ToString();

                    // centreTrackbarGTextBox
                    if (string.IsNullOrEmpty(f.attacheCentreGTrackbarTextBox.Text) || f.attacheCentreGTrackbarTextBox.Text == "-")
                    {
                        f.attacheCentreTrackbar.Value = 0;
                        f.attacheCentreGTrackbarTextBox.Text = QuarterLgFixe.ToString();
                    }
                    else if (Convert.ToInt32(f.attacheCentreGTrackbarTextBox.Text) > HalfLgFixe)
                    {
                        f.attacheCentreGTrackbarTextBox.Text = HalfLgFixe.ToString();
                    }
                    else if (Convert.ToInt32(f.attacheCentreGTrackbarTextBox.Text) < 0)
                    {
                        f.attacheCentreGTrackbarTextBox.Text = "0";
                    }
                    else
                    {
                        f.attacheCentreTrackbar.Value = Convert.ToInt32(f.attacheCentreGTrackbarTextBox.Text) - QuarterLgFixe;
                    }

                    // centreTrackbarDTextBox
                    if (string.IsNullOrEmpty(f.attacheCentreDTrackbarTextBox.Text) || f.attacheCentreDTrackbarTextBox.Text == "-")
                    {
                        f.attacheCentreDroitTrackbar.Value = 0;
                        f.attacheCentreDTrackbarTextBox.Text = QuarterLgFixe.ToString();
                    }
                    else if (Convert.ToInt32(f.attacheCentreDTrackbarTextBox.Text) > HalfLgFixe)
                    {
                        f.attacheCentreDTrackbarTextBox.Text = HalfLgFixe.ToString();
                    }
                    else if (Convert.ToInt32(f.attacheCentreDTrackbarTextBox.Text) < 0)
                    {
                        f.attacheCentreDTrackbarTextBox.Text = "0";
                    }
                    else
                    {
                        f.attacheCentreDroitTrackbar.Value = QuarterLgFixe - Convert.ToInt32(f.attacheCentreDTrackbarTextBox.Text);
                    }
                }
				else
				{
					// visibility
					f.attacheCentreDroitComboBox.Visible = false;
					f.attacheCentreDroitTrackbar.Visible = false;
					f.attacheCentreTrackbar.Width = 185 * Helpers.Revit.GetWindowsScaling();
					f.dLabel.Visible = false;
					f.dMaxiLabel.Visible = false;
					f.lblDi.Visible = true;
					f.lblD1i.Visible = false;
					f.lblD2i.Visible = false;

					// centreTrackbar, d, d1, d2
					f.attacheCentreTrackbar.Maximum = HalfLgFixe;// 165;
					f.attacheCentreTrackbar.Minimum = -HalfLgFixe; //-165;
					d1 = lgFixe / 2 + cGauche - (-f.attacheCentreTrackbar.Value);
					d2 = lgFixe / 2 + cDroite - (+f.attacheCentreTrackbar.Value);
					d = lgFixe / 2 - d1 - d2;
					f.d1Label.Text = d1.ToString();
					f.d2Label.Text = d2.ToString();
					f.dLabel.Text = d.ToString();

                    // centreTrackbarGTextBox
                    if (string.IsNullOrEmpty(f.attacheCentreGTrackbarTextBox.Text) || f.attacheCentreGTrackbarTextBox.Text == "-")
                    {
                        f.attacheCentreTrackbar.Value = 0;
                        f.attacheCentreGTrackbarTextBox.Text = HalfLgFixe.ToString();
                    }
                    else if (Convert.ToInt32(f.attacheCentreGTrackbarTextBox.Text) > lgFixe)
                    {
                        f.attacheCentreGTrackbarTextBox.Text = lgFixe.ToString();
                    }
                    else if (Convert.ToInt32(f.attacheCentreGTrackbarTextBox.Text) < 0)
                    {
                        f.attacheCentreGTrackbarTextBox.Text = "0";
                    }
                    else
                    {
                        f.attacheCentreTrackbar.Value = Convert.ToInt32(f.attacheCentreGTrackbarTextBox.Text) - HalfLgFixe;
                    }

                    // centreTrackbarDTextBox
                    if (string.IsNullOrEmpty(f.attacheCentreDTrackbarTextBox.Text) || f.attacheCentreDTrackbarTextBox.Text == "-")
                    {
                        f.attacheCentreTrackbar.Value = 0;
                        f.attacheCentreDTrackbarTextBox.Text = HalfLgFixe.ToString();
                    }
                    else if (Convert.ToInt32(f.attacheCentreDTrackbarTextBox.Text) > lgFixe)
                    {
                        f.attacheCentreDTrackbarTextBox.Text = lgFixe.ToString();
                    }
                    else if (Convert.ToInt32(f.attacheCentreDTrackbarTextBox.Text) < 0)
                    {
                        f.attacheCentreDTrackbarTextBox.Text = "0";
                    }
                    else
                    {
                        f.attacheCentreTrackbar.Value = HalfLgFixe - Convert.ToInt32(f.attacheCentreDTrackbarTextBox.Text);
                    }
                }
			}
			else
			{
				f.attacheCentreComboBox.Visible = false;
				f.attacheCentreTrackbar.Visible = false;
				f.attacheCentreGTrackbarTextBox.Visible = false;
				f.attacheCentreDTrackbarTextBox.Visible = false;
				f.lblDi.Visible = false;
				f.dLabel.Visible = true;
				f.dMaxiLabel.Visible = true;
				f.d1Label.Visible = false;
				f.d2Label.Visible = false;
				f.d1MaxiLabel.Visible = false;
				f.d2MaxiLabel.Visible = false;
				f.cCentreMaxiLabel.Visible = false;
				f.cCentreDroiteMaxiLabel.Visible = false;
				f.quatriemeAttacheCheckBox.Checked = false;
				f.attacheCentreDroitComboBox.Visible = false;
				f.attacheCentreDroitTrackbar.Visible = false;
				//if (f.quatriemeAttacheCheckBox.Checked) f.troisiemeAttacheCheckBox.Checked = true;
				f.lblDi.Visible = false;
				f.lblD1i.Visible = false;
				f.lblD2i.Visible = false;

				d = lgFixe + cGauche + cDroite;
				f.dLabel.Text = d.ToString();
			}
		}

		public static void DataUpdate(ConfigurateurManForm f)
		{
			#region affichage des valeurs courantes
			//on regarde si on est en angle biais et on prend le cas le plus défavorable
			if (f.angleBiaisGaucheCheckBox.Checked == true)
			{
				lecturegauche = Math.Max(Convert.ToInt32(f.extensionGaucheComboBox.SelectedItem), Convert.ToInt32(f.extensionGaucheIntComboBox.SelectedItem));
			}
			else lecturegauche = Convert.ToInt32(f.extensionGaucheComboBox.SelectedItem);

			if (f.angleBiaisDroitCheckBox.Checked == true)
			{
				lecturedroite = Math.Max(Convert.ToInt32(f.extensionDroiteComboBox.SelectedItem), Convert.ToInt32(f.extensionDroiteIntComboBox.SelectedItem));
			}
			else lecturedroite = Convert.ToInt32(f.extensionDroiteComboBox.SelectedItem);
			//lg totale
			lgTotale = lecturegauche + lecturedroite + lgFixe;
			f.lgTotaleLabel.Text = lgTotale.ToString();
			//GeneralClass.cGauche   
			cGauche = -f.attacheGaucheTrackbar.Value;
			distanceAttacheGauche = (cGauche + lgFixe / 2);
			//GeneralClass.cDroite
			cDroite = f.attacheDroiteTrackbar.Value;
			distanceAttacheDroite = (cDroite + lgFixe / 2);
			//GeneralClass.cCentre & cCentreDroite
			if (f.troisiemeAttacheCheckBox.Checked && !f.quatriemeAttacheCheckBox.Checked)
			{
				cCentre = HalfLgFixe + f.attacheCentreTrackbar.Value;
				cCentreDroite = HalfLgFixe - f.attacheCentreTrackbar.Value;
			}
			else if (f.quatriemeAttacheCheckBox.Checked)
            {
				cCentre = QuarterLgFixe + f.attacheCentreTrackbar.Value;
				cCentreDroite = QuarterLgFixe - f.attacheCentreDroitTrackbar.Value;
			}
			distanceAttacheCentre = pafGauche + cGauche + cCentre;
			distanceAttacheCentreDroit = pafDroit + cDroite + cCentreDroite;
			//paf
			pafDroit = lecturedroite - cDroite;
			pafGauche = lecturegauche - cGauche;
			f.pafGaucheLabel.Text = pafGauche.ToString();
			f.pafDroitLabel.Text = pafDroit.ToString();

			#endregion affichage des valeurs courantes
		}

		public static void TestIndexComboBox(ConfigurateurManForm f)
		{
			int IndexTest = (f.attacheDroiteComboBox.SelectedIndex + 1) * (f.attacheGaucheComboBox.SelectedIndex + 1) * (f.bancheComboBox.SelectedIndex + 1) * (f.extensionDroiteComboBox.SelectedIndex + 1) * (f.extensionGaucheComboBox.SelectedIndex + 1);
			if (f.troisiemeAttacheCheckBox.Checked == true)
				IndexTest *= (f.attacheCentreComboBox.SelectedIndex + 1);
			if (f.quatriemeAttacheCheckBox.Checked == true)
				IndexTest *= (f.attacheCentreDroitComboBox.SelectedIndex + 1);

			if (IndexTest == 0)
				f.applyButton.Enabled = false;
			else
				f.applyButton.Enabled = true;
		}
	}
}
#endregion