using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Helper;

namespace Model
{
    public class ConocimientoModel
    {
        private ResponseModel rm = new ResponseModel();

        public List<Conocimiento> Listar()
        {
            List<Conocimiento> conocimientos = new List<Conocimiento>();

            try
            {
                using (var context = new RedSocialContext())
                {
                    // Otra forma de hacer Query, usando Linq
                    conocimientos = (from c in context.Conocimientos
                                     orderby c.Nombre
                                     select c).ToList();
                }
            }
            catch (Exception e)
            {
                ELog.Save(this, e);
            }

            return conocimientos;
        }
    }
}
