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
        private readonly HttpClient FiClient;

        public FicSrvImportarWebApi()
        {
            FicLoBDContext = new FicBDContext(DependencyService.Get<IFicConfigSQLite>().FicGetDataBasePath());
            FiClient = new HttpClient();
            FiClient.MaxResponseContentBufferSize = 256000;
        }//CONSTRUCTOR

        private async Task<zt_inventatios_acumulados_conteos> FicGetListInventarioActualiza(int id=0)
        {
            try
            {
                string url = "";
                if (id != 0) url = "http://localhost:54068/api/inventarios/invacoconid" + "?id=" + id;
                else url = "http://localhost:54068/api/inventarios/invacocon";


                var response = await FiClient.GetAsync(url);
               return response.IsSuccessStatusCode ? JsonConvert.DeserializeObject<zt_inventatios_acumulados_conteos>(await response.Content.ReadAsStringAsync()) : null;
            }
            catch (Exception e)
            {
                await new Page().DisplayAlert("ALERTA", e.Message.ToString(), "OK");
                return null;
            }
        }//GET: A INVENTARIOS

        private async Task<zt_catalogos_productos_medidas_cedi_almacenes> FicGetListCatalogosActualiza()
        {
            const string url = "http://localhost:54068/api/inventarios/catalogos";

            try
            {
                var response = await FiClient.GetAsync(url);
                return response.IsSuccessStatusCode ? JsonConvert.DeserializeObject<zt_catalogos_productos_medidas_cedi_almacenes>(await response.Content.ReadAsStringAsync()) : null;
            }
            catch (Exception e)
            {
                await new Page().DisplayAlert("ALERTA", e.Message.ToString(), "OK");
                return null;
            }
        }

        private async Task<zt_inventarios> FicExistzt_inventarios(int id)
        {
            return await (from inv in FicLoBDContext.zt_inventarios where inv.IdInventario == id select inv).AsNoTracking().SingleOrDefaultAsync();
        }//buscar en local

        private async Task<zt_inventarios_conteos> FicExistzt_inventarios_conteos(int idinv, int IdAlmacen, string codigob, int NumCont, string ubicacion)
        {
            return await (from con in FicLoBDContext.zt_inventarios_conteos where con.IdInventario == idinv && con.IdAlmacen ==IdAlmacen && con.IdSKU == codigob && con.NumConteo == NumCont && con.IdUbicacion == ubicacion select con).AsNoTracking().SingleOrDefaultAsync();
        }//buscar en local

        private async Task<zt_inventarios_acumulados> FicExistzt_inventarios_acumulados(int idinv, string codigob)
        {
            return await (from acu in FicLoBDContext.zt_inventarios_acumulados where acu.IdInventario == idinv && acu.IdSKU == codigob  select acu).AsNoTracking().SingleOrDefaultAsync();
        }//buscar en local

        private async Task<zt_cat_productos> FicExistzt_cat_productos(string sku)
        {
            return await (from acu in FicLoBDContext.zt_cat_productos where acu.IdSKU == sku select acu).AsNoTracking().SingleOrDefaultAsync();
        }

        private async Task<zt_cat_unidad_medidas> FicExistzt_cat_unidad_medidas(string id)
        {
            return await (from acu in FicLoBDContext.zt_cat_unidad_medidas where acu.IdUnidadMedida == id select acu).AsNoTracking().SingleOrDefaultAsync();
        }

        private async Task<zt_cat_productos_medidas> FicExistzt_cat_productos_medidas(string sku,string id)
        {
            return await (from acu in FicLoBDContext.zt_cat_productos_medidas where acu.IdSKU == sku && acu.IdUnidadMedida == id select acu).AsNoTracking().SingleOrDefaultAsync();
        }

        private async Task<zt_cat_cedis> FicExistzt_cat_cedis(Int16 id)
        {
            return await (from acu in FicLoBDContext.zt_cat_cedis where acu.IdCEDI == id select acu).AsNoTracking().SingleOrDefaultAsync();
        }

        private async Task<zt_cat_almacenes> FicExistzt_cat_almacenes(Int16 id)
        {
            return await (from acu in FicLoBDContext.zt_cat_almacenes where acu.IdAlmacen == id select acu).SingleOrDefaultAsync();
        }

        public async Task<string> FicGetImportInventarios(int id=0)
        {
            string FicMensaje = "";
            try
            {
                FicMensaje = "IMPORTACION: \n";
                var FicGetReultREST = new zt_inventatios_acumulados_conteos();

                if (id!=0) FicGetReultREST = await FicGetListInventarioActualiza(id);
                else FicGetReultREST = await FicGetListInventarioActualiza();

                if (FicGetReultREST != null && FicGetReultREST.zt_inventarios != null)
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
                               // FicLoBDContext.Update(respuesta);
                                FicMensaje +=  await FicLoBDContext.SaveChangesAsync() > 0 ? "-UPDATE-> IdInventario: " + inv.IdInventario + " \n" : "-NO NECESITO ACTUALIZAR->  IdInventario: " + inv.IdInventario + " \n";
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
                                FicMensaje +=  await FicLoBDContext.SaveChangesAsync() > 0 ? "-INSERT-> IdInventario: " + inv.IdInventario + " \n" : "-ERROR EN INSERT-> IdInventario: " + inv.IdInventario + " \n";
                            }
                            catch (Exception e)
                            {
                                FicMensaje += "-ALERTA-> " + e.Message.ToString() + " \n";
                            }
                        }
                    }
                }
                else FicMensaje += "-> SIN DATOS. \n";

                if (FicGetReultREST != null && FicGetReultREST.zt_inventarios_conteos != null)
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
                                //FicLoBDContext.Update(respuesta);
                                FicMensaje +=  await FicLoBDContext.SaveChangesAsync() > 0 ? "-UPDATE-> IdInventario: " + inv.IdInventario + " ,IdAlmacen: " + inv.IdAlmacen + " ,IdSKU: " + inv.IdSKU + " ,NumConteo: " + inv.NumConteo + " ,IdUbicacion: " + inv.IdUbicacion + " \n" : "-NO NECESITO ACTUALIZAR-> IdInventario: " + inv.IdInventario + " ,IdAlmacen: " + inv.IdAlmacen + " ,IdSKU: " + inv.IdSKU + " ,NumConteo: " + inv.NumConteo + " ,IdUbicacion: " + inv.IdUbicacion + " \n";
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
                                FicMensaje +=  await FicLoBDContext.SaveChangesAsync() > 0 ? "-INSERT-> IdInventario: " + inv.IdInventario + " ,IdAlmacen: " + inv.IdAlmacen + " ,IdSKU: " + inv.IdSKU + " ,NumConteo: " + inv.NumConteo + " ,IdUbicacion: " + inv.IdUbicacion + " \n" : "-ERROR EN INSERT-> IdInventario: " + inv.IdInventario + " ,IdAlmacen: " + inv.IdAlmacen + " ,IdSKU: " + inv.IdSKU + " ,NumConteo: " + inv.NumConteo + " ,IdUbicacion: " + inv.IdUbicacion + " \n";
                            }
                            catch (Exception e)
                            {
                                FicMensaje += "-ALERTA-> " + e.Message.ToString() + " \n";
                            }
                        }
                    }
                }
                else FicMensaje += "-> SIN DATOS. \n";

                if (FicGetReultREST != null && FicGetReultREST.zt_inventarios_acumulados != null)
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
                                //FicLoBDContext.Update(respuesta);
                                FicMensaje += await FicLoBDContext.SaveChangesAsync() > 0 ? "-UPDATE-> IdInventario: " + inv.IdInventario + " ,IdSKU: " + inv.IdSKU + " \n" : "-NO NECESITO ACTUALIZAR-> IdInventario: " + inv.IdInventario + " ,IdSKU: " + inv.IdSKU + " \n";
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
                                FicMensaje +=  await FicLoBDContext.SaveChangesAsync() > 0 ? "-INSERT-> IdInventario: " + inv.IdInventario + " ,IdSKU: " + inv.IdSKU + " \n" : "-ERROR EN INSERT-> IdInventario: " + inv.IdInventario + " ,IdSKU: " + inv.IdSKU + " \n";
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
            catch (Exception e)
            {
                FicMensaje += "ALERTA: " + e.Message.ToString() + "\n";
            }
            return FicMensaje;
        }//FicGetImportInventarios()

        public async Task<string> FicGetImportCatalogos()
        {
            string FicMensaje="";
            try
            {
                FicMensaje = "IMPORTACION: \n";
                var FicGetReultREST = await FicGetListCatalogosActualiza();

                if (FicGetReultREST != null && FicGetReultREST.zt_cat_productos != null)
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
                                FicMensaje +=  await FicLoBDContext.SaveChangesAsync() > 0 ? "-UPDATE-> IdSKU: " + inv.IdSKU + " \n" : "-NO NECESITO ACTUALIZAR-> IdSKU: " + inv.IdSKU + " \n";
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
                                FicMensaje += await FicLoBDContext.SaveChangesAsync() > 0 ? "-INSERT-> IdSKU: " + inv.IdSKU + " \n" : "-ERROR EN INSERTAR-> IdSKU: " + inv.IdSKU + " \n";
                            }
                            catch (Exception e)
                            {
                                FicMensaje += "-ALERTA-> " + e.Message.ToString() + " \n";
                            }
                        }
                    }
                }
                else FicMensaje += "-> SIN DATOS. \n";

                if (FicGetReultREST != null && FicGetReultREST.zt_cat_unidad_medidas != null)
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
                                FicMensaje += await FicLoBDContext.SaveChangesAsync() > 0 ? "-UPDATE-> IdUnidadMedida: " + inv.IdUnidadMedida + " \n" : "-NO NECESITO ACTUALIZAR-> IdUnidadMedida: " + inv.IdUnidadMedida + " \n";
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
                                FicMensaje += await FicLoBDContext.SaveChangesAsync() > 0 ? "-INSERT-> IdUnidadMedida: " + inv.IdUnidadMedida + " \n" : "-ERROR EN INSERTAR-> IdUnidadMedida: " + inv.IdUnidadMedida + " \n";
                            }
                            catch (Exception e)
                            {
                                FicMensaje += "-ALERTA-> " + e.Message.ToString() + " \n";
                            }
                        }
                    }
                }
                else FicMensaje += "-> SIN DATOS. \n";

                if (FicGetReultREST != null && FicGetReultREST.zt_cat_productos_medidas != null)
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
                                FicMensaje += await FicLoBDContext.SaveChangesAsync()>0 ? "-UPDATE-> IdUnidadMedida: " + inv.IdUnidadMedida + " ,IdSKU: " + inv.IdSKU + " \n" : "-NO NECESITO ACTUALIZAR-> IdUnidadMedida: " + inv.IdUnidadMedida + " ,IdSKU: " + inv.IdSKU + " \n";
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
                                FicMensaje += await FicLoBDContext.SaveChangesAsync() > 0 ? "-INSERT-> IdUnidadMedida: " + inv.IdUnidadMedida + " ,IdSKU: " + inv.IdSKU + " \n" : "-ERROR EN INSERTAR-> IdUnidadMedida: " + inv.IdUnidadMedida + " ,IdSKU: " + inv.IdSKU + " \n";
                            }
                            catch (Exception e)
                            {
                                FicMensaje += "-ALERTA-> " + e.Message.ToString() + " \n";
                            }
                        }
                    }
                }
                else FicMensaje += "-> SIN DATOS. \n";

                if (FicGetReultREST != null && FicGetReultREST.zt_cat_cedis != null)
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
                                
                                FicMensaje += await FicLoBDContext.SaveChangesAsync() >0 ? "-UPDATE-> IdCEDI: " + inv.IdCEDI + " \n": "-NO NECESITO ACTUALIZAR-> IdCEDI: " + inv.IdCEDI + " \n";
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
                                FicMensaje += await FicLoBDContext.SaveChangesAsync() >0? "-INSERT-> IdCEDI: " + inv.IdCEDI + " \n": "-ERROR EN INSERTAR-> IdCEDI: " + inv.IdCEDI + " \n";
                            }
                            catch (Exception e)
                            {
                                FicMensaje += "-ALERTA-> " + e.Message.ToString() + " \n";
                            }
                        }
                    }
                }
                else FicMensaje += "-> SIN DATOS. \n";

                if (FicGetReultREST != null && FicGetReultREST.zt_cat_almacenes != null)
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
                                FicMensaje += await FicLoBDContext.SaveChangesAsync() >0 ?  "-UPDATE-> IdAlmacen: " + inv.IdAlmacen + " \n": "-NO NECESITO ACTUALIZAR-> IdAlmacen: " + inv.IdAlmacen + " \n";
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
                                FicMensaje += await FicLoBDContext.SaveChangesAsync() > 0 ? "-INSERT-> IdAlmacen: " + inv.IdAlmacen + " \n": "-ERROR EN INSERTAR-> IdAlmacen: " + inv.IdAlmacen + " \n";
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
            catch (Exception e)
            {
                FicMensaje += "ALERTA: " + e.Message.ToString() + "\n";
            }
            return FicMensaje;
        }//FicGetImportCatalogos()

    }//CLASS
}//NAMESPACE
