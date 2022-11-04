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
        private SerialPort port;
        

        public MainWindow()
        {
            InitializeComponent();
            Debug.WriteLine("this is working");
        }

        private void SelectPort(object sender, SelectionChangedEventArgs e)
        {
            portName = ((ComboBoxItem)ChosenPort.SelectedItem).Content.ToString();
            Debug.WriteLine(portName);
        }
        
        private void ConnectToPort(object sender, RoutedEventArgs e)
        {
            port = new SerialPort(portName, 9600);
            try { 
                port.Open();
                Debug.WriteLine("opened port " + portName);
                }
            catch { }
            Task.Delay(100);
            Tracking();
        }

        private void DisconnectFromPort(object sender, RoutedEventArgs e)
        {
            if (port.IsOpen)
            {
                port.Close();
                Debug.WriteLine("port has been closed");
            }
        }

        private async void Tracking()
        {
            while (true)
            {
                Debug.WriteLine(Size((int)Slider1.Value));
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
