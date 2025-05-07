using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedioUnicoDePago.Models
{
    public class DirectorMV
    {
        public string token { get; set; }
        public string sign { get; set; }
        public string userId { get; set; }
    }
}