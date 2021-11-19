using faceitapi.Context;
using faceitapi.Models;
using faceitapi.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace faceitapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly faceitContext faceitContext;
        private readonly IConfiguration config;

        public LoginController(IConfiguration configuration, faceitContext context)
        {
            faceitContext = context;
            config = configuration;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Login([FromBody] LoginEntry loginGet)
        {
            if (string.IsNullOrEmpty(loginGet.Senha) && string.IsNullOrEmpty(loginGet.GoogleId.ToString()))
            {
                return BadRequest();
            }

            var pessoa = await faceitContext.Pessoa
                .FirstOrDefaultAsync(x => x.Email == loginGet.Email && (x.Senha == loginGet.Senha || x.GoogleID.GetValueOrDefault() == loginGet.GoogleId));

            if (pessoa != null && pessoa.Excluido != true)
            {
                try
                {
                    if (pessoa.Tipo.Equals("PF"))
                    {
                        pessoa = await faceitContext.Pessoa
                            .Include(x => x.PessoaFisica)
                            .Include(x => x.Endereco)
                            .Include(x => x.PessoaSkill)
                            .Include(x => x.Anexo)
                            .Include(x => x.Imagem)
                            .FirstOrDefaultAsync(x => x.Email == loginGet.Email && (x.Senha == loginGet.Senha || x.GoogleID == loginGet.GoogleId));
                    }
                    else
                    {
                        pessoa = await faceitContext.Pessoa
                            .Include(x => x.PessoaJuridica)
                            .Include(x => x.Endereco)
                            .Include(x => x.PessoaSkill)
                            .Include(x => x.Anexo)
                            .Include(x => x.Imagem)
                            .FirstOrDefaultAsync(x => x.Email == loginGet.Email && (x.Senha == loginGet.Senha || x.GoogleID == loginGet.GoogleId));
                    }

                    var token = new Token { Value = GerarToken(pessoa), Date = DateTime.Now };

                    return Ok(new LoginRetun() { pessoa = pessoa, token = token });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(ex.Message, "Contate um administrador");
                    return BadRequest(ModelState);
                }
            }
            else
            {
                return NotFound();
            }
        }

        private string GerarToken(Pessoa pessoa)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            //Obtem a chave
            var securityKey = Encoding.ASCII.GetBytes(config["Jwt:Key"]);

            //Descreve o token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, pessoa.Email),
                    new Claim(ClaimTypes.Role, pessoa.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(securityKey), SecurityAlgorithms.HmacSha256Signature)
            };

            //Cria o token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}