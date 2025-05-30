// MainWindow.cs
// -------------
// Dit is het hoofdvenster van de WpfUser-applicatie. 
// Bij het opstarten wordt automatisch de LoginPage geladen in het centrale Frame.

using System.Windows;

namespace WpfUser
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Constructor van het hoofdvenster. Initialiseert de componenten en zet de startpagina op LoginPage.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            // Laad de LoginPage als eerste pagina in het MainFrame
            MainFrame.Content = new LoginPage(MainFrame);
        }
    }
}
