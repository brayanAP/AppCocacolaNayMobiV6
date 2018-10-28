using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AppCocacolaNayMobiV6.Models.Eva
{
    public class cat_tipos_generales
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int16 IdTipoGeneral { get; set; }//PK
        [StringLength(100)]
        public string DesTipo { get; set; }
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
    }//ok

    public class cat_generales
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int16 IdGeneral { get; set; }//PK
        public Int16 IdTipoGeneral { get; set; }//FK
        public cat_tipos_generales cat_tipos_generales { get; set; }
        [StringLength(20)]
        public string Clave { get; set; }
        [StringLength(100)]
        public string DesGeneral { get; set; }
        public string IdLlaveClasifica { get; set; }
        [StringLength(50)]
        public string Referencia { get; set; }
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
    }//ok

    public class cat_institutos
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int16 IdInstituto { get; set; }//PK
        [StringLength(50)]
        public string DesInstituto { get; set; }
        [StringLength(20)]
        public string Alias { get; set; }
        [StringLength(1)]
        public string Matriz { get; set; }
        public Nullable<Int16> IdInstitutoPadre { get; set; }//FK
        public cat_institutos cat_institutos_padre { get; set; }
        public Nullable<Int16> IdTipoGenGiro { get; set; }
        public cat_tipos_generales cat_tipos_generales { get; set; }
        public Nullable<Int16> IdGenGiro { get; set; }
        public cat_generales cat_generales { get; set; }
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
    }//ok

    public class cat_usuarios
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdUsuario { get; set; }//PK
        public int IdPersona { get; set; }//FK
        public rh_cat_personas rh_cat_personas { get; set; }
        [StringLength(20)]
        public string Usuario { get; set; }
        [StringLength(1)]
        public string Expira { get; set; }
        [StringLength(1)]
        public string Conectado { get; set; }
        public Nullable<DateTime> FechaAlta { get; set; }
        public Nullable<Int16> NumIntentos { get; set; }
        public Nullable<DateTime> FechaReg { get; set; }
        public Nullable<DateTime> FechaUltMod { get; set; }
        [StringLength(20)]
        public string UsuarioReg { get; set; }
        [StringLength(20)]
        public string UsuarioMod { get; set; }
        [StringLength(1)]
        public string Activo { get; set; }
        [StringLength(1)]
        public string Borrado { get; set; }

    }//ok

    public class rh_cat_dir_web
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdDirWeb { get; set; }//PK
        [StringLength(50)]
        public string DesDirWeb { get; set; }
        [StringLength(255)]
        public string DirWeb { get; set; }
        [StringLength(1)]
        public string Principal { get; set; }
        public Nullable<Int16> IdTipoGenDirWeb { get; set; }//FK
        public cat_tipos_generales cat_tipos_generales { get; set; }
        public Nullable<Int16> IdGenDirWeb { get; set; }//FK
        public cat_generales cat_generales { get; set; }
        [StringLength(50)]
        public string ClaveReferencia { get; set; }
        [StringLength(50)]
        public string Referencia { get; set; }
        public Nullable<DateTime> FechaReg { get; set; }
        public Nullable<DateTime> FechaUltMod { get; set; }
        [StringLength(20)]
        public string UsuarioReg { get; set; }
        [StringLength(20)]
        public string UsuarioMod { get; set; }
        [StringLength(1)]
        public string Activo { get; set; }
        [StringLength(1)]
        public string Borrado { get; set; }
    }//ok

    public class cat_tipos_estatus
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int16 IdTipoEstatus { get; set; }//PK
        [StringLength(30)]
        public string DesTipoEstatus { get; set; }
        public Nullable<DateTime> FechaReg { get; set; }
        public Nullable<DateTime> FechaUltMod { get; set; }
        [StringLength(20)]
        public string UsuarioReg { get; set; }
        [StringLength(20)]
        public string UsuarioMod { get; set; }
        [StringLength(1)]
        public string Activo { get; set; }
        [StringLength(1)]
        public string Borrado { get; set; }
    }//Ok

    public class cat_estatus
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int16 IdEstatus { get; set; }//PK
        public Int16 IdTipoEstatus { get; set; }//FK
        public cat_tipos_estatus cat_tipos_estatus { get; set; }
        [StringLength(50)]
        public string Clave { get; set; }
        [StringLength(30)]
        public string DesEstatus { get; set; }
        public Nullable<DateTime> FechaReg { get; set; }
        public Nullable<DateTime> FechaUltMod { get; set; }
        [StringLength(20)]
        public string UsuarioReg { get; set; }
        [StringLength(20)]
        public string UsuarioMod { get; set; }
        [StringLength(1)]
        public string Activo { get; set; }
        [StringLength(1)]
        public string Borrado { get; set; }
    }//Ok

}//NAMESPACE
