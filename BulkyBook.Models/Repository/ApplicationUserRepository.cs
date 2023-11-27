using BulkyBook.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models.Repository.IRepository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationContext _db;

        public ApplicationUserRepository(ApplicationContext db) : base(db)
        {
            _db = db;

        }
        public void Update(ApplicationUser user)
        {
            _db.ApplicationUsers.Update(user);
        }
    }
}
