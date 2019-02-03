

using System.Collections.Generic;
using XAMLSnaps;

namespace SnapsLibrary
{
    public static partial class SnapsEngine
    {
        /// <summary>
        /// Current SnapsEngine version as a string
        /// </summary>
        public static string Version
        {
            get
            {
                return manager.Version;
            }
        }

        private static SnapsManager manager;

        /// <summary>
        /// Binds the active Snaps Manager to the engine
        /// </summary>
        /// <param name="newManager">manager to bind</param>
        public static void SetManager (SnapsManager newManager)
        {
            manager = newManager;
        }

        /// <summary>
        /// Set to true if you want Snaps methods to throw exceptions when they fail:
        /// Snaps methods affected:
        ///     PlaySound
        /// False when program starts
        /// </summary>
        public static bool ThrowExceptions = false;

        /// <summary>
        /// Set to true if you want to display the program to display the control menu when it ends.
        /// True when program starts
        /// </summary>
        public static bool DisplayControMenuAtProgramEnd = true;

        /// <summary>
        /// Sets the title string on a Snaps program page
        /// </summary>
        /// <param name="title">the title to be displayed</param>
        public static void SetTitleString(string title)
        {
            manager.SetTitleString(title);
        }

        /// <summary>
        /// Clears the title string on a Snaps program page
        /// </summary>
        public static void ClearTitleString()
        {
            manager.ClearTitleString();
        }

        /// <summary>
        /// Sets the color of the text output from a Snaps program
        /// </summary>
        /// <param name="red">amount of red in range 0-255</param>
        /// <param name="green">amount of green in range 0-255</param>
        /// <param name="blue">amount of blue in range 0-255</param>
        public static void SetTextColor(int red, int green, int blue)
        {
            manager.SetTextColor(red, green, blue);
        }

        /// <summary>
        /// Sets the color of the text output from a Snaps program
        /// </summary>
        /// <param name="color">selected color</param>
        public static void SetTextColor(SnapsColor color)
        {
            manager.SetTextColor(color);
        }

        /// <summary>
        /// Sets the color of the title string for a Snaps program
        /// </summary>
        /// <param name="red">amount of red in range 0-255</param>
        /// <param name="green">amount of green in range 0-255</param>
        /// <param name="blue">amount of blue in range 0-255</param>
        public static void SetTitleColor(int red, int green, int blue)
        {
            manager.SetTitleColor(red, green, blue);
        }

        /// <summary>
        /// Sets the color of the title string for a Snaps program
        /// </summary>
        /// <param name="color"></param>
        public static void SetTitleColor(SnapsColor color)
        {
            manager.SetTitleColor(color);
        }

        /// <summary>
        /// Displays a string in the Snaps display region
        /// </summary>
        /// <param name="message">string to display</param>
        /// <param name="alignment">horizontal alignment of text</param>
        /// <param name="fadeType">how the text is faded onto the display</param>
        /// <param name="size">size of the text in pixels</param>
        public static void DisplayString(string message, SnapsTextAlignment alignment, SnapsFadeType fadeType, int size)
        {
            manager.DisplayString(message, alignment, fadeType, size);
        }

        /// <summary>
        /// Displays a string on the Snaps page
        /// </summary>
        /// <param name="message">the string to be displayed. Can be multiple lines.</param>
        public static void DisplayString(string message)
        {
            manager.DisplayString(message);
        }

        /// <summary>
        /// Sets the size of the currently displayed string
        /// </summary>
        /// <param name="size">required size in pixels</param>
        public static void SetDisplayStringSize(double size)
        {
            manager.SetDisplayStringSize(size);
        }

        /// <summary>
        /// Sets the width of the textblock used by Snaps to display strings of text
        /// </summary>
        /// <param name="width">required width in pixels</param>
        public static void SetDisplayStringWidth(double width)
        {
            manager.SetDisplayStringWidth(width);
        }

