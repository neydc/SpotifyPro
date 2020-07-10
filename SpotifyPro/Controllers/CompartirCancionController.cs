using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotifyPro.BD;
using SpotifyPro.Extensions;
using SpotifyPro.Models;

namespace SpotifyPro.Controllers
{
    public class CompartirCancionController : Controller
    {
        SpotifyContext context = new SpotifyContext();
        public IActionResult Index()
        {
            var user = HttpContext.Session.Get<Usuario>("sessionUser");
            Dictionary<int, Cancion> DCancion = new Dictionary<int, Cancion>();
            var Canciones = context.Canciones.ToList();
            foreach (var x in Canciones)
            {
                DCancion.Add(x.Id, x);

            }
            Dictionary<int, Usuario> Usuarios = new Dictionary<int, Usuario>();
            var usuarios = context.Usuarios.ToList();
            foreach (var x in usuarios)
            {
                Usuarios.Add(x.Id, x);

            }
          
            var CancionesCompartidas = context.CompartirCanciones.Where(o => o.IdLocalC == user.Id);
            var CancionesCompartidasAMi = context.CompartirCanciones.Where(o => o.IdAmigoC == user.Id);
            ViewBag.usuarios = Usuarios;
            ViewBag.canciones = DCancion;

            ViewBag.CancionesCompartidas = CancionesCompartidas;

            ViewBag.cancionesCompartidasAMi = CancionesCompartidasAMi;

            return View();

        }
    
        public IActionResult Compartir(int IdCancion, int IdAmigo)
        {
            var user = HttpContext.Session.Get<Usuario>("sessionUser");
            
            CompartirCancion compartirCancion = new CompartirCancion(user.Id, IdAmigo, IdCancion);
            var amigos = context.Amistades.Where(o => o.UsuarioLocal.Id == user.Id).ToList();
            var usuarios = new List<Usuario>();
            for (int i = 0; i < amigos.Count; i++)
            {
                var Amistades = context.Usuarios.Where(o => o.Id == amigos[i].IdAmigo).FirstOrDefault();
                usuarios.Add(Amistades);
            }
            context.CompartirCanciones.Add(compartirCancion);
            context.SaveChanges();
            return RedirectToAction("Index", "CompartirCancion"); 
        }

        public IActionResult _compartirCancion()
        {
            var user = HttpContext.Session.Get<Usuario>("sessionUser");
            var amigos = context.Amistades.Where(o => o.UsuarioLocal.Id == user.Id).ToList();
            var usuarios = new List<Usuario>();
            for (int i = 0; i < amigos.Count; i++)
            {
                var Amistades = context.Usuarios.Where(o => o.Id == amigos[i].IdAmigo).First();
                usuarios.Add(Amistades);
            }

            ViewBag.amigos = usuarios;
            return View();
        }



    }
}