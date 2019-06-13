using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;

namespace Zewis
{
    //Klasse zum Hinzufügen von Arbeitszeiten
    public partial class AddTimeRecording : ContentPage
    {
        //Datum
        DateTime date;
        //Liste der Aktivitäten
        List<Activity> activities;

        //Konstruktor
        public AddTimeRecording(DateTime date)
        {
            //Standard von Xamarin erzeugt Objekte und Ansichten aus dem XAML Dateien
            InitializeComponent();
            this.date = date;
            //Wochentag
            DayOfWeek weekday = date.DayOfWeek;
            //Formatierte Ausgabe des Wochentages in Deutsch
            header.Text = " " + date.ToShortDateString() + " - " + App.culture.DateTimeFormat.GetDayName(weekday);
            //Holen der Aktivitäten aus der Datenbank
            var task = App.Database.GetActivitiesAsync();
            activities = task.Result;
            foreach (var act in activities)
            {
                //Aktivität in Dropdownmenu hinzufügen
                acts.Items.Add(act.Name);
            }
        }

        //Methode die den Speichern-Button steuert
        void Handle_Save(object sender, System.EventArgs e)
        {
            //Zeit als double Wert aus dem Eingabefeld konvertieren
            var ttime = Double.Parse(time.Text, NumberStyles.Float);
            //Speichern der einegegebenen Daten in die Datenbank
            App.Database.saveTimeRecordingAsync(date, proj.Text, App.Database.GetActivityIdAsync(acts.Items[acts.SelectedIndex]),ttime , invoiceable.IsToggled);

            //Seite vom Stack löschen
            Navigation.PopModalAsync();
            //Seite vom Stack löschen
            Navigation.PopModalAsync();
            //DayPage mit dem zuvor besuchten Datum aufrufen und 1 Sekunde warten
            //damit die gespeicherten Änderungen schon sichtbar sind(funktioniert leider weniger gut)
            Navigation.PushModalAsync(new DayPage(date),false).Wait(1000);

        }

        void Handle_Cancel(object sender, System.EventArgs e)
        {
            //Seite vom Stack löschen
            Navigation.PopModalAsync();
        }
    }
}
