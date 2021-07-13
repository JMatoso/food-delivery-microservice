using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Order.Data.Repositories
{
    public interface IOrderRepo
    {
        Task<bool> Add(DTO.Order model);
        Task<List<DTO.Order>> All();
        Task<List<DTO.Order>> All(Guid clientId);
        Task<DTO.Order> Get(Guid orderId);
        Task<bool> ChangeStatus(DTO.Order model);
    }

    public class OrderRepo : IOrderRepo
    {
        private readonly AppDbContext _db;
        public OrderRepo(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<DTO.Order>> All() => await _db.Orders.ToListAsync();

        public async Task<List<DTO.Order>> All(Guid clientId) => await _db.Orders.Where(x => x.ClientId == clientId).ToListAsync();

        public async Task<DTO.Order> Get(Guid orderId)
        {
            var order = await _db.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
            return order == null ? null : order;
        }

        public async Task<bool> Add(DTO.Order model)
        {
            await _db.AddAsync(model);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangeStatus(DTO.Order model)
        {
            _db.Update(model);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}