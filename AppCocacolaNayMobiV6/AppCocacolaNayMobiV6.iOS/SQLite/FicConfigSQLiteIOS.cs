using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AppCocacolaNayMobiV6.Interfaces.SQlite;
using AppCocacolaNayMobiV6.iOS.SQLite;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(FicConfigSQLiteIOS))]
namespace AppCocacolaNayMobiV6.iOS.SQLite
{
    class FicConfigSQLiteIOS : IFicConfigSQLite
    {
        public string FicGetDataBasePath()
        {
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

            if (!Directory.Exists(libFolder))
            {
                Directory.CreateDirectory(libFolder);
            }

            return Path.Combine(libFolder, FicAppSettings.FicDataBaseName);
        }//TRAER LA RUTA FISICA DE IOS DONDE SE GUARDA LA BD SQLITE
    }//CLASS
}//NAMESPACE