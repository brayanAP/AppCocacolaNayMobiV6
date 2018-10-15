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
            const string url = "http://localhost:60304/api/inventarios/invacocon";

            HttpResponseMessage response = await FiClient.PostAsync(
                new Uri(string.Format(url, string.Empty)), 
                new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json")
            );
           
            return await response.Content.ReadAsStringAsync();
        }//POST: A INVENTARIOS

        public async Task<string> FicPostExportInventarios()
        {
            var FicSourceInventarios = await (
                from inv in FicLoBDContext.zt_inventarios
                join acu in FicLoBDContext.zt_inventarios_acumulados on inv.IdInventario equals acu.IdInventario
                join con in FicLoBDContext.zt_inventarios_conteos on inv.IdInventario equals con.IdInventario
                select new {inv,acu,con}
                ).GroupBy(x => x).Select(group => new
                {
                    list_zt_inventarios = group.Select(v => v.inv),
                    list_zt_inventarios_acumulados = group.Select(v => v.acu),
                    list_zt_inventarios_conteos = group.Select(v => v.con)
                }).ToListAsync();

            return FicSourceInventarios == null ? await FicPostListInventarios(new zt_inventatios_acumulados_conteos()
            {
                zt_inventarios = FicSourceInventarios.First().list_zt_inventarios as List<zt_inventarios>,
                zt_inventarios_acumulados = FicSourceInventarios.First().list_zt_inventarios_acumulados as List<zt_inventarios_acumulados>,
                zt_inventarios_conteos = FicSourceInventarios.First().list_zt_inventarios_conteos as List<zt_inventarios_conteos>
            }) : "SIN DATOS, QUE EXPORTAR";
        }//METODO DE EXPORT INVENTARIOS

    }//CLASS 
}//NAMESPACE
