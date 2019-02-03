using System;
using Windows.Foundation;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace XAMLSnaps
{
    public partial class SnapsManager 
    {

        #region Weather Information

        public async static Task<XDocument> GetWeatherXMLDocument(double latitude, double longitude)
        {
            string url = string.Format("http://forecast.weather.gov/MapClick.php?lat={0}&lon={1}&unit=0&lg=english&FcstType=dwml",
                latitude.ToString(), longitude.ToString());

            string xml;

            try
            {
                xml = await GetPageAsString(url);
            }
            catch
            {
                throw new Exception("Weather network request failed.");
            }

            XDocument weatherDoc = XDocument.Parse(xml);


            return weatherDoc;
        }

        public async static Task<int> GetTodayTemperatureInFahrenheitAsync(double latitude, double longitude)
        {
            XDocument weatherDoc = await GetWeatherXMLDocument(latitude,longitude);

            string currentTempString = null;

            foreach (var weatherElement in weatherDoc.Element("dwml").Elements())
            {
                string s = weatherElement.Name.ToString();
                if (s != "data")
                    continue;
                foreach (var elementAttribute in weatherElement.Attributes())
                {
                    string elementName = elementAttribute.Name.ToString();
                    if (elementName != "type")
                        continue;
                    if (elementAttribute.Value != "current observations")
                        continue;

                    foreach (var tempElement in weatherElement.Element("parameters").Elements())
                    {
                        if (tempElement.Name.ToString() != "temperature")
                            continue;
                        foreach (var tempAttribute in tempElement.Attributes())
                        {
                            string tempElementName = tempAttribute.Name.ToString();
                            if (elementName != "type")
                                continue;
                            if (tempAttribute.Value != "apparent")
                                continue;
                            currentTempString = tempElement.Value;
                        }
                    }
                }
            }
            return int.Parse(currentTempString);
        }

        const int INVALID_TEMPERATURE = 1000;

        public int GetTodayTemperatureInFahrenheit(double latitude, double longitude)
        {
            int weatherTemperatureResult = 0;
            AutoResetEvent GotTemperatureEvent = new AutoResetEvent(false);

            IAsyncAction asyncAction = Windows.System.Threading.ThreadPool.RunAsync(
            async (workItem) =>
            {
                try
                {
                    weatherTemperatureResult = await GetTodayTemperatureInFahrenheitAsync(latitude, longitude);
                }
                catch
                {
                    weatherTemperatureResult = INVALID_TEMPERATURE;
                }
                GotTemperatureEvent.Set();
            });

            GotTemperatureEvent.WaitOne();
            return weatherTemperatureResult;
        }

        public async static Task<string> GetWeatherConditionsAsync(double latitude, double longitude)
        {
            string conditionsString = null;

            XDocument weatherDoc = await GetWeatherXMLDocument(latitude, longitude);

            foreach (var weatherElement in weatherDoc.Element("dwml").Elements())
            {
                string s = weatherElement.Name.ToString();
                if (s != "data")
                    continue;
                foreach (var elementAttribute in weatherElement.Attributes())
                {
                    string elementName = elementAttribute.Name.ToString();
                    if (elementName != "type")
                        continue;
                    if (elementAttribute.Value != "current observations")
                        continue;

                    foreach (var tempElement in weatherElement.Element("parameters").Elements())
                    {
                        if (tempElement.Name.ToString() != "weather")
                            continue;

                        conditionsString = tempElement.Element("weather-conditions").Attribute("weather-summary").Value;
                    }
                }
            }
            return conditionsString;
        }

        public string GetWeatherConditionsDescription(double latitude, double longitude)
        {
            string weatherConditionsResult = "";
            AutoResetEvent GotWeatherConditionsEvent = new AutoResetEvent(false);
            Exception errorException = null;

            IAsyncAction asyncAction = Windows.System.Threading.ThreadPool.RunAsync(
            async (workItem) =>
            {
                try
                {
                    weatherConditionsResult = await GetWeatherConditionsAsync(latitude, longitude);
                }
                catch (Exception e)
                {
                    errorException = e;
                }
                GotWeatherConditionsEvent.Set();
            });

            GotWeatherConditionsEvent.WaitOne();

            if (errorException != null)
                weatherConditionsResult = "Weather not available for this location" ;
            return weatherConditionsResult;
        }

        #endregion
    }
}
