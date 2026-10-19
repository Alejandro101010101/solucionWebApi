using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebAPISolution.Models
{
    public class Curso
    {
        [Key]
        public int CursoId { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [StringLength(50)]
        public string Nombre { get; set; }

        // Un curso puede tener muchas asignaciones (1 → N)
        [JsonIgnore]
        public ICollection<Asignacion> Asignaciones { get; set; } = new List<Asignacion>();
    }
}
