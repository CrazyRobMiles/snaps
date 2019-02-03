using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace XAMLSnaps
{
    public partial class SnapsManager
    {
        public async Task<bool> TakePhotographAsync()
        {
            bool result = true;

            try {

                CameraCaptureUI camera = new CameraCaptureUI();

                StorageFile file = await camera.CaptureFileAsync(CameraCaptureUIMode.Photo);
                if (file != null)
                {
                    using (IRandomAccessStream ras = await file.OpenAsync(FileAccessMode.Read))
                    {
                        BitmapImage source = new BitmapImage();
                        source.SetSource(ras);

                        graphicsCanvas.DisplayImage.Source = source;
                    }
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public bool TakePhotograph()
        {
            AutoResetEvent gotTextEvent = new AutoResetEvent(false);

            bool result = false;

            manager.InvokeOnUIThread(
                async () =>
                {
                    result = await TakePhotographAsync();
                    gotTextEvent.Set();
                }
            );

            gotTextEvent.WaitOne();

            return result;
        }
    }
}
