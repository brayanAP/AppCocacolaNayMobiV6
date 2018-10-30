using AppCocacolaNayMobiV6.Data;
using AppCocacolaNayMobiV6.Interfaces.SQlite;
using AppCocacolaNayMobiV6.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AppCocacolaNayMobiV6.Interfaces.Inventarios;

namespace AppCocacolaNayMobiV6.Services.Inventarios
{
    public class FicSrvExportarWebApi : IFicSrvExportarWebApi
    {
        private readonly FicBDContext FicLoBDContext;
        private readonly HttpClient FiClient;

        public FicSrvExportarWebApi()
        {
            FicLoBDContext = new FicBDContext(DependencyService.Get<IFicConfigSQLite>().FicGetDataBasePath());
            FiClient = new HttpClient();
            FiClient.MaxResponseContentBufferSize = 256000;

        }//CONSTRUCTOR

        private async Task<string> FicPostListInventarios(zt_inventatios_acumulados_conteos item)
        {
            const string url = "http://localhost:54068/api/inventarios/invacocon/export";

            HttpResponseMessage response = await FiClient.PostAsync(
                new Uri(string.Format(url, string.Empty)), 
                new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json")
            );
           
            return await response.Content.ReadAsStringAsync();
        }//POST: A INVENTARIOS

        public async Task<string> FicPostExportInventarios(int idInv)
        {
            string FicMensaje = "ERROR: \n";
            int id;
            List<int> inv = new List<int>();

            foreach (zt_inventarios_acumulados a in await (from a in FicLoBDContext.zt_inventarios_acumulados select a).ToListAsync())
            {
                id = a.IdInventario;

                if(a.CantidadFisica == null && id!= 0)
                {
                    if(!inv.Contains(id))
                    {
                        FicMensaje += "-IMPOSIBLE EXPORTAR EL INVENTARIO " + a.IdInventario + ": \n";
                        var sku = await (from b in FicLoBDContext.zt_inventarios_acumulados where b.IdInventario == id select b).ToListAsync();

                        if (sku != null && sku.Count != 0) foreach (zt_inventarios_acumulados c in sku) FicMensaje += "    *-> SKU: " + c.IdSKU + "\n";
                    }

                    id = 0;
                    inv.Add(a.IdInventario);
                }
            }

            if (FicMensaje != "ERROR: \n")
            {
                await new Page().DisplayAlert("ALERTA", "IMPOSIBLE EXPORTAR.", "OK");
                return FicMensaje;
            }

            if(idInv == 0)
                return await FicPostListInventarios(new zt_inventatios_acumulados_conteos()
                {
                    zt_inventarios = await (from a in FicLoBDContext.zt_inventarios select a).AsNoTracking().ToListAsync(),
                    zt_inventarios_acumulados = await (from a in FicLoBDContext.zt_inventarios_acumulados select a).AsNoTracking().ToListAsync(),
                    zt_inventarios_conteos = await (from a in FicLoBDContext.zt_inventarios_conteos select a).AsNoTracking().ToListAsync()
                });
 

            return await FicPostListInventarios(new zt_inventatios_acumulados_conteos()
            {
                zt_inventarios = await (from a in FicLoBDContext.zt_inventarios where a.IdInventario == idInv select a).AsNoTracking().ToListAsync(),
                zt_inventarios_acumulados = await (from a in FicLoBDContext.zt_inventarios_acumulados where a.IdInventario == idInv select a).AsNoTracking().ToListAsync(),
                zt_inventarios_conteos = await (from a in FicLoBDContext.zt_inventarios_conteos where a.IdInventario == idInv select a).AsNoTracking().ToListAsync()
            });
        }//METODO DE EXPORT INVENTARIOS

    }//CLASS 
}//NAMESPACE
