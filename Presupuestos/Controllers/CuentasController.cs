using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Presupuestos.Models;
using Presupuestos.Servicios;
using System.Reflection;

namespace Presupuestos.Controllers
{
    public class CuentasController: Controller
    {
        public IRepositoriosTiposCuentas RepositoriosTiposCuentas { get; }
        public IServicioUsuarios ServicioUsuarios { get; }
        public IRepositorioCuentas RepositorioCuentas { get; }
        public IMapper Mapper { get; }

        public CuentasController(IRepositoriosTiposCuentas repositoriosTiposCuentas, IServicioUsuarios servicioUsuarios,
            IRepositorioCuentas repositorioCuentas, IMapper mapper)
        {
            RepositoriosTiposCuentas = repositoriosTiposCuentas;
            ServicioUsuarios = servicioUsuarios;
            RepositorioCuentas = repositorioCuentas;
            Mapper = mapper;
        }

        //Accion Index
        public async Task<IActionResult> Index()
        {
            var usuarioId = ServicioUsuarios.ObtenerUsuarioId();
            var cuentasConTipoCuenta = await RepositorioCuentas.Buscar(usuarioId);

            var modelo = cuentasConTipoCuenta
                //Agrupar por tipo cuenta
                .GroupBy(x => x.TipoCuenta)
                .Select(grupo => new IndiceCuentasViewModel 
                {
                    TipoCuenta = grupo.Key,
                    Cuentas = grupo.AsEnumerable()

                }).ToList();

            return View(modelo);

        }

        [HttpGet]
        public async Task<IActionResult> Crear() 
        {
            var usuarioId = ServicioUsuarios.ObtenerUsuarioId();
            
            var modelo = new CuentaCreacionViewModel();
            modelo.TiposCuentas = await ObtenerTiposCuentas(usuarioId);

            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CuentaCreacionViewModel cuenta)
        {
            var usuarioId = ServicioUsuarios.ObtenerUsuarioId();
            var tipoCuenta = await RepositoriosTiposCuentas.ObtenerPorId(cuenta.TipoCuentaId, usuarioId);

            if (tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            if (!ModelState.IsValid)
            {
                //Obtener los tipos cuentas del usuario para volver a cargar la vista
                cuenta.TiposCuentas = await ObtenerTiposCuentas(usuarioId);
                return View(cuenta);
            }
            await RepositorioCuentas.Crear(cuenta);
            return RedirectToAction("Index");
        }

        //Accion editar
        public async Task<IActionResult> Editar(int Id) //id de la cuenta
        {
            var usuarioId = ServicioUsuarios.ObtenerUsuarioId();
            var cuenta = await RepositorioCuentas.ObtenerPorId(Id, usuarioId);

            if (cuenta is null)
            {
                return RedirectToAction("NoEncintrado", "Home");
            }

            //Modelo que espera la vista (CuentaCreacionViewModel)
            //MAPEO
            
            //var modelo = new CuentaCreacionViewModel()
            //{
            //    Id = cuenta.Id,
            //    Nombre = cuenta.Nombre,
            //    TipoCuentaId = cuenta.TipoCuentaId,
            //    Descripcion = cuenta.Descripcion,
            //    Balance = cuenta.Balance
            //};

            var modelo = Mapper.Map<CuentaCreacionViewModel>(cuenta);

            modelo.TiposCuentas = await ObtenerTiposCuentas(usuarioId);
            return View(modelo);

        }

        //posteo de datos Editar
        [HttpPost]
        public async Task<IActionResult> Editar(CuentaCreacionViewModel cuentaEditar)
        {
            var usuarioId = ServicioUsuarios.ObtenerUsuarioId();
            var cuenta = await RepositorioCuentas.ObtenerPorId(cuentaEditar.Id, usuarioId);

            if (cuenta is null) 
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            //validar tipo cuenta
            var tipoCuenta = await RepositoriosTiposCuentas.ObtenerPorId(cuentaEditar.TipoCuentaId, usuarioId);

            if (tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            //Actualizar cuenta
            await RepositorioCuentas.Actualizar(cuentaEditar);

            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<IActionResult> Borrar(int id)
        {
            var usuarioId = ServicioUsuarios.ObtenerUsuarioId();
            var cuenta = await RepositorioCuentas.ObtenerPorId(id, usuarioId);

            if (cuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(cuenta);
        }

        //posteo de datos Eliminar
        [HttpPost]
        public async Task<IActionResult> BorrarCuenta(int id)
        {
            var usuarioId = ServicioUsuarios.ObtenerUsuarioId();
            var cuenta = await RepositorioCuentas.ObtenerPorId(id, usuarioId);

            if (cuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            //Borrar cuenta
            await RepositorioCuentas.Borrar(id);

            return RedirectToAction("Index");

        }

        private async Task<IEnumerable<SelectListItem>> ObtenerTiposCuentas(int usuarioId) 
        {
            var tipoCuentas = await RepositoriosTiposCuentas.Obtener(usuarioId);
            return tipoCuentas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }

    }
}
