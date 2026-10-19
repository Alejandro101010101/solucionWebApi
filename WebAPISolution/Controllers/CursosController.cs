using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPISolution.Models;
using WebAPISolution.Contexts;

namespace WebAPISolution.Controllers
{
    [Route("api/[controller]")]
    //    api/cursos
    [ApiController]
    public class CursosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CursosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Cursos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Curso>>> GetCurso()
        {
            return await _context.Curso.ToListAsync();
        }

        // GET: api/Cursos/5
        [HttpGet("{cursoid}")]
        public async Task<ActionResult<Curso>> GetCurso(int cursoid)
        {
            var curso = await _context.Curso.FindAsync(cursoid);

            if (curso == null)
            {
                return NotFound();
            }

            return curso;
        }

        // PUT: api/Cursos/5
        [HttpPut("{cursoid}")]
        public async Task<IActionResult> PutCurso(int cursoid, Curso curso)
        {
            if (cursoid != curso.CursoId)
            {
                return BadRequest();
            }

            _context.Entry(curso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CursoExists(cursoid))
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

        // POST: api/Cursos
        [HttpPost]
        public async Task<ActionResult<Curso>> PostCurso(Curso curso)
        {
            _context.Curso.Add(curso);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCurso", new { cursoid = curso.CursoId }, curso);
        }

        // DELETE: api/Cursos/5
        [HttpDelete("{cursoid}")]
        public async Task<IActionResult> DeleteCurso(int cursoid)
        {
            var curso = await _context.Curso.FindAsync(cursoid);
            if (curso == null)
            {
                return NotFound();
            }

            _context.Curso.Remove(curso);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CursoExists(int cursoid)
        {
            return _context.Curso.Any(e => e.CursoId == cursoid);
        }
    }
}
