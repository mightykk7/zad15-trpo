using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using zad15_trpo.Models;
using zad15_trpo.Service;

namespace zad15_trpo.Pages
{
    public partial class MainPage : Page
    {
        private readonly Ratovsky15Context _db = DBService.Instance.Context;
        public ProductService service { get; set; } = new();
        public ICollectionView productsView { get; set; }

        public string searchQuery { get; set; } = null!;
        public string filterPriceFrom { get; set; } = null!;
        public string filterPriceTo { get; set; } = null!;

        public int? selectedBrandId { get; set; } = null;
        public int? selectedCategoryId { get; set; } = null;

        public MainPage(bool isManager)
        {
            InitializeComponent();
            if (isManager)
            {
                Add.Visibility = Visibility.Visible;
                Delete.Visibility = Visibility.Visible;
                Edit.Visibility = Visibility.Visible;
            }

            LoadFilters();
            productsView = CollectionViewSource.GetDefaultView(service.Products);
            productsView.Filter = FilterProducts;
        }

        private void LoadFilters()
        {
            BrandFilterComboBox.Items.Add(new { Name = "Все бренды" });
            var brands = _db.Brands.OrderBy(b => b.Name).ToList();
            foreach (var b in brands)
                BrandFilterComboBox.Items.Add(b);

            CategoryFilterComboBox.Items.Add(new { Name = "Все категории" });
            var categories = _db.Categories.OrderBy(c => c.Name).ToList();
            foreach (var c in categories)
                CategoryFilterComboBox.Items.Add(c);
        }

        private bool FilterProducts(object obj)
        {
            if (obj is not Product p) return false;

            // Поиск по названию
            if (!string.IsNullOrEmpty(searchQuery) &&
                !p.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                return false;

            // Фильтр по бренду
            if (selectedBrandId.HasValue && selectedBrandId.Value > 0 &&
                p.BrandId != selectedBrandId.Value)
                return false;

            // Фильтр по категории
            if (selectedCategoryId.HasValue && selectedCategoryId.Value > 0 &&
                p.CategoryId != selectedCategoryId.Value)
                return false;

            // Фильтр по цене
            if (!string.IsNullOrEmpty(filterPriceFrom) &&
                float.TryParse(filterPriceFrom.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out float minP) &&
                p.Price < minP)
                return false;

            if (!string.IsNullOrEmpty(filterPriceTo) &&
                float.TryParse(filterPriceTo.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out float maxP) &&
                p.Price > maxP)
                return false;

            return true;
        }

        private void BrandFilter(object sender, SelectionChangedEventArgs e)
        {
            selectedBrandId = (BrandFilterComboBox.SelectedItem as Brand)?.Id ?? 0;
            productsView.Refresh();
        }

        private void CategoryFilter(object sender, SelectionChangedEventArgs e)
        {
            selectedCategoryId = (CategoryFilterComboBox.SelectedItem as Category)?.Id ?? 0;
            productsView.Refresh();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            productsView.Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            searchQuery = "";
            filterPriceFrom = "";
            filterPriceTo = "";
            selectedBrandId = null;
            selectedCategoryId = null;
            BrandFilterComboBox.SelectedIndex = 0;
            CategoryFilterComboBox.SelectedIndex = 0;
            productsView.Refresh();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            productsView.SortDescriptions.Clear();
            var cb = (ComboBox)sender;
            if (cb.SelectedItem is not ComboBoxItem selected) return;

            switch (selected.Tag?.ToString())
            {
                case "Name":
                    productsView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                    break;
                case "PriceUp":
                    productsView.SortDescriptions.Add(new SortDescription("Price", ListSortDirection.Ascending));
                    break;
                case "PriceDown":
                    productsView.SortDescriptions.Add(new SortDescription("Price", ListSortDirection.Descending));
                    break;
                case "StockUp":
                    productsView.SortDescriptions.Add(new SortDescription("Stock", ListSortDirection.Ascending));
                    break;
                case "StockDown":
                    productsView.SortDescriptions.Add(new SortDescription("Stock", ListSortDirection.Descending));
                    break;
            }
            productsView.Refresh();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddProductPage());
        }
            

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsList.SelectedItem is not Product sel)
            {
                MessageBox.Show("Выберите товар", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show($"Удалить товар \"{sel.Name}\"?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;

            try
            {
                _db.Products.Remove(sel);
                _db.SaveChanges();
                service.Products.Remove(sel);
                productsView.Refresh();
                MessageBox.Show("Товар удалён", "Готово",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AttributesPage());
        }
            
    }
}