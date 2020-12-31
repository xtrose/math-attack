using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
//Zum ändern der Farben
using System.Windows.Media;
//Für Timer
using System.Windows.Threading;
//Zum auslesen und löschen der Daten
using System.IO;
using System.IO.IsolatedStorage;
//Zum erweiterten schneiden von strings
using System.Text.RegularExpressions;
//Für Benachrichtigung vor beenden
using System.ComponentModel;





namespace Magic_Square.Pages
{
    public partial class TrainingSettings : PhoneApplicationPage
    {





        //Timer erstellen
        //---------------------------------------------------------------------------------------------------------------------------------
        DispatcherTimer dt = new DispatcherTimer();
        //---------------------------------------------------------------------------------------------------------------------------------





        //Allgemeine Variabeln
        //---------------------------------------------------------------------------------------------------------------------------------
        string settings;
        int cmax = 50;
        int cmin = 200;
        string trainingsettings = "eA1";
        int level = 1;
        int difficulty = 1;
        string trd = "e";
        string sliderselected = "no";
        //---------------------------------------------------------------------------------------------------------------------------------
        




        //Wird am Anfang vom Spiel geladen
        //---------------------------------------------------------------------------------------------------------------------------------
        public TrainingSettings()
        {
            //Komponenten laden
            InitializeComponent();

            //Farben erstellen, Tastenfeld, Punkte und Zeit
            int min = 200;
            int max = 210;
            Random random = new Random();
            SolidColorBrush mySolidColorBrushN = new SolidColorBrush();
            r1 = random.Next(min, max); g1 = random.Next(min, max); b1 = random.Next(min, max);
            mySolidColorBrushN.Color = Color.FromArgb((byte)(255), (byte)(r1), (byte)(g1), (byte)(b1));
            SMIN.Foreground = mySolidColorBrushN;
            SMAX.Foreground = mySolidColorBrushN;

            //Timer erstellen
            dt.Stop();
            dt.Interval = new TimeSpan(0, 0, 0, 0, 20);
            dt.Tick += new EventHandler(dt_Tick);
            dt.Start();
        }
        //---------------------------------------------------------------------------------------------------------------------------------





        //Wird bei jedem Start der Seite geladen
        //---------------------------------------------------------------------------------------------------------------------------------
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            //Wenn einstellungen vorhanden
            IsolatedStorageFile file2 = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFile file3 = IsolatedStorageFile.GetUserStoreForApplication();
            if (file2.DirectoryExists("Settings"))
            {
                //Trainings Einstellungen laden
                IsolatedStorageFileStream filestream2 = file2.OpenFile("Settings/TrainingsSettings.txt", FileMode.Open);
                StreamReader sr2 = new StreamReader(filestream2);
                trainingsettings = sr2.ReadToEnd();
                filestream2.Close();
                //Einstellungen laden
                IsolatedStorageFileStream filestream3 = file3.OpenFile("Settings/Settings.txt", FileMode.Open);
                StreamReader sr3 = new StreamReader(filestream3);
                settings = sr3.ReadToEnd();
                filestream3.Close();

            }
            //Einstellungen erstellen
            else
            {
                //Trainings Einstellungen erstellen
                file2.CreateDirectory("Settings");
                IsolatedStorageFileStream filestream2 = file2.CreateFile("Settings/TrainingsSettings.txt");
                StreamWriter sw2 = new StreamWriter(filestream2);
                sw2.WriteLine("eA1");
                sw2.Flush();
                filestream2.Close();
                trainingsettings = "eA1";
                //Einstellungen erstellen
                IsolatedStorageFileStream filestream3 = file3.CreateFile("Settings/Settings.txt");
                StreamWriter sw3 = new StreamWriter(filestream3);
                sw3.WriteLine("50A200");
                sw3.Flush();
                filestream3.Close();
                settings = "50A200";
            }
            //Einstellungen in Variabeln schreiben
            string[] split3 = Regex.Split(settings, "A");
            cmin = Convert.ToInt32(split3[0]);
            cmax = Convert.ToInt32(split3[1]);
            //Trainings Einstellungen in Variabeln schreiben
            string[] split2 = Regex.Split(trainingsettings, "A");
            trd = Convert.ToString(split2[0]);
            level = Convert.ToInt32(split2[1]);
            //Trainings Einstellungen in Square schreiben
            if (trd == "e")
            {
                difficulty = 1;
                TBMIN2.Text = "Easy";
            }
            else if (trd == "m")
            {
                difficulty = 2;
                TBMIN2.Text = "Medium";
            }
            else if (trd == "h")
            {
                difficulty = 3;
                TBMIN2.Text = "Hard";
            }
            TBMAX2.Text = "" + level;


