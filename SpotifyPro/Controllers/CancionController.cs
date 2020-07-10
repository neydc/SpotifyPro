using System;
using System.Linq;
using SpotifyPro.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpotifyPro.BD;
using Microsoft.AspNetCore.Http;
using SpotifyPro.Extensions;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;

namespace SpotifyPro.Controllers
{
    [Authorize]
    public class CancionController : Controller
    {
        SpotifyContext context = new SpotifyContext();
        SoundPlayer player = new SoundPlayer();
        [DllImport("wmpps.dll")]
        private static extern long mciSendString(string strCommand, StringBuilder strReturn, int iReturnLength, IntPtr hwndCallback);
        public IActionResult _ReproducirCancion()
        {
            return View();
        }
        
        public IActionResult ReproducirCancion(int id)
        {
            player.Stop();
            var Ruta = "E:/SpotifyPro/SpotifyPro/wwwroot/Music/";
            var cancion = context.Canciones.Where(a=>a.Id==id).First();
           
            player.SoundLocation = Ruta + cancion.Musica;
            player.Play();
            return RedirectToAction("Index","Usuario");
        }

        public IActionResult PararCancion()
        {
            player.Stop();
            return RedirectToAction("Index", "Usuario");
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult _Index(string busqueda)
        {

            var user = HttpContext.Session.Get<Usuario>("sessionUser");
            var query = context.Canciones.AsQueryable();

            if (!string.IsNullOrEmpty(busqueda))
            {
                query = query.Where(o => o.Nombre.Contains(busqueda));
            }

            var misLikess = context.Likes.Where(o => o.IdUsuario == user.Id).ToList();
            ViewBag.favoritas = misLikess.Select(o => o.IdCancion).ToList();

            return View(query);
        }

        public IActionResult AgregarFavorita(int IdCancion)
        {
            var user = HttpContext.Session.Get<Usuario>("sessionUser");
            var aa = user.Id;
            bool estado = false;
            var misLikess = context.Likes.Where(o => o.IdUsuario == user.Id).ToList();
            var misCanciones = new List<Cancion>();
            for (int i = 0; i < misLikess.Count; i++)
            {
                var cancion = context.Canciones.Where(o => o.Id == misLikess[i].IdCancion).First();
                cancion.Estado = true;
                misCanciones.Add(cancion);
            }

            for (int i = 0; i < misCanciones.Count; i++)
            {
                if (misCanciones[i].Id == IdCancion)
                {
                    estado = true;
                }
            }

            if (!estado)
            {
                Like nuevo = new Like(true, user.Id, "", 1, 1, 1, IdCancion, 1);
                context.Likes.Add(nuevo);
                
                context.SaveChanges();
            }
            return RedirectToAction("MisFavoritas",new { usuarioId = aa});
        }
        [HttpPost]
        public IActionResult GuardarCancionEnLista(int ListaReproduccion, int CancionId)
        {
            if (ModelState.IsValid)
            {
                DetalleListaReproduccionCancion nuevo = new DetalleListaReproduccionCancion();
                nuevo.IdCancion = CancionId;

                nuevo.IdListaReproduccion = ListaReproduccion;
                context.DetalleListaReproduccionCanciones.Add(nuevo);
                context.SaveChanges();
                return RedirectToAction("Index", "Usuario");
            }

            ViewBag.ListaReproduccion = context.ListaReproducciones.ToList();
            return RedirectToAction("Index", "Usuario");
        }

        public IActionResult MisFavoritas(int usuarioId)
        {
            var user = HttpContext.Session.Get<Usuario>("sessionUser");
            var misLikes = context.Likes.Where(a=>a.Id==user.Id).ToList();


            //foreach(var item in misLikes) {
            //    var cancion = context.Canciones.Where(o=>o.Id==item.IdCancion).First();
            //}
            var probar = context.Likes.Where(a=>a.IdUsuario==usuarioId).Include("Cancion").Where(a=>a.Estado==true);
            //var  misCanciones = context.Likes.Where(a => a.IdCancion == IdCancion).ToList();
            return View(probar);
        }
    }
    }