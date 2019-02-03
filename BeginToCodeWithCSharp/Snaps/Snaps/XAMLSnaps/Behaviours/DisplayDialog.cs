using System;
using System.Threading;
using SnapsLibrary;
using Windows.UI.Popups;

namespace XAMLSnaps
{
    public partial class SnapsManager 
    {
        public void DisplayDialog(string dialogText)
        {
            AutoResetEvent dialogCompleteEvent = new AutoResetEvent(false);

            InvokeOnUIThread(
                async () =>
                {
                    var dialog = new MessageDialog(dialogText);
                    await dialog.ShowAsync();
                    dialogCompleteEvent.Set();
                }
            );

            dialogCompleteEvent.WaitOne();
        }
    }
}