        /// <summary>
        /// Displays the image at the given URL. Displays an "image not found"
        /// image if the image is not available.
        /// </summary>
        /// <param name="imageURL">url of image to be displayed</param>
        /// <returns>true if the image was found and displayed</returns>
        public static bool DisplayImageFromUrl(string imageURL)
        {
            return manager.DisplayImageFromUrl(imageURL);
        }

        /// <summary>
        /// Displays a Snaps image on the screen
        /// </summary>
        /// <param name="imageToDisplay">the image to display</param>
        /// <returns>true if the image was successfully displayed</returns>
        public static bool DisplaySnapsImage(SnapsImage imageToDisplay)
        {
            return manager.DisplaySnapsImage(imageToDisplay);
        }

        /// <summary>
        /// Display a file picker, let the user select an image and then display it.
        /// </summary>
        /// <returns>false if the selection was abandoned</returns>
        public static bool PickImage()
        {
            return manager.PickImage();
        }

        /// <summary>
        /// Display a file picker, let the user select multiple images and then 
        /// return them as a list of images.
        /// </summary>
        /// <returns>false if the selection was abandoned</returns>
        public static List<SnapsImage> PickImages()
        {
            return manager.PickImages();
        }

        /// <summary>
        /// Displays an image as the screen background
        /// </summary>
        /// <param name="imageURL">url of the image to display</param>
        /// <returns>true if the image was found and displayed</returns>
        public static bool DisplayBackground(string imageURL)
        {
            return manager.DisplayBackground(imageURL);
        }

        /// <summary>
        /// Displays a prompt and waits for the user to enter a string
        /// </summary>
        /// <param name="prompt">prompt to display</param>
        /// <returns>string that was entered</returns>
        public static string ReadString(string prompt)
        {
            return manager.ReadString(prompt);
        }

        /// <summary>
        /// Displays a prompt and waits for the user to enter a multi-line string
        /// </summary>
        /// <param name="prompt">prompt to display</param>
        /// <returns>string that was entered</returns>
        public static string ReadMultiLineString(string prompt)
        {
            return manager.ReadMultiLineString(prompt);
        }

        /// <summary>
        /// Displays a prompt and waits for the user to enter a string.
        /// The string contents are not displayed as they are typed.
        /// </summary>
        /// <param name="prompt">prompt to display</param>
        /// <returns>string that was entered</returns>
        public static string ReadPassword(string prompt)
        {
            return manager.ReadPassword(prompt);
        }

        /// <summary>
        /// Speaks the supplied string. The program is paused 
        /// while the string is being spoken.
        /// </summary>
        /// <param name="message">Message to speak</param>
        public static void SpeakString(string message)
        {
            manager.SpeakString(message);
        }

        /// <summary>
        /// Starts playing the given sound effect. The method
        /// returns when the sound has finished playing.
        /// </summary>
        /// <param name="effectName">name of the file in Sounds/SoundEffects to play</param>
        public static void PlaySoundEffect(string effectName)
        {
            manager.PlaySoundEffect(effectName);
        }

        /// <summary>
        /// Starts playing the given sound effect. The method
        /// returns as soon as the sound has started playing.
        /// </summary>
        /// <param name="effectName">name of the file in Sounds/SoundEffects to play</param>
        public static void PlayGameSoundEffect(string effectName)
        {
            manager.PlaySoundEffectNoWait(effectName);
        }

        /// <summary>
        /// Allows the user to choose from a number of options by speaking
        /// their selection.
        /// Speech version of SelectFromButtonArray
        /// </summary>
        /// <param name="prompt">prompt for the request</param>
        /// <param name="phrases">items to choose from</param>
        /// <returns>the selected phrase or an empty string if the phrase was not recognised</returns>
        public static string SelectSpokenPhrase(string prompt, string[] phrases)
        {
            return manager.SelectSpokenPhrase(prompt: prompt, phrases: phrases);
        }

