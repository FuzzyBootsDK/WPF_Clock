using System.Windows;

namespace WPF_CLock
{
    public partial class ApiKeyDialog : Window
    {
        public string ApiKey { get; private set; }

        public ApiKeyDialog()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            ApiKey = ApiKeyTextBox.Text;
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
