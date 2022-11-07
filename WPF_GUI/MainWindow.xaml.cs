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
    public partial class MainWindow : Window
    {
        static string portName;
        static bool printing = false;
        private SerialPort port = new SerialPort("COM1", 115200);
        public List<string> PosList = new List<string>();
        static bool Recording = false;
        

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            this.DragMove();
        }

        private void CloseApp(object sender, RoutedEventArgs e)
        {
            try
            {
                port.Close();
                Task.Delay(100);
            }
            catch { }
            this.Close();
        }

        private void MinWindow(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
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

        private  async void PlayRecording(object sender, RoutedEventArgs e)
        {
            printing = false;
            Play.IsEnabled = false;
            for (int i = 0; i < PosList.Count; i++)
            {
                try { 
                    Debug.WriteLine(PosList[i]);
                    port.WriteLine(PosList[i]);
                }
                catch { }
                await Task.Delay(10);
                
            }
            Play.IsEnabled = true;
            printing = true;
            Tracking();
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

                    if (Recording)
                    {
                        PosList.Add(Size((int)Slider1.Value) + Size((int)Slider2.Value) + Size((int)Slider3.Value) +
                        Size((int)Slider4.Value) + Size((int)Slider5.Value) + Size((int)Slider6.Value));
                    }
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
