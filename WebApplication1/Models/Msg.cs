using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Msg
    {
        public int id { get; set; }
        public string NomeDe { get; set; }
        public string NomePara { get; set; }
        public string msg { get; set; }
        public DateTime datahora { get; set; }
    }
}