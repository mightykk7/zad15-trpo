using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using zad15_trpo.Models;
using zad15_trpo.Service;

namespace zad15_trpo.Pages
{
    public partial class AttributesPage : Page
    {
        private readonly DBService _dbService = DBService.Instance;
        private List<Brand> _brandsList = new();
        private List<Category> _categoriesList = new();
        private List<Tag> _tagsList = new();

        public AttributesPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            _brandsList = _dbService.Context.Brands.ToList();
            dgBrands.ItemsSource = _brandsList;

            _categoriesList = _dbService.Context.Categories.ToList();
            dgCategories.ItemsSource = _categoriesList;

            _tagsList = _dbService.Context.Tags.ToList();
            dgTags.ItemsSource = _tagsList;
        }

        private void dgBrands_SelectionChanged(object sender, SelectionChangedEventArgs e) => txtBrandName.Text = (dgBrands.SelectedItem as Brand)?.Name ?? "";
        private void btnBrandAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBrandName.Text)) return;
            _dbService.Context.Brands.Add(new Brand { Name = txtBrandName.Text.Trim() });
            _dbService.Context.SaveChanges();
            LoadData(); txtBrandName.Clear();
            MessageBox.Show("Бренд добавлен");
        }
        private void btnBrandUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (dgBrands.SelectedItem is not Brand sel) { MessageBox.Show("Выберите бренд"); return; }
            if (string.IsNullOrWhiteSpace(txtBrandName.Text)) return;
            sel.Name = txtBrandName.Text.Trim();
            _dbService.Context.SaveChanges();
            LoadData();
            MessageBox.Show("Бренд обновлен");
        }
        private void btnBrandDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgBrands.SelectedItem is not Brand sel) return;
            if (MessageBox.Show($"Удалить '{sel.Name}'?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _dbService.Context.Brands.Remove(sel);
                _dbService.Context.SaveChanges();
                LoadData(); txtBrandName.Clear();
            }
        }

        private void dgCategories_SelectionChanged(object sender, SelectionChangedEventArgs e) => txtCategoryName.Text = (dgCategories.SelectedItem as Category)?.Name ?? "";
        private void btnCatAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCategoryName.Text)) return;
            _dbService.Context.Categories.Add(new Category { Name = txtCategoryName.Text.Trim() });
            _dbService.Context.SaveChanges();
            LoadData(); txtCategoryName.Clear();
            MessageBox.Show("Категория добавлена");
        }
        private void btnCatUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (dgCategories.SelectedItem is not Category sel) { MessageBox.Show("Выберите категорию"); return; }
            if (string.IsNullOrWhiteSpace(txtCategoryName.Text)) return;
            sel.Name = txtCategoryName.Text.Trim();
            _dbService.Context.SaveChanges();
            LoadData();
            MessageBox.Show("Категория обновлена");
        }
        private void btnCatDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgCategories.SelectedItem is not Category sel) return;
            if (MessageBox.Show($"Удалить '{sel.Name}'?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _dbService.Context.Categories.Remove(sel);
                _dbService.Context.SaveChanges();
                LoadData(); txtCategoryName.Clear();
            }
        }

        // === ТЕГИ ===
        private void dgTags_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtTagName.Text = (dgTags.SelectedItem as Tag)?.Name ?? "";
        }
        private void btnTagAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTagName.Text)) return;
            _dbService.Context.Tags.Add(new Tag { Name = txtTagName.Text.Trim() });
            _dbService.Context.SaveChanges();
            LoadData(); txtTagName.Clear();
            MessageBox.Show("Тег добавлен");
        }
        private void btnTagUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (dgTags.SelectedItem is not Tag sel) { MessageBox.Show("Выберите тег"); return; }
            if (string.IsNullOrWhiteSpace(txtTagName.Text)) return;
            sel.Name = txtTagName.Text.Trim();
            _dbService.Context.SaveChanges();
            LoadData();
            MessageBox.Show("Тег обновлен");
        }
        private void btnTagDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgTags.SelectedItem is not Tag sel) return;
            if (MessageBox.Show($"Удалить '{sel.Name}'?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _dbService.Context.Tags.Remove(sel);
                _dbService.Context.SaveChanges();
                LoadData(); txtTagName.Clear();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) => NavigationService.GoBack();
    }
}