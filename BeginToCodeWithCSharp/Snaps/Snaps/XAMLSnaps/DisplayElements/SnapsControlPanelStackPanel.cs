using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using SnapsLibrary;
using Windows.UI.Xaml.Media;
using Windows.UI;
using System.Collections.Generic;
using Windows.System.Threading;

namespace XAMLSnaps
{
    public class SnapsControlPanelStackPanel : StackPanel, IClearableDisplayElement
    {
        SnapsManager manager;

        Button stopButton;
        Button snapAppButton;
        Button repeatAppButton;

        TextBlock versionTextBlock;

        ListView chapterListView;
        ListView demoSelectListView;

        public SnapsControlPanelStackPanel(SnapsManager manager)
        {
            this.manager = manager;

            this.Opacity = 0;
            this.VerticalAlignment = VerticalAlignment.Top;

            StackPanel DemoSelect = new StackPanel();
            DemoSelect.Orientation = Orientation.Horizontal;
            DemoSelect.BorderBrush = new SolidColorBrush(Colors.Gray);
            DemoSelect.BorderThickness = new Thickness(2);

            StackPanel ChapterSelection = new StackPanel();
            ChapterSelection.Orientation = Orientation.Vertical;
            DemoSelect.Children.Add(ChapterSelection);

            TextBlock chapterTitleTextBlock = new TextBlock();
            chapterTitleTextBlock.Text = "Folder";
            ChapterSelection.Children.Add(chapterTitleTextBlock);

            chapterListView = new ListView();
            List<string> foldernames = manager.GetDemoFolders();
            chapterListView.ItemsSource = manager.GetDemoFolders();
            chapterListView.SelectionChanged += ChapterListView_SelectionChanged;
            chapterListView.Height = 200;
            chapterListView.Width = 400;
            ChapterSelection.Children.Add(chapterListView);

            StackPanel DemoSelection = new StackPanel();
            DemoSelection.Orientation = Orientation.Vertical;
            DemoSelect.Children.Add(DemoSelection);

            TextBlock exampleTitleTextBlock = new TextBlock();
            exampleTitleTextBlock.Text = "Snaps apps";
            DemoSelection.Children.Add(exampleTitleTextBlock);

            demoSelectListView = new ListView();
            demoSelectListView.Height = 200;
            demoSelectListView.Width = 400;
            demoSelectListView.SelectionChanged += DemoSelectListView_SelectionChanged;
            DemoSelection.Children.Add(demoSelectListView);

            DemoSelect.VerticalAlignment = VerticalAlignment.Center;
            DemoSelect.HorizontalAlignment = HorizontalAlignment.Center;

            this.Children.Add(DemoSelect);

            StackPanel buttonsStackPanel = new StackPanel();
            buttonsStackPanel.Orientation = Orientation.Horizontal;
            buttonsStackPanel.Margin = new Thickness(10, 10, 10, 10);
            buttonsStackPanel.Width = double.NaN;
            buttonsStackPanel.VerticalAlignment = VerticalAlignment.Center;
            buttonsStackPanel.HorizontalAlignment = HorizontalAlignment.Center;

            stopButton = new Button();
            stopButton.Name = "StopButton";
            stopButton.Margin = new Thickness(10, 10, 10, 10);
            stopButton.VerticalAlignment = VerticalAlignment.Bottom;
            stopButton.Content = "Stop";
            stopButton.HorizontalAlignment = HorizontalAlignment.Right;
            stopButton.Click += stopButton_Click;
            stopButton.Width = double.NaN; // select the auto width

            //buttonsStackPanel.Children.Add(stopButton);

            snapAppButton = new Button();
            snapAppButton.Name = "snapAppButton";
            snapAppButton.Margin = new Thickness(10, 10, 10, 10);
            snapAppButton.VerticalAlignment = VerticalAlignment.Bottom;
            snapAppButton.Content = "Run an app";
            snapAppButton.HorizontalAlignment = HorizontalAlignment.Right;
            snapAppButton.Click += SnapAppButton_Click;
            snapAppButton.Width = double.NaN; // select the auto width

            buttonsStackPanel.Children.Add(snapAppButton);

            repeatAppButton = new Button();
            repeatAppButton.Name = "repeatAppButton";
            repeatAppButton.Margin = new Thickness(10, 10, 10, 10);
            repeatAppButton.VerticalAlignment = VerticalAlignment.Bottom;
            repeatAppButton.Content = "Run that app again";
            repeatAppButton.HorizontalAlignment = HorizontalAlignment.Right;
            repeatAppButton.Click += repeatAppButton_Click;
            repeatAppButton.Width = double.NaN; // select the auto width

            buttonsStackPanel.Children.Add(repeatAppButton);

            this.Children.Add(buttonsStackPanel);

            versionTextBlock = new TextBlock();
            versionTextBlock.HorizontalAlignment = HorizontalAlignment.Right;
            versionTextBlock.VerticalAlignment = VerticalAlignment.Bottom;
            versionTextBlock.Text = "Version " + manager.Version;
            versionTextBlock.Margin = new Thickness(10, 10, 10, 10);

            this.Children.Add(versionTextBlock);

            this.Background = manager.BackgroundBrush;
        }

        string selectedAppName = null;

        private void DemoSelectListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (demoSelectListView.SelectedItem == null)
                return;

            selectedAppName = demoSelectListView.SelectedItem.ToString();
        }

        private void ChapterListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (chapterListView.SelectedItem == null)
                return;
            List<string> demos = manager.GetDemosForGroup(chapterListView.SelectedItem.ToString());
            demoSelectListView.ItemsSource = demos;
        }

        void stopButton_Click(object sender, RoutedEventArgs e)
        {
            manager.StopProgram();
        }

        private void disableButtons()
        {
            snapAppButton.IsEnabled = false;
            repeatAppButton.IsEnabled = false;
        }

        private void enableButtons()
        {
            snapAppButton.IsEnabled = true;
            repeatAppButton.IsEnabled = true;
        }

        private void SnapAppButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedAppName == null)
                return;
            disableButtons();
            manager.RunSelectedApp(selectedAppName);
        }

        private void repeatAppButton_Click(object sender, RoutedEventArgs e)
        {
            disableButtons();
            manager.RepeatSnapApp();
        }

        public void Clear()
        {
            enableButtons();
            this.Visibility = Visibility.Collapsed;
        }
    }
}
