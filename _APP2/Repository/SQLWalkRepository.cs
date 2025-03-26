using API.Data;
using API.Mapper;
using API.Model.Domain;
using API.Model.Dto.Walk;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
//using Microsoft.Identity.Client;
using System.Drawing;

namespace API.Repository
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly AppDBContext appDBContext;
        private readonly IMapper mapper;

        public SQLWalkRepository(AppDBContext appDBContext, IMapper mapper)
        {
            this.appDBContext = appDBContext;
            this.mapper = mapper;
        }

        public async Task<Walk?> createAsync(Walk walk)
        {  
            await appDBContext.AddAsync(walk);
            await appDBContext.SaveChangesAsync();
            return walk;
        }

        public async Task<List<Walk>> getAllAsync(string filterOn = null, string filterQuery = null, string sortBy = null,
            bool accending = false, int pageNumber = 1, int pageSize = 20)
        {
            int pageSkip = (pageNumber - 1) * pageSize;
            var query = appDBContext.walk.Include("Difficulty").Include("Region").AsQueryable();
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery))
            {
                // use if else 
                //if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase)) query = query.Where(q => q.Name.Contains(filterQuery));
                switch (filterOn.ToUpper())
                {
                    case "NAME":
                        query = query.Where(q => q.Name.Contains(filterQuery));
                        break;
                    case "DESCRIPTION":
                        query = query.Where(q => q.Description.Contains(filterQuery));
                        break;
                    default: break;
                }   
            }

            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                switch(sortBy.ToUpper())
                {
                    case "NAME":
                        query = accending ? query.OrderBy(q => q.Name) : query.OrderByDescending(q => q.Name);
                        break;
                    case "DESCRIPTION":
                        query = accending ? query.OrderBy(q => q.Description) : query.OrderByDescending(q => q.Description);
                        break;
                    case "LengthInKm":
                        query = accending ? query.OrderBy(q => q.LengthInKm) : query.OrderByDescending(q => q.LengthInKm);
                        break;
                    default: break;
                }
            }
     

            return await query.Skip(pageSkip).ToListAsync();
        }

        public async Task<Walk> getByIdAsync(Guid Id)
        {
            return await appDBContext.walk
                            .Include("Difficulty")
                            .Include("Region")
                            .FirstOrDefaultAsync(w => w.Id == Id);
        }

        public async Task<Walk> updateAsync(Guid Id, Walk walk)
        {
            Walk responseWalk = await appDBContext.walk.FirstOrDefaultAsync(w => w.Id == Id);
            if (responseWalk == null) return null;

            responseWalk.Name = walk.Name;
            responseWalk.WalkImageUrl = walk.WalkImageUrl;
            responseWalk.Description = walk.Description;
            responseWalk.LengthInKm = walk.LengthInKm;

            appDBContext.Update(responseWalk);
            await appDBContext.SaveChangesAsync();

            return responseWalk;
        }

        public async Task<Walk> deleteAsync(Guid Id)
        {
            Walk walk = await appDBContext.walk.FirstOrDefaultAsync(w => w.Id == Id);
            if (walk == null) return null;

            appDBContext.Remove(walk);
            await appDBContext.SaveChangesAsync();
            return walk;
            
        }

    }
}
