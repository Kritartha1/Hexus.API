using AutoMapper;
using Hexus.Extensions;
using Hexus.Helper;
using Hexus.Models.Domain;
using Hexus.Models.DTO;
using Hexus.Repositories.Implementation;
using Hexus.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Hexus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> logger;
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;
        private readonly ITokenRepository tokenRepository;
        private readonly IUserRepository userRepository;

        public AuthController(ILogger<AuthController> logger,
            UserManager<User> userManager,
            IMapper mapper,
            ITokenRepository tokenRepository,
            IUserRepository userRepository)
        {
            this.logger = logger;
            this.userManager = userManager;
            this.mapper = mapper;
            this.tokenRepository = tokenRepository;
            this.userRepository = userRepository;
        }

        [HttpPost]
        [Route("Register")]

        public async Task<IActionResult> Register([FromBody] RegisterUserRequestDto registerRequestDto)
        {

            var identityUser = new User
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username,
                DisplayName=registerRequestDto.Username
            };
            var user_ = await userManager.FindByEmailAsync(registerRequestDto.Username);
            if (user_ != null)
            {
                return BadRequest("Already an user! Please sign in.");
            }

            //var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);
            //var c = identityResult;

            if (identityResult.Succeeded)
            {
                //Add roles to this User
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);
                    if (identityResult.Succeeded)
                    {
                        /*var identity_user = await userRepository.CreateAsync(identityUser);
                        if (identity_user == null)
                        {
                            await userManager.DeleteAsync(identityUser);

                            return BadRequest("Oops! something went wrong!");
                        }*/
                        var user = await userManager.FindByEmailAsync(identityUser.UserName);


                        var userDTO = mapper.Map<UserDto>(user);

                        return CreatedAtAction(nameof(GetById), new { id = userDTO.Id }, userDTO);


                    }
                }
            }

            // await userRepository.DeleteAsync(identityUser.Id);
            await userManager.DeleteAsync(identityUser);
            return BadRequest("Oops! something went wrong!");

        }


        [HttpGet]
        [Route("{id}")]
        //[Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {

            var userDomain = await userManager.FindByIdAsync(id);

            if (userDomain == null)
            {
                return NotFound();
            }

            var userDto = mapper.Map<UserDto>(userDomain);


            return Ok(userDto);


        }

       

        [HttpPost]
        [Route("Login")]

        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            
            var user = await userManager.FindByEmailAsync(loginRequestDto.Username);
            if (user != null)
            {
                var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                if (checkPasswordResult)
                {
                    //Get the roles of user
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles != null)
                    {
                        var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());
                        var response = new LoginResponseDto
                        {
                            Email = loginRequestDto.Username,
                            Roles = roles.ToList(),
                            JwtToken = jwtToken,
                            Id = user.Id
                        };
                        return Ok(response);

                    }
                    //create token
                    return BadRequest("No roles provided!");
                }
                return BadRequest("Wrong password!");
            }


            return BadRequest("No user found!");
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<UserDto>>> GetUsers([FromQuery]UserParams userParams)
        {
            var users=await userRepository.GetUsersAsync(userParams);
            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage,users.PageSize,
                users.TotalCount,users.TotalPages));
            return Ok(users);   
        }
    }
}
