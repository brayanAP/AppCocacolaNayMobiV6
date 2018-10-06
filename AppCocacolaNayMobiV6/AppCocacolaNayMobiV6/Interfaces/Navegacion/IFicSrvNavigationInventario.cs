using System;
using System.Collections.Generic;
using System.Text;

namespace AppCocacolaNayMobiV6.Interfaces.Navegacion
{
    public interface IFicSrvNavigationInventario
    {
        /*METODOS PARA LA NAVEGACION ENTRE VIEWS DE LA APP*/
        void FicMetNavigateTo<FicTDestinationViewModel>(object FicNavigationContext = null);
        void FicMetNavigateTo(Type FicDestinationType, object FicNavigationContext = null);
        void FicMetNavigateBack();
    }//INTERFACE
}//NAMESPACE
