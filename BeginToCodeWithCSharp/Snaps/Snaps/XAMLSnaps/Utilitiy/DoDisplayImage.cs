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
using Windows.Storage;

namespace XAMLSnaps
{
    public partial class SnapsManager
    {
        public bool DoDisplayUrlImage(string imageURL, Image graphicsImage)
        {
            return DoDisplayUrlImage(new Uri(imageURL,UriKind.RelativeOrAbsolute), graphicsImage);
        }

        public bool DoDisplayUrlImage(Uri imageURI, Image graphicsImage)
        {

            AutoResetEvent DisplayComplete = new AutoResetEvent(false);

            AutoResetEvent loadCompletedEvent = new AutoResetEvent(false);

            BitmapImage result = null;

            bool failed = false;

            var tcs = new TaskCompletionSource<object>();

            RoutedEventHandler endedLambda = (s, e) =>
                tcs.TrySetResult(null);

            ExceptionRoutedEventHandler endedFailed = (s, e) =>
            {
                failed = true;
                tcs.TrySetResult(null);
            };

            InvokeOnUIThread(
            async () =>
            {
                if (graphicsImage.Opacity == 1)
                {
                    await FadeElements.FadeElementOpacityAsync(graphicsImage, 1, 0, new TimeSpan(0, 0, 1));
                }

                try
                {
                    result = new BitmapImage();
                    result.ImageFailed += endedFailed;
                    result.ImageOpened += endedLambda;
                    try
                    {
                        result.UriSource = imageURI;
                        graphicsImage.Width = graphicsCanvas.ActualWidth;
                        graphicsImage.Height = graphicsCanvas.ActualHeight;
                        graphicsImage.Source = result;
                        await tcs.Task;
                    }
                    catch
                    {
                        failed = true;
                    }

                    if (failed)
                    {
                        imageURI = new Uri("ms-appx:///Images/ImageNotFound.png");
                        result.UriSource = imageURI;
                        graphicsImage.Source = result;
                        await tcs.Task;
                    }
                }
                finally
                {
                    result.ImageFailed -= endedFailed;
                    result.ImageOpened -= endedLambda;
                }

                await FadeElements.FadeElementOpacityAsync(graphicsImage, 0, 1, new TimeSpan(0, 0, 1));

                DisplayComplete.Set();
            });

            DisplayComplete.WaitOne();

            return !failed;
        }
        public bool DoDisplayUrlImage(string imageURL, Image graphicsImage,int width, int height)
        {
            AutoResetEvent DisplayComplete = new AutoResetEvent(false);

            AutoResetEvent loadCompletedEvent = new AutoResetEvent(false);

            BitmapImage result = null;

            bool failed = false;

            var tcs = new TaskCompletionSource<object>();

            RoutedEventHandler endedLambda = (s, e) =>
                tcs.TrySetResult(null);

            ExceptionRoutedEventHandler endedFailed = (s, e) =>
            {
                failed = true;
                tcs.TrySetResult(null);
            };

            InvokeOnUIThread(
            async () =>
            {

                if (graphicsImage.Opacity == 1)
                {
                    await FadeElements.FadeElementOpacityAsync(graphicsImage, 1, 0, new TimeSpan(0, 0, 1));
                }

                try
                {
                    Uri imageURI = new Uri(imageURL, UriKind.RelativeOrAbsolute);
                    result = new BitmapImage();
                    result.ImageFailed += endedFailed;
                    result.ImageOpened += endedLambda;
                    result.UriSource = imageURI;
                    graphicsImage.Source = result;
                    await tcs.Task;
                    if (failed)
                    {
                        imageURI = new Uri("ms-appx:///Images/ImageNotFound.png");
                        result.UriSource = imageURI;
                        graphicsImage.Source = result;
                        await tcs.Task;
                    }
                }
                finally
                {
                    result.ImageFailed -= endedFailed;
                    result.ImageOpened -= endedLambda;
                }

                graphicsImage.Width = width;
                graphicsImage.Height = height;
                graphicsImage.HorizontalAlignment = HorizontalAlignment.Stretch;

                await FadeElements.FadeElementOpacityAsync(graphicsImage, 0, 1, new TimeSpan(0, 0, 1));

                DisplayComplete.Set();
            });

            DisplayComplete.WaitOne();

            return !failed;
        }

