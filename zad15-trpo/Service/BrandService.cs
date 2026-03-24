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
    public class BrandService
    {
        private readonly Ratovsky15Context _db = DBService.Instance.Context;
        public ObservableCollection<Brand> Brands { get; set; } = new();
        public int Commit() => _db.SaveChanges();
        public void Add(Brand brand)
        {
            var _brand = new Brand
            {
                Id = brand.Id,
                Name = brand.Name,
                Products = brand.Products,
            };
            _db.Add(_brand);
            Commit();
            Brands.Add(_brand);
        }
        public void GetAll()
        {
            var brands = _db.Brands
                            .Include(b => b.Products)
                            .ToList();
            Brands.Clear();
            foreach (var brand in brands)
            {
                Brands.Add(brand);
            }
        }

        public BrandService()
        {
            GetAll();
        }

        public void Remove(Brand brand)
        {
            _db.Remove(brand);
            if (Commit() > 0)
                if (Brands.Contains(brand))
                    Brands.Remove(brand);
        }
    }
}
