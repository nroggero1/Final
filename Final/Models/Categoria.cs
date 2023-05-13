using System.ComponentModel.DataAnnotations;

namespace Final.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        public string? Nombre { get; set; }
    }
}
