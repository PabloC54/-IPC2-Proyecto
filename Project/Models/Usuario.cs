using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IPC2_P1.Models
{
    public class Usuario
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Nombres { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Apellidos { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string Nickname { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 8)]
        public string Contrasena { get; set; }

        [Required]
        public DateTime Nacimiento { get; set; }

        [Required]
        public string Pais { get; set; }

        [Required]
        [EmailAddress]
        public DateTime Email { get; set; } 

    }
}