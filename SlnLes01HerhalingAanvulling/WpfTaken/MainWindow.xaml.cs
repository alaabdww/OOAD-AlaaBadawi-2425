using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfTaken
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Checkform() 
        {
            if (string.IsNullOrWhiteSpace(NaamTxt.Text))
            {
                ErrorTxt.Text += "Gelieve een naam toe te voegen" + Environment.NewLine;
            }
            if (PrioriteitCmbbx.SelectedIndex == 3)
            {
                ErrorTxt.Text += "Gelieve een prioriteit te kiezen" + Environment.NewLine;
            }
            if (Datumpicker.SelectedDate == null)
            {
                ErrorTxt.Text += "Gelieve een datum te kiezen" + Environment.NewLine;
            }
            if (radAdam.IsChecked == false && radBilal.IsChecked == false && radChelsey.IsChecked == false)
            {
                ErrorTxt.Text += "Gelieve een verantwoordelijke te kiezen" + Environment.NewLine;
            }
        }

        private void Toevoegen_Click(object sender, RoutedEventArgs e)
        {
            Checkform();
            
            ErrorTxt.Text = null;

           
                ListBoxItem item = new ListBoxItem();
                item.Content = $"{NaamTxt.Text} (deadline: {Datumpicker.SelectedDate}; door ";

                Lstbx.Items.Add(item);

        }
    }
}
