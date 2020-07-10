
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SpotifyPro.BD;
using SpotifyPro.Extensions;
using SpotifyPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SpotifyPro.Controllers
{
    public class AuthController : Controller
    {
        SpotifyContext context = new SpotifyContext();
        private IConfiguration configuration;

        public AuthController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Usuario user)
        {
            var userv = context.Usuarios.FirstOrDefault(o => o.Nickname == user.Nickname && o.Password == user.Password);

            if (userv!=null) {
                validarUserLogin(user);
                if (ModelState.IsValid)
                {
                    HttpContext.Session.Set("sessionUser", userv);

                    if (userv != null)
                    {
                        var claims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.Name, userv.Nickname),
                        };

                        var userIdentity = new ClaimsIdentity(claims, "Login");
                        var principal = new ClaimsPrincipal(userIdentity);

                        HttpContext.SignInAsync(principal);
                        string img = userv.Imagen;
                        HttpContext.Session.SetString("Imagen", img);
                        return RedirectToAction("Index", "Usuario");
                    }
                }
            }
            ViewBag.Nickanam =user.Nickname;
            ViewBag.Pass= user.Password;
            return View(user);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Auth");
        }

        public string GetBashPassword(string pass)
        {
            string token = configuration.GetValue<string>("Token");
            pass = pass + token;
            var sha = SHA256.Create();
            var hashdata = sha.ComputeHash(Encoding.Default.GetBytes(pass));
            return Convert.ToBase64String(hashdata);
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
    }
}