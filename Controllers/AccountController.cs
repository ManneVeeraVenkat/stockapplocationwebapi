using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using stockapplocation.Dtos;
using stockapplocation.Interface;
using stockapplocation.Models;
using System.Linq;
using System.Threading.Tasks;

namespace stockapplocation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;

        }

        [HttpPost("Login")]

        public async Task<IActionResult> Logon([FromBody] LoginDto login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == login.UserName.ToLower());
            if (user == null)
            {
                return Unauthorized("Invalid UserName");
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized(" Invalid Username or Password");
            }
            return Ok(
                new NewUserDto
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                });

        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerRequestBody)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);



            var appUser = new AppUser
            {
                UserName = registerRequestBody.UserName,
                Email = registerRequestBody.Email
            };

            var createdUser = await _userManager.CreateAsync(appUser, registerRequestBody.Password);

            if (!createdUser.Succeeded)
                return BadRequest(createdUser.Errors.Select(e => e.Description));

            var roleResult = await _userManager.AddToRoleAsync(appUser, "User");

            if (!roleResult.Succeeded)
                return BadRequest(roleResult.Errors.Select(e => e.Description));

            return Ok(
                           new NewUserDto
                           {
                               UserName = appUser.UserName,
                               Email = appUser.Email,
                               Token = _tokenService.CreateToken(appUser)
                           }
                       );
        }
    }
}
