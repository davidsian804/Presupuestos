using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Presupuestos.Models
{
	public class Transaccion
	{
        public int Id { get; set; }
        public int UsuarioId { get; set; }

        [DisplayName("Fecha Transacción")]
        [DataType(DataType.DateTime)]
        //Mostrar la fecha actual con la hora y minutos        
        public DateTime FechaTransaccion { get; set; } = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd hh:MM tt"));
        public decimal Monto { get; set; }

        [Range(1, maximum: int.MaxValue, ErrorMessage ="Debe seleccionar una Categoría")]
        public int CategoriaId { get; set; }

        [StringLength(maximumLength:1000, ErrorMessage ="La {0} no puede pasar de {1} caracteres")]
        public string Nota{ get; set; }

		[Range(1, maximum: int.MaxValue, ErrorMessage = "Debe seleccionar una Cuenta")]
		public int CuentaId { get; set; }
    }
}
