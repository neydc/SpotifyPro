using System;
using System.Collections.Generic;
using System.Linq;
using SpotifyPro.Models;
using Microsoft.AspNetCore.Mvc;
using SpotifyPro.BD;
using Microsoft.AspNetCore.Http;
using SpotifyPro.Extensions;

namespace SpotifyPro.Controllers
{
    public class ListaReproduccionController : Controller
    {
 
        SpotifyContext context = new SpotifyContext();

 
        public IActionResult Index()
        {
            var user = HttpContext.Session.Get<Usuario>("sessionUser");
            var miLista = context.ListaReproducciones.Where(o => o.UsuarioId == user.Id).AsQueryable();

            return View(miLista.ToList());
        }

        public IActionResult IndexPrincipal()
        {
            var user = HttpContext.Session.Get<Usuario>("sessionUser");
            var miLista = context.ListaReproducciones.Where(o => o.UsuarioId == user.Id).AsQueryable();

            return View(miLista.ToList());
        }

        public IActionResult _Index(string busqueda)
        {

            var user = HttpContext.Session.Get<Usuario>("sessionUser");
            var query = context.ListaReproducciones.Where(o => o.UsuarioId == user.Id).AsQueryable();
            
            if (!string.IsNullOrEmpty(busqueda))
            {
                query = query.Where(o => o.Nombre.Contains(busqueda));
            }

            return View(query);
        }

        public IActionResult _Index1(int busqueda)
        {

            var user = HttpContext.Session.Get<Usuario>("sessionUser");
           
            var query = context.ListaReproducciones.Where(o => o.Id == busqueda).AsQueryable();

            return View(query/*.ToList()*/);
        }

        public IActionResult DetalleDeLaLista(int IdLista)
        {
            var user = HttpContext.Session.Get<Usuario>("sessionUser");
            var detalleListaReproduccion = context.DetalleListaReproduccionCanciones
                .Where(o => o.IdListaReproduccion == IdLista).ToList();
            var CancionesLista = new List<Cancion>();
            for (int i = 0; i < detalleListaReproduccion.Count; i++)
            {
               var cancion = context.Canciones.Where(o => o.Id == detalleListaReproduccion[i].IdCancion).First();
               CancionesLista.Add(cancion);
            }
            var miLista = context.ListaReproducciones.Where(o => o.UsuarioId == user.Id).Where(o=>o.Id==IdLista).First();
            ViewBag.nombre = user.Nombre + " " +user.Apellido;
            ViewBag.NombreLista = miLista.Nombre;
            return View(CancionesLista);
        }
        
        [HttpPost]
        public IActionResult AgregarListaReproduccion(string Nombre)
        {
            var user = HttpContext.Session.Get<Usuario>("sessionUser");
            bool estado = false;
            
            
            if (ModelState.IsValid)
            {
                List<ListaReproduccion> ListaReproduccion = context.ListaReproducciones.ToList();
                for (int i = 0; i < ListaReproduccion.Count(); i++)
                {
                    if (Nombre == ListaReproduccion[i].Nombre || Nombre==""||Nombre==null)
                    {
                        estado = true;
                    }
                }

                if (!estado)
                {


                    ListaReproduccion nuevo = new ListaReproduccion(Nombre, user.Id);
                    nuevo.FechaCreacion = DateTime.Now;
                    nuevo.Imagen = "album01.jpg";
                    context.ListaReproducciones.Add(nuevo);
                    context.SaveChanges();
                }              
            }

            return RedirectToAction("Index","Usuario");
        }
        
        
    }
}