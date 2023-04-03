using Microsoft.Data.SqlClient;
using Presupuestos.Models;
using Dapper;

namespace Presupuestos.Servicios
{
    public interface IRepositorioCategorias
    {
        Task Actualizar(Categoria categoria);
        Task Borrar(int id);
        Task Crear(Categoria categoria);
        Task<IEnumerable<Categoria>> Obtener(int usuarioId);
        Task<Categoria> ObtenerPorId(int id, int usuarioId);
    }
    public class RepositorioCategorias : IRepositorioCategorias
    {
        //Conection string
        private readonly string connectionString;
        public RepositorioCategorias(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        //metodo para crear una categoria
        public async Task Crear(Categoria categoria)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"INSERT INTO Categorias 
                                    (Nombre, TipoOperacionId, UsuarioId)
                                    VALUES(@Nombre, @TipoOperacionId, @UsuarioId);
                                    SELECT SCOPE_IDENTITY();            
                                    ", categoria);

            categoria.Id = id;
        }

        //Obtener todas las categorias creadas por un usuario determinado
        public async Task<IEnumerable<Categoria>> Obtener(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Categoria>
                ("SELECT * FROM Categorias WHERE usuarioId = @usuarioId", new { usuarioId });
        }

        public async Task<Categoria> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Categoria>
                (@"Select * From Categorias Where Id = @Id And UsuarioId = @UsuarioId", new {id, usuarioId});

        }

        public async Task Actualizar(Categoria categoria)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync
                (@"Update Categorias Set Nombre = @Nombre, TipoOperacionId = @TipoOperacionId
                 Where Id = @Id", categoria);

        }

        //Borrar
        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync
                (@"Delete Categorias Where Id = @Id", new {id});
        }

    }
}
