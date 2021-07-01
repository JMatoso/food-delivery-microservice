using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Product.DTO;

namespace Product.Data.Repositories
{
    public interface ICategoryRepo
    {
        Task<bool> Add(Category model);
        Task<List<Category>> All();
        Task<bool> ChangeStatus(Guid categoryId);
        Task<Category> Get(Guid categoryId);
        Task<Category> Get(string categoryName);
        Task<bool> Remove(Guid categoryId);
        Task<bool> Update(Category model);
    }

    public class CategoryRepo : ICategoryRepo
    {
        private readonly AppDbContext _db;
        private readonly IMemoryCache _memoryCache;
        public CategoryRepo(
            AppDbContext db,
            IMemoryCache memoryCache
        )
        {
            _db = db;
            _memoryCache = memoryCache;
        }

        public async Task<List<Category>> All() 
        {
            List<Category> categories = new List<Category>();

            if (!_memoryCache.TryGetValue("CategoryCache", out categories))
            {
                if (categories == null)
                {
                    categories = await _db.Categories.ToListAsync();

                    var cacheOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(2));

                    _memoryCache.Set("CategoryCache", categories, cacheOptions);
                }
            }

            return categories;
        }

        public async Task<Category> Get(Guid categoryId)
        {
            var category = await _db.Categories.FirstOrDefaultAsync(x => x.Id == categoryId);
            return category == null ? null : category;
        }

        public async Task<Category> Get(string categoryName)
        {
            var category = await _db.Categories.FirstOrDefaultAsync(x => x.Name == categoryName);
            return category == null ? null : category;
        }

        public async Task<bool> Add(Category model)
        {
            if (await _db.Categories.AnyAsync(x => x.Name == model.Name))
            {
                return false;
            }

            await _db.AddAsync(model);
            await _db.SaveChangesAsync();

            _memoryCache.Remove("CategoryCache");

            return true;
        }

        public async Task<bool> Update(Category model)
        {
            _db.Update(model);
            await _db.SaveChangesAsync();

            _memoryCache.Remove("CategoryCache");

            return true;
        }

        public async Task<bool> ChangeStatus(Guid categoryId)
        {
            var category = await Get(categoryId);

            if (category == null)
            {
                return false;
            }

            if (category.IsDisabled)
            {
                category.IsDisabled = false;
            }
            else
            {
                category.IsDisabled = true;
            }

            _db.Update(category);
            await _db.SaveChangesAsync();

            _memoryCache.Remove("CategoryCache");

            return true;
        }

        public async Task<bool> Remove(Guid categoryId)
        {
            var category = await Get(categoryId);

            if (category == null)
            {
                return false;
            }

            _db.Remove(category);
            await _db.SaveChangesAsync();

            _memoryCache.Remove("CategoryCache");
            return true;
        }
    }
}