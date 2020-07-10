using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyPro.Models
{
    public class CompartirCancion
    {
       
            public int Id { get; set; }
            public int IdLocalC { get; set; }
            public int IdAmigoC { get; set; }
            public int IdCancion { get; set; }

            public Usuario UsuarioLocalC { get; set; }
            public Usuario UsuarioAmigoC { get; set; }

            public Cancion cancion { get; set; }

            public CompartirCancion(int idLocalC, int idAmigoC, int idCancion)
            {
                IdLocalC = idLocalC;
                IdAmigoC = idAmigoC;
                IdCancion = idCancion;
            }

            public CompartirCancion()
            {
            }
        
    }

}

