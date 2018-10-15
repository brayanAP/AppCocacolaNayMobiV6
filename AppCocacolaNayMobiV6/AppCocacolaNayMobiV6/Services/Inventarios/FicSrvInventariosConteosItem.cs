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
using Microsoft.EntityFrameworkCore.Storage;

namespace AppCocacolaNayMobiV6.Services.Inventarios
{
    public class FicSrvInventariosConteosItem : IFicSrvInventariosConteosItem
    {
        private readonly FicBDContext FicLoBDContext;

        public FicSrvInventariosConteosItem()
        {
            FicLoBDContext = new FicBDContext(DependencyService.Get<IFicConfigSQLite>().FicGetDataBasePath());
        }//CONSTRUCTOR

        public async Task<IList<zt_inventarios_conteos>> FicMetGetListInventariosConteos()
        {
            return await (from conteo in FicLoBDContext.zt_inventarios_conteos
                          select conteo).ToListAsync();
        }//LIST ALL

        private async Task<bool> FicExitInventariosConteo(zt_inventarios_conteos zt_inventarios_conteos)
        {
            return await (from conteo in FicLoBDContext.zt_inventarios_conteos
                          where conteo.IdInventario == zt_inventarios_conteos.IdInventario && conteo.IdSKU == zt_inventarios_conteos.IdSKU && conteo.IdAlmacen == zt_inventarios_conteos.IdAlmacen && conteo.IdUnidadMedida == zt_inventarios_conteos.IdUnidadMedida && conteo.IdUbicacion == zt_inventarios_conteos.IdUbicacion
                          select conteo).SingleOrDefaultAsync() == null ? true : false;
        }//BUSCA SI EXISTE UN REGISTRO

        private async Task<float> FicMetCantPza(string IdSku, string IdUnm, float cf)
        {
            var temp = await (from prome in FicLoBDContext.zt_cat_productos_medidas
                              join pro in FicLoBDContext.zt_cat_productos on prome.IdSKU equals pro.IdSKU
                              join unm in FicLoBDContext.zt_cat_unidad_medidas on prome.IdUnidadMedida equals unm.IdUnidadMedida
                              where pro.IdSKU == IdSku && unm.IdUnidadMedida == IdUnm
                              select prome).SingleOrDefaultAsync();
            return temp == null ? 0 : temp.CantidadPZA * cf;
        }

        private async Task<int> FicExitConteo(zt_inventarios_conteos zt_inventarios_conteos)
        {
            var bus = await (from conteo in FicLoBDContext.zt_inventarios_conteos
                             join inv in FicLoBDContext.zt_inventarios on conteo.IdInventario equals inv.IdInventario
                             join pro in FicLoBDContext.zt_cat_productos on conteo.IdSKU equals pro.IdSKU
                             join alm in FicLoBDContext.zt_cat_almacenes on conteo.IdAlmacen equals alm.IdAlmacen
                             join unm in FicLoBDContext.zt_cat_unidad_medidas on conteo.IdUnidadMedida equals unm.IdUnidadMedida
                             where pro.IdSKU == zt_inventarios_conteos.IdSKU && alm.IdAlmacen == zt_inventarios_conteos.IdAlmacen && unm.IdUnidadMedida == zt_inventarios_conteos.IdUnidadMedida && conteo.IdUbicacion == zt_inventarios_conteos.IdUbicacion && inv.IdInventario == zt_inventarios_conteos.IdInventario //&& conteo.NumConteo == zt_inventarios_conteos.NumConteo
                             select conteo).ToListAsync();

            if (bus != null && bus.Count != 0)
            {
                List<int> temp = new List<int>();
                foreach (zt_inventarios_conteos a in bus) temp.Add(a.NumConteo);
                return temp.Max() + 1;
            }

            return 1;
        }//BUSCA SI EXISTE UN REGISTRO

