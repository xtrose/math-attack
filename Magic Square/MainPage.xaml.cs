using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Magic_Square.Resources;
//Zum ändern der Farben
using System.Windows.Media;
//Für Timer
using System.Windows.Threading;
//Zum auslesen und löschen der Daten
using System.IO;
using System.IO.IsolatedStorage;
//Zum erweiterten schneiden von strings
using System.Text.RegularExpressions;
//Für Trial
using Microsoft.Phone.Marketplace;
using Microsoft.Phone.Tasks;





namespace Magic_Square
{
    public partial class MainPage : PhoneApplicationPage
    {





        //Spielvariabeln erstellen
        //---------------------------------------------------------------------------------------------------------------------------------
        //Größen Variabeln
        int smin = 120;
        int smax = 148;
        //Highscore Variabeln
        int hse = 0;
        int hsm = 0;
        int hsh = 0;
        string highscores = "";
        string trainingsettings = "";
        string settings = "";
        string playedlast = "";
        string playnext = "";
        int cmin = 50;
        int cmax = 170;
        //---------------------------------------------------------------------------------------------------------------------------------

        



        //Wird am Anfang vom Spiel geladen
        //---------------------------------------------------------------------------------------------------------------------------------
        public MainPage()
        {
            //Komponenten laden
            InitializeComponent();
        }
        //---------------------------------------------------------------------------------------------------------------------------------





        //Wird bei jedem Start der Seite geladen
        //---------------------------------------------------------------------------------------------------------------------------------
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            //Highscore laden oder erstellen
            //-----------------------------------------------
            //Prüfen ob Highscores schon erstellt wurden
            IsolatedStorageFile file1 = IsolatedStorageFile.GetUserStoreForApplication();
            //Wenn Highscores vorhanden
            if (file1.DirectoryExists("Highscores"))
            {
                //Highscores laden
                IsolatedStorageFileStream filestream1 = file1.OpenFile("Highscores/Highscores.txt", FileMode.Open);
                StreamReader sr1 = new StreamReader(filestream1);
                highscores = sr1.ReadToEnd();
                filestream1.Close();
            }
            //Highscores erstellen
            else
            {
                file1.CreateDirectory("Highscores");
                IsolatedStorageFileStream filestream1 = file1.CreateFile("Highscores/Highscores.txt");
                StreamWriter sw1 = new StreamWriter(filestream1);
                sw1.WriteLine("0A0A0");
                sw1.Flush();
                filestream1.Close();
                highscores = "0A0A0";
            }
            //Highscores in Variabeln schreiben
            string[] split = Regex.Split(highscores, "A");
            hse = Convert.ToInt32(split[0]);
            hsm = Convert.ToInt32(split[1]);
            hsh = Convert.ToInt32(split[2]);
            //-----------------------------------------------


            //Einstellungen laden oder erstellen
            //-----------------------------------------------
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
                sw3.WriteLine("50A170");
                sw3.Flush();
                filestream3.Close();
                settings = "50A170";
            }
            //Einstellungen in Variabeln schreiben
            string[] split3 = Regex.Split(settings, "A");
            cmin = Convert.ToInt32(split3[0]);
            cmax = Convert.ToInt32(split3[1]);
            //Trainings Einstellungen in Variabeln schreiben
            string[] split2 = Regex.Split(trainingsettings, "A");
            string trd = Convert.ToString(split2[0]);
            int trl = Convert.ToInt32(split2[1]);
            //Trainings Einstellungen in Square schreiben
            if (trd == "e")
            {
                TBTD.Text = "Easy";
            }
            else if (trd == "m")
            {
                TBTD.Text = "Medium";
            }
            else if (trd == "h")
            {
                TBTD.Text = "Hard";
            }
            TBTL.Text = "Level " + trl;
            //-----------------------------------------------


