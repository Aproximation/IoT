using System;
using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MirrorApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private DispatcherTimer timer;
        public MainPage()
        {
            //PrmaryScreenResolution.ChangeResolution();
            
            this.InitializeComponent();
            Setup();
        }

        private void Setup()
        {
            var now = DateTime.Now;
            SetTime(now);
            SetDate(now);
            SetWeather();
            SetBottomText(); 

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(20);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void SetBottomText()
        {
            textBlock.Text = "Pięknie dzisiaj wyglądasz";
        }

        private async void SetWeather()
        {
            var response = await Weather.UpdateData();

            if (response == "ok")
            {
                BitmapImage bi = new BitmapImage();
                bi.UriSource = new Uri(Weather.IconUrl);
                image.Source = bi;

                conditionsText.Text = Weather.WeatherText;
                cityText.Text = Weather.CityName;
                windText.Text = String.Format("Wiatr: {0} {1}", Weather.WindSpeed, "km/h");
                degreesNumberText.Text = Weather.Temperature;
            }
            else
                textBlock.Text = response;
        }


        private void SetDate(DateTime now)
        {
            dateText.Text = String.Format("{0}, {1} {2}", DateTimeFormatInfo.CurrentInfo.GetDayName(now.DayOfWeek), now.Day, DateTimeFormatInfo.CurrentInfo.GetMonthName(now.Month));
        }

        private void SetTime(DateTime now)
        {
            timeText.Text = String.Format("{0} : {1}", now.ToString("HH"), now.ToString("mm"));
        }

        private void Timer_Tick(object sender, object e)
        {
            var now = DateTime.Now;
            SetTime(now);
            if (now.Hour % 6 == 0) {
                SetWeather();

                if (now.Hour == 0 && now.Minute == 0 && now.Second < 30)
                {
                    SetDate(now);
                    SetBottomText();
                }                
            }
        }
    }
}