            //Einstellungen auf Slider anwenden
            SMIN.Minimum = 1;
            SMIN.Maximum = 3;
            SMIN.Value = difficulty;
            SMAX.Minimum = 1;
            SMAX.Maximum = 500;
            SMAX.Value = level;

            //Quadrate neu erstellen
            CreateNewSquares(255, cmin, cmax);
        }
        //---------------------------------------------------------------------------------------------------------------------------------





        //Quadrate neu erstellen
        //---------------------------------------------------------------------------------------------------------------------------------
        //Variabaln der Farben erstellen
        int r1 = 0; int g1 = 0; int b1 = 0;
        int r2 = 0; int g2 = 0; int b2 = 0;
        int r3 = 0; int g3 = 0; int b3 = 0;
        int r4 = 0; int g4 = 0; int b4 = 0;
        int r5 = 0; int g5 = 0; int b5 = 0;
        int r6 = 0; int g6 = 0; int b6 = 0;
        int r7 = 0; int g7 = 0; int b7 = 0;
        int r8 = 0; int g8 = 0; int b8 = 0;
        int r9 = 0; int g9 = 0; int b9 = 0;
        int r10 = 0; int g10 = 0; int b10 = 0;
        //Variabeln der Größen erstellen
        int s1 = 0; int s2 = 0; int s3 = 0; int s4 = 0; int s5 = 0; int s6 = 0; int s7 = 0; int s8 = 0; int s9 = 0; int s10 = 0;
        //Farbe erstellen
        SolidColorBrush mySolidColorBrush1 = new SolidColorBrush();
        SolidColorBrush mySolidColorBrush2 = new SolidColorBrush();
        SolidColorBrush mySolidColorBrush3 = new SolidColorBrush();
        SolidColorBrush mySolidColorBrush4 = new SolidColorBrush();
        SolidColorBrush mySolidColorBrush5 = new SolidColorBrush();
        SolidColorBrush mySolidColorBrush6 = new SolidColorBrush();
        SolidColorBrush mySolidColorBrush7 = new SolidColorBrush();
        SolidColorBrush mySolidColorBrush8 = new SolidColorBrush();
        SolidColorBrush mySolidColorBrush9 = new SolidColorBrush();

        //Quadrate neu errechnen
        void CreateNewSquares(int a, int min, int max)
        {
            //Zufallsgenerator erstellen
            Random random = new Random();
            //Quadrat Füllen
            r1 = random.Next(min, max); g1 = random.Next(min, max); b1 = random.Next(min, max);
            mySolidColorBrush1.Color = Color.FromArgb((byte)(a), (byte)(r1), (byte)(g1), (byte)(b1));
            RTMIN.Fill = mySolidColorBrush1;
            TBMIN1.Foreground = mySolidColorBrush1;
            TBMIN2.Foreground = mySolidColorBrush1;
            s1 = random.Next(175, 225);
            RTMIN.Width = s1;
            RTMIN.Height = s1;
            r2 = random.Next(min, max); g2 = random.Next(min, max); b2 = random.Next(min, max);
            mySolidColorBrush2.Color = Color.FromArgb((byte)(a), (byte)(r2), (byte)(g2), (byte)(b2));
            RTMAX.Fill = mySolidColorBrush2;
            TBMAX1.Foreground = mySolidColorBrush2;
            TBMAX2.Foreground = mySolidColorBrush2;
            s2 = random.Next(175, 225);
            RTMAX.Width = s2;
            RTMAX.Height = s2;
            r3 = random.Next(min, max); g3 = random.Next(min, max); b3 = random.Next(min, max);
            mySolidColorBrush3.Color = Color.FromArgb((byte)(a), (byte)(r3), (byte)(g3), (byte)(b3));
            RTSMIN.Fill = mySolidColorBrush3;
            r4 = random.Next(min, max); g4 = random.Next(min, max); b4 = random.Next(min, max);
            mySolidColorBrush4.Color = Color.FromArgb((byte)(a), (byte)(r4), (byte)(g4), (byte)(b4));
            RTSMAX.Fill = mySolidColorBrush4;
            r6 = random.Next(min, max); g6 = random.Next(min, max); b6 = random.Next(min, max);
            mySolidColorBrush6.Color = Color.FromArgb((byte)(a), (byte)(r6), (byte)(g6), (byte)(b6));
            RTRES.Fill = mySolidColorBrush6;
            TBRES.Foreground = mySolidColorBrush6;
            s6 = random.Next(175, 225);
            RTRES.Width = s6;
            RTRES.Height = s6;
        }
        //---------------------------------------------------------------------------------------------------------------------------------





