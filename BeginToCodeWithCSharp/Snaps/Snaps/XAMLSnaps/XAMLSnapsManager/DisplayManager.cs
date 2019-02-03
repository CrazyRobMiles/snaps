using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

using SnapsLibrary;
using Windows.Foundation;
using Windows.UI.Xaml;

using System.Reflection;
using System.Threading;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace XAMLSnaps
{
    #region Display Build

    /*

            +------------------------------------------+
            |     Background                           |
            |  +------------------------------------------+ 
            |  |    Image                                 |
            |  |  +------------------------------------------+
            |  |  |    Drawing                               |
            |  |  |  +------------------------------------------+
            |  |  |  |    Messages and Buttons                  |
            |  |  |  |  +------------------------------------------+ 
            |  |  |  |  |   System                                 |
            |  |  |  |  |                                          |
            |  |  |  |  |                                          |
            |  |  |  |  |                                          |
            |  |  |  |  |                                          |
            +  |  |  |  |                                          |
               |  |  |  |                                          |
               +  |  |  |                                          |
                  |  |  |                                          |
                  +  |  |                                          |
                     +  |                                          |
                        +------------------------------------------+ 
    
        
        
        


            |           0          |          1        |
         -- +------------------------------------------+
         0  |                    Title                 |      Grid.SetColumnSpan(titleTextBlock, 2); Grid.SetColumn(titleTextBlock, 0); Grid.SetRow(titleTextBlock, 0);
            +------------------------------------------+      Grid.SetColumnSpan(backgroundImage, 2); Grid.SetRowSpan(backgroundImage, 4); Grid.SetRow(backgroundImage, 0);Grid.SetColumn(backgroundImage, 0);
         1  |            Messages and buttons          |
            |                                          |
            +------------------------------------------+ 
         2  |                                          |
            |                                          |
            +------------------------------------------+ 
            |                                          |
            |                                          |
        
        
        
        */


    public partial class SnapsManager
    {
        const int BACKGROUND_LAYER = 0;
        const int IMAGE_LAYER = 1;
        const int DRAWING_LAYER = 2;
        const int MESSAGE_AND_BUTTON_LAYER = 3;
        const int SYSTEM_LAYER = 4;
        const int TOUCH_LAYER = 5;

        public const int DEBUG_VISIBLE_BACKGROUNDS = 1;
        public const int DEBUG_SHOW_TEST_BUTTON = 2;

        string version = "1.1.0";

        public string Version
        {
            get
            {
                return version;
            }
        }

        void placeOnDisplayGrid(FrameworkElement element, int row, int column, int rowSpan, int columnSpan)
        {
            if (element == null) return;

            Grid.SetColumnSpan(element, columnSpan);
            Grid.SetRowSpan(element, rowSpan);
            Grid.SetRow(element, row);
            Grid.SetColumn(element, column);
            DisplayGrid.Children.Add(element);
        }

        protected void PlaceDisplayElement(FrameworkElement component, int row, int column, int rowSpan, int columnSpan)
        {
            placeOnDisplayGrid(component, row, column, rowSpan, columnSpan);
        }

        void buildDisplay(Grid xamlPanel, int debugOption)
        {
            /*
                     <Grid.ColumnDefinitions>
                        <ColumnDefinition Width="*"/>
                        <ColumnDefinition Width="auto"/>
                    </Grid.ColumnDefinitions>
                    <Grid.RowDefinitions>
                        <RowDefinition Height="*"/>
                        <RowDefinition Height="Auto"/>
                        <RowDefinition Height="Auto"/>
                        <RowDefinition Height="*"/>
                    </Grid.RowDefinitions>

             * * 
             */

            DisplayGrid = xamlPanel;

            ColumnDefinition col0 = new ColumnDefinition();
            col0.Width = new GridLength(1, GridUnitType.Star);
            DisplayGrid.ColumnDefinitions.Add(col0);

            ColumnDefinition col1 = new ColumnDefinition();
            col1.Width = new GridLength(1, GridUnitType.Auto);
            DisplayGrid.ColumnDefinitions.Add(col1);

            RowDefinition row0 = new RowDefinition();
            row0.Height = new GridLength(1, GridUnitType.Star);
            DisplayGrid.RowDefinitions.Add(row0);

            RowDefinition row1 = new RowDefinition();
            row1.Height = new GridLength(1, GridUnitType.Auto);
            DisplayGrid.RowDefinitions.Add(row1);

            RowDefinition row2 = new RowDefinition();
            row2.Height = new GridLength(1, GridUnitType.Auto);
            DisplayGrid.RowDefinitions.Add(row2);

            RowDefinition row3 = new RowDefinition();
            row3.Height = new GridLength(1, GridUnitType.Star);
            DisplayGrid.RowDefinitions.Add(row3);

            setupDispatchers(debugOption);
        }

        #endregion

        #region Snaps Message Dispatcher

        List<IClearableDisplayElement> clearableComponents = new List<IClearableDisplayElement>();

        SnapsControlPanelStackPanel controlPanel;
        ReadPasswordStackPanel readPassword;
        ReadStringStackPanel readString;
        SoundAndSpeech soundAndSpeech;
        TapDetection tapDetection;
        TouchInputCanvas touchInput;
        GPIOSnap gpioSnap;

        // Buttons and text are displayed in the same panel by default
        ButtonPanel buttonPanel = null;
        DisplayTextBlock displayTextBlock = null;
        StackPanel buttonsAndText = null;
        SnapsGraphicsCanvas graphicsCanvas;
        //Image backgroundImage = null;
        public SolidColorBrush BackgroundBrush = new SolidColorBrush(Colors.White);

        TextBlock titleTextBlock = null;
        Brush originalTitleBrush;

        JoyPadPanel joyPad = null;

        void setupDispatchers(int debugOption)
        {
            graphicsCanvas = new SnapsGraphicsCanvas(this);
            clearableComponents.Add(graphicsCanvas);
            PlaceDisplayElement(graphicsCanvas, row: 0, column: 0, rowSpan: 4, columnSpan: 2);

            joyPad = new JoyPadPanel(this);
            joyPad.Visibility = Visibility.Collapsed;
            PlaceDisplayElement(joyPad, row: 3, column: 0, rowSpan: 1, columnSpan: 2);

            titleTextBlock = new TextBlock();
            titleTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
            titleTextBlock.VerticalAlignment = VerticalAlignment.Bottom;
            titleTextBlock.FontSize = 40;
            titleTextBlock.Text = "";
            originalTitleBrush = titleTextBlock.Foreground;
            PlaceDisplayElement(titleTextBlock,row: 0, column: 0, rowSpan: 1, columnSpan: 2);

            readPassword = new ReadPasswordStackPanel(this);
            PlaceDisplayElement(readPassword, row: 1, column: 0, rowSpan: 3, columnSpan: 2);
            readPassword.Visibility = Visibility.Collapsed;

            readString = new ReadStringStackPanel(this);
            PlaceDisplayElement(readString, row: 1, column: 0, rowSpan: 3, columnSpan: 2);
            readString.Visibility = Visibility.Collapsed;

            buttonPanel = new ButtonPanel(this);
            clearableComponents.Add(buttonPanel);

            displayTextBlock = new DisplayTextBlock(this);
            clearableComponents.Add(displayTextBlock);

            buttonsAndText = new StackPanel();
            buttonsAndText.Children.Add(displayTextBlock);
            buttonsAndText.Children.Add(buttonPanel);
            PlaceDisplayElement(buttonsAndText, row: 1, column: 0, rowSpan: 1, columnSpan: 2);

            soundAndSpeech = new SoundAndSpeech(this);
            DisplayGrid.Children.Add(soundAndSpeech.SoundOutputElement);

            controlPanel = new SnapsControlPanelStackPanel(this);
            PlaceDisplayElement(controlPanel, row: 3, column: 0, rowSpan: 1, columnSpan: 2);
            clearableComponents.Add(controlPanel);

            tapDetection = new TapDetection(this);

            touchInput = new TouchInputCanvas(this);
            PlaceDisplayElement(touchInput, row: 0, column: 0, rowSpan: 4, columnSpan: 2);
            clearableComponents.Add(touchInput);

            try {
                gpioSnap = new GPIOSnap();
            }
            catch
            {
                gpioSnap = null;
            }

        }

        #region SNAPS bindings

        #region Image Snaps

        public bool DisplayBackground(string imageURL)
        {
            return DoDisplayUrlImage(imageURL, graphicsCanvas.BackgroundImage); 
        }

        public bool DisplayImageFromUrl(string imageURL)
        {
            return DoDisplayUrlImage(imageURL, graphicsCanvas.DisplayImage);
        }

        public bool DisplaySnapsImage(SnapsImage image)
        {
            return DoDisplaySnapsImage(image, graphicsCanvas.DisplayImage);
        }

        public bool PickImage()
        {
            return PickAndDisplayImage();
        }

        public bool PickAndDisplayImage()
        {
            AutoResetEvent dialogCompleteEvent = new AutoResetEvent(false);

            StorageFile file = null;
            InvokeOnUIThread(
                async () =>
                {
                    FileOpenPicker openPicker = new FileOpenPicker();
                    openPicker.ViewMode = PickerViewMode.Thumbnail;
                    openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                    openPicker.FileTypeFilter.Add(".jpg");
                    openPicker.FileTypeFilter.Add(".jpeg");
                    openPicker.FileTypeFilter.Add(".png");

                    file = await openPicker.PickSingleFileAsync();

                    dialogCompleteEvent.Set();
                }
            );

            dialogCompleteEvent.WaitOne();

            if (file != null)
            {
                DoDisplayFileImage(file, graphicsCanvas.DisplayImage);
            }

            return file != null;
        }

        public List<SnapsImage> PickImages()
        {
            List<SnapsImage> result = new List<SnapsImage>();

            AutoResetEvent dialogCompleteEvent = new AutoResetEvent(false);

            InvokeOnUIThread(
                async () =>
                {
                    FileOpenPicker openPicker = new FileOpenPicker();
                    openPicker.ViewMode = PickerViewMode.Thumbnail;
                    openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                    openPicker.FileTypeFilter.Add(".jpg");
                    openPicker.FileTypeFilter.Add(".jpeg");
                    openPicker.FileTypeFilter.Add(".png");

                    var files = await openPicker.PickMultipleFilesAsync();

                    foreach (StorageFile file in files)
                    {
                        SnapsImage image = new SnapsImage(file);
                        result.Add(image);
                    }

                    dialogCompleteEvent.Set();
                }
            );

            dialogCompleteEvent.WaitOne();

            return result;
        }

        public bool LoadGraphicsPNGImageFromLocalStore(string filename)
        {
            return LoadGraphicsPNGImageFromLocalStore(filename, graphicsCanvas.DisplayImage);
        }

        public bool DisplayBingImageOfTheDay()
        {
            return DoDisplayBingImageOfTheDay(graphicsCanvas.DisplayImage);
        }
        #endregion

        #region Graphics Snaps Bindings

        public void ClearGraphics()
        {
            graphicsCanvas.ClearGraphics();
        }

        public void SetDrawingColor(int red, int green, int blue)
        {
            graphicsCanvas.SetDrawingColor(red, green, blue);
        }

        public void SetDrawingColor(SnapsColor color)
        {
            graphicsCanvas.SetDrawingColor(color);
        }

        public void SetBackgroundColor(int red, int green, int blue)
        {
            graphicsCanvas.SetBackgroundColor(red, green, blue);
        }

        public void SetBackgroundColor(SnapsColor color)
        {
            graphicsCanvas.SetBackgroundColor(color);
        }

        public void DrawBlock(int x, int y, int width, int height)
        {
            graphicsCanvas.DrawBlock(x, y, width, height);
        }

        public void DrawDot(int x, int y, int width)
        {
            graphicsCanvas.DrawDot(x, y, width);
        }

        public void DrawDot(SnapsCoordinate pos, int width)
        {
            graphicsCanvas.DrawDot(pos.XValue, pos.YValue, width);
        }

        public void DrawLine(int x1, int y1, int x2, int y2)
        {
            graphicsCanvas.DrawLine(x1, y1, x2, y2);
        }

        public void DrawLine(SnapsCoordinate p1, SnapsCoordinate p2)
        {
            graphicsCanvas.DrawLine(p1.XValue, p1.YValue, p2.XValue, p2.YValue);
        }

        public SnapsCoordinate GetScreenSize()
        {
            return graphicsCanvas.GetScreenSize();
        }

        public void MakeLightPanel(int xCells, int yCells)
        {
            graphicsCanvas.MakeLightPanel(xCells, yCells);
        }

        public void SetPanelCell(int x, int y, int red, int green, int blue)
        {
            graphicsCanvas.SetPanelCell(x, y, red, green, blue);
        }

        public void SetPanelCell(int x, int y, SnapsColor panelColor)
        {
            graphicsCanvas.SetPanelCell(x, y, panelColor.RedValue, panelColor.GreenValue, panelColor.BlueValue);
        }

        public bool SaveGraphicsImageToFileAsPNG()
        {
            return graphicsCanvas.SaveGraphicsImageToFileAsPNG();
        }

        public void SaveVisualElementToLocalStorageAsPNG(string filename)
        {
            SaveVisualElementToLocalStorageAsPNG(graphicsCanvas, filename);
        }

        #endregion

        #region Text Input

        public string ReadPassword(string prompt)
        {
            return readPassword.DoReadPassword(prompt);
        }

        public string ReadString(string prompt)
        {
            return readString.DoReadString(prompt);
        }

        public string ReadMultiLineString(string prompt)
        {
            return readString.DoReadMultiLineString(prompt);
        }

        #endregion

        #region Text Display

        public void SetTextColor(int red, int green, int blue)
        {
            displayTextBlock.DoSetTextColor(red, green, blue);
        }

        public void SetTextColor(SnapsColor color)
        {
            displayTextBlock.DoSetTextColor(color);
        }

        public void DisplayString(string message, SnapsTextAlignment alignment, SnapsFadeType fadeType, int size )
        {
            displayTextBlock.DoDisplayString(message, alignment, fadeType, size );
        }

        public void SetDisplayStringWidth(double width)
        {
            displayTextBlock.SetDisplayStringWidth(width);
        }

        public void SetDisplayStringSize(double size)
        {
            displayTextBlock.SetDisplayStringSize(size);
        }

        public void DisplayString(string message)
        {
            displayTextBlock.DoDisplayString(message);
        }

        #endregion

        #region Buttons

        public string SelectFromButtonArray(string[] buttonTexts)
        {
            return buttonPanel.DoSelectFromButtonArray(buttonTexts);
        }

        public string WaitForButton(string prompt)
        {
            return buttonPanel.DoWaitForButton(prompt);
        }

        public void SetButtonColor(int red, int green, int blue)
        {
            buttonPanel.DoSetButtonColor(red, green, blue);
        }

        public void SetButtonColor(SnapsColor color)
        {
            buttonPanel.DoSetButtonColor(color);
        }


        #endregion

        #region Title String

        public void SetTitleColor(int red, int green, int blue)
        {
            InvokeOnUIThread(
                () =>
                {
                    Color newBackground = Color.FromArgb(
                        255,
                        SnapsManager.ColorClamp(red),
                        SnapsManager.ColorClamp(green),
                        SnapsManager.ColorClamp(blue));
                    SolidColorBrush brush = new SolidColorBrush(newBackground);
                    titleTextBlock.Foreground = brush;
                }
            );
        }

        public void SetTitleColor(SnapsColor color)
        {
            SetTitleColor(color.RedValue, color.GreenValue, color.BlueValue);
        }

        public void SetTitleString(string title)
        {
            AutoResetEvent gotTitleStringEvent = new AutoResetEvent(false);

            InvokeOnUIThread(
                async () =>
                {
                    if (titleTextBlock.Opacity == 1)
                        await FadeElements.FadeElementOpacityAsync(titleTextBlock, 1, 0, new TimeSpan(0, 0, 0, 0, 200));
                    titleTextBlock.Text = title;
                    await FadeElements.FadeElementOpacityAsync(titleTextBlock, 0, 1, new TimeSpan(0, 0, 0, 0, 200));
                    gotTitleStringEvent.Set();
                }
            );

            gotTitleStringEvent.WaitOne();
        }

        public void ClearTitleString()
        {
            InvokeOnUIThread(
                () =>
                {
                    titleTextBlock.Text = "";
                    titleTextBlock.Foreground = originalTitleBrush;
                }
            );
        }

        #endregion

        #region Tapped handlers

        public void ClearScreenTappedFlag()
        {
            tapDetection.ClearScreenTappedFlag();
        }

        public bool ScreenHasBeenTapped()
        {
            return tapDetection.ScreenHasBeenTapped();
        }

        #endregion

        #region Touch Input

        public SnapsCoordinate GetTappedCoordinate()
        {
            return touchInput.GetTappedCoordinate();
        }

        public SnapsCoordinate GetDraggedCoordinate()
        {
            return touchInput.GetDraggedCoordinate();
        }

        #endregion

        #region Sound and Speech

        public void SpeakString(string message)
        {
            soundAndSpeech.SpeakString(message);
        }

        public void PlaySoundEffect(string effectName)
        {
            soundAndSpeech.PlaySoundEffect(effectName);
        }

        public void PlaySoundEffectNoWait(string effectName)
        {
            soundAndSpeech.PlaySoundEffectNoWait(effectName);
        }

        public void PlayNote(int pitch, double duration)
        {
            soundAndSpeech.PlayNote(pitch, duration);
        }

        public string SelectSpokenPhrase(string prompt, string[] phrases)
        {
            return soundAndSpeech.SelectSpokenWord(prompt, phrases);
        }

        #endregion

        #region Display Clear

        void clearDisplay()
        {
            // first fade out the display

            AutoResetEvent fadeCompleteEvent = new AutoResetEvent(false);

            InvokeOnUIThread(
                async () =>
                {
                    if (DisplayGrid.Opacity == 1)
                        await FadeElements.FadeElementOpacityAsync(DisplayGrid, 1, 0, new TimeSpan(0, 0, 0, 0, 200));

                    fadeCompleteEvent.Set();
                }
            );

            fadeCompleteEvent.WaitOne();

            ClearGameEngine();

            InvokeOnUIThread(
               async () =>
                {
                    // clear the background image
                    this.graphicsCanvas.BackgroundImage.Source = null;

                    // clear the title string
                    titleTextBlock.Text = "";
                    titleTextBlock.Foreground = originalTitleBrush;

                    foreach (IClearableDisplayElement element in clearableComponents)
                        element.Clear();

                    if (DisplayGrid.Opacity == 0)
                        await FadeElements.FadeElementOpacityAsync(DisplayGrid, 0, 1, new TimeSpan(0, 0, 0, 0, 200));
                    fadeCompleteEvent.Set();
                }
            );

            fadeCompleteEvent.WaitOne();
        }

        #endregion

        #region GPIO

        public void WriteToPin(int pinNumber, bool value)
        {
            gpioSnap.WriteToPin(pinNumber, value);
        }

        public bool ReadFromPin(int pinNumber)
        {
            return gpioSnap.ReadFromPin(pinNumber);
        }

        public void WaitForPinHigh(int pinNumber)
        {
            gpioSnap.WaitForPinHigh(pinNumber);
        }

        #endregion

        #endregion

    }
    #endregion
}