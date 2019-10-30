using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (HttpContext.Cache["ObjectList"] == null)
            {
                HttpContext.Cache["ObjectList"] = new List<Models.Msg>();
            }

            var s = Models.OnlineUsers.v(GetIPAddress());
            if (String.IsNullOrEmpty(s)) return null; else ViewBag.n = s;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Login()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult getMgss()
        {
            List<Models.Msg> msgss = (List<Models.Msg>)HttpContext.Cache["ObjectList"];

            if(msgss!= null && msgss.Count > 0)
            {
                msgss = msgss.Where(e => e.datahora > DateTime.Now.AddMinutes(-5)).ToList();
            }            

            return Json(msgss.OrderByDescending(s => s.id), JsonRequestBehavior.AllowGet);
        }

        public JsonResult setMsg(string msg, string nome)
        {
            List<Models.Msg> msgss = (List<Models.Msg>)HttpContext.Cache["ObjectList"];

            Models.Msg omsg = new Models.Msg
            {
                nome = nome,
                msg = msg,
                datahora = DateTime.Now,
                id = msgss.Count + 1
            };

            msgss.Add(omsg);

            try
            {
                return Json(msgss, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        
        //public static string GetLocalIPAddress()
        //{
        //    var host = Dns.GetHostEntry(Dns.GetHostName());
        //    foreach (var ip in host.AddressList)
        //    {
        //        if (ip.AddressFamily == AddressFamily.InterNetwork)
        //        {
        //            return ip.ToString();
        //        }
        //    }
        //    throw new Exception("No network adapters with an IPv4 address in the system!");
        //}

        protected string GetIPAddress()
        {
            var ip = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(0).ToString();
            string IP = Request.Params["HTTP_CLIENT_IP"] ?? Request.UserHostAddress;
            var returns = "";

            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    returns = addresses[0];
                    if (returns.Contains(":")) return returns.Split(':')[0];
                }
            }

            returns = context.Request.ServerVariables["REMOTE_ADDR"];
            if (returns.Contains(":")) return returns.Split(':')[0];

            return "10.2.0.81";
        }
    }
}