namespace SpotifyPro.Models
{
    public class Amistad
    {
        public int Id { get; set; }
        public int IdLocal { get; set; }
        public int IdAmigo { get; set; }
        public bool Estado { get; set; }
        public Usuario UsuarioLocal { get; set; }
        public Usuario UsuarioAmigo { get; set; }

        public Amistad(int idLocal, int idAmigo, bool estado)
        {
            IdLocal = idLocal;
            IdAmigo = idAmigo;
            Estado = estado;
        }
        public Amistad()
        {

        }
    }
}