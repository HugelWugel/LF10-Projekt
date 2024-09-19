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
using System.ComponentModel;
using System.Xml.Serialization;
using Datenbankanbindung;
using System.Collections.ObjectModel;
using System.Data;

namespace LF10_Lager_Projekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Popup popup;
        public ObservableCollection<Lagerartikel> AllArtikel { get; set; }
        public ObservableCollection<kritArtikel> KritArtikel { get; set; }
        private dbService dbService = new dbService();
        public object dataEntry;
        public MainWindow()
        {
            InitializeComponent();
            popup = new Popup(this);
            LoadData();
        }

        private void hinzufuegenButton_Click(object sender, RoutedEventArgs e)
        {
            openPopup(1);
        }

        private void bearbeitenButton_Click(object sender, RoutedEventArgs e)
        {
            openPopup(2);
        }

        private void loeschenButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"Möchten Sie alle {AllDataTable.SelectedItems.Count} selektieren Einträge löschen?", "Fortfahren", MessageBoxButton.OKCancel, MessageBoxImage.Hand);
            if (result == MessageBoxResult.OK)
            {
                List<int> ids = new List<int>();
                for (int i = 0; AllDataTable.SelectedItems.Count > i; i++)
                {
                    Lagerartikel ausgewählterArtikel = AllDataTable.SelectedItems[i] as Lagerartikel;
                    ids.Add(ausgewählterArtikel.Materialnummer);
                }
                int affectedRows = dbService.deleteDataEntry(ids);
                MessageBox.Show($"{affectedRows} erfolgreich gelöscht", "Ok", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadData();
            }            
        }

        public void openPopup(int action)
        {
            switch (action)
            {
                case 1:
                    popup.clearTextboxes();
                    popup.Show();
                    popup.Activate();
                    popup.PopupActionName.Content = "Eintrag hinzufügen";
                    break;
                case 2:
                    popup.PopupActionName.Content = "Eintrag bearbeiten";
                    Lagerartikel ausgewählterArtikel = AllDataTable.SelectedItem as Lagerartikel;
                    popup.updateTextboxes(ausgewählterArtikel);
                    popup.Show();
                    popup.Activate();
                    break;
            }
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        public void LoadData()
        {
            AllArtikel = dbService.getAllData();
            KritArtikel = dbService.getKritData();
            AllDataTable.ItemsSource = AllArtikel;
            KritDataTable.ItemsSource = KritArtikel;
        }

        private void AllDataTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AllDataTable.SelectedCells != null)
            {
                if (AllDataTable.SelectedItems.Count == 1)
                {
                    bearbeitenButton.IsEnabled = true;
                    loeschenButton.IsEnabled = true;
                }
                else if (AllDataTable.SelectedItems.Count > 1) 
                {
                    loeschenButton.IsEnabled = true;
                    bearbeitenButton.IsEnabled = false;
                }
                else
                {
                    loeschenButton.IsEnabled = false;
                    bearbeitenButton.IsEnabled = false;
                }
            }
            else
            {
                loeschenButton.IsEnabled = false;
                bearbeitenButton.IsEnabled = false;
            }
        }
    }
}
