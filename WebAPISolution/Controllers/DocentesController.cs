using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPISolution.Models;
using WebAPISolution.Contexts;

namespace WebAPISolution.Controllers
{
    [Route("api/[controller]")]
    //    api/docentes
    [ApiController]
    public class DocentesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DocentesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Docentes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Docente>>> GetDocente()
        {
            return await _context.Docente.ToListAsync();
        }

        // GET: api/Docentes/5
        [HttpGet("{docenteid}")]
        public async Task<ActionResult<Docente>> GetDocente(int docenteid)
        {
            var docente = await _context.Docente.FindAsync(docenteid);

            if (docente == null)
            {
                return NotFound();
            }

            return docente;
        }

        // PUT: api/Docentes/5
        [HttpPut("{docenteid}")]
        public async Task<IActionResult> PutDocente(int docenteid, Docente docente)
        {
            if (docenteid != docente.DocenteId)
            {
                return BadRequest();
            }

            _context.Entry(docente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocenteExists(docenteid))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Docentes
        [HttpPost]
        public async Task<ActionResult<Docente>> PostDocente(Docente docente)
        {
            _context.Docente.Add(docente);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDocente", new { docenteid = docente.DocenteId }, docente);
        }

        // DELETE: api/Docentes/5
        [HttpDelete("{docenteid}")]
        public async Task<IActionResult> DeleteDocente(int docenteid)
        {
            var docente = await _context.Docente.FindAsync(docenteid);
            if (docente == null)
            {
                return NotFound();
            }

            _context.Docente.Remove(docente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DocenteExists(int docenteid)
        {
            return _context.Docente.Any(e => e.DocenteId == docenteid);
        }
    }
}
