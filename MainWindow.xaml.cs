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

namespace LF10_Lager_Projekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Popup popup = new Popup();

        public MainWindow()
        {
            InitializeComponent();
            AllDataTable.ItemsSource = Backend.getAllData().DefaultView;
            KritDataTable.ItemsSource = Backend.getKritData().DefaultView;
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
            openPopup(3);
        }

        public void openPopup(int action)
        {
            switch (action)
            {
                case 1:
                    popup.Show();
                    popup.Activate();
                    popup.PopupActionName.Content = "Eintrag hinzufügen";
                    break;
                case 2:
                    popup.Show();
                    popup.Activate();
                    popup.PopupActionName.Content = "Eintrag bearbeiten";
                    break;
                case 3:
                    popup.Show();
                    popup.Activate();
                    popup.PopupActionName.Content = "Eintrag löschen";
                    break;
            }
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
