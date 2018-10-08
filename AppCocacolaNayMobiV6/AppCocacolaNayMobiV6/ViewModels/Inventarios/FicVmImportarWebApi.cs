using AppCocacolaNayMobiV6.Interfaces.Inventarios;
using AppCocacolaNayMobiV6.Interfaces.Navegacion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace AppCocacolaNayMobiV6.ViewModels.Inventarios
{
    public class FicVmImportarWebApi : INotifyPropertyChanged
    {
        private string _FicTextAreaImpInv;

        private IFicSrvNavigationInventario IFicSrvNavigationInventario;
        private IFicSrvImportarWebApi IFicSrvImportarWebApi;
        public FicVmImportarWebApi(IFicSrvNavigationInventario IFicSrvNavigationInventario, IFicSrvImportarWebApi IFicSrvImportarWebApi)
        {
            this.IFicSrvNavigationInventario = IFicSrvNavigationInventario;
            this.IFicSrvImportarWebApi = IFicSrvImportarWebApi;
        }//CONSTRUCTOR

        public string FicTextAreaImpInv
        {
            get { return _FicTextAreaImpInv; }
        }
        public async void OnAppearing()
        {
            //var s = await IFicSrvImportarWebApi.FicGetImportInventarios();
            _FicTextAreaImpInv = await IFicSrvImportarWebApi.FicGetImportInventarios();
            RaisePropertyChanged("FicTextAreaImpInv");

        }//METODO QUE SE MANDA A LLAMAR EN LA VIEW

        #region  INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }//CLASS
}//NAMESPACE
