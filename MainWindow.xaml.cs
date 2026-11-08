using System.Windows;
using System.Windows.Threading;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using DotNetEnv;
using Newtonsoft.Json;
using System.Windows.Controls;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Media;

namespace WPF_CLock;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private static readonly string BaseUrl = "https://opendataapi.dmi.dk/v2/metObs/collections/observation/items";
    private static string ApiKey;
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
    private string _amOrPm = string.Empty;
    private CultureInfo _currentCulture = new CultureInfo("da-DK");

    static MainWindow()
    {
        // Load API key from .env file
        string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string envFilePath = System.IO.Path.Combine(appDirectory, ".env");
        Env.Load(envFilePath);
        ApiKey = Env.GetString("APIKEY");
    }

    public MainWindow()
    {
        InitializeComponent();
        InitializeApiKey();
        NameTextBlock.Text = _locationName;

        // Set up a timer to update the time and date every second
        DispatcherTimer timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        timer.Tick += (s, a) =>
        {
            UpdateTimeAndDate();
        };
        timer.Start();

        // Initial weather update
        UpdateWeather();
        Task.Delay(30000);

        // Set up a timer to update the weather every 10 minutes
        DispatcherTimer apiTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMinutes(10)
        };
        apiTimer.Tick += async (s, a) => { await UpdateWeather(); };
        apiTimer.Start();

        LoadLocationsIntoMenu(LocationMenuItem);
    }

    /// <summary>
    /// Updates the time and date displayed on the UI.
    /// </summary>
    private void UpdateTimeAndDate()
    {
        TimeTextBox.Text = DateTime.Now.ToString(_timeFormat) + " " + _amOrPm;
        DateTextBox.Text = DateTime.Now.ToString(_dateFormat, _currentCulture);
        string dayOfWeek = DateTime.Now.ToString("dddd", _currentCulture);
        dayOfWeek = char.ToUpper(dayOfWeek[0]) + dayOfWeek.Substring(1);
        DayTextBox.Text = dayOfWeek;
        CultureTextBlock.Text = _currentCulture.Name;
    }

    /// <summary>
    /// Toggles the European date format.
    /// </summary>
    /// <returns>The date format string.</returns>
    private string ToggleEuropeanDateFormat()
    {
        return _isMonthSpelledOut ? "dd-MMMM-yyyy" : "dd-MM-yyyy";
    }

    /// <summary>
    /// Toggles the American date format.
    /// </summary>
    /// <returns>The date format string.</returns>
    private string ToggleAmericanDateFormat()
    {
        return _isMonthSpelledOut ? "MMMM-dd-yyyy" : "MM-dd-yyyy";
    }

    /// <summary>
    /// Toggles the time format between 24-hour and 12-hour.
    /// </summary>
    /// <returns>The time format string.</returns>
    private string ToggleTimeFormat()
    {
        return _is24Hour ? "HH:mm:ss" : "hh:mm:ss";
    }

    /// <summary>
    /// Determines whether to display AM or PM based on the current time.
    /// </summary>
    /// <returns>AM or PM string, or empty if 24-hour format.</returns>
    private string DetermineAmOrPm()
    {
        return _is24Hour ? string.Empty : (DateTime.Now.Hour >= 12 ? "PM" : "AM");
    }

    /// <summary>
    /// Event handler for changing the date format.
    /// </summary>
    private void changeDateFormat_Click(object sender, RoutedEventArgs e)
    {
        _isEuropeanDateFormat = !_isEuropeanDateFormat;
        _dateFormat = _isEuropeanDateFormat ? ToggleEuropeanDateFormat() : ToggleAmericanDateFormat();
    }

    /// <summary>
    /// Event handler for changing the month format.
    /// </summary>
    private void changeMonthFormat_Click(object sender, RoutedEventArgs e)
    {
        _isMonthSpelledOut = !_isMonthSpelledOut;
        _dateFormat = _isEuropeanDateFormat ? ToggleEuropeanDateFormat() : ToggleAmericanDateFormat();
    }

    /// <summary>
    /// Event handler for changing the time format.
    /// </summary>
    private void changeTimeFormat_Click(object sender, RoutedEventArgs e)
    {
        _is24Hour = !_is24Hour;
        _timeFormat = ToggleTimeFormat();
        _amOrPm = DetermineAmOrPm();
    }

    /// <summary>
    /// Event handler for switching the culture between English and Danish.
    /// </summary>
    private void switchCulture_Click(object sender, RoutedEventArgs e)
    {
        _currentCulture = _currentCulture.Name == "en-US" ? new CultureInfo("da-DK") : new CultureInfo("en-US");
    }

    /// <summary>
    /// Updates the weather information displayed on the UI.
    /// </summary>
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

        string totalWeather = $"Temperature: {temp}, Humidity: {humidity}\n Wind Direction: {windDirection}, Wind Speed: {windSpeed}";

        WeatherTextBox.Text = totalWeather;
        LastApiCallTextBlock.Text = "Updated: " + DateTime.Now.ToString("HH:mm:ss");
    }

    /// <summary>
    /// Constructs the URL for the API request.
    /// </summary>
    /// <param name="parameterId">The parameter ID.</param>
    /// <param name="locationId">The location ID.</param>
    /// <returns>The constructed URL.</returns>
    private static string ConstructUrl(string parameterId, string locationId)
    {
        string callUrl = $"?period=latest-day&stationId={locationId}&parameterId={parameterId}&limit=1&sortorder=observed%2CDESC&bbox-crs=https%3A%2F%2Fwww.opengis.net%2Fdef%2Fcrs%2FOGC%2F1.3%2FCRS84";
        return $"{BaseUrl}{callUrl}";
    }

    /// <summary>
    /// Makes an API request to get weather data.
    /// </summary>
    /// <param name="parameterId">The parameter ID.</param>
    /// <param name="locationId">The location ID.</param>
    /// <returns>The feature collection from the API response.</returns>
    private static async Task<FeatureCollection?> MakeRequest(string parameterId, string locationId)
    {
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/geo+json"));

            string url = ConstructUrl(parameterId, locationId);

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<FeatureCollection>(responseBody);
                }
                else
                {
                    return null;
                }
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }
    }

    /// <summary>
    /// Converts a degree value to a cardinal direction.
    /// </summary>
    /// <param name="degree">The degree value.</param>
    /// <returns>The cardinal direction.</returns>
    public string DegreeToCardinalDirection(double degree)
    {
        degree = degree % 360;
        if (degree < 0)
        {
            degree += 360;
        }

        string[] cardinalDirections = { "N", "NE", "E", "SE", "S", "SW", "W", "NW", "N" };
        int index = (int)Math.Round(degree / 45) % 8;
        return cardinalDirections[index];
    }

    /// <summary>
    /// Event handler for when the location menu item is opened.
    /// </summary>
    private void LocationMenuItem_SubmenuOpened(object sender, RoutedEventArgs e)
    {
        LoadLocationsIntoMenu(sender as MenuItem);
    }

    /// <summary>
    /// Loads locations into the menu.
    /// </summary>
    /// <param name="locationMenuItem">The location menu item.</param>
    private void LoadLocationsIntoMenu(MenuItem locationMenuItem)
    {
        if (locationMenuItem == null) return;

        var locations = LoadStationsFromDatabase();
        Debug.WriteLine($"Found {locations.Count} locations in the database.");
        locationMenuItem.Items.Clear();

        foreach (var location in locations)
        {
            Debug.WriteLine($"Adding location: {location.name}");
            var locationSubMenuItem = new MenuItem
            {
                Header = location.name,
                Tag = location.stationId
            };
            locationSubMenuItem.Click += LocationMenuItem_Click;
            locationMenuItem.Items.Add(locationSubMenuItem);
        }
    }

    /// <summary>
    /// Loads stations from the database.
    /// </summary>
    /// <returns>A list of locations.</returns>
    private List<Location> LoadStationsFromDatabase()
    {
        using (var context = new WeatherDbContext())
        {
            return context.Locations.ToList();
        }
    }

    /// <summary>
    /// Event handler for when a location menu item is clicked.
    /// </summary>
    private async void LocationMenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (sender is MenuItem menuItem)
        {
            _locationId = menuItem.Tag.ToString();
            _locationName = menuItem.Header.ToString();
            Debug.WriteLine($"Selected locationId: {_locationId}");
            Debug.WriteLine($"Selected locationName: {_locationName}");
            Debug.WriteLine(ConstructUrl(_windDirection, _locationId));

            NameTextBlock.Text = _locationName;
            await UpdateWeather();
        }
    }

    /// <summary>
    /// Event handler for when the API key menu item is clicked.
    /// </summary>
    private void ApiKeyMenuItem_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new ApiKeyDialog();
        if (dialog.ShowDialog() == true)
        {
            string apiKey = dialog.ApiKey;
            SaveApiKey(apiKey);
        }
    }

    /// <summary>
    /// Updates the static API key field.
    /// </summary>
    /// <param name="apiKey">The new API key.</param>
    private static void UpdateApiKey(string apiKey)
    {
        typeof(MainWindow).GetField("ApiKey", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).SetValue(null, apiKey);
    }

    /// <summary>
    /// Saves the API key to the .env file and updates the UI.
    /// </summary>
    /// <param name="apiKey">The new API key.</param>
    private void SaveApiKey(string apiKey)
    {
        string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string envFilePath = System.IO.Path.Combine(appDirectory, ".env");

        var envLines = System.IO.File.Exists(envFilePath) ? System.IO.File.ReadAllLines(envFilePath).ToList() : new List<string>();

        bool apiKeyFound = false;
        for (int i = 0; i < envLines.Count; i++)
        {
            if (envLines[i].StartsWith("APIKEY="))
            {
                envLines[i] = $"APIKEY=\"{apiKey}\"";
                apiKeyFound = true;
                break;
            }
        }

        if (!apiKeyFound)
        {
            envLines.Add($"APIKEY=\"{apiKey}\"");
        }

        System.IO.File.WriteAllLines(envFilePath, envLines);
        UpdateApiKey(apiKey);
        InitializeApiKey();
    }

    /// <summary>
    /// Initializes the API key and updates the UI accordingly.
    /// </summary>
    private void InitializeApiKey()
    {
        string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string envFilePath = System.IO.Path.Combine(appDirectory, ".env");
        Env.Load(envFilePath);
        ApiKey = Env.GetString("APIKEY");

        // DMI's open data API no longer requires a key, so a missing key is not an error.
        Dispatcher.Invoke(() => NameTextBlock.Foreground = new SolidColorBrush(Colors.White));
    }
}
