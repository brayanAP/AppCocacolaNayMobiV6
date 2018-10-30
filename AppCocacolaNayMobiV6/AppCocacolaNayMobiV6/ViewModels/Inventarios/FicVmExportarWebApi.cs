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
        private string _FicTextAreaExpInv, _FicLabelIdInv;
        private ICommand _FicMetExpoInv, _FicMetExpoIdInvIdRange;

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

        public string FicLabelIdInv
        {
            get { return _FicLabelIdInv; }
            set
            {
                if(value != null)
                {
                    _FicLabelIdInv = value;
                    RaisePropertyChanged("FicLabelIdInv");
                }
            }
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
                _FicTextAreaExpInv = await IFicSrvExportarWebApi.FicPostExportInventarios(0);
                RaisePropertyChanged("FicTextAreaExpInv");
                await new Page().DisplayAlert("ALERTA", "Datos Actualizados.", "OK");
            }
            catch (Exception e)
            {
                await new Page().DisplayAlert("ALERTA", e.Message.ToString(), "OK");
            }
        }

        public ICommand FicMetExpoIdInvIdRange
        {
            get
            {
                return _FicMetExpoIdInvIdRange = _FicMetExpoIdInvIdRange ??
                      new FicVmDelegateCommand(FicMetExportInventarioId);
            }
        }//ESTE VENTO AGREGA EL COMANDO AL BOTON EN LA VIEW

        private async void FicMetExportInventarioId()
        {
            try
            {
                _FicTextAreaExpInv = await IFicSrvExportarWebApi.FicPostExportInventarios(int.Parse(_FicLabelIdInv));
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
