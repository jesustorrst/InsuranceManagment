using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;


namespace Data
{

    public class InsuranceManagmentDbContextFactory : IDesignTimeDbContextFactory<InsuranceManagmentDbContext>
    {
        public InsuranceManagmentDbContext CreateDbContext(string[] args)
        {
            string workingDirectory = Directory.GetCurrentDirectory();
            DirectoryInfo rootDirectory = new DirectoryInfo(workingDirectory);

            while (rootDirectory != null && rootDirectory.Name != "InsuranceManagment")
            {
                rootDirectory = rootDirectory.Parent;
            }

            if (rootDirectory == null)
                throw new Exception("No se pudo encontrar la carpeta raíz 'InsuranceManagment'.");

            string configPath = Path.Combine(rootDirectory.FullName, "PL", "PL.Server");

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(configPath)
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<InsuranceManagmentDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);

            return new InsuranceManagmentDbContext(optionsBuilder.Options);
        }
    }




    public class InsuranceManagmentDbContext : DbContext
    {
        public InsuranceManagmentDbContext(DbContextOptions<InsuranceManagmentDbContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Entities.Cliente> Clientes { get; set; }
        public DbSet<Models.Entities.Usuario> Usuarios { get; set; }
        public DbSet<Models.Entities.Rol> Roles { get; set; }
        public DbSet<Models.Entities.Poliza> Polizas { get; set; }
        public DbSet<Models.Entities.TipoPoliza> TiposPoliza { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigurarRol(modelBuilder);
            ConfigurarUsuario(modelBuilder);
            ConfigurarCliente(modelBuilder);
            ConfigurarTipoPoliza(modelBuilder);
            ConfigurarPoliza(modelBuilder);

        }


        private void ConfigurarCliente(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Entities.Cliente>(entity =>
            {
                entity.HasKey(e => e.IdCliente);

                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ApellidoPaterno).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);

                entity.HasOne(d => d.UsuarioCreador)
                    .WithMany()
                    .HasForeignKey(d => d.CreadoPor)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.UsuarioActualizador)
                    .WithMany()
                    .HasForeignKey(d => d.ActualizadoPor)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.Polizas)
                      .WithOne(p => p.Cliente)
                      .HasForeignKey(p => p.IdCliente);
            });
        }
        private void ConfigurarRol(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Entities.Rol>(entity =>
            {
                entity.HasKey(e => e.IdRol);

                entity.Property(e => e.Nombre)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.HasIndex(e => e.Nombre).IsUnique();
            });
        }


        private void ConfigurarUsuario(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Entities.Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
                entity.HasIndex(e => e.Email).IsUnique();

                // Relación con Rol
                entity.HasOne(e => e.Rol)
                      .WithMany(r => r.Usuarios)
                      .HasForeignKey(e => e.IdRol);

                // Relación con Cliente: Un usuario PUEDE ser un cliente
                entity.HasOne(e => e.Cliente)
                      .WithMany() // Si un cliente solo tiene un usuario, puede ser WithOne
                      .HasForeignKey(e => e.IdCliente)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
        private void ConfigurarTipoPoliza(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Entities.TipoPoliza>(entity =>
            {
                entity.HasKey(e => e.IdTipoPoliza);

                entity.Property(e => e.Nombre)
                      .IsRequired()
                      .HasMaxLength(100);
            });
        }

        private void ConfigurarPoliza(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Entities.Poliza>(entity =>
            {
                entity.HasKey(e => e.IdPoliza);

                entity.Property(e => e.MontoAsegurado)
                      .HasColumnType("decimal(18,2)");

                entity.HasOne(d => d.UsuarioCreador)
                    .WithMany()
                    .HasForeignKey(d => d.CreadoPor)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.UsuarioActualizador)
                    .WithMany()
                    .HasForeignKey(d => d.ActualizadoPor)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Cliente)
                    .WithMany(p => p.Polizas)
                    .HasForeignKey(d => d.IdCliente);

                entity.HasOne(d => d.TipoPoliza)
                    .WithMany(p => p.Polizas)
                    .HasForeignKey(d => d.IdTipoPoliza);

            });
        }

    }

}
