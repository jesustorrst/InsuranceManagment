using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models.DTO; 
namespace PL.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly Business.UsuarioBLL _usuarioService;
        private readonly IConfiguration _config;

        public UsuarioController(UsuarioBLL usuarioService, IConfiguration config)
        {
            _usuarioService = usuarioService;
            _config = config;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] UsuarioDTO usuarioDto)
        {
            var result = await _usuarioService.Add(usuarioDto);
            if (result.Correct) return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var result = await _usuarioService.Login(loginDto);

            if (result.Correct)
            {
                var authResponse = (AuthResponse)result.Object;
                authResponse.Token = GenerarToken(authResponse);

                result.Object = authResponse;
                return Ok(result);
            }

            return Unauthorized(result);
        }

        // Método privado para firmar el JWT
        private string GenerarToken(AuthResponse auth)
        {
            var secretKey = _config.GetSection("Jwt:Key").Value;
            var keyBytes = Encoding.ASCII.GetBytes(secretKey);

            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.Email, auth.Email));
            claims.AddClaim(new Claim(ClaimTypes.Role, auth.Rol));
            if (auth.IdCliente.HasValue)
                claims.AddClaim(new Claim("IdCliente", auth.IdCliente.ToString()));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(tokenConfig);
        }
    }
}
