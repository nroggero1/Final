namespace Final.Models
{
    public class DetalleVenta
    {
        public int Id { get; set; }
        public int IdVenta { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioVenta { get; set; }

        public DetalleVenta(int idProducto, int cantidad, decimal precioVenta)
        {
            IdProducto = idProducto;
            Cantidad = cantidad;
            PrecioVenta = precioVenta;
        }
    }
}
