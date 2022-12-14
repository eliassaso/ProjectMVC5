using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProjectMVC5.Models
{
    public class DB_Entities : DbContext
    {
        public DB_Entities() : base("Db_website") { }
        public DbSet<User> Users { get; set; }

        public DbSet<Roles> Roles { get; set; }
        public DbSet<Association> AsociacionPerfil { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Database.SetInitializer<demoEntities>(null);
            modelBuilder.Entity<Roles>().ToTable("Roles");
            modelBuilder.Entity<Association>().ToTable("AsociacionPerfil");
            modelBuilder.Entity<User>().ToTable("Users");
            
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            base.OnModelCreating(modelBuilder);


        }
    }
}