using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using sportolo13_b.Models;
using sportolo13_b.Models.DTOs;

namespace sportolo13_b.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EredmenyController : ControllerBase
    {
        private readonly string ConnectionString = "server=localhost;uid=root;password=;database=sportolo13b";

        [HttpGet]
        public List<Eredmeny> GetAll()
        {
            List<Eredmeny> eredmenyek = new();
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            string sql = "SELECT * FROM eredmeny";

            var cmd = new MySqlCommand(sql, connector);
            var dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                var eredmeny = new Eredmeny
                {
                    Id = dataReader.GetInt32(0),
                    Competition = dataReader.GetString(1),
                    Description = dataReader.GetString(2),
                    ResultTime = dataReader.GetDateTime(3),
                    UpdateTime = dataReader.GetDateTime(4),
                    SportoloId = dataReader.GetInt32(5)
                };
                eredmenyek.Add(eredmeny);
            }

            connector.Close();
            return eredmenyek;
        }
        
    }
}
