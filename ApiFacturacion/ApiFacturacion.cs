using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ApiFacturacionDocDigitales
{
        public class ApiFacturacion {
        
        private const string EMPRESA_API_KEY = "6DBXudR_TgjhBaAB5RcORA";
        
        // Genera un CFDI
        public Dictionary<string, dynamic> GeneracionFactura(Dictionary<string, dynamic> peticionGeneracion) {
            string _uriGeneracion = "https://api.docdigitales.com/v1/facturas/generar";

            try {
                string response = GetPostResponse(_uriGeneracion, peticionGeneracion);
                return JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(response);
            } catch(Exception e) {
                Console.WriteLine("{0} Excepcion.", e);
                return null;
            }
        }

        public Dictionary<string, dynamic> CancelacionFactura(Dictionary<string, dynamic> peticionCancelacion) {
            string _uriCancelacion = "https://api.docdigitales.com/v1/facturas/cancelar";

            try {
                string response = GetPostResponse(_uriCancelacion, peticionCancelacion);
                return JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(response);
            } catch(Exception e) {
                Console.WriteLine("{0} Excepcion.", e);
                return null;
            }
        }

        public Dictionary<string, dynamic> GeneracionRecepcionPago(Dictionary<string, dynamic> peticionGeneracionRecepcion) {
            string _uriCancelacion = "https://api.docdigitales.com/v1/recepciones_pago/generar";

            try {
                string response = GetPostResponse(_uriCancelacion, peticionGeneracionRecepcion);
                return JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(response);
            }
            catch (Exception e) {
                Console.WriteLine("{0} Excepcion.", e);
                return null;
            }
        }

        public Dictionary<string, dynamic> EnviarFactura(Dictionary<string, dynamic> peticionEnvio) {
            string _uriEnvio = "https://api.docdigitales.com/v1/facturas/enviar";
            
            try {
                string response = GetPostResponse(_uriEnvio, peticionEnvio);
                return JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(response);
            } catch (Exception e) {
                Console.WriteLine("{0} Excepcion.", e);
                return null;
            }            
        }

        public Dictionary<string, dynamic> DescargarFactura(Dictionary<string, dynamic> peticionDescarga) {
            string _uriDescarga = "https://api.docdigitales.com/v1/facturas/descargar";
            try {
                string response = GetPostResponse(_uriDescarga, peticionDescarga);
                return JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(response);
            } catch (Exception e) {
                Console.WriteLine("{0} Excepcion.", e);
                return null;
            }
        }

        private string GetPostResponse(String uri, Dictionary<string, dynamic> data) {
            try {
                string jsonData = JsonConvert.SerializeObject(data);
                using (var client = new HttpClient()) {
                    client.DefaultRequestHeaders.Add("Authorization", $"Token token={EMPRESA_API_KEY}");
                    client.DefaultRequestHeaders.Add("ACCEPT", "application/json");
                    client.DefaultRequestHeaders.Add("CONTENT_TYPE", "application/json");
                    client.DefaultRequestHeaders.Add("Access-Control-Allow-Origin", "*");
                    client.DefaultRequestHeaders.Add("Method", "POST");
                    var response = client.PostAsync(uri, new StringContent(jsonData, System.Text.Encoding.Default, "application/json"));
                    var response_string = response.Result.Content.ReadAsStringAsync();
                    Console.WriteLine(response_string.Result);
                    return response_string.Result;
                }
            } catch(Exception e) {
                Console.WriteLine("{0} Excepcion.", e);
                return null;
            }
        }
    }
}
