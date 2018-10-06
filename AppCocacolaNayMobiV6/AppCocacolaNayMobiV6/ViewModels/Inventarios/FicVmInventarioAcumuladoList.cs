using AppCocacolaNayMobiV6.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace AppCocacolaNayMobiV6.ViewModels.Inventarios
{
    public class FicVmInventarioAcumuladoList : INotifyPropertyChanged
    {
        public object FicNavigationContext { get; set; }

        private ObservableCollection<zt_inventarios_acumulados> _FicSfDataGrid_ItemSource_Acomulado;

        public ObservableCollection<zt_inventarios_acumulados> FicSfDataGrid_ItemSource_Acomulado { get { return _FicSfDataGrid_ItemSource_Acomulado; } }

        public FicVmInventarioAcumuladoList()
        {

        }//CONSTRUCTOR

        public async void OnAppearing()
        {

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
