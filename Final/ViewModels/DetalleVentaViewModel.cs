namespace Final.ViewModels
{
    public class DetalleVentaViewModel
    {
        public int Linea { get; set; }
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
        public int IdVenta { get; internal set; }

        public decimal Total()
        {
            return Cantidad * PrecioUnitario;
        }
    }
}

