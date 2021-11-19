using faceitapi.Context;
using faceitapi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace faceitapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidatoController : ControllerBase
    {
        private readonly faceitContext _faceitContext;
        private readonly IConfiguration _configuration;

        public CandidatoController(faceitContext faceitContext, IConfiguration configuration)

        {
            _faceitContext = faceitContext;
            _configuration = configuration;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var data = await _faceitContext.Candidato
                    .ToListAsync();

                if (data.Count > 0)
                {
                    return Ok(data);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.Message, "Contate um administrador");
                return BadRequest(ModelState);
            }
        }

        [HttpGet("{idProposta}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> GetCandidatoByProposta(int idProposta)
        {
            try
            {
                var data = await _faceitContext.Candidato
                    .Where(x => x.IDProposta == idProposta)
                    .ToListAsync();

                if (data.Count > 0)
                {
                    return Ok(data);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.Message, "Contate um administrador");
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [Route("Propostas/{idPessoa}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> ObterPropostasCandidatadas(int idPessoa)
        {
            try
            {
                var data = await _faceitContext.Proposta
                    .Include(x => x.Candidato)
                    .Where(x => x.Candidato.Any(y => y.IDPessoa == idPessoa))
                    .ToListAsync();

                if (data.Count > 0)
                {
                    return Ok(data);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.Message, "Contate um administrador");
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> Insert([FromBody] Candidato candidato)
        {
            try
            {
                await _faceitContext.Candidato.AddAsync(candidato);
                await _faceitContext.SaveChangesAsync();
                EnviarEmailNovoCandidato(candidato);

                return Created("api/Candidato", candidato);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.Message, "Contate um administrador");
                return BadRequest(ModelState);
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> Delete([FromBody] Candidato candidato)
        {
            try
            {
                _faceitContext.Candidato.Remove(candidato);
                await _faceitContext.SaveChangesAsync();

                return Accepted();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.Message, "Contate um administrador");
                return BadRequest(ModelState);
            }
        }

        private void EnviarEmailNovoCandidato(Candidato candidato)
        {
            MailMessage mail = new MailMessage();

            StringBuilder body = new StringBuilder();
            body.AppendLine($"Olá!");
            body.AppendLine($"Estamos entrando em contato para informar que {ObterNomeCandidato(candidato.IDPessoa)} acabou de se candidatar a uma oferta de que você está oferecendo.");
            body.AppendLine("Para mais detalhes acesse o aplicativo WEB ou mobile.");
            body.AppendLine("Atenciosamente equipe FaceIT.");

            mail.From = new MailAddress("faceitapisenaiarytorres@gmail.com");//de
            mail.To.Add(ObterEmailEempresa(candidato.IDProposta)); // para
            mail.Subject = "Novo candidato"; // assunto
            mail.Body = body.ToString(); // mensagem

            // em caso de anexos
            //mail.Attachments.Add(new Attachment(@"C:\teste.txt"));

            using (var smtp = new SmtpClient("smtp.gmail.com"))
            {
                smtp.EnableSsl = true; // GMail requer SSL
                smtp.Port = 587;       // porta para SSL
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network; // modo de envio
                smtp.UseDefaultCredentials = false; // vamos utilizar credencias especificas

                // seu usuário e senha para autenticação
                smtp.Credentials = new NetworkCredential(_configuration["Email:email"], _configuration["Email:senha"]);

                // envia o e-mail
                smtp.Send(mail);
            }
        }

        private string ObterEmailEempresa(int idProdosta)
        {
            return _faceitContext.Proposta
                .Include(x => x.IDEmpresaNavigation)
                .FirstOrDefault(x => x.IDProposta.Equals(idProdosta))
                .IDEmpresaNavigation.Email;
        }

        private string ObterNomeCandidato(int idPessoa)
        {
            var pessoa = _faceitContext.Pessoa
                .Include(x => x.PessoaFisica)
                .Include(x => x.PessoaJuridica)
                .FirstOrDefault(x => x.IDPessoa.Equals(idPessoa));

            return pessoa.PessoaFisica?.Nome ?? pessoa.PessoaJuridica.RazaoSocial;
        }
    }
}