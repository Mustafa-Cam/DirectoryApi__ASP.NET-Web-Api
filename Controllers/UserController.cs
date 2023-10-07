using DirectoryApi.Data;
using DirectoryApi.Helpers;
using DirectoryApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace DirectoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataDpContext _datadbcontext;
        
        public UserController(DataDpContext datadbcontext)
        {
            _datadbcontext = datadbcontext;
        }
     
        [HttpPost("authenticate")]
        public async Task <IActionResult> Authenticate([FromBody] User userObj)
        {
            if(userObj == null)
                return BadRequest(string.Empty);
            var user = await _datadbcontext.Users.FirstOrDefaultAsync(x=>x.Email == userObj.Email);
            if(user == null)
                return NotFound(new {Message = "Usernot found"});

            if (!PasswordHashed.VerifyPassword(userObj.Password, user.Password))
            {
                return BadRequest(new { Message = "Password is Incorrect" });
            }
            user.Token = CreateJwt(user);
            return Ok(new
            {
                Message = "Login succsess",
                Token = user.Token
            });
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] User userObj)
        {
            if(userObj == null)
                return BadRequest(string.Empty);
            // check email
            if (await CheckEmailExistAsync(userObj.Email))
                return BadRequest(new { Message = "Email Already Exist" });

            var passMessage = CheckPasswordStrength(userObj.Password);
            if (!string.IsNullOrEmpty(passMessage))
                return BadRequest(new { Message = passMessage.ToString() });
            userObj.Password = PasswordHashed.HashPassword(userObj.Password);
            userObj.Role = "User";
            userObj.Token = "";
            await _datadbcontext.Users.AddAsync(userObj);
            await _datadbcontext.SaveChangesAsync();
            return Ok(new {Message="User Registered"});

        }

        private static string CheckPasswordStrength(string pass)
        {
            StringBuilder sb = new StringBuilder();
            if (pass.Length < 9)
                sb.Append("Minimum password length should be 8" + Environment.NewLine);
            if (!(Regex.IsMatch(pass, "[a-z]") && Regex.IsMatch(pass, "[A-Z]") && Regex.IsMatch(pass, "[0-9]")))
                sb.Append("Password should be AlphaNumeric" + Environment.NewLine);
            if (!Regex.IsMatch(pass, "[<,>,@,!,#,$,%,^,&,*,(,),_,+,\\[,\\],{,},?,:,;,|,',\\,.,/,~,`,-,=]"))
                sb.Append("Password should contain special charcter" + Environment.NewLine);
            return sb.ToString();
        }

        private Task<bool> CheckEmailExistAsync(string? email)
           => _datadbcontext.Users.AnyAsync(x => x.Email == email);

        private  string CreateJwt(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("veryverysceret.....");
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, $"{user.FirstName}"),
                 new Claim(ClaimTypes.Name, $"{user.LastName}"),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name,$"{user.Email}"),
                new Claim("UserId",$"{user.Id}")

            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddSeconds(10),
                SigningCredentials = credentials
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<User>> GetAllUsers()
        {
            return Ok(await _datadbcontext.Users.ToListAsync());
        }
    }
}
               
