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
    public class PublicacionModel
    {
        private ResponseModel rm = new ResponseModel();

        public List<Publicacion> Listar(int usuario_id = 0)
        {
            List<Publicacion> publicaciones = new List<Publicacion>();

            using (var context = new RedSocialContext())
            {
                try
                {
                    // Traemos las ultimoas 20 publicaciones
                    if (usuario_id == 0) // Si queremos traer todas las publicaciones
                    {
                        publicaciones = context.Publicacion
                                               .OrderByDescending(x => x.FechaRegistro)
                                               .Take(30).ToList(); //Take equivale a SELECT TOP 30
                    }
                    else // Si queremos traer las publicaciones de u nusuario especifico
                    {
                        publicaciones = context.Publicacion
                                               .OrderByDescending(x => x.FechaRegistro)
                                               .Where( x => x.Para == usuario_id)
                                               .Take(30).ToList(); //Take equivale a SELECT TOP 30
                    }

                    foreach (var p in publicaciones) 
                    {
                        // Obtenemos el emisor y su foto
                        p.Emisor = context.Usuario
                                          .Where(x => x.id == p.De).SingleOrDefault();

                        p.Emisor.Foto = context.Foto
                                          .Where(x => x.Relacion ==  "U" + p.De).SingleOrDefault();


                        if (p.Emisor.Foto == null) p.Emisor.Foto = new Foto(); // Si no tiene foto agregamos una por defecto

                        // Obtenemos el receptor y su foto
                        p.Receptor = context.Usuario
                                          .Where(x => x.id == p.Para).SingleOrDefault();

                        p.Receptor.Foto = context.Foto
                                          .Where(x => x.Relacion ==  "U" + p.Para).SingleOrDefault();

                        if (p.Receptor.Foto == null) p.Receptor.Foto = new Foto(); // Si no tiene foto agregamos una por defecto
                    }
                }
                catch (Exception e)
                {
                    ELog.Save(this, e);
                }
            }

            return publicaciones;
        }

        public ResponseModel Registrar(Publicacion publicacion)
        {
            using (var context = new RedSocialContext())
            {
                try
                {
                    // Llenamos los valores que faltan
                    publicacion.FechaRegistro = ViewHelper.getDate(true);

                    context.Publicacion.Add(publicacion);
                    context.SaveChanges();

                    rm.SetResponse(true);
                }
                catch (Exception e)
                {
                    ELog.Save(this, e);
                }
            }

            return rm;
        }
    }
}
