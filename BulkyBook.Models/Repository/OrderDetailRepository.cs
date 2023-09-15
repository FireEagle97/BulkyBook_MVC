using BulkyBook.Models.Models;
using BulkyBook.Models.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models.Repository
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private readonly ApplicationContext _db;
        public OrderDetailRepository(ApplicationContext db) : base(db)
        {
            _db = db;
        }

        public void update(OrderDetail orderDetail)
        {
            _db.Update(orderDetail);
        }
    }
}
