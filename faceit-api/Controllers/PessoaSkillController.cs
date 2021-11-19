using faceitapi.Context;
using faceitapi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace faceitapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PessoaSkillController : ControllerBase
    {
        private readonly faceitContext faceitContext;

        public PessoaSkillController(faceitContext context)
        {
            faceitContext = context;
        }

        [HttpGet("{IDPessoa}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSkillsPessoa(int IDPessoa)
        {
            try
            {
                var data = await faceitContext.PessoaSkill
                    .Where(x => x.IDPessoa == IDPessoa)
                    .Include(x => x.ID)
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Insert([FromBody] List<PessoaSkill> skills)
        {
            try
            {
                await faceitContext.PessoaSkill.AddRangeAsync(skills);
                await faceitContext.SaveChangesAsync();

                return StatusCode(StatusCodes.Status201Created, skills);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteSkillPessoa([FromBody] List<PessoaSkill> skills)
        {
            try
            {
                faceitContext.PessoaSkill.RemoveRange(skills);
                await faceitContext.SaveChangesAsync();
                return Accepted(skills);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}