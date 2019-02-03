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
    public class DisplayTextBlock : StackPanel, IClearableDisplayElement
    {
        TextBlock displayTextBlock = null;
        Brush originalTextColor;
        SnapsManager manager;

        private double defaultFontSize = 20;
        private double defaultWidth = 400;

        public DisplayTextBlock(SnapsManager manager)
        {
            this.manager = manager;
            this.HorizontalAlignment = HorizontalAlignment.Stretch;
            displayTextBlock = new TextBlock();
            displayTextBlock.HorizontalAlignment = HorizontalAlignment.Stretch;
            displayTextBlock.Width = double.NaN;
            displayTextBlock.TextAlignment = TextAlignment.Left;
            displayTextBlock.TextWrapping = TextWrapping.WrapWholeWords;
            displayTextBlock.Width = defaultWidth;
            displayTextBlock.Margin = new Thickness(10, 10, 10, 10);
            displayTextBlock.Name = "writeOutputTextBlock";
            originalTextColor = displayTextBlock.Foreground;
            displayTextBlock.FontSize = defaultFontSize;
            displayTextBlock.Text = "";

            this.Children.Add(displayTextBlock);
        }

        #region String Display

        public void SetDisplayStringWidth(double width)
        {
            manager.InvokeOnUIThread(
                () =>
                {
                    displayTextBlock.Width = width;
                });
        }

        public void SetDisplayStringSize(double size)
        {
            fontSize = size;
            manager.InvokeOnUIThread(
                () =>
                {
                    displayTextBlock.FontSize = size;
                });
        }

        private double fontSize = 20;

        public void DoDisplayString(string message, SnapsTextAlignment alignment, SnapsFadeType fadeType)
        {
            AutoResetEvent displayDone = new AutoResetEvent(false);

            if (fadeType == SnapsFadeType.nofade)
            {
                // No fade - just display the new txt
                manager.InvokeOnUIThread(
                    () =>
                    {
                        displayTextBlock.Text = message;
                        displayTextBlock.FontSize = fontSize;
                        switch (alignment)
                        {
                            case SnapsTextAlignment.centre:
                                displayTextBlock.TextAlignment = TextAlignment.Center;
                                break;
                            case SnapsTextAlignment.left:
                                displayTextBlock.TextAlignment = TextAlignment.Left;
                                break;
                            case SnapsTextAlignment.right:
                                displayTextBlock.TextAlignment = TextAlignment.Right;
                                break;
                            case SnapsTextAlignment.justify:
                                displayTextBlock.TextAlignment = TextAlignment.Justify;
                                break;
                        }
                        displayDone.Set();
                    }
                );
            }
            else
            {
                TimeSpan fadeSpeed;
                if (fadeType == SnapsFadeType.fast)
                    fadeSpeed = new TimeSpan(0, 0, 0, 0, 200);
                else
                    fadeSpeed = new TimeSpan(0, 0, 0, 0, 600);

                manager.InvokeOnUIThread(
                    async () =>
                    {
                        if (displayTextBlock.Opacity == 1)
                        {
                            await FadeElements.FadeElementOpacityAsync(displayTextBlock, 1, 0, new TimeSpan(0, 0, 0, 0, 200));
                        }
                        displayTextBlock.Text = message;
                        displayTextBlock.FontSize = fontSize;
                        switch (alignment)
                        {
                            case SnapsTextAlignment.centre:
                                displayTextBlock.TextAlignment = TextAlignment.Center;
                                break;
                            case SnapsTextAlignment.left:
                                displayTextBlock.TextAlignment = TextAlignment.Left;
                                break;
                            case SnapsTextAlignment.right:
                                displayTextBlock.TextAlignment = TextAlignment.Right;
                                break;
                            case SnapsTextAlignment.justify:
                                displayTextBlock.TextAlignment = TextAlignment.Justify;
                                break;
                        }
                        await FadeElements.FadeElementOpacityAsync(displayTextBlock, 0, 1, new TimeSpan(0, 0, 0, 0, 200));
                        displayDone.Set();
                    }
                );
            }

            displayDone.WaitOne();
        }

        public void DoDisplayString(string message)
        {
            DoDisplayString(message, SnapsTextAlignment.centre, SnapsFadeType.fast);
        }

        public void DoDisplayString(string message, SnapsTextAlignment alignment, SnapsFadeType fadeType, double size)
        {
            fontSize = size;
            DoDisplayString(message, alignment, fadeType);
        }

        public void DoSetTextColor(int red, int green, int blue)
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
                    displayTextBlock.Foreground = brush;
                }
            );
        }

        public void DoSetTextColor(SnapsColor color)
        {
            DoSetTextColor(color.RedValue, color.GreenValue, color.BlueValue);
        }

        #endregion


        public void Clear()
        {
            displayTextBlock.Text = "";
            displayTextBlock.Foreground = originalTextColor;
            displayTextBlock.Width = defaultWidth;
            displayTextBlock.FontSize = defaultFontSize;
            fontSize = defaultFontSize;
        }
    }
}
