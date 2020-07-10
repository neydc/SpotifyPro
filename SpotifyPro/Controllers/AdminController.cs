using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SpotifyPro.Controllers
{
    [Route("Admin")]
    public class AdminController : Controller
    {
        [Obsolete]
        private IHostingEnvironment ihostingEnvironment;

        [Obsolete]
        public AdminController(IHostingEnvironment ihostingEnvironment)
        {
            this.ihostingEnvironment = ihostingEnvironment;
        }

        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("Upload")]
        [HttpPost]
        [Obsolete]
        public IActionResult Upload(IFormFile photo)
        {
            if (photo!=null) {
                var path = Path.Combine(ihostingEnvironment.WebRootPath, "Images", photo.FileName);
                var stream = new FileStream(path, FileMode.Create);
                    photo.CopyToAsync(stream);
                ViewBag.photo = photo.FileName;
                
                return View("Succes");
            }
            ViewBag.message = "Ingrese una imagen";
            return View("Index");
        }

        [Route("Success")]
        public IActionResult Succes() {

            return View();
        }
    }
}