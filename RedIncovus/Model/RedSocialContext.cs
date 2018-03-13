namespace Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using Entity;

    public partial class RedSocialContext : DbContext
    {
        public RedSocialContext()
            : base("name=RedSocialContext")
        {
        }
        public DbSet<Conocimiento> Conocimientos { get; set; }
        public DbSet<UsuarioConocimiento> UsuarioConocimiento { get; set; }
        public DbSet<Foto> Foto { get; set; }
        public DbSet<Publicacion> Publicacion { get; set; }
        public DbSet<Usuario> Usuario { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>();
        }
    }
}
