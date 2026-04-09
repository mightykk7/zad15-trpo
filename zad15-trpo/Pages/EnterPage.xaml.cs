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
using System.Windows.Navigation;
using System.Windows.Shapes;

using zad15_trpo.Models;

namespace zad15_trpo.Pages
{
    /// <summary>
    /// Логика взаимодействия для EnterPage.xaml
    /// </summary>
    public partial class EnterPage : Page
    {
        public string pin { get; set; } = "";
        public EnterPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if ( pin.ToString() == "1234")
                NavigationService.Navigate(new MainPage(true));
            else
                MessageBox.Show("Введите правильный пароль", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage(false));
        }
    }
}
