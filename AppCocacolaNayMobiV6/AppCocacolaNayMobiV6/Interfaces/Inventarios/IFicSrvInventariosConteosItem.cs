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
        Task<string> FicMetIdAlmDes(Int16 id);
        Task<string> FicMetIdUnmDes(Int16 id);
        Task<string> Insert_zt_inventarios_conteos(zt_inventarios_conteos zt_inventarios_conteos, string alm, string unm, string zku);
        Task<string> Remove_zt_inventarios_conteos(zt_inventarios_conteos zt_inventarios_conteos);
    }//INTERFACE
}//NAMESPACE
