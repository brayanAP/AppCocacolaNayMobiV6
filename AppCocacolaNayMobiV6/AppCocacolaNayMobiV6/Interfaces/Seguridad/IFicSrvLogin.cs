using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppCocacolaNayMobiV6.Interfaces.Seguridad
{
    public interface IFicSrvLogin
    {
        Task<string> FicMetLoginUser(string user, string password);
        string FicMetEncripta(string texto);

    }//INTERFACE
}//NAMESPACE
