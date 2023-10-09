using ApiNotificaciones.Interfaces;
using ApiNotificaciones.Model;

namespace ApiNotificaciones.Services
{
    public class RequestSender : IRequestSender
    {
        public Response Send(HttpRequestMessage config)
        {

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            var client = new HttpClient(clientHandler);
            try
            {
                var response = client.SendAsync(config);
                var result = response.Result.Content.ReadAsStringAsync().Result;
                if (response.Result.IsSuccessStatusCode)
                {

                    return new Response
                    {
                        StatusCode = StatusCodes.Status200OK,
                        Result = result

                    };
                }

                return new Response
                {
                    StatusCode = (int)response.Result.StatusCode,
                    Result = result

                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetType().ToString() + " | " + e.Message);
                return new Response
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Result = "Servicio de proveedor de identidad no disponible"

                };
            }

        }

        public async Task<Response> SendAsync(HttpRequestMessage config)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            var client = new HttpClient(clientHandler);
            try
            {
                var response = await client.SendAsync(config);
                var result = response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode)
                {


                    return new Response
                    {
                        StatusCode = StatusCodes.Status200OK,
                        Result = result

                    };
                }

                return new Response
                {
                    StatusCode = (int)response.StatusCode,
                    Result = result

                };
            }
            catch (Exception e)
            {
                return new Response
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Result = "Servicio de proveedor de identidad no disponible"

                };
            }
        }
    }
}