namespace Final.Models
{
    public class Localidad
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public int IdProvincia { get; set; }
        public short CodigoPostal { get; set; }
    }
}
