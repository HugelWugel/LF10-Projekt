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
using System.Windows.Shapes;
using Datenbankanbindung;

namespace LF10_Lager_Projekt
{
    public partial class Popup : Window
    {
        private MainWindow mainWindow;
        public Popup(MainWindow window)
        {
            InitializeComponent();
            mainWindow = window;
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