        //Slider Verschieben
        //---------------------------------------------------------------------------------------------------------------------------------
        //MinSlider
        private void SliderMin(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            //Wert in Variable schreiben
            difficulty = Convert.ToInt32(SMIN.Value);
            if (difficulty == 1)
            {
                trd = "e";
                TBMIN2.Text = "Easy";
                SMIN.Value = 1;
            }
            if (difficulty == 2)
            {
                trd = "m";
                TBMIN2.Text = "Medium";
                SMIN.Value = 2;
            }
            if (difficulty == 3)
            {
                trd = "h";
                TBMIN2.Text = "Hard";
                SMIN.Value = 3;
            }
            //Quadrate neu erstellen
            CreateNewSquares(255, cmin, cmax);
        }

        //MaxSlider
        private void SliderMax(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            //Wert in Variable schreiben
            level = Convert.ToInt32(SMAX.Value);
            TBMAX2.Text = "" + level;
            SMAX.Value = level;
            //Quadrate neu erstellen
            CreateNewSquares(255, cmin, cmax);
        }

        //Angeben das Slider Min Selectiert ist
        private void SMIN_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            sliderselected = "min";
        }

        //Angeben das Slider Max Selectiert ist
        private void SMAX_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            sliderselected = "max";
        }
        //---------------------------------------------------------------------------------------------------------------------------------





        //Button Reset
        //---------------------------------------------------------------------------------------------------------------------------------
        private void btnReset(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            trd = "e";
            TBMIN2.Text = "Easy";
            SMIN.Value = 1;
            level = 1;
            TBMAX2.Text = "" + level;
            SMAX.Value = level;
            //Quadrate neu erstellen
            CreateNewSquares(255, cmin, cmax);
        }
        //---------------------------------------------------------------------------------------------------------------------------------





        //Beim beenden der App speichern
        //---------------------------------------------------------------------------------------------------------------------------------
        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            //Vor dem Beenden speichern
            IsolatedStorageFile file5 = IsolatedStorageFile.GetUserStoreForApplication();
            //Trainings Einstellungen erstellen
            file5.CreateDirectory("Settings");
            IsolatedStorageFileStream filestream5 = file5.CreateFile("Settings/TrainingsSettings.txt");
            StreamWriter sw5 = new StreamWriter(filestream5);
            sw5.WriteLine(trd + "A" + level);
            sw5.Flush();
            filestream5.Close();
        }
        //---------------------------------------------------------------------------------------------------------------------------------





        //Timer Ablauf
        //---------------------------------------------------------------------------------------------------------------------------------
        void dt_Tick(object sender, EventArgs e)
        {
            //Wenn min Slider ausgewählt
            if (sliderselected == "min")
            {
                //Wert in Variable schreiben
                difficulty = Convert.ToInt32(SMIN.Value);
                if (difficulty == 1)
                {
                    trd = "e";
                    TBMIN2.Text = "Easy";
                }
                if (difficulty == 2)
                {
                    trd = "m";
                    TBMIN2.Text = "Medium";
                }
                if (difficulty == 3)
                {
                    trd = "h";
                    TBMIN2.Text = "Hard";
                }
            }
            //Wenn max Slider ausgewählt
            if (sliderselected == "max")
            {
                //Wert in Variable schreiben
                level = Convert.ToInt32(SMAX.Value);
                TBMAX2.Text = "" + level;
            }
        }
        //---------------------------------------------------------------------------------------------------------------------------------




    }
}