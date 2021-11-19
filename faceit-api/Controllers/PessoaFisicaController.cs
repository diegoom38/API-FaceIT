using faceitapi.Context;
using faceitapi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace faceitapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PessoaFisicaController : ControllerBase
    {
        private readonly faceitContext faceitContext;

        public PessoaFisicaController(faceitContext context)
        {
            faceitContext = context;
        }

        //Esse metodo será removido para produção
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "adm")]
        public async Task<IActionResult> GetAll()
        {
            var data = await faceitContext.PessoaFisica
                .Include(x => x.IDPessoaNavigation)
                .Include(x => x.IDPessoaNavigation.Anexo)
                .Include(x => x.IDPessoaNavigation.Imagem)
                .Include(x => x.IDPessoaNavigation.Endereco)
                .Where(x => x.IDPessoaNavigation.Excluido == false)
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var data = await faceitContext.PessoaFisica
                .Include(x => x.IDPessoaNavigation)
                .Include(x => x.IDPessoaNavigation.PessoaSkill)
                .Include(x => x.IDPessoaNavigation.Endereco)
                .Include(x => x.IDPessoaNavigation.Anexo)
                .Include(x => x.IDPessoaNavigation.Imagem)
                .FirstOrDefaultAsync(x => x.IDPessoa == id);

                if (data.IDPessoaNavigation.Excluido == false)
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> Insert([FromBody] PessoaFisica model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.IDPessoaNavigation.Excluido = false;
                    model.IDPessoaNavigation.Tipo = "PF";
                    model.IDPessoaNavigation.Role = "user";
                    await faceitContext.Pessoa.AddAsync(model.IDPessoaNavigation);
                    await faceitContext.PessoaFisica.AddAsync(model);
                    await faceitContext.Endereco.AddAsync(model.IDPessoaNavigation.Endereco);

                    if (model.IDPessoaNavigation.PessoaSkill.Count > 0)
                    {
                        await faceitContext.PessoaSkill.AddRangeAsync(model.IDPessoaNavigation.PessoaSkill);
                    }
                    if (model.IDPessoaNavigation.Imagem != null)
                    {
                        await faceitContext.Imagem.AddAsync(model.IDPessoaNavigation.Imagem);
                    }
                    if (model.IDPessoaNavigation.Anexo != null)
                    {
                        await faceitContext.Anexo.AddAsync(model.IDPessoaNavigation.Anexo);
                    }

                    await faceitContext.SaveChangesAsync();

                    return Created("/api/PessoaFisica", model);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(ex.Message, "Contate um administrador");
                    return BadRequest(ModelState);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Metodo serve tanto para atualizar como para apagar, já que o registro não será apagado fisicamente.
        /// </summary>
        /// <param name="model">Objeto modificado</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> EditOrDelete([FromBody] PessoaFisica model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var skillAux = await faceitContext.PessoaSkill.Where(x => x.IDPessoa.Equals(model.IDPessoa)).ToListAsync();
                    faceitContext.PessoaSkill.RemoveRange(skillAux);
                    await faceitContext.SaveChangesAsync();

                    await faceitContext.PessoaSkill.AddRangeAsync(model.IDPessoaNavigation.PessoaSkill);
                    faceitContext.Pessoa.Update(model.IDPessoaNavigation);
                    faceitContext.PessoaFisica.Update(model);
                    faceitContext.Endereco.Update(model.IDPessoaNavigation.Endereco);

                    if (model.IDPessoaNavigation.Imagem != null)
                    {
                        try
                        {
                            faceitContext.Imagem.Update(model.IDPessoaNavigation.Imagem);
                        }
                        catch (Exception ex)
                        {
                            await faceitContext.Imagem.AddAsync(model.IDPessoaNavigation.Imagem);
                        }
                    }

                    if (model.IDPessoaNavigation.Anexo != null)
                    {
                        try
                        {
                            faceitContext.Anexo.Update(model.IDPessoaNavigation.Anexo);
                        }
                        catch (Exception)
                        {
                            await faceitContext.Anexo.AddAsync(model.IDPessoaNavigation.Anexo);
                        }
                    }

                    await faceitContext.SaveChangesAsync();

                    return Accepted(model);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(ex.Message, "Contate um administrador");
                    return BadRequest(ModelState);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}