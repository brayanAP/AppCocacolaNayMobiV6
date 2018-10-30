using AppCocacolaNayMobiV6.Interfaces.Inventarios;
using AppCocacolaNayMobiV6.Interfaces.Navegacion;
using AppCocacolaNayMobiV6.Models;
using AppCocacolaNayMobiV6.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppCocacolaNayMobiV6.ViewModels.Inventarios
{
    public class FicVmInventarioAcumuladoList : INotifyPropertyChanged
    {
        public object FicNavigationContext { get; set; }

        private ObservableCollection<zt_inventarios_acumulados> _FicSfDataGrid_ItemSource_Acumulado;
        private zt_inventarios_acumulados _FicSfDataGrid_SelectItem_Acumulado;
        private string _FicPickerFiltroSelected;

        public string FicPickerFiltroSelected { get { return _FicPickerFiltroSelected; }
            set {

                if(value != null)
                {
                    _FicPickerFiltroSelected = value;
                    RaisePropertyChanged("FicPickerFiltroSelected");
                }
            }
        }

        public zt_inventarios_acumulados FicSfDataGrid_SelectItem_Acumulado {
            get { return _FicSfDataGrid_SelectItem_Acumulado; }
            set
            {
                if(value != null)
                {
                    _FicSfDataGrid_SelectItem_Acumulado = value;
                    RaisePropertyChanged("FicSfDataGrid_SelectItem_Acumulado");
                }
            }
        }

        public ObservableCollection<zt_inventarios_acumulados> FicSfDataGrid_ItemSource_Acumulado { get { return _FicSfDataGrid_ItemSource_Acumulado; } }
        private ICommand _FicMetListaConteoICommand, _DoubleTappedCommandAction;


        private IFicSrvInventarioAcumuladoList IFicSrvInventarioAcumuladoList;
        private IFicSrvNavigationInventario IFicSrvNavigationInventario;

        public FicVmInventarioAcumuladoList(IFicSrvNavigationInventario IFicSrvNavigationInventario,IFicSrvInventarioAcumuladoList IFicSrvInventarioAcumuladoList)
        {
            this.IFicSrvNavigationInventario = IFicSrvNavigationInventario;
            this.IFicSrvInventarioAcumuladoList = IFicSrvInventarioAcumuladoList;
        }//CONSTRUCTOR

        public ICommand DoubleTappedCommandAction
        {
            get
            {
                return _DoubleTappedCommandAction = _DoubleTappedCommandAction ??
                      new FicVmDelegateCommand(FicMetDoubleTapped);
            }
        }//ESTE VENTO AGREGA EL COMANDO AL BOTON EN LA VIEW


        private async void FicMetDoubleTapped()
        {
            try
            {
               
                if(_FicSfDataGrid_SelectItem_Acumulado != null)
                {
                    object[] temp = { FicNavigationContext, new zt_inventarios_conteos() {
                        IdSKU = _FicSfDataGrid_SelectItem_Acumulado.IdSKU,
                        IdAlmacen = (FicNavigationContext as zt_inventarios).IdAlmacen,

                    },"NUEVO"};
                    IFicSrvNavigationInventario.FicMetNavigateTo<FicVmInventarioConteosItem>(temp);
                }
            }
            catch (Exception e)
            {
                await new Page().DisplayAlert("ALERTA", e.Message.ToString(), "OK");
            }
        }

        public ICommand FicMetListaConteoICommand
        {
            get
            {
                return _FicMetListaConteoICommand = _FicMetListaConteoICommand ??
                      new FicVmDelegateCommand(FicMetConteoList);
            }
        }//ESTE VENTO AGREGA EL COMANDO AL BOTON EN LA VIEW


        private async void FicMetConteoList()
        {
            try
            {
                if (_FicSfDataGrid_SelectItem_Acumulado == null)
                {
                    await new Page().DisplayAlert("ALERTA", "SELECCIONE UN ACUMULADO.", "OK");
                    return;
                }

                object[] temp = { FicNavigationContext, _FicSfDataGrid_SelectItem_Acumulado };
                IFicSrvNavigationInventario.FicMetNavigateTo<FicVmInventarioConteoList>(temp);
            }
            catch (Exception e)
            {
                await new Page().DisplayAlert("ALERTA", e.Message.ToString(), "OK");
            }
        }

        public async void OnAppearing()
        {
            try
            {
                var FicSourceZt_Inventarios = FicNavigationContext as zt_inventarios;

                _FicSfDataGrid_ItemSource_Acumulado = new ObservableCollection<zt_inventarios_acumulados>();

                foreach(zt_inventarios_acumulados au in await IFicSrvInventarioAcumuladoList.FicMetGetAcumuladosList(FicSourceZt_Inventarios.IdInventario))
                {
                    _FicSfDataGrid_ItemSource_Acumulado.Add(au);
                }

                RaisePropertyChanged("FicSfDataGrid_ItemSource_Acumulado");

            }
            catch(Exception e)
            {
                await new Page().DisplayAlert("ALERTA", e.Message.ToString(), "OK");
            }
        }//OnAppearing()

        public async void FicMetFiltroSKU()
        {
            try
            {
                var FicSourceZt_Inventarios = FicNavigationContext as zt_inventarios;
                _FicSfDataGrid_ItemSource_Acumulado = new ObservableCollection<zt_inventarios_acumulados>();
                ObservableCollection<zt_inventarios_acumulados> FicSinConteo = new ObservableCollection<zt_inventarios_acumulados>();
                ObservableCollection<zt_inventarios_acumulados> FicConConteo = new ObservableCollection<zt_inventarios_acumulados>();

                foreach (zt_inventarios_acumulados au in await IFicSrvInventarioAcumuladoList.FicMetGetAcumuladosList(FicSourceZt_Inventarios.IdInventario))
                {
                    _FicSfDataGrid_ItemSource_Acumulado.Add(au);
                    if (au.CantidadFisica == null) FicSinConteo.Add(au);
                    else if (au.CantidadFisica >= 0) FicConConteo.Add(au);
                }

                if (_FicPickerFiltroSelected == "Ver SKU sin conteo") _FicSfDataGrid_ItemSource_Acumulado = FicSinConteo;
                else if (_FicPickerFiltroSelected == "Ver SKU con conteo") _FicSfDataGrid_ItemSource_Acumulado = FicConConteo;
                RaisePropertyChanged("FicSfDataGrid_ItemSource_Acumulado");
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
