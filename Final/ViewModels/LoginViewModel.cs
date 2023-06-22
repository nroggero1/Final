using System.ComponentModel.DataAnnotations;

namespace Final.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Usuario requerido")]
        [StringLength(50, ErrorMessage = "El usuario no puede tener mas de 50 caracteres")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "Clave requerida")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "La clave no puede tener mas de 100 caracteres")]
        public string Clave{ get; set;}
    }
}
