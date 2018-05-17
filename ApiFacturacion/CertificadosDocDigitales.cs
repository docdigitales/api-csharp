using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using ApiFacturacionDocDigitales;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.OpenSsl;

namespace ApiFacturacionDocDigitales
{
    public class CertificadosDocDigitales {
        public string ExportarCertificadoPem(string pathCertificado) {
            X509Certificate cert = X509Certificate.CreateFromCertFile(pathCertificado);
            string contenido = Convert.ToBase64String(cert.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks);
            string pem = FormatoPEMCertificado(contenido, new StringBuilder());
            return pem;           
        }

        public string ExportarLlavePem(string pathLlave, string passwordLlave) {
            byte[] llaveBytes = File.ReadAllBytes(pathLlave);
            AsymmetricKeyParameter asp = PrivateKeyFactory.DecryptKey(passwordLlave.ToCharArray(), llaveBytes);
            StringWriter stWriter = new StringWriter();
            PemWriter pmw = new PemWriter(stWriter);
            pmw.WriteObject(asp);
            stWriter.Close();
            return stWriter.ToString();
        }

        private string FormatoPEMCertificado(string contenido, StringBuilder builder) {
            builder.AppendLine("-----BEGIN CERTIFICATE-----");
            builder.AppendLine(contenido);
            builder.AppendLine("-----END CERTIFICATE-----");
            return builder.ToString();
        }
    }
}