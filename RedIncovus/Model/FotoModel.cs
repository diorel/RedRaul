using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


using Entity;
using Helper;

namespace Model
{
    public class FotoModel
    {
        private ResponseModel rm = new ResponseModel();

        public static void Registrar(RedSocialContext context, HttpPostedFileBase file, int[] medidas, string FotoRelacion)
        {
            // Aplicamos la logica para crear los thumbnails
            ImageHelper imghelper = new ImageHelper();
            string nombre = ViewHelper.getNameForFiles() + Path.GetExtension(file.FileName);
            string ruta = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/" + nombre);

            file.SaveAs(ruta);

            imghelper.Path = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/");
            imghelper.Img = nombre;
            imghelper.Scales = medidas;
            imghelper.resizes();

            List<string> imagenes = imghelper.getNewImages();

            Foto Foto = new Foto
            {
                Foto1 = imagenes[0],
                Foto2 = imagenes[1],
                Foto3 = imagenes[2],
                Relacion = FotoRelacion,
                FechaRegistro = ViewHelper.getDate(true)
            };

            context.Database.ExecuteSqlCommand("DELETE FROM Foto WHERE relacion = @r", new SqlParameter("r", FotoRelacion));
            context.Entry(Foto).State = EntityState.Added;
            }
        }
}
