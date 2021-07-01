using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Order.DTO;

namespace Order.Data.Repositories
{
    public interface ICartRepo
    {
        Task<bool> Add(Cart model);
        Task<List<Cart>> All(Guid clienteId);
        Task<Cart> Get(Guid cartId);
        Task<bool> Remove(Guid cartId);
    }

    public class CartRepo : ICartRepo
    {
        private readonly AppDbContext _db;
        public CartRepo(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Cart>> All(Guid clienteId) => await _db.Cart.Where(x => x.ClientId == clienteId).ToListAsync();

        public async Task<Cart> Get(Guid cartId)
        {
            var cart = await _db.Cart.FirstOrDefaultAsync(x => x.Id == cartId);
            return cart == null ? null : cart;
        }

        public async Task<bool> Add(Cart model)
        {
            await _db.AddAsync(model);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Remove(Guid cartId)
        {
            var cart = await Get(cartId);

            if (cart == null)
            {
                return false;
            }

            _db.Remove(cart);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}