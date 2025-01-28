using CapaEntidad.Paypal;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CNPaypal
    {
        private static string urlPaypal = ConfigurationManager.AppSettings["UrlPaypal"];
        private static string clientID = ConfigurationManager.AppSettings["ClientID"];
        private static string secret = ConfigurationManager.AppSettings["Secret"];

        //metodo para solicitud de cobro en PAypal
        public async Task<Response_Paypal<Response_checkout>> CrearSolicitud(checkout_order orden)
        {
            Response_Paypal<Response_checkout> response_paypal = new Response_Paypal<Response_checkout>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlPaypal);
                var authToken = Encoding.ASCII.GetBytes($"{clientID}:{secret}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));

                var json = JsonConvert.SerializeObject(orden);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("/v2/checkout/orders", data);

                response_paypal.Status = response.IsSuccessStatusCode;

                //si la Api se ejecuta correctamente
                if (response.IsSuccessStatusCode)
                {
                    string jsonRespuesta = response.Content.ReadAsStringAsync().Result;

                    Response_checkout checkout = JsonConvert.DeserializeObject<Response_checkout>(jsonRespuesta);
                    response_paypal.Response = checkout;

                }
                return response_paypal;
            }
        }

        public async Task<Response_Paypal<Response_capture>> AprobarPago(string token)
        {
            Response_Paypal<Response_capture> response_paypal = new Response_Paypal<Response_capture>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlPaypal);
                var authToken = Encoding.ASCII.GetBytes($"{clientID}:{secret}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));


                var data = new StringContent("{}", Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync($"/v2/checkout/orders/{token}/capture", data);

                response_paypal.Status = response.IsSuccessStatusCode;

                //si la Api se ejecuta correctamente
                if (response.IsSuccessStatusCode)
                {
                    string jsonRespuesta = response.Content.ReadAsStringAsync().Result;

                    Response_capture capture = JsonConvert.DeserializeObject<Response_capture>(jsonRespuesta);
                    response_paypal.Response = capture;

                }
                return response_paypal;
            }
        }
    }
}
