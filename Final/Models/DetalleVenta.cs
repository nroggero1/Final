using System.ComponentModel.DataAnnotations;

namespace Final.Models
{
    public class DetalleVenta
    {
        [Key]
        public int Id { get; set; }
        public int IdVenta { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
