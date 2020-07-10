using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyPro.Models
{
    public class Artista
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Foto { get; set; }
        public string Nacionalidad { get; set; }
        public int Id_Like { get; set; }
    }
}
