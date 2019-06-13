using System;
using Xamarin.Forms;

namespace Zewis
{
    //Spezielle Klasse für Textanzeige
    public class SpecialLabel : Label
    {
        //Gestenerkennung
        public TapGestureRecognizer gesture;
        //Datum
        public DateTime date; 

        public SpecialLabel()
        {
            //Hinzufügen der Gestenerkennung
            gesture = new TapGestureRecognizer();
            this.GestureRecognizers.Add(gesture);
            gesture.Tapped += (s, e) =>
            {
                //Bei Tap auf den Tag wird die Tagesansicht geöffnet
                Navigation.PushModalAsync(new DayPage(date));
            };
        }
    }
}
