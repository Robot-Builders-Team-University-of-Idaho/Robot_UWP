using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace RobotUI_UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Home : Page
    {
        public Home()
        {
            this.InitializeComponent();
        }

        private void claw(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CtrlPage));
        }

        private void suction(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Compressor));
        }

        private void coffee(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CoffeeGrounds));
        }

        private void debug(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private async void OpenGit(object sender, RoutedEventArgs e)
        {
            // Set the URL of the website you want to open
            var uriBing = new Uri("https://github.com/Robot-Builders-Team-University-of-Idaho/Robot_UWP");

            // Use the Process.Start() method to open the website
            var success = await Launcher.LaunchUriAsync(uriBing);
        }
    }
}
