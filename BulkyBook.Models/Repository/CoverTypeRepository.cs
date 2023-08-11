using BulkyBook.Models.Models;
using BulkyBook.Models.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models.Repository
{
    public class CoverTypeRepository : Repository<CoverType>, ICoverTypeRepository
    {
        private readonly ApplicationContext _db;

        public CoverTypeRepository(ApplicationContext db) : base(db)
        {
            _db = db;
        }

        public void Update(CoverType coverType)
        {
            _db.Update(coverType);
        }
        public void Save()
        {
            _db.SaveChanges();
        }

    }
}
