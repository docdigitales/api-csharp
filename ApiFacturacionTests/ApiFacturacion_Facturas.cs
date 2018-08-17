using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Xunit;
using Newtonsoft.Json;
using ApiFacturacionDocDigitales;

namespace ApiFacturacionTests
{
    public class ApiFacturacion_Facturas {
        private readonly ApiFacturacion _factura;
        private readonly CertificadosDocDigitales _lib;
        private const string PATH_CERTIFICADO = @"/home/vic/workspace/tutorials/csharp/vendor/certificados/certificado.cer";
        private const string PATH_LLAVE = @"/home/vic/workspace/tutorials/csharp/vendor/certificados/llave.key";
        private const string PASSWORD_LLAVE = "DDM090629R13";
        private readonly string _contenidoCertificado, _contenidoLlave;

        public ApiFacturacion_Facturas() {
            _lib     = new CertificadosDocDigitales();
            _factura = new ApiFacturacion();
            _contenidoCertificado = _lib.ExportarCertificadoPem(PATH_CERTIFICADO);
            _contenidoLlave       = _lib.ExportarLlavePem(PATH_LLAVE, PASSWORD_LLAVE);
        }        

        [Fact]
        public void GeneraExitosamenteFactura() {
            string factura = "{\"meta\":{\"empresa_uid\":\"6e6d79f30d\",\"empresa_api_key\":\"JeqaaLyiRGykgYyQoX_7RQ\",\"ambiente\":\"S\",\"objeto\":\"factura\"},\"data\":[{\"datos_fiscales\":{\"certificado_pem\":\"\",\"llave_pem\":\"\",\"llave_password\":\"DDM090629R13\"},\"cfdi\":{\"cfdi__comprobante\":{\"folio\":\"123\",\"fecha\":\"2018-03-25T12:12:12\",\"tipo_comprobante\":\"I\",\"lugar_expedicion\":\"21100\",\"forma_pago\":\"01\",\"metodo_pago\":\"PUE\",\"moneda\":\"MXN\",\"tipo_cambio\":\"1\",\"subtotal\":\"99.00\",\"total\":\"99.00\",\"cfdi__emisor\":{\"rfc\":\"DDM090629R13\",\"nombre\":\"Emisor Test\",\"regimen_fiscal\":\"601\"},\"cfdi__receptor\":{\"rfc\":\"XEXX010101000\",\"nombre\":\"Receptor Test\",\"uso_cfdi\":\"G01\"},\"cfdi__conceptos\":{\"cfdi__concepto\":[{\"clave_producto_servicio\":\"01010101\",\"clave_unidad\":\"KGM\",\"cantidad\":\"1\",\"descripcion\":\"descripcion test\",\"valor_unitario\":\"99.00\",\"importe\":\"99.00\",\"unidad\":\"unidad\",\"no_identificacion\":\"KGM123\",\"cfdi__impuestos\":{\"cfdi__traslados\":{\"cfdi__traslado\":[{\"base\":\"99.00\",\"impuesto\":\"002\",\"tipo_factor\":\"Exento\"}]}}}]}}}}]}";
            var facturaDiccionario = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(factura);

            // Establecer parametros para generacion
            facturaDiccionario["data"][0]["datos_fiscales"]["certificado_pem"]  = _contenidoCertificado;
            facturaDiccionario["data"][0]["datos_fiscales"]["llave_pem"]        = _contenidoLlave;
            facturaDiccionario["data"][0]["datos_fiscales"]["llave_password"]   = PASSWORD_LLAVE;
            facturaDiccionario["data"][0]["cfdi"]["cfdi__comprobante"]["fecha"] = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"); 
            // Generar
            var facturaGenerada = _factura.GeneracionFactura(facturaDiccionario);
            // Assertions
            Assert.NotNull(facturaGenerada["data"][0]["cfdi_respuesta"]);
            Assert.True(Regex.IsMatch(facturaGenerada["data"][0]["cfdi_complemento"]["uuid"].ToString(), "([a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12})"));
        }

