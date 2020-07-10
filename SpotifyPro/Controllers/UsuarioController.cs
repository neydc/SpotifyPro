using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using SpotifyPro.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;//agregue esto
using Microsoft.AspNetCore.Mvc;
using SpotifyPro.BD;
using SpotifyPro.Extensions;
using Microsoft.EntityFrameworkCore.Internal;

namespace SpotifyPro.Controllers
{
    [Route("Usuario")]
    public class UsuarioController : Controller
    {
        public SpotifyContext context = new SpotifyContext();
        [Obsolete]
        private IHostingEnvironment env;

        [Obsolete]
        public UsuarioController(IHostingEnvironment env)
        {

            this.env = env;
        }
        [Route("VerAmistades")]
        public IActionResult VerAmistades()
        {
        var user = HttpContext.Session.Get<Usuario>("sessionUser");

        var amigos = context.Amistades.Where(o => o.UsuarioLocal.Id == user.Id).ToList();
        var usuarios = new List<Usuario>();

        foreach(var item in amigos)
        {
            var Amistades = context.Usuarios.Where(o => o.Id == item.IdAmigo).FirstOrDefault();
                if (Amistades!=null) {
                    usuarios.Add(Amistades);
                }
        }

            return View(usuarios);
        }

        [Route("AgregarAmigo")]
        [HttpGet]
        public IActionResult AgregarAmigo()
        {
            return View();
        }

        [Route("_AgregarAmigo")]
        [HttpGet]
        public IActionResult _AgregarAmigo(string busqueda = "")
        {
            var user = HttpContext.Session.Get<Usuario>("sessionUser");

            var query = context.Usuarios.Where(o => o.Id != user.Id).AsQueryable();


            var amigos = context.Amistades.Where(o => o.UsuarioLocal.Id == user.Id).ToList();

            ViewBag.Amigos = amigos.Select(o => o.IdAmigo).ToList();
            if (!string.IsNullOrEmpty(busqueda))
            {
                query = query.Where(o => o.Nickname.Contains(busqueda));
            }

            var usuario = query.ToList();

            return View(usuario.ToList().Take(30));
        }

        [Route("Agregar")]
        public IActionResult Agregar(int IdUsuario)
        {
            var user = HttpContext.Session.Get<Usuario>("sessionUser");
          
            Amistad nueva = new Amistad(user.Id, IdUsuario, true);
            context.Amistades.Add(nueva);
            context.SaveChanges();
            return RedirectToAction("Index","Usuario");
        }
    

        [Route("EliminarAmistad")]
        public IActionResult EliminarAmistad(int IdUsuario)
        {
           var user = HttpContext.Session.Get<Usuario>("sessionUser");
                   
            var Amistad = context.Amistades.Where(o => o.IdAmigo==IdUsuario).First();
            
            context.Amistades.Remove(Amistad);
            context.SaveChanges();
            return RedirectToAction("VerAmistades", "Usuario");
           
        }
        [Route("Index")]
        public IActionResult Index(string query){
            try
            {
                var user = HttpContext.Session.Get<Usuario>("sessionUser");
                var misLikess = context.Likes.Where(o => o.IdUsuario == user.Id).ToList();
                var misCanciones = new List<Cancion>();
                var canciones = context.Canciones;

                var amigos = context.Amistades.Where(a=>a.UsuarioLocal.Id==user.Id).ToList();             
                var misAmigos = new List<Usuario>();

                foreach (var item in amigos)
                {
                    var Amistades = context.Usuarios.Where(o => o.Id == item.IdAmigo).FirstOrDefault();
                    if (Amistades != null)
                    {
                        misAmigos.Add(Amistades);
                    }
                }

                ViewBag.usuariosAmigos = misAmigos;

                ViewBag.Name = user.Nickname;
                ViewBag.Id = user.Id;
                ViewBag.RutaImag = HttpContext.Session.GetString("Imagen");

                for (int i = 0; i < misLikess.Count; i++)
                {
                    var cancion = context.Canciones.Where(o => o.Id == misLikess[i].IdCancion).First();
                    misCanciones.Add(cancion);
                }

                ViewBag.MisFav = misCanciones;

                ViewBag.ListasReproduccion = context.ListaReproducciones.Where(o => o.UsuarioId == user.Id);

                ViewBag.quer = query;

                if (!string.IsNullOrEmpty(query))
                {
                    return View(canciones.Where(o => o.Nombre.Contains(query)));
                }
                return View(canciones.ToList());
            }
            catch (Exception)
            {
                return View("Login","Auth");
            }
        }
        [Route("Registrarse")]
        [HttpGet]
        public ViewResult Registrarse()
        {
            return View(new Usuario());
        }
        [Route("Registrarse")]
        [HttpPost]
        public IActionResult Registrarse(Usuario usuario)
        {
            try
            {
                validarUsuarios(usuario);
                if (ModelState.IsValid)
                {
                    usuario.Imagen ="UserNew.png";
                    usuario.FechaCreacion = DateTime.Now;
                    var agregarUsuario = context.Add(usuario);
                    context.SaveChanges();
                    return RedirectToAction("Login","Auth");
                }
            }
            catch (Exception)
            {
                return View(usuario);
            }
            return View(usuario);
        }