            //Prüfen was zuletzt gespielt wurde
            //-----------------------------------------------
            //Prüfen ob Datei besteht
            IsolatedStorageFile file4 = IsolatedStorageFile.GetUserStoreForApplication();
            if (file4.DirectoryExists("Played"))
            {
                //Einstellungen laden
                IsolatedStorageFileStream filestream4 = file4.OpenFile("Played/Played.txt", FileMode.Open);
                StreamReader sr4 = new StreamReader(filestream4);
                playedlast = sr4.ReadToEnd();
                filestream4.Close();
                if (Convert.ToString(playedlast[0]) == "E")
                {
                    playedlast = "Easy";
                }
                else if (Convert.ToString(playedlast[0]) == "M")
                {
                    playedlast = "Medium";
                }
                else
                {
                    playedlast = "Hard";
                }
            }
            //Datei erstellen
            else
            {
                //Einstellungen erstellen
                file4.CreateDirectory("Played");
                IsolatedStorageFileStream filestream4 = file4.CreateFile("Played/Played.txt");
                StreamWriter sw4 = new StreamWriter(filestream4);
                sw4.WriteLine("Easy");
                sw4.Flush();
                filestream4.Close();
                playedlast = "Easy";
            }
            //-----------------------------------------------

            
            //Game Buttons erstellen
            //-----------------------------------------------
            //Wenn zuletzt Easy gespielt wurde
            if (playedlast == "Easy")
            {
                //Texte erstellen
                TBB11.Text = "Easy";
                TBB21.Text = "Medium";
                TBB31.Text = "Hard";
                //Highscore in Textblock schreiben
                if (hse > 0)
                {
                    TBB12.Text = "" + hse;
                    TBB11.Margin = new Thickness(0, -60, 0, 0);
                }
                else
                {
                    TBB12.Text = "";
                    TBB11.Margin = new Thickness(0, -25, 0, 0);
                }
                if (hsm > 0)
                {
                    TBB22.Text = "" + hsm;
                    TBB21.Margin = new Thickness(0, -30, 0, 0);
                }
                else
                {
                    TBB22.Text = "";
                    TBB21.Margin = new Thickness(0, -10, 0, 0);
                }
                if (hsh > 0)
                {
                    TBB32.Text = "" + hsh;
                    TBB31.Margin = new Thickness(0, -30, 0, 0);
                }
                else
                {
                    TBB32.Text = "";
                    TBB31.Margin = new Thickness(0, -10, 0, 0);
                }
            }
            //Wenn zuletzt Medium gespielt wurde
            else if (playedlast == "Medium")
            {
                //Texte erstellen
                TBB11.Text = "Medium";
                TBB21.Text = "Easy";
                TBB31.Text = "Hard";
                //Highscore in Textblock schreiben
                if (hsm > 0)
                {
                    TBB12.Text = "" + hsm;
                    TBB11.Margin = new Thickness(0, -60, 0, 0);
                }
                else
                {
                    TBB12.Text = "";
                    TBB11.Margin = new Thickness(0, -25, 0, 0);
                }
                if (hse > 0)
                {
                    TBB22.Text = "" + hse;
                    TBB21.Margin = new Thickness(0, -30, 0, 0);
                }
                else
                {
                    TBB22.Text = "";
                    TBB21.Margin = new Thickness(0, -10, 0, 0);
                }
                if (hsh > 0)
                {
                    TBB32.Text = "" + hsh;
                    TBB31.Margin = new Thickness(0, -30, 0, 0);
                }
                else
                {
                    TBB32.Text = "";
                    TBB31.Margin = new Thickness(0, -10, 0, 0);
                }
            }
            //Wenn zuletzt Hard gespielt wurde
            else if (playedlast == "Hard")
            {
                //Texte erstellen
                TBB11.Text = "Hard";
                TBB21.Text = "Easy";
                TBB31.Text = "Medium";
                //Highscore in Textblock schreiben
                if (hsh > 0)
                {
                    TBB12.Text = "" + hsh;
                    TBB11.Margin = new Thickness(0, -60, 0, 0);
                }
                else
                {
                    TBB12.Text = "";
                    TBB11.Margin = new Thickness(0, -25, 0, 0);
                }
                if (hse > 0)
                {
                    TBB22.Text = "" + hse;
                    TBB21.Margin = new Thickness(0, -30, 0, 0);
                }
                else
                {
                    TBB22.Text = "";
                    TBB21.Margin = new Thickness(0, -10, 0, 0);
                }
                if (hsm > 0)
                {
                    TBB32.Text = "" + hsm;
                    TBB31.Margin = new Thickness(0, -30, 0, 0);
                }
                else
                {
                    TBB32.Text = "";
                    TBB31.Margin = new Thickness(0, -10, 0, 0);
                }
            }
            //-----------------------------------------------