        /// <summary>
        /// Select from two spoken options using speech recogntion.
        /// </summary>
        /// <param name="prompt">prompt to display</param>
        /// <param name="phrase1">phrase to select</param>
        /// <param name="phrase2">phrase to select</param>
        /// <returns>the selected phrase or an empty string if the phrase was not recognised</returns>
        public static string SelectFromTwoSpokenPhrases( string prompt, string phrase1, string phrase2)
        {
            return SelectSpokenPhrase(prompt, new string[] { phrase1, phrase2 });
        }

        /// <summary>
        /// Select from three spoken options using speech recogntion.
        /// </summary>
        /// <param name="prompt">prompt to display</param>
        /// <param name="phrase1">phrase to select</param>
        /// <param name="phrase2">phrase to select</param>
        /// <param name="phrase3">phrase to select</param>
        /// <returns>the selected phrase or an empty string if the phrase was not recognised</returns>
        public static string SelectFromThreeSpokenPhrases(string prompt, string phrase1, string phrase2, string phrase3)
        {
            return SelectSpokenPhrase(prompt, new string[] { phrase1, phrase2, phrase3 });
        }

        /// <summary>
        /// Select from four spoken options using speech recogntion.
        /// </summary>
        /// <param name="prompt">prompt to display</param>
        /// <param name="phrase1">phrase to select</param>
        /// <param name="phrase2">phrase to select</param>
        /// <param name="phrase3">phrase to select</param>
        /// <param name="phrase4">phrase to select</param>
        /// <returns>the selected phrase or an empty string if the phrase was not recognised</returns>
        public static string SelectFromFourSpokenPhrases(string prompt, string phrase1, string phrase2, string phrase3, string phrase4)
        {
            return SelectSpokenPhrase(prompt,new string[] { phrase1, phrase2, phrase3, phrase4 });
        }

        /// <summary>
        /// Select from five spoken options using speech recogntion.
        /// </summary>
        /// <param name="prompt">prompt to display</param>
        /// <param name="phrase1">phrase to select</param>
        /// <param name="phrase2">phrase to select</param>
        /// <param name="phrase3">phrase to select</param>
        /// <param name="phrase4">phrase to select</param>
        /// <param name="phrase5">phrase to select</param>
        /// <returns>the selected phrase or an empty string if the phrase was not recognised</returns>
        public static string SelectFromFiveSpokenPhrases(string prompt, string phrase1, string phrase2, string phrase3, string phrase4, string phrase5)
        {
            return SelectSpokenPhrase(prompt, new string[] { phrase1, phrase2, phrase3, phrase4, phrase5 });
        }

        /// <summary>
        /// Select from six spoken options using speech recogntion.
        /// </summary>
        /// <param name="prompt">prompt to display</param>
        /// <param name="phrase1">phrase to select</param>
        /// <param name="phrase2">phrase to select</param>
        /// <param name="phrase3">phrase to select</param>
        /// <param name="phrase4">phrase to select</param>
        /// <param name="phrase5">phrase to select</param>
        /// <param name="phrase6">phrase to select</param>
        /// <returns>the selected phrase or an empty string if the phrase was not recognised</returns>
        public static string SelectFromSixSpokenPhrases(string prompt, string phrase1, string phrase2, string phrase3, 
            string phrase4, string phrase5, string phrase6)
        {
            return SelectSpokenPhrase(prompt, new string[] { phrase1, phrase2, phrase3, phrase4, phrase5, phrase6 });
        }

        /// <summary>
        /// Play a piano note of a given pitch for a particular duration
        /// </summary>
        /// <param name="pitch">There are 12 pitch values 
        /// 0 - middle C, successive notes values are 1 semitone
        /// higher, up to the end of the octave.</param>
        /// <param name="duration">note duration in seconds</param>
        public static void PlayNote(int pitch, double duration)
        {
            manager.PlayNote(pitch,duration);
        }

        /// <summary>
        /// Gets the given web page as a string of text.
        /// Throws an exception if the page cannot be loaded
        /// </summary>
        /// <param name="url">url of the web page to be loaded, must start with http://</param>
        /// <returns>web page as a single string of text</returns>
        public static string GetWebPageAsString(string url)
        {
            return manager.GetWebPageAsString(url);
        }

