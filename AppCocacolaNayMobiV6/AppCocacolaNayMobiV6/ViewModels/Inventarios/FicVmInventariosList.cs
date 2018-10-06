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
    public class FicVmInventariosList : INotifyPropertyChanged
    {
        public ObservableCollection<zt_inventarios> _FicSfDataGrid_ItemSource_Inventario;
        public zt_inventarios _FicSfDataGrid_SelectItem_Inventario;
        private ICommand _FicMetAddConteoICommand, _FicMetAcumuladosICommand;

        private IFicSrvNavigationInventario IFicSrvNavigationInventario;
        private IFicSrvInventariosList IFicSrvInventariosList;

        public FicVmInventariosList(IFicSrvNavigationInventario IFicSrvNavigationInventario, IFicSrvInventariosList IFicSrvInventariosList)
        {
            this.IFicSrvNavigationInventario = IFicSrvNavigationInventario;
            this.IFicSrvInventariosList = IFicSrvInventariosList;
            _FicSfDataGrid_ItemSource_Inventario = new ObservableCollection<zt_inventarios>();
        }//CONSTRUCTOR

        public ObservableCollection<zt_inventarios> FicSfDataGrid_ItemSource_Inventario
        {
            get
            {
                return _FicSfDataGrid_ItemSource_Inventario;
            }
        }//ESTE APUNTA ATRAVEZ DEL BindingContext AL GRID DE LA VIEW

        public zt_inventarios FicSfDataGrid_SelectItem_Inventario
        {
            get
            {
                return _FicSfDataGrid_SelectItem_Inventario;
            }
            set
            {
                if(value != null)
                {
                    _FicSfDataGrid_SelectItem_Inventario = value;
                    RaisePropertyChanged();
                }
            }
        }//ESTE APUNTA A UN ITEM SELECCIONADO EN EL GRID DE LA VIEW

        public ICommand FicMetAddConteoICommand
        {
            get
            {
                return _FicMetAddConteoICommand = _FicMetAddConteoICommand ??
                      new FicVmDelegateCommand(FicMetAddConteo);
            }
        }//ESTE VENTO AGREGA EL COMANDO AL BOTON EN LA VIEW


        private void FicMetAddConteo()
        {
            if (_FicSfDataGrid_SelectItem_Inventario != null)
            {
                IFicSrvNavigationInventario.FicMetNavigateTo<FicVmInventarioConteoList>
                    (_FicSfDataGrid_SelectItem_Inventario);
            }
        }

        public ICommand FicMetAcumuladosICommand
        {
            get
            {
                return _FicMetAcumuladosICommand = _FicMetAcumuladosICommand ??
                      new FicVmDelegateCommand(FicMetAcomulados);
            }
        }//ESTE VENTO AGREGA EL COMANDO AL BOTON EN LA VIEW


        private void FicMetAcomulados()
        {
            if (_FicSfDataGrid_SelectItem_Inventario != null)
            {
                IFicSrvNavigationInventario.FicMetNavigateTo<FicVmInventarioAcumuladoList>
                    (_FicSfDataGrid_SelectItem_Inventario);
            }
        }

        public async void OnAppearing()
        {
            try
            {
                var source_local_inv = await IFicSrvInventariosList.FicMetGetListInventarios();

                if (source_local_inv != null)
                {
                    foreach(zt_inventarios inv in source_local_inv)
                    {
                        _FicSfDataGrid_ItemSource_Inventario.Add(inv);
                    }
                }//LLENAR EL GRID

            }
            catch(Exception e)
            {
                await new Page().DisplayAlert("ALERTA", e.Message.ToString(), "OK");
            }
        }//SOBRE CARGA AL METODO OnAppearing() DE LA VIEW

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
