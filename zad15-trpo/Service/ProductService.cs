using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using zad15_trpo.Models;

namespace zad15_trpo.Service
{
    public class ProductService
    {
        private readonly Ratovsky15Context _db = DBService.Instance.Context;

        public ObservableCollection<Product> Products { get; set; } = new();

        public int Commit() => _db.SaveChanges();
        public void Add(Product product)
        {
            var _product = new Product
            {
                Id = product.Id,
                Name = product.Name,
                Brand = product.Brand,
                BrandId = product.BrandId,
                Category = product.Category,
                CategoryId = product.CategoryId,
                CreatedAt = product.CreatedAt,
                Description = product.Description,
                Price = product.Price,
                Rating = product.Rating,
                Stock = product.Stock,
                Tags = product.Tags
            };
            _db.Add(_product);
            Commit();
            Products.Add(_product);
        }
        public void GetAll()
        {
            var products = _db.Products
                              .Include(p => p.Category)
                              .Include(p => p.Brand)
                              .Include(p => p.Tags)
                              .ToList();
            Products.Clear();
            foreach (var product in products)
            {
                Products.Add(product);
            }
        }

        public ProductService()
        {
            GetAll();
        }

        public void Remove(Product product)
        {
            _db.Remove(product);
            if (Commit() > 0)
                if (Products.Contains(product))
                    Products.Remove(product);
        }
    }
}
