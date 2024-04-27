using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ProyectoPrograAvanzadaAPI.Entities;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using ProyectoPrograAvanzadaAPI.Interfaces;
using Microsoft.AspNetCore.Diagnostics;

namespace ProyectoPrograAvanzadaAPI.Controllers
{
    [Route("api/error")]
    [ApiExplorerSettings(IgnoreApi= true)]
    [ApiController]
    public class ErrorController(IConfiguration _configuration,IUtilitariosModel _utilitariosModel, IHttpContextAccessor _accesor) : ControllerBase
    {
        [Route("error")]
        public IActionResult Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
                using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    Respuesta respuesta = new Respuesta();
                    
                        long IdUsuario = (User.Identity!.Name! != null ? long.Parse(_utilitariosModel.Decrypt(User.Identity!.Name!)) : 0);
                        string Mensaje = context!.Error.Message;
                        string Origen = context!.Path;
                        
                        string DireccionIp = _accesor.HttpContext?.Connection.RemoteIpAddress?.ToString()!;

                    var resultado = db.Execute("RegistrarError",
                        new { IdUsuario, Mensaje, DireccionIp, Origen },
                        commandType: CommandType.StoredProcedure);


                return Problem(
                    detail: context.Error.StackTrace,
                    title: context!.Error.Message
                    );



            }
        }
           
        }

    }

