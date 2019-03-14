using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SignalrServerDemo.Identity;
using SignalrServerDemo.Models;

namespace SignalrServerDemo.Controllers {
    [Route ("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController (UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost ("Login")]
        public async Task<ActionResult<TokenDto>> Login ([FromBody] LoginDto dto) {
            var user = await _userManager.FindByNameAsync (dto.Login);
            if (user == null) {
                return NotFound ();
            }
            var r = await _signInManager.CheckPasswordSignInAsync (user, dto.Password, false);
            if (r.Succeeded) {
                var claims = await _userManager.GetClaimsAsync (user);
                claims.Add (new Claim (ClaimTypes.Name, user.UserName));
                claims.Add (new Claim (ClaimTypes.Role, "User"));
                return new TokenDto {
                    AccessToken = await GenerateJwt (claims),
                        RefreshToken = "refresh"
                };
            }
            return BadRequest ();
        }

        [HttpPost ("Register")]
        public async Task<ActionResult<TokenDto>> Register ([FromBody] LoginDto dto) {
            var user = new ApplicationUser {
                UserName = dto.Login
            };
            var r = await _userManager.CreateAsync (user, dto.Password);
            if (r.Succeeded) {
                var claims = await _userManager.GetClaimsAsync (user);
                claims.Add (new Claim (ClaimTypes.Name, user.UserName));
                claims.Add (new Claim (ClaimTypes.Role, "User"));
                return new TokenDto {
                    AccessToken = await GenerateJwt (claims),
                        RefreshToken = "refresh"
                };
            }
            return BadRequest ();
        }

        private Task<string> GenerateJwt (IList<Claim> claims) {
            var jwt = new JwtSecurityToken (
                issuer: "me",
                audience: "me",
                notBefore : DateTime.Now,
                claims : claims,
                expires : DateTime.Now.Add (TimeSpan.FromMinutes (30)),
                signingCredentials : new SigningCredentials (new SymmetricSecurityKey (Encoding.ASCII.GetBytes ("mysupersecret_secretkey!123")), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler ().WriteToken (jwt);
            return Task.FromResult (encodedJwt);
        }
    }
}