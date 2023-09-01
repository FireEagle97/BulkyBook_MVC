using BulkyBook.Models.Models;
using BulkyBook.Models.Repository.IRepository;
using System.Runtime.CompilerServices;

namespace BulkyBook.Models.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationContext _db;

		public ICategoryRepository Category { get; private set; }

		public IProductRepository Product { get; private set; }
        public ICompanyRepository Company { get; private set; }

		public UnitOfWork(ApplicationContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Product = new ProductRepository(_db);
            //Company = new CompanyRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
