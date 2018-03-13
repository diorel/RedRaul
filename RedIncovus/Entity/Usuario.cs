namespace Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("Usuario")]
    public  class Usuario
    {
        public int id { get; set; }

        public byte Admin { get; set; }

        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Correo { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "Debe ser mayor a 4 caracteres y menor a 10.")]
        public string Contrasena { get; set; }

        [Required]
        [StringLength(20)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50)]
        public string Apellido { get; set; }

        public byte? Sexo { get; set; }

        [StringLength(10)]
        public string FechaNacimiento { get; set; }

        public List<UsuarioConocimiento> UsuarioConocimientos { get; set; }

        [NotMapped]
        /* Le inidica al entity framework que este campo no va a ser considerando,
         * ya que supongamos que hagamos un insert este poria crear el insert incluyendo 
         * a la propiedad Foto como campo, cosa que no es asi
         */
        public Foto Foto { get; set; }

        [StringLength(200)]
        public string Url { get; set; }


    }
}
