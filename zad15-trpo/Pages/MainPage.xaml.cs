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
using zad15_trpo.Service;

using zad15_trpo.Models;
using System.ComponentModel;

namespace zad15_trpo.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public Ratovsky15Context db = DBService.Instance.Context;
        public ProductService service { get; set; } = new();
        public ICollectionView productsView { get; set; }
        public string searchQuery { get; set; } = null!;
        public CategoryService categoryService { get; set; } = new();
        List<string> categoryNames = new();
        public BrandService brandService { get; set; } = new();
        List<string> brandNames = new();
        public int startSum { get; set; } = 0;
        public int endSum { get; set; } = 0;

        public MainPage()
        {
            InitializeComponent();
            productsView = CollectionViewSource.GetDefaultView(service.Products);
            productsView.Filter = FilterProducts;
        }
        public bool FilterProducts(object obj)
        {
            if (obj is not Product)
                return false;
            var product = (Product)obj;
            if (searchQuery != null && !product.Name.Contains(searchQuery,
            StringComparison.CurrentCultureIgnoreCase))
                return false;

            bool isPresentedCategory = false;
            foreach (string category in categoryNames)
            {
                if (product.Category.Name.Contains(category, StringComparison.CurrentCultureIgnoreCase))
                {
                    isPresentedCategory = true;
                    break;
                }
            }

            if (!isPresentedCategory && categoryNames.Count > 0)
                return false;

            bool isPresentedBrand = false;
            foreach (string brand in brandNames)
            {
                if (product.Brand.Name.Contains(brand, StringComparison.CurrentCultureIgnoreCase))
                {
                    isPresentedBrand = true;
                    break;
                }
            }

            if (!isPresentedBrand && brandNames.Count > 0)
                return false;

            if (startSum > 0 && startSum > product.Price)
                return false;

            if (endSum > 0 && endSum < product.Price)
                return false;

            return true;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            productsView.Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            searchQuery = "";

            productsView.Refresh();
        }
    }
}
