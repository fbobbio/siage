using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Siage.Base
{
    public class Email
    {
        public string Asunto { get; set; }
        public string Mensaje { get; set; }

        public string RemitenteNombre { get; set; }
        public string RemitenteCuenta { get; set; }
        public string RemitentePassword { get; set; }

        private List<string> _destinatarios;
        public List<string> Destinatarios 
        {
            get { return _destinatarios; }
            set { _destinatarios = value; } 
        }

        public void AddDestinatario(string email)
        {
            _destinatarios.Add(email);
        }

        private List<string> _adjuntos;
        public List<string> UrlArchivosAdjuntos 
        {
            get { return _adjuntos; }
            set { _adjuntos = value; }
        }

        public void AddArchivoAdjunto(string url)
        {
            _adjuntos.Add(url);
        }

        public Email()
        {
            _destinatarios = new List<string>();
            _adjuntos = new List<string>();
        }
        
        public void Enviar()
        {
            string tmp = "";
            string TipoCorreoDeTuCuenta = "";
            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();


            if (email_bien_escrito(RemitenteCuenta) == true)
            {
                msg.From = new MailAddress(RemitenteCuenta, RemitenteNombre, System.Text.Encoding.UTF8);

                foreach (string x in Destinatarios)
                {
                    tmp = x.Trim();
                    if (tmp != "" && email_bien_escrito(tmp) == true)
                        msg.To.Add(tmp);
                    else
                        throw new Exception("La dirección de correo del destinatario no está bien escrita (Ej. ejemplo@dominio.com)");
                }
            }
            else
            {
                throw new Exception("La dirección de correo no está bien escrita (Ej. ejemplo@dominio.com)");
            }

            msg.Subject = Asunto;
            msg.SubjectEncoding = System.Text.Encoding.UTF8;
            msg.Body = Mensaje;
            msg.BodyEncoding = System.Text.Encoding.UTF8;

            // Archivos adjuntos
            foreach (var url in UrlArchivosAdjuntos)
            {
                msg.Attachments.Add(new Attachment(url));
            }
            
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(RemitenteCuenta, RemitentePassword);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            TipoCorreoDeTuCuenta = tipoEmail(RemitenteCuenta.ToLower());
            switch (TipoCorreoDeTuCuenta.ToLower())
            {
                    //hotmail.com
                case "hotmail":
                    msg.IsBodyHtml = true; // solo para hotmail                  
                    client.Host = "smtp.live.com"; // solo para hotmail
                    client.EnableSsl = true; //Esto es para que vaya a través de SSL que es obligatorio con GMail
                    break;
                    //gmail.com
                case "gmail":
                    msg.IsBodyHtml = false;
                    client.Host = "smtp.gmail.com"; // solo para Gmail
                    client.Port = 587;
                    client.EnableSsl = true; //Esto es para que vaya a través de SSL que es obligatorio con GMail
                    break;
                    //yahoo.com.ar
                case "yahoo":
                    msg.IsBodyHtml = false; 
                    client.Host = "smtp.mail.yahoo.com.ar";
                    client.Port = 587;
                    client.EnableSsl = false; //Yahoo no soporta SSL. 
                    break;
                default:
                    throw new Exception("El tipo de correo es inválido");
            }
          
            try
            {
                client.Send(msg);
                // se envio correctamente
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                throw new ApplicationException("Ha ocurrido un error durante en envío de e-mail. Comunicarse con el administrador a través de la página de contacto.");
            }
        }
        
        // Validar si el email tiene la forma xx@xx.xx
        public bool email_bien_escrito(String email)
        {
            String expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, String.Empty).Length == 0)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public string tipoEmail(string tuCuenta)
        {
            string cuenta = "";
            int arroba;

            cuenta = tuCuenta;

            arroba = cuenta.LastIndexOf("@");
            if (cuenta.IndexOf("@hotmail.com", arroba) < 0 && cuenta.IndexOf("@yahoo.com.ar", arroba) < 0 && cuenta.IndexOf("@yahoo.com", arroba) < 0 && cuenta.IndexOf("@gmail.com", arroba) < 0)
                throw new Exception("El tipo de correo no es válido. Solo se aceptan @hotmail.com, @yahoo.com.ar, @yahoo.com, @gmail.com");

            if (cuenta.IndexOf("@hotmail.com", arroba) >= 0)
                return tuCuenta = "HOTMAIL";
            else
                if (cuenta.IndexOf("@yahoo.com.ar", arroba) >= 0 || cuenta.IndexOf("@yahoo.com", arroba) >= 0)
                    return tuCuenta = "YAHOO";
                else
                    return tuCuenta = "GMAIL";
        }
    }
}   
