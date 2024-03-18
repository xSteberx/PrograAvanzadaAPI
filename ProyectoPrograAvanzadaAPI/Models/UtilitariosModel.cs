﻿using Microsoft.IdentityModel.Tokens;
using ProyectoPrograAvanzadaAPI.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ProyectoPrograAvanzadaAPI.Models
{
    public class UtilitariosModel (IConfiguration _configuration) : IUtilitariosModel
    {
        string SecretKey = _configuration.GetSection("Settings:SecretKey").Value ?? string.Empty;

        public string GenerarToken(string correo)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("username", Encrypt(correo)));


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: cred);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public string GenerarNuevaContrasenna()
        {
            int length = 8;
            const string valid = "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456789";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        public void EnviarCorreo(string Destinatario, string Asunto, string Mensaje)
        {
            string correoSMTP = _configuration.GetSection("Settings:correoSMTP").Value!;
            string claveSMTP = _configuration.GetSection("Settings:claveSMTP").Value!;

            MailMessage msg = new MailMessage();
            msg.To.Add(new MailAddress(Destinatario));
            msg.From = new MailAddress(correoSMTP);
            msg.Subject = Asunto;
            msg.Body = Mensaje;
            msg.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(correoSMTP, claveSMTP);
            client.Port = 587;
            client.Host = "smtp.office365.com";
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Send(msg);
        }

        public string Encrypt(string texto)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(SecretKey);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(texto);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

    }
}
