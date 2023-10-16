using System.ComponentModel.DataAnnotations;
using DotNetEnv;
using Newtonsoft.Json;


namespace ApiNotificaciones.Model
{
    public class Notificacion
    {

        private string Id;

        public Notificacion()
        {
            Id = GenerarStringAleatorio();
        }

        //[JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string? id
        {
            get { return ObtenerValorId(); }
            //set { id = GenerarStringAleatorio(); }
        }

        private string ObtenerValorId()
        {
            return Id;
        }

        [Required(ErrorMessage = "El tipo de mensaje a enviar es obligatorio")]
        [RegularExpression("^(sms|todus|telegram)$", ErrorMessage = "El campo {0} solo puede contener las palabras 'sms', 'todus' o 'telegram'.")]
        public string tipo { get; set; } // sms, todus, etc.

        [Required(ErrorMessage = "El número de teléfono para el envío del mensaje es obligatorio")]
        [RegularExpression("^53[0-9]{8}$", ErrorMessage = "El campo {0} debe comenzar con '53' seguido de 8 dígitos numéricos.")]
        public string telefono { get; set; }

        [Required(ErrorMessage = "El texto del mensaje a enviar es obligatorio")]
        [MaxLength(160, ErrorMessage = "El texto ha excedido los 160 caracteres")]
        public string mstxt { get; set; }

        public string GenerarStringAleatorio()
        {
            Random random = new Random();

            // Generar un número aleatorio de 30 dígitos
            string numeroAleatorio = "";
            for (int i = 0; i < 30; i++)
            {
                int digito = random.Next(0, 10);
                numeroAleatorio += digito.ToString();
            }

            // Concatenar el parámetro y el número aleatorio
            string resultado = Env.GetString("idbanco") + numeroAleatorio;

            return resultado;

            /*Guid random = Guid.NewGuid();
            string numeroAleatorio = random.ToString("N");
            string alet = numeroAleatorio.Substring(2);

            // Concatenar el parámetro y el número aleatorio
            string resultado = Env.GetString("idbanco") + alet;
            return resultado;*/
        }
    }
}
