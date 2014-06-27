using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WindowsPhone_Bg_Service.Resources;

namespace WindowsPhone_Bg_Service
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Terminate();
        }

        private void PDF_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PdfDownload.xaml", UriKind.Relative));
        }

        private void Image_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/ImageDownload.xaml", UriKind.Relative));
        }

        private void TextFile_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/TextFileDownload.xaml", UriKind.Relative));
        }

    }
}