using BulkyBook.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models.Repository.IRepository
{
    public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
    {
        private readonly ApplicationContext _db;

        public ProductImageRepository(ApplicationContext db) : base(db)
        {
           _db = db;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(ProductImage productImage)
        {
            _db.ProductImages.Update(productImage);
        }

    }
}
