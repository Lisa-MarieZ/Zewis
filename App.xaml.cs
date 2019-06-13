using System;
using System.Globalization;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Zewis
{
    //Hauptklasse steuert die App
    public partial class App : Application
    {
        //Datenbankinitialisierung
        static ZewisDatabase database;
        //Setzen der Sprache Deutsch
        public static CultureInfo culture = new System.Globalization.CultureInfo("de-DE");

        //Methode zum Erzeugen der Datenbank
        public static ZewisDatabase Database
        {
            get
            {
                if (database == null)
                {   
                    //Erzeugen der Datenbank als Instanz
                    database = new ZewisDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ZewisSQLite.db3"));
                }
                return database;
            }
        }

        public App()
        {
            InitializeComponent();
            //Hauptseite erzeugen
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
