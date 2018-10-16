using AppCocacolaNayMobiV6.Data;
using AppCocacolaNayMobiV6.Interfaces.Inventarios;
using AppCocacolaNayMobiV6.Interfaces.SQlite;
using AppCocacolaNayMobiV6.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AppCocacolaNayMobiV6.Services.Inventarios
{
    public class FicSrvInventariosList : IFicSrvInventariosList
    {
        private readonly FicBDContext FicLoBDContext;

        public FicSrvInventariosList()
        {
            FicLoBDContext = new FicBDContext(DependencyService.Get<IFicConfigSQLite>().FicGetDataBasePath());
        }//CONSTRUCTOR

        public async Task<IEnumerable<zt_inventarios>> FicMetGetListInventarios()
        {
            return await (from inv in FicLoBDContext.zt_inventarios select inv).AsNoTracking().ToListAsync();
        }//TRAER UNA LISTA CON TODOS LOS zt_inventarios

    }//CLASS
}//NAMESPACE
