using AppCocacolaNayMobiV6.Models;
using AppCocacolaNayMobiV6.Models.Eva;
using Microsoft.EntityFrameworkCore;
using System;
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
        public DbSet<zt_cat_estatus> zt_cat_estatus { get; set; }
        public DbSet<zt_inventarios> zt_inventarios { get; set; }
        public DbSet<zt_inventarios_acumulados> zt_inventarios_acumulados { get; set; }
        public DbSet<zt_inventarios_conteos> zt_inventarios_conteos { get; set; }
        //Catalogos Unidades de Medida
        public DbSet<zt_cat_grupos_sku> zt_cat_grupos_sku { get; set; }
        public DbSet<zt_cat_productos> zt_cat_productos { get; set; }
        public DbSet<zt_cat_unidad_medidas> zt_cat_unidad_medidas { get; set; }
        public DbSet<zt_cat_productos_medidas> zt_cat_productos_medidas { get; set; }
        //Catalogo CEDIS y Almacenes
        public DbSet<zt_cat_cedis> zt_cat_cedis { get; set; }
        public DbSet<zt_cat_almacenes> zt_cat_almacenes { get; set; }
        public DbSet<zt_cat_ubicaciones> zt_cat_ubicaciones { get; set; }
        public DbSet<zt_almacenes_ubicaciones> zt_almacenes_ubicaciones { get; set; }

        /*PAGINACION*/
        //public DbSet<seg_cat_modulos> seg_cat_modulos { get; set; }
        //public DbSet<seg_cat_submodulos> seg_cat_submodulos { get; set; }
        //public DbSet<seg_cat_paginas> seg_cat_paginas { get; set; }

        /*SEGURIDAD*/
        //public DbSet<cat_usuarios> cat_usuarios { get; set; }
        //public DbSet<seg_expira_claves> seg_expira_claves { get; set; }
        //public DbSet<seg_usuarios_estatus> seg_usuarios_estatus { get; set; }
        //public DbSet<cat_tipos_estatus> cat_tipos_estatus { get; set; }
        //public DbSet<cat_estatus> cat_estatus { get; set; }

        protected async override void OnModelCreating(ModelBuilder modelBuilder)
        {
            try
            {
                #region INVENTARIOS
                /*CREACION DE LLAVES PRIMARIAS*/
                modelBuilder.Entity<zt_cat_cedis>()
                    .HasKey(c => new { c.IdCEDI });

                modelBuilder.Entity<zt_cat_almacenes>()
                    .HasKey(c => new { c.IdAlmacen });

                modelBuilder.Entity<zt_cat_grupos_sku>()
                    .HasKey(c => new { c.IdGrupoSKU });

                modelBuilder.Entity<zt_cat_productos>()
                    .HasKey(c => new { c.IdSKU });

                modelBuilder.Entity<zt_cat_unidad_medidas>()
                    .HasKey(c => new { c.IdUnidadMedida });

                modelBuilder.Entity<zt_almacenes_ubicaciones>()
                    .HasKey(c => new { c.IdAlmacen, c.IdUbicacion });

                modelBuilder.Entity<zt_cat_estatus>()
                    .HasKey(c => new { c.IdEstatus });

                modelBuilder.Entity<zt_inventarios>()
                    .HasKey(c => new { c.IdInventario});

                modelBuilder.Entity<zt_cat_ubicaciones>()
                    .HasKey(c => new { c.IdUbicacion });

                modelBuilder.Entity<zt_inventarios_conteos>()
                    .HasKey(c => new { c.NumConteo, c.IdInventario, c.IdAlmacen, c.IdSKU, c.IdUnidadMedida, c.IdUbicacion });

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

                modelBuilder.Entity<zt_inventarios>()
                .HasOne(s => s.zt_cat_almacenes).
                WithMany().HasForeignKey(s => new { s.IdAlmacen });

                modelBuilder.Entity<zt_inventarios>()
                .HasOne(s => s.zt_cat_estatus).
                WithMany().HasForeignKey(s => new { s.IdEstatus});

                modelBuilder.Entity<zt_cat_productos>()
                .HasOne(s => s.zt_cat_grupos_sku).
                WithMany().HasForeignKey(s => new { s.IdGrupoSKU });

                modelBuilder.Entity<zt_cat_productos>()
                .HasOne(s => s.zt_cat_unidad_medidas).
                WithMany().HasForeignKey(s => new { s.IdUMedidaBase });

                modelBuilder.Entity<zt_almacenes_ubicaciones>()
                .HasOne(s => s.zt_cat_almacenes).
                WithMany().HasForeignKey(s => new { s.IdAlmacen });

                modelBuilder.Entity<zt_almacenes_ubicaciones>()
                .HasOne(s => s.zt_cat_ubicaciones).
                WithMany().HasForeignKey(s => new { s.IdUbicacion });

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

                modelBuilder.Entity<zt_inventarios_conteos>()
                .HasOne(s => s.zt_cat_ubicaciones).
                WithMany().HasForeignKey(s => new { s.IdUbicacion });

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
                #endregion

                //#region PAGINACION 
                ///*PAGINACION*/
                //modelBuilder.Entity<seg_cat_modulos>()
                //    .HasKey(c => new { c.IdModulo });

                //modelBuilder.Entity<seg_cat_submodulos>()
                //    .HasKey(c => new { c.IdSubmodulo });

                //modelBuilder.Entity<seg_cat_submodulos>()
                //.HasOne(s => s.seg_cat_modulos).
                //WithMany().HasForeignKey(s => new { s.IdModulo });

                //modelBuilder.Entity<seg_cat_paginas>()
                //    .HasKey(c => new { c.IdPagina, c.IdModulo, c.IdSubmodulo });

                //modelBuilder.Entity<seg_cat_paginas>()
                //.HasOne(s => s.seg_cat_modulos).
                //WithMany().HasForeignKey(s => new { s.IdModulo });

                //modelBuilder.Entity<seg_cat_paginas>()
                //.HasOne(s => s.seg_cat_submodulos).
                //WithMany().HasForeignKey(s => new { s.IdSubmodulo });
                //#endregion

               // #region EVA
               // //CAT_TIPOS_GENERALES
               // modelBuilder.Entity<cat_tipos_generales>()
               //     .HasKey(c => new { c.IdTipoGeneral });

               // //CAT_GENERALES
               // modelBuilder.Entity<cat_generales>()
               //     .HasKey(c => new { c.IdGeneral });

               // modelBuilder.Entity<cat_generales>()
               // .HasOne(s => s.cat_tipos_generales).
               // WithMany().HasForeignKey(s => new { s.IdTipoGeneral });

               // //CAT_INSTITUTOS
               // modelBuilder.Entity<cat_institutos>()
               //     .HasKey(c => new { c.IdInstituto });

               // modelBuilder.Entity<cat_institutos>()
               // .HasOne(s => s.cat_institutos_padre).
               // WithMany().HasForeignKey(s => new { s.IdInstitutoPadre });

               // modelBuilder.Entity<cat_institutos>()
               // .HasOne(s => s.cat_tipos_generales).
               // WithMany().HasForeignKey(s => new { s.IdTipoGenGiro });

               // modelBuilder.Entity<cat_institutos>()
               //.HasOne(s => s.cat_generales).
               //WithMany().HasForeignKey(s => new { s.IdGenGiro });

               // //RH_CAT_PERSONAS
               // modelBuilder.Entity<rh_cat_personas>()
               //     .HasKey(c => new { c.IdPersona });

               // modelBuilder.Entity<rh_cat_personas>()
               // .HasOne(s => s.cat_institutos).
               // WithMany().HasForeignKey(s => new { s.IdInstituto });

               // modelBuilder.Entity<rh_cat_personas>()
               // .HasOne(s => s.cat_tipos_generales).
               // WithMany().HasForeignKey(s => new { s.IdTipoGenOcupacion, s.IdTipoGenEstadoCivil });

               // modelBuilder.Entity<rh_cat_personas>()
               // .HasOne(s => s.cat_generales).
               // WithMany().HasForeignKey(s => new { s.IdGenOcupacion, s.IdGenEstadoCivil });

               // //CAT_USUARIOS
               // modelBuilder.Entity<cat_usuarios>()
               //     .HasKey(c => new { c.IdUsuario });

               // modelBuilder.Entity<cat_usuarios>()
               // .HasOne(s => s.rh_cat_personas).
               // WithMany().HasForeignKey(s => new { s.IdPersona });

               // //RH_CAT_DIR_WEB
               // modelBuilder.Entity<rh_cat_dir_web>()
               //     .HasKey(c => new { c.IdDirWeb });

               // modelBuilder.Entity<rh_cat_dir_web>()
               // .HasOne(s => s.cat_tipos_generales).
               // WithMany().HasForeignKey(s => new { s.IdTipoGenDirWeb });

               // modelBuilder.Entity<rh_cat_dir_web>()
               // .HasOne(s => s.cat_generales).
               // WithMany().HasForeignKey(s => new { s.IdGenDirWeb });

               // //RH_CAT_DOMICILIOS
               // modelBuilder.Entity<rh_cat_domicilios>()
               //     .HasKey(c => new { c.IdDomicilio});

               // modelBuilder.Entity<rh_cat_domicilios>()
               // .HasOne(s => s.cat_tipos_generales).
               // WithMany().HasForeignKey(s => new { s.IdTipoGenDom});

               // modelBuilder.Entity<rh_cat_domicilios>()
               // .HasOne(s => s.cat_generales).
               // WithMany().HasForeignKey(s => new { s.IdGenDom });

               // //RH_CAT_TELEFONOS
               // modelBuilder.Entity<rh_cat_telefonos>()
               //     .HasKey(c => new { c.IdTelefono });

               // modelBuilder.Entity<rh_cat_telefonos>()
               // .HasOne(s => s.cat_tipos_generales).
               // WithMany().HasForeignKey(s => new { s.IdTipoGenTelefono });

               // modelBuilder.Entity<rh_cat_telefonos>()
               // .HasOne(s => s.cat_generales).
               // WithMany().HasForeignKey(s => new { s.IdGenTelefono });

               // //CAT_TIPOS_ESTATUS
               // modelBuilder.Entity<cat_tipos_estatus>()
               //     .HasKey(c => new { c.IdTipoEstatus });

               // //CAT_ESTATUS
               // modelBuilder.Entity<cat_estatus>()
               //     .HasKey(c => new { c.IdEstatus });

               // modelBuilder.Entity<cat_estatus>()
               // .HasOne(s => s.cat_tipos_estatus).
               // WithMany().HasForeignKey(s => new { s.IdTipoEstatus });

               // //SEG_USUARIOS_ESTATUS
               // modelBuilder.Entity<seg_usuarios_estatus>()
               //     .HasKey(c => new { c.IdCrtlEstatus });

               // modelBuilder.Entity<seg_usuarios_estatus>()
               // .HasOne(s => s.cat_tipos_estatus).
               // WithMany().HasForeignKey(s => new { s.IdTipoEstatus });

               // modelBuilder.Entity<seg_usuarios_estatus>()
               // .HasOne(s => s.cat_estatus).
               // WithMany().HasForeignKey(s => new { s.IdEstatus });

               // //SEG_EXPIRA_CLAVES
               // modelBuilder.Entity<seg_expira_claves>()
               //     .HasKey(c => new { c.IdClave, c.IdUsuario });

               // modelBuilder.Entity<seg_expira_claves>()
               // .HasOne(s => s.cat_usuarios).
               // WithMany().HasForeignKey(s => new { s.IdUsuario });

               // #endregion


            }
            catch (Exception e)
            {
                await new Page().DisplayAlert("ALERTA", e.Message.ToString(), "OK");
            }

        }//AL CREAR EL MODELO

    }//CLASS
}//NAMESPACE
