using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Product.Data.Repositories
{
    public interface IProductRepo
    {
        Task<bool> Add(DTO.Product model);
        Task<List<DTO.Product>> All();
        Task<List<DTO.Product>> All(Guid categoryId);
        Task<bool> ChangeStatus(Guid productId);
        Task<DTO.Product> Get(Guid productId);
        Task<DTO.Product> Get(string productName);
        Task<bool> Remove(Guid productId);
        Task<bool> Update(DTO.Product model);
    }

    public class ProductRepo : IProductRepo
    {
        private readonly AppDbContext _db;
        public ProductRepo(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<DTO.Product>> All() => await _db.Products.ToListAsync();

        public async Task<List<DTO.Product>> All(Guid categoryId) => await _db.Products.Where(x => x.CategoryId == categoryId).ToListAsync();

        public async Task<DTO.Product> Get(Guid productId)
        {
            var product = await _db.Products.FirstOrDefaultAsync(x => x.Id == productId);
            return product == null ? null : product;
        }

        public async Task<DTO.Product> Get(string productName)
        {
            var product = await _db.Products.FirstOrDefaultAsync(x => x.Name == productName);
            return product == null ? null : product;
        }

        public async Task<bool> Add(DTO.Product model)
        {
            await _db.AddAsync(model);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Update(DTO.Product model)
        {
            if (await _db.Products.AnyAsync(x => x.Id == model.Id) == false)
            {
                return false;
            }

            _db.Update(model);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangeStatus(Guid productId)
        {
            var product = await Get(productId);

            if (product == null)
            {
                return false;
            }

            if (product.IsDisabled)
            {
                product.IsDisabled = false;
            }
            else
            {
                product.IsDisabled = true;
            }

            _db.Update(product);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Remove(Guid productId)
        {
            var product = await Get(productId);

            if (product == null)
            {
                return false;
            }

            _db.Remove(product);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}