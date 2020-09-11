using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IPC2_P1.Models
{
    public class Archivo
    {
        public HttpPostedFile archivo { get; set; }
    }
}