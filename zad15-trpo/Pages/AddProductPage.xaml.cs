using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using Microsoft.EntityFrameworkCore;

using zad15_trpo.Models;
using zad15_trpo.Service;

namespace zad15_trpo.Pages
{
    public partial class AddProductPage : Page
    {
        private readonly Ratovsky15Context _db = DBService.Instance.Context;
        public Product CurrentProduct { get; set; }

        public AddProductPage()
        {
            InitializeComponent();
            CurrentProduct = new Product
            {
                CreatedAt = DateOnly.FromDateTime(DateTime.Now),
                Rating = 0,
                Stock = 0,
                Price = 0
            };
            DataContext = CurrentProduct;
            LoadComboBoxes();
        }

        public AddProductPage(Product productToEdit)
        {
            InitializeComponent();
            CurrentProduct = _db.Products.Include(p => p.Tags).FirstOrDefault(p => p.Id == productToEdit.Id) ?? productToEdit;
            DataContext = CurrentProduct;
            LoadComboBoxes();
        }

        private void LoadComboBoxes()
        {
            CategoryComboBox.ItemsSource = _db.Categories.OrderBy(c => c.Name).ToList();
            BrandComboBox.ItemsSource = _db.Brands.OrderBy(b => b.Name).ToList();

            if (CurrentProduct.Id != 0)
            {
                CategoryComboBox.SelectedValue = CurrentProduct.CategoryId;
                BrandComboBox.SelectedValue = CurrentProduct.BrandId;
            }

            PriceTextBox.Text = CurrentProduct.Price.ToString();
            StockTextBox.Text = CurrentProduct.Stock.ToString();
            RatingTextBox.Text = CurrentProduct.Rating.ToString();

            var tags = _db.Tags.OrderBy(t => t.Name).ToList();
            TagsListBox.ItemsSource = tags;

            if (CurrentProduct.Id != 0 && CurrentProduct.Tags != null)
            {
                foreach (var tag in tags)
                {
                    if (CurrentProduct.Tags.Any(t => t.Id == tag.Id))
                        TagsListBox.SelectedItems.Add(tag);
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CurrentProduct.Name)) { ShowErr("Введите название"); NameTextBox.Focus(); return; }
            if (string.IsNullOrWhiteSpace(CurrentProduct.Description)) { ShowErr("Введите описание"); DescriptionTextBox.Focus(); return; }

            if (!float.TryParse(PriceTextBox.Text.Replace(',', '.'), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float price) || price < 0)
            { ShowErr("Цена должна быть неотрицательным числом"); PriceTextBox.Focus(); return; }

            if (!int.TryParse(StockTextBox.Text, out int stock) || stock <= 0)
            { ShowErr("Количество должно быть неотрицательным целым числом"); StockTextBox.Focus(); return; }

            if (!double.TryParse(RatingTextBox.Text.Replace(',', '.'), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double rating) || rating < 0 || rating > 5)
            { ShowErr("Рейтинг от 0 до 5"); RatingTextBox.Focus(); return; }

            if (CategoryComboBox.SelectedValue == null || BrandComboBox.SelectedValue == null)
            { ShowErr("Выберите категорию и бренд"); return; }

            CurrentProduct.Price = price;
            CurrentProduct.Stock = stock;
            CurrentProduct.Rating = rating;
            CurrentProduct.CategoryId = (int)CategoryComboBox.SelectedValue;
            CurrentProduct.BrandId = (int)BrandComboBox.SelectedValue;

            try
            {
                if (CurrentProduct.Id == 0)
                {
                    CurrentProduct.Tags.Clear();
                    foreach (Tag t in TagsListBox.SelectedItems) CurrentProduct.Tags.Add(t);
                    _db.Products.Add(CurrentProduct);
                }
                else
                {
                    // Обновляем связи многие-ко-многим
                    var existingTags = _db.Tags.Where(t => CurrentProduct.Tags.Any(pt => pt.Id == t.Id)).ToList();
                    foreach (var t in existingTags) CurrentProduct.Tags.Remove(t);
                    foreach (Tag t in TagsListBox.SelectedItems) CurrentProduct.Tags.Add(t);
                    _db.Products.Update(CurrentProduct);
                }

                _db.SaveChanges();
                MessageBox.Show("Успешно сохранено!", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                if (NavigationService.CanGoBack) NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowErr(string msg)
        {
            MessageBox.Show(msg, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}