using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPISolution.Models;
using WebAPISolution.Contexts;

namespace WebAPISolution.Controllers
{
    [Route("api/[controller]")]
    //    api/asignaciones
    [ApiController]
    public class AsignacionesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AsignacionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Asignaciones  (incluye los datos del estudiante, curso y docente)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Asignacion>>> GetAsignacion()
        {
            return await _context.Asignacion
                .Include(a => a.Estudiante)
                .Include(a => a.Curso)
                .Include(a => a.Docente)
                .ToListAsync();
        }

        // GET: api/Asignaciones/5
        [HttpGet("{asignacionid}")]
        public async Task<ActionResult<Asignacion>> GetAsignacion(int asignacionid)
        {
            var asignacion = await _context.Asignacion
                .Include(a => a.Estudiante)
                .Include(a => a.Curso)
                .Include(a => a.Docente)
                .FirstOrDefaultAsync(a => a.AsignacionId == asignacionid);

            if (asignacion == null)
            {
                return NotFound();
            }

            return asignacion;
        }

        // PUT: api/Asignaciones/5
        [HttpPut("{asignacionid}")]
        public async Task<IActionResult> PutAsignacion(int asignacionid, Asignacion asignacion)
        {
            if (asignacionid != asignacion.AsignacionId)
            {
                return BadRequest();
            }

            var error = await ValidarLlavesForaneas(asignacion);
            if (error != null)
            {
                return BadRequest(error);
            }

            _context.Entry(asignacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AsignacionExists(asignacionid))
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

        // POST: api/Asignaciones
        [HttpPost]
        public async Task<ActionResult<Asignacion>> PostAsignacion(Asignacion asignacion)
        {
            var error = await ValidarLlavesForaneas(asignacion);
            if (error != null)
            {
                return BadRequest(error);
            }

            _context.Asignacion.Add(asignacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAsignacion", new { asignacionid = asignacion.AsignacionId }, asignacion);
        }

        // DELETE: api/Asignaciones/5
        [HttpDelete("{asignacionid}")]
        public async Task<IActionResult> DeleteAsignacion(int asignacionid)
        {
            var asignacion = await _context.Asignacion.FindAsync(asignacionid);
            if (asignacion == null)
            {
                return NotFound();
            }

            _context.Asignacion.Remove(asignacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AsignacionExists(int asignacionid)
        {
            return _context.Asignacion.Any(e => e.AsignacionId == asignacionid);
        }

        // Verifica que el estudiante, el curso y el docente existan antes de guardar,
        // para devolver un 400 claro en lugar de un error 500 de llave foránea.
        private async Task<string?> ValidarLlavesForaneas(Asignacion asignacion)
        {
            if (!await _context.Estudiante.AnyAsync(e => e.EstudianteId == asignacion.EstudianteId))
                return $"No existe el estudiante con id {asignacion.EstudianteId}";

            if (!await _context.Curso.AnyAsync(c => c.CursoId == asignacion.CursoId))
                return $"No existe el curso con id {asignacion.CursoId}";

            if (!await _context.Docente.AnyAsync(d => d.DocenteId == asignacion.DocenteId))
                return $"No existe el docente con id {asignacion.DocenteId}";

            return null;
        }
    }
}
