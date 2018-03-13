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
    public class UsuarioModel
    {
        private ResponseModel rm = new ResponseModel();
        private string FotoRelacion = "U";

        public List<Usuario> Listar(int take, int skip)
        {
            List<Usuario> Usuarios = new List<Usuario>();

            using (var context = new RedSocialContext())
            {
                try
                {
                    Usuarios = context.Usuario
                               .OrderBy(x => x.Nombre)
                               .Skip(take * skip)
                               .Take(take)
                               .ToList();

                    // Debemos agregar sus fotos
                    foreach (var u in Usuarios)
                    {
                        // Ahora obtenemos su foto
                        u.Foto = context.Foto.Where(
                                x => x.Relacion == FotoRelacion + u.id).SingleOrDefault();

                        // Si la foto no existe le ponemos una por defecto
                        if (u.Foto == null)
                        {
                            u.Foto = new Foto();
                        }

                        // Ahora obtenemos los conocimientos de nuestro usuario
                        u.UsuarioConocimientos = context.UsuarioConocimiento
                                                      .Where(x => x.Usuario_id == u.id)
                                                      .ToList();
                    }
                }
                catch (Exception e)
                {
                    ELog.Save(this, e);
                }
            }
            return Usuarios;
        }
        public int Total()
        {
            int t = 0;

            using (var context = new RedSocialContext())
            {
                try
                {
                    t = context.Usuario
                               .OrderBy(x => x.Nombre)
                               .Count();
                }
                catch (Exception e)
                {
                    ELog.Save(this, e);
                }
            }
            return t;
        }

        public List<Usuario> ListarAlAzar()
        {
            List<Usuario> Usuarios = new List<Usuario>();

            using (var context = new RedSocialContext()) 
            {
                try
                {
                    Usuarios = context.Usuario.
                               OrderBy(x => Guid.NewGuid()).
                               Take(10).ToList();

                    // Debemos agregar sus fotos
                    foreach (var u in Usuarios) 
                    {
                        // Ahora obtenemos su foto
                        u.Foto = context.Foto.Where(
                                x => x.Relacion == FotoRelacion + u.id).SingleOrDefault();

                        // Si la foto no existe le ponemos una por defecto
                        if (u.Foto == null)
                        {
                            u.Foto = new Foto();
                        }

                        // Ahora obtenemos los conocimientos de nuestro usuario
                        u.UsuarioConocimientos = context.UsuarioConocimiento
                                                      .Where(x => x.Usuario_id == u.id)
                                                      .ToList();
                    }
                }
                catch (Exception e)
                {
                    ELog.Save(this, e);
                }
            }
            return Usuarios;
        }

        public Usuario Obtener(int id)
        {
            Usuario usuario = new Usuario();

            using (var context = new RedSocialContext())
            {
                try
                {
                    usuario = context.Usuario
                        .Where( x => x.id == id).Single();

                    // Damos formato a la fecha en yyyy/mm/dd
                    usuario.FechaNacimiento = ViewHelper.ConvertToDate(usuario.FechaNacimiento);

                    // Ahora obtenemos su foto
                    usuario.Foto = context.Foto.Where(
                            x => x.Relacion == FotoRelacion + usuario.id).SingleOrDefault();

                    // Si la foto no existe le ponemos una por defecto
                    if (usuario.Foto == null) 
                    {
                        usuario.Foto = new Foto();
                    }

                    /* Ahora obtenemos los conocimientos de nuestro usuario, pero vamos a traerlos de la tabla conocimiento
                     * para esto haremos un LINQ INNER JOIN */
                    usuario.UsuarioConocimientos = (from c in context.Conocimientos.ToList()
                                             join cu in context.UsuarioConocimiento.ToList()
                                             on c.id equals cu.Conocimiento_id
                                             where cu.Usuario_id == usuario.id
                                             orderby c.Nombre select cu).ToList();

                    /* Como en nuestra entidad UsuarioConocimiento le hemos indicado que son llaves foraneas, el entity framework
                     * ha llegando los objetos Usuario y Conocimiento de esta clase automaticamente */
                }
                catch (Exception e)
                {
                    ELog.Save(this, e);
                }
            }

            return usuario;
        }

        /// <summary>
        /// Este método es usado para registrar la información básica del usuario, autentificar y redireccionar al home de nuestro aplicativo.
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public ResponseModel RegistroDeLogin(Usuario usuario)
        {
            using (var context = new RedSocialContext())
            {
                try
                {
                    // Guardamos la clave en formato MD5
                    usuario.Contrasena = HashHelper.MD5(usuario.Contrasena);

                    // Ignoramos la validacion para que la contraseña pase
                    context.Configuration.ValidateOnSaveEnabled = false;

                    context.Usuario.Add(usuario);
                    context.SaveChanges();

                    context.Database.ExecuteSqlCommand("UPDATE Usuario SET Url = @url WHERE id = @id", 
                        new SqlParameter("url", ViewHelper.ConvertNameToUrl(usuario.Nombre, usuario.Apellido, usuario.id.ToString())),
                        new SqlParameter("id", usuario.id));

                    // Guardamos en session al usuario actual, solo necesitamos guardar su ID
                    SessionHelper.AddUserToSession(usuario.id.ToString());

                    rm.SetResponse(true);
                    rm.href = "inicio";
                }
                catch(Exception e)
                {
                    ELog.Save(this, e);
                }
            }

            return rm;
        }

        public ResponseModel Actualizar(Usuario usuario, HttpPostedFileBase file)
        {
            using (var context = new RedSocialContext())
            {
                try
                {
                    if (file != null)
                    {
                        // Este es un helper que yo cree para crear copias de imagenes y validar, no existe en el marco de ASP.NET MVC
                        var rpta = ImageHelper.TryParse(file, 500);

                        if (rpta != "")
                        {
                            rm.SetResponse(false, rpta);

                            // Lanzamos una exception en caso que la imagen no sea válida
                            throw new Exception(rpta);
                        }

                        /* Ahora debemos guardar la foto, agregando el ID del usuario al campo Relacion de la tabla Foto
                         * Lo que hice fue crear un Modelo que implemente la logica de guardar una foto en la base de datos
                         * y crear copia usando el Helper que yo creo para Imagenes */

                        FotoModel.Registrar(context, file, new int[] { 500, 300, 100 }, FotoRelacion + usuario.id);
                    }

                    // Quitamos la validacion
                    context.Configuration.ValidateOnSaveEnabled = false;

                    // Registramos la entidad
                    var ctxUsuario = context.Entry(usuario);

                    // Le indicamos que es del tipo Update
                    ctxUsuario.State = EntityState.Modified;

                    // Seteamos la fecha al formato YYYY/MM/DD
                    usuario.FechaNacimiento = ViewHelper.ConvertToDate(usuario.FechaNacimiento);

                    // Url del Usuario
                    usuario.Url = ViewHelper.ConvertNameToUrl(usuario.Nombre, usuario.Apellido, usuario.id.ToString());

                    // Campos que no queremos que toque
                    ctxUsuario.Property(x => x.Admin).IsModified = false;

                    if (usuario.Contrasena == null) ctxUsuario.Property(x => x.Contrasena).IsModified = false; // Retiramos contrasena de la actualizacion
                    else usuario.Contrasena = HashHelper.MD5(usuario.Contrasena); // Si la contraseña ha sido cambiada la actualizamos a MD5

                    /* Antes de agregar los conocimientos debemos borrar los conocimientos que ya tenga este usuario
                        * para evitar registros duplicados. Si seguimos el esquema de entity framework, primero tendriamos
                        * que traer todo los conocimientos que tiene este usuario e indificarle que los elimine. Pero
                        * esto no me parece muy optimo, ya que si tuviera 1000 conocimientos, hariamos demasiados querys */

                    // Asi que la solucion es hacer un query manual
                    context.Database.ExecuteSqlCommand("DELETE FROM UsuarioConocimiento WHERE usuario_id = @usuario_id",
                                                        new SqlParameter("usuario_id", usuario.id));

                    // Agregamos los conocimientos
                    if (usuario.UsuarioConocimientos != null)
                    {
                        // Indicamos que el estado es del tipo insert
                        foreach (var c in usuario.UsuarioConocimientos)
                        {
                            context.Entry(c).State = EntityState.Added;
                        }
                    }

                    // Grabamos
                    context.SaveChanges();

                    rm.SetResponse(true);
                    if (file != null) rm.href = "self";
                }
                catch (Exception e)
                {
                    ELog.Save(this, e);
                }
            }

            return rm;
        }

        public ResponseModel Eliminar(int id)
        {
            using (var context = new RedSocialContext())
            {
                try
                {
                    context.Entry(new Usuario { id = id }).State = EntityState.Deleted;
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

        public ResponseModel Acceder(Usuario usuario) 
        {
            using (var context = new RedSocialContext())
            {
                try
                {
                    // Encriptamos la contraseña para comparar
                    usuario.Contrasena = HashHelper.MD5(usuario.Contrasena);

                    var _usuario = context.Usuario
                                .Where(x =>
                                    x.Contrasena == usuario.Contrasena &&
                                    x.Correo == usuario.Correo
                                    ).SingleOrDefault();

                    if (_usuario != null)
                    {
                        SessionHelper.AddUserToSession(_usuario.id.ToString());

                        rm.SetResponse(true);
                        rm.href = "inicio";
                    }
                    else 
                    {
                        rm.SetResponse(false, "El usuario ingresado no existe");
                    }
                }
                catch (Exception e)
                {
                    ELog.Save(this, e);
                }
            }

            return rm;
        }

        public bool EsAdmin(int usuario_id)
        {
            var EsAdmin = false;

            using (var context = new RedSocialContext())
            {
                try
                {
                    var u = context.Usuario
                                .Where(x => x.id == usuario_id && x.Admin == 1)
                                .FirstOrDefault();

                    if (u != null) EsAdmin = true;
                }
                catch (Exception e)
                {
                    ELog.Save(this, e);
                }
            }

            return EsAdmin;
        }
    }
}
