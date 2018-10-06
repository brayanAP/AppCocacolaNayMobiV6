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

        private async Task<string> FicMetIdSku(string sku)
        {
            var temp = await (from conteo in FicLoBDContext.zt_cat_productos
                              where conteo.IdSKU == sku
                              select conteo).SingleOrDefaultAsync();
            return temp.IdSKU;
        }

        private async Task<Int16> FicMetIdAlm(string alm)
        {
            var temp = await (from conteo in FicLoBDContext.zt_cat_almacenes
                              where conteo.DesAlmacen == alm
                              select conteo).SingleOrDefaultAsync();
            return temp.IdAlmacen;
        }

        public async Task<string> FicMetIdAlmDes(Int16 id)
        {
            var temp = await (from conteo in FicLoBDContext.zt_cat_almacenes
                              where conteo.IdAlmacen == id
                              select conteo).SingleOrDefaultAsync();
            return temp.DesAlmacen;
        }

        private async Task<Int16> FicMetIdUnm(string unm)
        {
            var temp = await (from conteo in FicLoBDContext.zt_cat_unidad_medidas
                              where conteo.DesUMedida == unm
                              select conteo).SingleOrDefaultAsync();
            return temp.IdUnidadMedida;
        }

        public async Task<string> FicMetIdUnmDes(Int16 id)
        {
            var temp = await (from conteo in FicLoBDContext.zt_cat_unidad_medidas
                              where conteo.IdUnidadMedida == id
                              select conteo).SingleOrDefaultAsync();
            return temp.DesUMedida;
        }

        private async Task<float> FicMetCantPza(string IdSku, Int16 IdUnm, float cf)
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

            if (bus != null)
            {
                if (bus.Count != 0)
                {
                    List<int> temp = new List<int>();
                    foreach (zt_inventarios_conteos a in bus)
                    {
                        temp.Add(a.NumConteo);
                    }

                    return temp.Max() + 1;
                }

            }
            return 1;
        }//BUSCA SI EXISTE UN REGISTRO

        public async Task<string> Insert_zt_inventarios_conteos(zt_inventarios_conteos zt_inventarios_conteos, string alm, string unm, string zku)
        {
            //using (var context = new FicBDContext(DependencyService.Get<IFicConfigSQLiteNETStd>().FicGetDataBasePath()))
            //{
            //    using (IDbContextTransaction transaction = context.Database.BeginTransaction())
            //    {
            try
            {
                zt_inventarios_conteos.IdSKU = await FicMetIdSku(zku);
                zt_inventarios_conteos.IdAlmacen = await FicMetIdAlm(alm);
                zt_inventarios_conteos.IdUnidadMedida = await FicMetIdUnm(unm);
                zt_inventarios_conteos.NumConteo = await FicExitConteo(zt_inventarios_conteos);
                zt_inventarios_conteos.CantidadPZA = await FicMetCantPza(zt_inventarios_conteos.IdSKU, zt_inventarios_conteos.IdUnidadMedida, zt_inventarios_conteos.CantidadFisica);
                FicLoBDContext.Entry(zt_inventarios_conteos).State = EntityState.Detached;

                //if (await FicExitInventariosConteo(zt_inventarios_conteos))
                //{
                //    FicLoBDContext.Update(zt_inventarios_conteos);
                //    await FicLoBDContext.SaveChangesAsync();
                //}
                //else
                //{
                await FicLoBDContext.AddAsync(zt_inventarios_conteos);
                await FicLoBDContext.SaveChangesAsync();
                //  }//BUSCAR SI YA SE INSERTO UN REGISTRO

                //transaction.Commit(); //CONFIRMA/GUARDA
                return "OK";
            }
            catch (Exception ex)
            {
                // transaction.Rollback();
                return ex.Message.ToString();
            }
            //    }//ENTRA EN CONTEXTO DE TRANSACIONES
            //}//ATRAVEZ DEL CONTEXTO DE LA BD
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
