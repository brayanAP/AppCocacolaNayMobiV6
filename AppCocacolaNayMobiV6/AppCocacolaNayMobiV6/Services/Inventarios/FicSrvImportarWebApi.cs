using AppCocacolaNayMobiV6.Data;
using AppCocacolaNayMobiV6.Interfaces.Inventarios;
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

namespace AppCocacolaNayMobiV6.Services.Inventarios
{
    public class FicSrvImportarWebApi : IFicSrvImportarWebApi
    {
        private readonly FicBDContext FicLoBDContext;
        private HttpClient FiClient;

        public FicSrvImportarWebApi()
        {
            FicLoBDContext = new FicBDContext(DependencyService.Get<IFicConfigSQLite>().FicGetDataBasePath());
            FiClient = new HttpClient();
            FiClient.MaxResponseContentBufferSize = 256000;
        }//CONSTRUCTOR

        private async Task<zt_inventatios_acumulados_conteos> FicGetListInventarioActualiza()
        {
            const string url = "http://localhost:60304/api/inventarios/invacocon";

            try
            {
                var response = await FiClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<zt_inventatios_acumulados_conteos>(content);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                await new Page().DisplayAlert("ALERTA", e.Message.ToString(), "OK");
                return null;
            }

        }//GET: A INVENTARIOS

        private async Task<zt_catalogos_productos_medidas_cedi_almacenes> FicGetListCatalogosActualiza()
        {
            const string url = "http://localhost:60304/api/inventarios/catalogos";

            try
            {
                var response = await FiClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<zt_catalogos_productos_medidas_cedi_almacenes>(content);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                await new Page().DisplayAlert("ALERTA", e.Message.ToString(), "OK");
                return null;
            }
        }

        private async Task<zt_inventarios> FicExistzt_inventarios(int id)
        {
            return await (from inv in FicLoBDContext.zt_inventarios where inv.IdInventario == id select inv).SingleOrDefaultAsync();
        }//buscar en local

        private async Task<zt_inventarios_conteos> FicExistzt_inventarios_conteos(int idinv, int IdAlmacen, string codigob, int NumCont, string ubicacion)
        {
            return await (from con in FicLoBDContext.zt_inventarios_conteos where con.IdInventario == idinv && con.IdAlmacen ==IdAlmacen && con.IdSKU == codigob && con.NumConteo == NumCont && con.IdUbicacion == ubicacion select con).SingleOrDefaultAsync();
        }//buscar en local

        private async Task<zt_inventarios_acumulados> FicExistzt_inventarios_acumulados(int idinv, string codigob)
        {
            return await (from acu in FicLoBDContext.zt_inventarios_acumulados where acu.IdInventario == idinv && acu.IdSKU == codigob  select acu).SingleOrDefaultAsync();
        }//buscar en local

        private async Task<zt_cat_productos> FicExistzt_cat_productos(string sku)
        {
            return await (from acu in FicLoBDContext.zt_cat_productos where acu.IdSKU == sku select acu).SingleOrDefaultAsync();
        }

        private async Task<zt_cat_unidad_medidas> FicExistzt_cat_unidad_medidas(Int16 id)
        {
            return await (from acu in FicLoBDContext.zt_cat_unidad_medidas where acu.IdUnidadMedida == id select acu).SingleOrDefaultAsync();
        }

        private async Task<zt_cat_productos_medidas> FicExistzt_cat_productos_medidas(string sku,Int16 id)
        {
            return await (from acu in FicLoBDContext.zt_cat_productos_medidas where acu.IdSKU == sku && acu.IdUnidadMedida == id select acu).SingleOrDefaultAsync();
        }

        private async Task<zt_cat_cedis> FicExistzt_cat_cedis(Int16 id)
        {
            return await (from acu in FicLoBDContext.zt_cat_cedis where acu.IdCEDI == id select acu).SingleOrDefaultAsync();
        }

        private async Task<zt_cat_almacenes> FicExistzt_cat_almacenes(Int16 id)
        {
            return await (from acu in FicLoBDContext.zt_cat_almacenes where acu.IdAlmacen == id select acu).SingleOrDefaultAsync();
        }