        [Fact]
        public void GenerarExitosamenteRecepcion() {
            string recepcion = "{\"meta\":{\"empresa_uid\":\"6e6d79f30d\",\"empresa_api_key\":\"JeqaaLyiRGykgYyQoX_7RQ\",\"ambiente\":\"S\",\"objeto\":\"recepcion\"},\"data\":[{\"datos_fiscales\":{\"certificado_pem\":\"\",\"llave_pem\":\"\",\"llave_password\":\"DDM090629R13\"},\"cfdi\":{\"cfdi__comprobante\":{\"folio\":\"793\",\"serie\":\"C\",\"fecha\":\"\",\"tipo_comprobante\":\"P\",\"lugar_expedicion\":\"83448\",\"moneda\":\"XXX\",\"subtotal\":\"0\",\"total\":\"0\",\"cfdi__emisor\":{\"rfc\":\"DDM090629R13\",\"nombre\":\"CARLOS CESAR OCHOA GAXIOLA\",\"regimen_fiscal\":\"601\"},\"cfdi__receptor\":{\"rfc\":\"GNP9211244P0\",\"nombre\":\"GRUPO NACIONAL PROVINCIAL S.A.B.\",\"uso_cfdi\":\"P01\"},\"cfdi__conceptos\":{\"cfdi__concepto\":[{\"clave_producto_servicio\":\"84111506\",\"clave_unidad\":\"ACT\",\"cantidad\":\"1\",\"descripcion\":\"Pago\",\"valor_unitario\":\"0\",\"importe\":\"0\"}]},\"cfdi__complemento\":{\"pago10__pagos\":{\"pago10__pago\":{\"fecha-pago\":\"2018-08-10T11:21:09\",\"forma-pago\":\"01\",\"tipo-cambio\":\"\",\"moneda\":\"MXN\",\"monto\":\"5000.00\",\"rfc-emisor-ordenante\":\"\",\"rfc-emisor-beneficiario\":\"\",\"nombre-banco-ordenante\":\"\",\"cuenta-ordenante\":\"\",\"cuenta-beneficiario\":\"\",\"tipo-cadena-pago\":\"\",\"num-operacion\":\"1123121241\",\"certificado-pago\":\"\",\"cadena-pago\":\"\",\"sello-pago\":\"\",\"pago10__docto_relacionado\":[{\"id-documento\":\"682C60DC-2A2F-47CD-A34D-39180EFAB2B1\",\"serie\":\"ASD\",\"folio\":\"53452\",\"moneda-dr\":\"MXN\",\"tipo-cambio-dr\":\"\",\"metodo-pago-dr\":\"PPD\",\"numero-parcialidad\":\"1\",\"importe-pagado\":\"5000.00\",\"importe-saldo-anterior\":\"7000.00\",\"importe-saldo-insoluto\":\"2000.00\"}]}}}}}}]}";
            var recepcionDiccionario = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(recepcion);
            
            // Establecer parametros para generacion
            recepcionDiccionario["data"][0]["datos_fiscales"]["certificado_pem"] = _contenidoCertificado;
            recepcionDiccionario["data"][0]["datos_fiscales"]["llave_pem"] = _contenidoLlave;
            recepcionDiccionario["data"][0]["datos_fiscales"]["llave_password"] = PASSWORD_LLAVE;
            recepcionDiccionario["data"][0]["cfdi"]["cfdi__comprobante"]["fecha"] = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");

            // Generar
            var recepcionGenerada = _factura.GeneracionRecepcionPago(recepcionDiccionario);
            Assert.NotNull(recepcionGenerada["data"][0]["cfdi_respuesta"]);
            Assert.True(Regex.IsMatch(recepcionGenerada["data"][0]["cfdi_complemento"]["uuid"].ToString(), "([a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12})"));
        }

        [Fact]
        public void CancelarExitosamenteFactura() {
            string facturaCancelacion = "{\"meta\":{\"empresa_uid\":\"asd123asd\",\"empresa_api_key\":\"123123123\",\"ambiente\":\"S\",\"objeto\":\"factura\"},\"data\":[{\"rfc\":\"\",\"uuid\":[\"\"],\"datos_fiscales\":{\"certificado_pem\":\"\",\"llave_pem\":\"\",\"password_llave\":\"\"},\"acuse\": false}]}";
            var cancelacionDiccionario = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(facturaCancelacion);
            string uuidCancelar = "C39C7784-B41E-40D6-89E7-46683205ED6C";

            // Establecer parametros para cancelacion
            cancelacionDiccionario["data"][0]["rfc"]     = "DDM090629R13";
            cancelacionDiccionario["data"][0]["uuid"][0] = uuidCancelar;
            cancelacionDiccionario["data"][0]["datos_fiscales"]["certificado_pem"] = _contenidoCertificado;
            cancelacionDiccionario["data"][0]["datos_fiscales"]["llave_pem"]       = _contenidoLlave;
            cancelacionDiccionario["data"][0]["datos_fiscales"]["llave_password"]  = PASSWORD_LLAVE;
            // Generar Cancelacion
            var facturaCancelada    = _factura.CancelacionFactura(cancelacionDiccionario);
            // Assertions
            Assert.NotNull(facturaCancelada["meta"]["respuesta_uid"]);
        }

        [Fact]
        public void EnvioExitosoFactura() {
            string facturaEnvio = "{\"meta\":{\"empresa_uid\":\"asd123asd\",\"empresa_api_key\":\"123123123\",\"ambiente\":\"S\",\"objeto\":\"factura\"},\"data\":[{\"uuid\":[\"\"],\"destinatarios\":[{\"correo\":\"sandbox@docdigitales.com\"}],\"titulo\":\"Envio de Factura: 123\",\"texto\":\"Envio de Factura con folio 123, para su revision.\",\"pdf\":\"true\"}]}";
            var facturaDiccionario = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(facturaEnvio);
            string uuidEnviar   = "ACF6B8DB-AA7C-4FBC-A0A2-D8FE04220E2B";

            // Establecer Parametros de envio
            facturaDiccionario["data"][0]["uuid"][0] = uuidEnviar;
            // Generar envio
            var facturaEnviada = _factura.EnviarFactura(facturaDiccionario);
            // Assertions
            Assert.NotNull(facturaEnviada);
        }

        [Fact]
        public void DescargaExitosaFactura() {
            string facturaDescarga  = "{\"meta\":{\"empresa_uid\":\"asd123asd\",\"empresa_api_key\":\"123123123\",\"ambiente\":\"S\",\"objeto\":\"factura\"},\"data\":[{\"uuid\":[\"\"],\"destinatarios\":[{\"correo\":\"sandbox@docdigitales.com\"}],\"titulo\":\"Descargar factura\",\"texto\":\"Adjunto factura generada\",\"pdf\":\"true\"}]}";
            var descargaDiccionario = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(facturaDescarga);
            string uuidDescargar    = "ACF6B8DB-AA7C-4FBC-A0A2-D8FE04220E2B";
            
            // Establecer parametros de envio
            descargaDiccionario["data"][0]["uuid"][0] = uuidDescargar;
            var facturaDescargada = _factura.DescargarFactura(descargaDiccionario);
            // Assertions
            Assert.NotNull(facturaDescargada["data"][0]["link"]);
        }
    }
}