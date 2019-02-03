using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace XAMLSnaps
{
    public class TapDetection
    {
        protected SnapsManager manager;

        private bool TappedFlag;

        TappedEventHandler TappedHandler = null;
        PointerEventHandler PointHandler = null;

        public TapDetection(SnapsManager manager)
        {
            this.manager = manager;
        }

        void bindTappedHandlers()
        {
            // Tapped and point handlers are always managed together
            // So if one is set the other is also set. 

            if (TappedHandler != null)
                return;

            // Create the handlers

            TappedHandler = (s, e) =>
            {
                TappedFlag = true;
                e.Handled = false;
            };

            PointHandler = (s, e) =>
            {
                TappedFlag = true;
                e.Handled = false;
            };

            // Bind them - this must happen on the UI thread

            manager.InvokeOnUIThread(
                    () =>
                    {
                        manager.DisplayGrid.Tapped += TappedHandler;
                        manager.DisplayGrid.PointerPressed += PointHandler;
                    });
        }

        void unBindTappedHandlers()
        {
            // Tapped and point handlers are always managed together
            // So if one is set the other is also set. 

            if (TappedHandler == null)
                return;

            manager.InvokeOnUIThread(
                    () =>
                    {
                        if (TappedHandler != null)
                        {
                            manager.DisplayGrid.Tapped -= TappedHandler;
                            TappedHandler = null;
                        }
                        if (PointHandler != null)
                        {
                            manager.DisplayGrid.PointerPressed -= PointHandler;
                            PointHandler = null;
                        }
                    });
        }

        // Implementation of Snaps behaviours

        /// <summary>
        /// Resets the tapped flag. Also binds the handlers if required
        /// </summary>

        public void ClearScreenTappedFlag()
        {
            bindTappedHandlers();
            TappedFlag = false;
        }

        /// <summary>
        /// Tests to see if the screen has been tapped. 
        /// </summary>
        /// <returns>true if the screen has been tapped.
        /// The method will throw an exception if ResetTappedFlag has not been called before we try to read the tapped state.</returns>
        public bool ScreenHasBeenTapped()
        {
            bindTappedHandlers();
            return TappedFlag;
        }
    }
}
