using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Gaming.Input;
using Windows.Devices.Enumeration;
using Windows.System;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace RobotUI_UWP
{
    public sealed partial class MainPage : Page
    {
        //Global Variables
        static string portName;
        static bool printing = false;
        private SerialPort port = new SerialPort("COM1", 115200);
        public List<string> PosList = new List<string>();
        static bool Recording = false;

        public MainPage()
        {
            this.InitializeComponent();

            ApplicationView.PreferredLaunchViewSize = new Size(650, 500);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
        }


        private void RecordPositions(object sender, RoutedEventArgs e)
        {
            if (Recording)
            {
                Recording = false;
                Record.Opacity = 1;
            }
            else
            {
                PosList = new List<string>();
                Recording = true;
                Record.Opacity = 0.5;
            }


        }

        private async void PlayRecording(object sender, RoutedEventArgs e)
        {
            printing = false;
            Play.IsEnabled = false;
            for (int i = 0; i < PosList.Count; i++)
            {
                try
                {
                    Debug.WriteLine(PosList[i]);
                    //port.WriteLine(PosList[i]);
                }
                catch { }
                await Task.Delay(10);

            }
            Play.IsEnabled = true;
            printing = true;
            Tracking();
        }

        private void ChangePage(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CtrlPage));
        }



        private void SelectPort(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                portName = ((ComboBoxItem)ChosenPort.SelectedItem).Content.ToString();
                Debug.WriteLine(portName);
            }
            catch { }

        }

        private void ConnectToPort(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!port.IsOpen)
                {
                    port = new SerialPort(portName, 115200);
                    port.Open();
                    Debug.WriteLine("opened port " + portName);
                }
            }
            catch { }
            Task.Delay(100);
            printing = true;
            Tracking();
        }

        private void CtrlConnect(object sender, RoutedEventArgs e)
        {

        }

        private void DisconnectFromPort(object sender, RoutedEventArgs e)
        {
            if (port.IsOpen)
            {
                port.Close();
                Debug.WriteLine("port has been closed");
            }
            printing = false;
        }

        private async void Tracking()
        {
            while (printing)
            {
                Debug.WriteLine(Size((int)Slider1.Value) + Size((int)Slider2.Value) + Size((int)Slider3.Value) +
                    Size((int)Slider4.Value) + Size((int)Slider5.Value) + Size((int)Slider6.Value));

                try
                {
                    port.WriteLine(Size((int)Slider1.Value) + Size((int)Slider2.Value) + Size((int)Slider3.Value) +
                    Size((int)Slider4.Value) + Size((int)Slider5.Value) + Size((int)Slider6.Value));
                }
                catch { printing = false; }

                await Task.Delay(10);
            }
        }

        private async void SavePos()
        {
            await Task.Delay(10);
        }

        private string Size(int x)
        {
            string output = x.ToString();
            if (output.Length == 1)
            {
                output = "00" + output;
            }
            else if (output.Length == 2)
            {
                output = "0" + output;
            }
            return output;
        }

        private async void OpenGit(object sender, RoutedEventArgs e)
        {
            // Set the URL of the website you want to open
            var uriBing = new Uri("https://github.com/Robot-Builders-Team-University-of-Idaho/Robot_UWP");

            // Use the Process.Start() method to open the website
            var success = await Launcher.LaunchUriAsync(uriBing);
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
