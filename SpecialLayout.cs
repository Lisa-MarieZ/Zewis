using System;
using Xamarin.Forms;

namespace Zewis
{
    //Siehe SpecialLabel
    public class SpecialLayout : StackLayout
    {
        public TapGestureRecognizer tapGestureRecognizer;
        public DateTime date;
        public INavigation navigation;

        public SpecialLayout()
        {
        }
        public void addGestureRecognizer()
        {
            tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += Handle_Tap;
            this.GestureRecognizers.Add(tapGestureRecognizer);
        }
        public void addSwipeGestureRecognizer(SwipeGestureRecognizer gestureRecognizer)
        {
            this.GestureRecognizers.Add(gestureRecognizer);
        }
        void Handle_Tap(object sender, EventArgs eventArgs)
        {
            navigation.PushModalAsync(new DayPage(date));
            System.Diagnostics.Debug.WriteLine(Navigation.ModalStack.ToString());
            System.Diagnostics.Debug.WriteLine("Tap on " + date.ToShortDateString());
        }
    }
}
