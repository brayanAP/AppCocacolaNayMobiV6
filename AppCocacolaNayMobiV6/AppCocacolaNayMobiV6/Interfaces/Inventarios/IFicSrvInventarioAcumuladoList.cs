using AppCocacolaNayMobiV6.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppCocacolaNayMobiV6.Interfaces.Inventarios
{
    public interface IFicSrvInventarioAcumuladoList
    {
        Task<IList<zt_inventarios_acumulados>> FicMetGetAcumuladosList(int _idinventario);
    }//CLASS
}//NAMESPACE
