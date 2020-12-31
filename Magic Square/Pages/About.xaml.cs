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
//Für Marktplatz usw.
using Microsoft.Phone.Tasks;





namespace Magic_Square.Pages
{
    public partial class About : PhoneApplicationPage
    {





        //Spielvariabeln erstellen
        //---------------------------------------------------------------------------------------------------------------------------------
        //Größen Variabeln
        int smin = 120;
        int smax = 150;
        string settings = "";
        int cmin = 50;
        int cmax = 200;
        //---------------------------------------------------------------------------------------------------------------------------------




        //Wird am Anfang vom Spiel geladen
        //---------------------------------------------------------------------------------------------------------------------------------
        public About()
        {
            //Komponenten laden
            InitializeComponent();
        }
        //---------------------------------------------------------------------------------------------------------------------------------





         //Wird bei jedem Start der Seite geladen
        //---------------------------------------------------------------------------------------------------------------------------------
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            //Einstellungen laden oder erstellen
            //-----------------------------------------------
            //Wenn einstellungen vorhanden
            IsolatedStorageFile file3 = IsolatedStorageFile.GetUserStoreForApplication();
            if (file3.DirectoryExists("Settings"))
            {
                //Einstellungen laden
                IsolatedStorageFileStream filestream3 = file3.OpenFile("Settings/Settings.txt", FileMode.Open);
                StreamReader sr3 = new StreamReader(filestream3);
                settings = sr3.ReadToEnd();
                filestream3.Close();

            }
            //Einstellungen erstellen
            else
            {
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
            //-----------------------------------------------



            //Quadrate neu estellen
            //-----------------------------------------------
            CreateNewSquares(255, cmin, cmax);
            //-----------------------------------------------
        }
        //---------------------------------------------------------------------------------------------------------------------------------





        //Alle Quadrate neu errechnen
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

        //Quadrate neu erstellen
        void CreateNewSquares(int a, int min, int max)
        {
            //Zufallsgenerator erstellen
            Random random = new Random();
            //Quadrat Füllen
            r1 = random.Next(min, max); g1 = random.Next(min, max); b1 = random.Next(min, max);
            mySolidColorBrush1.Color = Color.FromArgb((byte)(a), (byte)(r1), (byte)(g1), (byte)(b1));
            RTAB.Fill = mySolidColorBrush1;
            r2 = random.Next(min, max); g2 = random.Next(min, max); b2 = random.Next(min, max);
            mySolidColorBrush2.Color = Color.FromArgb((byte)(a), (byte)(r2), (byte)(g2), (byte)(b2));
            RTRATE.Fill = mySolidColorBrush2;
            TBRATE.Foreground = mySolidColorBrush2;
            s2 = random.Next(smin, smax);
            RTRATE.Width = s2;
            RTRATE.Height = s2;
            r3 = random.Next(min, max); g3 = random.Next(min, max); b3 = random.Next(min, max);
            mySolidColorBrush3.Color = Color.FromArgb((byte)(a), (byte)(r3), (byte)(g3), (byte)(b3));
            RTMore.Fill = mySolidColorBrush3;
            TBMore1.Foreground = mySolidColorBrush3;
            TBMore2.Foreground = mySolidColorBrush3;
            TBMore3.Foreground = mySolidColorBrush3;
            s3 = random.Next(smin, smax);
            RTMore.Width = s3;
            RTMore.Height = s3;
            r4 = random.Next(min, max); g4 = random.Next(min, max); b4 = random.Next(min, max);
            mySolidColorBrush4.Color = Color.FromArgb((byte)(a), (byte)(r4), (byte)(g4), (byte)(b4));
            RTFB.Fill = mySolidColorBrush4;
            s4 = random.Next(smin, smax);
            RTFB.Width = s4;
            RTFB.Height = s4;
        }
        //---------------------------------------------------------------------------------------------------------------------------------





        //Buttons
        //---------------------------------------------------------------------------------------------------------------------------------
        //Button Rate
        private void btnRate(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MarketplaceReviewTask review = new MarketplaceReviewTask();
            review.Show();
        }


        //Button More
        private void btnMore(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MarketplaceSearchTask marketplaceSearchTask = new MarketplaceSearchTask();
            marketplaceSearchTask.SearchTerms = "xtrose";
            marketplaceSearchTask.Show();
        }

        //Button Facebook
        private void btnFacebook(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var wb = new WebBrowserTask();
            wb.URL = "http://www.facebook.com/windowsphonesevendev";
            wb.Show();
        }
        //---------------------------------------------------------------------------------------------------------------------------------






    }
}