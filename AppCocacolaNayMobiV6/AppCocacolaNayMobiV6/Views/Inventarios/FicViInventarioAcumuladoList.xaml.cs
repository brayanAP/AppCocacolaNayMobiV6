using AppCocacolaNayMobiV6.ViewModels.Inventarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppCocacolaNayMobiV6.Views.Inventarios
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FicViInventarioAcumuladoList : ContentPage
	{
        private object FicNavigationContext { get; set; }

        public FicViInventarioAcumuladoList(object FicNavigationContext)
        {
            InitializeComponent();
            BindingContext = App.FicVmLocator.FicVmInventarioAcumuladoList;
        }//CONSTRUCTOR

        protected async override void OnAppearing()
        {
            var FicViewModel = BindingContext as FicVmInventarioAcumuladoList;
            if (FicViewModel != null)
            {
                FicViewModel.FicNavigationContext = FicNavigationContext;
                FicViewModel.OnAppearing();
            }
        }//SE EJECUTA CUANDO SE ABRE LA VIEW

    }//CLASS
}//NAMESPACE