using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

using Windows.Media.SpeechSynthesis;
using System.Threading;
using System.Threading.Tasks;
using Windows.System.Threading;
using Windows.UI;
using Windows.UI.Xaml.Shapes;

using System.Net.Http;
using System.Xml.Linq;
using SnapsLibrary;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Popups;

namespace XAMLSnaps
{
    public static class FadeElements
    {
        public static async Task FadeElementOpacityAsync(FrameworkElement target, float from, float to, TimeSpan duration)
        {
            Storyboard storyboard = new Storyboard();

            // Create a DoubleAnimation to fade the not selected option control
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = from;
            animation.To = to;
            animation.Duration = new Duration(duration);
            // Configure the animation to target de property Opacity
            Storyboard.SetTarget(animation, target);
            Storyboard.SetTargetProperty(animation, "Opacity");
            // Add the animation to the storyboard
            storyboard.Children.Add(animation);

            // Begin the storyboard
            var tcs = new TaskCompletionSource<object>();

            EventHandler<object> lambda = (s, e) =>
            {
                tcs.TrySetResult(null);
            };
            try
            {
                storyboard.Completed += lambda;
                storyboard.Begin();
                await tcs.Task;
            }
            finally
            {
                storyboard.Completed -= lambda;
            }
        }
    }
}
