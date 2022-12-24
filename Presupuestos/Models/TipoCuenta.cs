using Microsoft.AspNetCore.Mvc;
using Presupuestos.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace Presupuestos.Models
{
    public class TipoCuenta: IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es Requerido")]
        [PrimeraLetraMayuscula]
        [Remote(action: "VerificarExisteTipoCuenta", controller:"TiposCuentas")]

        public string Nombre { get; set; }        
        public int UsuarioId { get; set; }
        public int Orden { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Nombre != null && Nombre.Length > 0)
            {
                var primeraletra = Nombre[0].ToString();

                if (primeraletra != primeraletra.ToUpper())
                {
                    yield return new ValidationResult("La primera letra debe ser Mayuscula", new[] { nameof(Nombre) });
                }
            }
        }
    }
}
