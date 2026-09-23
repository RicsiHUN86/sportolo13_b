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
        [HttpPost]
        public Eredmeny AddNewEredmeny(AddEredmenyDTO eredmeny)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            var newE = new Eredmeny
            {
                Competition = eredmeny.Competition,
                Description = eredmeny.Description,
                ResultTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                SportoloId = eredmeny.SportoloId
            };

            var sql = "INSERT INTO `eredmeny`(`Competition`, `Description`, `ResultTime`, `UpdateTime`, `SportoloId`) VALUES (@competition,@description,@resulttime,@updatetime,@sportoloid)";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@competition", newE.Competition);
            cmd.Parameters.AddWithValue("@description", newE.Description);
            cmd.Parameters.AddWithValue("@resulttime", newE.ResultTime);
            cmd.Parameters.AddWithValue("@updatetime", newE.UpdateTime);
            cmd.Parameters.AddWithValue("@sportoloid", newE.SportoloId);
            cmd.ExecuteNonQuery();
            connector.Close();
            return newE;
        }
        [HttpPut]
        public Eredmeny Update([FromQuery] int id, [FromBody] UpdateEredmenyDTO eredmeny)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            var updatedE = new Eredmeny
            {
                Competition = eredmeny.Competition,
                Description = eredmeny.Description,
                UpdateTime = DateTime.Now,
                SportoloId = eredmeny.SportoloId
            };

            string sql = "UPDATE `eredmeny` SET `Competition`=@competition,`Description`=@description,`UpdateTime`=@updatetime,`SportoloId`=@sportoloid WHERE Id=@id;";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@competition", updatedE.Competition);
            cmd.Parameters.AddWithValue("@description", updatedE.Description);
            cmd.Parameters.AddWithValue("@updatetime", updatedE.UpdateTime);
            cmd.Parameters.AddWithValue("@sportoloid", updatedE.SportoloId);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            connector.Close();
            return updatedE;
        }
        [HttpDelete]
        public object Delete(int id)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            var sql = "DELETE FROM `eredmeny` WHERE Id=@id;";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            connector.Close();
            return new { message = "Eredmény sikeresen törölve" };
        }
        [HttpGet("byId")]
        public object GetById(int id)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            var sql = "SELECT * FROM eredmeny WHERE Id=@id;";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);

            var dataReader = cmd.ExecuteReader();
            dataReader.Read();

            var eredmeny = new Eredmeny
            {
                Id = dataReader.GetInt32(0),
                Competition = dataReader.GetString(1),
                Description = dataReader.GetString(2),
                ResultTime = dataReader.GetDateTime(3),
                UpdateTime = dataReader.GetDateTime(4),
                SportoloId = dataReader.GetInt32(5)
            };

            connector.Close();
            return eredmeny;
        }
    }
}
