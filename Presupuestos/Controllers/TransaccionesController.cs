using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Presupuestos.Models;
using Presupuestos.Servicios;

namespace Presupuestos.Controllers
{
	public class TransaccionesController : Controller
	{
		private readonly IServicioUsuarios servicioUsuarios;
		private readonly IRepositorioCuentas repositorioCuentas;

		public IActionResult Index()
		{
			return View();
		}

		//PASO 1. inyectar IServicioUsuarios y IRepositorioCuentas y asignar campos
        public TransaccionesController(IServicioUsuarios servicioUsuarios, IRepositorioCuentas repositorioCuentas)
        {
			this.servicioUsuarios = servicioUsuarios;
			this.repositorioCuentas = repositorioCuentas;
		}

		//PASO 2. 
		public async Task<IActionResult> Crear()
		{
			var usuarioId = servicioUsuarios.ObtenerUsuarioId();
			var modelo = new TransaccionCreacionViewModel();

			modelo.Cuentas = await ObtenerCuentas(usuarioId);			
			return View(modelo);

			//PASO 4. Creacion de la vista (Transacciones => Crear)
		}

		//PASO 3. Creacion de metodo privado para obtener las cuentas del usuario
		private async Task<IEnumerable<SelectListItem>> ObtenerCuentas(int usuarioId)
		{
			var cuentas = await repositorioCuentas.Buscar(usuarioId);
			return cuentas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
			//Agreando el modelo al metodo Crear
		}
    }
}
