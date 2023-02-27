using Microsoft.AspNetCore.Mvc;
using Presupuestos.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace Presupuestos.Models
{
    public class Cuenta
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es Requerido")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }

        [Display(Name = "Tipo Cueta")]
        public int TipoCuentaId { get; set; }
        public decimal Balance { get; set; }

        [StringLength(maximumLength:500)]
        public string Descripcion { get; set; }

        public string TipoCuenta { get; set; }

    }
}
