using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebAPISolution.Models
{
    public class Estudiante
    {
        [Key]
        public int EstudianteId { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [StringLength(50)]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [StringLength(50)]
        public string Apellidos { get; set; }

        [StringLength(100)]
        public string? Direccion { get; set; }

        // Un estudiante puede tener muchas asignaciones (1 → N)
        [JsonIgnore]
        public ICollection<Asignacion> Asignaciones { get; set; } = new List<Asignacion>();
    }
}