        /// <summary>
        /// Gets the temperature of a US location from http://forecast.weather.gov
        /// Doesn't work for worldwide locations
        /// </summary>
        /// <param name="latitude">latitude of location</param>
        /// <param name="longitude">longitude of location</param>
        /// <returns>temperature in fahrenheit</returns>
        public static int GetTodayTemperatureInFahrenheit(double latitude, double longitude)
        {
            return manager.GetTodayTemperatureInFahrenheit(latitude, longitude);
        }

        /// <summary>
        /// Gets a text description of the weather for US locations from http://forecast.weather.gov
        /// Doesn't work for worldwide locations
        /// </summary>
        /// <param name="latitude">latitude of location</param>
        /// <param name="longitude">longitude of location</param>
        /// <returns>string description</returns>
        public static string GetWeatherConditionsDescription(double latitude, double longitude)
        {
            return manager.GetWeatherConditionsDescription(latitude, longitude);
        }

        /// <summary>
        /// Pause program execution for given duration
        /// </summary>
        /// <param name="durationInSeconds">duration of delay</param>
        public static void Delay(double durationInSeconds)
        {
            manager.Delay(durationInSeconds);
        }

        /// <summary>
        /// Sets the background colour of the drawing area
        /// </summary>
        /// <param name="red">red intensity in range 0-255. Value is clamped in this range.</param>
        /// <param name="green">green intensity in range 0-255. Value is clamped in this range.</param>
        /// <param name="blue">blue intensity in range 0-255. Value is clamped in this range.</param>
        public static void SetBackgroundColor(int red, int green, int blue)
        {
            manager.SetBackgroundColor(red, green, blue);
        }

        /// <summary>
        /// Clear the graphics screen of all drawn objects
        /// </summary>
        public static void ClearGraphics()
        {
            manager.ClearGraphics();
        }

        /// <summary>
        /// Sets the color of successive drawing actions
        /// </summary>
        /// <param name="red">red intensity in range 0-255. Value is clamped in this range.</param>
        /// <param name="green">green intensity in range 0-255. Value is clamped in this range.</param>
        /// <param name="blue">blue intensity in range 0-255. Value is clamped in this range.</param>
        public static void SetDrawingColor(int red, int green, int blue)
        {
            manager.SetDrawingColor(red, green, blue);
        }

        /// <summary>
        /// Sets the color of successive drawing actions
        /// </summary>
        /// <param name="color">color to be used for the drawing</param>
        public static void SetDrawingColor(SnapsColor color)
        {
            manager.SetDrawingColor(color);
        }

        /// <summary>
        /// Sets the background colour of the drawing area
        /// </summary>
        /// <param name="color">color to be used</param>
        public static void SetBackgroundColor(SnapsColor color)
        {
            manager.SetBackgroundColor(color);
        }

        /// <summary>
        /// Draw a solid block on the screen
        /// </summary>
        /// <param name="x">x position of top left hand corner in pixels</param>
        /// <param name="y">y position of top left hand corner in pixels</param>
        /// <param name="width">width of the block in pixels</param>
        /// <param name="height">height of the block in pixels</param>
        public static void DrawBlock(int x, int y, int width, int height)
        {
            manager.DrawBlock(x, y, width, height);
        }

        /// <summary>
        /// Draw a solid dot on the screen
        /// </summary>
        /// <param name="x">x position of the center of the dot in pixels</param>
        /// <param name="y">y position of the center of the dot in pixels</param>
        /// <param name="width">width of the dot in pixels</param>
        public static void DrawDot(int x, int y, int width)
        {
            manager.DrawDot(x, y, width);
        }

        /// <summary>
        /// Draw a solid dot on the screen
        /// </summary>
        /// <param name="pos">position of the dot</param>
        /// <param name="width">width of the dot in pixels</param>
        public static void DrawDot(SnapsCoordinate pos, int width)
        {
            manager.DrawDot(pos, width);
        }