        [HttpGet]
        [Route("EditarUsuario")]
        public IActionResult EditarUsuario(int id)
        {
            try
            {
                var user = HttpContext.Session.Get<Usuario>("sessionUser");

                var user1 = context.Usuarios.Where(o => o.Id == id).First();
                ViewBag.IDSSSS = user1.Id;
                ViewBag.ImagenEditar = user1.Imagen;
                return View(user1);
            }
            catch (Exception)
            {
                return RedirectToAction("Login","Auth");
            }
        }


        [Route("EditarUsuario")]
        [HttpPost]
        [Obsolete]
        public IActionResult EditarUsuario(Usuario user, IFormFile photo)
        {
            try
            {
                var userDb = context.Usuarios.Where(o => o.Id == user.Id).First();
                //validar esto

                userDb.Apellido = user.Apellido;
                userDb.Nombre = user.Nombre;
                userDb.Correo = user.Correo;
                userDb.Nickname = user.Nickname;
                userDb.Password = user.Password;

                if (photo.Length > 0)
                {
                    var filePath = Path.Combine(env.WebRootPath, "Images", photo.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        photo.CopyTo(stream);
                    }
                }
                user.Imagen = photo.FileName;
                userDb.Imagen = user.Imagen;
                ViewBag.RutaImag = userDb.Imagen;
                string img = userDb.Imagen;
                HttpContext.Session.SetString("Imagen", img);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Auth");
            }
            
        }
        //////////////////////////////////////////////////////////////

        public void validarUsuarios(Usuario usuario)
        {


            if (usuario.Apellido == null || usuario.Apellido == "")
            {
                ModelState.AddModelError("Apellido", "El campo Apellido es requerido");
            }
            if (usuario.Password == null || usuario.Password == "")
            {
                ModelState.AddModelError("Password", "El campo Password es requerido");
            }
            if (usuario.Nombre == null || usuario.Nombre == "")
            {
                ModelState.AddModelError("Nombre", "El campo Nombre es requerido");
            }
            if (!validarLetras(usuario.Nombre))
            {
                ModelState.AddModelError("Nombre1", "Solo ingrese caracteres alfabeticos");
            }

            if (!validarLetras(usuario.Apellido))
            {
                ModelState.AddModelError("Apellido1", "Solo ingrese caracteres alfabeticos");
            }
            if (usuario.Correo == null || usuario.Correo == "")
            {
                ModelState.AddModelError("Correo", "El campo Correo es requerido");
            }
            if (usuario.Nickname == null || usuario.Nickname == "")
            {
                ModelState.AddModelError("Nickname", "El campo Nickname es requerido");
            }
           

            
           
        }
        public void validarUserLogin(Usuario usuario)
        {
            
            if (usuario.Nickname == null || usuario.Nickname == "")
            {
                ModelState.AddModelError("Nickname2", "El campo Nickname es requerido");
            }
           
            if (usuario.Password == null || usuario.Password == "")
            {
                ModelState.AddModelError("Password2", "El campo Password es requerido");
            }
           
        }

        //Metodos de validación
        public bool validarLetras(string numString)
        {
            string parte = numString.Trim();
            int count = parte.Count(s => s == ' ');
            if (parte == "")
            {
                return false;
            }
            else if (count > 1)
            {
                return false;
            }
            char[] charArr = parte.ToCharArray();
            foreach (char cd in charArr)
            {
                if (!char.IsLetter(cd) && !char.IsSeparator(cd))
                    return false;
            }
            return true;
        }
      
    }
}