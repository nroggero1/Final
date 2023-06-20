using System.ComponentModel.DataAnnotations;

namespace Final.Models
{
    public class Venta
    {
        [Key]
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int IdUsuario { get; set; }
        public int IdCliente { get; set; }
        public decimal Importe { get; set; }
    }
}
