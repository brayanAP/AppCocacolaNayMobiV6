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
        private static FicViewModelLocator FicLocalVmLocator;

        public static FicViewModelLocator FicVmLocator
        {
            get { return FicLocalVmLocator = FicLocalVmLocator ?? new FicViewModelLocator(); }
        }

        public App()
        {
            InitializeComponent();

            /*MANDAMOS NUESTRO MAESTRO DETALLE COMO MAINPAGE*/
            MainPage = new Views.Navegacion.FicMasterPage();
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
