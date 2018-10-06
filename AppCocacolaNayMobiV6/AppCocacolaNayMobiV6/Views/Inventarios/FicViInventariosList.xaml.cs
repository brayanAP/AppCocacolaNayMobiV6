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
    public partial class FicViInventariosList : ContentPage
    {
        public FicViInventariosList()
        {
            InitializeComponent();
            BindingContext = App.FicVmLocator.FicVmInventariosList;
        }//CONSTRUCTOR

        public FicViInventariosList(object FicNavigationContext)
        {
            InitializeComponent();
            BindingContext = App.FicVmLocator.FicVmInventariosList;
        }//CONSTRUCTOR

        protected async override void OnAppearing()
        {
            var FicViewModel = BindingContext as FicVmInventariosList;
            if (FicViewModel != null)
            {
                FicViewModel.OnAppearing();
            }
        }//SE EJECUTA CUANDO SE ABRE LA VIEW

    }//CLASSE
}//NAMESPACE