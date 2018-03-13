using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class ReportePublicacionesPorUsuario
    {
        public int id { get; set; }
        public string Nombre { get; set; }
        public byte Sexo { get; set; }
        public string FechaNacimiento { get; set; }
        public int Publicaciones { get; set; }
    }
}
