using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Lab_14_s_chasrp.Models;
using System.Xml.Linq;

namespace Lab_14_s_chasrp.Controllers
{
    public class NationalityController : Controller
    {
        public IActionResult Index()
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.Development.json").Build();
            List<NationalityModel> countryes = new List<NationalityModel>();
            List<GroupModel> continents = new List<GroupModel>();
            using (var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var firstCommand = new NpgsqlCommand("select * from sport_clubs", connection);
                var firstReader = firstCommand.ExecuteReader();
                while (firstReader.Read())
                {
                    var country = new NationalityModel
                    {
                        id = firstReader.GetInt32(0),
                        
                        name = firstReader.GetString(1),
                        photo = firstReader.IsDBNull(2) ? null : firstReader.GetFieldValue<byte[]>(2),
                        GroupId = firstReader.GetString(3)


                    };
                    countryes.Add(country);
                }
                firstReader.Close();
                var secondCommand = new NpgsqlCommand("select * from sport_kinds", connection);
                var secondReader = secondCommand.ExecuteReader();
                while (secondReader.Read())
                {
                    var continent = new GroupModel
                    {
                        id = secondReader.GetInt32(0),
                        name = secondReader.GetString(1),
                        //description = secondReader.GetString(2)
                };
                    continents.Add(continent);
                }
                secondReader.Close();
                connection.Close();
            }
            Models.Models models = new Models.Models(countryes, continents);
            return View(models);
        }

        [HttpPost]
        public IActionResult Insert(IFormFile photo, string name, string l_group_id)
        {
            byte[] imageBytes;
            using (var memoryStream = new MemoryStream())
            {
                photo.CopyTo(memoryStream);
                imageBytes = memoryStream.ToArray();
            }
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.Development.json").Build();
            using (var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var command = new NpgsqlCommand("insert into sport_clubs (name, kind_name, photo) values (@name, @l_group_id, @photo)", connection);
               // command.Parameters.AddWithValue("description", description);
                command.Parameters.AddWithValue("name", name);
                command.Parameters.AddWithValue("l_group_id", l_group_id);
                command.Parameters.AddWithValue("photo", imageBytes);               
                command.ExecuteNonQuery();
                connection.Close();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Update(int id, IFormFile photo, string name, string description, string l_group_id)
        {
            byte[] imageBytes;
            using (var memoryStream = new MemoryStream())
            {
                photo.CopyTo(memoryStream);
                imageBytes = memoryStream.ToArray();
            }
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.Development.json").Build();
            using (var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var command = new NpgsqlCommand("update sport_clubs set name = @name, kind_name= @l_group_id, photo = @photo where id = @id", connection);
                //command.Parameters.AddWithValue("@description", description);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@l_group_id", l_group_id);
                command.Parameters.AddWithValue("@photo", imageBytes);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
                connection.Close();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.Development.json").Build();
            using (var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var command = new NpgsqlCommand("delete from sport_clubs where id = @id", connection);
                command.Parameters.AddWithValue("id", id);
                command.ExecuteNonQuery();
                connection.Close();
            }
            return RedirectToAction("Index");
        }
    
}
}
