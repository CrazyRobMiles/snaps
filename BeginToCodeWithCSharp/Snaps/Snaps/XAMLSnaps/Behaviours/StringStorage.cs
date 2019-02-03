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
    public partial class SnapsManager 
    {
        public void SaveStringToLocalStorage(string itemName, string itemValue)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            localSettings.Values[itemName.ToLower()] = itemValue;
        }

        public string FetchStringFromLocalStorage(string itemName)
        {
            itemName = itemName.ToLower();

            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            if (localSettings.Values.ContainsKey(itemName))
                return localSettings.Values[itemName] as string;
            return null;
        }

        public void SaveStringToRoamingStorage(string itemName, string itemValue)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;

            localSettings.Values[itemName.ToLower()] = itemValue;
        }

        public string FetchStringFromRoamingStorage(string itemName)
        {
            itemName = itemName.ToLower();

            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;

            if (localSettings.Values.ContainsKey(itemName))
                return localSettings.Values[itemName] as string;
            return null;
        }
    
    }
}
