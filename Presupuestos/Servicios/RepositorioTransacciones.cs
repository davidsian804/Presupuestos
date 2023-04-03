using Dapper;
using Microsoft.Data.SqlClient;
using Presupuestos.Models;
using System.Data;

namespace Presupuestos.Servicios
{
	//paso 1, creacion de interfaz
	public interface IRepositorioTrasacciones
	{
		Task Crear(Transaccion transaccion);
		//Paso 4, trabajar con la vista de transaccion, creacion de CuentasCreacionviewmodel
	}

	//paso 2 Implementar la interfaz
	public class RepositorioTransacciones : IRepositorioTrasacciones
	{
		//paso 2.1 agregar la cadena de conexion
		private readonly string connectionString;

		//paso 3 generar el construcctoe e implemetar IConfiguration
		public RepositorioTransacciones(IConfiguration configuration)
		{
			//paso 3.1 Conexion 
			connectionString = configuration.GetConnectionString("DefaultConnection");

			//paso 3.2 Creacion de la entidad transaccion
						
		}

		//Metodo Crear
		//paso 1 crear metodo
		//Pase 3 hacer un pull up al metodo Crear hacia la interfaz
		public async Task Crear(Transaccion transaccion)
		{
			//paso 2 connection
			using var connection = new SqlConnection(connectionString);
			//Recibe el ID del registro Creado, Procedimiento almacenado
			//Crear procedimiento almacenado para crear una transaccion que tambien modifica el monto y balance de la cuenta
			var id = await connection.QuerySingleAsync<int>("Transacciones_Insertar",
			new
			{
				transaccion.UsuarioId,
				transaccion.FechaTransaccion,	
				transaccion.Monto,
				transaccion.CategoriaId,
				transaccion.CuentaId,
				transaccion.Nota,
				CommandType = CommandType.StoredProcedure

			});
			
			transaccion.Id = id;
		}
	}
}
