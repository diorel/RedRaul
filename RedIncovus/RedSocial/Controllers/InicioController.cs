using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Helper;
using Model;
using RedSocial.CustomAttributes;




namespace RedSocial.Controllers
{
    [IfNotLoggedActionAttribute]
    public class InicioController : Controller
    {
        private UsuarioModel um = new UsuarioModel();
        private PublicacionModel pm = new PublicacionModel();

        public ActionResult Index()
        {
            return View();
        }

        [OnlyAjaxRequestAttribute]
        public PartialViewResult Publicaciones() 
        {
            ViewBag.Usuario_id = SessionHelper.GetUser();
            return PartialView(pm.Listar());
        }
    }
}
