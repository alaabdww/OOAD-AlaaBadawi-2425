using System.Windows;
using System.Windows.Controls;
using BenchmarkToolLibrary.Data;

namespace WpfUser
{
    public partial class LoginPage : Page
    {
        private Frame frame;

        public LoginPage(Frame frame)
        {
            InitializeComponent();
            this.frame = frame;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            txtFeedback.Text = "";
            string login = txtLogin.Text.Trim();
            string wachtwoord = pwdWachtwoord.Password;

            int bedrijfId = UserData.ValidateLogin(login, wachtwoord);
            if (bedrijfId > 0)
            {
                Application.Current.Properties["BedrijfId"] = bedrijfId;
                frame.Content = new DashboardPage(frame);
            }
            else
            {
                txtFeedback.Text = "Ongeldige login of wachtwoord.";
            }
        }
    }
}