        public bool DoDisplayFileImage(StorageFile file, Image graphicsImage)
        {
            bool result = true;

            AutoResetEvent DisplayComplete = new AutoResetEvent(false);

            InvokeOnUIThread(
            async () =>
            {
                if (graphicsImage.Opacity == 1)
                {
                    await FadeElements.FadeElementOpacityAsync(graphicsImage, 1, 0, new TimeSpan(0, 0, 1));
                }

                try
                {
                    var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);

                    var bitmapImage = new Windows.UI.Xaml.Media.Imaging.BitmapImage();

                    await bitmapImage.SetSourceAsync(stream);

                    graphicsImage.Source = bitmapImage;

                    graphicsImage.Width = graphicsCanvas.ActualWidth;
                    graphicsImage.Height = graphicsCanvas.ActualHeight;

                    await FadeElements.FadeElementOpacityAsync(graphicsImage, 0, 1, new TimeSpan(0, 0, 1));
                }
                catch
                {
                    result = false;
                }
                finally
                {
                    DisplayComplete.Set();
                }
            });

            DisplayComplete.WaitOne();

            return result;
        }

        public bool LoadGraphicsPNGImageFromLocalStore(string filename, Image graphicsImage)
        {
            AutoResetEvent DisplayComplete = new AutoResetEvent(false);

            bool displayedOK = true;

            InvokeOnUIThread(
            async () =>
            {
                try
                {
                    if (graphicsImage.Opacity == 1)
                    {
                        await FadeElements.FadeElementOpacityAsync(graphicsImage, 1, 0, new TimeSpan(0, 0, 1));
                    }

                    var file = await ApplicationData.Current.LocalFolder.GetFileAsync(filename);

                    var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);

                    var bitmapImage = new Windows.UI.Xaml.Media.Imaging.BitmapImage();

                    await bitmapImage.SetSourceAsync(stream);

                    graphicsImage.Source = bitmapImage;

                    graphicsImage.Width = graphicsCanvas.ActualWidth;
                    graphicsImage.Height = graphicsCanvas.ActualHeight;
                }
                catch
                {
                    displayedOK = false;
                    graphicsImage.Source = null;
                }
                finally
                {
                    await FadeElements.FadeElementOpacityAsync(graphicsImage, 0, 1, new TimeSpan(0, 0, 1));
                }

                DisplayComplete.Set();
            });

            DisplayComplete.WaitOne();

            return displayedOK;
        }

        public bool DoDisplaySnapsImage(SnapsImage image, Image graphicsImage)
        {
            return DoDisplayFileImage(image.File,graphicsImage);
        }

        public enum Resolution { Unspecified, _800x600, _1024x768, _1366x768, _1920x1080, _1920x1200 }

        public static async Task<Uri> GetBingImageOfTheDayUriAsync(
            Resolution resolution = Resolution.Unspecified,
            string market = "en-ww")
        {
            var request = new Uri($"http://www.bing.com/hpimagearchive.aspx?n=1&mkt={market}");
            string result = null;
            using (var httpClient = new HttpClient())
            {
                result = await httpClient.GetStringAsync(request);
            }
            var targetElement = resolution == Resolution.Unspecified ? "url" : "urlBase";
            var pathString = XDocument.Parse(result).Descendants(targetElement).First().Value;
            var resolutionString = resolution == Resolution.Unspecified ? "" : $"{resolution}.jpg";
            return new Uri($"http://www.bing.com{pathString}{resolutionString}");
        }

        public static Uri GetBingImageOfTheDay(
            Resolution resolution = Resolution.Unspecified,
            string market = "en-ww")
        {
            AutoResetEvent GotWebPage = new AutoResetEvent(false);
            Exception errorException = null;

            Uri result = null;

            IAsyncAction asyncAction = Windows.System.Threading.ThreadPool.RunAsync(
            async (workItem) =>
            {
                try
                {
                    result = await GetBingImageOfTheDayUriAsync(resolution, market);
                }
                catch (Exception e)
                {
                    errorException = e;
                }
                finally
                {
                    GotWebPage.Set();
                }
            });

            GotWebPage.WaitOne();

            if (errorException != null)
                throw errorException;

            return result;
        }

        public bool DoDisplayBingImageOfTheDay(Image graphicsImage, 
            Resolution resolution = Resolution.Unspecified,
            string market = "en-ww")
        {
            Uri bingURI = GetBingImageOfTheDay(resolution, market);
            return DoDisplayUrlImage(bingURI, graphicsImage);
        }
    }
}
