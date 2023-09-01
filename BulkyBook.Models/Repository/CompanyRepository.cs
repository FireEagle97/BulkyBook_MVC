//using BulkyBook.Models.Models;
//using BulkyBook.Models.Repository.IRepository;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;


//public class CompanyRepository : Repository<Company>, ICompanyRepository
//{
//    private readonly ApplicationContext _db;

//    public CompanyRepository(ApplicationContext db) : base(db)
//    {
//        _db = db;
//    }

//    public void Update(Company company) 
//    {
//    var objFromDb = _db.Company.FirstOrDefault(u => u.Id == company.Id);
//    if (objFromDb != null)
//    {
//        objFromDb.StreetAddress = company.StreetAddress;
//        objFromDb.City = company.City;
//        objFromDb.State = company.State;
//        objFromDb.Name = company.Name;
//        objFromDb.PhoneNumber = company.PhoneNumber;
//        objFromDb.PostalCode = company.PostalCode;
//    }
//    }
//    public void Save()
//    {
//        _db.SaveChanges();
//    }
//}

