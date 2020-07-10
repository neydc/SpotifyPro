using System;
using System.Collections.Generic;

namespace SpotifyPro.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public DateTime FechaCreacion { get; set; }

        public string Password { get; set; }
        
        public string Nickname { get; set; }
        public string Imagen{ get; set; }

        public List<Like> likes { get; set; }
        
        public List<Amistad> Amistad1 { get; set; }
        public List<Amistad> Amistad2 { get; set; }
        
        public List<ListaReproduccion> ListaReproducciones { get; set; }
        public List<CompartirCancion> Amistad1C { get; set; }
        public List<CompartirCancion> Amistad2C { get; set; }

    }
}