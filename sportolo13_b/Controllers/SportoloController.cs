using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using sportolo13_b.Models;
using sportolo13_b.Models.DTOs;

namespace sportolo13_b.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SportoloController : ControllerBase
    {
        private readonly string ConnectionString = "server=localhost;uid=root;password=;database=sportolo13b";

        [HttpGet]
        public List<Sportolo> GetAll()
        {
            List<Sportolo> sportolok = new();
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            string sql = "SELECT * FROM sportolo";

            var cmd = new MySqlCommand(sql, connector);
            var dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                var sportolo = new Sportolo
                {
                    Id = dataReader.GetInt32(0),
                    Name = dataReader.GetString(1),
                    Email = dataReader.GetString(2),
                    Age = dataReader.GetInt32(3),
                    Password = dataReader.GetString(4),
                    RegistrationTime = dataReader.GetDateTime(5)
                };
                sportolok.Add(sportolo);
            }

            connector.Close();
            return sportolok;
        }

        
    }
}
