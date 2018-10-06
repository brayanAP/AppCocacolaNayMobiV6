using AppCocacolaNayMobiV6.Models;
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
	public partial class FicViInventarioConteoList : ContentPage
	{
        private object FicParameter { get; set; }

        public FicViInventarioConteoList(object FicNavigationContext)
        {
            InitializeComponent();
            FicParameter = FicNavigationContext;
            BindingContext = App.FicVmLocator.FicVmInventarioConteoList;
        }//CONSTRUCTOR

        protected async override void OnAppearing()
        {
            var FicViewModel = BindingContext as FicVmInventarioConteoList;
            if (FicViewModel != null)
            {
                FicViewModel.FicNavigationContextE = FicParameter;
                FicViewModel.OnAppearing();
            }

        }//SE EJECUTA CUANDO SE ABRE LA VIEW

    }//CLASS
}//NAMESPACE