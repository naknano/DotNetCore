using API.Data;
using API.Model.Domain;
using API.Model.Dto;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class SQLRegionRepository : IRegionRepository
    {

        private readonly AppDBContext appDBContext;

        public SQLRegionRepository(AppDBContext appDBContext)
        {
            this.appDBContext = appDBContext;
        }

        public async Task<List<Region>> GetAllAsync(string filterOn= null, string filterQuery = null, string orderBy = null,
            bool accending = false, int pageNumber = 1, int pageSize = 20)
        {
            int pageSkip = (pageNumber - 1) * pageSize;
            var query = appDBContext.region.AsQueryable();

            if (string.IsNullOrWhiteSpace(filterOn) == false 
                    && string.IsNullOrWhiteSpace(filterQuery)) query = query.Where(q => q.Name.Contains(filterQuery));

            if (string.IsNullOrWhiteSpace(orderBy) == false) query = accending ? query.OrderBy(q => q.Name) : query.OrderByDescending(q => q.Name);
            return await appDBContext.region.Skip(pageSize).ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid Id)
        {
            return await appDBContext.region.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await appDBContext.AddAsync(region);
            await appDBContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region> DeleteAsync(Guid Id)
        {
            var region = await appDBContext.region.FirstOrDefaultAsync(r => r.Id == Id);
            if (region == null) return null;
            appDBContext.region.Remove(region);
            await appDBContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> UpdateAsync(Guid Id, Region region)
        {
            var request = await appDBContext.region.FirstOrDefaultAsync(r => r.Id == Id);
            if (request == null) return null;
            request.Code = region.Code;
            request.Name = region.Name;
            request.RegionImageUrl = region.RegionImageUrl;
            appDBContext.region.Update(request);
            await appDBContext.SaveChangesAsync();
            return region;
        }
    }
}
