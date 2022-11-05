using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Ports;
using System.Diagnostics;
using System.Threading;

namespace WPF_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string portName;
        static bool printing = false;
        private SerialPort port;
        

        public MainWindow()
        {
            InitializeComponent();
            Debug.WriteLine("this is working");
        }

        private void CloseApp(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SelectPort(object sender, SelectionChangedEventArgs e)
        {
            try { 
                portName = ((ComboBoxItem)ChosenPort.SelectedItem).Content.ToString(); 
                Debug.WriteLine(portName);
            }
            catch { }
            
        }
        
        private void ConnectToPort(object sender, RoutedEventArgs e)
        {  
            try { 
                port = new SerialPort(portName, 9600);
                port.Open();
                Debug.WriteLine("opened port " + portName);
                }
            catch { }
            Task.Delay(100);
            printing = true;
            Tracking();
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

        private string Size(int x)
        {
            string output = x.ToString();
            if(output.Length == 1)
            {
                output = "00" + output;
            }
            else if (output.Length == 2)
            {
                output = "0" + output;
            }
            return output;
        }
    }
}
