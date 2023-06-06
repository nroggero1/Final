using System.ComponentModel.DataAnnotations;

namespace Final.Models
{
    public class Compra
    {
        [Key]
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int IdUsuario { get; set; }
        public int IdProveedor { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioCompra { get; set; }
    }
}
