using _APP2.Data;
using _APP2.Model.Dto.Auth;
using _APP2.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _APP2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> userRole;
        private readonly ITokenRepositoryl toekn;

        private readonly AuthDBContext authDBContext;


        public AuthController(UserManager<IdentityUser> userManager, AuthDBContext authDBContext,
            RoleManager<IdentityRole> userRole, ITokenRepositoryl toekn)
        {
            this.userManager = userManager;
            this.authDBContext = authDBContext;
            this.userRole = userRole;
            this.toekn = toekn;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return StatusCode(200, await userManager.Users.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            await using var transaction = await authDBContext.Database.BeginTransactionAsync();
            try
            {
                string id = Guid.NewGuid().ToString();
                IdentityUser user = new IdentityUser
                {
                    UserName = registerRequestDTO.Username,
                    Email = registerRequestDTO.Username
                };

                string password = registerRequestDTO.Password.ToString();

                var response = await userManager.CreateAsync(user, password);
                if (response.Succeeded)
                {
                    if (registerRequestDTO.Roles.Any())
                    {
                        response = await userManager.AddToRolesAsync(user, registerRequestDTO.Roles);
                        if (response.Succeeded)
                        {
                            // commit transaction
                            await transaction.CommitAsync();
                            return StatusCode(200, "Successfully!");
                        }
                        return StatusCode(400, "Add role fail!");
                    }
                } return StatusCode(400, "Create Sser is fail!");

            }
            catch(Exception ex)
            {
                // rollback
                await transaction.RollbackAsync();
                return StatusCode(400, ex.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO )
        {

            var username = await userManager.FindByEmailAsync(loginRequestDTO.Username);
            if (username is null) return StatusCode(400, "Wrong username!");

            var response = await userManager.CheckPasswordAsync(username, loginRequestDTO.Password);
            if (!response) return StatusCode(400, "Username or Password are not match!");


            var roles = await userManager.GetRolesAsync(username);
            if (roles is null) return StatusCode(400, "Roles are not found!");

            var JwtToken  = toekn.CreateJWTToken(username, roles.ToList());
            if (JwtToken is null) return StatusCode(400, "Token is not found!");

            var tokenResponse = new LoginResponseDTO {  token = JwtToken };

            return StatusCode(200, tokenResponse);
        }
    }
}
