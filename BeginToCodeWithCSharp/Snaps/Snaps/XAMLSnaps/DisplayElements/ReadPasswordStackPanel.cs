using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI;
using SnapsLibrary;

namespace XAMLSnaps
{
    public class ReadPasswordStackPanel : StackPanel, IClearableDisplayElement
    {
        TextBlock PasswordInputPromptTextBlock;
        StackPanel UserPasswordInputStackPanel;
        PasswordBox PasswordInputPasswordBox;
        Button ReadPasswordButton;

        SnapsManager manager;

        public ReadPasswordStackPanel(SnapsManager manager)
        {
            this.manager = manager;
            this.Opacity = 0;
            this.VerticalAlignment = VerticalAlignment.Center;
            this.HorizontalAlignment = HorizontalAlignment.Center;
            this.Margin = new Thickness(10, 10, 10, 10);
            this.Orientation = Orientation.Vertical;

            PasswordInputPromptTextBlock = new TextBlock();
            PasswordInputPromptTextBlock.Text = "Prompt for input goes here";
            PasswordInputPromptTextBlock.FontSize = 25;
            this.Children.Add(PasswordInputPromptTextBlock);

            UserPasswordInputStackPanel = new StackPanel();
            UserPasswordInputStackPanel.Orientation = Orientation.Horizontal;
            this.Children.Add(UserPasswordInputStackPanel);

            /*
             * 
             *         <StackPanel Opacity="0" VerticalAlignment="Center" HorizontalAlignment="Center" Margin="10,10,10,10"  Orientation="Vertical" Name="PasswordInputStackPanel" Grid.ColumnSpan="2" Grid.Row="3" Visibility="Collapsed">
                        <TextBlock Name="PasswordInputPromptTextBlock" Text="Prompt for the input goes here" FontSize="25"></TextBlock>
                        <StackPanel Orientation="Horizontal">
                            <TextBox Width="400" IsEnabled="True" VerticalAlignment="Center" Margin="10,10,10,10" Name="TextInputTextBox" PlaceholderText="Enter Your Text"/>
                            <Button VerticalAlignment="Center" IsEnabled="True" Name="ReadTextButton" Margin="10,10,10,10" Content="Enter" Width="Auto"/>
                        </StackPanel>
                    </StackPanel>

             * 
             * */

            PasswordInputPasswordBox = new PasswordBox();

            PasswordInputPasswordBox.Width = 400;
            PasswordInputPasswordBox.VerticalAlignment = VerticalAlignment.Center;
            PasswordInputPasswordBox.Margin = new Thickness(10, 10, 10, 10);
            UserPasswordInputStackPanel.Children.Add(PasswordInputPasswordBox);

            ReadPasswordButton = new Button();
            ReadPasswordButton.VerticalAlignment = VerticalAlignment.Center;
            ReadPasswordButton.IsEnabled = true;
            ReadPasswordButton.Margin = new Thickness(10, 10, 10, 10);
            ReadPasswordButton.Content = "Enter";
            ReadPasswordButton.Width = double.NaN;
            UserPasswordInputStackPanel.Children.Add(ReadPasswordButton);
        }

        public async Task<string> ReadPasswordAsync(string prompt)
        {
            string result = "";

            var tcs = new TaskCompletionSource<object>();

            RoutedEventHandler clickLambda = async (s, e) =>
            {
                result = PasswordInputPasswordBox.Password;
                await FadeElements.FadeElementOpacityAsync(this, 1, 0, new TimeSpan(0, 0, 0, 0, 100));
                this.Visibility = Visibility.Collapsed;
                tcs.TrySetResult(null);
            };

            KeyEventHandler keyboardLambda = async (s, e) =>
            {
                if (e.Key == Windows.System.VirtualKey.Enter)
                {
                    result = PasswordInputPasswordBox.Password;
                    await FadeElements.FadeElementOpacityAsync(this, 1, 0, new TimeSpan(0, 0, 0, 0, 100));
                    this.Visibility = Visibility.Collapsed;
                    tcs.TrySetResult(null);
                }
            };

            try
            {
                PasswordInputPromptTextBlock.Text = prompt;
                PasswordInputPasswordBox.Password = "";
                this.Visibility = Visibility.Visible;
                await FadeElements.FadeElementOpacityAsync(this, 0, 1, new TimeSpan(0, 0, 1));
                ReadPasswordButton.Click += clickLambda;
                PasswordInputPasswordBox.KeyDown += keyboardLambda;
                await tcs.Task;
            }
            finally
            {
                ReadPasswordButton.Click -= clickLambda;
                PasswordInputPasswordBox.KeyDown -= keyboardLambda;
                await FadeElements.FadeElementOpacityAsync(this, 1, 0, new TimeSpan(0, 0, 1));
                this.Visibility = Visibility.Collapsed;
            }

            return result;
        }

        public string DoReadPassword(string prompt)
        {
            AutoResetEvent gotTextEvent = new AutoResetEvent(false);
            string result = "";
            manager.InvokeOnUIThread(
                async () =>
                {
                    result = await ReadPasswordAsync(prompt);
                    gotTextEvent.Set();
                }
            );

            gotTextEvent.WaitOne();

            return result;
        }

        public void Clear()
        {
            PasswordInputPasswordBox.Password = "";
        }
    }
}
