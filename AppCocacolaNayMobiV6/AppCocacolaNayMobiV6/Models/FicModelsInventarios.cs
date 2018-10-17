using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace AppCocacolaNayMobiV6.Models
{
    public class zt_cat_cedis
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int16 IdCEDI { get; set; }
        [StringLength(50)]
        public string DesCEDI { get; set; }
        public DateTime FechaReg { get; set; }
        [StringLength(20)]
        public string UsuarioReg { get; set; }
        public Nullable<DateTime> FechaUltMod { get; set; }
        [StringLength(20)]
        public string UsuarioMod { get; set; }
        [StringLength(1)]
        public string Activo { get; set; }
        [StringLength(1)]
        public string Borrado { get; set; }
    }//OK

    public class zt_cat_almacenes
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int16 IdAlmacen { get; set; }
        public Int16 IdCEDI { get; set; }
        public zt_cat_cedis zt_cat_cedis { get; set; }
        [StringLength(50)]
        public string DesAlmacen { get; set; }
        public DateTime FechaReg { get; set; }
        [StringLength(20)]
        public string UsuarioReg { get; set; }
        public Nullable<DateTime> FechaUltMod { get; set; }
        [StringLength(20)]
        public string UsuarioMod { get; set; }
        [StringLength(1)]
        public string Activo { get; set; }
        [StringLength(1)]
        public string Borrado { get; set; }
    }//OK

    public class zt_cat_productos
    {
        [StringLength(20)]
        public string IdSKU { get; set; }
        [StringLength(20)]
        public string CodigoBarras { get; set; }
        [StringLength(50)]
        public string DesSKU { get; set; }
        public DateTime FechaReg { get; set; }
        [StringLength(20)]
        public string UsuarioReg { get; set; }
        public Nullable<DateTime> FechaUltMod { get; set; }
        [StringLength(20)]
        public string UsuarioMod { get; set; }
        [StringLength(1)]
        public string Activo { get; set; }
        [StringLength(1)]
        public string Borrado { get; set; }
    }//OK

    public class zt_cat_unidad_medidas
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
       // public Int16 IdUnidadMedida { get; set; }
       public string IdUnidadMedida { get; set; }
        [StringLength(20)]
        public string DesUMedida { get; set; }
        public DateTime FechaReg { get; set; }
        [StringLength(20)]
        public string UsuarioReg { get; set; }
        public Nullable<DateTime> FechaUltMod { get; set; }
        [StringLength(20)]
        public string UsuarioMod { get; set; }
        [StringLength(1)]
        public string Activo { get; set; }
        [StringLength(1)]
        public string Borrado { get; set; }
    }//ok

    public class zt_inventarios
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdInventario { get; set; }
        public Int16 IdCEDI { get; set; }
        public zt_cat_cedis zt_cat_cedis { get; set; }
        public Nullable<DateTime> FechaReg { get; set; }
        [StringLength(20)]
        public string UsuarioReg { get; set; }
        public Nullable<DateTime> FechaUltMod { get; set; }
        [StringLength(20)]
        public string UsuarioMod { get; set; }
        [StringLength(1)]
        public string Activo { get; set; }
        [StringLength(1)]
        public string Borrado { get; set; }
    }//OK

    public class zt_inventarios_acumulados
    {
        public int IdInventario { get; set; }
        public zt_inventarios zt_inventarios { get; set; }
        [StringLength(20)]
        public string IdSKU { get; set; }
        public zt_cat_productos zt_cat_productos { get; set; }
        public float CantidadTeorica { get; set; }
        public float CantidadFisica { get; set; }
        public float Diferencia { get; set; }
        public string IdUnidadMedida { get; set; }
        public zt_cat_unidad_medidas zt_cat_unidad_medidas { get; set; }
        public DateTime FechaReg { get; set; }
        [StringLength(20)]
        public string UsuarioReg { get; set; }
        public Nullable<DateTime> FechaUltMod { get; set; }
        [StringLength(20)]
        public string UsuarioMod { get; set; }
        [StringLength(1)]
        public string Activo { get; set; }
        [StringLength(1)]
        public string Borrado { get; set; }
    }

    public class zt_inventarios_conteos
    {
        public int IdInventario { get; set; }
        public zt_inventarios zt_inventarios { get; set; }
        public Int16 IdAlmacen { get; set; }
        public zt_cat_almacenes zt_cat_almacenes { get; set; }
        public int NumConteo { get; set; }
        [StringLength(20)]
        public string IdSKU { get; set; }
        public zt_cat_productos zt_cat_productos { get; set; }
        [StringLength(20)]
        public string CodigoBarras { get; set; }
        [StringLength(20)]
        public string IdUbicacion { get; set; }
        public float CantidadFisica { get; set; }
        public string IdUnidadMedida { get; set; }
        public zt_cat_unidad_medidas zt_cat_unidad_medidas { get; set; }
        public float CantidadPZA { get; set; }
        [StringLength(30)]
        public string Lote { get; set; }
        public Nullable<DateTime> FechaReg { get; set; }
        [StringLength(20)]
        public string UsuarioReg { get; set; }
        [StringLength(1)]
        public string Activo { get; set; }
        [StringLength(1)]
        public string Borrado { get; set; }
    }//ok

    public class zt_cat_productos_medidas
    {
        [StringLength(20)]
        public string IdSKU { get; set; }
        public zt_cat_productos zt_cat_productos { get; set; }
        public string IdUnidadMedida { get; set; }
        public zt_cat_unidad_medidas zt_cat_unidad_medidas { get; set; }
        public float CantidadPZA { get; set; }
        public DateTime FechaReg { get; set; }
        [StringLength(20)]
        public string UsuarioReg { get; set; }
        public Nullable<DateTime> FechaUltMod { get; set; }
        [StringLength(20)]
        public string UsuarioMod { get; set; }
        [StringLength(1)]
        public string Activo { get; set; }
        [StringLength(1)]
        public string Borrado { get; set; }
    }//ok

    public class zt_inventatios_acumulados_conteos
    {
        public List<zt_inventarios> zt_inventarios { get; set; }
        public List<zt_inventarios_acumulados> zt_inventarios_acumulados { get; set; }
        public List<zt_inventarios_conteos> zt_inventarios_conteos { get; set; }
    }

    public class zt_inventarios_conteos_grid
    {
        public string IdInventario { get; set; }
        public string NumConteo { get; set; }
        public string IdAlmacen { get; set; }
        public string IdSKU { get; set; }
        public string CantidadFisica { get; set; }
        public string CantidadPZA { get; set; }
        public string IdUnidadMedida { get; set; }
        public string UsuarioReg { get; set; }
    }//ESTE MODELO SIRVE DE MANERA TEMPORAL

    public class zt_catalogos_productos_medidas_cedi_almacenes
    {
        public List<zt_cat_productos> zt_cat_productos { get; set; }
        public List<zt_cat_unidad_medidas> zt_cat_unidad_medidas { get; set; }
        public List<zt_cat_productos_medidas> zt_cat_productos_medidas { get; set; }
        public List<zt_cat_cedis> zt_cat_cedis { get; set; }
        public List<zt_cat_almacenes> zt_cat_almacenes { get; set; }
    }

    public class body_edit_conteo_item
    {
        public zt_cat_almacenes zt_cat_almacenes { get; set; }
        public zt_cat_productos zt_cat_productos { get; set; }
        public zt_cat_unidad_medidas zt_cat_unidad_medidas { get; set; }
    }


    #region PAGINACION
    public class seg_cat_modulos
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int16 IdModulo { get; set; } //PK
        [StringLength(100)]
        public string DesModulo { get; set; }
        public Nullable<Int16> Prioridad { get; set; }
        [StringLength(255)]
        public string RutaIcono { get; set; }
        [StringLength(10)]
        public string Version { get; set; }
        [StringLength(20)]
        public string Abreviatura { get; set; }
        public Nullable<DateTime> FechaReg { get; set; }
        [StringLength(20)]
        public string UsuarioReg { get; set; }
        public Nullable<DateTime> FechaUltMod { get; set; }
        [StringLength(20)]
        public string UsuarioMod { get; set; }
        [StringLength(1)]
        public string Activo { get; set; }
        [StringLength(1)]
        public string Borrado { get; set; }
    }

    public class seg_cat_submodulos
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int16 IdSubmodulo { get; set; } //PK
        public Int16 IdModulo{ get; set; } //PK
        public seg_cat_modulos seg_cat_modulos { get; set; }
        [StringLength(100)]
        public string DesSubmodulo { get; set; }
        public Nullable<Int16> Prioridad { get; set; }
        [StringLength(255)]
        public string RutaIcono { get; set; }
        [StringLength(10)]
        public string Version { get; set; }
        [StringLength(20)]
        public string Abreviatura { get; set; }
        public Nullable<DateTime> FechaReg { get; set; }
        [StringLength(20)]
        public string UsuarioReg { get; set; }
        public Nullable<DateTime> FechaUltMod { get; set; }
        [StringLength(20)]
        public string UsuarioMod { get; set; }
        [StringLength(1)]
        public string Activo { get; set; }
        [StringLength(1)]
        public string Borrado { get; set; }
    }

    public class seg_cat_paginas
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int16 IdPagina { get; set; } //PK
        public Int16 IdModulo { get; set; }
        public seg_cat_modulos seg_cat_modulos { get; set; }
        public Int16 IdSubmodulo { get; set; }
        public seg_cat_submodulos seg_cat_submodulos { get; set; }
        [StringLength(50)]
        public string DesPagina { get; set; }
        [StringLength(500)]
        public string Detalle { get; set; }
        [StringLength(10)]
        public string Version { get; set; }
        public Nullable<Int16> Orden { get; set; }
        [StringLength(500)]
        public string RutaPagina { get; set; }
        [StringLength(255)]
        public string RutaImagen { get; set; }
        [StringLength(1)]
        public string Visible { get; set; }
        public Nullable<DateTime> FechaReg { get; set; }
        [StringLength(20)]
        public string UsuarioReg { get; set; }
        public Nullable<DateTime> FechaUltMod { get; set; }
        [StringLength(20)]
        public string UsuarioMod { get; set; }
        [StringLength(1)]
        public string Activo { get; set; }
        [StringLength(1)]
        public string Borrado { get; set; }
    }

    public class seg_cat_mod_sub_pag
    {
        public List<seg_cat_modulos> seg_cat_modulos { get; set; }
        public List<seg_cat_submodulos> seg_cat_submodulos { get; set; }
        public List<seg_cat_paginas> seg_cat_paginas { get; set; }
    }
    #endregion
}//NAMESPACE
