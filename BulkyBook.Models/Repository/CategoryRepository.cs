using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models.Repository.IRepository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationContext _db;

        public CategoryRepository(ApplicationContext db) : base(db)
        {
           _db = db;
        }

        public void Update(Category category)
        {
            _db.Update(category);
        }

    }
}
