using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

using System.Threading;
using System.Threading.Tasks;
using Windows.UI;
using SnapsLibrary;
using Windows.UI.Input;

namespace XAMLSnaps
{
    public class TouchInputCanvas : Canvas, IClearableDisplayElement
    {
        SnapsManager manager;

        public TouchInputCanvas(SnapsManager manager)
        {
            this.manager = manager;
            this.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.VerticalAlignment = VerticalAlignment.Stretch;
            Color background = Colors.Orange;
            background.A = 0;
            this.Background = new SolidColorBrush(background);
            this.Visibility = Visibility.Collapsed;
        }

        void enableTouch()
        {
            if (this.Visibility == Visibility.Visible)
                return;
            this.Visibility = Visibility.Visible;
        }

        void disableTouch()
        {
            if (this.Visibility == Visibility.Collapsed)
                return;
            this.Visibility = Visibility.Collapsed;
        }

        public async Task<SnapsCoordinate> GetTappedCoordinateAsync()
        {
            enableTouch();

            SnapsCoordinate tappedPositionResult = new SnapsCoordinate();

            AutoResetEvent GetTappedPositionCompleteEvent = new AutoResetEvent(false);

            var tcs = new TaskCompletionSource<object>();
            TappedEventHandler lambda = (s, e) =>
            {
                // Get the position relative to the graphics canvas as this 
                // is the one we will be drawing 
                Point p = e.GetPosition(manager.DisplayGrid);//   (Grid) s);
                tappedPositionResult.XValue = (int)Math.Round(p.X);
                tappedPositionResult.YValue = (int)Math.Round(p.Y);
                tcs.TrySetResult(null);
            };
            try
            {
                this.Tapped += lambda;
                await tcs.Task;
            }
            finally
            {
                this.Tapped -= lambda;
                disableTouch();
            }
            return tappedPositionResult;
        }

        public SnapsCoordinate GetTappedCoordinate()
        {
            SnapsCoordinate tappedPositionResult = new SnapsCoordinate();
            AutoResetEvent GetTappedCompleteEvent = new AutoResetEvent(false);

            manager.InvokeOnUIThread(
                    async () =>
                    {
                        tappedPositionResult = await GetTappedCoordinateAsync();
                        GetTappedCompleteEvent.Set();
                    });

            GetTappedCompleteEvent.WaitOne();
            return tappedPositionResult;
        }

        public async Task<SnapsCoordinate> GetDraggedCoordinateAsync()
        {
            enableTouch();

            SnapsCoordinate draggedPositionResult = new SnapsCoordinate();

            AutoResetEvent GetDraggedPositionCompleteEvent = new AutoResetEvent(false);

            var tcs = new TaskCompletionSource<object>();

            TappedEventHandler pointerTappedHandler = (s, e) =>
            {
                // Get the position relative to the graphics canvas as this 
                // is the one we will be drawing 
                Point p = e.GetPosition(manager.DisplayGrid);//   (Grid) s);
                draggedPositionResult.XValue = (int)Math.Round(p.X);
                draggedPositionResult.YValue = (int)Math.Round(p.Y);
                tcs.TrySetResult(null);
            };


            PointerEventHandler pointerMovedHandler = (s, e) =>
            {
                bool doDraw = false;

                // Get the position relative to the graphics canvas as this 
                // is the one we will be drawing 
                PointerPoint p = e.GetCurrentPoint(this);//   (Grid) s);

                switch (p.PointerDevice.PointerDeviceType)
                {
                    case Windows.Devices.Input.PointerDeviceType.Mouse:

                        if (p.Properties.IsLeftButtonPressed)
                            doDraw = true;
                        break;

                    case Windows.Devices.Input.PointerDeviceType.Pen:
                        if (p.Properties.Pressure > 0)
                            doDraw = true;
                        break;

                    case Windows.Devices.Input.PointerDeviceType.Touch:
                        doDraw = true;
                        break;

                }

                if (doDraw)
                {
                    draggedPositionResult.XValue = (int)Math.Round(p.RawPosition.X);
                    draggedPositionResult.YValue = (int)Math.Round(p.RawPosition.Y);
                    tcs.TrySetResult(null);
                }
            };

            try
            {
                this.Tapped += pointerTappedHandler;
                this.PointerMoved += pointerMovedHandler;
                await tcs.Task;
            }
            finally
            {
                this.Tapped -= pointerTappedHandler;
                this.PointerMoved -= pointerMovedHandler;
                disableTouch();
            }
            return draggedPositionResult;
        }

        public SnapsCoordinate GetDraggedCoordinate()
        {
            SnapsCoordinate draggedResult = new SnapsCoordinate();

            AutoResetEvent GetDraggedPositionCompleteEvent = new AutoResetEvent(false);

            manager.InvokeOnUIThread(
                    async () =>
                    {
                        draggedResult = await GetDraggedCoordinateAsync();
                        GetDraggedPositionCompleteEvent.Set();
                    });

            GetDraggedPositionCompleteEvent.WaitOne();
            return draggedResult;
        }


        public void Clear()
        {
        }
    }
}