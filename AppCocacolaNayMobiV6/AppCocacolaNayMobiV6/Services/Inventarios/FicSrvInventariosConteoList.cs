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
    public class FicSrvInventariosConteoList : IFicSrvInventariosConteoList
    {
        private readonly FicBDContext FicLoBDContext;

        public FicSrvInventariosConteoList()
        {
            FicLoBDContext = new FicBDContext(DependencyService.Get<IFicConfigSQLite>().FicGetDataBasePath());
        }//CONSTRUCTOR

        public async Task<IList<zt_inventarios_conteos>> FicMetGetListInventariosConteos(int IdInventario)
        {
            return await (from conteo in FicLoBDContext.zt_inventarios_conteos
                          join inv in FicLoBDContext.zt_inventarios on conteo.IdInventario equals inv.IdInventario
                          where inv.IdInventario == IdInventario
                          select conteo).AsNoTracking().ToListAsync();
        }//LIST ALL

        public async Task<IList<zt_inventarios_conteos>> FicMetGetListInventariosConteos(int IdInventario, zt_inventarios_acumulados item)
        {
            return await (from conteo in FicLoBDContext.zt_inventarios_conteos
                          join inv in FicLoBDContext.zt_inventarios on conteo.IdInventario equals inv.IdInventario
                          where inv.IdInventario == IdInventario && conteo.IdSKU == item.IdSKU
                          select conteo).AsNoTracking().ToListAsync();
        }//LIST ALL

    }//CLASS
}//NAMESPACE
