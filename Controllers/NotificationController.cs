using ApiNotificaciones.Interfaces;
using ApiNotificaciones.Model;
using Microsoft.AspNetCore.Mvc;
using DotNetEnv;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiNotificaciones.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly ILogger<NotificationController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IKeycloakService _keycloakService;
        private Response response;
        private Cliente cliente;
        private IConsumer consumer;
        private IBancoRepository _bancorepositorio;

        public NotificationController(ILogger<NotificationController> logger, IConfiguration configuration, IKeycloakService keycloakService, IConsumer consumer, IBancoRepository bancoRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _keycloakService = keycloakService;
            response = new Response();
            this.cliente = new Cliente();
            this.consumer = consumer;
            _bancorepositorio = bancoRepository;
        }

        // POST api/<NotificationController>
        [HttpPost("notificacion")]
        public async Task<ActionResult> PostNotificacionAsync(Notificacion notificacion)
        {
            var token = "";
            try
            {
                if (notificacion != null || ModelState.IsValid != false)
                {
                    Console.WriteLine("antes de kc");
                    response = _keycloakService.get_token(cliente);
                    Console.WriteLine("despues de kc");
                    if (response.StatusCode == 200)
                    {
                        //var access_token = JObject.Parse(response.Result.ToString());
                        token = response.Result.ToString();
                        //response = consumer.GetApiDataAsync(Env.GetString("apicore"), token, notificacion).GetAwaiter().GetResult();

                        Task.Run(() =>
                        {
                            response = consumer.GetApiDataAsync(Env.GetString("apicore"), token, notificacion).Result;
                        });
                    }
                    if (response.StatusCode != 200)
                    {
                        if (response.StatusCode == 401)
                        {
                            //error con idp token
                            Console.WriteLine(response.Result);
                            _bancorepositorio.InsertarNotificacion(notificacion, 9, response.Result.ToString());
                            response.Result = new ResponseModel(notificacion.id, 9);
                        }
                        else
                        {
                            //500 y rodos los demas errores
                            Console.WriteLine(response.Result);
                            _bancorepositorio.InsertarNotificacion(notificacion, 8, response.Result.ToString());
                            response.Result = new ResponseModel(notificacion.id, 8);
                        }
                    }
                    else
                    {
                        _bancorepositorio.InsertarNotificacion(notificacion, 1, null);
                        response.Result = new ResponseModel(notificacion.id, 1);
                    }
                }

                else
                {
                    response.StatusCode = 401;
                    response.Result = "Empty Request";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = StatusCodes.Status500InternalServerError;
                response.Result = ex.Message;
            }
            return StatusCode(response.StatusCode, response.Result);
        }
    }
}