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


using System.Windows.Navigation;
using Microsoft.Phone.BackgroundAudio;


//Für Soundeffekte
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;





namespace Magic_Square.Pages
{
    public partial class Game : PhoneApplicationPage
    {





        //Timer erstellen
        //---------------------------------------------------------------------------------------------------------------------------------
        DispatcherTimer dt = new DispatcherTimer();
        //---------------------------------------------------------------------------------------------------------------------------------



        

        //Spielvariabeln erstellen
        //---------------------------------------------------------------------------------------------------------------------------------
        //Allgemeine Variabeln
        string difficulty;
        int points = 0;
        int tpoints = 0;
        int timems = 0;
        int timestartms = 0;
        int highscore = 0;
        int startlevel = 1;
        //Für Animation der Punkte
        string anipoints = "";
        int anipointsms = 0;
        //Für Animation der Zeit
        string anitime = "";
        int anitimems = 0;
        //Farb Variabeln
        int smin = 80;
        int smax = 120;
        //Highscore Variabeln
        int hse = 0;
        int hsm = 0;
        int hsh = 0;
        string highscores = "";
        //Trainings Variabeln
        string training = "no";
        string trainingssettings = "eA1";
        //Farbeinstellungen
        string settings = "50A170";
        int cmin = 50;
        int cmax = 170;
        //Trial einstellungen
        string trial = "yes";
        //Soundtrack
        int Soundtrack;
        //---------------------------------------------------------------------------------------------------------------------------------





        //Wird am Anfang vom Spiel geladen
        //---------------------------------------------------------------------------------------------------------------------------------
        public Game()
        {
            //Komponenten laden
            InitializeComponent();

            //Farben erstellen, Tastenfeld, Punkte und Zeit
            int min = 160;
            int max = 170;
            Random random = new Random();
            SolidColorBrush mySolidColorBrushN = new SolidColorBrush();
            r1 = random.Next(min, max); g1 = random.Next(min, max); b1 = random.Next(min, max);
            mySolidColorBrushN.Color = System.Windows.Media.Color.FromArgb((byte)(255), (byte)(r1), (byte)(g1), (byte)(b1));
            RTZ1.Fill = mySolidColorBrushN;
            RTZ2.Fill = mySolidColorBrushN;
            RTZ3.Fill = mySolidColorBrushN;
            RTZ4.Fill = mySolidColorBrushN;
            RTZ5.Fill = mySolidColorBrushN;
            RTZ6.Fill = mySolidColorBrushN;
            RTZ7.Fill = mySolidColorBrushN;
            RTZ8.Fill = mySolidColorBrushN;
            RTZ9.Fill = mySolidColorBrushN;
            RTZXX.Fill = mySolidColorBrushN;
            RTZ0.Fill = mySolidColorBrushN;
            RTZX.Fill = mySolidColorBrushN;
            POINTS.Foreground = mySolidColorBrushN;
            TIME.Foreground = mySolidColorBrushN;
            LEVEL.Foreground = mySolidColorBrushN;
            HIGHSCORE.Foreground = mySolidColorBrushN;
            COUNT.Foreground = mySolidColorBrushN;
            //Highscore erstellen
            HIGHSCORE.Text = "Highscore " + highscore;

            //Timer erstellen
            dt.Stop();
            dt.Interval = new TimeSpan(0, 0, 0, 0, 1);
            dt.Tick += new EventHandler(dt_Tick);
            dt.Start();
        }
        //---------------------------------------------------------------------------------------------------------------------------------





        //Datei laden
        //---------------------------------------------------------------------------------------------------------------------------------
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            //Variable für Schwierigkeitsgrad ermitteln
            difficulty = NavigationContext.QueryString["difficulty"];
            base.OnNavigatedTo(e);

            //Prüfen ob Free Version
            if ((Application.Current as App).IsTrial)
            {
                trial = "yes";
            }
            else
            {
                trial = "no";
            }

            //Prüfen ob Training und Trainingsoptionen laden
            if (difficulty == "training")
            {
                training = "yes";
                TIME.Text = "Training";
                IsolatedStorageFile file2 = IsolatedStorageFile.GetUserStoreForApplication();
                //Wenn einstellungen vorhanden
                if (file2.DirectoryExists("Settings"))
                {
                    //Highscores laden
                    IsolatedStorageFileStream filestream2 = file2.OpenFile("Settings/TrainingsSettings.txt", FileMode.Open);
                    StreamReader sr2 = new StreamReader(filestream2);
                    trainingssettings = sr2.ReadToEnd();
                    filestream2.Close();
                }
                //Highscores erstellen
                else
                {
                    file2.CreateDirectory("Highscores");
                    IsolatedStorageFileStream filestream2 = file2.CreateFile("Settings/TrainingsSettings.txt");
                    StreamWriter sw2 = new StreamWriter(filestream2);
                    sw2.WriteLine("eA1");
                    sw2.Flush();
                    filestream2.Close();
                    trainingssettings = "eA1";
                }
                //Trainings Einstellungen erstellen
                string[] split2 = Regex.Split(trainingssettings, "A");
                string trd = Convert.ToString(split2[0]);
                int trl = Convert.ToInt32(split2[1]);
                if (trd == "e")
                {
                    difficulty = "easy";
                }
                else if (trd == "m")
                {
                    difficulty = "medium";
                }
                else if (trd == "h")
                {
                    difficulty = "hard";
                }
                LEVEL.Text = "Level " + trl;
                startlevel = Convert.ToInt32(trl);
            }


            //Highscore laden oder erstellen
            //-----------------------------------------------
            //Prüfen ob Highscores schon erstellt wurden
            IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication();
            //Wenn einstellungen vorhanden
            if (file.DirectoryExists("Highscores"))
            {
                //Highscores laden
                IsolatedStorageFileStream filestream = file.OpenFile("Highscores/Highscores.txt", FileMode.Open);
                StreamReader sr = new StreamReader(filestream);
                highscores = sr.ReadToEnd();
                filestream.Close();
            }
            //Highscores erstellen
            else
            {
                file.CreateDirectory("Highscores");
                IsolatedStorageFileStream filestream = file.CreateFile("Highscores/Highscores.txt");
                StreamWriter sw = new StreamWriter(filestream);
                sw.WriteLine("0A0A0");
                sw.Flush();
                filestream.Close();
                highscores = "0A0A0";
            }
            //Highscores in Variabeln schreiben
            string[] split = Regex.Split(highscores, "A");
            hse = Convert.ToInt32(split[0]);
            hsm = Convert.ToInt32(split[1]);
            hsh = Convert.ToInt32(split[2]);
            //Highscore ausgeben
            if (difficulty == "easy")
            {
                highscore = hse;
                HIGHSCORE.Text = "Highscore " + hse;
            }
            else if (difficulty == "medium")
            {
                highscore = hsm;
                HIGHSCORE.Text = "Highscore " + hsm;
            }
            else if (difficulty == "hard")
            {
                highscore = hsh;
                HIGHSCORE.Text = "Highscore " + hsh;
            }

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

            //SelectSound("Random");
            BackgroundAudioPlayer.Instance.Play();
        }
        //---------------------------------------------------------------------------------------------------------------------------------





        //Aktionen
        //---------------------------------------------------------------------------------------------------------------------------------
                
        //Quadrate neu erstellen
        //---------------------------------------------------------------
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
        //Variabeln der Größen erstellen
        int s1 = 0; int s2 = 0; int s3 = 0; int s4 = 0; int s5 = 0; int s6 = 0; int s7 = 0; int s8 = 0; int s9 = 0;
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
        SolidColorBrush mySolidColorBrushB = new SolidColorBrush();

