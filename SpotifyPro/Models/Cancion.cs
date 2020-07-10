using System;
using System.Collections.Generic;

namespace SpotifyPro.Models
{
    public class Cancion
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Imagen { get; set; }

        public DateTime FechaLanzamiento { get; set; }
        public int IdAlbum { get; set; }
        public string Musica { get; set; }
        
        public int SiMeGusta { get; set; }
        public int NoMeGusta { get; set; }
        public bool Estado { get; set; }

        public List<Like> likes { get; set; }
        public List<DetalleListaReproduccionCancion> DetalleListaReproduccionCanciones { get; set; }
        public List<CompartirCancion> CompartirCanciones { get; set; }

    }
}