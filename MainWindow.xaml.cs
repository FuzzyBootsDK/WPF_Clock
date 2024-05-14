using System.Windows;
using System.Windows.Threading;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace WPF_CLock;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private static readonly HttpClient Client = new HttpClient();
    private bool _is24Hour = true;
    private bool _isEuropeanDateFormat = true;
    private bool _isMonthSpelledOut;
    private string _dateFormat = "dd-MM-yyyy";
    private string _timeFormat = "HH:mm:ss";
    private string _amOrPm = String.Empty;
    CultureInfo _currentCulture = new CultureInfo("da-DK");
    
    public MainWindow()
    {
        InitializeComponent();
        DispatcherTimer timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(1);
        timer.Tick += (s, a) => 
        {
            TimeTextBox.Text = DateTime.Now.ToString(_timeFormat) + " " + _amOrPm;
            DateTextBox.Text = DateTime.Now.ToString(_dateFormat, _currentCulture);
            string dayOfWeek = DateTime.Now.ToString("dddd", _currentCulture);
            dayOfWeek = char.ToUpper(dayOfWeek[0]) + dayOfWeek.Substring(1);
            DayTextBox.Text = dayOfWeek;
            CultureTextBlock.Text = _currentCulture.Name;
        };
        timer.Start();

        // Make an initial call to the API
        UpdateWeather();

        DispatcherTimer apiTimer = new DispatcherTimer();
        apiTimer.Interval = TimeSpan.FromHours(1);
        apiTimer.Tick += async (s, a) =>
        {
            await UpdateWeather();
        };
        apiTimer.Start();
    }

    private string ToggleEuropeanDateFormat()
    {
        return _isMonthSpelledOut ? "dd-MMMM-yyyy " : "dd-MM-yyyy";
    }

    private string ToggleAmericanDateFormat()
    {
        return _isMonthSpelledOut ? "MMMM-dd-yyyy" : "MM-dd-yyyy";
    }
    private string ToggleTimeFormat()
    {
        return _is24Hour ? "HH:mm:ss" : "hh:mm:ss";
    }
    
    private string DetermineAmOrPm()
    {
        if(_is24Hour)
        {
            // In 24-hour format, there is no AM or PM. So return empty string.
            return string.Empty;
        }
        else
        {
            // When in 12-hour format, decide AM or PM based on current time.
            return DateTime.Now.Hour >= 12 ? "PM" : "AM";
        }
    }

    private void changeDateFormat_Click(object sender, RoutedEventArgs e)
    {
        _isEuropeanDateFormat = !_isEuropeanDateFormat; // Toggle the boolean value
        _dateFormat = _isEuropeanDateFormat ? ToggleEuropeanDateFormat() : ToggleAmericanDateFormat();
    }
    
    private void changeMonthFormat_Click(object sender, RoutedEventArgs e)
    {
        _isMonthSpelledOut = !_isMonthSpelledOut; // Toggle the boolean value
        _dateFormat = _isEuropeanDateFormat ? ToggleEuropeanDateFormat() : ToggleAmericanDateFormat();
    }
    
    private void changeTimeFormat_Click(object sender, RoutedEventArgs e)
    {
        _is24Hour = !_is24Hour; // Toggle the boolean value
        _timeFormat = ToggleTimeFormat();
        
        _amOrPm = DetermineAmOrPm();
    }
    private void switchCulture_Click(object sender, RoutedEventArgs e)
    {
        //if the _currentCulture is not set or if it's set to English, switch to Danish.
        //if it's Danish switch it back to English.
        if (_currentCulture.Name == "en-US")
        {
            _currentCulture = new CultureInfo("da-DK");
        }
        else
        {
            _currentCulture = new CultureInfo("en-US");
        }
    }

    private async Task UpdateWeather()
    {
        Root? root = await MakeRequest();
        if (root != null)
        {
            WeatherTextBox.Text = root.features[0].properties.value.ToString() + "°C";
        }
    }
    
    private static async Task<Root?> MakeRequest()
    {
        // Set request headers
        Client.DefaultRequestHeaders.Accept.Clear();
        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/geo+json"));
        Client.DefaultRequestHeaders.Add("X-Gravitee-Api-Key", "8e67acbe-1cb2-48de-98d3-c6055fb527ef");

        // Specify the request URL
        string url = "https://dmigw.govcloud.dk/v2/metObs/collections/observation/items?period=latest-day&stationId=06030&parameterId=temp_mean_past1h&limit=24&offset=1&sortorder=observed%2CDESC&bbox-crs=https%3A%2F%2Fwww.opengis.net%2Fdef%2Fcrs%2FOGC%2F1.3%2FCRS84&api-key=8e67acbe-1cb2-48de-98d3-c6055fb527ef";
        
        try
        {
            // Send a GET request to the specified Uri and get the response
            HttpResponseMessage response = await Client.GetAsync(url);

            // Check that the response was successful
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!
                string responseBody = await response.Content.ReadAsStringAsync();
                // Deserialize the JSON response to the Root object
                Root? root = JsonConvert.DeserializeObject<Root>(responseBody);
                // You can use the root object now
                return root;
            }
            else
            {
                return null;
                // Log or handle the error situation
            }
        }
        catch (HttpRequestException e)
        {
            return null;
            // Handle any errors that occurred and provide more information
        }
    }
}