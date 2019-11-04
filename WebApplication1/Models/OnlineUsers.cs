using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class OnlineUsers
    {
        public static string getNickByIp(string a, bool local)
        {
            try
            {
                var cod = System.Configuration.ConfigurationManager.AppSettings["cod_P"];
                var base64EncodedBytes = System.Convert.FromBase64String(cod);
                string decode = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
                if (decode.Contains(a))
                {
                    var l = decode.Split(';').ToList();
                    var n = l.Where(s => s.Contains(a)).First();
                    return n.Split('=')[1];
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            if (!local) return null;
            else return "local";
        }


        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}

/*

;*/

