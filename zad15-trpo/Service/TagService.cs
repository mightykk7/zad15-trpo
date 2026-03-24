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
    public class TagService
    {
        private readonly Ratovsky15Context _db = DBService.Instance.Context;
        public ObservableCollection<Tag> Tags { get; set; } = new();
        public int Commit() => _db.SaveChanges();
        public void Add(Tag tag)
        {
            var _tag = new Tag
            {
                Id = tag.Id,
                Name = tag.Name,
                Products = tag.Products,
            };
            _db.Add(_tag);
            Commit();
            Tags.Add(_tag);
        }
        public void GetAll()
        {
            var tags = _db.Tags
                          .Include(t => t.Products)
                          .ToList();
            Tags.Clear();
            foreach (var tag in tags)
            {
                Tags.Add(tag);
            }
        }

        public TagService()
        {
            GetAll();
        }

        public void Remove(Tag tag)
        {
            _db.Remove(tag);
            if (Commit() > 0)
                if (Tags.Contains(tag))
                    Tags.Remove(tag);
        }
    }
}
