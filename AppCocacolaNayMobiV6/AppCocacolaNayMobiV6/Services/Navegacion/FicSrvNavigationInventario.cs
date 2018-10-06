using AppCocacolaNayMobiV6.Interfaces.Navegacion;
using AppCocacolaNayMobiV6.ViewModels.Inventarios;
using AppCocacolaNayMobiV6.Views.Inventarios;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppCocacolaNayMobiV6.Services.Navegacion
{
    public class FicSrvNavigationInventario : IFicSrvNavigationInventario
    {
        private IDictionary<Type, Type> FicViewModelRouting = new Dictionary<Type, Type>()
        { 
            //AQUI SE HACE UNA UNION ENTRE LA VM Y VI DE CADA VIEW DE LA APP
            { typeof(FicVmInventariosList),typeof(FicViInventariosList) },
            { typeof(FicVmInventarioConteoList),typeof(FicViInventarioConteoList) },
            { typeof(FicVmInventarioConteosItem),typeof(FicViInventarioConteosItem) },
            { typeof(FicVmInventarioAcumuladoList),typeof(FicViInventarioAcumuladoList)}
        };

        #region METODOS DE IMPLEMENTACION DE LA INTERFACE -> IFicSrvNavigationInventario
                public void FicMetNavigateTo<FicTDestinationViewModel>(object FicNavigationContext = null)
                    {
                        Type FicPageType = FicViewModelRouting[typeof(FicTDestinationViewModel)];
                        var FicPage = Activator.CreateInstance(FicPageType, FicNavigationContext) as Page;

                        if (FicPage != null)
                        {
                            var mdp = Application.Current.MainPage as MasterDetailPage;
                            mdp.Detail.Navigation.PushAsync(FicPage);
                        }
                }

                public void FicMetNavigateTo(Type FicDestinationType, object FicNavigationContext = null)
                {
                    Type FicPageType = FicViewModelRouting[FicDestinationType];
                    var FicPage = Activator.CreateInstance(FicPageType, FicNavigationContext) as Page;

                    if (FicPage != null)
                    {
                        var mdp = Application.Current.MainPage as MasterDetailPage;
                        mdp.Detail.Navigation.PushAsync(FicPage);
                    }
                }

                public void FicMetNavigateBack()
                {
                    Application.Current.MainPage.Navigation.PopAsync(true);
                }
            #endregion

    }//CLASS
}//NAMESPACE
