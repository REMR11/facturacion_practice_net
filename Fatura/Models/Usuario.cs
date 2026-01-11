using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fatura.Models
{
    public partial class Usuario 
    {

        public int Idusurio { get; set; }
        public string? NombreUsuario { get; set; }
        public int? Contraseña { get; set; }
    }
}
