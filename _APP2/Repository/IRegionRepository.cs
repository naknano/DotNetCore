using API.Model.Domain;

namespace API.Repository
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllAsync(string filterOn, string filterQuery, string orderBy, bool accending, int pageNumber, int pageSize);
        Task<Region?> GetByIdAsync(Guid Id);
        Task<Region> CreateAsync(Region region);
        Task<Region?> UpdateAsync(Guid Id,Region region);
        Task<Region> DeleteAsync(Guid Id);
    }
}
