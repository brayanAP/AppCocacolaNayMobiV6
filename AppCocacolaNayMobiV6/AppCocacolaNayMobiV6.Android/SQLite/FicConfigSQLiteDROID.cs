using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AppCocacolaNayMobiV6.Droid.SQLite;
using AppCocacolaNayMobiV6.Interfaces.SQlite;
using Xamarin.Forms;

[assembly: Dependency(typeof(FicConfigSQLiteDROID))]
namespace AppCocacolaNayMobiV6.Droid.SQLite
{
    class FicConfigSQLiteDROID : IFicConfigSQLite
    {
        public string FicGetDataBasePath()
        {

            var FicPathFile = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
            var FicDirectorioDB = FicPathFile.Path;
            FicDirectorioDB = FicDirectorioDB + "/CocacolaNay/";
            string FicPathDB = Path.Combine(FicDirectorioDB, FicAppSettings.FicDataBaseName);
            return FicPathDB;
        }//TRAER LA RUTA FISICA DONDE ESTARA LA BASE DE DATOS SQLITE
    }//CLASS
}//NAMESPACE