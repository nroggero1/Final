namespace Final.Models
{
    public class Proveedor
    {
        public int Id { get; set; }
        public long? CodigoTributario { get; set; }
        public string? Direccion { get; set; }
        public int? CodigoPostal { get; set; }
        public int? IdLocalidad { get; set; }
        public string? Telefono { get; set; }
        public string? Mail { get; set; }
        public string? Denominacion { get; set; }
        public DateTime FechaAlta { get; set; }
        public bool Activo { get; set; }
    }
}

