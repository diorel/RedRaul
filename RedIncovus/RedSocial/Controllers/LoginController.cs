using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entity;
using Model;
using System.Web.Mvc;
using RedSocial.CustomAttributes;

namespace RedSocial.Controllers
{
    public class LoginController : Controller
    {

        private UsuarioModel um = new UsuarioModel();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [OnlyAjaxRequestAttribute]
        public JsonResult Acceder(string Correo, string Contrasena)
        {
            if (ModelState.IsValid)
            {
                return Json(um.Acceder(new Usuario { Correo = Correo, Contrasena = Contrasena }));
            }
            else
            {
                return Json(new { response = false, message = "Ocurrio un error con la validación del Formulario." });
            }
        }

        [HttpPost]
        [OnlyAjaxRequestAttribute]
        public JsonResult Registrar(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                return Json(um.RegistroDeLogin(usuario));
            }
            else
            {
                return Json(new { response = false, message = "Ocurrio un error con la validación del Formulario." });
            }

        }
    }
}