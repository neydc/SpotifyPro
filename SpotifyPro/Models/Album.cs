using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyPro.Models
{
    public class Album
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Id_Grupo{ get; set; }
        public int Id_Like { get; set; }
    }
}