        /// <summary>
        /// Draw a line on the screen
        /// </summary>
        /// <param name="x1">x position of the start of the line, in pixels</param>
        /// <param name="y1">y position of the start of the line, in pixels</param>
        /// <param name="x2">x position of the end of the line, in pixels</param>
        /// <param name="y2">y position of the end of the line, in pixels</param>
        public static void DrawLine(int x1, int y1, int x2, int y2)
        {
            manager.DrawLine(x1, y1, x2, y2);
        }

        /// <summary>
        /// Draw a line on the screen
        /// </summary>
        /// <param name="p1">start of the line</param>
        /// <param name="p2">end of the line</param>
        public static void DrawLine(SnapsCoordinate p1, SnapsCoordinate p2)
        {
            manager.DrawLine(p1, p2);
        }

        /// <summary>
        /// Take a photograph using the camera device. The photograph is 
        /// displayed on the graphics canvas and will be saved if the graphics
        /// canvas is saved. It can also be annotated by drawing actions on 
        /// the canvas.
        /// </summary>
        /// <returns>false if the user canceled the action</returns>
        public static bool TakePhotograph()
        {
            return manager.TakePhotograph();
        }

        /// <summary>
        /// Get the size of the application screen
        /// </summary>
        /// <returns>x value of coordinate gives the width, y gives the height</returns>
        public static SnapsCoordinate GetScreenSize()
        {
            return manager.GetScreenSize();
        }

        /// <summary>
        /// Make and display a light panel of the given size
        /// The lightpanel is scaled to fit the application screen area
        /// An exception is thrown if the cells values are out of range
        /// </summary>
        /// <param name="xCells">number of x cells, upper limit 32</param>
        /// <param name="yCells">number of y cells, upper limit 32</param>
        public static void MakeLightPanel(int xCells, int yCells)
        {
            manager.MakeLightPanel(xCells, yCells);
        }

        /// <summary>
        /// Set the colour of the specified cell
        /// </summary>
        /// <param name="x">x position of cell, 0 is top left hand corner</param>
        /// <param name="y">y position of cell, 0 is top left hand corner</param>
        /// <param name="red">red intensity in range 0-255. Value is clamped in this range.</param>
        /// <param name="green">green intensity in range 0-255. Value is clamped in this range.</param>
        /// <param name="blue">blue intensity in range 0-255. Value is clamped in this range.</param>
        public static void SetPanelCell(int x, int y, int red, int green, int blue)
        {
            manager.SetPanelCell(x, y, red, green, blue);
        }

        /// <summary>
        /// Set the colour of the specified cell
        /// </summary>
        /// <param name="x">x position of cell, 0 is top left hand corner</param>
        /// <param name="y">y position of cell, 0 is top left hand corner</param>
        /// <param name="panelColor">color of the cell</param>
        public static void SetPanelCell(int x, int y, SnapsColor panelColor)
        {
            manager.SetPanelCell(x, y, panelColor);
        }

        /// <summary>
        /// Saves the current drawing as a PNG file. The user is asked to
        /// select a save location. 
        /// </summary>
        /// <returns>false if the user cancelled the save</returns>
        public static bool SaveGraphicsImageToFileAsPNG()
        {
            return manager.SaveGraphicsImageToFileAsPNG();
        }

        /// <summary>
        /// Saves the current drawing to application local storage in the given file.
        /// The user will not have access to this storage, the file is for 
        /// application use only. The method may throw exceptions if the 
        /// filename is invalid or the storage fails. 
        /// </summary>
        /// <param name="filename">name of the file to be created</param>
        public static void SaveGraphicsImageToLocalStoreAsPNG(string filename)
        {
            manager.SaveVisualElementToLocalStorageAsPNG(filename);
        }

        /// <summary>
        /// Loads a previously saved image from local storage and displays it
        /// </summary>
        /// <param name="filename">name of the image file</param>
        /// <returns>true if the image was loaded and displayed</returns>
        public static bool LoadGraphicsPNGImageFromLocalStore(string filename)
        {
            return manager.LoadGraphicsPNGImageFromLocalStore(filename);
        }

