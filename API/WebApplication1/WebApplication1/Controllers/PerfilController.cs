using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WebApplication1.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerfilController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public PerfilController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            SELECT id, nombre, correo, telefono, foto, contraseña
                            FROM dbo.Perfil";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("HotelesAppCon");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    using (SqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        table.Load(myReader);
                    }
                }
            }

            return new JsonResult(table);
        }
        [HttpPost]
        public JsonResult Post(Perfil perfil)
        {
            string query = @"
    INSERT INTO dbo.Perfil (nombre, correo, telefono, foto, contraseña)
    VALUES (@nombre, @correo, @telefono, @foto, @contraseña)";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("HotelesAppCon");

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@nombre", perfil.Nombre);
                    myCommand.Parameters.AddWithValue("@correo", perfil.Correo);
                    myCommand.Parameters.AddWithValue("@telefono", perfil.Telefono);
                    myCommand.Parameters.AddWithValue("@contraseña", perfil.Contraseña);

                    // Si foto es null o está vacía, asignamos DBNull.Value
                    if (string.IsNullOrEmpty(perfil.Foto))
                    {
                        myCommand.Parameters.AddWithValue("@foto", DBNull.Value);
                    }
                    else
                    {
                        myCommand.Parameters.AddWithValue("@foto", perfil.Foto);
                    }

                    using (SqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        table.Load(myReader);
                    }
                }
            }

            return new JsonResult("Se agregó con éxito");
        }

        [HttpPut]
        public JsonResult Put(Perfil perfil)
        {
            string query = @"
        UPDATE dbo.Perfil
        SET Nombre = @Nombre,
            Correo = @Correo,
            Telefono = @Telefono,
            Foto = @Foto
        WHERE Id = @Id";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("HotelesAppCon");

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Id", perfil.Id);
                    myCommand.Parameters.AddWithValue("@Nombre", perfil.Nombre);
                    myCommand.Parameters.AddWithValue("@Correo", perfil.Correo);
                    myCommand.Parameters.AddWithValue("@Telefono", perfil.Telefono);
                    myCommand.Parameters.AddWithValue("@Foto", perfil.Foto);

                    using (SqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        table.Load(myReader);
                    }
                }
            }

            return new JsonResult("Actualizo con exito");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
        DELETE FROM dbo.Perfil
        WHERE Id = @Id";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("HotelesAppCon");

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        table.Load(myReader);
                    }
                }
            }

            return new JsonResult("Se elimibo con exito");
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                if (httpRequest.Files.Count == 0)
                {
                    return new JsonResult("anonymous.png");
                }

                var postedFile = httpRequest.Files[0];

                // Ruta de la carpeta donde se guardarán las imágenes
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Photos");

                // Verificar si la carpeta existe, si no, crearla
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Crear un nombre único para la imagen
                string filename = Guid.NewGuid().ToString() + Path.GetExtension(postedFile.FileName);
                var filePath = Path.Combine(folderPath, filename);

                // Guardar la imagen en la carpeta
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(filename);
            }
            catch (Exception)
            {
                return new JsonResult("anonymous.png");
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Perfil perfil)
        {
            string query = @"
        SELECT Id, Nombre, Correo FROM dbo.Perfil 
        WHERE Correo = @Correo AND Contraseña = @Contraseña";

            using (SqlConnection myCon = new SqlConnection(_configuration.GetConnectionString("HotelesAppCon")))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Correo", perfil.Correo);
                    myCommand.Parameters.AddWithValue("@Contraseña", perfil.Contraseña);

                    using (SqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        if (myReader.Read()) // Si hay resultado, el usuario existe
                        {
                            return Ok(new
                            {
                                id = Convert.ToInt32(myReader["Id"]),
                                nombre = myReader["Nombre"].ToString(),
                                correo = myReader["Correo"].ToString()
                            });
                        }
                        else
                        {
                            return Unauthorized(new { mensaje = "Correo o contraseña incorrectos" });
                        }
                    }
                }
            }
        }

    }



}
