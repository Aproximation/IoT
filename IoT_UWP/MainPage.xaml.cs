using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Gpio;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace IoT_UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const int LED_PIN_NUM =17;
        private GpioPin pin;
        private SolidColorBrush redBrush = new SolidColorBrush(Windows.UI.Colors.Red);
        private SolidColorBrush grayBrush = new SolidColorBrush(Windows.UI.Colors.LightGray);
        private DispatcherTimer timer;

        public MainPage()
        {
            this.InitializeComponent();
            InitializeGPIO();

        }

        public void InitializeGPIO()
        {
            var gpio = GpioController.GetDefault();

            // Show an error if there is no GPIO controller
            if (gpio == null)
            {
                pin = null;
                return;
            }

            pin = gpio.OpenPin(LED_PIN_NUM);
            pin.Write(GpioPinValue.High);
            pin.SetDriveMode(GpioPinDriveMode.Output);
            LED.Fill = new SolidColorBrush(Windows.UI.Colors.Red);

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;
        }
        private void ChangeState()
        {
            if (pin.Read() == GpioPinValue.High)
            {
                pin.Write(GpioPinValue.Low);
                LED.Fill = new SolidColorBrush(Windows.UI.Colors.Gray);
            }
            else
            {
                pin.Write(GpioPinValue.High);
                LED.Fill = new SolidColorBrush(Windows.UI.Colors.Red);
            }
        }

        private void LED_Tapped(object sender, TappedRoutedEventArgs e)
        {

            ChangeState();

        }

        private void Timer_Tick(object sender, object e)
        {
            ChangeState();
        }

        private void toggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            var ts= sender as ToggleSwitch;
            if (ts.IsOn)
                timer.Start(); 
            else
                timer.Stop();
        }
    }
}