        /// <summary>
        /// Displays the Bing image of the day
        /// </summary>
        /// <returns>true if the image was successfully displayed</returns>
        public static bool DisplayBingImageOfTheDay()
        {
            return manager.DisplayBingImageOfTheDay();
        }

        /// <summary>
        /// Waits for the user to tap the screen, touch with a pen or click the left
        /// mouuse button.
        /// </summary>
        /// <returns>the x and y position of the tap on the screen in a SnapsCoordinate</returns>
        public static SnapsCoordinate GetTappedCoordinate()
        {
            return manager.GetTappedCoordinate();
        }

        /// <summary>
        /// Waits for the user to left click the mouse and drag the cursor, drag with 
        /// a touch action or click the left mouuse button and drag on the screen 
        /// </summary>
        /// <returns>the x and y position of the end of the drag action</returns>
        public static SnapsCoordinate GetDraggedCoordinate()
        {
            return manager.GetDraggedCoordinate();
        }


        /// <summary>
        /// Clears the screen tapped flag so that the next tap action will be registered
        /// </summary>
        public static void ClearScreenTappedFlag()
        {
            manager.ClearScreenTappedFlag();
        }

        /// <summary>
        /// Polls the screen tapped state and returns immediately.
        /// </summary>
        /// <returns>true if the screen has been tapped, false if not</returns>
        public static bool ScreenHasBeenTapped()
        {
            return manager.ScreenHasBeenTapped();
        }

        /// <summary>
        /// Save a string in application local storage with a particular name
        /// </summary>
        /// <param name="itemName">the name of the storage location to use</param>
        /// <param name="itemValue">the string to be stored</param>
        public static void SaveStringToLocalStorage(string itemName, string itemValue)
        {
            manager.SaveStringToLocalStorage(itemName, itemValue);
        }

        /// <summary>
        /// Fetches a string from application local storage
        /// </summary>
        /// <param name="itemName">Name of the string to fetch</param>
        /// <returns>string that was found, or null if the item does not exist</returns>
        public static string FetchStringFromLocalStorage(string itemName)
        {
            return manager.FetchStringFromLocalStorage(itemName);
        }

        /// <summary>
        /// Save a string in roaming application storage with a particular name
        /// </summary>
        /// <param name="itemName">the name of the storage location to use</param>
        /// <param name="itemValue">the string to be stored</param>
        public static void SaveStringToRoamingStorage(string itemName, string itemValue)
        {
            manager.SaveStringToRoamingStorage(itemName, itemValue);
        }

        /// <summary>
        /// Fetches a string from application roaming storage
        /// </summary>
        /// <param name="itemName">Name of the string to fetch</param>
        /// <returns>string that was found, or null if the item does not exist</returns>
        public static string FetchStringFromRoamingStorage(string itemName)
        {
            return manager.FetchStringFromRoamingStorage(itemName);
        }

        /// <summary>
        /// Display a message to the user in a pop-up dialog. The program 
        /// continues running when the user acknowledges the dialog.
        /// </summary>
        /// <param name="dialogText">message to be displayed</param>
        public static void DisplayDialog(string dialogText)
        {
            manager.DisplayDialog(dialogText);
        }

        /// <summary>
        /// Displays a number of button and waits for the user to press one of them.
        /// </summary>
        /// <param name="buttonTexts">an array of button strings</param>
        /// <returns>the selected string</returns>
        public static string SelectFromButtonArray(string[] buttonTexts)
        {
            return manager.SelectFromButtonArray(buttonTexts);
        }

        /// <summary>
        /// Displays a button with a prompt and waits for the user to press the button.
        /// </summary>
        /// <param name="prompt">the prompt to be displayed on the button</param>
        /// <returns>the prompt on the button</returns>
        public static string WaitForButton(string prompt)
        {
            return manager.WaitForButton(prompt);
        }