        public async Task<string> FicGetImportInventarios()
        {
            string FicMensaje = "IMPORTACION: \n";
            try
            {
                var FicGetReultREST = await FicGetListInventarioActualiza();
                if (FicGetReultREST != null)
                {
                    if (FicGetReultREST.zt_inventarios != null)
                    {
                        FicMensaje += "IMPORTANDO: zt_inventarios \n";
                        foreach (zt_inventarios inv in FicGetReultREST.zt_inventarios)
                        {
                            var respuesta = await FicExistzt_inventarios(inv.IdInventario);
                            if (respuesta != null)
                            {
                                try
                                {
                                    respuesta.IdInventario = inv.IdInventario;
                                    respuesta.IdCEDI = inv.IdCEDI;
                                    respuesta.FechaReg = inv.FechaReg;
                                    respuesta.UsuarioReg = inv.UsuarioReg;
                                    respuesta.FechaUltMod = inv.FechaUltMod;
                                    respuesta.UsuarioMod = inv.UsuarioMod;
                                    respuesta.Activo = inv.Activo;
                                    respuesta.Borrado = inv.Borrado;
                                    FicLoBDContext.Update(respuesta);
                                    await FicLoBDContext.SaveChangesAsync();
                                    FicMensaje += "-UPDATE-> IdInventario: " + inv.IdInventario + " \n";
                                }
                                catch (Exception e)
                                {
                                    FicMensaje += "-ALERTA-> " + e.Message.ToString() + " \n";
                                }
                            }
                            else
                            {
                                try
                                {
                                    FicLoBDContext.Add(inv);
                                    await FicLoBDContext.SaveChangesAsync();
                                    FicMensaje += "-INSERT-> IdInventario: " + inv.IdInventario + " \n";
                                }
                                catch (Exception e)
                                {
                                    FicMensaje += "-ALERTA-> " + e.Message.ToString() + " \n";
                                }
                            }
                        }
                    }
                    else FicMensaje += "-> SIN DATOS. \n";

                    if (FicGetReultREST.zt_inventarios_conteos != null)
                    {
                        FicMensaje += "IMPORTANDO: zt_inventarios_conteos \n";
                        foreach (zt_inventarios_conteos inv in FicGetReultREST.zt_inventarios_conteos)
                        {
                            var respuesta = await FicExistzt_inventarios_conteos(inv.IdInventario, inv.IdAlmacen, inv.IdSKU, inv.NumConteo, inv.IdUbicacion);
                            if (respuesta != null)
                            {
                                try
                                {
                                    respuesta.IdInventario = inv.IdInventario;
                                    respuesta.IdAlmacen = inv.IdAlmacen;
                                    respuesta.NumConteo = inv.NumConteo;
                                    respuesta.IdSKU = inv.IdSKU;
                                    respuesta.CodigoBarras = inv.CodigoBarras;
                                    respuesta.IdUbicacion = inv.IdUbicacion;
                                    respuesta.CantidadFisica = inv.CantidadFisica;
                                    respuesta.IdUnidadMedida = inv.IdUnidadMedida;
                                    respuesta.CantidadPZA = inv.CantidadPZA;
                                    respuesta.Lote = inv.Lote;
                                    respuesta.FechaReg = inv.FechaReg;
                                    respuesta.UsuarioReg = inv.UsuarioReg;
                                    respuesta.Activo = inv.Activo;
                                    respuesta.Borrado = inv.Borrado;
                                    FicLoBDContext.Update(respuesta);
                                    await FicLoBDContext.SaveChangesAsync();
                                    FicMensaje += "-UPDATE-> IdInventario: " + inv.IdInventario + " ,IdAlmacen: " + inv.IdAlmacen + " ,IdSKU: " + inv.IdSKU + " ,NumConteo: " + inv.NumConteo + " ,IdUbicacion: " + inv.IdUbicacion + " \n";
                                }
                                catch (Exception e)
                                {
                                    FicMensaje += "-ALERTA-> " + e.Message.ToString() + " \n";
                                }
                            }
                            else
                            {
                                try
                                {
                                    FicLoBDContext.Add(inv);
                                    await FicLoBDContext.SaveChangesAsync();
                                    FicMensaje += "-INSERT-> IdInventario: " + inv.IdInventario + " ,IdAlmacen: " + inv.IdAlmacen + " ,IdSKU: " + inv.IdSKU + " ,NumConteo: " + inv.NumConteo + " ,IdUbicacion: " + inv.IdUbicacion + " \n";
                                }
                                catch (Exception e)
                                {
                                    FicMensaje += "-ALERTA-> " + e.Message.ToString() + " \n";
                                }
                            }
                        }
                    }
                    else FicMensaje += "-> SIN DATOS. \n";

                    if (FicGetReultREST.zt_inventarios_acumulados != null)
                    {
                        FicMensaje += "IMPORTANDO: zt_inventarios_acumulados \n";
                        foreach (zt_inventarios_acumulados inv in FicGetReultREST.zt_inventarios_acumulados)
                        {
                            var respuesta = await FicExistzt_inventarios_acumulados(inv.IdInventario, inv.IdSKU);
                            if (respuesta != null)
                            {
                                try
                                {
                                    respuesta.IdInventario = inv.IdInventario;
                                    respuesta.IdSKU = inv.IdSKU;
                                    respuesta.CantidadTeorica = inv.CantidadTeorica;
                                    respuesta.CantidadFisica = inv.CantidadFisica;
                                    respuesta.Diferencia = inv.Diferencia;
                                    respuesta.IdUnidadMedida = inv.IdUnidadMedida;
                                    respuesta.FechaReg = inv.FechaReg;
                                    respuesta.UsuarioReg = inv.UsuarioReg;
                                    respuesta.FechaUltMod = inv.FechaUltMod;
                                    respuesta.UsuarioMod = inv.UsuarioMod;
                                    respuesta.Activo = inv.Activo;
                                    respuesta.Borrado = inv.Borrado;
                                    FicLoBDContext.Update(respuesta);
                                    await FicLoBDContext.SaveChangesAsync();
                                    FicMensaje += "-UPDATE-> IdInventario: " + inv.IdInventario + " ,IdSKU: " + inv.IdSKU + " \n";
                                }
                                catch (Exception e)
                                {
                                    FicMensaje += "-ALERTA-> " + e.Message.ToString() + " \n";
                                }
                            }
                            else
                            {
                                try
                                {
                                    FicLoBDContext.Add(inv);
                                    await FicLoBDContext.SaveChangesAsync();
                                    FicMensaje += "-INSERT-> IdInventario: " + inv.IdInventario + " ,IdSKU: " + inv.IdSKU + " \n";
                                }
                                catch (Exception e)
                                {
                                    FicMensaje += "-ALERTA-> " + e.Message.ToString() + " \n";
                                }
                            }
                        }
                    }
                    else FicMensaje += "-> SIN DATOS. \n";
                }
                else FicMensaje += "ALERTA: " + "SIN DATOS \n";
            }
            catch (Exception e)
            {
                FicMensaje += "ALERTA: " + e.Message.ToString() + "\n";
            }
            return FicMensaje;
        }//FicGetImportInventarios()

