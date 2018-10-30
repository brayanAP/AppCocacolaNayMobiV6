using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppCocacolaNayMobiV6.Interfaces.Inventarios
{
    public interface IFicSrvExportarWebApi
    {
        Task<string> FicPostExportInventarios(int idInv);
    }//INTERFACE
}//NAMESPACE
