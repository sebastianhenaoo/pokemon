using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PokemonsGame.Models
{
    public class Pokemon
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Vida { get; set; }
        public int Defensa { get; set; }
        public int Ataque { get; set; }
        public string UrlImagenPpal { get; set; }
        public string UrlImagenSec { get; set; }

        public virtual List<Campo> Campos { get; set; }

    }
}