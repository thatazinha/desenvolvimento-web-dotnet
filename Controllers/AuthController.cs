using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DW01.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DW01.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(IConfiguration configuration,UserManager<IdentityUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var usuario = await _userManager.FindByNameAsync(request.Usuario);

            if (usuario == null || !await _userManager.CheckPasswordAsync(usuario, request.Senha))
            {
                return Unauthorized(new
                {
                    mensagem = "Usuario ou senha invalidos."
                });
            }

            var roles = await _userManager.GetRolesAsync(usuario);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id),new Claim(ClaimTypes.Name, usuario.UserName!)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var jwtKey = _configuration["Jwt:Key"]!;
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            var chave = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey));

            var credenciais = new SigningCredentials(
                chave, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credenciais);

            return Ok(new{token = new JwtSecurityTokenHandler().WriteToken(token),usuario = usuario.UserName,perfil = roles});
        }
    }
}
