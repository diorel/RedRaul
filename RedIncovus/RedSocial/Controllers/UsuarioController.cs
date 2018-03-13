using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entity;
using Helper;
using Model;
using RedSocial.CustomAttributes;

namespace RedSocial.Controllers
{
    [IfNotLoggedActionAttribute]
    public class UsuarioController : Controller
    {
        private UsuarioModel um = new UsuarioModel();
        private ConocimientoModel cm = new ConocimientoModel();
        private PublicacionModel pm = new PublicacionModel();

        public ActionResult Index(int id = 0)
        {
            return View();
        }
        public ActionResult Perfil()
        {
            ViewBag.Conocimientos = cm.Listar();
            return View(um.Obtener(SessionHelper.GetUser()));
        }

        public ActionResult Ver(string name)
        {
            int id = ViewHelper.GetIdFromUrl(name);
            return View(um.Obtener(id));
        }

        [HttpPost]
        public JsonResult Actualizar(Usuario usuario, List<int> Conocimiento_id = null, HttpPostedFileBase file = null)
        {
            if (usuario.Contrasena == null) 
            {
                ModelState.Remove("Contrasena");
            }

            if (ModelState.IsValid)
            {
                // Agregamos el usuario que queremos actualizar
                usuario.id = SessionHelper.GetUser();

                if (Conocimiento_id != null) 
                {
                    // Agregamos todo los valores seleccionados a una lista de UsuarioConocimiento
                    List<UsuarioConocimiento> conocimientos = new List<UsuarioConocimiento>();
                    foreach (var c in Conocimiento_id) conocimientos.Add(new UsuarioConocimiento { Conocimiento_id = c, Usuario_id = usuario.id });

                    usuario.UsuarioConocimientos = conocimientos;
                }

                return Json(um.Actualizar(usuario, file));
            }
            else
            {
                return Json(new { response = false, message = "Ocurrio un error con la validación del Formulario." });
            }
        }

        [HttpPost]
        [OnlyAjaxRequestAttribute]
        public JsonResult Publicar(Publicacion publicacion)
        {
            if (ModelState.IsValid)
            {
                return Json(pm.Registrar(publicacion));
            }
            else
            {
                return Json(new { response = false, message = "Ocurrio un error con la validación del Formulario." });
            }
        }

        public ActionResult Logout() 
        {
            SessionHelper.DestroyUserSession();
            return Redirect("~");
        }

        [OnlyAjaxRequestAttribute]
        public PartialViewResult Publicaciones(int usuario_id)
        {
            ViewBag.Usuario_id = usuario_id;
            /* Como en esta caso la vista no se encuentra en view/usuario/publicaciones.cshtml
             * le indicamos que la busque en otro lado pasandole el primero parametro */
            return PartialView("~/views/inicio/publicaciones.cshtml", pm.Listar(usuario_id));
        }
    }
}
