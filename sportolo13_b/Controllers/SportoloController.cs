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

        [HttpPost]
        public Sportolo AddNewSportolo(AddSportoloDTO sportolo)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            var newS = new Sportolo
            {
                Name = sportolo.Name,
                Email = sportolo.Email,
                Age = sportolo.Age,
                Password = sportolo.Password,
                RegistrationTime = DateTime.Now
            };

            var sql = "INSERT INTO `sportolo`(`name`, `email`, `age`, `password`, `registrationTime`) VALUES (@name, @email, @age, @password, @registrationtime)";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@name", newS.Name);
            cmd.Parameters.AddWithValue("@email", newS.Email);
            cmd.Parameters.AddWithValue("@age", newS.Age);
            cmd.Parameters.AddWithValue("@password", newS.Password);
            cmd.Parameters.AddWithValue("@registrationtime", newS.RegistrationTime);
            cmd.ExecuteNonQuery();
            connector.Close();
            return newS;
        }
        [HttpPut]
        public Sportolo UpdateSportolo(int id, [FromBody] UpdateSportoloDTO sportolo)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            var updatedS = new Sportolo
            {
                Name = sportolo.Name,
                Email = sportolo.Email,
                Age = sportolo.Age,
                Password = sportolo.Password
            };

            string sql = "UPDATE `sportolo` SET `name`=@name, `email`=@email, `age`=@age, `password`=@password WHERE id=@id";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@name", updatedS.Name);
            cmd.Parameters.AddWithValue("@email", updatedS.Email);
            cmd.Parameters.AddWithValue("@age", updatedS.Age);
            cmd.Parameters.AddWithValue("@password", updatedS.Password);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            connector.Close();
            return updatedS;
        }
        [HttpDelete]
        public bool DeleteSportolo(int id)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            string sql = "DELETE FROM `sportolo` WHERE id=@id";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);
            var result = cmd.ExecuteNonQuery();
            connector.Close();
            return result > 0;
        }
        [HttpGet("sportoloInfoById")]
        public object GetSportoloInfo(int sportoloId)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            var sql = "SELECT name, email FROM sportolo WHERE Id=@id;";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", sportoloId);

            var dataReader = cmd.ExecuteReader();
            dataReader.Read();

            var sportoloInfo = new
            {
                Name = dataReader.GetString(0),
                Email = dataReader.GetString(1)
            };

            connector.Close();
            return sportoloInfo;
        }
        [HttpGet("OsszesEredmeny")]
        public object GetOsszesEredmeny()
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            var sql = "SELECT COUNT(*) FROM eredmeny;";
            var cmd = new MySqlCommand(sql, connector);
            var osszes = cmd.ExecuteScalar();

            connector.Close();
            return new { message = $"Összes eredmény: {osszes}" };
        }
    }
}
