using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI;
using SnapsLibrary;

namespace XAMLSnaps
{
    public class ButtonPanel : StackPanel, IClearableDisplayElement
    {
        const int MAX_NO_OF_BUTTONS = 10;

        SnapsManager manager;

        public ButtonPanel(SnapsManager manager)
        {
            this.manager = manager;
            for (int i = 0; i < MAX_NO_OF_BUTTONS; i++)
            {
                Button b = new Button();
                b.Visibility = Visibility.Collapsed;
                b.HorizontalAlignment = HorizontalAlignment.Stretch;
                b.Width = 400;
                b.Margin = new Thickness(4);
                this.Children.Add(b);
            }
        }

        public async Task<string> SelectButtonAsync(string[] buttonTexts)
        {
            string result = "";

            var tcs = new TaskCompletionSource<object>();
            RoutedEventHandler lambda = async (s, e) =>
            {
                Button source = (Button)e.OriginalSource;
                result = (string)source.Content;
                await FadeElements.FadeElementOpacityAsync(this, 1, 0, new TimeSpan(0, 0, 0, 0, 50));
                tcs.TrySetResult(null);
            };
            try
            {
                // First time we are called we need to make the buttons

                int buttonCount = 0;

                foreach (Button b in this.Children)
                {
                    if (buttonCount < buttonTexts.Length)
                    {
                        // got text for this button
                        b.Content = buttonTexts[buttonCount];
                        b.Visibility = Visibility.Visible;
                        b.Click += lambda;
                    }
                    else
                    {
                        b.Visibility = Visibility.Collapsed;
                    }
                    buttonCount++;
                }

                await FadeElements.FadeElementOpacityAsync(this, 0, 1, new TimeSpan(0, 0, 0, 0, 50));
                await tcs.Task;
            }
            finally
            {
                int buttonCount = 0;

                foreach (Button b in this.Children)
                {
                    if (buttonCount < buttonTexts.Length)
                    {
                        b.Visibility = Visibility.Collapsed;
                        b.Click -= lambda;
                    }
                    buttonCount++;
                }
            }

            return result;
        }

        #region Button selection bindings

        public string DoSelectFromButtonArray(string[] buttonTexts)
        {
            if (buttonTexts.Length > MAX_NO_OF_BUTTONS)
            {
                throw new Exception("Too many buttons in call to SelectButton");
            }

            AutoResetEvent gotTextEvent = new AutoResetEvent(false);
            string result = "";
            manager.InvokeOnUIThread(
                async () =>
                {
                    result = await SelectButtonAsync(buttonTexts);
                    gotTextEvent.Set();
                }
            );

            gotTextEvent.WaitOne();

            return result;
        }

        public string DoWaitForButton(string prompt)
        {
            return DoSelectFromButtonArray(new string[] { prompt });
        }

        #endregion

        public void Clear()
        {
            foreach (Button b in this.Children)
            {
                b.Visibility = Visibility.Collapsed;
            }
        }

        public void DoSetButtonColor(int red, int green, int blue)
        {
            manager.InvokeOnUIThread(
                () =>
                {
                    Color newBackground = Color.FromArgb(
                        255,
                        SnapsManager.ColorClamp(red),
                        SnapsManager.ColorClamp(green),
                        SnapsManager.ColorClamp(blue));
                    SolidColorBrush brush = new SolidColorBrush(newBackground);
                    foreach (Button b in this.Children)
                    {
                        b.Foreground = brush;
                    }
                }
            );
        }

        public void DoSetButtonColor(SnapsColor color)
        {
            DoSetButtonColor(color.RedValue, color.GreenValue, color.BlueValue);
        }
    }
}
