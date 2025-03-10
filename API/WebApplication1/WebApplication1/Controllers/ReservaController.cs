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

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservaController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ReservaController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
            SELECT id, id_perfil, fecha_entrada, fecha_salida, cantidad_personas, estado, id_hotel
            FROM dbo.Reservas";

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
        public JsonResult Post(Reserva res)
        {
            string query = @"
        INSERT INTO dbo.Reservas (id_perfil, fecha_entrada, fecha_salida, cantidad_personas, estado, hotel) 
        VALUES (@IdPerfil, @FechaEntrada, @FechaSalida, @CantidadPersonas, @Estado, @Hotel)";

            using (SqlConnection myCon = new SqlConnection(_configuration.GetConnectionString("HotelesAppCon")))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@IdPerfil", res.IdPerfil);
                    myCommand.Parameters.AddWithValue("@FechaEntrada", res.FechaEntrada);
                    myCommand.Parameters.AddWithValue("@FechaSalida", res.FechaSalida);
                    myCommand.Parameters.AddWithValue("@CantidadPersonas", res.CantidadPersonas);
                    myCommand.Parameters.AddWithValue("@Estado", res.Estado);
                    myCommand.Parameters.AddWithValue("@Hotel", res.Hotel);

                    myCommand.ExecuteNonQuery();
                }
            }

            return new JsonResult("Agregado con éxito");
        }




        [HttpPut]
        public JsonResult Put(Reserva res)
        {
            string query = @"
            UPDATE dbo.Reservas 
            SET id_perfil = @IdPerfil, 
                id_hotel = @Hotel,
                fecha_entrada = @FechaEntrada, 
                fecha_salida = @FechaSalida, 
                cantidad_personas = @CantidadPersonas, 
                estado = @Estado
            WHERE id = @Id";

            using (SqlConnection myCon = new SqlConnection(_configuration.GetConnectionString("HotelesAppCon")))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Id", res.Id);
                    myCommand.Parameters.AddWithValue("@IdPerfil", res.IdPerfil);
                    myCommand.Parameters.AddWithValue("@Hotel", res.Hotel);
                    myCommand.Parameters.AddWithValue("@FechaEntrada", res.FechaEntrada);
                    myCommand.Parameters.AddWithValue("@FechaSalida", res.FechaSalida);
                    myCommand.Parameters.AddWithValue("@CantidadPersonas", res.CantidadPersonas);
                    myCommand.Parameters.AddWithValue("@Estado", res.Estado);

                    myCommand.ExecuteNonQuery();
                }
            }

            return new JsonResult("Actualizado con exito");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = "DELETE FROM dbo.Reservas WHERE id = @Id";

            using (SqlConnection myCon = new SqlConnection(_configuration.GetConnectionString("HotelesAppCon")))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Id", id);
                    myCommand.ExecuteNonQuery();
                }
            }

            return new JsonResult("Eliminado con exito");
        }
        [HttpGet("usuario/{idPerfil}")]
        public JsonResult GetByUsuario(int idPerfil)
        {
            string query = @"
        SELECT id, id_perfil, fecha_entrada, fecha_salida, cantidad_personas, estado, hotel
        FROM dbo.Reservas
        WHERE id_perfil = @IdPerfil";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("HotelesAppCon");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@IdPerfil", idPerfil);
                    using (SqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        table.Load(myReader);
                    }
                }
            }
            return new JsonResult(table);
        }
    }
}
