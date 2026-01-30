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
            // Lógica robusta para encontrar la raíz
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

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Poliza> Polizas { get; set; }
        public DbSet<TipoPoliza> TiposPoliza { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigurarCliente(modelBuilder);
            ConfigurarUsuario(modelBuilder);
            ConfigurarRol(modelBuilder);
            ConfigurarPoliza(modelBuilder);
            ConfigurarTipoPoliza(modelBuilder);
        }

        private void ConfigurarCliente(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.IdCliente);

                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ApellidoPaterno).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ApellidoMaterno).HasMaxLength(100);

                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
                entity.Property(e => e.NumeroIdentificacion).IsRequired().HasMaxLength(20);

                entity.Property(e => e.Foto).HasColumnType("varbinary(max)");

                entity.HasMany(e => e.Polizas)
                      .WithOne(p => p.Cliente)
                      .HasForeignKey(p => p.IdCliente);
            });
        }

        private void ConfigurarRol(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rol>(entity =>
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
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario);

                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
                entity.HasIndex(e => e.Email).IsUnique();

                entity.HasOne(e => e.Rol)
                      .WithMany(r => r.Usuarios)
                      .HasForeignKey(e => e.IdRol);

                entity.HasOne(e => e.Cliente)
                      .WithMany()
                      .HasForeignKey(e => e.IdCliente)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private void ConfigurarTipoPoliza(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TipoPoliza>(entity =>
            {
                entity.HasKey(e => e.IdTipoPoliza);

                entity.Property(e => e.Nombre)
                      .IsRequired()
                      .HasMaxLength(100);
            });
        }

        private void ConfigurarPoliza(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Poliza>(entity =>
            {
                entity.HasKey(e => e.IdPoliza);

                entity.Property(e => e.MontoAsegurado)
                      .HasColumnType("decimal(18,2)");

                entity.HasOne(e => e.Cliente)
                      .WithMany(c => c.Polizas)
                      .HasForeignKey(e => e.IdCliente);

                entity.HasOne(e => e.TipoPoliza)
                      .WithMany(tp => tp.Polizas)
                      .HasForeignKey(e => e.IdTipoPoliza);
            });
        }

    }

}
