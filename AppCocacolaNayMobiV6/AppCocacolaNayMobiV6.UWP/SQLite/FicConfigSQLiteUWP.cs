using AppCocacolaNayMobiV6.Interfaces.SQlite;
using AppCocacolaNayMobiV6.UWP.SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Xamarin.Forms;

[assembly: Dependency(typeof(FicConfigSQLiteUWP))]
namespace AppCocacolaNayMobiV6.UWP.SQLite
{
    class FicConfigSQLiteUWP : IFicConfigSQLite
    {
        public string FicGetDataBasePath()
        {
            return Path.Combine(ApplicationData.Current.LocalFolder.Path, FicAppSettings.FicDataBaseName);
        }//TRAER LA RUTA FISICA DONDE SE GUARDA LA BASE DE DATOS EN UWP
    }//CLASS
}//NAMESPACE
