using API.Data;
using API.Model.Domain;
using API.Model.Dto;
using API.Model.Dto.Region;
using API.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegionController : ControllerBase
    {

        private readonly AppDBContext appDBContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionController(AppDBContext appDBContext, IRegionRepository regionRepository, IMapper mapper)
        {
            this.appDBContext = appDBContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? orderBy,
            [FromQuery] bool accending, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            try
            {
                //For Connect to db
                var regions = await regionRepository.GetAllAsync(filterOn,filterQuery, orderBy, accending, pageNumber, pageSize);

                var regionDTO = regions.Select(r => new RegionDTO
                {
                    Id = r.Id,
                    Name = r.Name,
                    Code = r.Code,
                    RegionImageUrl = r.RegionImageUrl
                }).ToList();
                return Ok(regions);

                if (regionDTO.Any()) return Ok(regionDTO);
                else return BadRequest(regionDTO);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("{Id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid Id)
        {
            try
            {
                var region = await regionRepository.GetByIdAsync(Id);
                if (region == null) return Ok("Region not found!");
                else
                {
                    var regionDTO = new RegionDTO
                    {
                        Id = region.Id,
                        Name = region.Name,
                        Code = region.Code,
                        RegionImageUrl = region.RegionImageUrl
                    };
                    return Ok(regionDTO);
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRequestRegionDTO addRequetRegionDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var regionDomainModel = new Region
                    {
                        Code = addRequetRegionDTO.Code,
                        Name = addRequetRegionDTO.Name,

                        RegionImageUrl = addRequetRegionDTO.RegionImageUrl
                    };

                    regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);
                    if (regionDomainModel == null) return StatusCode(400, "Fail to Add!");

                    var regionDto = mapper.Map<List<RegionDTO>>(regionDomainModel);

                    // CreatedAtAction response status code like 200 400 500 or ..
                    return CreatedAtAction(nameof(GetById), new { Id = regionDto[0].Id }, regionDto[0]);
                }
                else return StatusCode(400, ModelState);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        [Route("{Id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid Id, [FromBody] UpdateRequestRegionDTO updateRequestRegionDTO)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    Region request = new Region
                    {
                        Code = updateRequestRegionDTO.Code,
                        Name = updateRequestRegionDTO.Name,
                        RegionImageUrl = updateRequestRegionDTO.RegionImageUrl
                    };

                    request = await regionRepository.UpdateAsync(Id, request);
                    if (request == null) return StatusCode(400, "Update fail!");

                    var regionDto = mapper.Map<RegionDTO>(updateRequestRegionDTO);

                    // CreatedAtAction response status code like 200 400 500 or ..
                    return CreatedAtAction(nameof(GetById), new { Id = regionDto.Id }, regionDto);
                }
                else return StatusCode(400, ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        [Route("{Id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid Id)
        {
            try
            {
                Region region = await regionRepository.DeleteAsync(Id);
                if (region == null) return StatusCode(400, "Fail to delete!");
                return StatusCode(200, "Delete successfully!");
            }
            catch(Exception ex)
            {
                return StatusCode(400, "Fail to delete!");
            }
        }
        

    
    }
}
