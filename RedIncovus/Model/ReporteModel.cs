using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Entity;
using Helper;

namespace Model
{
    public class ReporteModel
    {
        public List<ReportePublicacionesPorUsuario> ReportePublicacionesPorUsuario() 
        {
            var reporte = new List<ReportePublicacionesPorUsuario>();

            using (var ctx = new RedSocialContext())
            {
                try
                {
                    reporte = ctx.Database
                                .SqlQuery<ReportePublicacionesPorUsuario>("[USP_ReportePublicacionesPorUsuario]")
                                .ToList();
                }
                catch (Exception e)
                {
                    ELog.Save(this, e);
                }
            }

            return reporte;
        } 
    }
}
