using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using System.Threading;
using Windows.UI;
using Windows.UI.Xaml.Shapes;
using SnapsLibrary;
using System.Collections.Generic;
using System.Text;

namespace XAMLSnaps
{
    public class SnapsGraphicsCanvas : Canvas, IClearableDisplayElement
    {
        static int MaxNoOfGraphics = 3000;

        SnapsManager manager;

        private Image displayImageValue;

        public Image DisplayImage
        {
            get
            {
                return displayImageValue;
            }
        }

        private Image backgroundImageValue;

        public Image BackgroundImage
        {
            get
            {
                return backgroundImageValue;
            }
        }

        public SnapsGraphicsCanvas (SnapsManager manager)
        {
            this.manager = manager;

            backgroundImageValue = new Image();
            backgroundImageValue.HorizontalAlignment = HorizontalAlignment.Center;
            backgroundImageValue.VerticalAlignment = VerticalAlignment.Center;
            backgroundImageValue.Stretch = Stretch.UniformToFill;
            Canvas.SetTop(backgroundImageValue, 0);
            Canvas.SetLeft(backgroundImageValue, 0);
            Children.Add(backgroundImageValue);

            displayImageValue = new Image();
            displayImageValue.HorizontalAlignment = HorizontalAlignment.Center;
            displayImageValue.VerticalAlignment = VerticalAlignment.Center;
            displayImageValue.Stretch = Stretch.Uniform;
            Canvas.SetTop(displayImageValue, 0);
            Canvas.SetLeft(displayImageValue, 0);
            Children.Add(displayImageValue);
        }

        List<UIElement> activeElements = new List<UIElement>();

        /// <summary>
        /// Adds an item to the screen. Makes sure we never draw more than
        /// the maximum number of items.
        /// </summary>
        /// <param name="element">item to add</param>
        void drawItem(UIElement element)
        {
            if (Children.Count == MaxNoOfGraphics)
            {
                // remove the oldest graphics item
                UIElement oldest = activeElements[0];
                activeElements.RemoveAt(0);
                Children.Remove(oldest);
            }
            // draw this element
            Children.Add(element);
            // Add to the list of active elements - for removal later
            activeElements.Add(element);
        }

