using System.ComponentModel.DataAnnotations;

namespace WEBCodigo.Models
{
    // Modelo para el login
    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
