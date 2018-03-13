using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("UsuarioConocimiento")]
    public class UsuarioConocimiento
    {
        // Es una clave compuesta le indicamos el ordgen y la tabla de referencia
        [Key, Column(Order = 0), ForeignKey("Conocimiento")]
        public int Conocimiento_id { get; set; }
        public Conocimiento Conocimiento { get; set; }

        // Es una clave compuesta le indicamos el ordgen y la tabla de referencia
        [Key, Column(Order = 1), ForeignKey("Usuario")]
        public int Usuario_id { get; set; }
        public Usuario Usuario { get; set; }

    }
}