        public async Task<string> FicGetImportCatalogos()
        {
            string FicMensaje = "IMPORTACION: \n";
            try
            {
                var FicGetReultREST = await FicGetListCatalogosActualiza();
                if (FicGetReultREST != null)
                {
                    if (FicGetReultREST.zt_cat_productos != null)
                    {
                        FicMensaje += "IMPORTANDO: zt_cat_productos \n";
                        foreach (zt_cat_productos inv in FicGetReultREST.zt_cat_productos)
                        {
                            var respuesta = await FicExistzt_cat_productos(inv.IdSKU);
                            if (respuesta != null)
                            {
                                try
                                {
                                    respuesta.IdSKU = inv.IdSKU;
                                    respuesta.CodigoBarras = inv.CodigoBarras;
                                    respuesta.DesSKU = inv.DesSKU;
                                    respuesta.FechaReg = inv.FechaReg;
                                    respuesta.UsuarioReg = inv.UsuarioReg;
                                    respuesta.FechaUltMod = inv.FechaUltMod;
                                    respuesta.UsuarioMod = inv.UsuarioMod;
                                    respuesta.Activo = inv.Activo;
                                    respuesta.Borrado = inv.Borrado;
                                    await FicLoBDContext.SaveChangesAsync();
                                    FicMensaje += "-UPDATE-> IdSKU: " + inv.IdSKU + " \n";
                                }
                                catch (Exception e)
                                {
                                    FicMensaje += "-ALERTA-> " + e.Message.ToString() + " \n";
                                }
                            }
                            else
                            {
                                try
                                {
                                    FicLoBDContext.Add(inv);
                                    await FicLoBDContext.SaveChangesAsync();
                                    FicMensaje += "-INSERT-> IdSKU: " + inv.IdSKU + " \n";
                                }
                                catch (Exception e)
                                {
                                    FicMensaje += "-ALERTA-> " + e.Message.ToString() + " \n";
                                }
                            }
                        }
                    }
                    else FicMensaje += "-> SIN DATOS. \n";

                    if (FicGetReultREST.zt_cat_unidad_medidas != null)
                    {
                        FicMensaje += "IMPORTANDO: zt_cat_unidad_medidas \n";
                        foreach (zt_cat_unidad_medidas inv in FicGetReultREST.zt_cat_unidad_medidas)
                        {
                            var respuesta = await FicExistzt_cat_unidad_medidas(inv.IdUnidadMedida);
                            if (respuesta != null)
                            {
                                try
                                {
                                    respuesta.IdUnidadMedida = inv.IdUnidadMedida;
                                    respuesta.DesUMedida = inv.DesUMedida;
                                    respuesta.FechaReg = inv.FechaReg;
                                    respuesta.UsuarioReg = inv.UsuarioReg;
                                    respuesta.FechaUltMod = inv.FechaUltMod;
                                    respuesta.UsuarioMod = inv.UsuarioMod;
                                    respuesta.Activo = inv.Activo;
                                    respuesta.Borrado = inv.Borrado;
                                    await FicLoBDContext.SaveChangesAsync();
                                    FicMensaje += "-UPDATE-> IdUnidadMedida: " + inv.IdUnidadMedida + " \n";
                                }
                                catch (Exception e)
                                {
                                    FicMensaje += "-ALERTA-> " + e.Message.ToString() + " \n";
                                }
                            }
                            else
                            {
                                try
                                {
                                    FicLoBDContext.Add(inv);
                                    await FicLoBDContext.SaveChangesAsync();
                                    FicMensaje += "-INSERT-> IdUnidadMedida: " + inv.IdUnidadMedida + " \n";
                                }
                                catch (Exception e)
                                {
                                    FicMensaje += "-ALERTA-> " + e.Message.ToString() + " \n";
                                }
                            }
                        }
                    }
                    else FicMensaje += "-> SIN DATOS. \n";

                    if (FicGetReultREST.zt_cat_productos_medidas != null)
                    {
                        FicMensaje += "IMPORTANDO: zt_cat_productos_medidas \n";
                        foreach (zt_cat_productos_medidas inv in FicGetReultREST.zt_cat_productos_medidas)
                        {
                            var respuesta = await FicExistzt_cat_productos_medidas(inv.IdSKU, inv.IdUnidadMedida);
                            if (respuesta != null)
                            {
                                try
                                {
                                    respuesta.IdSKU = inv.IdSKU;
                                    respuesta.IdUnidadMedida = inv.IdUnidadMedida;
                                    respuesta.CantidadPZA = inv.CantidadPZA;
                                    respuesta.FechaReg = inv.FechaReg;
                                    respuesta.UsuarioReg = inv.UsuarioReg;
                                    respuesta.FechaUltMod = inv.FechaUltMod;
                                    respuesta.UsuarioMod = inv.UsuarioMod;
                                    respuesta.Activo = inv.Activo;
                                    respuesta.Borrado = inv.Borrado;
                                    await FicLoBDContext.SaveChangesAsync();
                                    FicMensaje += "-UPDATE-> IdUnidadMedida: " + inv.IdUnidadMedida + " ,IdSKU: " + inv.IdSKU + " \n";
                                }
                                catch (Exception e)
                                {
                                    FicMensaje += "-ALERTA-> " + e.Message.ToString() + " \n";
                                }
                            }
                            else
                            {
                                try
                                {
                                    FicLoBDContext.Add(inv);
                                    await FicLoBDContext.SaveChangesAsync();
                                    FicMensaje += "-INSERT-> IdUnidadMedida: " + inv.IdUnidadMedida + " ,IdSKU: " + inv.IdSKU + " \n";
                                }
                                catch (Exception e)
                                {
                                    FicMensaje += "-ALERTA-> " + e.Message.ToString() + " \n";
                                }
                            }
                        }
                    }
                    else FicMensaje += "-> SIN DATOS. \n";

                    if (FicGetReultREST.zt_cat_cedis != null)
                    {
                        FicMensaje += "IMPORTANDO: zt_cat_cedis \n";
                        foreach (zt_cat_cedis inv in FicGetReultREST.zt_cat_cedis)
                        {
                            var respuesta = await FicExistzt_cat_cedis(inv.IdCEDI);
                            if (respuesta != null)
                            {
                                try
                                {
                                    respuesta.IdCEDI = inv.IdCEDI;
                                    respuesta.DesCEDI = inv.DesCEDI;
                                    respuesta.FechaReg = inv.FechaReg;
                                    respuesta.UsuarioReg = inv.UsuarioReg;
                                    respuesta.FechaUltMod = inv.FechaUltMod;
                                    respuesta.UsuarioMod = inv.UsuarioMod;
                                    respuesta.Activo = inv.Activo;
                                    respuesta.Borrado = inv.Borrado;
                                    await FicLoBDContext.SaveChangesAsync();
                                    FicMensaje += "-UPDATE-> IdCEDI: " + inv.IdCEDI + " \n";
                                }
                                catch (Exception e)
                                {
                                    FicMensaje += "-ALERTA-> " + e.Message.ToString() + " \n";
                                }
                            }
                            else
                            {
                                try
                                {
                                    FicLoBDContext.Add(inv);
                                    await FicLoBDContext.SaveChangesAsync();
                                    FicMensaje += "-INSERT-> IdCEDI: " + inv.IdCEDI + " \n";
                                }
                                catch (Exception e)
                                {
                                    FicMensaje += "-ALERTA-> " + e.Message.ToString() + " \n";
                                }
                            }
                        }
                    }
                    else FicMensaje += "-> SIN DATOS. \n";

                    if (FicGetReultREST.zt_cat_almacenes != null)
                    {
                        FicMensaje += "IMPORTANDO: zt_cat_almacenes \n";
                        foreach (zt_cat_almacenes inv in FicGetReultREST.zt_cat_almacenes)
                        {
                            var respuesta = await FicExistzt_cat_almacenes(inv.IdAlmacen);
                            if (respuesta != null)
                            {
                                try
                                {
                                    respuesta.IdAlmacen = inv.IdAlmacen;
                                    respuesta.IdCEDI = inv.IdCEDI;
                                    respuesta.DesAlmacen = inv.DesAlmacen;
                                    respuesta.FechaReg = inv.FechaReg;
                                    respuesta.UsuarioReg = inv.UsuarioReg;
                                    respuesta.FechaUltMod = inv.FechaUltMod;
                                    respuesta.UsuarioMod = inv.UsuarioMod;
                                    respuesta.Activo = inv.Activo;
                                    respuesta.Borrado = inv.Borrado;
                                    await FicLoBDContext.SaveChangesAsync();
                                    FicMensaje += "-UPDATE-> IdAlmacen: " + inv.IdAlmacen + " \n";
                                }
                                catch (Exception e)
                                {
                                    FicMensaje += "-ALERTA-> " + e.Message.ToString() + " \n";
                                }
                            }
                            else
                            {
                                try
                                {
                                    FicLoBDContext.Add(inv);
                                    await FicLoBDContext.SaveChangesAsync();
                                    FicMensaje += "-INSERT-> IdAlmacen: " + inv.IdAlmacen + " \n";
                                }
                                catch (Exception e)
                                {
                                    FicMensaje += "-ALERTA-> " + e.Message.ToString() + " \n";
                                }
                            }
                        }
                    }
                    else FicMensaje += "-> SIN DATOS. \n";
                }
                else FicMensaje += "ALERTA: " + "SIN DATOS \n";
            }
            catch (Exception e)
            {
                FicMensaje += "ALERTA: " + e.Message.ToString() + "\n";
            }
            return FicMensaje;
        }
    }//CLASS
}//NAMESPACE
