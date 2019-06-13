using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.Timers;

namespace Zewis
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class MainPage : ContentPage
    {
        //Monat
        private int month;
        //Jahr
        private int year;
        //Anzahl der Tage eines Monats
        private int daysOfMonth;
        //Gestenerkennung
        private SwipeGestureRecognizer leftSwipeGesture;
        private SwipeGestureRecognizer rightSwipeGesture;

        //Navigations-Stack
        public NavigableElement navigation;

        //Farbe zum Zeichnen der Pfeile
        public static SKPaint thickGrayStroke = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Gray,
            IsAntialias = true,
            StrokeWidth = 5
        };

        //Farbe zum Zeichnen der Pfeile
        public static SKPaint grayStroke = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Gray,
            IsAntialias = true,
            StrokeWidth = 2
        };

        //Andere Farbe zum Zeichnen der Pfeile
        public static SKPaint lightGrayStroke = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.LightGray,
            IsAntialias = true,
            StrokeWidth = 2
        };

        //Konstruktor
        public MainPage()
        {
            //Standard...
            InitializeComponent();
            //Holen des Monats des jetzigen Datums
            month = DateTime.Today.Month;
            //Holen des Jahres des jetzigen Datums
            year = DateTime.Today.Year;
            //Holen der Anzahl der Tage des Monats
            daysOfMonth = DateTime.DaysInMonth(year, month);

            //Gestenerkennung erzeugen
            createGestures();
            //Seite erzeugen
            createPage();
        }

        //Konstruktor mit Datum als Übergabefeld
        public MainPage(DateTime date)
        {
            InitializeComponent();
            month = date.Month;
            year = date.Year;
            daysOfMonth = DateTime.DaysInMonth(year, month);

            createGestures();
            createPage();
        }

        //Methode zum Zeichnen der ...
        private void canvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear(SKColors.Transparent);

            int width = e.Info.Width;
            int height = e.Info.Height;

            //Transform
            canvas.Translate(width / 16, height / 2);
            canvas.Scale(width / 100f);

            //Draw line 
            canvas.Save();
            canvas.RotateDegrees(45f);
            canvas.DrawLine(0, 1, 0, -50, grayStroke);
            canvas.Restore();

            //Draw 2 line
            canvas.Save();
            canvas.RotateDegrees(-45f);
            canvas.DrawLine(0, -1, 0, 50, grayStroke);
            canvas.Restore();

            canvas.ResetMatrix();

            //Transform
            canvas.Translate(width / 8, height / 2);
            canvas.Scale(width / 100f);

            //Draw line 
            canvas.Save();
            canvas.RotateDegrees(45f);
            canvas.DrawLine(0, 1, 0, -50, lightGrayStroke);
            canvas.Restore();

            //Draw 2 line
            canvas.Save();
            canvas.RotateDegrees(-45f);
            canvas.DrawLine(0, -1, 0, 50, lightGrayStroke);
            canvas.Restore();


            canvas.ResetMatrix();
            //RIGHT SIDE
            //Transform
            canvas.Translate(width - (width / 16), height / 2);
            canvas.Scale(width / 100f);

            //Draw line 
            canvas.Save();
            canvas.RotateDegrees(-45f);
            canvas.DrawLine(0, 1, 0, -50, grayStroke);
            canvas.Restore();

            //Draw 2 line
            canvas.Save();
            canvas.RotateDegrees(45f);
            canvas.DrawLine(0, -1, 0, 50, grayStroke);
            canvas.Restore();

            canvas.ResetMatrix();

            //Transform
            canvas.Translate(width - (width / 8), height / 2);
            canvas.Scale(width / 100f);

            //Draw line 
            canvas.Save();
            canvas.RotateDegrees(-45f);
            canvas.DrawLine(0, 1, 0, -50, lightGrayStroke);
            canvas.Restore();

            //Draw 2 line
            canvas.Save();
            canvas.RotateDegrees(45f);
            canvas.DrawLine(0, -1, 0, 50, lightGrayStroke);
            canvas.Restore();
        }

        //Methode die durch Swipe aufgerufen wird (Event Listener)
        public void OnTableSwiped(object sender, SwipedEventArgs e)
        {
            //Anfangsdatum der jetzigen Hauptseite holen
            var date = DateTime.Parse(ConvertSingleInt(1) + "/" + ConvertSingleInt(month) + "/" + year + " 00:00:00", App.culture);
            //Anfangsdatum in Bezug auf den heutigen Monat holen
            var todaydate = DateTime.Parse(ConvertSingleInt(1) + "/" + ConvertSingleInt(DateTime.Today.Month) + "/" + DateTime.Today.Year + " 00:00:00", App.culture);
            switch (e.Direction)
            {
                case SwipeDirection.Left:
                    //Ein Monat hinzufügen
                    date = date.AddMonths(1);
                    //Erlaube nur einen Monat in die Zukunft zu gehen
                    if ( date <= todaydate.AddMonths(1) )
                    {
                        //Überschreiben der Hauptseite eine Navigation zurück ist nicht mehr möglich!
                        App.Current.MainPage = new MainPage(date);
                    }
                    break;
                case SwipeDirection.Right:
                    //Ein Monat abziehen
                    date = date.AddMonths(-1);
                    //Überschreiben der Hauptseite eine Navigation zurück ist nicht mehr möglich!
                    App.Current.MainPage = new MainPage(date);
                    
                    break;
                default:
                    break;
            }
        }

        // Gesten erzeugen siehe DAYPAGE
        private void createGestures()
        {
            leftSwipeGesture = new SwipeGestureRecognizer();
            rightSwipeGesture = new SwipeGestureRecognizer();
            leftSwipeGesture.Swiped += OnTableSwiped;
            leftSwipeGesture.Direction = SwipeDirection.Left;
            rightSwipeGesture.Swiped += OnTableSwiped;
            rightSwipeGesture.Direction = SwipeDirection.Right;
        }

        private void createPage()
        {
            //Anfangsdatum des Monats der HAuptseite holen
            var firstdate = DateTime.Parse(ConvertSingleInt(1) + "/" + ConvertSingleInt(month) + "/" + year + " 00:00:00", App.culture);
            //Liste von Tagen erstellen
            var days = new List<ViewCell>();
            //Gesamtsumme der Stunden erstellen
            var sumOverall = 0.0;
            //Über alle Tage ilterieren
            for (int i = 0; i < daysOfMonth; i++)
            {
                //Tagesdatum des Schleifendurchlaufs erzeugen
                var daydate = DateTime.Parse(ConvertSingleInt(i + 1) + "/" + ConvertSingleInt(month) + "/" + year + " 00:00:00", App.culture);
                //Layout
                var layout = new SpecialLayout() { Orientation = StackOrientation.Horizontal, date = daydate, navigation = this.Navigation };
                //Alle Arbeitszeiten für den Tag des Schleifendurchlaufs holen 
                var dayTimeRecordings = App.Database.GetTimeRecordingAsync(daydate).Result;
                //Liste der Projektnamen des Tages erstellen.
                List<string> allProjects = new List<string>();
                var projects = "";
                var sum = 0.0;
                foreach (var item in dayTimeRecordings)
                {
                    //Projektnamen zur Liste hinzufügen
                    allProjects.Add(item.ProjectName);
                    //Stunden der Projekte summieren
                    sum += item.Time;
                }
                //Projeknamen formatieren
                projects = string.Join(",", allProjects.ToArray());

                layout.Children.Add(new SpecialLabel()
                {
                    date = daydate,
                    Text = "" + (i + 1),
                    TextColor = Color.FromHex("#000000"),
                    VerticalOptions = LayoutOptions.Center
                });
                layout.Children.Add(new Label()
                {
                    //Projektnamen mit Komma getrennt anzeigen
                    Text = projects,

                    TextColor = Color.FromHex("#000000"),
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.CenterAndExpand
                });
                layout.Children.Add(new Label()
                {
                    //Gesamtstunden des Tages anzeigen
                    Text = sum.ToString(),
                    TextColor = Color.FromHex("#000000"),
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.End
                });

                //Gestenerkennung hinzufügen
                layout.addGestureRecognizer();
                //Einzel Tag formatiert erzeugen
                var day = new ViewCell() { View = layout };
                //Tag zur Tabelle hinzufügen
                days.Add(day);
                //Summe des einzelnen Tags zur Gesamtsumme des Monats hinzuzählen
                sumOverall += sum;
            }
            //Alle Tage zur Anzeigetabelle hinzufügen
            this.FindByName<TableSection>("az").Add(days);
            //Monatsnamen + Jahr formatiert ausgeben
            m.Text = App.culture.DateTimeFormat.GetMonthName(month) + " " + year;
            //Sollstunden anzeigen
            soll.Text += getSoll(firstdate).ToString();
            //Ist Stunden anzeigen
            ist.Text += sumOverall.ToString();
            //Saldo anzeigen (Soll Stunden - Ist Stunden)
            saldo.Text += (sumOverall - getSoll(firstdate)).ToString();
            //Gestenerkennung hinzufügen
            canvasView.GestureRecognizers.Add(leftSwipeGesture);
            canvasView.GestureRecognizers.Add(rightSwipeGesture);
        }

        //Hilfsmethode zum erstellen von 2 stelligen zahlen für 1 z.B 01
        public static string ConvertSingleInt(int i)
        {
            if (i <= 9)
            {
                return "0" + i;
            }
            else
            {
                return i + "";
            }
        }

        //Hilfsmethode zum Berechnen der Soll Stunden 
        public static int getSoll(DateTime date)
        {
            //Anfangsdatum des Monats der Hauptseite
            var localdate = date;
            //Anzahl der Arbeitstage
            int workingDays = 0;
            //Anzahl der Tage des Monats
            var days = DateTime.DaysInMonth(localdate.Year, localdate.Month);
            for (int i = 0; i < days; i++)
            {
                //Alle Wochentage werden als Arbeitstage gezählt
                if (!(App.culture.DateTimeFormat.GetDayName(localdate.DayOfWeek) == "Samstag" || App.culture.DateTimeFormat.GetDayName(localdate.DayOfWeek) == "Sonntag"))
                {
                    workingDays += 1;
                }
                localdate = localdate.AddDays(1);
            }
            //Alle Arbeitstage werden mit 8 addiert (wegen 8 Stunden festgelegte Arbeitszeit - ausgehend von einer 40Stunden Woche)
            return workingDays * 8;
        }
    }
}
