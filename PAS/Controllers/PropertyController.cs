using Microsoft.AspNetCore.Mvc;
using Npgsql;
using PAS.Models;
using System.Data;
using Dapper;

namespace PAS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public static String _connectionString { get; set; }

        public PropertyController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("defaultConnection");
        }

        public static IDbConnection OpenConnection(string connStr)
        {
            var res = new NpgsqlConnection(connStr);
            res.Open();
            return res;
        }

        private static IConfiguration RetrieveConfig()
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }

        /// <summary>
        ///  Este ejemplo utiliza sintaxis Dapper para acceso a los datos
        ///  Dado que una propiedad esté desactivada puede optarse por no mostrarse desde una lista de 
        ///  selección, en este caso no se muestra por no tener el campo status = 'Active'
        ///  O Bien pueden mostrarse todas las propiedades y manejarse la validación del estado de la 
        ///  propiedad posterior a la selección
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Property> Get()
        {
            string query = @"
                select id, title, address, description, created_at, updated_at, disabled_at, status
	            from property
                where status = 'Active';
            ";

            IList<Property> list;
            using (var conn = OpenConnection(_connectionString))
            {
                list = conn.Query<Property>(query).ToList();
            }

            return list;
        }

    }
}
