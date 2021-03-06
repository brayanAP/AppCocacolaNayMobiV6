﻿using AppCocacolaNayMobiV6.Interfaces.Inventarios;
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
    public class FicVmImportarWebApi : INotifyPropertyChanged
    {
        private string _FicTextAreaImpInv, _FicLabelIdInv;
        private ICommand _FicMecImportIdInv,_FicMecImportInv, _FicMecImportCat;

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

        public string FicLabelIdInv
        {
            get { return _FicLabelIdInv; }
            set
            {
                if (value != null) _FicLabelIdInv = value;
            }
        }

        public async void OnAppearing()
        {

        }//METODO QUE SE MANDA A LLAMAR EN LA VIEW

        public ICommand FicMecImportIdInv
        {
            get
            {
                return _FicMecImportIdInv = _FicMecImportIdInv ??
                      new FicVmDelegateCommand(FicMecImportInventarioId);
            }
        }//ESTE VENTO AGREGA EL COMANDO AL BOTON EN LA VIEW

        private async void FicMecImportInventarioId()
        {
            try
            {
                if(_FicLabelIdInv.Length > 0)
                {
                    _FicTextAreaImpInv = await IFicSrvImportarWebApi.FicGetImportInventarios(int.Parse(_FicLabelIdInv));
                    RaisePropertyChanged("FicTextAreaImpInv");
                    await new Page().DisplayAlert("ALERTA", "Datos Actualizados.", "OK");
                }
                else await new Page().DisplayAlert("ALERTA", "ID NO VALIDO.", "OK");

            }
            catch (Exception e)
            {
                await new Page().DisplayAlert("ALERTA", e.Message.ToString(), "OK");
            }
        }

        public ICommand FicMecImportInv
        {
            get
            {
                return _FicMecImportInv = _FicMecImportInv ??
                      new FicVmDelegateCommand(FicMecImportInventario);
            }
        }//ESTE VENTO AGREGA EL COMANDO AL BOTON EN LA VIEW

        private async void FicMecImportInventario()
        {
            try
            {
                _FicTextAreaImpInv = await IFicSrvImportarWebApi.FicGetImportInventarios();
                RaisePropertyChanged("FicTextAreaImpInv");
                await new Page().DisplayAlert("ALERTA", "Datos Actualizados.", "OK");
            }
            catch (Exception e)
            {
                await new Page().DisplayAlert("ALERTA", e.Message.ToString(), "OK");
            }
        }

        public ICommand FicMecImportCat
        {
            get
            {
                return _FicMecImportCat = _FicMecImportCat ??
                      new FicVmDelegateCommand(FicMecImportCatalogo);
            }
        }//ESTE VENTO AGREGA EL COMANDO AL BOTON EN LA VIEW

        private async void FicMecImportCatalogo()
        {
            try
            {
                _FicTextAreaImpInv = await IFicSrvImportarWebApi.FicGetImportCatalogos();
                RaisePropertyChanged("FicTextAreaImpInv");
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