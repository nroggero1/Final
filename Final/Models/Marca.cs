using System.ComponentModel.DataAnnotations;

namespace Final.Models
{
    public class Marca
    {
        [Key]
        public int Id { get; set; }

        public string? Nombre { get; set; }

        public DateTime FechaAlta { get; set; }

        public bool Activo { get; set; }
    }
}
