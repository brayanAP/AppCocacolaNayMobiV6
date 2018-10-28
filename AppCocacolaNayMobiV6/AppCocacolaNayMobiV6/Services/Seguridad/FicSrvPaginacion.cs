using AppCocacolaNayMobiV6.Data;
using AppCocacolaNayMobiV6.Interfaces.Seguridad;
using AppCocacolaNayMobiV6.Interfaces.SQlite;
using AppCocacolaNayMobiV6.Models;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AppCocacolaNayMobiV6.Services.Seguridad
{
    public class FicSrvPaginacion : IFicSrvPaginacion
    {
        private readonly FicBDContext FicLoBDContext;
        private readonly HttpClient FiClient;

        public FicSrvPaginacion()
        {
            FicLoBDContext = new FicBDContext(DependencyService.Get<IFicConfigSQLite>().FicGetDataBasePath());
            FiClient = new HttpClient();
            FiClient.MaxResponseContentBufferSize = 256000;
        }//CONSTRUCTOR

        private async Task<seg_cat_mod_sub_pag> FicGetListSegCatModSubPag()
        {
            const string url = "http://localhost:54068/api/seguridad/paginacion";

            try
            {
                var response = await FiClient.GetAsync(url);
                return response.IsSuccessStatusCode ? JsonConvert.DeserializeObject<seg_cat_mod_sub_pag>(await response.Content.ReadAsStringAsync()) : null;
            }
            catch (Exception e)
            {
                await new Page().DisplayAlert("ALERTA", e.Message.ToString(), "OK");
                return null;
            }
        }

        //private async Task<seg_cat_modulos> FicExistseg_cat_modulos(Int16 id)
        //{
        //    return await (from t in FicLoBDContext.seg_cat_modulos where t.IdModulo == id select t).SingleOrDefaultAsync();
        //}

        //private async Task<seg_cat_submodulos> FicExistseg_cat_submodulos(Int16 id,Int16 fk)
        //{
        //    return await (from t in FicLoBDContext.seg_cat_submodulos where (t.IdModulo == id && t.IdSubmodulo == fk) select t).SingleOrDefaultAsync();
        //}

        //private async Task<seg_cat_paginas> FicExistseg_cat_paginas(Int16 id, Int16 fk1, Int16 fk2)
        //{
        //    return await (from t in FicLoBDContext.seg_cat_paginas where (t.IdPagina == id && t.IdModulo == fk1 && t.IdSubmodulo == fk2) select t).SingleOrDefaultAsync();
        //}

        //public async Task<bool> FicMetGetWebApiPaginas()
        //{
        //    if (!(CrossConnectivity.Current.IsConnected)) return false;

        //    var FicSourcePaginacion = await FicGetListSegCatModSubPag();

        //    if (FicSourcePaginacion == null) return false;

        //    if(FicSourcePaginacion.seg_cat_modulos.Count != 0)
        //    {
        //        foreach (seg_cat_modulos mod in FicSourcePaginacion.seg_cat_modulos)
        //        {
        //            var respuesta = await FicExistseg_cat_modulos(mod.IdModulo);
        //            if (respuesta != null)
        //            {
        //                try
        //                {
        //                    respuesta.IdModulo = mod.IdModulo;
        //                    respuesta.DesModulo = mod.DesModulo;
        //                    respuesta.Prioridad = mod.Prioridad;
        //                    respuesta.RutaIcono = mod.RutaIcono;
        //                    respuesta.Version = mod.Version;
        //                    respuesta.Abreviatura = mod.Abreviatura;
        //                    respuesta.FechaReg = mod.FechaReg;
        //                    respuesta.UsuarioReg = mod.UsuarioReg;
        //                    respuesta.FechaUltMod = mod.FechaUltMod;
        //                    respuesta.UsuarioMod = mod.UsuarioMod;
        //                    respuesta.Activo = mod.Activo;
        //                    respuesta.Borrado = mod.Borrado;
        //                }
        //                catch (Exception e){}
        //            }
        //            else
        //            {
        //                try
        //                {
        //                    FicLoBDContext.Add(mod);
        //                    await FicLoBDContext.SaveChangesAsync();
        //                }
        //                catch (Exception e){}
        //            }
        //        }
        //    }//IMPORTAR MODULOS

        //    if (FicSourcePaginacion.seg_cat_submodulos.Count != 0)
        //    {
        //        foreach (seg_cat_submodulos mod in FicSourcePaginacion.seg_cat_submodulos)
        //        {
        //            var respuesta = await FicExistseg_cat_submodulos(mod.IdSubmodulo,mod.IdModulo);
        //            if (respuesta != null)
        //            {
        //                try
        //                {
        //                    respuesta.IdModulo = mod.IdModulo;
        //                    respuesta.IdSubmodulo = mod.IdSubmodulo;
        //                    respuesta.DesSubmodulo = mod.DesSubmodulo;
        //                    respuesta.Prioridad = mod.Prioridad;
        //                    respuesta.RutaIcono = mod.RutaIcono;
        //                    respuesta.Version = mod.Version;
        //                    respuesta.Abreviatura = mod.Abreviatura;
        //                    respuesta.FechaReg = mod.FechaReg;
        //                    respuesta.UsuarioReg = mod.UsuarioReg;
        //                    respuesta.FechaUltMod = mod.FechaUltMod;
        //                    respuesta.UsuarioMod = mod.UsuarioMod;
        //                    respuesta.Activo = mod.Activo;
        //                    respuesta.Borrado = mod.Borrado;
        //                }
        //                catch (Exception e) { }
        //            }
        //            else
        //            {
        //                try
        //                {
        //                    FicLoBDContext.Add(mod);
        //                    await FicLoBDContext.SaveChangesAsync();
        //                }
        //                catch (Exception e) { }
        //            }
        //        }
        //    }//IMPORTAR SUB MODULOS

        //    if (FicSourcePaginacion.seg_cat_paginas.Count != 0)
        //    {
        //        foreach (seg_cat_paginas mod in FicSourcePaginacion.seg_cat_paginas)
        //        {
        //            var respuesta = await FicExistseg_cat_paginas(mod.IdPagina, mod.IdModulo, mod.IdSubmodulo);
        //            if (respuesta != null)
        //            {
        //                try
        //                {
        //                    respuesta.IdPagina = mod.IdPagina;
        //                    respuesta.IdModulo = mod.IdModulo;
        //                    respuesta.IdSubmodulo = mod.IdSubmodulo;
        //                    respuesta.DesPagina = mod.DesPagina;
        //                    respuesta.Detalle = mod.Detalle;
        //                    respuesta.Version = mod.Version;
        //                    respuesta.Orden = mod.Orden;
        //                    respuesta.RutaPagina = mod.RutaPagina;
        //                    respuesta.RutaImagen = mod.RutaImagen;
        //                    respuesta.Visible = mod.Visible;
        //                    respuesta.FechaReg = mod.FechaReg;
        //                    respuesta.UsuarioReg = mod.UsuarioReg;
        //                    respuesta.FechaUltMod = mod.FechaUltMod;
        //                    respuesta.UsuarioMod = mod.UsuarioMod;
        //                    respuesta.Activo = mod.Activo;
        //                    respuesta.Borrado = mod.Borrado;
        //                }
        //                catch (Exception e) { }
        //            }
        //            else
        //            {
        //                try
        //                {
        //                    FicLoBDContext.Add(mod);
        //                    await FicLoBDContext.SaveChangesAsync();
        //                }
        //                catch (Exception e) { }
        //            }
        //        }
        //    }//IMPORTAR PAGINAS

        //    return true;
        //}//FicMetGetWebApiPaginas()

    }//CLASS
}//NAMESPACE
