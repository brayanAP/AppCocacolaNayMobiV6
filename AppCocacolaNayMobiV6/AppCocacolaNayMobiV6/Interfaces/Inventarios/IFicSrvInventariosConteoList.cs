using AppCocacolaNayMobiV6.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppCocacolaNayMobiV6.Interfaces.Inventarios
{
    public interface IFicSrvInventariosConteoList
    {
        Task<IList<zt_inventarios_conteos>> FicMetGetListInventariosConteos(int IdInventario);
        Task<IList<zt_inventarios_conteos>> FicMetGetListInventariosConteos(int IdInventario, zt_inventarios_acumulados item);
    }//INTERFACE
}//NAMESPACE
