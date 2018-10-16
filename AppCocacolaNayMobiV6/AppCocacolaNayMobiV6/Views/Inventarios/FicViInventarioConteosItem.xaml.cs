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
    public partial class FicViInventarioConteosItem : ContentPage
    {
        private object[] FicCuerpoNavigationContext { get; set; }

        private bool FicModo { get; set; }

        public FicViInventarioConteosItem(object[] FicNavigationContext)
        {
            InitializeComponent();
            FicCuerpoNavigationContext = FicNavigationContext;
            BindingContext = App.FicVmLocator.FicVmInventarioConteosItem;
        }//CONSTRUCTOR


        protected async override void OnAppearing()
        {
            var FicViewModel = BindingContext as FicVmInventarioConteosItem;
            if (FicViewModel != null)
            {
                FicViewModel.FicNavigationContextC = FicCuerpoNavigationContext;

                if (FicCuerpoNavigationContext[1] != null) {
                    FicViewModel.FicModo = true;
                    FicCodigoBarras.IsEnabled = false;
                    FicSKU.IsEnabled = false;
                    FicUnm.IsEnabled = false;
                    FicAlm.IsEnabled = false;
                    FicUb.IsEnabled = false;
                } 
                else FicViewModel.FicModo = false;

                FicViewModel.OnAppearing();
                FicCodigoBarras.ValueChanged += (object sender, Syncfusion.SfAutoComplete.XForms.ValueChangedEventArgs e) =>
                {
                    FicViewModel.FicMetLoadInfoTomaCodigoBarra();
                };

                FicSKU.ValueChanged += (object sender, Syncfusion.SfAutoComplete.XForms.ValueChangedEventArgs e) =>
                {
                    FicViewModel.FicMetLoadInfoTomaSKU();
                };
            }
        }//SE EJECUTA CUANDO SE ABRE LA VIEW

    }//CLASS
}//NAMESPACE