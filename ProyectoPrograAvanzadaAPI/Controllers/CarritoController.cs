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
    public class CarritoController(IConfiguration _configuration) : ControllerBase
    {

       
        [HttpGet]
        [Route("ConsultarCarrito")]
        public IActionResult ConsultarCarrito(long IdUsuario)
        {
            try {

				using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
				{
					CarritoRespuesta respuesta = new CarritoRespuesta();

					var resultado = db.Query<Carrito>("ConsultarCarrito",
						new { IdUsuario },
						commandType: CommandType.StoredProcedure).ToList();

					if (resultado == null)
					{
						respuesta.Codigo = "-1";
						respuesta.Mensaje = "No hay productos en el carrito";
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


        [HttpPost]
		[Route("AgregarCarrito")]
		public IActionResult AgregarCarrito(Carrito entidad)
		{
			try
			{
				using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
				{
					CarritoRespuesta respuesta = new CarritoRespuesta();

					var resultado = db.Execute("AgregarCarrito",
						new { entidad.IdUsuario, entidad.IdProducto, entidad.Cantidad, entidad.Fecha },
						commandType: CommandType.StoredProcedure);

					if (resultado <= 0)
					{
						respuesta.Codigo = "-1";
						respuesta.Mensaje = "Error al agrgar al carrito";
					}

					return Ok(respuesta);

				}
			}catch(Exception ex) {
				return BadRequest(ex.Message);
			}
		}



	}
}
