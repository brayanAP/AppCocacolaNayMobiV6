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

        public async Task<string> FicPostExportInventarios()
        {
            return await FicPostListInventarios(new zt_inventatios_acumulados_conteos()
            {
                zt_inventarios = await (from a in FicLoBDContext.zt_inventarios select a).ToListAsync(),
                zt_inventarios_acumulados = await (from a in FicLoBDContext.zt_inventarios_acumulados select a).ToListAsync(),
                zt_inventarios_conteos = await (from a in FicLoBDContext.zt_inventarios_conteos select a).ToListAsync()
            });
        }//METODO DE EXPORT INVENTARIOS

    }//CLASS 
}//NAMESPACE
