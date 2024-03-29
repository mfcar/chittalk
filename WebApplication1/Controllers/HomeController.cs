﻿using System;
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
        private string _user
        {
            get
            {
                return Models.OnlineUsers.getNickByIp(GetIPAddress(), Request.Url.AbsoluteUri.Contains("localhost"));
            }
        }

        public ActionResult Index()
        {
            if (HttpContext.Cache["ObjectList"] == null)
            {
                HttpContext.Cache["ObjectList"] = new List<Models.Msg>();
            }

            if (HttpContext.Cache["UserList"] == null)
            {
                HttpContext.Cache["UserList"] = new List<String>();
            }

            //user = ;
            if (String.IsNullOrEmpty(_user)) return null;
            else
            {
                List<String> list = (List<String>)HttpContext.Cache["UserList"];
                ViewBag.n = _user;

                if(!list.Contains(_user))
                {
                    list.Add(_user);
                }
                
                HttpContext.Cache["UserList"] = list;
            }

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
            List<String> listousers = (List<String>)HttpContext.Cache["UserList"];

            if (msgss!= null && msgss.Count > 0)
            {
                msgss = msgss.Where(e => e.datahora > DateTime.Now.AddMinutes(-5)).ToList();
                msgss = msgss.Where(e => e.NomePara == _user || e.NomePara == "all-online-users" || e.NomeDe == _user).ToList();
            }

            var obj = new { msgss = msgss.OrderByDescending(s => s.id), onlineusers = listousers };

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public JsonResult setMsg(string msg, string nome, string para)
        {
            List<Models.Msg> msgss = (List<Models.Msg>)HttpContext.Cache["ObjectList"];

            Models.Msg omsg = new Models.Msg
            {
                NomeDe = nome,
                NomePara = para,
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

        public JsonResult removeUser(string user)
        {
            List<String> list = (List<String>)HttpContext.Cache["UserList"];
            list.Remove(user);
            HttpContext.Cache["UserList"] = list;

            try
            {
                return Json(new { msg = "removido" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

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