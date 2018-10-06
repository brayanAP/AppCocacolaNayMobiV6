using AppCocacolaNayMobiV6.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppCocacolaNayMobiV6.Interfaces.Inventarios
{
    public interface IFicSrvInventariosList
    {
        Task<IEnumerable<zt_inventarios>> FicMetGetListInventarios();
    }//INTERFACE
}//NAMESPACE
