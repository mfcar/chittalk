using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if(HttpContext.Cache["ObjectList"] == null)
            {
                HttpContext.Cache["ObjectList"] = new List<Models.Msg>();
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

            //Models.Msg msg = new Models.Msg
            //{
            //    nome = "pedin",
            //    msg = DateTime.Now.ToString(),
            //    id = msgss.Count+1
            //};

            //msgss.Add(msg);

            return Json(msgss.OrderByDescending(s => s.id), JsonRequestBehavior.AllowGet);
        }

        public JsonResult setMsg(string msg, string nome)
        {
            List<Models.Msg> msgss = (List<Models.Msg>)HttpContext.Cache["ObjectList"];

            Models.Msg omsg = new Models.Msg
            {
                nome = nome,
                msg = msg,
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
    }
}