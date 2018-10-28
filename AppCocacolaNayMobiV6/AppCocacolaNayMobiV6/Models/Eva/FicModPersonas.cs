using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AppCocacolaNayMobiV6.Models.Eva
{
    public class rh_cat_personas
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdPersona { get; set; }//PK
        public Int16 IdInstituto { get; set; } //FK
        public cat_institutos cat_institutos { get; set; }
        [StringLength(20)]
        public string NumControl { get; set; }
        [StringLength(100)]
        public string Nombre { get; set; }
        [StringLength(60)]
        public string ApPaterno { get; set; }
        [StringLength(60)]
        public string ApMaterno { get; set; }
        [StringLength(15)]
        public string RFC { get; set; }
        [StringLength(25)]
        public string CURP { get; set; }
        public Nullable<DateTime> FechaNac { get; set; }
        [StringLength(1)]
        public string TipoPersona { get; set; }
        [StringLength(1)]
        public string Sexo { get; set; }
        [StringLength(255)]
        public string RutaFoto { get; set; }
        [StringLength(20)]
        public string Alias { get; set; }
        public Nullable<Int16> IdTipoGenOcupacion { get; set; }//FK
        public Nullable<Int16> IdGenOcupacion { get; set; }//FK
        public Nullable<Int16> IdTipoGenEstadoCivil { get; set; }//FK
        public Nullable<Int16> IdGenEstadoCivil { get; set; }//FK
        public cat_tipos_generales cat_tipos_generales { get; set; }
        public cat_generales cat_generales { get; set; }
        public Nullable<DateTime> FechaReg { get; set; }
        [StringLength(20)]
        public string UsuarioReg { get; set; }
        public Nullable<DateTime> FechaUltMod { get; set; }
        [StringLength(20)]
        public string UsuarioMod { get; set; }
        [StringLength(1)]
        public bool Activo { get; set; }
        [StringLength(1)]
        public bool Borrado { get; set; }
    }//OK

    public class rh_cat_domicilios
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdDomicilio { get; set; }//PK
        [StringLength(200)]
        public string Domicilio { get; set; }
        [StringLength(150)]
        public string EntreCalle1 { get; set; }
        [StringLength(150)]
        public string EntreCalle2 { get; set; }
        [StringLength(10)]
        public string CodigoPostal { get; set; }
        [StringLength(255)]
        public string Coordenadas { get; set; }
        [StringLength(1)]
        public string Principal { get; set; }
        public Int16 IdTipoGenDom { get; set; }//FK
        public cat_tipos_generales cat_tipos_generales { get; set; }
        public Int16 IdGenDom { get; set; }//FK
        public cat_generales cat_generales { get; set; }
        [StringLength(50)]
        public string Pais { get; set; }
        [StringLength(50)]
        public string Estado { get; set; }
        [StringLength(50)]
        public string Municipio { get; set; }
        [StringLength(50)]
        public string Localidad { get; set; }
        [StringLength(100)]
        public string Colonia { get; set; }
        [StringLength(50)]
        public string Referencia { get; set; }
        [StringLength(50)]
        public string ClaveReferencia { get; set; }
        [StringLength(1)]
        public string TipoDomicilio { get; set; }
        public Nullable<DateTime> FechaReg { get; set; }
        public Nullable<DateTime> FechaUltMod { get; set; }
        [StringLength(20)]
        public string UsuarioReg { get; set; }
        [StringLength(20)]
        public string UsuarioMod { get; set; }
        [StringLength(1)]
        public bool Activo { get; set; }
        [StringLength(1)]
        public bool Borrado { get; set; }
    }//OK

    public class rh_cat_telefonos
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdTelefono { get; set; } //PK
        [StringLength(2)]
        public string CodPais { get; set; }
        [StringLength(20)]
        public string NumTelefono { get; set; }
        [StringLength(30)]
        public string NumExtension { get; set; }
        [StringLength(1)]
        public string Principal { get; set; }
        public Nullable<Int16> IdTipoGenTelefono { get; set; }//FK
        public cat_tipos_generales cat_tipos_generales { get; set; }
        public Nullable<Int16> IdGenTelefono { get; set; }//FK
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
    }//OK

}//NAMESPACE
