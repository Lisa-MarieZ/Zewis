using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace Zewis
{
    //Seite Tagesübersicht
    public partial class DayPage : ContentPage
    {
        //Datum
        private DateTime date;
        //Wochentag
        private DayOfWeek weekday;
        //Gestenerkennung
        private SwipeGestureRecognizer leftSwipeGesture;
        private SwipeGestureRecognizer rightSwipeGesture;
        private TapGestureRecognizer tapGesture;

        public DayPage(DateTime date)
        {   
            //Standard...
            InitializeComponent();
            //Datum
            this.date = date;
            //Gesten erzeugen
            createGestures();
            //Wochentag erzeugen
            weekday = date.DayOfWeek;
            //Tabellenliste erzeugen
            var timeRecordings = new List<ViewCell>();
            double s = 0;
            //Holen der gespeicherten Arbeitszeiten für den Tag aus der Datenbank
            List<TimeRecording> recordings = App.Database.GetTimeRecordingAsync(date).Result;

            //Über die Arbeitszeiten ilterieren
            foreach (var record in recordings)
            {
                var layout = new StackLayout() { Orientation = StackOrientation.Horizontal };
                layout.Children.Add(new Label()
                {   
                    //Projektnamen einfügen
                    Text = record.ProjectName,
                    TextColor = Color.FromHex("#000000"),
                    VerticalOptions = LayoutOptions.Center
                });
                layout.Children.Add(new Label()
                {
                    //Zeit einfügen
                    Text = record.Time.ToString(),
                    TextColor = Color.FromHex("#503026"),
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.EndAndExpand
                });
                //Einzelnen Eintrag mit formatierten Ansicht erzeugen
                var timeRecord = new ViewCell() { View = layout };
                //Zur Anzeigetabelle hinzufügen
                timeRecordings.Add(timeRecord);
                //Arbeitszeiten addieren
                s += record.Time;
            }
            //Tabelle zum View hinzufügen
            this.FindByName<TableSection>("az").Add(timeRecordings);
            //Datum + Wochentag als formatierten Überschrift anzeigen
            m.Text = date.ToShortDateString() + " - " + App.culture.DateTimeFormat.GetDayName(weekday);
            //Summe der Arbeitszeiten anzeigen
            sum.Text += s.ToString();
            //Gestenerkennung zum Swipebalken hinzufügen
            canvasView.GestureRecognizers.Add(leftSwipeGesture);
            canvasView.GestureRecognizers.Add(rightSwipeGesture);
            backView.GestureRecognizers.Add(tapGesture);
        }


        //Methode zum Erzeugen der Gestenerkennung
        private void createGestures()
        {
            //Gestenerkennung instanziieren
            leftSwipeGesture = new SwipeGestureRecognizer();
            rightSwipeGesture = new SwipeGestureRecognizer();
            tapGesture = new TapGestureRecognizer();
            //Methoden verknüpfen
            leftSwipeGesture.Swiped += OnDaySwiped;
            //Richtung auswählen
            leftSwipeGesture.Direction = SwipeDirection.Left;
            rightSwipeGesture.Swiped += OnDaySwiped;
            rightSwipeGesture.Direction = SwipeDirection.Right;
            tapGesture.Tapped += OnBackTapped;
        }

        private void OnBackTapped(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        //Methode die durch Gestenerkennung gerufen wird (EventListener)
        private void OnDaySwiped(object sender, SwipedEventArgs e)
        {
            //Holen der maximalen Anzahl der Tage des jetzigen Monats
            var maxdays = DateTime.DaysInMonth(DateTime.Today.AddMonths(1).Year, DateTime.Today.AddMonths(1).Month);
            //Maximales Zukunftsdatum ermitteln
            var maxdate = DateTime.Parse(MainPage.ConvertSingleInt(maxdays) + "/" + MainPage.ConvertSingleInt(DateTime.Today.AddMonths(1).Month) + "/" + DateTime.Today.AddMonths(1).Year + " 00:00:00", App.culture);
            switch (e.Direction)
            {
                case SwipeDirection.Left:
                    // Ein Tag hinzufügen
                    date = date.AddDays(1);
                    //Falls das maximale Zukunftsdatum nicht überschritten wird, wird zur nächsten Tagesansicht gesprungen 
                    if (date <= maxdate)
                    {
                        Navigation.PopModalAsync();
                        Navigation.PushModalAsync(new DayPage(date),false);
                    }
                    break;
                case SwipeDirection.Right:
                    // Ein Tag abziehen
                    date = date.AddDays(-1);
                    //Es wird zur vorherigen Tagesansicht gesprungen(Bis zum 01.01.1901 möglich)
                    Navigation.PopModalAsync();
                    Navigation.PushModalAsync(new DayPage(date),false);
                    break;
                default:
                    break;
            }
        }

        //Methode die den + Button steuert
        void AddTime(object sender, System.EventArgs e)
        {
            //Navigiert zur Seite zum Hinzufügen der Arbeitszeit
            Navigation.PushModalAsync(new AddTimeRecording(date));
        }

        //Methode zum Zeichnen des Swipebereichs
        private void canvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear(SKColors.Transparent);

            int width = e.Info.Width;
            int height = e.Info.Height;

            //Anpassen der Breite und Höhe an den Bildschirm = Transform
            canvas.Translate(width / 16, height / 2);
            canvas.Scale(width / 100f);

            //oberer Teil des 1. Pfeils
            canvas.Save();
            canvas.RotateDegrees(45f);
            canvas.DrawLine(0, 1, 0, -50, MainPage.grayStroke);
            canvas.Restore();

            //unterer Teil des 1. Pfeils
            canvas.Save();
            canvas.RotateDegrees(-45f);
            canvas.DrawLine(0, -1, 0, 50, MainPage.grayStroke);
            canvas.Restore();

            canvas.ResetMatrix();

            //Transform
            canvas.Translate(width / 8, height / 2);
            canvas.Scale(width / 100f);

            //oberer Teil des 2. Pfeils
            canvas.Save();
            canvas.RotateDegrees(45f);
            canvas.DrawLine(0, 1, 0, -50, MainPage.lightGrayStroke);
            canvas.Restore();

            //unterer Teil des 2. Pfeils
            canvas.Save();
            canvas.RotateDegrees(-45f);
            canvas.DrawLine(0, -1, 0, 50, MainPage.lightGrayStroke);
            canvas.Restore();


            canvas.ResetMatrix();

            //Transform
            canvas.Translate(width - (width / 16), height / 2);
            canvas.Scale(width / 100f);

            ////oberer Teil des 3. Pfeils
            canvas.Save();
            canvas.RotateDegrees(-45f);
            canvas.DrawLine(0, 1, 0, -50, MainPage.grayStroke);
            canvas.Restore();

            ////unterer Teil des 3. Pfeils
            canvas.Save();
            canvas.RotateDegrees(45f);
            canvas.DrawLine(0, -1, 0, 50, MainPage.grayStroke);
            canvas.Restore();

            canvas.ResetMatrix();

            //Transform
            canvas.Translate(width - (width / 8), height / 2);
            canvas.Scale(width / 100f);

            //oberer Teil des 4. Pfeils
            canvas.Save();
            canvas.RotateDegrees(-45f);
            canvas.DrawLine(0, 1, 0, -50, MainPage.lightGrayStroke);
            canvas.Restore();

            //unterer Teil des 4. Pfeils
            canvas.Save();
            canvas.RotateDegrees(45f);
            canvas.DrawLine(0, -1, 0, 50, MainPage.lightGrayStroke);
            canvas.Restore();
        }

        private void backView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear(SKColors.Transparent);

            int width = e.Info.Width;
            int height = e.Info.Height;

            //Anpassen der Breite und Höhe an den Bildschirm = Transform
            canvas.Translate(width / 2, height / 2);
            canvas.Scale(width / 100f);

            //oberer Teil des 1. Pfeils
            canvas.Save();
            canvas.RotateDegrees(45f);
            canvas.DrawLine(0, 1, 0, -50, MainPage.thickGrayStroke);
            canvas.Restore();

            //unterer Teil des 1. Pfeils
            canvas.Save();
            canvas.RotateDegrees(-45f);
            canvas.DrawLine(0, -1, 0, 50, MainPage.thickGrayStroke);
            canvas.Restore();
        }
    }
}
