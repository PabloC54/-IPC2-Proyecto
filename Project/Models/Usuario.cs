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
        [StringLength(20, MinimumLength = 3)]
        public string Nombres { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Apellidos { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 4)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 8)]
        public string Contraseña { get; set; }

        [Required]
        public DateTime Fecha_Nacimiento { get; set; }

        [Required]
        [StringLength(30)]
        public string Pais { get; set; }

    }
}