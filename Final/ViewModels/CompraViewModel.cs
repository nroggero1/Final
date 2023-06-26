using Final.Models;

namespace Final.ViewModels
{
    public class CompraViewModel
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int IdProveedor { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioCompra { get; set; }
        public int IdMarca { get; set; }
        public int IdCategoria { get; set; }
        public decimal Importe { get; set; }
        public List<DetalleCompraViewModel>? DetallesCompra { get; set; }
    }
}