        /// <summary>
        /// Displays two buttons and waits for the user to select one of them.
        /// </summary>
        /// <param name="item1">text on the first button</param>
        /// <param name="item2">text on the second button</param>
        /// <returns>text on the selected button</returns>
        public static string SelectFrom2Buttons(string item1, string item2)
        {
            return manager.SelectFromButtonArray(new string[] { item1, item2 });
        }

        /// <summary>
        /// Displays three buttons and waits for the user to select one of them.
        /// </summary>
        /// <param name="item1">text on the first button</param>
        /// <param name="item2">text on the second button</param>
        /// <param name="item3">text on the second button</param>
        /// <returns>text on the selected button</returns>
        public static string SelectFrom3Buttons(string item1, string item2, string item3)
        {
            return manager.SelectFromButtonArray(new string[] { item1, item2, item3 });
        }

        /// <summary>
        /// Displays four buttons and waits for the user to select one of them.
        /// </summary>
        /// <param name="item1">text on the first button</param>
        /// <param name="item2">text on the second button</param>
        /// <param name="item3">text on the second button</param>
        /// <param name="item4">text on the second button</param>
        /// <returns>text on the selected button</returns>
        public static string SelectFrom4Buttons(string item1, string item2, string item3, string item4)
        {
            return manager.SelectFromButtonArray(new string[] { item1, item2, item3, item4 });
        }

        /// <summary>
        /// Displays five buttons and waits for the user to select one of them.
        /// </summary>
        /// <param name="item1">text on the first button</param>
        /// <param name="item2">text on the second button</param>
        /// <param name="item3">text on the second button</param>
        /// <param name="item4">text on the second button</param>
        /// <param name="item5">text on the second button</param>
        /// <returns>text on the selected button</returns>
        public static string SelectFrom5Buttons(string item1, string item2, string item3, string item4, string item5)
        {
            return manager.SelectFromButtonArray(new string[] { item1, item2, item3, item4, item5 });
        }

        /// <summary>
        /// Displays six buttons and waits for the user to select one of them.
        /// </summary>
        /// <param name="item1">text on the first button</param>
        /// <param name="item2">text on the second button</param>
        /// <param name="item3">text on the second button</param>
        /// <param name="item4">text on the second button</param>
        /// <param name="item5">text on the second button</param>
        /// <param name="item6">text on the second button</param>
        /// <returns>text on the selected button</returns>
        public static string SelectFrom6Buttons(string item1, string item2, string item3, string item4, string item5, string item6)
        {
            return manager.SelectFromButtonArray(new string[] { item1, item2, item3, item4, item5, item6 });
        }

        /// <summary>
        /// Writes to a GPIO pin on a Raspberry Pi
        /// Will throw an exception if the program is not running on a Pi or
        /// an invalid pin number is used. 
        /// </summary>
        /// <param name="pinNumber">number of the selected pin</param>
        /// <param name="value">true to make the pin high, false to make it low</param>
        public static void WriteToPin(int pinNumber, bool value)
        {
            manager.WriteToPin(pinNumber, value);
        }

        /// <summary>
        /// Read from GPIO pin on a Rasperry Pi
        /// Will throw an exception if the program is not running on a Pi 
        /// or an invalid pin number is used. 
        /// </summary>
        /// <param name="pinNumber">number of the pin to read</param>
        /// <returns>true if the pin is high and false if it is low</returns>
        public static bool ReadFromPin(int pinNumber)
        {
            return manager.ReadFromPin(pinNumber);
        }

        /// <summary>
        /// Pauses the program until the given GPIO pin becomes high.
        /// Will throw an exception if the program is not running on a Pi 
        /// or an invalid pin number is used. 
        /// </summary>
        /// <param name="pinNumber">pin number to wait for</param>
        public static void WaitForPinHigh(int pinNumber)
        {
            manager.WaitForPinHigh(pinNumber);
        }

        #region Game Engine 

        /// <summary>
        /// Add the given sprite to the game engine. The sprite will
        /// be drawn each time the game engine redraws.
        /// </summary>
        /// <param name="sprite">sprite to be added</param>
        public static void AddSpriteToGame(ISnapsSprite sprite)
        {
            manager.AddSpriteToGame(sprite);
        }

