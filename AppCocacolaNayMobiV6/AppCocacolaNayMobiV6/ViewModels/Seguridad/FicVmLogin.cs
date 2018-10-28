using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace AppCocacolaNayMobiV6.ViewModels.Seguridad
{
    public class FicVmLogin : INotifyPropertyChanged
    {
        public FicVmLogin()
        {

        }//CONSTRUCTOR

        public async void OnAppearing()
        {

        }//CUANDO INICIA LA VENTANA

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
