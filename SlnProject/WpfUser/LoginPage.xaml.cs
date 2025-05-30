// LoginPage.xaml.cs
// ------------------
// Login-pagina voor gebruikers (bedrijven) binnen WpfUser. 
// Hier kan een gebruiker inloggen met login en wachtwoord, of eventueel debuggen zonder wachtwoord.

using BenchmarkToolLibrary.Data;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace WpfUser
{
    public partial class LoginPage : Page
    {
        private Frame frame;

        /// <summary>
        /// Initialiseert de loginpagina, onthoudt het frame voor navigatie.
        /// </summary>
        public LoginPage(Frame frame)
        {
            InitializeComponent();
            this.frame = frame;
        }

        /// <summary>
        /// Login-knop: Valideert login en wachtwoord. Bij succes: ga naar dashboard.
        /// </summary>
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            txtFeedback.Text = "";

            string login = txtLogin.Text.Trim();
            string wachtwoord = chkShowPassword.IsChecked == true
                ? txtWachtwoord.Text
                : pwdWachtwoord.Password;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(wachtwoord))
            {
                txtFeedback.Text = "Gelieve login én wachtwoord in te vullen.";
                return;
            }

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

        /// <summary>
        /// Debug-functie: bypass login zonder wachtwoordcontrole (voor testdoeleinden).
        /// </summary>
        private void btnBypassLogin_Click(object sender, RoutedEventArgs e)
        {
            txtFeedback.Text = "";

            string login = txtLogin.Text.Trim();
            if (string.IsNullOrWhiteSpace(login))
            {
                txtFeedback.Text = "Vul eerst een login in om in te loggen.";
                return;
            }

            int bedrijfId = GetBedrijfIdZonderWachtwoord(login);
            if (bedrijfId > 0)
            {
                Application.Current.Properties["BedrijfId"] = bedrijfId;
                frame.Content = new DashboardPage(frame);
            }
            else
            {
                txtFeedback.Text = "Login niet gevonden.";
            }
        }

        /// <summary>
        /// Haalt het bedrijfId op puur via login, zonder wachtwoordcontrole (enkel voor debug).
        /// </summary>
        private int GetBedrijfIdZonderWachtwoord(string login)
        {
            string connString = @"Server=(localdb)\MSSQLLocalDB;Database=BenchmarkDB;Trusted_Connection=True;";
            string query = "SELECT id FROM Companies WHERE login = @login";

            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@login", login);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader.GetInt32(reader.GetOrdinal("id"));
                        }
                    }
                }
            }

            return 0;
        }

        /// <summary>
        /// Wachtwoord tonen als checkbox aangevinkt is (wisselt tussen PasswordBox en TextBox).
        /// </summary>
        private void chkShowPassword_Checked(object sender, RoutedEventArgs e)
        {
            txtWachtwoord.Text = pwdWachtwoord.Password;
            pwdWachtwoord.Visibility = Visibility.Collapsed;
            txtWachtwoord.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Wachtwoord verbergen als checkbox uitgevinkt is.
        /// </summary>
        private void chkShowPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            pwdWachtwoord.Password = txtWachtwoord.Text;
            txtWachtwoord.Visibility = Visibility.Collapsed;
            pwdWachtwoord.Visibility = Visibility.Visible;
        }
    }
}