        /// <summary>
        /// Remove the given sprite from the game engine. 
        /// The sprite will no longer be drawn. If the sprite
        /// is not in the engine the method has no effect.
        /// </summary>
        /// <param name="sprite">sprite to be removed</param>
        public static void RemoveSpriteFromGame(ISnapsSprite sprite)
        {
            manager.RemoveSpriteFromGame(sprite);
        }

        /// <summary>
        /// Start the game engine running
        /// </summary>
        /// <param name="fullScreen">true if the game engine is to run fullscreen
        /// </param>
        /// <param name="framesPerSecond">rate the game is to update in frames per second</param>
        public static void StartGameEngine(bool fullScreen, int framesPerSecond)
        {
            manager.StartGameEngine(fullScreen, framesPerSecond);
        }

        /// <summary>
        /// Get the framerate set for the game engine
        /// </summary>
        /// <returns>framerate as a fraction of a second between each frame</returns>
        public static double GetFrameRate()
        {
            return manager.GetFrameRate();
        }

        /// <summary>
        /// Get the width of the game viewport in pixels
        /// </summary>
        public static double GameViewportWidth
        {
            get
            {
                return manager.GameViewportWidth;
            }
        }

        /// <summary>
        /// Get the height of the game viewport in pixels
        /// </summary>
        public static double GameViewportHeight
        {
            get
            {
                return manager.GameViewportHeight;
            }
        }

        /// <summary>
        /// Draw the game page. The method draws the game
        /// objects and then pauses for the time required to
        /// ensure that successive calls of DrawGamePage will
        /// complete at the given frame rate.
        /// </summary>
        /// <returns>true if the draw completed within the frame time
        /// false if the draw action took longer than the frame time</returns>
        public static bool DrawGamePage()
        {
            return manager.DrawGamePage();
        }

        /// <summary>
        /// Read the gamepad which is implemented as a screen area, Xbox 360 or Xbox One controller,
        /// or keyboard keys
        /// </summary>
        /// <returns>true if the up area in the screen gamepad is being clicked with the mouse, 
        /// touched, or selected with a pen or the up key is pressed</returns>
        public static bool GetUpGamepad()
        {
            return manager.GetUpGamepad();
        }

        /// <summary>
        /// Read the gamepad which is implemented as a screen area, Xbox 360 or Xbox One controller,
        /// or keyboard keys
        /// </summary>
        /// <returns>true if the down area in the screen gamepad is being clicked with the mouse, 
        /// touched, or selected with a pen or the down key is pressed</returns>
        public static bool GetDownGamepad()
        {
            return manager.GetDownGamepad();
        }

        /// <summary>
        /// Read the gamepad which is implemented as a screen area, Xbox 360 or Xbox One controller,
        /// or keyboard keys
        /// </summary>
        /// <returns>true if the left area in the screen gamepad is being clicked with the mouse, 
        /// touched, or selected with a pen or the left key is pressed</returns>
        public static bool GetLeftGamepad()
        {
            return manager.GetLeftGamepad();
        }

        /// <summary>
        /// Read the gamepad which is implemented as a screen area, Xbox 360 or Xbox One controller,
        /// or keyboard keys
        /// </summary>
        /// <returns>true if the right area in the screen gamepad is being clicked with the mouse, 
        /// touched, or selected with a pen or the right key is pressed</returns>
        public static bool GetRightGamepad()
        {
            return manager.GetRightGamepad();
        }

        /// <summary>
        /// Read the gamepad which is implemented as a screen area, Xbox 360 or Xbox One controller,
        /// or keyboard keys
        /// </summary>
        /// <returns>true if the fire area in the screen gamepad is being clicked with the mouse, 
        /// touched, or selected with a pen or the space bar is pressed</returns>
        public static bool GetFireGamepad()
        {
            return manager.GetFireGamepad();
        }

        #endregion
    }
}
