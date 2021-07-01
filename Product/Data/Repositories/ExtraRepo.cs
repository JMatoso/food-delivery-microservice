using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Product.DTO;

namespace Product.Data.Repositories
{
    public interface IExtraRepo
    {
        Task<bool> Add(Extra model);
        Task<List<Extra>> All();
        Task<List<Extra>> All(Guid productId);
        Task<bool> ChangeStatus(Guid extraId);
        Task<Extra> Get(Guid extraId);
        Task<Extra> Get(string extraName);
        Task<bool> Remove(Guid extraId);
        Task<bool> Update(Extra model);
    }

    public class ExtraRepo : IExtraRepo
    {
        private readonly AppDbContext _db;
        public ExtraRepo(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Extra>> All() => await _db.Extras.ToListAsync();

        public async Task<List<Extra>> All(Guid productId) => await _db.Extras.Where(x => x.ProductId == productId).ToListAsync();

        public async Task<Extra> Get(Guid extraId)
        {
            var extra = await _db.Extras.FirstOrDefaultAsync(x => x.Id == extraId);
            return extra == null ? null : extra;
        }

        public async Task<Extra> Get(string extraName)
        {
            var extra = await _db.Extras.FirstOrDefaultAsync(x => x.Name == extraName);
            return extra == null ? null : extra;
        }

        public async Task<bool> Add(Extra model)
        {
            await _db.AddAsync(model);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Update(Extra model)
        {
            if (await _db.Extras.AnyAsync(x => x.Id == model.Id) == false)
            {
                return false;
            }

            _db.Update(model);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangeStatus(Guid extraId)
        {
            var extra = await Get(extraId);

            if (extra == null)
            {
                return false;
            }

            if (extra.IsDisabled)
            {
                extra.IsDisabled = false;
            }
            else
            {
                extra.IsDisabled = true;
            }

            _db.Update(extra);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Remove(Guid extraId)
        {
            var extra = await Get(extraId);

            if (extra == null)
            {
                return false;
            }

            _db.Remove(extra);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}