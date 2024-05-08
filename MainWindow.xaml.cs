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

namespace WPF_CLock;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DispatcherTimer timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(1);
        timer.Tick += (s, a) => 
        {
            TimeTextBox.Text = DateTime.Now.ToString("hh:mm:ss");
            DateTextBox.Text = DateTime.Now.ToString("dd-MM-yyyy");
            DayTextBox.Text = DateTime.Now.ToString("dddd");
        };
        timer.Start();
    }
}