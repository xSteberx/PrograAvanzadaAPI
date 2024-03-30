using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProyectoPrograAvanzadaAPI.Entities;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace ProyectoPrograAvanzadaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController(IConfiguration _configuration) : ControllerBase
    {

        [Authorize]
        [HttpGet]
        [Route("ConsultarProducto")]
        public IActionResult ConsultarProducto(bool MostrarTodos)
        {
            try
            {
				using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
				{
					ProductoRespuesta respuesta = new ProductoRespuesta();

					var resultado = db.Query<Producto>("ConsultarProducto",
						new { MostrarTodos },
						commandType: CommandType.StoredProcedure).ToList();

					if (resultado == null)
					{
						respuesta.Codigo = "-1";
						respuesta.Mensaje = "No hay productos registrados";
					}
					else
					{
						respuesta.Datos = resultado;
					}

					return Ok(respuesta);
				}
			}catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
        }




    }
}