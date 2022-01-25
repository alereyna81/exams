using Microsoft.AspNetCore.Mvc;
using Npgsql;
using PAS.Models;
using System.Data;
using Dapper;


namespace PAS.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public static String _connectionString { get; set; }

        public ActivityController(IConfiguration configuration)
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
        ///  Consultando (fecha_actual - 3 días) <= schedule <= (fecha_actual + 2 
        ///  where schedule BETWEEN CURRENT_DATE - INTERVAL '2 day' AND CURRENT_DATE + INTERVAL '14 day'
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Activity> Get()
        {
            string query = @"
                select activity.id, activity.property_id, activity.schedule, activity.title, activity.created_at, activity.updated_at, activity.status, 
                case  when activity.status='Active' and activity.schedule >= current_date then 'Pendiente a realizar'
	                when activity.status='Active' and activity.schedule < current_date then 'Atrasada'
	                when activity.status='Done' then 'Finalizada'
	                else 'Cancelada'
                end as condition,
                property.id::text || ' - ' || property.title::text || ' - ' || property.address::text as property
	            from activity
                join property on property.id = activity.property_id
                where schedule BETWEEN CURRENT_DATE - INTERVAL '2 day' AND CURRENT_DATE + INTERVAL '14 day'
                order by activity.id desc;
            ";

            IList<Activity> list;
            using (var conn = OpenConnection(_connectionString))
            {
                list = conn.Query<Activity>(query).ToList();
            }

            return list;
        }

        /// <summary>
        /// Este ejemplo utiliza Sintaxis ADO .Net en conjunto con Npgsql
        /// </summary>
        /// <param name="act"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Post(Activity act)
        {
            string query = @"
                    insert into activity(property_id, schedule, title, created_at, updated_at, status)
	                values ( @property_id, @schedule, @title, @created_at, @updated_at, @status);
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("defaultConnection");
            NpgsqlDataReader reader;
            using (NpgsqlConnection conn = new NpgsqlConnection(sqlDataSource))
            {
                conn.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@property_id", act.property_id);
                    command.Parameters.AddWithValue("@schedule", DateTime.Now);
                    command.Parameters.AddWithValue("@title", act.title);
                    command.Parameters.AddWithValue("@created_at", DateTime.Now);
                    command.Parameters.AddWithValue("@updated_at", DateTime.Now);
                    command.Parameters.AddWithValue("@status", "Active");
                    reader = command.ExecuteReader();
                    table.Load(reader);

                    reader.Close();
                    conn.Close();

                }
            }

            return new JsonResult("Actividad añadida satisfactoriamente");
        }

        [HttpPut]
        public JsonResult Put(Activity act)
        {
            string query = @"
                update activity
	            set property_id=@property_id, schedule=@schedule, title=@title, updated_at=@updated_at, status=@status
	            WHERE id = @id;
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("defaultConnection");
            NpgsqlDataReader reader;
            using (NpgsqlConnection conn = new NpgsqlConnection(sqlDataSource))
            {
                conn.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@id", act.id);
                    command.Parameters.AddWithValue("@property_id", act.property_id);
                    command.Parameters.AddWithValue("@schedule", act.schedule);
                    command.Parameters.AddWithValue("@title", act.title);
                    command.Parameters.AddWithValue("@updated_at", act.updated_at);
                    command.Parameters.AddWithValue("@status", act.status);
                    reader = command.ExecuteReader();
                    table.Load(reader);

                    reader.Close();
                    conn.Close();

                }
            }

            return new JsonResult("Actividad actualizada satisfactoriamente");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                update activity
	            set status='Cancelled'
	            WHERE id = @id; 
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("defaultConnection");
            NpgsqlDataReader reader;
            using (NpgsqlConnection conn = new NpgsqlConnection(sqlDataSource))
            {
                conn.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@id", id);
                    reader = command.ExecuteReader();
                    table.Load(reader);

                    reader.Close();
                    conn.Close();

                }
            }

            return new JsonResult("Actividad eliminada satisfactoriamente.");
        }
    }
}
