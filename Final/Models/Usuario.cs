using System.ComponentModel.DataAnnotations;

namespace Final.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        public string? NombreUsuario { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Correo { get; set; }
        public DateTime? FechaNacimiento { get; set;}
        public string? Clave { get; set; }
        public bool Administrador { get; set; }
        public DateTime? FechaAlta { get; set; }
        public bool Activo { get; set; }
    }
}
