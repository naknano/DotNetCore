using API.Model.Domain;

namespace API.Repository
{
    public interface IWalkRepository
    {

        Task<Walk?> createAsync(Walk walk);
        Task<List<Walk>> getAllAsync(string filterOn, string filterQuery, string sortBy,
            bool Asscending, int pageNumber, int pageSize );
        Task<Walk> getByIdAsync(Guid Id);
        Task<Walk> updateAsync(Guid Id, Walk walk);
        Task<Walk> deleteAsync(Guid Id);
    }
}
