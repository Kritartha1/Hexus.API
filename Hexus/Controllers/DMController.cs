using AutoMapper;
using Hexus.Models.Domain;
using Hexus.Models.DTO;
using Hexus.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Hexus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DMController : ControllerBase
    {
        private readonly IDMRepository dMRepository;
        private readonly IMapper mapper;
      //  private readonly UserManager<User> userManager;

        public DMController(IDMRepository dMRepository,
            IMapper mapper
            )
        {
            this.dMRepository = dMRepository;
            this.mapper = mapper;
           
        }

        [HttpGet]
        [Route("{id:Guid}")]
        /* [Authorize(Roles = "User,Admin")]*/
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {

            var dmDomain = await dMRepository.GetByIdAsync(id);

            if (dmDomain == null)
            {
                return NotFound();
            }
            var dmDto = mapper.Map<DMDto>(dmDomain);

            return Ok(dmDto);

        }

        [HttpPost]
       // [Authorize(Roles = "User")]

        public async Task<IActionResult> Create([FromBody] AddDMRequestDto addDMRequestDto)
        {
            
            var DMModel = mapper.Map<DM>(addDMRequestDto);

            DMModel = await dMRepository.CreateAsync(DMModel);

            var DMDTO = mapper.Map<DMDto>(DMModel);

            return CreatedAtAction(nameof(GetById), new { id = DMDTO.Id }, DMDTO);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {


            var DMDomain = await dMRepository.GetAllAsync();

           

            var DMsDto = mapper.Map<List<DMDto>>(DMDomain);

            return Ok(DMsDto);

        }

    }
}