        public async Task<body_edit_conteo_item> FicExitBodyEdit(zt_inventarios_conteos zt_inventarios_conteos)
        {

            return new body_edit_conteo_item()
            {
                zt_cat_almacenes = await (from con in FicLoBDContext.zt_cat_almacenes where con.IdAlmacen == zt_inventarios_conteos.IdAlmacen select con).SingleOrDefaultAsync() as zt_cat_almacenes,
                zt_cat_productos = await (from con in FicLoBDContext.zt_cat_productos where con.IdSKU == zt_inventarios_conteos.IdSKU select con).SingleOrDefaultAsync() as zt_cat_productos,
                zt_cat_unidad_medidas = await (from con in FicLoBDContext.zt_cat_unidad_medidas where con.IdUnidadMedida == zt_inventarios_conteos.IdUnidadMedida select con).SingleOrDefaultAsync() as zt_cat_unidad_medidas
            };
        }

        public async Task<string> Insert_zt_inventarios_conteos(zt_inventarios_conteos zt_inventarios_conteos,bool modo)
        {
            try
            {
                zt_inventarios_conteos.CantidadPZA = await FicMetCantPza(zt_inventarios_conteos.IdSKU, zt_inventarios_conteos.IdUnidadMedida, zt_inventarios_conteos.CantidadFisica);

                if (modo)
                {
                    zt_inventarios_conteos.NumConteo = await FicExitConteo(zt_inventarios_conteos);
                    await FicLoBDContext.AddAsync(zt_inventarios_conteos);
                    return await FicLoBDContext.SaveChangesAsync() > 0 ? "OK" : "ERROR AL REGISTRAR";
                }
                else
                {
                    var FicSourceInvExits = await (
                       from con in FicLoBDContext.zt_inventarios_conteos
                       join inv in FicLoBDContext.zt_inventarios on con.IdInventario equals inv.IdInventario
                       where con.IdSKU == zt_inventarios_conteos.IdSKU && con.IdAlmacen == zt_inventarios_conteos.IdAlmacen && con.IdUnidadMedida == zt_inventarios_conteos.IdUnidadMedida && con.NumConteo == zt_inventarios_conteos.NumConteo && con.IdUbicacion == zt_inventarios_conteos.IdUbicacion
                       select con
                        ).SingleOrDefaultAsync();

                    FicSourceInvExits.Lote = zt_inventarios_conteos.Lote;
                    FicSourceInvExits.CantidadFisica = zt_inventarios_conteos.CantidadFisica;
                    FicSourceInvExits.CantidadPZA = zt_inventarios_conteos.CantidadPZA;

                    FicLoBDContext.Update(FicSourceInvExits);
                    return await FicLoBDContext.SaveChangesAsync() > 0 ? "OK" : "ERROR AL ACTUALIZAR";
                }
                
              
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }//INSERTAR NUEVO CONTEO

        public async Task<string> Remove_zt_inventarios_conteos(zt_inventarios_conteos zt_inventarios_conteos)
        {
            using (var context = new FicBDContext(DependencyService.Get<IFicConfigSQLite>().FicGetDataBasePath()))
            {
                using (IDbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (await FicExitInventariosConteo(zt_inventarios_conteos))
                        {
                            return "Registro no encontrado.";
                        }//BUSCAR SI YA SE INSERTO UN REGISTRO

                        FicLoBDContext.Remove(zt_inventarios_conteos);
                        await FicLoBDContext.SaveChangesAsync();

                        transaction.Commit(); //CONFIRMA/GUARDA
                        return "OK";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return ex.Message.ToString();
                    }
                }//ENTRA EN CONTEXTO DE TRANSACIONES
            }//ATRAVEZ DEL CONTEXTO DE LA BD
        }//ELIMINAR UN  CONTEO

        public async Task<IList<zt_cat_almacenes>> FicMetGetListAlmacenes()
        {
            return await (from conteo in FicLoBDContext.zt_cat_almacenes
                          select conteo).ToListAsync();
        }

        public async Task<IList<zt_cat_unidad_medidas>> FicMetGetListCatUnidadMedida()
        {
            return await (from conteo in FicLoBDContext.zt_cat_unidad_medidas
                          select conteo).ToListAsync();
        }

        public async Task<IList<zt_cat_productos>> FicMetGetListCatProductos()
        {
            return await (from conteo in FicLoBDContext.zt_cat_productos
                          select conteo).ToListAsync();
        }

        public async Task<IList<zt_cat_cedis>> FicMetGetListCedis()
        {
            return await (from conteo in FicLoBDContext.zt_cat_cedis
                          select conteo).ToListAsync();
        }

    }//CLASS
}//NAMESPACE
