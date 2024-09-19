using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using Datenbankanbindung;

namespace LF10_Lager_Projekt
{
    public partial class Popup : Window
    {
        private MainWindow mainWindow;
        public ObservableCollection<Lagerartikel> AllArtikel { get; set; }
        public Popup(MainWindow window)
        {
            InitializeComponent();
            mainWindow = window;
        }

        public void updateTextboxes(Lagerartikel Eintrag)
        {
            idTextbox.Text = Eintrag.Materialnummer.ToString();
            NameTextbox.Text = Eintrag.Materialname;
            WarengruppeTextbox.Text = Eintrag.Warengruppe;
            MengeTextbox.Text = Eintrag.Menge.ToString();
            GrenzwertTextbox.Text = Eintrag.Grenzwert.ToString();

            idLabel.Visibility = Visibility.Visible;
            idTextbox.Visibility = Visibility.Visible;
        }

        public void clearTextboxes()
        {
            idTextbox.Text = "";
            NameTextbox.Text = "";
            WarengruppeTextbox.Text = "";
            MengeTextbox.Text = "";
            GrenzwertTextbox.Text = "";

            idLabel.Visibility = Visibility.Hidden;
            idTextbox.Visibility = Visibility.Hidden;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            switch (PopupActionName.Content)
            {
                case "Eintrag hinzufügen":
                    dbService.createDataEntry(NameTextbox.Text, WarengruppeTextbox.Text, Convert.ToInt32(MengeTextbox.Text), Convert.ToInt32(GrenzwertTextbox.Text));
                    MessageBox.Show($"Eintrag erstellt", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    mainWindow.LoadData();
                    Hide();
                    break;
                case "Eintrag bearbeiten":
                    dbService.changeDataEntry(Convert.ToInt32(idTextbox.Text), NameTextbox.Text, WarengruppeTextbox.Text, Convert.ToInt32(MengeTextbox.Text), Convert.ToInt32(GrenzwertTextbox.Text));
                    MessageBox.Show($"Eintrag bearbeitet", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    mainWindow.LoadData();
                    Hide();
                    break;
                case "Eintrag löschen":
                    mainWindow.LoadData();
                    Hide();
                    break;
            }
        }
    }
}
