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
	public partial class FicViExportarWebApi : ContentPage
	{
		public FicViExportarWebApi ()
		{
			InitializeComponent ();
            BindingContext = App.FicVmLocator.FicVmExportarWebApi;
        }//CONSTRUCTOR

        protected async override void OnAppearing()
        {
            var FicViewModel = BindingContext as FicVmExportarWebApi;
            if (FicViewModel != null)
            {
                FicViewModel.OnAppearing();
            }

        }//SE EJECUTA CUANDO SE ABRE LA VIEW

    }//CLASS
}//NAMESPACE