        //Qadrate neu erstellen
        void ChangeAllSquareColors(int a, int min, int max)
        {
            //Zufallsgenerator erstellen
            Random random = new Random();
            //Quadrat Füllen
            r1 = random.Next(min, max); g1 = random.Next(min, max); b1 = random.Next(min, max);
            mySolidColorBrush1.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r1), (byte)(g1), (byte)(b1));
            RT1.Fill = mySolidColorBrush1;
            s1 = random.Next(smin, smax);
            RT1.Width = s1;
            RT1.Height = s1;
            r2 = random.Next(min, max); g2 = random.Next(min, max); b2 = random.Next(min, max);
            mySolidColorBrush2.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r2), (byte)(g2), (byte)(b2));
            RT2.Fill = mySolidColorBrush2;
            s2 = random.Next(smin, smax);
            RT2.Width = s2;
            RT2.Height = s2;
            r3 = random.Next(min, max); g3 = random.Next(min, max); b3 = random.Next(min, max);
            mySolidColorBrush3.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r3), (byte)(g3), (byte)(b3));
            RT3.Fill = mySolidColorBrush3;
            s3 = random.Next(smin, smax);
            RT3.Width = s3;
            RT3.Height = s3;
            r4 = random.Next(min, max); g4 = random.Next(min, max); b4 = random.Next(min, max);
            mySolidColorBrush4.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r4), (byte)(g4), (byte)(b4));
            RT4.Fill = mySolidColorBrush4;
            s4 = random.Next(smin, smax);
            RT4.Width = s4;
            RT4.Height = s4;
            r5 = random.Next(min, max); g5 = random.Next(min, max); b5 = random.Next(min, max);
            mySolidColorBrush5.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r5), (byte)(g5), (byte)(b5));
            RT5.Fill = mySolidColorBrush5;
            s5 = random.Next(smin, smax);
            RT5.Width = s5;
            RT5.Height = s5;
            r6 = random.Next(min, max); g6 = random.Next(min, max); b6 = random.Next(min, max);
            mySolidColorBrush6.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r6), (byte)(g6), (byte)(b6));
            RT6.Fill = mySolidColorBrush6;
            s6 = random.Next(smin, smax);
            RT6.Width = s6;
            RT6.Height = s6;
            r7 = random.Next(min, max); g7 = random.Next(min, max); b7 = random.Next(min, max);
            mySolidColorBrush7.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r7), (byte)(g7), (byte)(b7));
            RT7.Fill = mySolidColorBrush7;
            s7 = random.Next(smin, smax);
            RT7.Width = s7;
            RT7.Height = s7;
            r8 = random.Next(min, max); g8 = random.Next(min, max); b8 = random.Next(min, max);
            mySolidColorBrush8.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r8), (byte)(g8), (byte)(b8));
            RT8.Fill = mySolidColorBrush8;
            s8 = random.Next(smin, smax);
            RT8.Width = s8;
            RT8.Height = s8;
            r9 = random.Next(min, max); g9 = random.Next(min, max); b9 = random.Next(min, max);
            mySolidColorBrush9.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r9), (byte)(g9), (byte)(b9));
            RT9.Fill = mySolidColorBrush9;
            s9 = random.Next(smin, smax);
            RT9.Width = s9;
            RT9.Height = s9;
        }
        //---------------------------------------------------------------


        //Levels neu erstellen
        //---------------------------------------------------------------
        //Variabeln erstellen
        int bonus = 0;
        int level = 1;
        int n1 = 0;
        int n2 = 0;
        int n3 = 0;
        int n4 = 0;
        int n5 = 0;
        int n6 = 0;
        int n7 = 0;
        int n8 = 0;
        int n9 = 0;
        string set = "000000000";
        int setmax = 52;
        string[] sets = new string[] { "110110000", "011011000", "000011011", "000110110", "101011000", "001010011", "000110101", "110010100", "110011000", "001011010", "000110011", "010110100", "011110000", "010011001", "000011110", "100110010", "100101001", "001101100", "110000011", "011000110", "110100001", "011001100", "100001011", "001100110", "110100010", "011101000", "010001011", "000101110", "110101000", "011001010", "000101011", "010100110", "011000101", "100001101", "101000110", "101100001", "110000101", "101001100", "101000011", "001100101", "010101100", "110001010", "001101010", "010100011", "010101001", "010001110", "100101010", "011100010", "101101000", "000101101", "110000110", "011000011" };
        string setbonus = "101000101";

        //Level erstellen
        void CreateNewLevel()
        {
            //Random erstellen
            Random random = new Random();
            //Variabeln erstellen
            int numbers;
            //Prüfen ob Bonus Level
            if (bonus != 0)
            {
                numbers = bonus;
            }
            else
            {
                numbers = level * 2;
            }
            //Zahlen erstellen
            n1 = random.Next(1, numbers);
            n2 = random.Next(1, numbers);
            n3 = n1 + n2;
            n4 = random.Next(1, numbers);
            n5 = random.Next(1, numbers);
            n6 = n4 + n5;
            n7 = n1 + n4;
            n8 = n2 + n5;
            n9 = n3 + n6;

            //Zahlen mischen ************************************************************************* Medium, Hard ********************************************************************
            if (difficulty == "medium" | difficulty == "hard")
            {
                int shuffle = random.Next(1, 4);
                if (shuffle == 1)
                {
                    int t1 = n3; int t2 = n6; int t3 = n9; int t4 = n2; int t6 = n8; int t7 = n1; int t8 = n4; int t9 = n7;
                    n1 = t1; n2 = t2; n3 = t3; n4 = t4; n6 = t6; n7 = t7; n8 = t8; n9 = t9;
                }
                else if (shuffle == 2)
                {
                    int t1 = n9; int t2 = n8; int t3 = n7; int t4 = n6; int t6 = n4; int t7 = n3; int t8 = n2; int t9 = n1;
                    n1 = t1; n2 = t2; n3 = t3; n4 = t4; n6 = t6; n7 = t7; n8 = t8; n9 = t9;
                }
                else if (shuffle == 3)
                {
                    int t1 = n7; int t2 = n4; int t3 = n1; int t4 = n8; int t6 = n2; int t7 = n9; int t8 = n6; int t9 = n3;
                    n1 = t1; n2 = t2; n3 = t3; n4 = t4; n6 = t6; n7 = t7; n8 = t8; n9 = t9;
                }
            }
            //***************************************************************************************************************************************************************************

            //Set auswählen
            if (bonus == 0)
            {
                int temp = random.Next(1, setmax);
                set = sets[temp];
            }
            //Bonus Set auswählen
            else
            {
                set = setbonus;
            }
            //Level erstellen
            if (set[0].ToString() == "1")
            {
                TB1.Text = "" + n1;
            }
            else
            {
                TB1.Text = "";
            }
            if (set[1].ToString() == "1")
            {
                TB2.Text = "" + n2;
            }
            else
            {
                TB2.Text = "";
            }
            if (set[2].ToString() == "1")
            {
                TB3.Text = "" + n3;
            }
            else
            {
                TB3.Text = "";
            }
            if (set[3].ToString() == "1")
            {
                TB4.Text = "" + n4;
            }
            else
            {
                TB4.Text = "";
            }
            if (set[4].ToString() == "1")
            {
                TB5.Text = "" + n5;
            }
            else
            {
                TB5.Text = "";
            }
            if (set[5].ToString() == "1")
            {
                TB6.Text = "" + n6;
            }
            else
            {
                TB6.Text = "";
            }
            if (set[6].ToString() == "1")
            {
                TB7.Text = "" + n7;
            }
            else
            {
                TB7.Text = "";
            }
            if (set[7].ToString() == "1")
            {
                TB8.Text = "" + n8;
            }
            else
            {
                TB8.Text = "";
            }
            if (set[8].ToString() == "1")
            {
                TB9.Text = "" + n9;
            }
            else
            {
                TB9.Text = "";
            }
        }
        //---------------------------------------------------------------


        //Ergebnisse vergleichen
        //---------------------------------------------------------------
        void CheckResult()
        {
            //Bei Feld 1
            if (btnactiv == 1)
            {
                //Feld in int umwandeln
                int tz = Convert.ToInt32(TBB.Text);
                string stz = "" + tz;
                int temppoints = 0;
                //Prüfen ob Zahl richtig ist
                if (n1 == tz)
                {
                    //Zahl in Textbox schreiben
                    TB1.Text = "" + n1;
                    //Punkte erstellen
                    temppoints += tz;
                    //Prüfen ob Zahlenreihen bestehen
                    if (TB2.Text != "" & TB3.Text != "")
                    {
                        temppoints += Convert.ToInt32(TB2.Text);
                        temppoints += Convert.ToInt32(TB3.Text);
                        r2 = r1; g2 = g1; b2 = b1; s2 = s1;
                        r3 = r1; g3 = g1; b3 = b1; s3 = s1;
                    }
                    if (TB4.Text != "" & TB7.Text != "")
                    {
                        temppoints += Convert.ToInt32(TB4.Text);
                        temppoints += Convert.ToInt32(TB7.Text);
                        r4 = r1; g4 = g1; b4 = b1; s4 = s1;
                        r7 = r1; g7 = g1; b7 = b1; s7 = s1 ;
                    }
                    //Punkte dazu rechnen
                    points += temppoints;
                    //Punkteanimation erstellen
                    anipoints = "+ " + temppoints;
                    //btnactive zurücksetzen
                    btnactiv = 0;
                    //Buttons zurücksetzen
                    ButtonsReset();
                    //Extra Button zurücksetzen
                    GRB.Margin = new Thickness(-200, 0, 0, 0);

                    //Bei Hard, Zahlen drehen ***************************************************************** Hard ***************************************
                    if ((TB1.Text == "" | TB2.Text == "" | TB3.Text == "" | TB4.Text == "" | TB5.Text == "" | TB6.Text == "" | TB7.Text == "" | TB8.Text == "" | TB9.Text == "") & difficulty == "hard")
                    {
                        int tl = stz.Length;
                        if (stz[(tl-1)].ToString() == "1" | stz[(tl-1)].ToString() == "3" | stz[(tl-1)].ToString() == "5" | stz[(tl-1)].ToString() == "7" | stz[(tl-1)].ToString() == "9")
                        {
                            statusturn = "right";
                        }
                        else
                        {
                            statusturn = "left";
                        }
                    }
                    //*****************************************************************************************************************************************
                }
            }

            //Bei Feld 2
            if (btnactiv == 2)
            {
                //Feld in int umwandeln
                int tz = Convert.ToInt32(TBB.Text);
                string stz = "" + tz;
                int temppoints = 0;
                //Prüfen ob Zahl richtig ist
                if (n2 == tz)
                {
                    //Zahl in Textbox schreiben
                    TB2.Text = "" + n2;
                    //Punkte erstellen
                    temppoints += tz;
                    //Prüfen ob Zahlenreihen bestehen
                    if (TB1.Text != "" & TB3.Text != "")
                    {
                        temppoints += Convert.ToInt32(TB1.Text);
                        temppoints += Convert.ToInt32(TB3.Text);
                        r1 = r2; g1 = g2; b1 = b2; s1 = s2;
                        r3 = r2; g3 = g2; b3 = b2; s3 = s2;
                    }
                    if (TB5.Text != "" & TB8.Text != "")
                    {
                        temppoints += Convert.ToInt32(TB5.Text);
                        temppoints += Convert.ToInt32(TB8.Text);
                        r5 = r2; g5 = g2; b5 = b2; s5 = s2;
                        r8 = r2; g8 = g2; b8 = b2; s8 = s2;
                    }
                    //Punkte dazu rechnen
                    points += temppoints;
                    //Punkteanimation erstellen
                    anipoints = "+ " + temppoints;
                    //btnactive zurücksetzen
                    btnactiv = 0;
                    //Buttons zurücksetzen
                    ButtonsReset();
                    //Extra Button zurücksetzen
                    GRB.Margin = new Thickness(-200, 0, 0, 0);

                    //Bei Hard, Zahlen drehen ***************************************************************** Hard ***************************************
                    if ((TB1.Text == "" | TB2.Text == "" | TB3.Text == "" | TB4.Text == "" | TB5.Text == "" | TB6.Text == "" | TB7.Text == "" | TB8.Text == "" | TB9.Text == "") & difficulty == "hard")
                    {
                        int tl = stz.Length;
                        if (stz[(tl - 1)].ToString() == "1" | stz[(tl - 1)].ToString() == "3" | stz[(tl - 1)].ToString() == "5" | stz[(tl - 1)].ToString() == "7" | stz[(tl - 1)].ToString() == "9")
                        {
                            statusturn = "right";
                        }
                        else
                        {
                            statusturn = "left";
                        }
                    }
                    //*****************************************************************************************************************************************
                }
            }

            //Bei Feld 3
            if (btnactiv == 3)
            {
                //Feld in int umwandeln
                int tz = Convert.ToInt32(TBB.Text);
                string stz = "" + tz;
                int temppoints = 0;
                //Prüfen ob Zahl richtig ist
                if (n3 == tz)
                {
                    //Zahl in Textbox schreiben
                    TB3.Text = "" + n3;
                    //Punkte erstellen
                    temppoints += tz;
                    //Prüfen ob Zahlenreihen bestehen
                    if (TB1.Text != "" & TB2.Text != "")
                    {
                        temppoints += Convert.ToInt32(TB1.Text);
                        temppoints += Convert.ToInt32(TB2.Text);
                        r1 = r3; g1 = g3; b1 = b3; s1 = s3;
                        r2 = r3; g2 = g3; b2 = b3; s2 = s3;
                    }
                    if (TB6.Text != "" & TB9.Text != "")
                    {
                        temppoints += Convert.ToInt32(TB6.Text);
                        temppoints += Convert.ToInt32(TB9.Text);
                        r6 = r3; g6 = g2; b6 = b3; s6 = s3;
                        r9 = r3; g9 = g3; b9 = b3; s9 = s3;
                    }
                    //Punkte dazu rechnen
                    points += temppoints;
                    //Punkteanimation erstellen
                    anipoints = "+ " + temppoints;
                    //btnactive zurücksetzen
                    btnactiv = 0;
                    //Buttons zurücksetzen
                    ButtonsReset();
                    //Extra Button zurücksetzen
                    GRB.Margin = new Thickness(-200, 0, 0, 0);

                    //Bei Hard, Zahlen drehen ***************************************************************** Hard ***************************************
                    if ((TB1.Text == "" | TB2.Text == "" | TB3.Text == "" | TB4.Text == "" | TB5.Text == "" | TB6.Text == "" | TB7.Text == "" | TB8.Text == "" | TB9.Text == "") & difficulty == "hard")
                    {
                        int tl = stz.Length;
                        if (stz[(tl - 1)].ToString() == "1" | stz[(tl - 1)].ToString() == "3" | stz[(tl - 1)].ToString() == "5" | stz[(tl - 1)].ToString() == "7" | stz[(tl - 1)].ToString() == "9")
                        {
                            statusturn = "right";
                        }
                        else
                        {
                            statusturn = "left";
                        }
                    }
                    //*****************************************************************************************************************************************
                }
            }

            //Bei Feld 4
            if (btnactiv == 4)
            {
                //Feld in int umwandeln
                int tz = Convert.ToInt32(TBB.Text);
                string stz = "" + tz;
                int temppoints = 0;
                //Prüfen ob Zahl richtig ist
                if (n4 == tz)
                {
                    //Zahl in Textbox schreiben
                    TB4.Text = "" + n4;
                    //Punkte erstellen
                    temppoints += tz;
                    //Prüfen ob Zahlenreihen bestehen
                    if (TB5.Text != "" & TB6.Text != "")
                    {
                        temppoints += Convert.ToInt32(TB5.Text);
                        temppoints += Convert.ToInt32(TB6.Text);
                        r5 = r4; g5 = g4; b5 = b4; s5 = s4;
                        r6 = r4; g6 = g4; b6 = b4; s6 = s4;
                    }
                    if (TB1.Text != "" & TB7.Text != "")
                    {
                        temppoints += Convert.ToInt32(TB1.Text);
                        temppoints += Convert.ToInt32(TB7.Text);
                        r1 = r4; g1 = g4; b1 = b4; s1 = s4;
                        r7 = r4; g7 = g4; b7 = b4; s7 = s4;
                    }
                    //Punkte dazu rechnen
                    points += temppoints;
                    //Punkteanimation erstellen
                    anipoints = "+ " + temppoints;
                    //btnactive zurücksetzen
                    btnactiv = 0;
                    //Buttons zurücksetzen
                    ButtonsReset();
                    //Extra Button zurücksetzen
                    GRB.Margin = new Thickness(-200, 0, 0, 0);

                    //Bei Hard, Zahlen drehen ***************************************************************** Hard ***************************************
                    if ((TB1.Text == "" | TB2.Text == "" | TB3.Text == "" | TB4.Text == "" | TB5.Text == "" | TB6.Text == "" | TB7.Text == "" | TB8.Text == "" | TB9.Text == "") & difficulty == "hard")
                    {
                        int tl = stz.Length;
                        if (stz[(tl - 1)].ToString() == "1" | stz[(tl - 1)].ToString() == "3" | stz[(tl - 1)].ToString() == "5" | stz[(tl - 1)].ToString() == "7" | stz[(tl - 1)].ToString() == "9")
                        {
                            statusturn = "right";
                        }
                        else
                        {
                            statusturn = "left";
                        }
                    }
                    //*****************************************************************************************************************************************
                }
            }

            //Bei Feld 5
            if (btnactiv == 5)
            {
                //Feld in int umwandeln
                int tz = Convert.ToInt32(TBB.Text);
                string stz = "" + tz;
                int temppoints = 0;
                //Prüfen ob Zahl richtig ist
                if (n5 == tz)
                {
                    //Zahl in Textbox schreiben
                    TB5.Text = "" + n5;
                    //Punkte erstellen
                    temppoints += tz;
                    //Prüfen ob Zahlenreihen bestehen
                    if (TB4.Text != "" & TB6.Text != "")
                    {
                        temppoints += Convert.ToInt32(TB4.Text);
                        temppoints += Convert.ToInt32(TB6.Text);
                        r4 = r5; g4 = g5; b4 = b5; s4 = s5;
                        r6 = r5; g6 = g5; b6 = b5; s6 = s5;
                    }
                    if (TB2.Text != "" & TB8.Text != "")
                    {
                        temppoints += Convert.ToInt32(TB2.Text);
                        temppoints += Convert.ToInt32(TB8.Text);
                        r2 = r5; g2 = g5; b2 = b5; s2 = s5;
                        r8 = r5; g8 = g5; b8 = b5; s8 = s5;
                    }
                    //Punkte dazu rechnen
                    points += temppoints;
                    //Punkteanimation erstellen
                    anipoints = "+ " + temppoints;
                    //btnactive zurücksetzen
                    btnactiv = 0;
                    //Buttons zurücksetzen
                    ButtonsReset();
                    //Extra Button zurücksetzen
                    GRB.Margin = new Thickness(-200, 0, 0, 0);

                    //Bei Hard, Zahlen drehen ***************************************************************** Hard ***************************************
                    if ((TB1.Text == "" | TB2.Text == "" | TB3.Text == "" | TB4.Text == "" | TB5.Text == "" | TB6.Text == "" | TB7.Text == "" | TB8.Text == "" | TB9.Text == "") & difficulty == "hard")
                    {
                        int tl = stz.Length;
                        if (stz[(tl - 1)].ToString() == "1" | stz[(tl - 1)].ToString() == "3" | stz[(tl - 1)].ToString() == "5" | stz[(tl - 1)].ToString() == "7" | stz[(tl - 1)].ToString() == "9")
                        {
                            statusturn = "right";
                        }
                        else
                        {
                            statusturn = "left";
                        }
                    }
                    //*****************************************************************************************************************************************
                }
            }

            //Bei Feld 6
            if (btnactiv == 6)
            {
                //Feld in int umwandeln
                int tz = Convert.ToInt32(TBB.Text);
                string stz = "" + tz;
                int temppoints = 0;
                //Prüfen ob Zahl richtig ist
                if (n6 == tz)
                {
                    //Zahl in Textbox schreiben
                    TB6.Text = "" + n6;
                    //Punkte erstellen
                    temppoints += tz;
                    //Prüfen ob Zahlenreihen bestehen
                    if (TB4.Text != "" & TB5.Text != "")
                    {
                        temppoints += Convert.ToInt32(TB4.Text);
                        temppoints += Convert.ToInt32(TB5.Text);
                        r4 = r6; g4 = g6; b4 = b6; s4 = s6;
                        r5 = r6; g5 = g6; b5 = b6; s5 = s6;
                    }
                    if (TB3.Text != "" & TB9.Text != "")
                    {
                        temppoints += Convert.ToInt32(TB3.Text);
                        temppoints += Convert.ToInt32(TB9.Text);
                        r3 = r6; g3 = g6; b3 = b6; s3 = s6;
                        r9 = r6; g9 = g6; b9 = b6; s9 = s6;
                    }
                    //Punkte dazu rechnen
                    points += temppoints;
                    //Punkteanimation erstellen
                    anipoints = "+ " + temppoints;
                    //btnactive zurücksetzen
                    btnactiv = 0;
                    //Buttons zurücksetzen
                    ButtonsReset();
                    //Extra Button zurücksetzen
                    GRB.Margin = new Thickness(-200, 0, 0, 0);

                    //Bei Hard, Zahlen drehen ***************************************************************** Hard ***************************************
                    if ((TB1.Text == "" | TB2.Text == "" | TB3.Text == "" | TB4.Text == "" | TB5.Text == "" | TB6.Text == "" | TB7.Text == "" | TB8.Text == "" | TB9.Text == "") & difficulty == "hard")
                    {
                        int tl = stz.Length;
                        if (stz[(tl - 1)].ToString() == "1" | stz[(tl - 1)].ToString() == "3" | stz[(tl - 1)].ToString() == "5" | stz[(tl - 1)].ToString() == "7" | stz[(tl - 1)].ToString() == "9")
                        {
                            statusturn = "right";
                        }
                        else
                        {
                            statusturn = "left";
                        }
                    }
                    //*****************************************************************************************************************************************
                }
            }

            //Bei Feld 7
            if (btnactiv == 7)
            {
                //Feld in int umwandeln
                int tz = Convert.ToInt32(TBB.Text);
                string stz = "" + tz;
                int temppoints = 0;
                //Prüfen ob Zahl richtig ist
                if (n7 == tz)
                {
                    //Zahl in Textbox schreiben
                    TB7.Text = "" + n7;
                    //Punkte erstellen
                    temppoints += tz;
                    //Prüfen ob Zahlenreihen bestehen
                    if (TB8.Text != "" & TB9.Text != "")
                    {
                        temppoints += Convert.ToInt32(TB8.Text);
                        temppoints += Convert.ToInt32(TB9.Text);
                        r8 = r7; g8 = g7; b8 = b7; s8 = s7;
                        r9 = r7; g9 = g7; b9 = b7; s9 = s7;
                    }
                    if (TB1.Text != "" & TB4.Text != "")
                    {
                        temppoints += Convert.ToInt32(TB1.Text);
                        temppoints += Convert.ToInt32(TB4.Text);
                        r1 = r7; g1 = g7; b1 = b7; s1 = s7;
                        r4 = r7; g4 = g7; b4 = b7; s4 = s7;
                    }
                    //Punkte dazu rechnen
                    points += temppoints;
                    //Punkteanimation erstellen
                    anipoints = "+ " + temppoints;
                    //btnactive zurücksetzen
                    btnactiv = 0;
                    //Buttons zurücksetzen
                    ButtonsReset();
                    //Extra Button zurücksetzen
                    GRB.Margin = new Thickness(-200, 0, 0, 0);

                    //Bei Hard, Zahlen drehen ***************************************************************** Hard ***************************************
                    if ((TB1.Text == "" | TB2.Text == "" | TB3.Text == "" | TB4.Text == "" | TB5.Text == "" | TB6.Text == "" | TB7.Text == "" | TB8.Text == "" | TB9.Text == "") & difficulty == "hard")
                    {
                        int tl = stz.Length;
                        if (stz[(tl - 1)].ToString() == "1" | stz[(tl - 1)].ToString() == "3" | stz[(tl - 1)].ToString() == "5" | stz[(tl - 1)].ToString() == "7" | stz[(tl - 1)].ToString() == "9")
                        {
                            statusturn = "right";
                        }
                        else
                        {
                            statusturn = "left";
                        }
                    }
                    //*****************************************************************************************************************************************
                }
            }

            //Bei Feld 8
            if (btnactiv == 8)
            {
                //Feld in int umwandeln
                int tz = Convert.ToInt32(TBB.Text);
                string stz = "" + tz;
                int temppoints = 0;
                //Prüfen ob Zahl richtig ist
                if (n8 == tz)
                {
                    //Zahl in Textbox schreiben
                    TB8.Text = "" + n8;
                    //Punkte erstellen
                    temppoints += tz;
                    //Prüfen ob Zahlenreihen bestehen
                    if (TB7.Text != "" & TB9.Text != "")
                    {
                        temppoints += Convert.ToInt32(TB7.Text);
                        temppoints += Convert.ToInt32(TB9.Text);
                        r7 = r8; g7 = g8; b7 = b8; s7 = s8;
                        r9 = r8; g9 = g8; b9 = b8; s9 = s8;
                    }
                    if (TB2.Text != "" & TB5.Text != "")
                    {
                        temppoints += Convert.ToInt32(TB2.Text);
                        temppoints += Convert.ToInt32(TB5.Text);
                        r2 = r8; g2 = g8; b2 = b8; s2 = s8;
                        r5 = r8; g5 = g8; b5 = b8; s5 = s8;
                    }
                    //Punkte dazu rechnen
                    points += temppoints;
                    //Punkteanimation erstellen
                    anipoints = "+ " + temppoints;
                    //btnactive zurücksetzen
                    btnactiv = 0;
                    //Buttons zurücksetzen
                    ButtonsReset();
                    //Extra Button zurücksetzen
                    GRB.Margin = new Thickness(-200, 0, 0, 0);

                    //Bei Hard, Zahlen drehen ***************************************************************** Hard ***************************************
                    if ((TB1.Text == "" | TB2.Text == "" | TB3.Text == "" | TB4.Text == "" | TB5.Text == "" | TB6.Text == "" | TB7.Text == "" | TB8.Text == "" | TB9.Text == "") & difficulty == "hard")
                    {
                        int tl = stz.Length;
                        if (stz[(tl - 1)].ToString() == "1" | stz[(tl - 1)].ToString() == "3" | stz[(tl - 1)].ToString() == "5" | stz[(tl - 1)].ToString() == "7" | stz[(tl - 1)].ToString() == "9")
                        {
                            statusturn = "right";
                        }
                        else
                        {
                            statusturn = "left";
                        }
                    }
                    //*****************************************************************************************************************************************
                }
            }

            //Bei Feld 9
            if (btnactiv == 9)
            {
                //Feld in int umwandeln
                int tz = Convert.ToInt32(TBB.Text);
                string stz = "" + tz;
                int temppoints = 0;
                //Prüfen ob Zahl richtig ist
                if (n9 == tz)
                {
                    //Zahl in Textbox schreiben
                    TB9.Text = "" + n9;
                    //Punkte erstellen
                    temppoints += tz;
                    //Prüfen ob Zahlenreihen bestehen
                    if (TB7.Text != "" & TB8.Text != "")
                    {
                        temppoints += Convert.ToInt32(TB7.Text);
                        temppoints += Convert.ToInt32(TB8.Text);
                        r7 = r9; g7 = g9; b7 = b9; s7 = s9;
                        r8 = r9; g8 = g9; b8 = b9; s8 = s9;
                    }
                    if (TB3.Text != "" & TB6.Text != "")
                    {
                        temppoints += Convert.ToInt32(TB3.Text);
                        temppoints += Convert.ToInt32(TB6.Text);
                        r3 = r9; g3 = g9; b3 = b9; s3 = s9;
                        r6 = r9; g6 = g9; b6 = b9; s6 = s9;
                    }
                    //Punkte dazu rechnen
                    points += temppoints;
                    //Punkteanimation erstellen
                    anipoints = "+ " + temppoints;
                    //btnactive zurücksetzen
                    btnactiv = 0;
                    //Buttons zurücksetzen
                    ButtonsReset();
                    //Extra Button zurücksetzen
                    GRB.Margin = new Thickness(-200, 0, 0, 0);

                    //Bei Hard, Zahlen drehen ***************************************************************** Hard ***************************************
                    if ((TB1.Text == "" | TB2.Text == "" | TB3.Text == "" | TB4.Text == "" | TB5.Text == "" | TB6.Text == "" | TB7.Text == "" | TB8.Text == "" | TB9.Text == "") & difficulty == "hard")
                    {
                        int tl = stz.Length;
                        if (stz[(tl - 1)].ToString() == "1" | stz[(tl - 1)].ToString() == "3" | stz[(tl - 1)].ToString() == "5" | stz[(tl - 1)].ToString() == "7" | stz[(tl - 1)].ToString() == "9")
                        {
                            statusturn = "right";
                        }
                        else
                        {
                            statusturn = "left";
                        }
                    }
                    //*****************************************************************************************************************************************
                }
            }

            //Wenn alle Felder voll
            if (TB1.Text != "" & TB2.Text != "" & TB3.Text != "" & TB4.Text != "" & TB5.Text != "" & TB6.Text != "" & TB7.Text != "" & TB8.Text != "" & TB9.Text != "")
            {
                //Prüfen ob Free Version
                if (trial == "yes" & level >= 20)
                {
                    //Zeit auf null setzen
                    timems = 0;
                    //Nachricht ausgeben
                    MessageBox.Show("Free version ends at level 20.\nBuy the game for all levels and an extended trainings mode.");
                }
                //Bei Kaufversion
                else
                {
                    //Wenn kein Bonus Level gespielt wurde
                    if (bonus == 0)
                    {
                        //Punkte Prüfen ob Bonus Level kommt
                        string tempp = Convert.ToString(points);
                        int templ = Convert.ToInt32(tempp.Length);
                        //Wenn Bonuslevel kommt
                        if (templ >= 2)
                        {
                            if (Convert.ToString(tempp[(templ - 1)]) == Convert.ToString(tempp[(templ - 2)]))
                            {
                                bonus = level * 3;
                                LEVEL.Text = "Bonus " + level;
                                //Zeit erhöhen
                                timems += 15000;
                                //Animation Time starten
                                anitime = "+ 15";
                                //Nächste Runde Starten
                                statussite = "startgame";
                            }
                            //Wenn kein Bonus Level kommt
                            else
                            {
                                //Level erhöhen
                                level++;
                                LEVEL.Text = "Level " + level;
                                //Zeit erhöhen
                                timems += 15000;
                                //Animation Time starten
                                anitime = "+ 15";
                            }
                        }
                        //Wenn kein Bonus Level kommt
                        else
                        {
                            //Level erhöhen
                            level++;
                            LEVEL.Text = "Level " + level;
                            //Zeit erhöhen
                            timems += 15000;
                            //Animation Time starten
                            anitime = "+ 15";
                        }
                    }
                    //Wenn Bonus Level gespielt wurde
                    else
                    {
                        //Bonus Level zurücksetzen
                        bonus = 0;
                        //Level erhöhen
                        level++;
                        LEVEL.Text = "Level " + level;
                        //Zeit erhöhen
                        timems += 30000;
                        //Animation Time starten
                        anitime = "+ 30";
                    }
                }

                //Nächste Runde Starten
                statussite = "startgame";
            }
        }
        //---------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------------------------





        //Animationen
        //---------------------------------------------------------------------------------------------------------------------------------
        //Allgemeine Variabeln
        int uhrzeitms = 0;
        //Koordinaten
        int l1 = -15; int l2 = 140; int l3 = 295; int l4 = -15; int l5 = 140; int l6 = 295; int l7 = -15; int l8 = 140; int l9 = 295; 
        int u1 = -15; int u2 = -15; int u3 = -15; int u4 = 140; int u5 = 140; int u6 = 140; int u7 = 295; int u8 = 295; int u9 = 295;
        //Angaben der Felder
        int f1 = 1;
        //Status
        string statussite = "start";
        int statussitems = 0;
        //Variabeln für die Drehung
        string statusturn = "no";
        int statusturnms = 0;


        //Timer Ablauf
        void dt_Tick(object sender, EventArgs e)
        {

            //uhrzeitms neu erstellen
            //---------------------------------------------------------------
            DateTime Uhrzeit = DateTime.Now;
            //Aktuelle Uhrzeit Millisekunden erstellen
            uhrzeitms = (Uhrzeit.Hour * 3600000) + (Uhrzeit.Minute * 60000) + (Uhrzeit.Second * 1000) + Uhrzeit.Millisecond;
            //---------------------------------------------------------------


            //Zeit errechnen
            //---------------------------------------------------------------
            if(training == "no")
            {
                if (statussite == "play")
                {
                    //Zeit neu errechnen
                    int timenowms = timems - (uhrzeitms - timestartms);
                    if (timenowms > 0)
                    {
                        TIME.Text = "" + (timenowms / 1000);
                    }
                    else
                    {
                        statussite = "stop";
                        statussitems = 0;
                        TIME.Text = "0";
                    }
                }
            }
            else
            {
                TIME.Text = "Training";
            }
            //---------------------------------------------------------------


            //Punkte erstellen
            //---------------------------------------------------------------
            //Punkte
            if (points > tpoints)
            {
                //Temp Punkte erhöhen
                tpoints++;
                //Punkte ausgeben
                POINTS.Text = "" + tpoints;
            }
            if(training == "no")
            {
                //Highscore
                if (points > highscore & training == "no")
                {
                    //Temp Punkte erhöhen
                    highscore++;
                    //Punkte ausgeben
                    HIGHSCORE.Text = "Highscore " + highscore;
                }
            }
            //---------------------------------------------------------------


            //Animation Anfang (Startknopf anzeigen)
            //---------------------------------------------------------------
            if (statussite == "start")
            {
                //Wenn Animation noch nicht läuft
                if (statussitems == 0)
                {
                    //statussitems erstellen
                    statussitems = uhrzeitms;
                    //Quadrate erstellen
                    ChangeAllSquareColors(255, cmin, cmax);
                    //Button GRB Farbe erstellen
                    mySolidColorBrush5.Color = System.Windows.Media.Color.FromArgb((byte)(0), (byte)(r5), (byte)(g5), (byte)(b5));
                    RTB.Fill = mySolidColorBrush5;
                    //Button GRB Größe erstellen
                    RTB.Height = 190;
                    RTB.Width = 190;
                    //Button GRB erstellen
                    GRB.Height = GR5.Height;
                    GRB.Width = GR5.Width;
                    GRB.Margin = new Thickness(140, 140, 0, 0);
                    //Schrift für Button Start erstellen
                    TBB.Text = "START";
                }
                // Wenn Animation läuft
                else if (statussitems + 500 > uhrzeitms)
                {
                    //Prozent erstellen
                    int tp = (100000 / 500 * (uhrzeitms - statussitems) / 1000);
                    //Grid Weg erstellen
                    int way = 300 - (300 / 100 * tp);
                    //Grid Weg ablaufen
                    GR1.Margin = new Thickness((way - (way * 2) - 15), (way - (way * 2) - 15), 0, 0);
                    GR2.Margin = new Thickness(140, (way - (way * 2) - 15), 0, 0);
                    GR3.Margin = new Thickness(way + 295, (way - (way * 2) - 15), 0, 0);
                    GR4.Margin = new Thickness((way - (way * 2) - 15), 140, 0, 0);
                    GR6.Margin = new Thickness(way + 295, 140, 0, 0);
                    GR7.Margin = new Thickness((way - (way * 2) - 15), way + 295, 0, 0);
                    GR8.Margin = new Thickness(140, way + 295, 0, 0);
                    GR9.Margin = new Thickness(way + 295, way + 295, 0, 0);
                    //Temp größen erstellen
                    int ts1 = ((200 - s1) / 100 * tp) + s1;
                    RT1.Width = ts1; RT1.Height = ts1;
                    int ts2 = ((200 - s2) / 100 * tp) + s2;
                    RT2.Width = ts2; RT2.Height = ts2;
                    int ts3 = ((200 - s3) / 100 * tp) + s3;
                    RT3.Width = ts3; RT3.Height = ts3;
                    int ts4 = ((200 - s4) / 100 * tp) + s4;
                    RT4.Width = ts4; RT4.Height = ts4;
                    //int ts5 = ((300 - s5) / 100 * tp) + s5;
                    //RTB.Width = ts5; RTB.Height = ts5;
                    int ts6 = ((200 - s6) / 100 * tp) + s6;
                    RT6.Width = ts6; RT6.Height = ts6;
                    int ts7 = ((200 - s7) / 100 * tp) + s7;
                    RT7.Width = ts7; RT7.Height = ts7;
                    int ts8 = ((200 - s8) / 100 * tp) + s8;
                    RT8.Width = ts8; RT8.Height = ts8;
                    int ts9 = ((200 - s9) / 100 * tp) + s9;
                    RT9.Width = ts9; RT9.Height = ts9;
                    //Farben neu erstellen und Alpha neu errechnen
                    int a = 255 / 100 * tp;
                    mySolidColorBrush1.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r1), (byte)(g1), (byte)(b1));
                    RT1.Fill = mySolidColorBrush1;
                    mySolidColorBrush2.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r2), (byte)(g2), (byte)(b2));
                    RT2.Fill = mySolidColorBrush2;
                    mySolidColorBrush3.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r3), (byte)(g3), (byte)(b3));
                    RT3.Fill = mySolidColorBrush3;
                    mySolidColorBrush4.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r4), (byte)(g4), (byte)(b4));
                    RT4.Fill = mySolidColorBrush4;
                    mySolidColorBrush5.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r5), (byte)(g5), (byte)(b5));
                    RTB.Fill = mySolidColorBrush5;
                    mySolidColorBrush6.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r6), (byte)(g6), (byte)(b6));
                    RT6.Fill = mySolidColorBrush6;
                    mySolidColorBrush7.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r7), (byte)(g7), (byte)(b7));
                    RT7.Fill = mySolidColorBrush7;
                    mySolidColorBrush8.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r8), (byte)(g8), (byte)(b8));
                    RT8.Fill = mySolidColorBrush8;
                    mySolidColorBrush9.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r9), (byte)(g9), (byte)(b9));
                    RT9.Fill = mySolidColorBrush9;
                }
                //Felder völlig reinfahren
                else if (statussitems + 1000 > uhrzeitms)
                {
                    //Grids auf Startposition stellen
                    GR1.Margin = new Thickness(-15, -15, 0, 0);
                    GR2.Margin = new Thickness(140, -15, 0, 0);
                    GR3.Margin = new Thickness(295, -15, 0, 0);
                    GR4.Margin = new Thickness(-15, 140, 0, 0);
                    GR6.Margin = new Thickness(295, 140, 0, 0);
                    GR7.Margin = new Thickness(-15, 295, 0, 0);
                    GR8.Margin = new Thickness(140, 295, 0, 0);
                    GR9.Margin = new Thickness(295, 295, 0, 0);
                    //Quadrate auf Startgröße stellen
                    RT1.Width = s1; RT1.Height = s1;
                    RT2.Width = s2; RT2.Height = s2;
                    RT3.Width = s3; RT3.Height = s3;
                    RT4.Width = s4; RT4.Height = s4;
                    RTB.Width = 190; RTB.Height = 190;
                    RT6.Width = s6; RT6.Height = s6;
                    RT7.Width = s7; RT7.Height = s7;
                    RT8.Width = s8; RT8.Height = s8;
                    RT9.Width = s9; RT9.Height = s9;
                    //Farben neu erstellen und Alpha neu errechnen
                    int a = 255;
                    mySolidColorBrush1.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r1), (byte)(g1), (byte)(b1));
                    RT1.Fill = mySolidColorBrush1;
                    mySolidColorBrush2.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r2), (byte)(g2), (byte)(b2));
                    RT2.Fill = mySolidColorBrush2;
                    mySolidColorBrush3.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r3), (byte)(g3), (byte)(b3));
                    RT3.Fill = mySolidColorBrush3;
                    mySolidColorBrush4.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r4), (byte)(g4), (byte)(b4));
                    RT4.Fill = mySolidColorBrush4;
                    mySolidColorBrush5.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r5), (byte)(g5), (byte)(b5));
                    RTB.Fill = mySolidColorBrush5;
                    mySolidColorBrush6.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r6), (byte)(g6), (byte)(b6));
                    RT6.Fill = mySolidColorBrush6;
                    mySolidColorBrush7.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r7), (byte)(g7), (byte)(b7));
                    RT7.Fill = mySolidColorBrush7;
                    mySolidColorBrush8.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r8), (byte)(g8), (byte)(b8));
                    RT8.Fill = mySolidColorBrush8;
                    mySolidColorBrush9.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r9), (byte)(g9), (byte)(b9));
                    RT9.Fill = mySolidColorBrush9;
                }
                //Mittleres Feld Alpha
                else if (statussitems + 3500 > uhrzeitms)
                {
                    //Prozent erstellen
                    int tp = (1000000 / 2500 * (uhrzeitms - (statussitems + 1000)) / 10000);
                    int ta = (155000 / 100 * tp) / 1000;
                    int a = 255 - ta;
                    //Alpha erstellen
                    mySolidColorBrush5.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r5), (byte)(g5), (byte)(b5));
                    RTB.Fill = mySolidColorBrush5;
                }
                //Mittleres Feld Alpha
                else if (statussitems + 6000 > uhrzeitms)
                {
                    //Prozent erstellen
                    int tp = (1000000 / 2500 * (uhrzeitms - (statussitems + 3500)) / 10000);
                    int ta = (155000 / 100 * tp) / 1000;
                    int a = 100 + ta;
                    mySolidColorBrush5.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r5), (byte)(g5), (byte)(b5));
                    RTB.Fill = mySolidColorBrush5;
                }
                //Animation Schleife
                else
                {
                    //Drehung erstellen
                    Random turn = new Random();
                    int i = turn.Next(1, 3);
                    //Drehung einleiten
                    if (i == 1)
                    {
                        statusturn = "right";
                    }
                    else
                    {
                        statusturn = "left";
                    }
                    //statussitems umstellen
                    statussitems = statussitems + 5000;
                }

            }
            //---------------------------------------------------------------



            //Animation Spiel starten (Felder transparent machen)
            //---------------------------------------------------------------
            if (statussite == "getstart")
            {
                //Wenn Animation noch nicht läuft
                if (statussitems == 0)
                {
                    //Millisekunden erstellen
                    statussitems = uhrzeitms;
                    //Level zurücksetzen
                    level = startlevel;
                    LEVEL.Text = "Level " + startlevel;
                    POINTS.Text = "0";
                    points = 0;
                    tpoints = 0;
                }
                // Wenn Animation läuft
                else if (statussitems + 750 > uhrzeitms)
                {
                    //Prozent erstellen
                    int tp = (10000 / 750 * (uhrzeitms - statussitems) / 100);
                    int ta = (255000 / 100 * tp) / 1000;
                    int a = 255 - ta;
                    //Farben neu erstellen und Alpha neu errechnen
                    mySolidColorBrush1.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r1), (byte)(g1), (byte)(b1));
                    RT1.Fill = mySolidColorBrush1;
                    mySolidColorBrush2.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r2), (byte)(g2), (byte)(b2));
                    RT2.Fill = mySolidColorBrush2;
                    mySolidColorBrush3.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r3), (byte)(g3), (byte)(b3));
                    RT3.Fill = mySolidColorBrush3;
                    mySolidColorBrush4.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r4), (byte)(g4), (byte)(b4));
                    RT4.Fill = mySolidColorBrush4;
                    mySolidColorBrush5.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r5), (byte)(g5), (byte)(b5));
                    RTB.Fill = mySolidColorBrush5;
                    mySolidColorBrush6.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r6), (byte)(g6), (byte)(b6));
                    RT6.Fill = mySolidColorBrush6;
                    mySolidColorBrush7.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r7), (byte)(g7), (byte)(b7));
                    RT7.Fill = mySolidColorBrush7;
                    mySolidColorBrush8.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r8), (byte)(g8), (byte)(b8));
                    RT8.Fill = mySolidColorBrush8;
                    mySolidColorBrush9.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r9), (byte)(g9), (byte)(b9));
                    RT9.Fill = mySolidColorBrush9;
                }
                // Wenn Animation läuft
                else if (statussitems + 1250 > uhrzeitms)
                {
                    int a = 0;
                    //Farben neu erstellen und Alpha neu errechnen
                    mySolidColorBrush1.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r1), (byte)(g1), (byte)(b1));
                    RT1.Fill = mySolidColorBrush1;
                    mySolidColorBrush2.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r2), (byte)(g2), (byte)(b2));
                    RT2.Fill = mySolidColorBrush2;
                    mySolidColorBrush3.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r3), (byte)(g3), (byte)(b3));
                    RT3.Fill = mySolidColorBrush3;
                    mySolidColorBrush4.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r4), (byte)(g4), (byte)(b4));
                    RT4.Fill = mySolidColorBrush4;
                    mySolidColorBrush5.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r5), (byte)(g5), (byte)(b5));
                    RTB.Fill = mySolidColorBrush5;
                    mySolidColorBrush6.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r6), (byte)(g6), (byte)(b6));
                    RT6.Fill = mySolidColorBrush6;
                    mySolidColorBrush7.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r7), (byte)(g7), (byte)(b7));
                    RT7.Fill = mySolidColorBrush7;
                    mySolidColorBrush8.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r8), (byte)(g8), (byte)(b8));
                    RT8.Fill = mySolidColorBrush8;
                    mySolidColorBrush9.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r9), (byte)(g9), (byte)(b9));
                    RT9.Fill = mySolidColorBrush9;
                }
                // Countdown 3
                else if (statussitems + 1750 > uhrzeitms)
                {
                    TBB.Text = "";
                    COUNT.Text = "3";
                }
                // Countdown 2
                else if (statussitems + 2250 > uhrzeitms)
                {
                    TBB.Text = "";
                    COUNT.Text = "2";
                }
                // Countdown 1
                else if (statussitems + 2750 > uhrzeitms)
                {
                    TBB.Text = "";
                    COUNT.Text = "1";
                }
                //Level Starten
                else
                {
                    //Felder zurücksetzen
                    GRB.Margin = new Thickness(-200, 0, 0, 0);
                    TBB.Text = "";
                    COUNT.Text = "";
                    GR5.Margin = new Thickness(140, 140, 0, 0);
                    //Status ändern
                    statussite = "startgame";
                    //Zeit erstellen
                    timems = 30000;
                    timestartms = uhrzeitms;
                    //Statussitems zurückstellen
                    statussitems = 0;
                }
            }
            //---------------------------------------------------------------


            //Spiel stoppen
            //---------------------------------------------------------------
            if (statussite == "stop")
            {
                //Wenn Animation noch nicht läuft
                if (statussitems == 0)
                {
                    //Millisekunden erstellen
                    statussitems = uhrzeitms;
                    //Buttons Deaktivieren
                    ButtonsDeaktivate();
                }
                // Wenn Animation läuft
                else if (statussitems + 1000 > uhrzeitms)
                {
                    //Prozent erstellen
                    int tp = (10000 / 1000 * (uhrzeitms - statussitems) / 100);
                    int ta = (255000 / 100 * tp) / 1000;
                    int a = 255 - ta;
                    //Farben neu erstellen und Alpha neu errechnen
                    mySolidColorBrush1.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r1), (byte)(g1), (byte)(b1));
                    RT1.Fill = mySolidColorBrush1;
                    mySolidColorBrush2.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r2), (byte)(g2), (byte)(b2));
                    RT2.Fill = mySolidColorBrush2;
                    mySolidColorBrush3.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r3), (byte)(g3), (byte)(b3));
                    RT3.Fill = mySolidColorBrush3;
                    mySolidColorBrush4.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r4), (byte)(g4), (byte)(b4));
                    RT4.Fill = mySolidColorBrush4;
                    mySolidColorBrush5.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r5), (byte)(g5), (byte)(b5));
                    RT5.Fill = mySolidColorBrush5;
                    mySolidColorBrush6.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r6), (byte)(g6), (byte)(b6));
                    RT6.Fill = mySolidColorBrush6;
                    mySolidColorBrush7.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r7), (byte)(g7), (byte)(b7));
                    RT7.Fill = mySolidColorBrush7;
                    mySolidColorBrush8.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r8), (byte)(g8), (byte)(b8));
                    RT8.Fill = mySolidColorBrush8;
                    mySolidColorBrush9.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r9), (byte)(g9), (byte)(b9));
                    RT9.Fill = mySolidColorBrush9;
                }
                //Animation Pause
                else if (statussitems + 2000 > uhrzeitms)
                {
                    //Transparenz auf null stellen
                    int a = 0;
                    //Farben neu erstellen und Alpha neu errechnen
                    mySolidColorBrush1.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r1), (byte)(g1), (byte)(b1));
                    RT1.Fill = mySolidColorBrush1;
                    mySolidColorBrush2.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r2), (byte)(g2), (byte)(b2));
                    RT2.Fill = mySolidColorBrush2;
                    mySolidColorBrush3.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r3), (byte)(g3), (byte)(b3));
                    RT3.Fill = mySolidColorBrush3;
                    mySolidColorBrush4.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r4), (byte)(g4), (byte)(b4));
                    RT4.Fill = mySolidColorBrush4;
                    mySolidColorBrush5.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r5), (byte)(g5), (byte)(b5));
                    RT5.Fill = mySolidColorBrush5;
                    mySolidColorBrush6.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r6), (byte)(g6), (byte)(b6));
                    RT6.Fill = mySolidColorBrush6;
                    mySolidColorBrush7.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r7), (byte)(g7), (byte)(b7));
                    RT7.Fill = mySolidColorBrush7;
                    mySolidColorBrush8.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r8), (byte)(g8), (byte)(b8));
                    RT8.Fill = mySolidColorBrush8;
                    mySolidColorBrush9.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r9), (byte)(g9), (byte)(b9));
                    RT9.Fill = mySolidColorBrush9;
                    //Texte zurücksetzen
                    TB1.Text = "";
                    TB2.Text = "";
                    TB3.Text = "";
                    TB4.Text = "";
                    TB5.Text = "";
                    TB6.Text = "";
                    TB7.Text = "";
                    TB8.Text = "";
                    TB9.Text = "";
                    TBB.Text = "";
                }
                else
                {
                    //Button 5 nach aussen setzen
                    GR5.Margin = new Thickness(-200, 0, 0, 0);
                    //Highscores neu erstellen
                    if (training == "no")
                    {
                        int write = 0;
                        if (difficulty == "easy")
                        {
                            if (points > hse)
                            {
                                write = 1;
                                highscores = points + "A" + hsm + "A" + hsh;
                            }
                        }
                        if (difficulty == "medium")
                        {
                            if (points > hsm)
                            {
                                write = 1;
                                highscores = hse + "A" + points + "A" + hsh;
                            }
                        }
                        if (difficulty == "hard")
                        {
                            if (points > hsh)
                            {
                                write = 1;
                                highscores = hse + "A" + hsm + "A" + points;
                            }
                        }
                        if (write == 1)
                        {
                            //Highscores neu erstellen
                            IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication();
                            IsolatedStorageFileStream filestream = file.CreateFile("Highscores/Highscores.txt");
                            StreamWriter sw = new StreamWriter(filestream);
                            sw.WriteLine(highscores);
                            sw.Flush();
                            filestream.Close();
                            write = 0;
                        }
                    }
                    //Animation beenden
                    statussite = "start";
                    statussitems = 0;
                }
            }



            //Level erstellen (Rein Zoomen)
            //---------------------------------------------------------------
            if (statussite == "startgame")
            {
                //Wenn Animation noch nicht läuft
                if (statussitems == 0)
                {
                    //Millisekunden erstellen
                    statussitems = uhrzeitms;
                    //Farben erneuern
                    ChangeAllSquareColors(255, cmin, cmax);
                    //Level neu erstellen
                    CreateNewLevel();
                }
                // Wenn Animation läuft
                else if (statussitems + 250 > uhrzeitms)
                {
                    //Prozent erstellen
                    int tp = (10000 / 250 * (uhrzeitms - statussitems) / 100);
                    //Grid Weg erstellen
                    int way = 300 - (300 / 100 * tp);
                    //Grid Weg ablaufen
                    GR1.Margin = new Thickness((way - (way * 2) - 15), (way - (way * 2) - 15), 0, 0);
                    GR2.Margin = new Thickness(140, (way - (way * 2) - 15), 0, 0);
                    GR3.Margin = new Thickness(way + 295, (way - (way * 2) - 15), 0, 0);
                    GR4.Margin = new Thickness((way - (way * 2) - 15), 140, 0, 0);
                    GR6.Margin = new Thickness(way + 295, 140, 0, 0);
                    GR7.Margin = new Thickness((way - (way * 2) - 15), way + 295, 0, 0);
                    GR8.Margin = new Thickness(140, way + 295, 0, 0);
                    GR9.Margin = new Thickness(way + 295, way + 295, 0, 0);
                    //Temp größen erstellen
                    int ts1 = ((300 - s1) / 100 * tp) + s1;
                    RT1.Width = ts1; RT1.Height = ts1;
                    int ts2 = ((300 - s2) / 100 * tp) + s2;
                    RT2.Width = ts2; RT2.Height = ts2;
                    int ts3 = ((300 - s3) / 100 * tp) + s3;
                    RT3.Width = ts3; RT3.Height = ts3;
                    int ts4 = ((300 - s4) / 100 * tp) + s4;
                    RT4.Width = ts4; RT4.Height = ts4;
                    int ts5 = ((300 - s5) / 100 * tp) + s5;
                    RT5.Width = ts5; RT5.Height = ts5;
                    int ts6 = ((300 - s6) / 100 * tp) + s6;
                    RT6.Width = ts6; RT6.Height = ts6;
                    int ts7 = ((300 - s7) / 100 * tp) + s7;
                    RT7.Width = ts7; RT7.Height = ts7;
                    int ts8 = ((300 - s8) / 100 * tp) + s8;
                    RT8.Width = ts8; RT8.Height = ts8;
                    int ts9 = ((300 - s9) / 100 * tp) + s9;
                    RT9.Width = ts9; RT9.Height = ts9;
                    //Farben neu erstellen und Alpha neu errechnen
                    int a = 255 / 100 * tp;
                    mySolidColorBrush1.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r1), (byte)(g1), (byte)(b1));
                    RT1.Fill = mySolidColorBrush1;
                    mySolidColorBrush2.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r2), (byte)(g2), (byte)(b2));
                    RT2.Fill = mySolidColorBrush2;
                    mySolidColorBrush3.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r3), (byte)(g3), (byte)(b3));
                    RT3.Fill = mySolidColorBrush3;
                    mySolidColorBrush4.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r4), (byte)(g4), (byte)(b4));
                    RT4.Fill = mySolidColorBrush4;
                    mySolidColorBrush5.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r5), (byte)(g5), (byte)(b5));
                    RT5.Fill = mySolidColorBrush5;
                    mySolidColorBrush6.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r6), (byte)(g6), (byte)(b6));
                    RT6.Fill = mySolidColorBrush6;
                    mySolidColorBrush7.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r7), (byte)(g7), (byte)(b7));
                    RT7.Fill = mySolidColorBrush7;
                    mySolidColorBrush8.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r8), (byte)(g8), (byte)(b8));
                    RT8.Fill = mySolidColorBrush8;
                    mySolidColorBrush9.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r9), (byte)(g9), (byte)(b9));
                    RT9.Fill = mySolidColorBrush9;
                }
                // Animation beenden
                else
                {
                    //Grids auf Startposition stellen
                    GR1.Margin = new Thickness(-15, -15, 0, 0);
                    GR2.Margin = new Thickness(140, -15, 0, 0);
                    GR3.Margin = new Thickness(295, -15, 0, 0);
                    GR4.Margin = new Thickness(-15, 140, 0, 0);
                    GR6.Margin = new Thickness(295, 140, 0, 0);
                    GR7.Margin = new Thickness(-15, 295, 0, 0);
                    GR8.Margin = new Thickness(140, 295, 0, 0);
                    GR9.Margin = new Thickness(295, 295, 0, 0);
                    //Quadrate auf Startgröße stellen
                    RT1.Width = s1; RT1.Height = s1;
                    RT2.Width = s2; RT2.Height = s2;
                    RT3.Width = s3; RT3.Height = s3;
                    RT4.Width = s4; RT4.Height = s4;
                    RT5.Width = s5; RT5.Height = s5;
                    RT6.Width = s6; RT6.Height = s6;
                    RT7.Width = s7; RT7.Height = s7;
                    RT8.Width = s8; RT8.Height = s8;
                    RT9.Width = s9; RT9.Height = s9;
                    //Farben neu erstellen und Alpha neu errechnen
                    int a = 255;
                    mySolidColorBrush1.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r1), (byte)(g1), (byte)(b1));
                    RT1.Fill = mySolidColorBrush1;
                    mySolidColorBrush2.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r2), (byte)(g2), (byte)(b2));
                    RT2.Fill = mySolidColorBrush2;
                    mySolidColorBrush3.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r3), (byte)(g3), (byte)(b3));
                    RT3.Fill = mySolidColorBrush3;
                    mySolidColorBrush4.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r4), (byte)(g4), (byte)(b4));
                    RT4.Fill = mySolidColorBrush4;
                    mySolidColorBrush5.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r5), (byte)(g5), (byte)(b5));
                    RT5.Fill = mySolidColorBrush5;
                    mySolidColorBrush6.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r6), (byte)(g6), (byte)(b6));
                    RT6.Fill = mySolidColorBrush6;
                    mySolidColorBrush7.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r7), (byte)(g7), (byte)(b7));
                    RT7.Fill = mySolidColorBrush7;
                    mySolidColorBrush8.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r8), (byte)(g8), (byte)(b8));
                    RT8.Fill = mySolidColorBrush8;
                    mySolidColorBrush9.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r9), (byte)(g9), (byte)(b9));
                    RT9.Fill = mySolidColorBrush9;
                    //statusms zurücksetzen
                    statussitems = 0;
                    //Felder zurücksetzen
                    f1 = 1;
                    //Status umstellen
                    statussite = "play";
                }
            }
            //---------------------------------------------------------------



            //Animation Drehung rechts
            //---------------------------------------------------------------
            if (statusturn == "right")
            {
                //Wenn Animation noch nicht läuft
                if (statusturnms == 0)
                {
                    //Millisekunden erstellen
                    statusturnms = uhrzeitms;
                }
                //0 - 50% der Animation
                else if (statusturnms + 150 > uhrzeitms)
                {
                    //Prozent erstellen
                    int tp = (10000 / 150 * (uhrzeitms - statusturnms) / 100);
                    int tw = (15500 / 100 * tp) / 100;
                    //Größe erstellen
                    int tpgr1 = (((s1 * 100) / 100 * 160) / 100) - s1;
                    RT1.Width = s1 + (((tpgr1 * 100) / 100 * tp) / 100);
                    RT1.Height = s1 + (((tpgr1 * 100) / 100 * tp) / 100);
                    int tpgr3 = (((s3 * 100) / 100 * 160) / 100) - s3;
                    RT3.Width = s3 + (((tpgr3 * 100) / 100 * tp) / 100);
                    RT3.Height = s3 + (((tpgr3 * 100) / 100 * tp) / 100);
                    int tpgr7 = (((s7 * 100) / 100 * 160) / 100) - s7;
                    RT7.Width = s7 + (((tpgr7 * 100) / 100 * tp) / 100);
                    RT7.Height = s7 + (((tpgr7 * 100) / 100 * tp) / 100);
                    int tpgr9 = (((s9 * 100) / 100 * 160) / 100) - s9;
                    RT9.Width = s9 + (((tpgr9 * 100) / 100 * tp) / 100);
                    RT9.Height = s9 + (((tpgr9 * 100) / 100 * tp) / 100);
                    //Alpha neu erstellen
                    int a = 255 - (tp * 2);
                    mySolidColorBrush1.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r1), (byte)(g1), (byte)(b1));
                    RT1.Fill = mySolidColorBrush1;
                    mySolidColorBrush2.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r2), (byte)(g2), (byte)(b2));
                    RT2.Fill = mySolidColorBrush2;
                    mySolidColorBrush3.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r3), (byte)(g3), (byte)(b3));
                    RT3.Fill = mySolidColorBrush3;
                    mySolidColorBrush4.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r4), (byte)(g4), (byte)(b4));
                    RT4.Fill = mySolidColorBrush4;
                    mySolidColorBrush5.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r5), (byte)(g5), (byte)(b5));
                    RT5.Fill = mySolidColorBrush5;
                    mySolidColorBrush6.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r6), (byte)(g6), (byte)(b6));
                    RT6.Fill = mySolidColorBrush6;
                    mySolidColorBrush7.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r7), (byte)(g7), (byte)(b7));
                    RT7.Fill = mySolidColorBrush7;
                    mySolidColorBrush8.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r8), (byte)(g8), (byte)(b8));
                    RT8.Fill = mySolidColorBrush8;
                    mySolidColorBrush9.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r9), (byte)(g9), (byte)(b9));
                    RT9.Fill = mySolidColorBrush9;
                    //Wenn Feld 1 auf Feld 1
                    if (f1 == 1)
                    {
                        GR1.Margin = new Thickness((-15 + tw), -15, 0, 0);
                        GR2.Margin = new Thickness((140 + tw), -15, 0, 0);
                        GR3.Margin = new Thickness(295, (-15 + tw), 0, 0);
                        GR6.Margin = new Thickness(295, (140 + tw), 0, 0);
                        GR9.Margin = new Thickness((295 - tw), 295, 0, 0);
                        GR8.Margin = new Thickness((140 - tw), 295, 0, 0);
                        GR7.Margin = new Thickness(-15, (295 - tw), 0, 0);
                        GR4.Margin = new Thickness(-15, (140 - tw), 0, 0);
                    }
                    //Wenn Feld 1 auf Feld 3
                    if (f1 == 3)
                    {
                        GR7.Margin = new Thickness((-15 + tw), -15, 0, 0);
                        GR4.Margin = new Thickness((140 + tw), -15, 0, 0);
                        GR1.Margin = new Thickness(295, (-15 + tw), 0, 0);
                        GR2.Margin = new Thickness(295, (140 + tw), 0, 0);
                        GR3.Margin = new Thickness((295 - tw), 295, 0, 0);
                        GR6.Margin = new Thickness((140 - tw), 295, 0, 0);
                        GR9.Margin = new Thickness(-15, (295 - tw), 0, 0);
                        GR8.Margin = new Thickness(-15, (140 - tw), 0, 0);
                    }
                    //Wenn Feld 1 auf Feld 9
                    if (f1 == 9)
                    {
                        GR9.Margin = new Thickness((-15 + tw), -15, 0, 0);
                        GR8.Margin = new Thickness((140 + tw), -15, 0, 0);
                        GR7.Margin = new Thickness(295, (-15 + tw), 0, 0);
                        GR4.Margin = new Thickness(295, (140 + tw), 0, 0);
                        GR1.Margin = new Thickness((295 - tw), 295, 0, 0);
                        GR2.Margin = new Thickness((140 - tw), 295, 0, 0);
                        GR3.Margin = new Thickness(-15, (295 - tw), 0, 0);
                        GR6.Margin = new Thickness(-15, (140 - tw), 0, 0);
                    }
                    //Wenn Feld 1 auf Feld 7
                    else if (f1 == 7)
                    {
                        GR3.Margin = new Thickness((-15 + tw), -15, 0, 0);
                        GR6.Margin = new Thickness((140 + tw), -15, 0, 0);
                        GR9.Margin = new Thickness(295, (-15 + tw), 0, 0);
                        GR8.Margin = new Thickness(295, (140 + tw), 0, 0);
                        GR7.Margin = new Thickness((295 - tw), 295, 0, 0);
                        GR4.Margin = new Thickness((140 - tw), 295, 0, 0);
                        GR1.Margin = new Thickness(-15, (295 - tw), 0, 0);
                        GR2.Margin = new Thickness(-15, (140 - tw), 0, 0);
                    }

                }
                //50 - 100% der Animation
                else if (statusturnms + 300 > uhrzeitms)
                {
                    //Prozent erstellen
                    int tp = (10000 / 150 * (uhrzeitms - (statusturnms + 150)) / 100);
                    int tw = (15500 / 100 * tp) / 100;
                    //Größe erstellen
                    int tpgr1 = (((s1 * 100) / 100 * 160) / 100) - s1;
                    RT1.Width = s1 + (tpgr1 - (((tpgr1 * 100) / 100 * tp) / 100));
                    RT1.Height = s1 + (tpgr1 - (((tpgr1 * 100) / 100 * tp) / 100));
                    int tpgr3 = (((s3 * 100) / 100 * 160) / 100) - s3;
                    RT3.Width = s3 + (tpgr3 - (((tpgr3 * 100) / 100 * tp) / 100));
                    RT3.Height = s3 + (tpgr3 - (((tpgr3 * 100) / 100 * tp) / 100));
                    int tpgr7 = (((s7 * 100) / 100 * 160) / 100) - s7;
                    RT7.Width = s7 + (tpgr7 - (((tpgr7 * 100) / 100 * tp) / 100));
                    RT7.Height = s7 + (tpgr7 - (((tpgr7 * 100) / 100 * tp) / 100));
                    int tpgr9 = (((s9 * 100) / 100 * 160) / 100) - s9;
                    RT9.Width = s9 + (tpgr9 - (((tpgr9 * 100) / 100 * tp) / 100));
                    RT9.Height = s9 + (tpgr9 - (((tpgr9 * 100) / 100 * tp) / 100));
                    //Alpha neu erstellen
                    int a = 55 + (tp * 2);
                    mySolidColorBrush1.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r1), (byte)(g1), (byte)(b1));
                    RT1.Fill = mySolidColorBrush1;
                    mySolidColorBrush2.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r2), (byte)(g2), (byte)(b2));
                    RT2.Fill = mySolidColorBrush2;
                    mySolidColorBrush3.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r3), (byte)(g3), (byte)(b3));
                    RT3.Fill = mySolidColorBrush3;
                    mySolidColorBrush4.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r4), (byte)(g4), (byte)(b4));
                    RT4.Fill = mySolidColorBrush4;
                    mySolidColorBrush5.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r5), (byte)(g5), (byte)(b5));
                    RT5.Fill = mySolidColorBrush5;
                    mySolidColorBrush6.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r6), (byte)(g6), (byte)(b6));
                    RT6.Fill = mySolidColorBrush6;
                    mySolidColorBrush7.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r7), (byte)(g7), (byte)(b7));
                    RT7.Fill = mySolidColorBrush7;
                    mySolidColorBrush8.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r8), (byte)(g8), (byte)(b8));
                    RT8.Fill = mySolidColorBrush8;
                    mySolidColorBrush9.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r9), (byte)(g9), (byte)(b9));
                    RT9.Fill = mySolidColorBrush9;
                    //Wenn Feld 1 auf Feld 1
                    if (f1 == 1)
                    {
                        GR1.Margin = new Thickness((140 + tw), -15, 0, 0);
                        GR2.Margin = new Thickness(295, (-15 + tw), 0, 0);
                        GR3.Margin = new Thickness(295, (140 + tw), 0, 0);
                        GR6.Margin = new Thickness((295 - tw), 295, 0, 0);
                        GR9.Margin = new Thickness((140 - tw), 295, 0, 0);
                        GR8.Margin = new Thickness(-15, (295 - tw), 0, 0);
                        GR7.Margin = new Thickness(-15, (140 - tw), 0, 0);
                        GR4.Margin = new Thickness((-15 + tw), -15, 0, 0);
                    }
                    //Wenn Feld 1 auf Feld 3
                    else if (f1 == 3)
                    {
                        GR7.Margin = new Thickness((140 + tw), -15, 0, 0);
                        GR4.Margin = new Thickness(295, (-15 + tw), 0, 0);
                        GR1.Margin = new Thickness(295, (140 + tw), 0, 0);
                        GR2.Margin = new Thickness((295 - tw), 295, 0, 0);
                        GR3.Margin = new Thickness((140 - tw), 295, 0, 0);
                        GR6.Margin = new Thickness(-15, (295 - tw), 0, 0);
                        GR9.Margin = new Thickness(-15, (140 - tw), 0, 0);
                        GR8.Margin = new Thickness((-15 + tw), -15, 0, 0);
                    }
                    //Wenn Feld 1 auf Feld 9
                    else if (f1 == 9)
                    {
                        GR9.Margin = new Thickness((140 + tw), -15, 0, 0);
                        GR8.Margin = new Thickness(295, (-15 + tw), 0, 0);
                        GR7.Margin = new Thickness(295, (140 + tw), 0, 0);
                        GR4.Margin = new Thickness((295 - tw), 295, 0, 0);
                        GR1.Margin = new Thickness((140 - tw), 295, 0, 0);
                        GR2.Margin = new Thickness(-15, (295 - tw), 0, 0);
                        GR3.Margin = new Thickness(-15, (140 - tw), 0, 0);
                        GR6.Margin = new Thickness((-15 + tw), -15, 0, 0);
                    }
                    //Wenn Feld 1 auf Feld 7
                    else if (f1 == 7)
                    {
                        GR3.Margin = new Thickness((140 + tw), -15, 0, 0);
                        GR6.Margin = new Thickness(295, (-15 + tw), 0, 0);
                        GR9.Margin = new Thickness(295, (140 + tw), 0, 0);
                        GR8.Margin = new Thickness((295 - tw), 295, 0, 0);
                        GR7.Margin = new Thickness((140 - tw), 295, 0, 0);
                        GR4.Margin = new Thickness(-15, (295 - tw), 0, 0);
                        GR1.Margin = new Thickness(-15, (140 - tw), 0, 0);
                        GR2.Margin = new Thickness((-15 + tw), -15, 0, 0);
                    }
                }
                //Animation Beenden
                else
                {
                    //Größe zurücksetzten
                    RT1.Width = s1;
                    RT1.Height = s1;
                    RT3.Width = s3;
                    RT3.Height = s3;
                    RT7.Width = s7;
                    RT7.Height = s7;
                    RT9.Width = s9;
                    RT9.Height = s9;
                    //Alpha zurücksetzen
                    int a = 255;
                    mySolidColorBrush1.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r1), (byte)(g1), (byte)(b1));
                    RT1.Fill = mySolidColorBrush1;
                    mySolidColorBrush2.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r2), (byte)(g2), (byte)(b2));
                    RT2.Fill = mySolidColorBrush2;
                    mySolidColorBrush3.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r3), (byte)(g3), (byte)(b3));
                    RT3.Fill = mySolidColorBrush3;
                    mySolidColorBrush4.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r4), (byte)(g4), (byte)(b4));
                    RT4.Fill = mySolidColorBrush4;
                    mySolidColorBrush5.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r5), (byte)(g5), (byte)(b5));
                    RT5.Fill = mySolidColorBrush5;
                    mySolidColorBrush6.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r6), (byte)(g6), (byte)(b6));
                    RT6.Fill = mySolidColorBrush6;
                    mySolidColorBrush7.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r7), (byte)(g7), (byte)(b7));
                    RT7.Fill = mySolidColorBrush7;
                    mySolidColorBrush8.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r8), (byte)(g8), (byte)(b8));
                    RT8.Fill = mySolidColorBrush8;
                    mySolidColorBrush9.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r9), (byte)(g9), (byte)(b9));
                    RT9.Fill = mySolidColorBrush9;
                    //Wenn Feld 1 auf Feld 1
                    if (f1 == 1)
                    {
                        GR1.Margin = new Thickness(l3, u3, 0, 0);
                        GR2.Margin = new Thickness(l6, u6, 0, 0);
                        GR3.Margin = new Thickness(l9, u9, 0, 0);
                        GR6.Margin = new Thickness(l8, u8, 0, 0);
                        GR9.Margin = new Thickness(l7, u7, 0, 0);
                        GR8.Margin = new Thickness(l4, u4, 0, 0);
                        GR7.Margin = new Thickness(l1, u1, 0, 0);
                        GR4.Margin = new Thickness(l2, u2, 0, 0);
                        f1 = 3;
                    }
                    //Wenn Feld 1 auf Feld 3
                    else if (f1 == 3)
                    {
                        GR7.Margin = new Thickness(l3, u3, 0, 0);
                        GR4.Margin = new Thickness(l6, u6, 0, 0);
                        GR1.Margin = new Thickness(l9, u9, 0, 0);
                        GR2.Margin = new Thickness(l8, u8, 0, 0);
                        GR3.Margin = new Thickness(l7, u7, 0, 0);
                        GR6.Margin = new Thickness(l4, u4, 0, 0);
                        GR9.Margin = new Thickness(l1, u1, 0, 0);
                        GR8.Margin = new Thickness(l2, u2, 0, 0);
                        f1 = 9;
                    }
                    //Wenn Feld 1 auf Feld 1
                    else if (f1 == 9)
                    {
                        GR9.Margin = new Thickness(l3, u3, 0, 0);
                        GR8.Margin = new Thickness(l6, u6, 0, 0);
                        GR7.Margin = new Thickness(l9, u9, 0, 0);
                        GR4.Margin = new Thickness(l8, u8, 0, 0);
                        GR1.Margin = new Thickness(l7, u7, 0, 0);
                        GR2.Margin = new Thickness(l4, u4, 0, 0);
                        GR3.Margin = new Thickness(l1, u1, 0, 0);
                        GR6.Margin = new Thickness(l2, u2, 0, 0);
                        f1 = 7;
                    }
                    //Wenn Feld 1 auf Feld 7
                    else if (f1 == 7)
                    {
                        GR3.Margin = new Thickness(l3, u3, 0, 0);
                        GR6.Margin = new Thickness(l6, u6, 0, 0);
                        GR9.Margin = new Thickness(l9, u9, 0, 0);
                        GR8.Margin = new Thickness(l8, u8, 0, 0);
                        GR7.Margin = new Thickness(l7, u7, 0, 0);
                        GR4.Margin = new Thickness(l4, u4, 0, 0);
                        GR1.Margin = new Thickness(l1, u1, 0, 0);
                        GR2.Margin = new Thickness(l2, u2, 0, 0);
                        f1 = 1;
                    }
                    //Status umstellen
                    statusturnms = 0;
                    statusturn = "no";
                }
            }
            //---------------------------------------------------------------


            //Animation Drehung links
            //---------------------------------------------------------------
            if (statusturn == "left")
            {
                //Wenn Animation noch nicht läuft
                if (statusturnms == 0)
                {
                    //Millisekunden erstellen
                    statusturnms = uhrzeitms;
                }
                //0 - 50% der Animation
                else if (statusturnms + 150 > uhrzeitms)
                {
                    //Prozent erstellen
                    int tp = (10000 / 150 * (uhrzeitms - statusturnms) / 100);
                    int tw = (15500 / 100 * tp) / 100;
                    //Größe erstellen
                    int tpgr1 = (((s1 * 100) / 100 * 160) / 100) - s1;
                    RT1.Width = s1 + (((tpgr1 * 100) / 100 * tp) / 100);
                    RT1.Height = s1 + (((tpgr1 * 100) / 100 * tp) / 100);
                    int tpgr3 = (((s3 * 100) / 100 * 160) / 100) - s3;
                    RT3.Width = s3 + (((tpgr3 * 100) / 100 * tp) / 100);
                    RT3.Height = s3 + (((tpgr3 * 100) / 100 * tp) / 100);
                    int tpgr7 = (((s7 * 100) / 100 * 160) / 100) - s7;
                    RT7.Width = s7 + (((tpgr7 * 100) / 100 * tp) / 100);
                    RT7.Height = s7 + (((tpgr7 * 100) / 100 * tp) / 100);
                    int tpgr9 = (((s9 * 100) / 100 * 160) / 100) - s9;
                    RT9.Width = s9 + (((tpgr9 * 100) / 100 * tp) / 100);
                    RT9.Height = s9 + (((tpgr9 * 100) / 100 * tp) / 100);
                    //Alpha neu erstellen
                    int a = 255 - (tp * 2);
                    mySolidColorBrush1.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r1), (byte)(g1), (byte)(b1));
                    RT1.Fill = mySolidColorBrush1;
                    mySolidColorBrush2.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r2), (byte)(g2), (byte)(b2));
                    RT2.Fill = mySolidColorBrush2;
                    mySolidColorBrush3.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r3), (byte)(g3), (byte)(b3));
                    RT3.Fill = mySolidColorBrush3;
                    mySolidColorBrush4.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r4), (byte)(g4), (byte)(b4));
                    RT4.Fill = mySolidColorBrush4;
                    mySolidColorBrush5.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r5), (byte)(g5), (byte)(b5));
                    RT5.Fill = mySolidColorBrush5;
                    mySolidColorBrush6.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r6), (byte)(g6), (byte)(b6));
                    RT6.Fill = mySolidColorBrush6;
                    mySolidColorBrush7.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r7), (byte)(g7), (byte)(b7));
                    RT7.Fill = mySolidColorBrush7;
                    mySolidColorBrush8.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r8), (byte)(g8), (byte)(b8));
                    RT8.Fill = mySolidColorBrush8;
                    mySolidColorBrush9.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r9), (byte)(g9), (byte)(b9));
                    RT9.Fill = mySolidColorBrush9;
                    //Wenn Feld 1 auf Feld 1
                    if (f1 == 1)
                    {
                        GR1.Margin = new Thickness(-15, (-15 + tw), 0, 0);
                        GR2.Margin = new Thickness((140 - tw), -15, 0, 0);
                        GR3.Margin = new Thickness((295 - tw), -15, 0, 0);
                        GR6.Margin = new Thickness(295, (140 - tw), 0, 0);
                        GR9.Margin = new Thickness(295, (295 - tw), 0, 0);
                        GR8.Margin = new Thickness((140 + tw), 295, 0, 0);
                        GR7.Margin = new Thickness((-15 + tw), 295, 0, 0);
                        GR4.Margin = new Thickness(-15, (140 + tw), 0, 0);
                    }
                    //Wenn Feld 1 auf Feld 3
                    else if (f1 == 3)
                    {
                        GR7.Margin = new Thickness(-15, (-15 + tw), 0, 0);
                        GR4.Margin = new Thickness((140 - tw), -15, 0, 0);
                        GR1.Margin = new Thickness((295 - tw), -15, 0, 0);
                        GR2.Margin = new Thickness(295, (140 - tw), 0, 0);
                        GR3.Margin = new Thickness(295, (295 - tw), 0, 0);
                        GR6.Margin = new Thickness((140 + tw), 295, 0, 0);
                        GR9.Margin = new Thickness((-15 + tw), 295, 0, 0);
                        GR8.Margin = new Thickness(-15, (140 + tw), 0, 0);
                    }
                    //Wenn Feld 1 auf Feld 9
                    else if (f1 == 9)
                    {
                        GR9.Margin = new Thickness(-15, (-15 + tw), 0, 0);
                        GR8.Margin = new Thickness((140 - tw), -15, 0, 0);
                        GR7.Margin = new Thickness((295 - tw), -15, 0, 0);
                        GR4.Margin = new Thickness(295, (140 - tw), 0, 0);
                        GR1.Margin = new Thickness(295, (295 - tw), 0, 0);
                        GR2.Margin = new Thickness((140 + tw), 295, 0, 0);
                        GR3.Margin = new Thickness((-15 + tw), 295, 0, 0);
                        GR6.Margin = new Thickness(-15, (140 + tw), 0, 0);
                    }
                    //Wenn Feld 1 auf Feld 7
                    else if (f1 == 7)
                    {
                        GR3.Margin = new Thickness(-15, (-15 + tw), 0, 0);
                        GR6.Margin = new Thickness((140 - tw), -15, 0, 0);
                        GR9.Margin = new Thickness((295 - tw), -15, 0, 0);
                        GR8.Margin = new Thickness(295, (140 - tw), 0, 0);
                        GR7.Margin = new Thickness(295, (295 - tw), 0, 0);
                        GR4.Margin = new Thickness((140 + tw), 295, 0, 0);
                        GR1.Margin = new Thickness((-15 + tw), 295, 0, 0);
                        GR2.Margin = new Thickness(-15, (140 + tw), 0, 0);
                    }
                }
                //50 - 100% der Animation
                else if (statusturnms + 300 > uhrzeitms)
                {
                    //Prozent erstellen
                    int tp = (10000 / 150 * (uhrzeitms - (statusturnms + 150)) / 100);
                    int tw = (15500 / 100 * tp) / 100;
                    //Alpha neu erstellen
                    int a = 55 + (tp * 2);
                    mySolidColorBrush1.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r1), (byte)(g1), (byte)(b1));
                    RT1.Fill = mySolidColorBrush1;
                    mySolidColorBrush2.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r2), (byte)(g2), (byte)(b2));
                    RT2.Fill = mySolidColorBrush2;
                    mySolidColorBrush3.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r3), (byte)(g3), (byte)(b3));
                    RT3.Fill = mySolidColorBrush3;
                    mySolidColorBrush4.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r4), (byte)(g4), (byte)(b4));
                    RT4.Fill = mySolidColorBrush4;
                    mySolidColorBrush5.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r5), (byte)(g5), (byte)(b5));
                    RT5.Fill = mySolidColorBrush5;
                    mySolidColorBrush6.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r6), (byte)(g6), (byte)(b6));
                    RT6.Fill = mySolidColorBrush6;
                    mySolidColorBrush7.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r7), (byte)(g7), (byte)(b7));
                    RT7.Fill = mySolidColorBrush7;
                    mySolidColorBrush8.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r8), (byte)(g8), (byte)(b8));
                    RT8.Fill = mySolidColorBrush8;
                    mySolidColorBrush9.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r9), (byte)(g9), (byte)(b9));
                    RT9.Fill = mySolidColorBrush9;
                    //Größe erstellen
                    int tpgr1 = (((s1 * 100) / 100 * 160) / 100) - s1;
                    RT1.Width = s1 + (tpgr1 - (((tpgr1 * 100) / 100 * tp) / 100));
                    RT1.Height = s1 + (tpgr1 - (((tpgr1 * 100) / 100 * tp) / 100));
                    int tpgr3 = (((s3 * 100) / 100 * 160) / 100) - s3;
                    RT3.Width = s3 + (tpgr3 - (((tpgr3 * 100) / 100 * tp) / 100));
                    RT3.Height = s3 + (tpgr3 - (((tpgr3 * 100) / 100 * tp) / 100));
                    int tpgr7 = (((s7 * 100) / 100 * 160) / 100) - s7;
                    RT7.Width = s7 + (tpgr7 - (((tpgr7 * 100) / 100 * tp) / 100));
                    RT7.Height = s7 + (tpgr7 - (((tpgr7 * 100) / 100 * tp) / 100));
                    int tpgr9 = (((s9 * 100) / 100 * 160) / 100) - s9;
                    RT9.Width = s9 + (tpgr9 - (((tpgr9 * 100) / 100 * tp) / 100));
                    RT9.Height = s9 + (tpgr9 - (((tpgr9 * 100) / 100 * tp) / 100));
                    //Wenn Feld 1 auf Feld 1
                    if (f1 == 1)
                    {
                        GR1.Margin = new Thickness(-15, (140 + tw), 0, 0);
                        GR2.Margin = new Thickness(-15, (-15 + tw), 0, 0);
                        GR3.Margin = new Thickness((140 - tw), -15, 0, 0);
                        GR6.Margin = new Thickness((295 - tw), -15, 0, 0);
                        GR9.Margin = new Thickness(295, (140 -tw), 0, 0);
                        GR8.Margin = new Thickness(295, (295 - tw), 0, 0);
                        GR7.Margin = new Thickness((140 + tw), 295, 0, 0);
                        GR4.Margin = new Thickness((-15 + tw), 295, 0, 0);
                    }
                    //Wenn Feld 1 auf Feld 3
                    else if (f1 == 3)
                    {
                        GR7.Margin = new Thickness(-15, (140 + tw), 0, 0);
                        GR4.Margin = new Thickness(-15, (-15 + tw), 0, 0);
                        GR1.Margin = new Thickness((140 - tw), -15, 0, 0);
                        GR2.Margin = new Thickness((295 - tw), -15, 0, 0);
                        GR3.Margin = new Thickness(295, (140 - tw), 0, 0);
                        GR6.Margin = new Thickness(295, (295 - tw), 0, 0);
                        GR9.Margin = new Thickness((140 + tw), 295, 0, 0);
                        GR8.Margin = new Thickness((-15 + tw), 295, 0, 0);
                    }
                    //Wenn Feld 1 auf Feld 9
                    else if (f1 == 9)
                    {
                        GR9.Margin = new Thickness(-15, (140 + tw), 0, 0);
                        GR8.Margin = new Thickness(-15, (-15 + tw), 0, 0);
                        GR7.Margin = new Thickness((140 - tw), -15, 0, 0);
                        GR4.Margin = new Thickness((295 - tw), -15, 0, 0);
                        GR1.Margin = new Thickness(295, (140 - tw), 0, 0);
                        GR2.Margin = new Thickness(295, (295 - tw), 0, 0);
                        GR3.Margin = new Thickness((140 + tw), 295, 0, 0);
                        GR6.Margin = new Thickness((-15 + tw), 295, 0, 0);
                    }
                    //Wenn Feld 1 auf Feld 7
                    else if (f1 == 7)
                    {
                        GR3.Margin = new Thickness(-15, (140 + tw), 0, 0);
                        GR6.Margin = new Thickness(-15, (-15 + tw), 0, 0);
                        GR9.Margin = new Thickness((140 - tw), -15, 0, 0);
                        GR8.Margin = new Thickness((295 - tw), -15, 0, 0);
                        GR7.Margin = new Thickness(295, (140 - tw), 0, 0);
                        GR4.Margin = new Thickness(295, (295 - tw), 0, 0);
                        GR1.Margin = new Thickness((140 + tw), 295, 0, 0);
                        GR2.Margin = new Thickness((-15 + tw), 295, 0, 0);
                    }
                }
                //Animation Beenden
                else
                {
                    //Größe zurücksetzten
                    RT1.Width = s1;
                    RT1.Height = s1;
                    RT3.Width = s3;
                    RT3.Height = s3;
                    RT7.Width = s7;
                    RT7.Height = s7;
                    RT9.Width = s9;
                    RT9.Height = s9;
                    //Alpha zurücksetzten
                    int a = 255;
                    mySolidColorBrush1.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r1), (byte)(g1), (byte)(b1));
                    RT1.Fill = mySolidColorBrush1;
                    mySolidColorBrush2.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r2), (byte)(g2), (byte)(b2));
                    RT2.Fill = mySolidColorBrush2;
                    mySolidColorBrush3.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r3), (byte)(g3), (byte)(b3));
                    RT3.Fill = mySolidColorBrush3;
                    mySolidColorBrush4.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r4), (byte)(g4), (byte)(b4));
                    RT4.Fill = mySolidColorBrush4;
                    mySolidColorBrush5.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r5), (byte)(g5), (byte)(b5));
                    RT5.Fill = mySolidColorBrush5;
                    mySolidColorBrush6.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r6), (byte)(g6), (byte)(b6));
                    RT6.Fill = mySolidColorBrush6;
                    mySolidColorBrush7.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r7), (byte)(g7), (byte)(b7));
                    RT7.Fill = mySolidColorBrush7;
                    mySolidColorBrush8.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r8), (byte)(g8), (byte)(b8));
                    RT8.Fill = mySolidColorBrush8;
                    mySolidColorBrush9.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r9), (byte)(g9), (byte)(b9));
                    RT9.Fill = mySolidColorBrush9;
                    //Wenn Feld 1 auf Feld 1
                    if (f1 == 1)
                    {
                        GR9.Margin = new Thickness(l3, u3, 0, 0);
                        GR8.Margin = new Thickness(l6, u6, 0, 0);
                        GR7.Margin = new Thickness(l9, u9, 0, 0);
                        GR4.Margin = new Thickness(l8, u8, 0, 0);
                        GR1.Margin = new Thickness(l7, u7, 0, 0);
                        GR2.Margin = new Thickness(l4, u4, 0, 0);
                        GR3.Margin = new Thickness(l1, u1, 0, 0);
                        GR6.Margin = new Thickness(l2, u2, 0, 0);
                        f1 = 7;
                    }
                    //Wenn Feld 1 auf Feld 3
                    else if (f1 == 3)
                    {
                        GR3.Margin = new Thickness(l3, u3, 0, 0);
                        GR6.Margin = new Thickness(l6, u6, 0, 0);
                        GR9.Margin = new Thickness(l9, u9, 0, 0);
                        GR8.Margin = new Thickness(l8, u8, 0, 0);
                        GR7.Margin = new Thickness(l7, u7, 0, 0);
                        GR4.Margin = new Thickness(l4, u4, 0, 0);
                        GR1.Margin = new Thickness(l1, u1, 0, 0);
                        GR2.Margin = new Thickness(l2, u2, 0, 0);
                        f1 = 1;
                    }
                    //Wenn Feld 1 auf Feld 9
                    else if (f1 == 9)
                    {
                        GR1.Margin = new Thickness(l3, u3, 0, 0);
                        GR2.Margin = new Thickness(l6, u6, 0, 0);
                        GR3.Margin = new Thickness(l9, u9, 0, 0);
                        GR6.Margin = new Thickness(l8, u8, 0, 0);
                        GR9.Margin = new Thickness(l7, u7, 0, 0);
                        GR8.Margin = new Thickness(l4, u4, 0, 0);
                        GR7.Margin = new Thickness(l1, u1, 0, 0);
                        GR4.Margin = new Thickness(l2, u2, 0, 0);
                        f1 = 3;
                    }
                    //Wenn Feld 1 auf Feld 7
                    else if (f1 == 7)
                    {
                        GR7.Margin = new Thickness(l3, u3, 0, 0);
                        GR4.Margin = new Thickness(l6, u6, 0, 0);
                        GR1.Margin = new Thickness(l9, u9, 0, 0);
                        GR2.Margin = new Thickness(l8, u8, 0, 0);
                        GR3.Margin = new Thickness(l7, u7, 0, 0);
                        GR6.Margin = new Thickness(l4, u4, 0, 0);
                        GR9.Margin = new Thickness(l1, u1, 0, 0);
                        GR8.Margin = new Thickness(l2, u2, 0, 0);
                        f1 = 9;
                    }
                    //Status umstellen
                    statusturnms = 0;
                    statusturn = "no";
                }
            }
            //---------------------------------------------------------------


            //Animationen der Felder
            //---------------------------------------------------------------
            //Feld 1
            if (btnactiv == 1)
            {
                if (btnms == 0)
                {
                    //btnms erstellen
                    btnms = uhrzeitms;
                    //Buttonfeld über Feld legen
                    GRB.Margin = GR1.Margin;
                    RTB.Height = RT1.Height;
                    RTB.Width = RT1.Width;
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb(255, (byte)(r1), (byte)(g1), (byte)(b1));
                    RTB.Fill = mySolidColorBrushB;
                    //Feld transparent machen
                    mySolidColorBrush1.Color = System.Windows.Media.Color.FromArgb(0, (byte)(r1), (byte)(g1), (byte)(b1));
                    RT1.Fill = mySolidColorBrush1;
                }
                else if (btnms + 50 > uhrzeitms)
                {
                    //Prozent errechnen
                    int tp = (10000 / 50 * (uhrzeitms - btnms) / 100);
                    //Größe erstellen
                    int tgr = s1 - (((s1 * 100) / 100 * 80) / 100);
                    RTB.Height = s1 - (((tgr * 100) / 100 * tp) / 100);
                    RTB.Width = s1 - (((tgr * 100) / 100 * tp) / 100);
                    //Transparenz erstellen
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb(240, (byte)(r1), (byte)(g1), (byte)(b1));
                    RTB.Fill = mySolidColorBrushB;
                }
                else if (btnms + 150 > uhrzeitms)
                {
                    //Prozent erstellen
                    int tp = (10000 / 100 * (uhrzeitms - (btnms + 50)) / 100);
                    //Größe erstellen
                    int tgr = s1 - (((s1 * 100) / 100 * 80) / 100);
                    int way = 170 - s1 - tgr;
                    RTB.Height = (s1 - tgr) + (((way * 100) / 100 * tp) / 100);
                    RTB.Width = (s1 - tgr) + (((way * 100) / 100 * tp) / 100);
                    //Transparenz erstellen
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb(155, (byte)(r1), (byte)(g1), (byte)(b1));
                    RTB.Fill = mySolidColorBrushB;
                }
                else if (btnms + 1150 > uhrzeitms)
                {
                    //Prozent erstellen
                    int tp = (1000000 / 1150 * (uhrzeitms - (btnms + 150)) / 10000);
                    //Größe erstellen
                    int tgr = ((30 * 100) / 100 * tp) / 100;
                    RTB.Height = 170 - tgr;
                    RTB.Width = 170 - tgr;
                    int a = 155 + (((70 * 100) / 100 * tp) / 100);
                    //Transparenz erstellen
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r1), (byte)(g1), (byte)(b1));
                    RTB.Fill = mySolidColorBrushB;
                }
                else if (btnms + 2150 > uhrzeitms)
                {
                    //Prozent erstellen
                    int tp = (1000000 / 2150 * (uhrzeitms - (btnms + 1150)) / 10000);
                    //Größe erstellen
                    int tgr = ((30 * 100) / 100 * tp) / 100;
                    RTB.Height = 140 + tgr;
                    RTB.Width = 140 + tgr;
                    int a = 225 - (((70 * 100) / 100 * tp) / 100);
                    //Transparenz erstellen
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r1), (byte)(g1), (byte)(b1));
                    RTB.Fill = mySolidColorBrushB;
                }
                else
                {
                    btnms = uhrzeitms - 650;
                }
            }
            //Feld 2
            if (btnactiv == 2)
            {
                if (btnms == 0)
                {
                    //btnms erstellen
                    btnms = uhrzeitms;
                    //Buttonfeld über Feld legen
                    GRB.Margin = GR2.Margin;
                    RTB.Height = RT2.Height;
                    RTB.Width = RT2.Width;
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb(255, (byte)(r2), (byte)(g2), (byte)(b2));
                    RTB.Fill = mySolidColorBrushB;
                    //Feld transparent machen
                    mySolidColorBrush2.Color = System.Windows.Media.Color.FromArgb(0, (byte)(r2), (byte)(g2), (byte)(b2));
                    RT2.Fill = mySolidColorBrush2;
                }
                else if (btnms + 50 > uhrzeitms)
                {
                    //Prozent errechnen
                    int tp = (10000 / 50 * (uhrzeitms - btnms) / 100);
                    //Größe erstellen
                    int tgr = s2 - (((s2 * 100) / 100 * 80) / 100);
                    RTB.Height = s2 - (((tgr * 100) / 100 * tp) / 100);
                    RTB.Width = s2 - (((tgr * 100) / 100 * tp) / 100);
                    //Transparenz erstellen
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb(240, (byte)(r2), (byte)(g2), (byte)(b2));
                    RTB.Fill = mySolidColorBrushB;
                }
                else if (btnms + 150 > uhrzeitms)
                {
                    //Prozent erstellen
                    int tp = (10000 / 100 * (uhrzeitms - (btnms + 50)) / 100);
                    //Größe erstellen
                    int tgr = s2 - (((s2 * 100) / 100 * 80) / 100);
                    int way = 170 - s2 - tgr;
                    RTB.Height = (s2 - tgr) + (((way * 100) / 100 * tp) / 100);
                    RTB.Width = (s2 - tgr) + (((way * 100) / 100 * tp) / 100);
                    //Transparenz erstellen
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb(155, (byte)(r2), (byte)(g2), (byte)(b2));
                    RTB.Fill = mySolidColorBrushB;
                }
                else if (btnms + 1150 > uhrzeitms)
                {
                    //Prozent erstellen
                    int tp = (1000000 / 1150 * (uhrzeitms - (btnms + 150)) / 10000);
                    //Größe erstellen
                    int tgr = ((30 * 100) / 100 * tp) / 100;
                    RTB.Height = 170 - tgr;
                    RTB.Width = 170 - tgr;
                    int a = 155 + (((70 * 100) / 100 * tp) / 100);
                    //Transparenz erstellen
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r2), (byte)(g2), (byte)(b2));
                    RTB.Fill = mySolidColorBrushB;
                }
                else if (btnms + 2150 > uhrzeitms)
                {
                    //Prozent erstellen
                    int tp = (1000000 / 2150 * (uhrzeitms - (btnms + 1150)) / 10000);
                    //Größe erstellen
                    int tgr = ((30 * 100) / 100 * tp) / 100;
                    RTB.Height = 140 + tgr;
                    RTB.Width = 140 + tgr;
                    int a = 225 - (((70 * 100) / 100 * tp) / 100);
                    //Transparenz erstellen
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r2), (byte)(g2), (byte)(b2));
                    RTB.Fill = mySolidColorBrushB;
                }
                else
                {
                    btnms = uhrzeitms - 650;
                }
            }
            //Feld 3
            if (btnactiv == 3)
            {
                if (btnms == 0)
                {
                    //btnms erstellen
                    btnms = uhrzeitms;
                    //Buttonfeld über Feld legen
                    GRB.Margin = GR3.Margin;
                    RTB.Height = RT3.Height;
                    RTB.Width = RT3.Width;
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb(255, (byte)(r3), (byte)(g3), (byte)(b3));
                    RTB.Fill = mySolidColorBrushB;
                    //Feld transparent machen
                    mySolidColorBrush3.Color = System.Windows.Media.Color.FromArgb(0, (byte)(r3), (byte)(g3), (byte)(b3));
                    RT3.Fill = mySolidColorBrush3;
                }
                else if (btnms + 50 > uhrzeitms)
                {
                    //Prozent errechnen
                    int tp = (10000 / 50 * (uhrzeitms - btnms) / 100);
                    //Größe erstellen
                    int tgr = s3 - (((s3 * 100) / 100 * 80) / 100);
                    RTB.Height = s3 - (((tgr * 100) / 100 * tp) / 100);
                    RTB.Width = s3 - (((tgr * 100) / 100 * tp) / 100);
                    //Transparenz erstellen
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb(240, (byte)(r3), (byte)(g3), (byte)(b3));
                    RTB.Fill = mySolidColorBrushB;
                }
                else if (btnms + 150 > uhrzeitms)
                {
                    //Prozent erstellen
                    int tp = (10000 / 100 * (uhrzeitms - (btnms + 50)) / 100);
                    //Größe erstellen
                    int tgr = s3 - (((s3 * 100) / 100 * 80) / 100);
                    int way = 170 - s3 - tgr;
                    RTB.Height = (s3 - tgr) + (((way * 100) / 100 * tp) / 100);
                    RTB.Width = (s3 - tgr) + (((way * 100) / 100 * tp) / 100);
                    //Transparenz erstellen
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb(155, (byte)(r3), (byte)(g3), (byte)(b3));
                    RTB.Fill = mySolidColorBrushB;
                }
                else if (btnms + 1150 > uhrzeitms)
                {
                    //Prozent erstellen
                    int tp = (1000000 / 1150 * (uhrzeitms - (btnms + 150)) / 10000);
                    //Größe erstellen
                    int tgr = ((30 * 100) / 100 * tp) / 100;
                    RTB.Height = 170 - tgr;
                    RTB.Width = 170 - tgr;
                    int a = 155 + (((70 * 100) / 100 * tp) / 100);
                    //Transparenz erstellen
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r3), (byte)(g3), (byte)(b3));
                    RTB.Fill = mySolidColorBrushB;
                }
                else if (btnms + 2150 > uhrzeitms)
                {
                    //Prozent erstellen
                    int tp = (1000000 / 2150 * (uhrzeitms - (btnms + 1150)) / 10000);
                    //Größe erstellen
                    int tgr = ((30 * 100) / 100 * tp) / 100;
                    RTB.Height = 140 + tgr;
                    RTB.Width = 140 + tgr;
                    int a = 225 - (((70 * 100) / 100 * tp) / 100);
                    //Transparenz erstellen
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r3), (byte)(g3), (byte)(b3));
                    RTB.Fill = mySolidColorBrushB;
                }
                else
                {
                    btnms = uhrzeitms - 650;
                }
            }
            //Feld 4
            if (btnactiv == 4)
            {
                if (btnms == 0)
                {
                    //btnms erstellen
                    btnms = uhrzeitms;
                    //Buttonfeld über Feld legen
                    GRB.Margin = GR4.Margin;
                    RTB.Height = RT4.Height;
                    RTB.Width = RT4.Width;
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb(255, (byte)(r4), (byte)(g4), (byte)(b4));
                    RTB.Fill = mySolidColorBrushB;
                    //Feld transparent machen
                    mySolidColorBrush4.Color = System.Windows.Media.Color.FromArgb(0, (byte)(r4), (byte)(g4), (byte)(b4));
                    RT4.Fill = mySolidColorBrush4;
                }
                else if (btnms + 50 > uhrzeitms)
                {
                    //Prozent errechnen
                    int tp = (10000 / 50 * (uhrzeitms - btnms) / 100);
                    //Größe erstellen
                    int tgr = s4 - (((s4 * 100) / 100 * 80) / 100);
                    RTB.Height = s4 - (((tgr * 100) / 100 * tp) / 100);
                    RTB.Width = s4 - (((tgr * 100) / 100 * tp) / 100);
                    //Transparenz erstellen
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb(240, (byte)(r4), (byte)(g4), (byte)(b4));
                    RTB.Fill = mySolidColorBrushB;
                }
                else if (btnms + 150 > uhrzeitms)
                {
                    //Prozent erstellen
                    int tp = (10000 / 100 * (uhrzeitms - (btnms + 50)) / 100);
                    //Größe erstellen
                    int tgr = s4 - (((s4 * 100) / 100 * 80) / 100);
                    int way = 170 - s4 - tgr;
                    RTB.Height = (s4 - tgr) + (((way * 100) / 100 * tp) / 100);
                    RTB.Width = (s4 - tgr) + (((way * 100) / 100 * tp) / 100);
                    //Transparenz erstellen
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb(155, (byte)(r4), (byte)(g4), (byte)(b4));
                    RTB.Fill = mySolidColorBrushB;
                }
                else if (btnms + 1150 > uhrzeitms)
                {
                    //Prozent erstellen
                    int tp = (1000000 / 1150 * (uhrzeitms - (btnms + 150)) / 10000);
                    //Größe erstellen
                    int tgr = ((30 * 100) / 100 * tp) / 100;
                    RTB.Height = 170 - tgr;
                    RTB.Width = 170 - tgr;
                    int a = 155 + (((70 * 100) / 100 * tp) / 100);
                    //Transparenz erstellen
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r4), (byte)(g4), (byte)(b4));
                    RTB.Fill = mySolidColorBrushB;
                }
                else if (btnms + 2150 > uhrzeitms)
                {
                    //Prozent erstellen
                    int tp = (1000000 / 2150 * (uhrzeitms - (btnms + 1150)) / 10000);
                    //Größe erstellen
                    int tgr = ((30 * 100) / 100 * tp) / 100;
                    RTB.Height = 140 + tgr;
                    RTB.Width = 140 + tgr;
                    int a = 225 - (((70 * 100) / 100 * tp) / 100);
                    //Transparenz erstellen
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r4), (byte)(g4), (byte)(b4));
                    RTB.Fill = mySolidColorBrushB;
                }
                else
                {
                    btnms = uhrzeitms - 650;
                }
            }
            //Feld 5
            if (btnactiv == 5)
            {
                if (btnms == 0)
                {
                    //btnms erstellen
                    btnms = uhrzeitms;
                    //Buttonfeld über Feld legen
                    GRB.Margin = GR5.Margin;
                    RTB.Height = RT5.Height;
                    RTB.Width = RT5.Width;
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb(255, (byte)(r5), (byte)(g5), (byte)(b5));
                    RTB.Fill = mySolidColorBrushB;
                    //Feld transparent machen
                    mySolidColorBrush5.Color = System.Windows.Media.Color.FromArgb(0, (byte)(r5), (byte)(g5), (byte)(b5));
                    RT5.Fill = mySolidColorBrush5;
                }
                else if (btnms + 50 > uhrzeitms)
                {
                    //Prozent errechnen
                    int tp = (10000 / 50 * (uhrzeitms - btnms) / 100);
                    //Größe erstellen
                    int tgr = s5 - (((s5 * 100) / 100 * 80) / 100);
                    RTB.Height = s5 - (((tgr * 100) / 100 * tp) / 100);
                    RTB.Width = s5 - (((tgr * 100) / 100 * tp) / 100);
                    //Transparenz erstellen
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb(240, (byte)(r5), (byte)(g5), (byte)(b5));
                    RTB.Fill = mySolidColorBrushB;
                }
                else if (btnms + 150 > uhrzeitms)
                {
                    //Prozent erstellen
                    int tp = (10000 / 100 * (uhrzeitms - (btnms + 50)) / 100);
                    //Größe erstellen
                    int tgr = s5 - (((s5 * 100) / 100 * 80) / 100);
                    int way = 170 - s5 - tgr;
                    RTB.Height = (s5 - tgr) + (((way * 100) / 100 * tp) / 100);
                    RTB.Width = (s5 - tgr) + (((way * 100) / 100 * tp) / 100);
                    //Transparenz erstellen
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb(155, (byte)(r5), (byte)(g5), (byte)(b5));
                    RTB.Fill = mySolidColorBrushB;
                }
                else if (btnms + 1150 > uhrzeitms)
                {
                    //Prozent erstellen
                    int tp = (1000000 / 1150 * (uhrzeitms - (btnms + 150)) / 10000);
                    //Größe erstellen
                    int tgr = ((30 * 100) / 100 * tp) / 100;
                    RTB.Height = 170 - tgr;
                    RTB.Width = 170 - tgr;
                    int a = 155 + (((70 * 100) / 100 * tp) / 100);
                    //Transparenz erstellen
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r5), (byte)(g5), (byte)(b5));
                    RTB.Fill = mySolidColorBrushB;
                }
                else if (btnms + 2150 > uhrzeitms)
                {
                    //Prozent erstellen
                    int tp = (1000000 / 2150 * (uhrzeitms - (btnms + 1150)) / 10000);
                    //Größe erstellen
                    int tgr = ((30 * 100) / 100 * tp) / 100;
                    RTB.Height = 140 + tgr;
                    RTB.Width = 140 + tgr;
                    int a = 225 - (((70 * 100) / 100 * tp) / 100);
                    //Transparenz erstellen
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r5), (byte)(g5), (byte)(b5));
                    RTB.Fill = mySolidColorBrushB;
                }
                else
                {
                    btnms = uhrzeitms - 650;
                }
            }
            //Feld 6
            if (btnactiv == 6)
            {
                if (btnms == 0)
                {
                    //btnms erstellen
                    btnms = uhrzeitms;
                    //Buttonfeld über Feld legen
                    GRB.Margin = GR6.Margin;
                    RTB.Height = RT6.Height;
                    RTB.Width = RT6.Width;
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb(255, (byte)(r6), (byte)(g6), (byte)(b6));
                    RTB.Fill = mySolidColorBrushB;
                    //Feld transparent machen
                    mySolidColorBrush6.Color = System.Windows.Media.Color.FromArgb(0, (byte)(r6), (byte)(g6), (byte)(b6));
                    RT6.Fill = mySolidColorBrush6;
                }
                else if (btnms + 50 > uhrzeitms)
                {
                    //Prozent errechnen
                    int tp = (10000 / 50 * (uhrzeitms - btnms) / 100);
                    //Größe erstellen
                    int tgr = s6 - (((s6 * 100) / 100 * 80) / 100);
                    RTB.Height = s6 - (((tgr * 100) / 100 * tp) / 100);
                    RTB.Width = s6 - (((tgr * 100) / 100 * tp) / 100);
                    //Transparenz erstellen
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb(240, (byte)(r6), (byte)(g6), (byte)(b6));
                    RTB.Fill = mySolidColorBrushB;
                }
                else if (btnms + 150 > uhrzeitms)
                {
                    //Prozent erstellen
                    int tp = (10000 / 100 * (uhrzeitms - (btnms + 50)) / 100);
                    //Größe erstellen
                    int tgr = s6 - (((s6 * 100) / 100 * 80) / 100);
                    int way = 170 - s6 - tgr;
                    RTB.Height = (s6 - tgr) + (((way * 100) / 100 * tp) / 100);
                    RTB.Width = (s6 - tgr) + (((way * 100) / 100 * tp) / 100);
                    //Transparenz erstellen
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb(155, (byte)(r6), (byte)(g6), (byte)(b6));
                    RTB.Fill = mySolidColorBrushB;
                }
                else if (btnms + 1150 > uhrzeitms)
                {
                    //Prozent erstellen
                    int tp = (1000000 / 1150 * (uhrzeitms - (btnms + 150)) / 10000);
                    //Größe erstellen
                    int tgr = ((30 * 100) / 100 * tp) / 100;
                    RTB.Height = 170 - tgr;
                    RTB.Width = 170 - tgr;
                    int a = 155 + (((70 * 100) / 100 * tp) / 100);
                    //Transparenz erstellen
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r6), (byte)(g6), (byte)(b6));
                    RTB.Fill = mySolidColorBrushB;
                }
                else if (btnms + 2150 > uhrzeitms)
                {
                    //Prozent erstellen
                    int tp = (1000000 / 2150 * (uhrzeitms - (btnms + 1150)) / 10000);
                    //Größe erstellen
                    int tgr = ((30 * 100) / 100 * tp) / 100;
                    RTB.Height = 140 + tgr;
                    RTB.Width = 140 + tgr;
                    int a = 225 - (((70 * 100) / 100 * tp) / 100);
                    //Transparenz erstellen
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r6), (byte)(g6), (byte)(b6));
                    RTB.Fill = mySolidColorBrushB;
                }
                else
                {
                    btnms = uhrzeitms - 650;
                }
            }
            //Feld 7
            if (btnactiv == 7)
            {
                if (btnms == 0)
                {
                    //btnms erstellen
                    btnms = uhrzeitms;
                    //Buttonfeld über Feld legen
                    GRB.Margin = GR7.Margin;
                    RTB.Height = RT7.Height;
                    RTB.Width = RT7.Width;
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb(255, (byte)(r7), (byte)(g7), (byte)(b7));
                    RTB.Fill = mySolidColorBrushB;
                    //Feld transparent machen
                    mySolidColorBrush7.Color = System.Windows.Media.Color.FromArgb(0, (byte)(r7), (byte)(g7), (byte)(b7));
                    RT7.Fill = mySolidColorBrush7;
                }
                else if (btnms + 50 > uhrzeitms)
                {
                    //Prozent errechnen
                    int tp = (10000 / 50 * (uhrzeitms - btnms) / 100);
                    //Größe erstellen
                    int tgr = s7 - (((s7 * 100) / 100 * 80) / 100);
                    RTB.Height = s7 - (((tgr * 100) / 100 * tp) / 100);
                    RTB.Width = s7 - (((tgr * 100) / 100 * tp) / 100);
                    //Transparenz erstellen
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb(240, (byte)(r7), (byte)(g7), (byte)(b7));
                    RTB.Fill = mySolidColorBrushB;
                }
                else if (btnms + 150 > uhrzeitms)
                {
                    //Prozent erstellen
                    int tp = (10000 / 100 * (uhrzeitms - (btnms + 50)) / 100);
                    //Größe erstellen
                    int tgr = s5 - (((s5 * 100) / 100 * 80) / 100);
                    int way = 170 - s5 - tgr;
                    RTB.Height = (s7 - tgr) + (((way * 100) / 100 * tp) / 100);
                    RTB.Width = (s7 - tgr) + (((way * 100) / 100 * tp) / 100);
                    //Transparenz erstellen
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb(155, (byte)(r7), (byte)(g7), (byte)(b7));
                    RTB.Fill = mySolidColorBrushB;
                }
                else if (btnms + 1150 > uhrzeitms)
                {
                    //Prozent erstellen
                    int tp = (1000000 / 1150 * (uhrzeitms - (btnms + 150)) / 10000);
                    //Größe erstellen
                    int tgr = ((30 * 100) / 100 * tp) / 100;
                    RTB.Height = 170 - tgr;
                    RTB.Width = 170 - tgr;
                    int a = 155 + (((70 * 100) / 100 * tp) / 100);
                    //Transparenz erstellen
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r7), (byte)(g7), (byte)(b7));
                    RTB.Fill = mySolidColorBrushB;
                }
                else if (btnms + 2150 > uhrzeitms)
                {
                    //Prozent erstellen
                    int tp = (1000000 / 2150 * (uhrzeitms - (btnms + 1150)) / 10000);
                    //Größe erstellen
                    int tgr = ((30 * 100) / 100 * tp) / 100;
                    RTB.Height = 140 + tgr;
                    RTB.Width = 140 + tgr;
                    int a = 225 - (((70 * 100) / 100 * tp) / 100);
                    //Transparenz erstellen
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r7), (byte)(g7), (byte)(b7));
                    RTB.Fill = mySolidColorBrushB;
                }
                else
                {
                    btnms = uhrzeitms - 650;
                }
            }
            //Feld 8
            if (btnactiv == 8)
            {
                if (btnms == 0)
                {
                    //btnms erstellen
                    btnms = uhrzeitms;
                    //Buttonfeld über Feld legen
                    GRB.Margin = GR8.Margin;
                    RTB.Height = RT8.Height;
                    RTB.Width = RT8.Width;
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb(255, (byte)(r8), (byte)(g8), (byte)(b8));
                    RTB.Fill = mySolidColorBrushB;
                    //Feld transparent machen
                    mySolidColorBrush8.Color = System.Windows.Media.Color.FromArgb(0, (byte)(r8), (byte)(g8), (byte)(b8));
                    RT8.Fill = mySolidColorBrush8;
                }
                else if (btnms + 50 > uhrzeitms)
                {
                    //Prozent errechnen
                    int tp = (10000 / 50 * (uhrzeitms - btnms) / 100);
                    //Größe erstellen
                    int tgr = s8 - (((s8 * 100) / 100 * 80) / 100);
                    RTB.Height = s8 - (((tgr * 100) / 100 * tp) / 100);
                    RTB.Width = s8 - (((tgr * 100) / 100 * tp) / 100);
                    //Transparenz erstellen
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb(240, (byte)(r8), (byte)(g8), (byte)(b8));
                    RTB.Fill = mySolidColorBrushB;
                }
                else if (btnms + 150 > uhrzeitms)
                {
                    //Prozent erstellen
                    int tp = (10000 / 100 * (uhrzeitms - (btnms + 50)) / 100);
                    //Größe erstellen
                    int tgr = s8 - (((s8 * 100) / 100 * 80) / 100);
                    int way = 170 - s8 - tgr;
                    RTB.Height = (s8 - tgr) + (((way * 100) / 100 * tp) / 100);
                    RTB.Width = (s8 - tgr) + (((way * 100) / 100 * tp) / 100);
                    //Transparenz erstellen
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb(155, (byte)(r8), (byte)(g8), (byte)(b8));
                    RTB.Fill = mySolidColorBrushB;
                }
                else if (btnms + 1150 > uhrzeitms)
                {
                    //Prozent erstellen
                    int tp = (1000000 / 1150 * (uhrzeitms - (btnms + 150)) / 10000);
                    //Größe erstellen
                    int tgr = ((30 * 100) / 100 * tp) / 100;
                    RTB.Height = 170 - tgr;
                    RTB.Width = 170 - tgr;
                    int a = 155 + (((70 * 100) / 100 * tp) / 100);
                    //Transparenz erstellen
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r8), (byte)(g8), (byte)(b8));
                    RTB.Fill = mySolidColorBrushB;
                }
                else if (btnms + 2150 > uhrzeitms)
                {
                    //Prozent erstellen
                    int tp = (1000000 / 2150 * (uhrzeitms - (btnms + 1150)) / 10000);
                    //Größe erstellen
                    int tgr = ((30 * 100) / 100 * tp) / 100;
                    RTB.Height = 140 + tgr;
                    RTB.Width = 140 + tgr;
                    int a = 225 - (((70 * 100) / 100 * tp) / 100);
                    //Transparenz erstellen
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r8), (byte)(g8), (byte)(b8));
                    RTB.Fill = mySolidColorBrushB;
                }
                else
                {
                    btnms = uhrzeitms - 650;
                }
            }
            //Feld 9
            if (btnactiv == 9)
            {
                if (btnms == 0)
                {
                    //btnms erstellen
                    btnms = uhrzeitms;
                    //Buttonfeld über Feld legen
                    GRB.Margin = GR9.Margin;
                    RTB.Height = RT9.Height;
                    RTB.Width = RT9.Width;
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb(255, (byte)(r9), (byte)(g9), (byte)(b9));
                    RTB.Fill = mySolidColorBrushB;
                    //Feld transparent machen
                    mySolidColorBrush9.Color = System.Windows.Media.Color.FromArgb(0, (byte)(r9), (byte)(g9), (byte)(b9));
                    RT9.Fill = mySolidColorBrush9;
                }
                else if (btnms + 50 > uhrzeitms)
                {
                    //Prozent errechnen
                    int tp = (10000 / 50 * (uhrzeitms - btnms) / 100);
                    //Größe erstellen
                    int tgr = s9 - (((s9 * 100) / 100 * 80) / 100);
                    RTB.Height = s9 - (((tgr * 100) / 100 * tp) / 100);
                    RTB.Width = s9 - (((tgr * 100) / 100 * tp) / 100);
                    //Transparenz erstellen
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb(240, (byte)(r9), (byte)(g9), (byte)(b9));
                    RTB.Fill = mySolidColorBrushB;
                }
                else if (btnms + 150 > uhrzeitms)
                {
                    //Prozent erstellen
                    int tp = (10000 / 100 * (uhrzeitms - (btnms + 50)) / 100);
                    //Größe erstellen
                    int tgr = s9 - (((s9 * 100) / 100 * 80) / 100);
                    int way = 170 - s9 - tgr;
                    RTB.Height = (s9 - tgr) + (((way * 100) / 100 * tp) / 100);
                    RTB.Width = (s9 - tgr) + (((way * 100) / 100 * tp) / 100);
                    //Transparenz erstellen
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb(155, (byte)(r9), (byte)(g9), (byte)(b9));
                    RTB.Fill = mySolidColorBrushB;
                }
                else if (btnms + 1150 > uhrzeitms)
                {
                    //Prozent erstellen
                    int tp = (1000000 / 1150 * (uhrzeitms - (btnms + 150)) / 10000);
                    //Größe erstellen
                    int tgr = ((30 * 100) / 100 * tp) / 100;
                    RTB.Height = 170 - tgr;
                    RTB.Width = 170 - tgr;
                    int a = 155 + (((70 * 100) / 100 * tp) / 100);
                    //Transparenz erstellen
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r9), (byte)(g9), (byte)(b9));
                    RTB.Fill = mySolidColorBrushB;
                }
                else if (btnms + 2150 > uhrzeitms)
                {
                    //Prozent erstellen
                    int tp = (1000000 / 2150 * (uhrzeitms - (btnms + 1150)) / 10000);
                    //Größe erstellen
                    int tgr = ((30 * 100) / 100 * tp) / 100;
                    RTB.Height = 140 + tgr;
                    RTB.Width = 140 + tgr;
                    int a = 225 - (((70 * 100) / 100 * tp) / 100);
                    //Transparenz erstellen
                    mySolidColorBrushB.Color = System.Windows.Media.Color.FromArgb((byte)(a), (byte)(r9), (byte)(g9), (byte)(b9));
                    RTB.Fill = mySolidColorBrushB;
                }
                else
                {
                    btnms = uhrzeitms - 650;
                }
            }


            //Animation Punkte
            //---------------------------------------------------------------
            if (anipoints != "")
            {
                if (anipointsms == 0)
                {
                    //ms erstellen
                    anipointsms = uhrzeitms;
                    //Points2 erstellen
                    POINTS2.Visibility = System.Windows.Visibility.Visible;
                    POINTS2.Opacity = 0.3;
                    POINTS2.FontSize = 90;
                    POINTS2.Text = anipoints;
                }
                else if (anipointsms + 50 > uhrzeitms)
                {
                    //Transparenz entfernen
                    POINTS2.Opacity = 0.4;
                }
                else if (anipointsms + 100 > uhrzeitms)
                {
                    //Transparenz entfernen
                    POINTS2.Opacity = 0.6;
                }
                else if (anipointsms + 150 > uhrzeitms)
                {
                    //Transparenz entfernen
                    POINTS2.Opacity = 0.7;
                }
                else if (anipointsms + 350 > uhrzeitms)
                {
                    //Prozent errechnen
                    int tp = (100000 / 200 * (uhrzeitms - (anipointsms + 150)) / 1000);
                    int tsize = 90 - (60000 / 100 * tp / 1000);
                    //Prozent auf Animation anwenden
                    POINTS2.FontSize = tsize;
                }
                else
                {
                    //Punkte zurücksetzen
                    POINTS2.Visibility = System.Windows.Visibility.Collapsed;
                    //Animation Punkte zurücksetzten
                    anipoints = "";
                    //ms zurücksetzen;
                    anipointsms = 0;
                }
            }
            //---------------------------------------------------------------


            //Animation der Zeit 
            //---------------------------------------------------------------
            if (anitime != "")
            {
                if (anitimems == 0)
                {
                    //ms erstellen
                    anitimems = uhrzeitms;
                    //TIME2 erstellen
                    TIME2.Visibility = System.Windows.Visibility.Visible;
                    TIME2.Opacity = 0.3;
                    TIME2.FontSize = 90;
                    TIME2.Text = anitime;
                }
                else if (anitimems + 50 > uhrzeitms)
                {
                    //Transparenz entfernen
                    TIME2.Opacity = 0.4;
                }
                else if (anitimems + 100 > uhrzeitms)
                {
                    //Transparenz entfernen
                    TIME2.Opacity = 0.6;
                }
                else if (anitimems + 150 > uhrzeitms)
                {
                    //Transparenz entfernen
                    TIME2.Opacity = 0.7;
                }
                else if (anitimems + 350 > uhrzeitms)
                {
                    //Prozent errechnen
                    int tp = (100000 / 200 * (uhrzeitms - (anitimems + 150)) / 1000);
                    int tsize = 90 - (60000 / 100 * tp / 1000);
                    //Prozent auf Animation anwenden
                    TIME2.FontSize = tsize;
                }
                else
                {
                    //Punkte zurücksetzen
                    TIME2.Visibility = System.Windows.Visibility.Collapsed;
                    //Animation Punkte zurücksetzten
                    anitime = "";
                    //ms zurücksetzen;
                    anitimems = 0;
                }
            }
            //---------------------------------------------------------------
        }
        //---------------------------------------------------------------------------------------------------------------------------------
        
        



        //Buttons Felder
        //---------------------------------------------------------------------------------------------------------------------------------
        //Variabeln
        int btnactiv = 0;
        int btnms = 0;

        //Button Feld 1
        private void RT1_MouseLeftButtonDown_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (statussite == "play")
            {
                if (TB1.Text == "")
                {
                    TBB.Text = "";
                    btnactiv = 1;
                    btnms = 0;
                    ButtonsReset();
                }
            }
        }

        //Button Feld 2
        private void RT2_MouseLeftButtonDown_2(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (statussite == "play")
            {
                if (TB2.Text == "")
                {
                    TBB.Text = "";
                    btnactiv = 2;
                    btnms = 0;
                    ButtonsReset();
                }
            }
        }

        //Button Feld 3
        private void RT3_MouseLeftButtonDown_3(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (statussite == "play")
            {
                if (TB3.Text == "")
                {
                    TBB.Text = "";
                    btnactiv = 3;
                    btnms = 0;
                    ButtonsReset();
                }
            }
        }

        //Button Feld 4
        private void RT4_MouseLeftButtonDown_4(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (statussite == "play")
            {
                if (TB4.Text == "")
                {
                    TBB.Text = "";
                    btnactiv = 4;
                    btnms = 0;
                    ButtonsReset();
                }
            }
        }

        //Button Feld 5
        private void RT5_MouseLeftButtonDown_5(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (statussite == "play")
            {
                if (TB5.Text == "")
                {
                    TBB.Text = "";
                    btnactiv = 5;
                    btnms = 0;
                    ButtonsReset();
                }
            }
        }

        //Button Feld 6
        private void RT6_MouseLeftButtonDown_6(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (statussite == "play")
            {
                if (TB6.Text == "")
                {
                    TBB.Text = "";
                    btnactiv = 6;
                    btnms = 0;
                    ButtonsReset();
                }
            }
        }

        //Button Feld 7
        private void RT7_MouseLeftButtonDown_7(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (statussite == "play")
            {
                if (TB7.Text == "")
                {
                    TBB.Text = "";
                    btnactiv = 7;
                    btnms = 0;
                    ButtonsReset();
                }
            }
        }

        //Button Feld 8
        private void RT8_MouseLeftButtonDown_8(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (statussite == "play")
            {
                if (TB8.Text == "")
                {
                    TBB.Text = "";
                    btnactiv = 8;
                    btnms = 0;
                    ButtonsReset();
                }
            }
        }

        //Button Feld 9
        private void RT9_MouseLeftButtonDown_9(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (statussite == "play")
            {
                if (TB9.Text == "")
                {
                    TBB.Text = "";
                    btnactiv = 9;
                    btnms = 0;
                    ButtonsReset();
                }
            }
        }

        //Extra Button
        private void RTB_MouseLeftButtonDown_B(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //Am Anfang des Spiels (Start Button)
            if (statussite == "start")
            {
                //Startanimation Starten
                statussite = "getstart";
                statussitems = 0;
            }
            //Während des Spiels
            else if (statussite == "play")
            {
                if (btnactiv != 0)
                {
                    TBB.Text = "";
                    btnactiv = 0;
                    btnms = 0;
                    ButtonsDeaktivate();
                }
            }
        }


        //Alle Buttons zurück setzen
        void ButtonsReset()
        {
            //Buttons zurücksetzen
            mySolidColorBrush1.Color = System.Windows.Media.Color.FromArgb(255, (byte)(r1), (byte)(g1), (byte)(b1));
            RT1.Fill = mySolidColorBrush1;
            RT1.Height = s1;
            RT1.Width = s1;
            mySolidColorBrush2.Color = System.Windows.Media.Color.FromArgb(255, (byte)(r2), (byte)(g2), (byte)(b2));
            RT2.Fill = mySolidColorBrush2;
            RT2.Height = s2;
            RT2.Width = s2;
            mySolidColorBrush3.Color = System.Windows.Media.Color.FromArgb(255, (byte)(r3), (byte)(g3), (byte)(b3));
            RT3.Fill = mySolidColorBrush3;
            RT3.Height = s3;
            RT3.Width = s3;
            mySolidColorBrush4.Color = System.Windows.Media.Color.FromArgb(255, (byte)(r4), (byte)(g4), (byte)(b4));
            RT4.Fill = mySolidColorBrush4;
            RT4.Height = s4;
            RT4.Width = s4;
            mySolidColorBrush5.Color = System.Windows.Media.Color.FromArgb(255, (byte)(r5), (byte)(g5), (byte)(b5));
            RT5.Fill = mySolidColorBrush5;
            RT5.Height = s5;
            RT5.Width = s5;
            mySolidColorBrush6.Color = System.Windows.Media.Color.FromArgb(255, (byte)(r6), (byte)(g6), (byte)(b6));
            RT6.Fill = mySolidColorBrush6;
            RT6.Height = s6;
            RT6.Width = s6;
            mySolidColorBrush7.Color = System.Windows.Media.Color.FromArgb(255, (byte)(r7), (byte)(g7), (byte)(b7));
            RT7.Fill = mySolidColorBrush7;
            RT7.Height = s7;
            RT7.Width = s7;
            mySolidColorBrush8.Color = System.Windows.Media.Color.FromArgb(255, (byte)(r8), (byte)(g8), (byte)(b8));
            RT8.Fill = mySolidColorBrush8;
            RT8.Height = s8;
            RT8.Width = s8;
            mySolidColorBrush9.Color = System.Windows.Media.Color.FromArgb(255, (byte)(r9), (byte)(g9), (byte)(b9));
            RT9.Fill = mySolidColorBrush9;
            RT9.Height = s9;
            RT9.Width = s9;
        }

        //Button Deaktivieren
        void ButtonsDeaktivate()
        {
            //btnactive zurücksetzen
            btnactiv = 0;
            //Buttons zurücksetzen
            ButtonsReset();
            //Extra Button zurücksetzen
            GRB.Margin = new Thickness(-200, 0, 0, 0);
        }

        //---------------------------------------------------------------------------------------------------------------------------------





        //Buttons Nummern
        //---------------------------------------------------------------------------------------------------------------------------------
        //Variabeln

        //Nummern Button 1
        private void NUM_MouseLeftButtonDown_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (btnactiv != 0 & statussite == "play")
            {
                if (TBB.Text.Length < 5)
                {
                    TBB.Text = TBB.Text + "1";
                    CheckResult();
                }
            }
        }

        //Nummern Button 2
        private void NUM_MouseLeftButtonDown_2(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (btnactiv != 0 & statussite == "play")
            {
                if (TBB.Text.Length < 5)
                {
                    TBB.Text = TBB.Text + "2";
                    CheckResult();
                }
            }
        }

        //Nummern Button 3
        private void NUM_MouseLeftButtonDown_3(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (btnactiv != 0 & statussite == "play")
            {
                if (TBB.Text.Length < 5)
                {
                    TBB.Text = TBB.Text + "3";
                    CheckResult();
                }
            }
        }

        //Nummern Button 4
        private void NUM_MouseLeftButtonDown_4(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (btnactiv != 0 & statussite == "play")
            {
                if (TBB.Text.Length < 5)
                {
                    TBB.Text = TBB.Text + "4";
                    CheckResult();
                }
            }
        }

        //Nummern Button 5
        private void NUM_MouseLeftButtonDown_5(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (btnactiv != 0 & statussite == "play")
            {
                if (TBB.Text.Length < 5)
                {
                    TBB.Text = TBB.Text + "5";
                    CheckResult();
                }
            }
        }

        //Nummern Button 6
        private void NUM_MouseLeftButtonDown_6(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (btnactiv != 0 & statussite == "play")
            {
                if (TBB.Text.Length < 5)
                {
                    TBB.Text = TBB.Text + "6";
                    CheckResult();
                }
            }
        }

        //Nummern Button 7
        private void NUM_MouseLeftButtonDown_7(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (btnactiv != 0 & statussite == "play")
            {
                if (TBB.Text.Length < 5)
                {
                    TBB.Text = TBB.Text + "7";
                    CheckResult();
                }
            }
        }

        //Nummern Button 8
        private void NUM_MouseLeftButtonDown_8(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (btnactiv != 0 & statussite == "play")
            {
                if (TBB.Text.Length < 5)
                {
                    TBB.Text = TBB.Text + "8";
                    CheckResult();
                }
            }
        }

        //Nummern Button 9
        private void NUM_MouseLeftButtonDown_9(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (btnactiv != 0 & statussite == "play")
            {
                if (TBB.Text.Length < 5)
                {
                    TBB.Text = TBB.Text + "9";
                    CheckResult();
                }
            }
        }

        //Nummern Button 0
        private void NUM_MouseLeftButtonDown_0(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (btnactiv != 0 & statussite == "play")
            {
                if (TBB.Text != "" & TBB.Text.Length < 5)
                {
                    TBB.Text = TBB.Text + "0";
                    CheckResult();
                }
            }
        }

        //Nummern Button DEL
        private void NUM_MouseLeftButtonDown_DEL(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (btnactiv != 0 & statussite == "play")
            {
                if (TBB.Text.Length >= 1)
                {
                    int tl = TBB.Text.Length;
                    string tta = TBB.Text;
                    string ttn = "";
                    for (int i = 0; i < (tl - 1) ; i++)
                    {
                        ttn = ttn + tta[i].ToString();
                    }
                    TBB.Text = ttn;
                }
            }
        }

        //Nummern Button CLR
        private void NUM_MouseLeftButtonDown_CLR(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (btnactiv != 0 & statussite == "play")
            {
                if (TBB.Text != "")
                {
                    TBB.Text = "";
                }
            }
        }
        //---------------------------------------------------------------------------------------------------------------------------------





        //Warnung beim verlassen des Spiels
        //---------------------------------------------------------------------------------------------------------------------------------
        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (statussite == "play")
            {
                //Warnung ausgeben
                if (MessageBox.Show("Are you sure you want to exit game?", "WARNING!", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
                {
                    //App Beenden abbrechen
                    e.Cancel = true;
                }
            }
        }
        //---------------------------------------------------------------------------------------------------------------------------------


        
    }
}