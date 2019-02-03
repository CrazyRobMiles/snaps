using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI;
using SnapsLibrary;

namespace XAMLSnaps
{
    public class ReadStringStackPanel : StackPanel, IClearableDisplayElement
    {
        SnapsManager manager;

        TextBlock InputPromptTextBlock;
        StackPanel UserInputStackPanel;
        TextBox TextInputTextBox;
        Button ReadTextButton;

        double TextBoxSingleLineHeight = 40;
        double TextBoxMultiLineHeight = 150;

        public ReadStringStackPanel(SnapsManager manager)
        {
            this.manager = manager;
            this.Opacity = 0;
            this.VerticalAlignment = VerticalAlignment.Center;
            this.HorizontalAlignment = HorizontalAlignment.Center;
            this.Margin = new Thickness(10, 10, 10, 10);
            this.Orientation = Orientation.Vertical;

            InputPromptTextBlock = new TextBlock();
            InputPromptTextBlock.Text = "Prompt for input goes here";
            InputPromptTextBlock.FontSize = 25;
            this.Children.Add(InputPromptTextBlock);

            UserInputStackPanel = new StackPanel();
            UserInputStackPanel.Orientation = Orientation.Horizontal;
            this.Children.Add(UserInputStackPanel);

            /*
             * 
             *         <StackPanel Opacity="0" VerticalAlignment="Center" HorizontalAlignment="Center" Margin="10,10,10,10"  Orientation="Vertical" Name="InputStackPanel" Grid.ColumnSpan="2" Grid.Row="3" Visibility="Collapsed">
                        <TextBlock Name="InputPromptTextBlock" Text="Prompt for the input goes here" FontSize="25"></TextBlock>
                        <StackPanel Orientation="Horizontal">
                            <TextBox Width="400" IsEnabled="True" VerticalAlignment="Center" Margin="10,10,10,10" Name="TextInputTextBox" PlaceholderText="Enter Your Text"/>
                            <Button VerticalAlignment="Center" IsEnabled="True" Name="ReadTextButton" Margin="10,10,10,10" Content="Enter" Width="Auto"/>
                        </StackPanel>
                    </StackPanel>

             * 
             * */

            TextInputTextBox = new TextBox();

            TextInputTextBox.Width = 400;
            TextInputTextBox.Height = TextBoxSingleLineHeight;
            TextInputTextBox.VerticalAlignment = VerticalAlignment.Center;
            TextInputTextBox.Margin = new Thickness(10, 10, 10, 10);
            UserInputStackPanel.Children.Add(TextInputTextBox);

            ReadTextButton = new Button();
            ReadTextButton.VerticalAlignment = VerticalAlignment.Center;
            ReadTextButton.IsEnabled = true;
            ReadTextButton.Margin = new Thickness(10, 10, 10, 10);
            ReadTextButton.Content = "Enter";
            ReadTextButton.Width = double.NaN;
            this.Children.Add(ReadTextButton);
        }

        public async Task<string> ReadStringAsync(string prompt)
        {
            string result = "";

            var tcs = new TaskCompletionSource<object>();

            RoutedEventHandler clickLambda = async (s, e) =>
            {
                result = TextInputTextBox.Text;
                await FadeElements.FadeElementOpacityAsync(this, 1, 0, new TimeSpan(0, 0, 0, 0, 100));
                this.Visibility = Visibility.Collapsed;
                tcs.TrySetResult(null);
            };

            KeyEventHandler keyDownLambda = async (s, e) =>
            {
                if (e.Key == Windows.System.VirtualKey.Enter)
                {
                    result = TextInputTextBox.Text;
                    await FadeElements.FadeElementOpacityAsync(this, 1, 0, new TimeSpan(0, 0, 0, 0, 100));
                    this.Visibility = Visibility.Collapsed;
                    tcs.TrySetResult(null);
                }
            };

            try
            {
                TextInputTextBox.Text = "";
                InputPromptTextBlock.Text = prompt;
                this.Visibility = Visibility.Visible;
                await FadeElements.FadeElementOpacityAsync(this, 0, 1, new TimeSpan(0, 0, 1));
                ReadTextButton.Click += clickLambda;
                TextInputTextBox.KeyDown += keyDownLambda;
                TextInputTextBox.Focus(FocusState.Keyboard);
                await tcs.Task;
            }
            finally
            {
                ReadTextButton.Click -= clickLambda;
                TextInputTextBox.KeyDown -= keyDownLambda;
                await FadeElements.FadeElementOpacityAsync(this, 1, 0, new TimeSpan(0, 0, 1));
                this.Visibility = Visibility.Collapsed;
            }

            return result;
        }

        public string DoReadMultiLineString(string prompt)
        {
            AutoResetEvent gotTextEvent = new AutoResetEvent(false);
            string result = "";
            manager.InvokeOnUIThread(
                async () =>
                {
                    TextInputTextBox.AcceptsReturn = true;
                    TextInputTextBox.Height = TextBoxMultiLineHeight;
                    result = await ReadStringAsync(prompt);
                    TextInputTextBox.AcceptsReturn = false;
                    TextInputTextBox.Height = TextBoxSingleLineHeight;
                    gotTextEvent.Set();
                }
            );

            gotTextEvent.WaitOne();

            return result;
        }

        public string DoReadString(string prompt)
        {
            AutoResetEvent gotTextEvent = new AutoResetEvent(false);
            string result = "";
            manager.InvokeOnUIThread(
                async () =>
                {
                    TextInputTextBox.AcceptsReturn = false;
                    TextInputTextBox.Height = TextBoxSingleLineHeight;
                    result = await ReadStringAsync(prompt);
                    gotTextEvent.Set();
                }
            );

            gotTextEvent.WaitOne();

            return result;
        }

        public void Clear()
        {
            TextInputTextBox.Text = "";
        }
    }
}
