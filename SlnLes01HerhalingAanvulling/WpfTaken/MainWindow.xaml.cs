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

        private bool Checkform() 
        {
            bool isValid = true;
             ErrorTxt.Text = null;
            if (string.IsNullOrWhiteSpace(NaamTxt.Text))
            {
                ErrorTxt.Text += "Gelieve een naam toe te voegen" + Environment.NewLine;
                isValid = false;
            }
            if (PrioriteitCmbbx.SelectedIndex == 3)
            {
                ErrorTxt.Text += "Gelieve een prioriteit te kiezen" + Environment.NewLine;
                isValid = false;
            }
            if (Datumpicker.SelectedDate == null)
            {
                ErrorTxt.Text += "Gelieve een datum te kiezen" + Environment.NewLine;
                isValid = false;
            }
            if (radAdam.IsChecked == false && radBilal.IsChecked == false && radChelsey.IsChecked == false)
            {
                ErrorTxt.Text += "Gelieve een verantwoordelijke te kiezen" + Environment.NewLine;
                isValid = false;
            }

            return isValid;

        }

        Stack<ListBoxItem> verwijderdeItems = new Stack<ListBoxItem>();

        private void Toevoegen_Click(object sender, RoutedEventArgs e)
        {


            if (!Checkform()) 
            {
                return;
            }

            string uitvoerder;
            if (radAdam.IsChecked == true)
            {
                uitvoerder = "Adam";
            }
            else if (radBilal.IsChecked == true)
            {
                uitvoerder = "Bilal";
            }
            else
            {
                uitvoerder = "Chelsey";
            }

            ListBoxItem item = new ListBoxItem();
            item.Content = $"{NaamTxt.Text} (deadline: {Datumpicker.SelectedDate.Value.ToString("dd/MM/yyyy")}; door {uitvoerder})";
            string prioriteit = ((ComboBoxItem)PrioriteitCmbbx.SelectedItem).Content.ToString();
            switch (prioriteit)
            {
                case "Hoog":
                    item.Background = Brushes.LightCoral;
                    break;

                case "Gemiddeld":
                    item.Background = Brushes.LightYellow;
                    break;

                case "Laag":
                    item.Background = Brushes.LightSeaGreen;
                    break;
            }

            Lstbx.Items.Add(item);

            NaamTxt.Text = "";
            PrioriteitCmbbx.SelectedIndex = 3;
            Datumpicker.SelectedDate = null;
            radAdam.IsChecked = false;
            radBilal.IsChecked = false;
            radChelsey.IsChecked = false;
        }

        private void Lstbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            verwijderenBtn.IsEnabled = true;
        }

        private void verwijderenBtn_Click(object sender, RoutedEventArgs e)
        {
            ListBoxItem selectedItem = (ListBoxItem)Lstbx.SelectedItem;
            if (selectedItem != null) 
            {
                verwijderdeItems.Push(selectedItem);
                Lstbx.Items.Remove(selectedItem);
            }

            terugzettenBtn.IsEnabled = true;
            verwijderenBtn.IsEnabled = false;
        }

        private void terugzettenBtn_Click(object sender, RoutedEventArgs e)
        {
            ListBoxItem item = verwijderdeItems.Pop();
            Lstbx.Items.Add(item);

            terugzettenBtn.IsEnabled = false;
            verwijderenBtn.IsEnabled = true;
        }
    }
}
