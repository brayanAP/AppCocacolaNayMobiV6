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
    public class FicVmInventarioConteoList : INotifyPropertyChanged
    {
        private string _FicLabelIdInventario, _FicLabelIdCEDI, _FicLabelFechaReg;
        private ObservableCollection<zt_inventarios_conteos> _FicSfDataGrid_ItemSource_Conteo;
        private zt_inventarios_conteos _FicSfDataGrid_SelectItem_Conteo;
        private IFicSrvNavigationInventario IFicSrvNavigationInventario;
        private IFicSrvInventariosConteoList IFicSrvInventariosConteoList;

        private ICommand _FicMetRegesarInventarioICommand, _FicMetAddInventarioConteoICommand, _FicMetEditInventarioConteoICommand;

        public FicVmInventarioConteoList(IFicSrvNavigationInventario IFicSrvNavigationInventario, IFicSrvInventariosConteoList IFicSrvInventariosConteoList)
        {
            this.IFicSrvNavigationInventario = IFicSrvNavigationInventario;
            this.IFicSrvInventariosConteoList = IFicSrvInventariosConteoList;
        }//CONSTRUCTOR

        public object[] FicNavigationContextE { get; set; }

        public string FicLabelIdInventario
        {
            get
            {
                return _FicLabelIdInventario;
            }
        }//REFERENCIA AL TEXT DE UN LABEL

        public string FicLabelIdCEDI
        {
            get
            {
                return _FicLabelIdCEDI;
            }
        }//REFERENCIA AL TEXT DE UN LABEL

        public string FicLabelFechaReg
        {
            get
            {
                return _FicLabelFechaReg;
            }
        }//REFERENCIA AL TEXT DE UN LABEL

        public ObservableCollection<zt_inventarios_conteos> FicSfDataGrid_ItemSource_Conteo
        {
            get
            {
                return _FicSfDataGrid_ItemSource_Conteo;
            }
        }

        public zt_inventarios_conteos FicSfDataGrid_SelectItem_Conteo
        {
            get
            {
                return _FicSfDataGrid_SelectItem_Conteo;
            }
            set
            {
                if(value != null)
                {
                    _FicSfDataGrid_SelectItem_Conteo = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ICommand FicMetRegesarInventarioICommand
        {
            get
            {
                return _FicMetRegesarInventarioICommand = _FicMetRegesarInventarioICommand ??
                      new FicVmDelegateCommand(FicMetRegesarInventario);
            }
        }//ESTE VENTO AGREGA EL COMANDO AL BOTON EN LA VIEW


        private async  void FicMetRegesarInventario()
        {
            try
            {
                IFicSrvNavigationInventario.FicMetNavigateTo<FicVmInventariosList>();
            }
            catch(Exception e)
            {
                await new Page().DisplayAlert("ALERTA", e.Message.ToString(), "OK");
            }
        }

        public ICommand FicMetAddInventarioConteoICommand
        {
            get
            {
                return _FicMetAddInventarioConteoICommand = _FicMetAddInventarioConteoICommand ??
                      new FicVmDelegateCommand(FicMetAddInventarioConteo);
            }
        }//ESTE VENTO AGREGA EL COMANDO AL BOTON EN LA VIEW

        private async void FicMetAddInventarioConteo()
        {
            try
            {
                object[] temp = { FicNavigationContextE[0], null };
                IFicSrvNavigationInventario.FicMetNavigateTo<FicVmInventarioConteosItem>(temp);
            }
            catch (Exception e)
            {
                await new Page().DisplayAlert("ALERTA", e.Message.ToString(), "OK");
            }
        }

        public ICommand FicMetEditInventarioConteoICommand
        {
            get
            {
                return _FicMetEditInventarioConteoICommand = _FicMetEditInventarioConteoICommand ??
                      new FicVmDelegateCommand(FicMetEditInventarioConteo);
            }
        }//ESTE VENTO AGREGA EL COMANDO AL BOTON EN LA VIEW

        private async void FicMetEditInventarioConteo()
        {
            try
            {
                if(_FicSfDataGrid_SelectItem_Conteo != null)
                {
                    object[] TempContext = { FicNavigationContextE[0], _FicSfDataGrid_SelectItem_Conteo };

                    IFicSrvNavigationInventario.FicMetNavigateTo<FicVmInventarioConteosItem>(TempContext);
                }
                
            }
            catch (Exception e)
            {
                await new Page().DisplayAlert("ALERTA", e.Message.ToString(), "OK");
            }
        }


        public async void OnAppearing()
        {
           var FicSourceInventarios= FicNavigationContextE[0] as zt_inventarios;

            /*LLENAR ENCABEZADO*/
            _FicLabelIdInventario = FicSourceInventarios.IdInventario + "";
            _FicLabelIdCEDI = FicSourceInventarios.IdCEDI + "";
            _FicLabelFechaReg = FicSourceInventarios.FechaReg.Value.ToShortDateString();
            RaisePropertyChanged("FicLabelIdInventario");
            RaisePropertyChanged("FicLabelIdCEDI");
            RaisePropertyChanged("FicLabelFechaReg");


            var FicSourceAcumulado = FicNavigationContextE[1] as zt_inventarios_acumulados;

            if(FicSourceAcumulado != null)
            {
                /*LLENAR EL GRID DE MODO FILTRO SKU*/
                _FicSfDataGrid_ItemSource_Conteo = new ObservableCollection<zt_inventarios_conteos>();
                var source = await IFicSrvInventariosConteoList.FicMetGetListInventariosConteos(FicSourceInventarios.IdInventario, FicSourceAcumulado);
                if (source != null)
                {
                    foreach (zt_inventarios_conteos con in source)
                    {
                        _FicSfDataGrid_ItemSource_Conteo.Add(con);
                    }

                    RaisePropertyChanged("FicSfDataGrid_ItemSource_Conteo");
                }
            }
            else
            {
                /*LLENAR EL GRID DE MODO TODOS*/
                _FicSfDataGrid_ItemSource_Conteo = new ObservableCollection<zt_inventarios_conteos>();
                var source = await IFicSrvInventariosConteoList.FicMetGetListInventariosConteos(FicSourceInventarios.IdInventario);
                if (source != null)
                {
                    foreach (zt_inventarios_conteos con in source)
                    {
                        _FicSfDataGrid_ItemSource_Conteo.Add(con);
                    }

                    RaisePropertyChanged("FicSfDataGrid_ItemSource_Conteo");
                }
            }
        }//SOBRE CARGA DEL METODO DE CUANDO SE INICIA LA APP DE LA VIEW

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
