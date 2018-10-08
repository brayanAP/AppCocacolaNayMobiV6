using AppCocacolaNayMobiV6.Interfaces.Inventarios;
using AppCocacolaNayMobiV6.Interfaces.Navegacion;
using AppCocacolaNayMobiV6.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace AppCocacolaNayMobiV6.ViewModels.Inventarios
{
    public class FicVmInventarioAcumuladoList : INotifyPropertyChanged
    {
        public object FicNavigationContext { get; set; }

        private ObservableCollection<zt_inventarios_acumulados> _FicSfDataGrid_ItemSource_Acomulado;

        public ObservableCollection<zt_inventarios_acumulados> FicSfDataGrid_ItemSource_Acomulado { get { return _FicSfDataGrid_ItemSource_Acomulado; } }

        private IFicSrvInventarioAcumuladoList IFicSrvInventarioAcumuladoList;
        private IFicSrvNavigationInventario IFicSrvNavigationInventario;

        public FicVmInventarioAcumuladoList(IFicSrvNavigationInventario IFicSrvNavigationInventario,IFicSrvInventarioAcumuladoList IFicSrvInventarioAcumuladoList)
        {
            this.IFicSrvNavigationInventario = IFicSrvNavigationInventario;
            this.IFicSrvInventarioAcumuladoList = IFicSrvInventarioAcumuladoList;
        }//CONSTRUCTOR

        public async void OnAppearing()
        {
            try
            {
                var FicSourceZt_Inventarios = FicNavigationContext as zt_inventarios;

                _FicSfDataGrid_ItemSource_Acomulado = new ObservableCollection<zt_inventarios_acumulados>();
                var t = await IFicSrvInventarioAcumuladoList.FicMetGetAcumuladosList(FicSourceZt_Inventarios.IdInventario);
                await new Page().DisplayAlert("ERROR", t[0].IdSKU, "OK");
            }
            catch(Exception e)
            {
                await new Page().DisplayAlert("ALERTA", e.Message.ToString(), "OK");
            }
        }//OnAppearing()

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
