using AppCocacolaNayMobiV6.Interfaces.Inventarios;
using AppCocacolaNayMobiV6.Interfaces.Navegacion;
using AppCocacolaNayMobiV6.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppCocacolaNayMobiV6.ViewModels.Inventarios
{
    public class FicVmExportarWebApi : INotifyPropertyChanged
    {
        private string _FicTextAreaExpInv;
        private ICommand _FicMetExpoInv;

        private IFicSrvNavigationInventario IFicSrvNavigationInventario;
        private IFicSrvExportarWebApi IFicSrvExportarWebApi;

        public FicVmExportarWebApi(IFicSrvNavigationInventario IFicSrvNavigationInventario, IFicSrvExportarWebApi IFicSrvExportarWebApi)
        {
            this.IFicSrvNavigationInventario = IFicSrvNavigationInventario;
            this.IFicSrvExportarWebApi = IFicSrvExportarWebApi;
        }//CONSTRUCTOR

        public string FicTextAreaExpInv
        {
            get { return _FicTextAreaExpInv; }
        }

        public async void OnAppearing()
        {

        }//AL INICIAR DE LA VIEW

        public ICommand FicMetExpoInv
        {
            get
            {
                return _FicMetExpoInv = _FicMetExpoInv ??
                      new FicVmDelegateCommand(FicMetExportInventario);
            }
        }//ESTE VENTO AGREGA EL COMANDO AL BOTON EN LA VIEW

        private async void FicMetExportInventario()
        {
            try
            {
                _FicTextAreaExpInv = await IFicSrvExportarWebApi.FicPostExportInventarios();
                RaisePropertyChanged("FicTextAreaExpInv");
                await new Page().DisplayAlert("ALERTA", "Datos Actualizados.", "OK");
            }
            catch (Exception e)
            {
                await new Page().DisplayAlert("ALERTA", e.Message.ToString(), "OK");
            }
        }

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
