using Microsoft.AspNetCore.Mvc.Rendering;

namespace Presupuestos.Models
{
	//Hereda de transaccion
	public class TransaccionCreacionViewModel: Transaccion
	{
		//Mostrara al usuario sus cuentas y categorias
		public IEnumerable<SelectListItem> Cuentas { get; set; }
		public IEnumerable<SelectListItem> Categorias { get; set; }

		//Crear un controlador de transacciones y Configurar en el sistema de inyeccion de dependencias
		//Nuestro nuevo repositorio IRepositorioTransacciones
	}
}

