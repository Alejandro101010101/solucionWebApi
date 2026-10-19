using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPISolution.Models;
using WebAPISolution.Contexts;

namespace WebAPISolution.Controllers
{
    [Route("api/[controller]")]
    //    api/estudiantes
    [ApiController]
    public class EstudiantesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EstudiantesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Estudiantes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estudiante>>> GetEstudiante()
        {
            return await _context.Estudiante.ToListAsync();
        }

        // GET: api/Estudiantes/5
        [HttpGet("{estudianteid}")]
        public async Task<ActionResult<Estudiante>> GetEstudiante(int estudianteid)
        {
            var estudiante = await _context.Estudiante.FindAsync(estudianteid);

            if (estudiante == null)
            {
                return NotFound();
            }

            return estudiante;
        }

        // PUT: api/Estudiantes/5
        [HttpPut("{estudianteid}")]
        public async Task<IActionResult> PutEstudiante(int estudianteid, Estudiante estudiante)
        {
            if (estudianteid != estudiante.EstudianteId)
            {
                return BadRequest();
            }

            _context.Entry(estudiante).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstudianteExists(estudianteid))
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

        // POST: api/Estudiantes
        [HttpPost]
        public async Task<ActionResult<Estudiante>> PostEstudiante(Estudiante estudiante)
        {
            _context.Estudiante.Add(estudiante);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEstudiante", new { estudianteid = estudiante.EstudianteId }, estudiante);
        }

        // DELETE: api/Estudiantes/5
        [HttpDelete("{estudianteid}")]
        public async Task<IActionResult> DeleteEstudiante(int estudianteid)
        {
            var estudiante = await _context.Estudiante.FindAsync(estudianteid);
            if (estudiante == null)
            {
                return NotFound();
            }

            _context.Estudiante.Remove(estudiante);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EstudianteExists(int estudianteid)
        {
            return _context.Estudiante.Any(e => e.EstudianteId == estudianteid);
        }
    }
}
