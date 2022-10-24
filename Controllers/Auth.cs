using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

using System.Threading.Tasks;
using DatingApp.API.DTOs;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;


using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace DatingApp.API.Controllers
{   
    // [Authorize]
    [ApiController]
    [Route("api/Auth")]
    public class Auth : ControllerBase
    {
        public IAuthRepository _repo { get; }
        public IConfiguration _config { get; }
      public Auth(IAuthRepository repo, IConfiguration config)
      {
            _config = config;
            _repo = repo;
        
      }  
      [AllowAnonymous]
      [HttpPost("register")]
      public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
      {
        userRegisterDto.username = userRegisterDto.username.ToLower();
        if(await _repo.UserExists(userRegisterDto.username))
            return BadRequest("User already exists");

        var userToBeCreated = new User{
            Username = userRegisterDto.username
        };

        var CreatedUser = await _repo.Register(userToBeCreated, userRegisterDto.password);
        return StatusCode(201);
      }
      // [AllowAnonymous]
      [HttpPost("login")]
      public async Task<IActionResult> Login(UserLoginDto userLoginDto)
      {
        var userFromRepo = await _repo.Login(userLoginDto.Username, userLoginDto.Password);
        if (userFromRepo == null){
          return Unauthorized();
        }
          
        var claims = new []
        {
          new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
          new Claim(ClaimTypes.Name, userFromRepo.Username)
        };
      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

      var tokenDescriptor = new SecurityTokenDescriptor{
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.Now.AddDays(1),
        SigningCredentials = creds,
      };

      var tokenHandler = new JwtSecurityTokenHandler();
      var token = tokenHandler.CreateToken(tokenDescriptor);

        return Ok(new{
          token = tokenHandler.WriteToken(token)
        });
      }
    }
}