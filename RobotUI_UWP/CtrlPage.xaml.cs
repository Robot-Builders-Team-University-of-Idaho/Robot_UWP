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


namespace RobotUI_UWP
{
    /// <summary>
    /// 
    /// This page connects to an Xbox controller, and then using its inputs, passes thoes values through inverse kinematics
    /// to generate the desired angles for the robot arm. These values then get sent to the Arduino through the serial port.
    /// 
    /// </summary>
    /// 

    public sealed partial class CtrlPage : Page
    {
        static string portName2;
        static bool printing2 = true;
        SerialPort port2 = new SerialPort("COM1", 115200);

        static double th1, th2, th3;
        static double x = 1;
        static double y = 0;
        static double z = 1;
        static double th4 = 90.0;
        static double th5 = 90.0;
        static double th6 = 90.0;

        private Gamepad _Gamepad = null;
        
        public CtrlPage()
        {
            this.InitializeComponent();
        }

        // returns to home page
        private void ReturnPage(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        
        // checks for any inputs from the controller, and runs the kinematic function, updating the angles
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
                    if (Math.Round(reading.LeftTrigger, 0) + th5 < 180)
                    {
                        th5 += Math.Round(reading.LeftTrigger, 0);
                    }

                    tbRightTrigger.Text = Math.Round(reading.RightTrigger, 1).ToString();
                    if (-Math.Round(reading.RightTrigger, 0) + th5 > 0)
                    {
                        th5 -= Math.Round(reading.RightTrigger, 0);
                    }

                    tbLeftThumbstickX.Text = Math.Round(reading.LeftThumbstickX, 1).ToString();
                    if (Math.Round(reading.LeftThumbstickX, 0) * 0.005 + x < 2 && Math.Round(reading.LeftThumbstickX, 0) * 0.005 + x > 0)
                    {
                        x += Math.Round(reading.LeftThumbstickX, 0) * 0.005;
                        x_text.Text = " " + x.ToString();
                    }

                    tbLeftThumbstickY.Text = Math.Round(reading.LeftThumbstickY, 1).ToString();
                    if (Math.Round(reading.LeftThumbstickY, 0) * 0.005 + y < 2 && Math.Round(reading.LeftThumbstickY, 0) * 0.005 + y > 0)
                    {
                        y += Math.Round(reading.LeftThumbstickY, 0) * 0.005;
                        y_text.Text = " " + y.ToString();
                    }

                    tbRightThumbstickX.Text = Math.Round(reading.RightThumbstickX, 1).ToString();
                    if (Math.Round(reading.RightThumbstickX, 0) * 0.5 + th4 < 180 && Math.Round(reading.RightThumbstickX, 0) * 0.5 + th4 > 0)
                    {
                        th4 += Math.Round(reading.RightThumbstickX, 0) * 0.5;
                    }

                    tbRightThumbstickY.Text = Math.Round(reading.RightThumbstickY, 1).ToString();
                    if (Math.Round(reading.RightThumbstickY, 0) * 0.005 + z < 2 && Math.Round(reading.RightThumbstickY, 0) * 0.005 + z > 0)
                    {
                        z += Math.Round(reading.RightThumbstickY, 0) * 0.005;
                        z_text.Text = " " + z.ToString();
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

                Debug.WriteLine(Size((int)th1) + " " + Size((int)th2) + " " + Size((int)th3) + " " + " " + Size((int)th4) + " " + " " + Size((int)th5) + " " + " " + Size((int)th6) + " ");
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

        // sets the desired port number from the dropdown
        private void SelectPort2(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                portName2 = ((ComboBoxItem)ChosenPort2.SelectedItem).Content.ToString();
                Debug.WriteLine(portName2);
            }
            catch { }

        }

        // Connects to the serial port
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

        // Closes the serial port
        private void DisconnectFromPort2(object sender, RoutedEventArgs e)
        {
            printing2 = false;
            if (port2.IsOpen)
            {
                port2.Close();
                Debug.WriteLine("port has been closed");
            }
            
        }

        // writes the angles to the serial port
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

        // converts the incoming int into a 3 character long string
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

        // Inverse Kinematics for the robot arm
        private string Kinematics(double x, double y, double z)
        {
            if (Math.Sqrt(x * x + y * y + (z-1) * (z - 1)) < 2 && Math.Sqrt(x * x + y * y + (z - 1) * (z - 1)) > 0.1)
            {
                double rp, B;
                double l1 = 1;
                double l2 = 1;
                double l3 = 1;

                rp = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z - l1, 2));
                th1 = Math.Atan2(y, x);
                B = Math.Acos((Math.Pow(rp, 2) - Math.Pow(l3, 2) - Math.Pow(l2, 2)) / (-2 * l3 * l2));
                th3 = Math.PI - B;
                th2 = Math.Asin((z - l1) / rp) + Math.Atan2((l3 * Math.Sin(th3)), (l2 + l3 * Math.Cos(th3)));

                th1 = Math.Clamp(th1 * 180 / Math.PI, 0, 180);
                th2 = Math.Clamp(th2 * 180 / Math.PI, 0, 180);
                //th3 = Math.Clamp(th3 * 180 / Math.PI, 0, 180); // Use this only when debugging/checking values
                th3 = Math.Clamp(th3 * 180 / Math.PI - th2, 0, 180);

                return Size((int)Math.Round(th1)) + Size((int)Math.Round(th2)) + Size((int)Math.Round(th3));
            }
            else { return Size((int)th1) + Size((int)th2) + Size((int)th3); }
        }

    }
}
