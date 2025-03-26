using API.Model.Domain;
using API.Model.Dto;
using API.Model.Dto.Difficulty;
using API.Model.Dto.Region;
using API.Model.Dto.Walk;
using AutoMapper;

namespace API.Mapper
{
    public class AutoMapperProfiles : Profile
    {

        public AutoMapperProfiles()
        {
            // FROM SOURCE TO DESTINATION

            // Region
            CreateMap<Region, RegionDTO>().ReverseMap();
            CreateMap<AddRequestRegionDTO, Region>().ReverseMap();
            CreateMap<UpdateRequestRegionDTO, Region>().ReverseMap();

            // Walk
            CreateMap<AddRequestWalkDTO, Walk>().ReverseMap();
            CreateMap<Walk, WalkDTO>().ReverseMap();
            CreateMap<UpdateRequestWalkDTO, Walk>().ReverseMap();

            // Difficulty
            CreateMap<Difficulty, DifficultyDTO>().ReverseMap();
            CreateMap<DifficultyDTO, Difficulty>().ReverseMap();
        }
    }
}
