using AppCocacolaNayMobiV6.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppCocacolaNayMobiV6.Interfaces.Inventarios
{
    public interface IFicSrvInventariosConteosItem
    {
        Task<IList<zt_inventarios_conteos>> FicMetGetListInventariosConteos();
        Task<IList<zt_cat_almacenes>> FicMetGetListAlmacenes();
        Task<IList<zt_cat_cedis>> FicMetGetListCedis();
        Task<IList<zt_cat_unidad_medidas>> FicMetGetListCatUnidadMedida();
        Task<IList<zt_cat_productos>> FicMetGetListCatProductos();
        Task<IList<zt_cat_ubicaciones>> FicMetGetListUbicacion(string idalm);
        Task<zt_cat_almacenes> FicMetGetItemAlmacenes(string id);
        Task<string> Insert_zt_inventarios_conteos(zt_inventarios_conteos zt_inventarios_conteos, bool modo);
        Task<string> Remove_zt_inventarios_conteos(zt_inventarios_conteos zt_inventarios_conteos);
        Task<body_edit_conteo_item> FicExitBodyEdit(zt_inventarios_conteos zt_inventarios_conteos);
    }//INTERFACE
}//NAMESPACE
