using DotNetEnv;
using ApiNotificaciones.Model;
using ApiNotificaciones.Interfaces;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Text;

namespace ApiNotificaciones.Services
{
    public class KeycloakService : IKeycloakService
    {
        //public static string _issuer { get; set; }
        private IRequestSender _sender;
        private IConfiguration _conf;

        public KeycloakService(IConfiguration configuration, IRequestSender sender)
        {
            _conf = configuration;
            //_issuer = _conf["Issuer"];
            _sender = sender;
        }

        public Response get_token(object cliente)
        {
            var json = JsonSerializer.Serialize(cliente);
            var config = new HttpRequestMessage()
            {                
                RequestUri = new Uri($"{Env.GetString("issuer")}/IDP/realms/{Env.GetString("realm")}/get_token"),
                Method = HttpMethod.Post,
                Content = new StringContent(json,Encoding.UTF8,"application/json")
            };

            var response = _sender.Send(config);
            if (response.StatusCode == 200)
            {
                var value = JObject.Parse(response.Result.ToString()).GetValue("access_token").ToString();

                return new Response
                {
                    StatusCode = (int)StatusCodes.Status200OK,
                    Result = value 

                };
            }
            return new Response
            {
                StatusCode = response.StatusCode,
                Result = response.Result

            };
        }
        /*public Response ClientInfo(string token)
        {
            Console.WriteLine(Env.GetString("issuer"));
            var config = new HttpRequestMessage()
            {
                RequestUri = new Uri($"{Env.GetString("issuer")}/IDP/clientinfo"),
                //RequestUri = new Uri($"{_issuer}/auth/realms/{realm}/protocol/openid-connect/token"),
                Method = HttpMethod.Get,
                /*Content = new FormUrlEncodedContent(new Dictionary<string, string>(){
                    {"realm","master"},

                }),
                Headers = {
                    {"Authorization",token}
                }
            };

            var response = _sender.Send(config);

            return new Response
            {
                StatusCode = response.StatusCode,
                Result = response.Result
            };
        }
        public Response ClientInfo(string token)
        {


            //Get client_id and realm from jwttoken

            var handler = new JwtSecurityTokenHandler();
            var jwttoken = handler.ReadJwtToken(token);
            var issuer = jwttoken.Claims.First(claim => claim.Type == "iss").Value;
            var client = new Client()
            {
                client_id = jwttoken.Claims.First(claim => claim.Type == "azp").Value,
                scope = jwttoken.Claims.First(claim => claim.Type == "scope").Value,
                realm = issuer.Split("/").Last(),

            };

            var config = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_issuer}/auth/admin/realms/{client.realm}/clients"),
                Content = new FormUrlEncodedContent(new Dictionary<string, string>(){
                    {"clientId",client.client_id},
                    {"Content-Type", "application/json"}
                }),
                Headers = {
                    {"Authorization",token},
                    {"Content-Type","application/json"}
                }};

            var response = sender.Send(config);
            if (response.StatusCode == 200)
            {
                var result = Convert.ToString(response.Result);
                result = result.Substring(result.IndexOf("{"), result.IndexOf("}")) + "}";                
                var jobject = JObject.Parse(result);
                client.id = jobject.GetValue("id").ToString();
                client.name = jobject.GetValue("name").ToString();
                return new Response
                {
                    StatusCode = response.StatusCode,
                    Result = client

                };
            }

            return new Response
            {
                StatusCode = response.StatusCode,
                Result = response.Result

            };

        }

        public Response CreateUser(string token, string realm, UserIDP user)
        {
            var conf = new HttpRequestMessage();


            var json = JsonConvert.SerializeObject(user);
            conf = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_issuer}/auth/admin/realms/{realm}/users"),
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            conf.Headers.Add("Authorization", token);
            var response = sender.Send(conf);

            if (response.StatusCode == 200)
            {



                /// <summary>
                /// Obtener info del usuario
                /// </summary>
                /// <returns></returns>                
                conf = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"{_issuer}/auth/admin/realms/{realm}/users?username={user.username}"),

                };
                conf.Headers.Add("Authorization", token);
                var info = sender.Send(conf);
                if (info.StatusCode == 200)
                {
                    var fix = info.Result.ToString().Substring(1, info.Result.ToString().Length - 2);
                    var jobject = JObject.Parse(fix);

                    info.Result = jobject.ToString();

                    /// <summary>
                    /// Asignar rol
                    /// </summary>
                    /// <returns></returns>
                    var idclient = ((Client)ClientInfo(token).Result).id;
                    var iduser = jobject.GetValue("id");

                    conf = new HttpRequestMessage()
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri($"{_issuer}/auth/admin/realms/{realm}/users/{iduser}/role-mappings/clients/idclient"),


                    };
                    return info;
                }
                else
                {
                    return new Response
                    {
                        StatusCode = info.StatusCode,
                        Result = info.Result
                    };
                }
            }
            else
                return response;

        }*/
    }
}