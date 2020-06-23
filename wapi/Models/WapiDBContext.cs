using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using wapi.Models;

namespace wapi.Models
{
    public partial class WapiDBContext : DbContext
    {
       

        public WapiDBContext()
        {
        }

        public WapiDBContext(DbContextOptions<WapiDBContext> options)
            : base(options)
        {
        }

        /*public virtual DbSet<Kbarticle> Kbarticle { get; set; }*/
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        /*public IEnumerable<Comment> Comments;*/
        public virtual DbSet<Kbarticle> Kbarticle { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("MSSQLLocalDB;Initial Catalog=WapiDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Kbarticle>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Content).HasColumnType("text");

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ProductType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Rolename)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Roletype)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Entry).HasColumnType("date");

                entity.Property(e => e.Reportsto).HasColumnName("reportsto");

                entity.Property(e => e.Fname)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Pname)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });
        }

        public DbSet<wapi.Models.Elearning> Elearning { get; set; }

        public DbSet<wapi.Models.userdoc> userdoc { get; set; }

        public DbSet<wapi.Models.Whatsthat> Whatsthat { get; set; }

        public DbSet<wapi.Models.Topic> Topic { get; set; }

        public DbSet<wapi.Models.Globals> Globals { get; set; }
    }
}
