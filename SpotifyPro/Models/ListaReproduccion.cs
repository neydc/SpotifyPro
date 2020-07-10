using System;
using System.Collections.Generic;

namespace SpotifyPro.Models
{
    public class ListaReproduccion
    {
        public int Id { get; set; }

        public string Imagen { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaCreacion { get; set; }
        
        public int UsuarioId{ get; set; }

        public Usuario usuario { get; set; }
        
        public List<DetalleListaReproduccionCancion> DetalleListaReproduccionCanciones { get; set; }

        public ListaReproduccion()
        {
        }

        public ListaReproduccion(string nombre, int usuarioId)
        {
            Nombre = nombre;
            UsuarioId = usuarioId;
        }
    }
}