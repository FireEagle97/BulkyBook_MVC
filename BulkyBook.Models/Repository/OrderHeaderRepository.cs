using BulkyBook.Models.Models;
using BulkyBook.Models.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationContext _db;
        public OrderHeaderRepository(ApplicationContext db) : base(db)
        {
            _db = db;
        }

        public void update(OrderHeader orderHeader)
        {
            _db.Update(orderHeader);
        }
        
    }
}
