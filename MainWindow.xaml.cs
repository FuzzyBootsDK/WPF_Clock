using System.Windows;
using System.Windows.Threading;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using DotNetEnv;
using Newtonsoft.Json;
using System.Windows.Controls;
using System.Linq; // Ensure this is included for LINQ operations
using System.Diagnostics; // For debugging
using System.Collections.Generic; // For List

namespace WPF_CLock;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private static readonly string BaseUrl = "https://dmigw.govcloud.dk/v2/metObs/collections/observation/items";
    private static readonly string ApiKey; //"8e67acbe-1cb2-48de-98d3-c6055fb527ef";
    private string _locationId = "06030";
    private string _locationName = "Flyvestation Ålborg";
    private string _windDirection = "wind_dir";
    private string _windSpeed = "wind_speed_past1h";
    private string _humidity = "humidity";
    private string _temperature = "temp_mean_past1h";
    private bool _is24Hour = true;
    private bool _isEuropeanDateFormat = true;
    private bool _isMonthSpelledOut;
    private string _dateFormat = "dd-MM-yyyy";
    private string _timeFormat = "HH:mm:ss";
    private string _amOrPm = String.Empty;
    CultureInfo _currentCulture = new CultureInfo("da-DK");

    static MainWindow()
    {
        string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string envFilePath = System.IO.Path.Combine(appDirectory, ".env");
        Env.Load(envFilePath);
        ApiKey = Env.GetString("APIKEY");
    }

    public MainWindow()
    {
        InitializeComponent();
        // Make an initial call to the API
        NameTextBlock.Text = _locationName;

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

        UpdateWeather();
        Task.Delay(30000);

        DispatcherTimer apiTimer = new DispatcherTimer();
        apiTimer.Interval = TimeSpan.FromMinutes(10);
        apiTimer.Tick += async (s, a) => { await UpdateWeather(); };
        apiTimer.Start();

        LoadLocationsIntoMenu(LocationMenuItem);
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
        if (_is24Hour)
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
        var tempRoot = await MakeRequest(_temperature, _locationId);
        await Task.Delay(5000);

        var windDirectionRoot = await MakeRequest(_windDirection, _locationId);
        await Task.Delay(5000);

        var windSpeedRoot = await MakeRequest(_windSpeed, _locationId);
        await Task.Delay(5000);

        var humidityRoot = await MakeRequest(_humidity, _locationId);

        string temp = tempRoot != null && tempRoot.Features.Count > 0
            ? tempRoot.Features[0].Properties.Value.ToString() + "°C"
            : "N/A";

        string windDirection = windDirectionRoot != null && windDirectionRoot.Features.Count > 0
            ? DegreeToCardinalDirection(Convert.ToDouble(windDirectionRoot.Features[0].Properties.Value))
            : "N/A";

        string windSpeed = windSpeedRoot != null && windSpeedRoot.Features.Count > 0
            ? windSpeedRoot.Features[0].Properties.Value.ToString() + " m/s"
            : "N/A";

        string humidity = humidityRoot != null && humidityRoot.Features.Count > 0
            ? humidityRoot.Features[0].Properties.Value.ToString() + "%"
            : "N/A";

        string totalWeather =
            $"Temperature: {temp}, Humidity: {humidity}\n Wind Direction: {windDirection}, Wind Speed: {windSpeed}";

        WeatherTextBox.Text = totalWeather;
        // Update the LastApiCallTextBlock with the current time
        LastApiCallTextBlock.Text = "Updated: " + DateTime.Now.ToString("HH:mm:ss");
        // if (tempRoot != null && windDirectionRoot != null && humidityRoot != null)
        // {
        //     WeatherTextBox.Text = totalWeather;
        // }
    }

    private static string ConstructUrl(string parameterId, string locationId)
    {
        string callUrl =
            $"?period=latest-day&stationId={locationId}&parameterId={parameterId}&limit=1&sortorder=observed%2CDESC&bbox-crs=https%3A%2F%2Fwww.opengis.net%2Fdef%2Fcrs%2FOGC%2F1.3%2FCRS84&api-key=";
        return $"{BaseUrl}{callUrl}{ApiKey}";
    }

    private static async Task<FeatureCollection?> MakeRequest(string parameterId, string locationId)
    {
        using (HttpClient client = new HttpClient())
        {
            // Set request headers
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/geo+json"));
            client.DefaultRequestHeaders.Add("X-Gravitee-Api-Key", ApiKey);

            // Construct the URL
            string url = ConstructUrl(parameterId, locationId);

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    FeatureCollection? root = JsonConvert.DeserializeObject<FeatureCollection>(responseBody);
                    return root;
                }
                else
                {
                    return null;
                }
            }
            catch (HttpRequestException e)
            {
                return null; // Optionally, you could log the exception here.
            }
        }
    }

    public string DegreeToCardinalDirection(double degree)
    {
        // Ensure the degree is within the range [0, 360)
        degree = degree % 360;
        if (degree < 0)
        {
            degree += 360;
        }

        string[] cardinalDirections = { "N", "NE", "E", "SE", "S", "SW", "W", "NW", "N" };
        int index = (int)Math.Round(degree / 45) % 8; // Ensure the index is within the range [0, 7]
        return cardinalDirections[index];
    }

    private void LocationMenuItem_SubmenuOpened(object sender, RoutedEventArgs e)
    {
        LoadLocationsIntoMenu(sender as MenuItem);
    }

    private void LoadLocationsIntoMenu(MenuItem locationMenuItem)
    {
        if (locationMenuItem == null) return;

        var locations = LoadStationsFromDatabase();
        Debug.WriteLine($"Found {locations.Count} locations in the database."); // Debug statement
        locationMenuItem.Items.Clear(); // Clear existing items

        foreach (var location in locations)
        {
            Debug.WriteLine($"Adding location: {location.name}"); // Debug statement
            var locationSubMenuItem = new MenuItem
            {
                Header = location.name,
                Tag = location.stationId // Set the stationId as the Tag
            };
            locationSubMenuItem.Click += LocationMenuItem_Click;
            locationMenuItem.Items.Add(locationSubMenuItem);
        }
    }

    private List<Location> LoadStationsFromDatabase()
    {
        using (var context = new WeatherDbContext())
        {
            return context.Locations.ToList();
        }
    }

    private async void LocationMenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (sender is MenuItem menuItem)
        {
            _locationId = menuItem.Tag.ToString(); // Assign the stationId to locationId
            _locationName = menuItem.Header.ToString(); // Assign the location name to locationName
            Debug.WriteLine($"Selected locationId: {_locationId}"); // Debug statement
            Debug.WriteLine($"Selected locationName: {_locationName}"); // Debug statement
            Debug.WriteLine(ConstructUrl(_windDirection,_locationId));

            // Update the NameTextBlock with the new location name
            NameTextBlock.Text = _locationName;

            // Call UpdateWeather() to fetch new weather data for the selected location
            await UpdateWeather();
        }
    }
}
