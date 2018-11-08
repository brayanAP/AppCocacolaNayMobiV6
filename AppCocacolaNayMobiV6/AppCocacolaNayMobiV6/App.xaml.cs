using AppCocacolaNayMobiV6.ViewModels.Base;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace AppCocacolaNayMobiV6
{
    public partial class App : Application
    {
        /*PARA COMUNICARNOS CON NUESTRO LOCATOR DENTRO DE LA APP*/
        private static FicVimLocator FicLocalVmLocator;

        public static FicVimLocator FicVmLocator
        {
            get { return FicLocalVmLocator = FicLocalVmLocator ?? new FicVimLocator(); }
        }

        public App()
        {
            InitializeComponent();

            /*MANDAMOS NUESTRO MAESTRO DETALLE COMO MAINPAGE*/
            MainPage = new Views.Navegacion.FicMasterPage();
            //Mzg0NjdAMzEzNjJlMzMyZTMwQWRwWFRqcFRTQ3l6V2FHcXFxY0ZUVE5mSlBjd3M1L2pKbE4xelBudmRGbz0=
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mzg0NjdAMzEzNjJlMzMyZTMwQWRwWFRqcFRTQ3l6V2FHcXFxY0ZUVE5mSlBjd3M1L2pKbE4xelBudmRGbz0=");
        }//CONSTRUCTOR

        #region METODOS DE LA CLASE
        protected override void OnStart()
            {
                // Handle when your app starts
            }

            protected override void OnSleep()
            {
                // Handle when your app sleeps
            }

            protected override void OnResume()
            {
                // Handle when your app resumes
            }
        #endregion

    }//CLASS
}//NAMESPACE
