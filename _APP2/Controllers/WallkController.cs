using API.Data;
using API.Model.Domain;
using API.Model.Dto.Walk;
using API.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class WallkController : ControllerBase
    {
        private readonly IWalkRepository repository;
        private readonly IMapper mapper;

        // test

        public WallkController(IWalkRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool accending, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            try
            {
               List<Walk> response = await repository.getAllAsync(filterOn, filterQuery, sortBy, accending, pageNumber, pageSize);
               if (response == null) return StatusCode(400, "Walk not found!");
               return StatusCode(200,response);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] AddRequestWalkDTO addRequestWalkDTO)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var walkDomain = mapper.Map<Walk>(addRequestWalkDTO);
                    Guid id = Guid.NewGuid();
                    walkDomain.Id = id;

                    Difficulty difficulty = new Difficulty
                    {
                        Id = id,
                        Name = addRequestWalkDTO.Name
                    };

                    Region region = new Region
                    {
                        Id = id,
                        Name = addRequestWalkDTO.Name,
                        Code = id.ToString()
         
                    };

                    walkDomain.Difficulty = difficulty;
                    walkDomain.Region = region;
                    var response = await repository.createAsync(walkDomain);
                    if (response == null) return StatusCode(400, "Walk can not add!");
                    return StatusCode(200, "Successfully add!");
                }
                else return StatusCode(400, ModelState);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("{Id:guid}")]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute] Guid Id)
        {
            try
            {
                Walk response = await repository.getByIdAsync(Id);
                if (response == null) return StatusCode(400, "Walk not found!");
                return StatusCode(200, response);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        [Route("{Id:guid}")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] Guid Id, UpdateRequestWalkDTO updateRequestWalkDTO)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    Walk walk = mapper.Map<Walk>(updateRequestWalkDTO);
                    Walk response = await repository.updateAsync(Id, walk);
                    if (response == null) return StatusCode(400, "Update walk fail!");
                    return CreatedAtAction(nameof(GetById), new { Id = response.Id }, response);
                }
                else return StatusCode(400, "Fail to delete!");
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        [Route("{Id:Guid}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] Guid Id)
        {
            try
            {
                Walk walk = await repository.deleteAsync(Id);
                if (walk == null) return StatusCode(400, "Delete walk fail!");
                return StatusCode(200, "Delete successfully!");
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
