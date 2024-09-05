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
        }

        private void hinzufuegenButton_Click(object sender, RoutedEventArgs e)
        {
            openPopup();
        }

        private void bearbeitenButton_Click(object sender, RoutedEventArgs e)
        {
            openPopup();
        }

        private void loeschenButton_Click(object sender, RoutedEventArgs e)
        {
            openPopup();
        }

        public void openPopup()
        {
            popup.Show();
            popup.Activate();
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
