using ApiNotificaciones.Interfaces;
using ApiNotificaciones.Model;
using DotNetEnv;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;

namespace ApiNotificaciones.Services
{
    public class Consumer : IConsumer
    {        
        private IRequestSender _sender;

        public Consumer(IRequestSender sender)
        {
            _sender = sender;
        }

        public async Task<Response> GetApiDataAsync(string url, string token, Notificacion notificacion)
        {
            /*httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string json = JsonConvert.SerializeObject(notificacion);
            StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage responseBody = await httpClient.GetAsync(new Uri(url));

            responseBody.EnsureSuccessStatusCode();

            response.Result = await responseBody.Content.ReadAsStringAsync();
            response.StatusCode = (int)responseBody.StatusCode;

            return response;*/

            var json = JsonConvert.SerializeObject(notificacion);
            var config = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Post,
                Content = new StringContent(json, Encoding.UTF8, "application/json"),
                Headers = {
                    {"Authorization",token}
                }
            };

            var response = await _sender.SendAsync(config);
            /*if (response.StatusCode == 200)
            {

                return new Response
                {
                    StatusCode = (int)StatusCodes.Status200OK,
                    Result = new { response.Result }
                };
            }*/
            return new Response
            {
                StatusCode = response.StatusCode,
                Result = response.Result

            };
        }
    }
}