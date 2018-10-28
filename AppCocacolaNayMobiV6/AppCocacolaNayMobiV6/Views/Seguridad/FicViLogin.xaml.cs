using AppCocacolaNayMobiV6.ViewModels.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppCocacolaNayMobiV6.Views.Seguridad
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FicViLogin : ContentPage
	{
		public FicViLogin ()
		{
			InitializeComponent ();
            BindingContext = App.FicVmLocator.FicVmLogin;
        }//CONSTRUCTOR

        protected async override void OnAppearing()
        {
            var FicViewModel = BindingContext as FicVmLogin;
            if (FicViewModel != null)
            {
                FicViewModel.OnAppearing();
            }

        }//SE EJECUTA CUANDO SE ABRE LA VIEW

    }//CLASS
}//NAMESPACE