            //Prüfen ob Free Version
            //-----------------------------------------------
            if ((Application.Current as App).IsTrial)
            {
            }
            //Bei Kaufversion
            else
            {
                TBBUY.Text = "Click Me";
                TBBUY.FontSize = 36;
            }
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
            RTB1.Fill = mySolidColorBrush1;
            TBB11.Foreground = mySolidColorBrush1;
            TBB12.Foreground = mySolidColorBrush1;
            s1 = random.Next(250, 290);
            RTB1.Width = s1;
            RTB1.Height = s1;
            r2 = random.Next(min, max); g2 = random.Next(min, max); b2 = random.Next(min, max);
            mySolidColorBrush2.Color = Color.FromArgb((byte)(a), (byte)(r2), (byte)(g2), (byte)(b2));
            RTB2.Fill = mySolidColorBrush2;
            TBB21.Foreground = mySolidColorBrush2;
            TBB22.Foreground = mySolidColorBrush2;
            s2 = random.Next(smin, smax);
            RTB2.Width = s2;
            RTB2.Height = s2;
            r3 = random.Next(min, max); g3 = random.Next(min, max); b3 = random.Next(min, max);
            mySolidColorBrush3.Color = Color.FromArgb((byte)(a), (byte)(r3), (byte)(g3), (byte)(b3));
            RTB3.Fill = mySolidColorBrush3;
            TBB31.Foreground = mySolidColorBrush3;
            TBB32.Foreground = mySolidColorBrush3;
            s3 = random.Next(smin, smax);
            RTB3.Width = s3;
            RTB3.Height = s3;
            r4 = random.Next(min, max); g4 = random.Next(min, max); b4 = random.Next(min, max);
            mySolidColorBrush4.Color = Color.FromArgb((byte)(a), (byte)(r4), (byte)(g4), (byte)(b4));
            RTTR.Fill = mySolidColorBrush4;
            TBTR.Foreground = mySolidColorBrush4;
            TBTD.Foreground = mySolidColorBrush4;
            TBTL.Foreground = mySolidColorBrush4;
            s4 = random.Next(smin, smax);
            RTTR.Width = s4;
            RTTR.Height = s4;
            r5 = random.Next(min, max); g5 = random.Next(min, max); b5 = random.Next(min, max);
            mySolidColorBrush5.Color = Color.FromArgb((byte)(a), (byte)(r5), (byte)(g5), (byte)(b5));
            RTTRS.Fill = mySolidColorBrush5;
            TBTRS.Foreground = mySolidColorBrush5;
            TBTRS2.Foreground = mySolidColorBrush5;
            s5 = random.Next(smin, smax);
            RTTRS.Width = s5;
            RTTRS.Height = s5;
            r6 = random.Next(min, max); g6 = random.Next(min, max); b6 = random.Next(min, max);
            mySolidColorBrush6.Color = Color.FromArgb((byte)(a), (byte)(r6), (byte)(g6), (byte)(b6));
            RTBUY.Fill = mySolidColorBrush6;
            TBBUY.Foreground = mySolidColorBrush6;
            s6 = random.Next(250, 300);
            RTBUY.Width = s6;
            RTBUY.Height = s6;
            r7 = random.Next(min, max); g7 = random.Next(min, max); b7 = random.Next(min, max);
            mySolidColorBrush7.Color = Color.FromArgb((byte)(a), (byte)(r7), (byte)(g7), (byte)(b7));
            RTHT.Fill = mySolidColorBrush7;
            TBHT.Foreground = mySolidColorBrush7;
            s7 = random.Next(smin, smax);
            RTHT.Width = s7;
            RTHT.Height = s7;
            r8 = random.Next(min, max); g8 = random.Next(min, max); b8 = random.Next(min, max);
            mySolidColorBrush8.Color = Color.FromArgb((byte)(a), (byte)(r8), (byte)(g8), (byte)(b8));
            RTAB.Fill = mySolidColorBrush8;
            TBAB.Foreground = mySolidColorBrush8;
            s8 = random.Next(smin, smax);
            RTAB.Width = s8;
            RTAB.Height = s8;
            r9 = random.Next(min, max); g9 = random.Next(min, max); b9 = random.Next(min, max);
            mySolidColorBrush9.Color = Color.FromArgb((byte)(a), (byte)(r9), (byte)(g9), (byte)(b9));
            RTST.Fill = mySolidColorBrush9;
            TBST.Foreground = mySolidColorBrush9;
            s9 = random.Next(smin, smax);
            RTST.Width = s9;
            RTST.Height = s9;
        }
        //---------------------------------------------------------------------------------------------------------------------------------





        //Spiel starten
        //---------------------------------------------------------------------------------------------------------------------------------
        void startgame()
        {
            //Zuletzt gespielt speichern
            IsolatedStorageFile file10 = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream filestream10 = file10.CreateFile("Played/Played.txt");
            StreamWriter sw10 = new StreamWriter(filestream10);
            sw10.WriteLine(playnext);
            sw10.Flush();
            filestream10.Close();
            playedlast = playnext;
            //Prüfen was ausgewählt und Spiel starten
            if (playnext == "Easy")
            {
                NavigationService.Navigate(new Uri("/Pages/Game.xaml?difficulty=easy", UriKind.Relative));
            }
            else if (playnext == "Medium")
            {
                NavigationService.Navigate(new Uri("/Pages/Game.xaml?difficulty=medium", UriKind.Relative));
            }
            else
            {
                NavigationService.Navigate(new Uri("/Pages/Game.xaml?difficulty=hard", UriKind.Relative));
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------





        //Buttons
        //---------------------------------------------------------------------------------------------------------------------------------
        //Button B1
        private void Button1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (playedlast == "Easy")
            {
                playnext = "Easy";
                startgame();
            }
            else if (playedlast == "Medium")
            {

                playnext = "Medium";
                startgame();
            }
            else if (playedlast == "Hard")
            {
                playnext = "Hard";
                startgame();
            }
        }

        //Button B2
        private void Button2(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (playedlast == "Easy")
            {
                playnext = "Medium";
                startgame();
            }
            else if (playedlast == "Medium")
            {
                playnext = "Easy";
                startgame();
            }
            else if (playedlast == "Hard")
            {
                playnext = "Easy";
                startgame();
            }
        }

        //Button unten
        private void Button3(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (playedlast == "Easy")
            {
                playnext = "Hard";
                startgame();
            }
            else if (playedlast == "Medium")
            {
                playnext = "Hard";
                startgame();
            }
            else if (playedlast == "Hard")
            {
                playnext = "Medium";
                startgame();
            }
        }


        //Button Settings
        private void btnSettings(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Settings.xaml", UriKind.Relative));
        }


        //Button Training
        private void btnMagicSquare_training(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Game.xaml?difficulty=training", UriKind.Relative));
        }

        //Button Training Settings
        private void btnTrainingSettings(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/TrainingSettings.xaml", UriKind.Relative));
        }

        //Button HowTo
        private void btnHowTo(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/HowTo.xaml", UriKind.Relative));
        }

        //Button About
        private void btnAbout(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/About.xaml", UriKind.Relative));
        }

        //Button Zum Kaufen
        MarketplaceDetailTask _marketPlaceDetailTask = new MarketplaceDetailTask();
        private void btnBuy(object sender, RoutedEventArgs e)
        {
            //Prüfen ob Free Version
            if ((Application.Current as App).IsTrial)
            {
                _marketPlaceDetailTask.Show();
            }
            //Bei Kaufversion
            else
            {
                CreateNewSquares(255, cmin, cmax);
            }
        }
        //---------------------------------------------------------------------------------------------------------------------------------
    }
}