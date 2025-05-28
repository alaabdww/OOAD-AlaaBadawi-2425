using System.Windows;

namespace WpfUser
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Content = new LoginPage(MainFrame);
        }
    }
}
