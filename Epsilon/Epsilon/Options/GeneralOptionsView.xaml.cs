using Microsoft.Win32;
using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Controls;

namespace Epsilon.Options
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class GeneralOptionsView : UserControl
    {
        public GeneralOptionsView()
        {
            InitializeComponent();
        }

        private void BrowseButtonClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            string filter = "Default Cache (*.dat)|*.dat";
            TextBlock textBlock = new TextBlock();

            switch (((Button)sender).Name)
            {
                case "DefaultCacheButton":
                    textBlock = DefaultCacheTextBlock;
                    break;
                case "DefaultModPackageButton":
                    textBlock = DefaultModPackageTextBlock;
                    filter = "Mod Package (*.pak)|*.pak";
                    break;
                default:
                    break;
            }

            var dialog = new OpenFileDialog
            {
                Filter = filter,
                //FileName = textBlock.Text.Split('\\').Last().Split('.').First()
            };

            if (!string.IsNullOrEmpty((string)textBlock.ToolTip))
                dialog.InitialDirectory = new System.IO.FileInfo((string)textBlock.ToolTip).Directory.ToString();

            if (dialog.ShowDialog() == false)
                return;

            textBlock.ToolTip = dialog.FileName;
        }

        private void ClearFileClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            TextBlock textBlock = new TextBlock();

            switch (((Button)sender).Name)
            {
                case "DefaultCacheClear":
                    textBlock = DefaultCacheTextBlock;
                    break;
                case "DefaultPakClear":
                    textBlock = DefaultModPackageTextBlock;
                    break;
                default:
                    break;
            }

            textBlock.ToolTip = "";
        }

        private void GetCurrentPositionClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            double left = System.Windows.Application.Current.MainWindow.Left;
            double top = System.Windows.Application.Current.MainWindow.Top;

            StartupLeftTextBox.Text = left.ToString();
            StartupTopTextBox.Text = top.ToString();
        }

        private void GetCurrentDimensionsClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            double width = System.Windows.Application.Current.MainWindow.Width;
            double height = System.Windows.Application.Current.MainWindow.Height;

            StartupWidthTextBox.Text = width.ToString();
            StartupHeightTextBox.Text = height.ToString();
        }
    }
}
