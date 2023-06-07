using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final.Models
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public string? CodigoBarras { get; set; }
        public decimal PrecioCompra { get; set; }
        public int PorcentajeGanancia { get; set; }
        public decimal PrecioVentaSugerido { get; set; }
        public decimal PrecioVenta { get; set; }
        public int Stock { get; set; }
        public int StockMinimo { get; set; }
        public bool Activo { get; set; }
        public DateTime? FechaAlta { get; set; }
        public int IdMarca { get; set; }
        public int IdCategoria { get; set; }
    }
}
