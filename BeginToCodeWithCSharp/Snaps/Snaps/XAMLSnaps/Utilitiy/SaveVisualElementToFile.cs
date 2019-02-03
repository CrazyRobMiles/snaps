using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace XAMLSnaps
{
    public partial class SnapsManager
    {

        async Task<bool> SaveVisualElementToFileAsPNGAsync(FrameworkElement element)
        {
            StorageFile file = null;

            FileSavePicker savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;

            savePicker.FileTypeChoices.Add("png file", new List<string>() { ".png" });

            file = await savePicker.PickSaveFileAsync();

            if (file == null)
                return false;

            if(file != null)
            {
                await SaveVisualElementToFileAsPNGAsync(element, file);
            }
            return true;
        }

        async Task SaveVisualElementToLocalStorageAsPNGAsync(FrameworkElement element, string filename)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;

            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);

            await SaveVisualElementToFileAsPNGAsync(element, file);
        }

        async Task SaveVisualElementToFileAsPNGAsync(FrameworkElement element, StorageFile file)
        {

            var renderTargetBitmap = new RenderTargetBitmap();
            await renderTargetBitmap.RenderAsync(element);//, (int)element.ActualWidth, (int)element.ActualHeight);
            var pixelBuffer = await renderTargetBitmap.GetPixelsAsync();

            float dpi = DisplayInformation.GetForCurrentView().LogicalDpi;

            using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
                encoder.SetPixelData(
                    BitmapPixelFormat.Bgra8,
                    BitmapAlphaMode.Straight,
                    (uint)renderTargetBitmap.PixelWidth,
                    (uint)renderTargetBitmap.PixelHeight, dpi, dpi,
                    pixelBuffer.ToArray());

                await encoder.FlushAsync();
            }

        }
    

        public bool SaveVisualElementToFileAsPNG(FrameworkElement element)
        {
            bool result = true;

            AutoResetEvent savedCompleteEvent = new AutoResetEvent(false);

            manager.InvokeOnUIThread(
                async () =>
                {
                    result = await SaveVisualElementToFileAsPNGAsync(element);
                    savedCompleteEvent.Set();
                });

            savedCompleteEvent.WaitOne();

            return result;
        }

        public void SaveVisualElementToLocalStorageAsPNG(FrameworkElement element, string filename)
        {
            AutoResetEvent savedCompleteEvent = new AutoResetEvent(false);

            manager.InvokeOnUIThread(
                async () =>
                {
                    await SaveVisualElementToLocalStorageAsPNGAsync(element,filename);
                    savedCompleteEvent.Set();
                });

            savedCompleteEvent.WaitOne();
        }
    }
}