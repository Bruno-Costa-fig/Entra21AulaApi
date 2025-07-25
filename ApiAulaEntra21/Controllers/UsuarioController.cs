using ApiAulaEntra21.Data;
using ApiAulaEntra21.Models;
using ApiAulaEntra21.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiAulaEntra21.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;
        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Post([FromBody] UsuarioCreateDto newUsuario)
        {
            var existeEmail = _context.Usuario.Where(x => x.Email == newUsuario.Email).FirstOrDefault();

            if(existeEmail is not null)
            {
                return BadRequest("Já existe um usuário com este email");
            }

            var usuario = new Usuario()
            {
                Nome = newUsuario.Nome,
                Email = newUsuario.Email,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(newUsuario.Senha),
                Role = newUsuario.Role
            };

            _context.Usuario.Add(usuario);
            _context.SaveChanges();

            return Created("/usuario", new
            {
                usuario.Nome,
                usuario.Role
            });
        }
    }
}
