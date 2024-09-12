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

        public void updateTextboxes(int id)
        {
            idTextbox.Text = dbService.AllArtikel[id].Materialnummer.ToString();
            NameTextbox.Text = dbService.AllArtikel[id].Materialname;
            WarengruppeTextbox.Text = dbService.AllArtikel[id].Warengruppe;
            MengeTextbox.Text = dbService.AllArtikel[id].Menge.ToString();
            GrenzwertTextbox.Text = dbService.AllArtikel[id].Menge.ToString();

            idLabel.Visibility = Visibility.Visible;
            idTextbox.Visibility = Visibility.Visible;
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
                    mainWindow.LoadData();
                    Hide();
                    break;
                case "Eintrag bearbeiten":
                    dbService.changeDataEntry(Convert.ToInt32(idTextbox.Text), NameTextbox.Text, WarengruppeTextbox.Text, Convert.ToInt32(MengeTextbox.Text), Convert.ToInt32(GrenzwertTextbox.Text));
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
