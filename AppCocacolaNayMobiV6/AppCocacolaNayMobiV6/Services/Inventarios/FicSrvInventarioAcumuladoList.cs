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
    public class FicSrvInventarioAcumuladoList : IFicSrvInventarioAcumuladoList
    {
        private readonly FicBDContext FicLoBDContext;

        public FicSrvInventarioAcumuladoList()
        {
            FicLoBDContext = new FicBDContext(DependencyService.Get<IFicConfigSQLite>().FicGetDataBasePath());
        }//CONSTRUCTOR

        public async Task<List<zt_inventarios_acumulados>> FicMetGetAcumuladosList(int _idinventario)
        {
            /*TRAEGO TODOS LOS CONTEOS*/
            var FicSourceConteos = await (from con in FicLoBDContext.zt_inventarios_conteos where con.IdInventario == _idinventario select con).AsNoTracking().ToListAsync();
            /*TRAEGO CADA UNO DE LOS PRODUCTOS, PERO SIN REPETIRCE*/
            var FicSourceProductos = await (from c in FicLoBDContext.zt_inventarios_conteos where c.IdInventario == _idinventario group c by c.IdSKU into c select c.Key).AsNoTracking().ToListAsync();

            if (FicSourceConteos != null)
            {
                if(FicSourceProductos != null)
                {
                    foreach (string c in FicSourceProductos)
                    {
                        var FicSourceSuma = (from t in FicSourceConteos where t.IdSKU == c select t).ToList();

                        if (FicSourceSuma != null)
                        {
                            var FicSuma = FicSourceSuma.GroupBy(x => x.IdSKU).Select(conteo => new
                            {
                                SumaPZA = conteo.Sum(l => l.CantidadPZA)
                            });

                            var FicSourceAcumulados = await (from t in FicLoBDContext.zt_inventarios_acumulados where t.IdSKU == c && t.IdInventario == _idinventario select t).SingleOrDefaultAsync();

                            if (FicSourceAcumulados == null)
                            {
                                await FicLoBDContext.AddAsync(new zt_inventarios_acumulados()
                                {
                                    IdInventario = _idinventario,
                                    IdSKU = c,
                                    CantidadTeorica = FicSuma.First().SumaPZA,
                                    CantidadFisica = FicSuma.First().SumaPZA,
                                    Diferencia = 0,
                                    IdUnidadMedida = "PZA",
                                    FechaReg = DateTime.Now.Date,
                                    UsuarioReg = "BUAP",
                                    Activo = "S",
                                    Borrado = "N"
                                });
                                await FicLoBDContext.SaveChangesAsync();
                            }
                            else
                            {
                                FicSourceAcumulados.CantidadFisica = FicSuma.First().SumaPZA;
                                FicSourceAcumulados.Diferencia = FicSourceAcumulados.CantidadTeorica - FicSourceAcumulados.CantidadFisica;
                                FicSourceAcumulados.FechaUltMod = DateTime.Now;
                                FicSourceAcumulados.UsuarioMod = "BUAP";
                               // FicLoBDContext.Update(FicSourceAcumulados);
                                await FicLoBDContext.SaveChangesAsync();
                            }//INSERT o UPDATE DEL ACUMULADO
                        }//TRAER LA SUMA DE PIEZAS DEL CONTEO DE ESE PRODUCTO

                    }//RECORRERER LA LISTA DE PRODUCTOS PARA OBTENER EL ENCABEZADO
                }//LISTA DE PRODUCTOS
            }//SI EXISTEN CONTEOS

            return await (from acu in FicLoBDContext.zt_inventarios_acumulados where acu.IdInventario == _idinventario select acu).AsNoTracking().ToListAsync();
        }
    }//CLASS
}//NAMESPACE
