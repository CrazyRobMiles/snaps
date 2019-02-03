using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using System.Threading;
using Windows.UI;
using Windows.UI.Xaml.Shapes;

namespace XAMLSnaps
{
    public class LightPanel : Canvas, IClearableDisplayElement
    {
        SnapsManager manager;

        Rectangle[,] lightPanels = null;

        public LightPanel(SnapsManager manager, double width, double height, int xCells, int yCells)
        {
            this.manager = manager;

            this.Width = width;
            this.Height = height;

            if (xCells > 32 || yCells > 32)
                throw new Exception("Maximum panel dimension exceeded. No more than 32 panels in each direction");

            AutoResetEvent lightPanelBuiltEvent = new AutoResetEvent(false);

            manager.InvokeOnUIThread(
                async () =>
                {
                    if (this.Opacity != 1)
                        await FadeElements.FadeElementOpacityAsync(this, 0, 1, new TimeSpan(0, 0, 1));

                    clearLightPanel();

                    lightPanels = new Rectangle[xCells, yCells];

                    float panelWidth = (float)this.ActualWidth / xCells;
                    float panelHeight = (float)this.ActualHeight / yCells;

                    for (int x = 0; x < xCells; x++)
                    {
                        for (int y = 0; y < yCells; y++)
                        {
                            Rectangle p = new Rectangle();
                            p.Fill = new SolidColorBrush(Colors.White);
                            p.Stroke = new SolidColorBrush(Colors.Black);
                            p.Width = panelWidth;
                            p.Height = panelHeight;
                            Canvas.SetLeft(p, x * panelWidth);
                            Canvas.SetTop(p, y * panelHeight);
                            lightPanels[x, y] = p;
                            this.Children.Add(p);
                        }
                    }

                    lightPanelBuiltEvent.Set();
                });

            lightPanelBuiltEvent.WaitOne();
        }

        void clearLightPanel()
        {
            if (lightPanels != null)
            {
                // remove the previous one
                for (int x = 0; x < lightPanels.GetLength(0); x++)
                {
                    for (int y = 0; y < lightPanels.GetLength(1); y++)
                    {
                        Rectangle r = lightPanels[x, y];
                        this.Children.Remove(r);
                    }
                }
            }
        }

        public void SetPanelCell(int x, int y, int red, int green, int blue)
        {
            if (lightPanels == null)
                throw new Exception("No LightPanel has been created");

            AutoResetEvent setPanelCompletedEvent = new AutoResetEvent(false);

            manager.InvokeOnUIThread(
                () =>
                {
                    if (x < 0) x = 0;
                    if (x >= lightPanels.GetLength(0)) x = lightPanels.GetLength(0) - 1;
                    if (y < 0) y = 0;
                    if (y >= lightPanels.GetLength(1)) y = lightPanels.GetLength(1) - 1;

                    Color color = Color.FromArgb(
                        255,
                        SnapsManager.ColorClamp(red),
                        SnapsManager.ColorClamp(green),
                        SnapsManager.ColorClamp(blue));

                    Brush b = new SolidColorBrush(color);
                    lightPanels[x, y].Fill = b;
                    //LightPanel[x, y].Stroke = b;
                    setPanelCompletedEvent.Set();

                });
            setPanelCompletedEvent.WaitOne();
        }

        public int LightPanelWidth
        {
            get
            {
                if (lightPanels == null)
                    return 0;
                else
                    return lightPanels.GetLength(0);
            }
        }

        public int LightPanelHeight
        {
            get
            {
                if (lightPanels == null)
                    return 0;
                else
                    return lightPanels.GetLength(1);
            }

        }

        public void Clear()
        {
            clearLightPanel();
        }
    }
}