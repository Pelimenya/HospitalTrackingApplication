using HospitalTrackingApplication.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace HospitalTrackingApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DispatcherTimer timer = new DispatcherTimer();
        private PersonLocations personLocations;
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await Get();
            AddEllipsesBasedOnNumber();
        }

        private async void Timer_Tick(object? sender, EventArgs e)
        {
            await Get();
            AddEllipsesBasedOnNumber();
        }

        private async Task Get()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync("http://localhost:8080/PersonLocations");
                    response.EnsureSuccessStatusCode();
                    string result = await response.Content.ReadAsStringAsync();
                    personLocations = JsonConvert.DeserializeObject<PersonLocations>(result);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddEllipse(StackPanel stackPanel, SolidColorBrush brush, string name)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Width = 10;
            ellipse.Height = 10;
            ellipse.Fill = brush;
            stackPanel.Children.Add(ellipse);
            ellipse.Name = name;
        }

        private void AddEllipsesBasedOnNumber()
        {
            try
            {
                foreach (var response in personLocations.response)
                {
                    SolidColorBrush brush = response.EliceColor;
                    string stackPanelName = "ckud" + response.lastSecurityPointNumber;
                    StackPanel stackPanel = (StackPanel)this.FindName(stackPanelName);
                    string name = "elipce" + response.personCode.ToString();
                    if (stackPanel != null && response.lastSecurityPointDirection == "in")
                    {
                        AddEllipse(stackPanel, brush, name);
                    }
                    if (stackPanel != null && response.lastSecurityPointDirection == "out")
                    {
                        DeleteEllipse(stackPanel, name);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeleteEllipse(StackPanel stackPanel, string name)
        {
            UIElement elementToRemove = (UIElement)stackPanel.FindName(name);
            if (elementToRemove != null)
            {
                stackPanel.Children.Remove(elementToRemove);
            }
        }
    }
}
