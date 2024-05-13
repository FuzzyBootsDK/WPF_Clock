using System.Windows;
using System.Windows.Threading;
using System.Globalization;

namespace WPF_CLock;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private bool _is24Hour = true;
    private bool _isEuropeanDateFormat = true;
    private bool _isMonthSpelledOut;
    private string _dateFormat = "dd-MM-yyyy";
    private string _timeFormat = "HH:mm:ss";
    private string _amOrPm = String.Empty;
    CultureInfo _currentCulture = new CultureInfo("da-DA");
    
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
}