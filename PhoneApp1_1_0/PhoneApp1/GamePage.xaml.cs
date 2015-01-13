using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO;
using System.Xml;
using System.Windows.Media.Imaging;

namespace PhoneApp1
{
    public partial class Page1 : PhoneApplicationPage
    {
        private const string strConnectionString = @"isostore:/UsernameDB.sdf";
        String aux;

        public Page1()
        {
            InitializeComponent();
        }

        private void changePassButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/changePasswordPage.xaml", UriKind.Relative));
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (PhoneApplicationService.Current.State.ContainsKey("Text"))
                userTextBlock.Text = String.Format("Logged in as " + (string)PhoneApplicationService.Current.State["Text"]);
                aux = (string)PhoneApplicationService.Current.State["Text"];
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            // Called when a page becomes the active page in a frame
            base.OnNavigatedFrom(e);
            // Text is param, you can define anything instead of Text 
            // but remember you need to further use same param.
            PhoneApplicationService.Current.State["Text"] = aux;
        }

        private void backToLogin_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem currentCity = listBox1.SelectedItem as ListBoxItem;
            cityTextBlock.Text = currentCity.Content.ToString();
            string strWOEID = string.Empty;
            switch (cityTextBlock.Text)
            {
                case "Bucharest":
                    strWOEID = "2346624";
                    break;
                case "Constanta":
                    strWOEID = "870151";
                    break;
            }

            
            GetTodaysWeather(strWOEID);
        }

        private void GetTodaysWeather(string strWOEID)
        {
            try
            {
                Uri url = new Uri("http://weather.yahooapis.com/forecastrss?w=" + strWOEID + "&u=c");
                WebClient client = new WebClient();
                client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
                client.DownloadStringAsync(url);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

           
        }


        void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                StringReader stream = new StringReader(e.Result);
                XmlReader reader = XmlReader.Create(stream);
                reader.ReadToFollowing("yweather:condition");
                //Populate Temperature
                reader.MoveToAttribute("temp");
                weatherTextBlock.Text = reader.Value;

                //Set Image based on the code
                reader.MoveToAttribute("code");
                Uri url;
                url = new Uri("http://l.yimg.com/a/i/us/nws/weather/gr/" + reader.Value + "d.png");

                BitmapImage img = new BitmapImage(url);
                image1.Source = new BitmapImage(url);

                //Forecast

                reader.ReadToFollowing("yweather:forecast");
                reader.MoveToAttribute("day");
                todayTextBlock.Text = reader.Value;
                reader.MoveToAttribute("low");
                todayTextBlock.Text += " Low: " + reader.Value;
                reader.MoveToAttribute("high");
                todayTextBlock.Text += " High: " + reader.Value;

                reader.ReadToNextSibling("yweather:forecast");
                reader.MoveToAttribute("day");
                tommorowTextBlock.Text = reader.Value;
                reader.MoveToAttribute("low");
                tommorowTextBlock.Text += " Low: " + reader.Value;
                reader.MoveToAttribute("high");
                tommorowTextBlock.Text += " High: " + reader.Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

           

        }

        private void backgroundButton_Click(object sender, RoutedEventArgs e)
        {
            my_popup_xaml.IsOpen = true;
        }


        private void background1_Click(object sender, RoutedEventArgs e)
        {
            my_popup_xaml.IsOpen = false;
            ImageBrush background = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("/PhoneApp1;component/Images/Background.png", UriKind.Relative)),
                Stretch = Stretch.UniformToFill
            };
            
            LayoutRoot.Background = background;

        }

        private void background2_Click(object sender, RoutedEventArgs e)
        {
            my_popup_xaml.IsOpen = false;
            ImageBrush background = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("/PhoneApp1;component/Images/PicardPalm.jpg", UriKind.Relative)),
                Stretch = Stretch.UniformToFill
            };
            LayoutRoot.Background = background;
        }

       

       
 
    }
}   