using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Gaming.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Gaming;
using Windows.UI.Xaml.Documents;
using Windows.System;
using System.Diagnostics;
using Windows.Networking;
using System.IO.Ports;
using System.ComponentModel.Design.Serialization;
using Windows.Devices.PointOfService;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace RobotUI_UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 

    public sealed partial class CtrlPage : Page
    {
        static string portName2;
        static bool printing2 = true;
        SerialPort port2 = new SerialPort("COM1", 115200);
        static double th1, th2, th3;
        static double x = 0;
        static double y = 0;
        static double z = 0;
        static double th4 = 0.0;
        static double th5 = 0.0;
        static double th6 = 0.0;
        
        private Gamepad _Gamepad = null;
        public CtrlPage()
        {
            this.InitializeComponent();
        }

        private void ReturnPage(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        

        private async void btnConnect_Click(object sender,RoutedEventArgs e)
        {
            Gamepad.GamepadAdded += Gamepad_GamepadAdded;
            Gamepad.GamepadRemoved += Gamepad_GamepadRemoved;

            while (true)
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    if (_Gamepad == null)
                    {
                        return;
                    }

                    // Get the current state
                    var reading = _Gamepad.GetCurrentReading();

                    tbLeftTrigger.Text = Math.Round(reading.LeftTrigger, 1).ToString();
                    if (Math.Round(reading.LeftTrigger, 1) + th4 < 180)
                    {
                        th4 += Math.Round(reading.LeftTrigger, 1);
                    }

                    tbRightTrigger.Text = Math.Round(reading.RightTrigger, 1).ToString();
                    if (-Math.Round(reading.RightTrigger, 1) + th4 > 0)
                    {
                        th4 -= Math.Round(reading.RightTrigger, 1);
                    }

                    tbLeftThumbstickX.Text = Math.Round(reading.LeftThumbstickX, 1).ToString();
                    if (Math.Round(reading.LeftThumbstickX, 1) * 0.01 + x < 2 && Math.Round(reading.LeftThumbstickX, 1) * 0.01 + x > 0)
                    {
                        x += Math.Round(reading.LeftThumbstickX, 1) * 0.001;
                    }

                    tbLeftThumbstickY.Text = Math.Round(reading.LeftThumbstickY, 1).ToString();
                    if (Math.Round(reading.LeftThumbstickY, 1) * 0.01 + y < 2 && Math.Round(reading.LeftThumbstickY, 1) * 0.01 + y > 0)
                    {
                        y += Math.Round(reading.LeftThumbstickY, 1) * 0.001;
                    }

                    tbRightThumbstickX.Text = Math.Round(reading.RightThumbstickX, 1).ToString();


                    tbRightThumbstickY.Text = Math.Round(reading.RightThumbstickY, 1).ToString();
                    if (Math.Round(reading.RightThumbstickY, 1) * 0.01 + z < 2 && Math.Round(reading.RightThumbstickY, 1) * 0.01 + z > 0)
                    {
                        z += Math.Round(reading.RightThumbstickY, 1) * 0.001;
                    }

                    tbButtons.Text = string.Empty;
                    tbButtons.Text += (reading.Buttons & GamepadButtons.A) == GamepadButtons.A ? "A " : "";
                    tbButtons.Text += (reading.Buttons & GamepadButtons.B) == GamepadButtons.B ? "B " : "";
                    tbButtons.Text += (reading.Buttons & GamepadButtons.X) == GamepadButtons.X ? "X " : "";
                    tbButtons.Text += (reading.Buttons & GamepadButtons.Y) == GamepadButtons.Y ? "Y " : "";
                    tbButtons.Text += (reading.Buttons & GamepadButtons.LeftShoulder) == GamepadButtons.LeftShoulder? "LeftShoulder " : "";
                    if (reading.Buttons == GamepadButtons.LeftShoulder && th6 + 0.5 < 180)
                    {
                        th6 += 0.5;
                    }

                    tbButtons.Text += (reading.Buttons & GamepadButtons.RightShoulder) == GamepadButtons.RightShoulder? "RightShoulder " : "";
                    if (reading.Buttons == GamepadButtons.RightShoulder && th6 - 0.5 > 0)
                    {
                        th6 -= 0.5;
                    }

                    tbButtons.Text += (reading.Buttons & GamepadButtons.LeftThumbstick) == GamepadButtons.LeftThumbstick? "LeftThumbstick " : "";
                    tbButtons.Text += (reading.Buttons & GamepadButtons.RightThumbstick) == GamepadButtons.RightThumbstick? "RightThumbstick " : "";
                    tbButtons.Text += (reading.Buttons & GamepadButtons.DPadLeft) == GamepadButtons.DPadLeft ? "DPadLeft " : "";
                    tbButtons.Text += (reading.Buttons & GamepadButtons.DPadRight) == GamepadButtons.DPadRight ? "DPadRight " : "";
                    tbButtons.Text += (reading.Buttons & GamepadButtons.DPadUp) == GamepadButtons.DPadUp ? "DPadUp " : "";
                    tbButtons.Text += (reading.Buttons & GamepadButtons.DPadDown) == GamepadButtons.DPadDown ? "DPadDown " : "";});
                Kinematics(x, y, z);
                await Task.Delay(TimeSpan.FromMilliseconds(5));
            }
        }
        private async void Gamepad_GamepadRemoved(object sender,Gamepad e)
        {
            _Gamepad = null;

            await Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal, () =>
                {
                    tbConnected.Text = "Controller removed";
                    //Return.IsEnabled = true;
                    btnConnect.IsEnabled = true;
                });
        }

        private async void Gamepad_GamepadAdded(object sender,Gamepad e)
        {
            
            _Gamepad = e;

            await Dispatcher.RunAsync(
            CoreDispatcherPriority.Normal, () =>
            {
                tbConnected.Text = "Controller added";
                //Return.IsEnabled = false;
                btnConnect.IsEnabled = false;
            });
        }

        private void SelectPort2(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                portName2 = ((ComboBoxItem)ChosenPort2.SelectedItem).Content.ToString();
                Debug.WriteLine(portName2);
            }
            catch { }

        }

        private void ConnectToPort2(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!port2.IsOpen)
                {
                    port2 = new SerialPort(portName2, 115200);
                    port2.Open();
                    Debug.WriteLine("opened port " + portName2);
                }
            }
            catch { }
            Task.Delay(100);
            printing2 = true;
            Tracking();
        }

        private void DisconnectFromPort2(object sender, RoutedEventArgs e)
        {
            printing2 = false;
            if (port2.IsOpen)
            {
                port2.Close();
                Debug.WriteLine("port has been closed");
            }
            
        }

        private async void Tracking()
        {
            while (printing2)
            {
                //Debug.WriteLine(Size((int)y));

                try
                {
                    if (th1 > 0 && th2 > 0 && th3 > 0 && th1 < 180 && th2 < 180 && th3 < 180)
                    {
                        port2.WriteLine(Kinematics(x, y, z) + Size((int)th4) + Size((int)th5) + Size((int)th6));
                    }
                }
                catch { printing2 = false; }

                await Task.Delay(10);
            }
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

        private string Kinematics(double x, double y, double z)
        {
            if (Math.Sqrt(x * x + y * y + z * z) < 2)
            {
                double r1, r2, alpha, beta, phi;
                double l1 = 1;
                //double l2 = 1;
                //double l3 = 1;

                th1 = Math.Atan2(y, x);
                r1 = Math.Sqrt(x * x + y * y);
                alpha = Math.Atan2(z, r1);
                r2 = Math.Sqrt(r1 * r1 + z * z);
                beta = Math.Acos(r2 / (l1 * 2));
                th2 = alpha + beta;
                phi = Math.PI - (2 * beta);
                th3 = Math.PI - phi + (Math.PI / 2);

                th1 = 180 - th1 * 180 / Math.PI;
                th2 = th2 * 180 / Math.PI;
                if ((th2 - 90) + 180 - (th3 * 180 / Math.PI - 90) > 0)
                {
                    th3 = (th2 - 90) + 180 - (th3 * 180 / Math.PI - 90);
                }
                else { th3 = 0; }
                Debug.WriteLine(th1.ToString() + " " + th2.ToString() + " " + th3.ToString() + " " + x.ToString() + 
                    " " + y.ToString() + " " + z.ToString());
                //Debug.WriteLine(Size((int)th1) + Size((int)th2) + Size((int)th3) + Size((int)th4) + Size((int)th5) + Size((int)th6) + "   " + x.ToString() + " " + y.ToString() + " " + z.ToString());
                return Size((int)th1) + Size((int)th2) + Size((int)th3);
            }
            else { return Size((int)th1) + Size((int)th2) + Size((int)th3); }
        }

    }
}
