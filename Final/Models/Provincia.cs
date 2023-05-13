using System.ComponentModel.DataAnnotations;

namespace Final.Models
{
    public class Provincia
    {
        [Key]
        public int Id { get; set; }

        public string? Nombre { get; set; }
    }
}
