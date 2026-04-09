using Microsoft.EntityFrameworkCore;

using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace zad15_trpo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new Pages.EnterPage());
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if(e.Content is Page page)
            {
                this.Title = page.Title;
            }
        }
    }
}