using AppCocacolaNayMobiV6.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppCocacolaNayMobiV6.Data
{
    public class FicBDContext : DbContext
    {
        private readonly string FicDataBasePath;

        public FicBDContext(string FicPaDataBasePath)
        {
            FicDataBasePath = FicPaDataBasePath;
            FicMetCrearBD();
        }//CONSTRUCTOR

        private async void FicMetCrearBD()
        {
            try
            {
                await Database.EnsureCreatedAsync();
            }
            catch(Exception e)
            {
                await new Page().DisplayAlert("ALERTA", e.Message.ToString(), "OK");
            }

        }//ESTE METODO CREA LA BASE DE DATOS

        protected async override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            try
            {
                optionsBuilder.UseSqlite($"Filename={FicDataBasePath}");
                optionsBuilder.EnableSensitiveDataLogging();
                
            }
            catch (Exception e)
            {
                await new Page().DisplayAlert("ALERTA", e.Message.ToString(), "OK");
            }
        }//CONFIGURACION DE LA CONEXION

        //Gestion de Inventarios
        public DbSet<zt_inventarios> zt_inventarios { get; set; }
        public DbSet<zt_inventarios_acumulados> zt_inventarios_acumulados { get; set; }
        public DbSet<zt_inventarios_conteos> zt_inventarios_conteos { get; set; }
        //Catalogos Unidades de Medida
        public DbSet<zt_cat_productos> zt_cat_productos { get; set; }
        public DbSet<zt_cat_unidad_medidas> zt_cat_unidad_medidas { get; set; }
        public DbSet<zt_cat_productos_medidas> zt_cat_productos_medidas { get; set; }
        //Catalogo CEDIS y Almacenes
        public DbSet<zt_cat_cedis> zt_cat_cedis { get; set; }
        public DbSet<zt_cat_almacenes> zt_cat_almacenes { get; set; }


        protected async override void OnModelCreating(ModelBuilder modelBuilder)
        {
            try
            {
                /*CREACION DE LLAVES PRIMARIAS*/
                modelBuilder.Entity<zt_cat_cedis>()
                    .HasKey(c => new { c.IdCEDI });

                modelBuilder.Entity<zt_cat_almacenes>()
                    .HasKey(c => new { c.IdAlmacen });

                modelBuilder.Entity<zt_cat_productos>()
                    .HasKey(c => new { c.IdSKU });

                modelBuilder.Entity<zt_cat_unidad_medidas>()
                    .HasKey(c => new { c.IdUnidadMedida });

                modelBuilder.Entity<zt_inventarios>()
                    .HasKey(c => new { c.IdInventario });

                modelBuilder.Entity<zt_inventarios_conteos>()
                    .HasKey(c => new { c.NumConteo, c.IdInventario, c.IdAlmacen, c.IdSKU, c.IdUnidadMedida });

                modelBuilder.Entity<zt_inventarios_conteos>().HasIndex(x => x.NumConteo).IsUnique(false);

                modelBuilder.Entity<zt_cat_productos_medidas>()
                    .HasKey(c => new { c.IdSKU, c.IdUnidadMedida });

                modelBuilder.Entity<zt_inventarios_acumulados>()
                   .HasKey(c => new { c.IdInventario, c.IdSKU, c.IdUnidadMedida });

                /*CREACION DE LLAVES FORANEAS*/
                modelBuilder.Entity<zt_cat_almacenes>()
                .HasOne(s => s.zt_cat_cedis).
                WithMany().HasForeignKey(s => new { s.IdCEDI });

                modelBuilder.Entity<zt_inventarios>()
                .HasOne(s => s.zt_cat_cedis).
                WithMany().HasForeignKey(s => new { s.IdCEDI });

                modelBuilder.Entity<zt_inventarios_conteos>()
                .HasOne(s => s.zt_inventarios).
                WithMany().HasForeignKey(s => new { s.IdInventario });

                modelBuilder.Entity<zt_inventarios_conteos>()
                .HasOne(s => s.zt_cat_almacenes).
                WithMany().HasForeignKey(s => new { s.IdAlmacen });

                modelBuilder.Entity<zt_inventarios_conteos>()
                .HasOne(s => s.zt_cat_productos).
                WithMany().HasForeignKey(s => new { s.IdSKU });

                modelBuilder.Entity<zt_inventarios_conteos>()
                .HasOne(s => s.zt_cat_unidad_medidas).
                WithMany().HasForeignKey(s => new { s.IdUnidadMedida });

                modelBuilder.Entity<zt_cat_productos_medidas>()
                .HasOne(s => s.zt_cat_productos).
                WithMany().HasForeignKey(s => new { s.IdSKU });

                modelBuilder.Entity<zt_cat_productos_medidas>()
                .HasOne(s => s.zt_cat_unidad_medidas).
                WithMany().HasForeignKey(s => new { s.IdUnidadMedida });

                modelBuilder.Entity<zt_inventarios_acumulados>()
                .HasOne(s => s.zt_inventarios).
                WithMany().HasForeignKey(s => new { s.IdInventario });

                modelBuilder.Entity<zt_inventarios_acumulados>()

                .HasOne(s => s.zt_cat_productos).
                WithMany().HasForeignKey(s => new { s.IdSKU });

                modelBuilder.Entity<zt_inventarios_acumulados>()
                .HasOne(s => s.zt_cat_unidad_medidas).
                WithMany().HasForeignKey(s => new { s.IdUnidadMedida });
            }
            catch (Exception e)
            {
                await new Page().DisplayAlert("ALERTA", e.Message.ToString(), "OK");
            }

        }//AL CREAR EL MODELO

    }//CLASS
}//NAMESPACE
