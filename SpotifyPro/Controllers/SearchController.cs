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
    public class SearchController : Controller
    {
        public SpotifyContext context = new SpotifyContext();
        [HttpGet]
        public IActionResult Index(string query)
        {
            return View();
        }
        public ActionResult ObtenerAlbumes() {
            var albumes = context.Albumes.ToList();
            return View(albumes);
        }
        [HttpGet]
        public IActionResult _Index(string query)
        {
            var user = HttpContext.Session.Get<Usuario>("sessionUser");
            var canciones = context.Canciones.AsQueryable();
            var ObtenerAlbum = context.Albumes.AsQueryable();
            if (!string.IsNullOrEmpty(query))
            {
                //Buscar cancion
                canciones = canciones.Where(a => a.Nombre.Contains(query));
                ViewBag.canciones = canciones;
            } //var canciones = Obtenercanciones.Take(3);                //var canciones = Obtenercanciones.Take(3);
              

            //Buscar Album
            if (!string.IsNullOrEmpty(query)) { 
                ObtenerAlbum = ObtenerAlbum.Where(a => a.Nombre.Contains(query));
                ViewBag.albumes = ObtenerAlbum;
            }

            if (string.IsNullOrEmpty(query))
            {
                return RedirectToAction("ObtenerAlbumes");
            }

            //Buscar Grupo
            //var Obtenercanciones = context.Canciones.Where(a => a.Nombre.Equals(query)).ToList();
            //var canciones = Obtenercanciones.Take(3);
            //ViewBag.canciones = canciones;


            //Buscar Artista
            //var Obtenercanciones = context.Canciones.Where(a => a.Nombre.Equals(query)).ToList();
            //var canciones = Obtenercanciones.Take(3);
            //ViewBag.canciones = canciones;


            return View();
        }
    }
}