        public void SetBackgroundColor(int red, int green, int blue)
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
                    Background = brush;
                }
            );
        }

        public void SetBackgroundColor(SnapsColor color)
        {
            SetBackgroundColor(color.RedValue, color.GreenValue, color.BlueValue);
        }

        public void ClearGraphics()
        {
            AutoResetEvent DisplayComplete = new AutoResetEvent(false);

            manager.InvokeOnUIThread(
                async () =>
                {
                    if (this.Opacity == 1)
                    {
                        await FadeElements.FadeElementOpacityAsync(this, 1, 0, new TimeSpan(0, 0, 1));
                    }
                    Clear();
                    await FadeElements.FadeElementOpacityAsync(this, 0, 1, new TimeSpan(0,0,0,0,10));
                    DisplayComplete.Set();
                }
            );

            DisplayComplete.WaitOne();
        }

        public void SetDrawingColor(SnapsColor color)
        {
            SetDrawingColor(color.RedValue, color.GreenValue, color.BlueValue);
        }

        public void SetDrawingColor(int red, int green, int blue)
        {
            manager.InvokeOnUIThread(
                () =>
                {
                    Color newBackground = Color.FromArgb(
                        255,
                        SnapsManager.ColorClamp(red),
                        SnapsManager.ColorClamp(green),
                        SnapsManager.ColorClamp(blue));
                    graphicsBrush = new SolidColorBrush(newBackground);
                }
            );
        }

        Brush graphicsBrush = new SolidColorBrush(Colors.Gray);
        int lineThickness = 2;

        public void DrawLine(int x1, int y1, int x2, int y2)
        {
            AutoResetEvent drawLineCompletedEvent = new AutoResetEvent(false);

            manager.InvokeOnUIThread(
                () =>
                {
                    Line l = new Line();
                    l.Stroke = graphicsBrush;
                    l.StrokeThickness = lineThickness;
                    l.X1 = x1;
                    l.Y1 = y1;
                    l.X2 = x2;
                    l.Y2 = y2;
                    drawItem(l);
                    drawLineCompletedEvent.Set();
                }
            );
            drawLineCompletedEvent.WaitOne();
        }

        public void DrawDot(int x, int y, int width)
        {
            AutoResetEvent drawCircleCompletedEvent = new AutoResetEvent(false);

            manager.InvokeOnUIThread(
                () =>
                {
                    Ellipse e = new Ellipse();
                    e.Stroke = graphicsBrush;
                    e.Fill = graphicsBrush;
                    e.Width = width;
                    e.Height = width;
                    Canvas.SetLeft(e, x - (width / 2));
                    Canvas.SetTop(e, y - (width / 2));
                    drawItem(e);
                    drawCircleCompletedEvent.Set();
                }
            );
            drawCircleCompletedEvent.WaitOne();
        }

        public void DrawBlock(int x, int y, int width, int height)
        {
            AutoResetEvent drawBlockCompletedEvent = new AutoResetEvent(false);

            manager.InvokeOnUIThread(
                () =>
                {
                    Rectangle e = new Rectangle();
                    e.Stroke = graphicsBrush;
                    e.Fill = graphicsBrush;
                    e.Width = width;
                    e.Height = height;
                    Canvas.SetLeft(e, x);
                    Canvas.SetTop(e, y);
                    drawItem(e);
                    drawBlockCompletedEvent.Set();
                }
            );
            drawBlockCompletedEvent.WaitOne();
        }

        public SnapsCoordinate GetScreenSize()
        {
            AutoResetEvent getSizeCompletedEvent = new AutoResetEvent(false);
            SnapsCoordinate position = new SnapsCoordinate();

            manager.InvokeOnUIThread(
                () =>
                {
                    position.XValue = (int)this.ActualWidth;
                    position.YValue = (int)this.ActualHeight;
                    getSizeCompletedEvent.Set();
                }
            );

            getSizeCompletedEvent.WaitOne();

            return position;
        }

        #region Light Panel

        Rectangle[,] LightPanel = null;

        private static int maxPanelSize = 32;

        public void MakeLightPanel(int xCells, int yCells)
        {
            StringBuilder error = new StringBuilder();

            if (xCells < 1)
            {
                error.AppendLine("xCells value must be greater than 0");
            }
            else
            {
                if (xCells > maxPanelSize)
                {
                    error.AppendLine("xCells value must be less than " + maxPanelSize);
                }
            }

            if (yCells < 1)
            {
                error.AppendLine("yCells value must be greater than 0");
            }
            else
            {
                if (yCells > maxPanelSize)
                {
                    error.AppendLine("yCells value must be less than " + maxPanelSize);
                }
            }

            string errorString = error.ToString();

            if(errorString.Length>0)
                throw new Exception(errorString);

            AutoResetEvent lightPanelBuiltEvent = new AutoResetEvent(false);

            manager.InvokeOnUIThread(
                async () =>
                {
                    if (this.Opacity != 1)
                        await FadeElements.FadeElementOpacityAsync(this, 0, 1, new TimeSpan(0, 0, 1));

                    if (LightPanel != null)
                    {
                        // remove the previous one
                        for (int x = 0; x < LightPanel.GetLength(0); x++)
                        {
                            for (int y = 0; y < LightPanel.GetLength(1); y++)
                            {
                                Rectangle r = LightPanel[x, y];
                                this.Children.Remove(r);
                            }
                        }
                    }

                    LightPanel = new Rectangle[xCells, yCells];

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
                            LightPanel[x, y] = p;
                            this.Children.Add(p);
                        }
                    }

                    lightPanelBuiltEvent.Set();
                });

            lightPanelBuiltEvent.WaitOne();
        }

        public void SetPanelCell(int x, int y, int red, int green, int blue)
        {
            if (LightPanel == null)
                throw new Exception("No LightPanel has been created");

            AutoResetEvent setPanelCompletedEvent = new AutoResetEvent(false);

            manager.InvokeOnUIThread(
                () =>
                {
                    if (x < 0) x = 0;
                    if (x >= LightPanel.GetLength(0)) x = LightPanel.GetLength(0) - 1;
                    if (y < 0) y = 0;
                    if (y >= LightPanel.GetLength(1)) y = LightPanel.GetLength(1) - 1;

                    Color color = Color.FromArgb(
                        255,
                        SnapsManager.ColorClamp(red),
                        SnapsManager.ColorClamp(green),
                        SnapsManager.ColorClamp(blue));

                    Brush b = new SolidColorBrush(color);
                    LightPanel[x, y].Fill = b;
                    //LightPanel[x, y].Stroke = b;
                    setPanelCompletedEvent.Set();

                });
            setPanelCompletedEvent.WaitOne();
        }

        public int LightPanelWidth
        {
            get
            {
                if (LightPanel == null)
                    return 0;
                else
                    return LightPanel.GetLength(0);
            }
        }

        enum PenModes
        {
            roundPen,
            squarePen,
            erasePen
        };

        public int LightPanelHeight
        {
            get
            {
                if (LightPanel == null)
                    return 0;
                else
                    return LightPanel.GetLength(1);
            }

        }

        #endregion

        #region Tests

        public void RedBackground()
        {
            manager.SetTitleString("Red Background");
            manager.SetBackgroundColor(255, 0, 0);
        }

        public void RandomLines()
        {
            manager.SetTitleString("Random Lines");
            System.Random rand = new System.Random();
            manager.SetTitleString("50 Random Lines");
            for (int i = 0; i < 50; i++)
            {
                manager.SetDrawingColor(rand.Next(255), rand.Next(255), rand.Next(255));
                manager.DrawLine(rand.Next(400), rand.Next(400), rand.Next(400), rand.Next(400));
                manager.Delay(0.1f);
            }
        }

        public void MatrixPanel()
        {
            manager.SetTitleString("Dot Matix Panel");
            System.Random rand = new System.Random();
            manager.MakeLightPanel(32, 32);
            for (int i = 0; i < 500; i++)
            {
                manager.SetPanelCell(rand.Next(32), rand.Next(32), rand.Next(255), rand.Next(255), rand.Next(255));
            }
        }

        #endregion

        public bool SaveGraphicsImageToFileAsPNG()
        {
            return manager.SaveVisualElementToFileAsPNG(manager.DisplayGrid);
        }

        public void Clear()
        {
            Children.Clear();
            DisplayImage.Source = null;
            BackgroundImage.Source = null;
            Children.Add(backgroundImageValue);
            Children.Add(displayImageValue);
            Background = manager.BackgroundBrush;
        }
    }
}
