using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI;
using SnapsLibrary;
using System.Collections.Generic;
using Windows.UI.Xaml.Shapes;
using Windows.System;

namespace XAMLSnaps
{
    public class Pad:Grid
    {
        public TextBlock PadTextBox;

        public bool IsDown = false;

        public string PadText
        {
            set
            {
                PadTextBox.Text = value;
            }
        }

        public Pad()
        {
            PadTextBox = new TextBlock();
            PadTextBox.TextAlignment = TextAlignment.Center;
            Children.Add(PadTextBox);
        }
    }

    public class JoyPadPanel : StackPanel, IClearableDisplayElement
    {
        SnapsManager manager;

        Dictionary<string, Pad> JoyPads = new Dictionary<string, Pad>();

        Pad up, down, left, right, fire;

        const int PAD_WIDTH = 50;
        const int PAD_HEIGHT = 20;
        Brush padBrush = new SolidColorBrush(Colors.Red);

        public JoyPadPanel(SnapsManager manager)
        {
            this.manager = manager;

            this.Width = 200;
            this.Background = new SolidColorBrush(Color.FromArgb(120, 100, 100, 100));

            up = new Pad();
            up.PadText = "UP";
            JoyPads.Add("UP",up);

            down = new Pad();
            down.PadText = "DOWN";
            JoyPads.Add("DOWN",down);

            left = new Pad();
            left.PadText = "LEFT";
            JoyPads.Add("LEFT",left);

            right = new Pad();
            right.PadText = "RIGHT";
            JoyPads.Add("RIGHT",right);

            fire = new Pad();
            fire.PadText = "FIRE";
            JoyPads.Add("FIRE",fire);

            foreach (Pad r in JoyPads.Values)
            {
                r.Width = PAD_WIDTH;
                r.Height = PAD_HEIGHT;
                r.Background = padBrush;
                r.HorizontalAlignment = HorizontalAlignment.Center;
                r.Margin = new Thickness(8, 8, 8, 8);
                r.PointerEntered += pad_pointerEntered;
                r.PointerExited += pad_pointerExited;
            }

            StackPanel topRow = new StackPanel();
            topRow.Children.Add(up);
            this.Children.Add(topRow);
            StackPanel middleRow = new StackPanel();
            middleRow.Orientation = Orientation.Horizontal;
            middleRow.Children.Add(left);
            middleRow.Children.Add(fire);
            middleRow.Children.Add(right);
            this.Children.Add(middleRow);
            StackPanel bottomRow = new StackPanel();
            bottomRow.Children.Add(down);
            this.Children.Add(bottomRow);
            this.HorizontalAlignment = HorizontalAlignment.Right;
            this.VerticalAlignment = VerticalAlignment.Bottom;

            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;
        }

        private void CoreWindow_KeyUp(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Left:
                    JoyPads["LEFT"].IsDown = false;
                    break;
                case VirtualKey.Right:
                    JoyPads["RIGHT"].IsDown = false;
                    break;
                case VirtualKey.Up:
                    JoyPads["UP"].IsDown = false;
                    break;
                case VirtualKey.Down:
                    JoyPads["DOWN"].IsDown = false;
                    break;
                case VirtualKey.Space:
                    JoyPads["FIRE"].IsDown = false;
                    break;
                default:
                    break;
            }
        }

        private void CoreWindow_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Left:
                    JoyPads["LEFT"].IsDown = true;
                    break;
                case VirtualKey.Right:
                    JoyPads["RIGHT"].IsDown = true;
                    break;
                case VirtualKey.Up:
                    JoyPads["UP"].IsDown = true;
                    break;
                case VirtualKey.Down:
                    JoyPads["DOWN"].IsDown = true;
                    break;
                case VirtualKey.Space:
                    JoyPads["FIRE"].IsDown = true;
                    break;
            }
        }

        public bool UpPressed
        {
            get
            {
                return up.IsDown;
            }
        }

        public bool DownPressed
        {
            get
            {
                return down.IsDown;
            }
        }

        public bool LeftPressed
        {
            get
            {
                return left.IsDown;
            }
        }

        public bool RightPressed
        {
            get
            {
                return right.IsDown;
            }
        }

        public bool FirePressed
        {
            get
            {
                return fire.IsDown;
            }
        }

        private void pad_pointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Pad p = sender as Pad;
            p.IsDown = false;
        }

        private void pad_pointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Pad p = sender as Pad;
            p.IsDown = true;
        }

        public void Clear()
        {
            Visibility = Visibility.Collapsed;
        }
    }
}
