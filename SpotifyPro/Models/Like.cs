using System;

namespace SpotifyPro.Models
{
    public class Like
    {
        public int Id { get; set; }
        public bool Estado { get; set; }
        public int IdUsuario { get; set; }
        public string Comentario { get; set; }
        public int IdGrupo { get; set; }
        public int IdArtista { get; set; }
        public int IdListaReproduccion { get; set; }
        public int IdCancion { get; set; }
        public int IdAlbum { get; set; }
   
        
        public Usuario Usuario  { get; set; }
        public Cancion Cancion  { get; set; }

        public Like( bool estado, int idUsuario, string comentario, int idGrupo, int idArtista, int idListaReproduccion, int idCancion, int idAlbum)
        {
            Estado = estado;
            IdUsuario = idUsuario;
            Comentario = comentario;
            IdGrupo = idGrupo;
            IdArtista = idArtista;
            IdListaReproduccion = idListaReproduccion;
            IdCancion = idCancion;
            IdAlbum = idAlbum;

        }
    }
}