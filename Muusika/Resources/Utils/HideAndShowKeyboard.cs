using Android.Content;
using Android.Views;
using Android.Views.InputMethods;

namespace Muusika.Resources.Utils
{
    class HideAndShowKeyboard
    {
        //From https://gist.github.com/icalderond/742f98f2f8cda1fae1b0bc877df76bbc

        /**
         * Shows the soft keyboard
         */
        public void showSoftKeyboard(Android.App.Activity activity, View view)
        {
            InputMethodManager inputMethodManager = (InputMethodManager)activity.GetSystemService(Context.InputMethodService);
            view.RequestFocus();
            inputMethodManager.ShowSoftInput(view, 0);
            inputMethodManager.ToggleSoftInput(ShowFlags.Forced, HideSoftInputFlags.ImplicitOnly);//personal line added
        }

        /**
         * Hides the soft keyboard
         */
        public void hideSoftKeyboard(Android.App.Activity activity)
        {
            var currentFocus = activity.CurrentFocus;
            if (currentFocus != null)
            {
                InputMethodManager inputMethodManager = (InputMethodManager)activity.GetSystemService(Context.InputMethodService);
                inputMethodManager.HideSoftInputFromWindow(currentFocus.WindowToken, HideSoftInputFlags.None);
            }
        }
    }
}