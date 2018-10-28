using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AppCocacolaNayMobiV6.Models.Eva
{
    public class seg_usuarios_estatus
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdCrtlEstatus { get; set; }//PK
        public int IdUsuario { get; set; }//FK
        public Nullable<DateTime> FechaEstatus { get; set; }
        public Nullable<Int16> IdTipoEstatus { get; set; }//FK
        public cat_tipos_estatus cat_tipos_estatus { get; set; }
        public Nullable<Int16> IdEstatus { get; set; }//FK
        public cat_estatus cat_estatus { get; set; }
        [StringLength(1)]
        public string Actual { get; set; }
        [StringLength(500)]
        public string Observacion { get; set; }
        public Nullable<DateTime> FechaReg { get; set; }
        [StringLength(20)]
        public string UsuarioReg { get; set; }
        [StringLength(1)]
        public string Activo { get; set; }
        [StringLength(1)]
        public string Borrado { get; set; }
    }//OK

    public class seg_expira_claves
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdClave { get; set; }//PK
        public int IdUsuario { get; set; }//PK FK
        public cat_usuarios cat_usuarios { get; set; }
        public Nullable<DateTime> FechaExpiraIni { get; set; }
        public Nullable<DateTime> FechaExpiraFin { get; set; }
        [StringLength(1)]
        public string Actual { get; set; }
        [StringLength(20)]
        public string Clave { get; set; }
        [StringLength(1)]
        public string ClaveAutoSys { get; set; }
        public Nullable<DateTime> FechaReg { get; set; }
        [StringLength(20)]
        public string UsuarioReg { get; set; }
        [StringLength(1)]
        public string Activo { get; set; }
        [StringLength(1)]
        public string Borrado { get; set; }
    }//OK

    public class temp_web_api_login
    {
        public cat_usuarios cat_usuarios { get; set; }
        public rh_cat_personas rh_cat_personas { get; set; }
        public seg_usuarios_estatus seg_usuarios_estatus { get; set; }
        public seg_expira_claves seg_expira_claves { get; set; }
        public rh_cat_dir_web rh_cat_dir_web { get; set; }
        public List<rh_cat_telefonos> list_telefonos { get; set; }
    }//ESTE MODELO TEMPORAL SIRVE PARA EL LOGIN
}//NAMESPACE
