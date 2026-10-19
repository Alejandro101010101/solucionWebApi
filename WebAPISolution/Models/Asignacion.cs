using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPISolution.Models
{
    public class Asignacion
    {
        [Key]
        public int AsignacionId { get; set; }

        // Llaves foráneas
        [Required(ErrorMessage = "Este campo es requerido")]
        public int EstudianteId { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        public int CursoId { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        public int DocenteId { get; set; }

        // Propiedades de navegación (nullable para que no sean obligatorias en el JSON del POST/PUT)
        [ForeignKey(nameof(EstudianteId))]
        public Estudiante? Estudiante { get; set; }

        [ForeignKey(nameof(CursoId))]
        public Curso? Curso { get; set; }

        [ForeignKey(nameof(DocenteId))]
        public Docente? Docente { get; set; }
    }
}
