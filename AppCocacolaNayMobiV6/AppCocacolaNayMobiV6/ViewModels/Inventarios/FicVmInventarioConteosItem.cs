using AppCocacolaNayMobiV6.Interfaces.Inventarios;
using AppCocacolaNayMobiV6.Interfaces.Navegacion;
using AppCocacolaNayMobiV6.Models;
using AppCocacolaNayMobiV6.Services.Inventarios;
using AppCocacolaNayMobiV6.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;

namespace AppCocacolaNayMobiV6.ViewModels.Inventarios
{
    public class FicVmInventarioConteosItem : INotifyPropertyChanged
    {
        #region VARIABLES LOCALES
        /*ESTAS VARIABLES NOS AYUDAN A COMUNICARNOS CON LOS SERVICIOS, DE FICHAS INTERFACES*/
        private IFicSrvNavigationInventario IFicSrvNavigationInventario;
        private IFicSrvInventariosConteosItem IFicSrvInventariosConteosItem;

        /*ESTAS VARIABLES SON PARA MANEJO LOGICO DE LOS BIDING DE LA VIEW*/
        private string _FicLabelInventario, _FicLabelCEDI, _FicLabelFecha, _IdUbicacion, _CodigoBarra, _IdSKU, _IdUnidadMedida, _IdAlmacen, _Lote, _DesSKU;
        private int _CantidadFisica;

        /*ESTAS VARIABLES SON PARA MANEJO LOGICO DE LOS COMANDOS DE LA VIEW*/
        private ICommand _FicMetRegesarConteoListICommand;
        private ICommand _FicMetAddConteoItemIcommand;

        /*ESTAS VARIABLES SON PARA EL LLEANADO DE LOS AUTOCOMPLETABLES DE LA VIEW*/
        private ObservableCollection<string> _FicSourceStringIdAlmacen;
        private ObservableCollection<string> _FicSourceIdUnidadMedida;
        private ObservableCollection<string> _FicSourceStringIdSku;
        private ObservableCollection<string> _FicSourceStringCodigoBarra;

        /*ESTA VARIABLE NOS AYUDARA LA LISTA DE PRODUCTOS QUE NOS TRAJO EL METODO QUE USAMOS PARA LLENAR EL AUTOCOMPLETABLE*/
        private List<zt_cat_productos> FicSourceZt_cat_productos;

        /*ESTA VARIABLE SIRVE PARA TOMAR EL VALOR QUE PASAMOS <DE LA VIEW PADRE A LA VIEW HIJA>*/
        public object[] FicNavigationContextC { get; set; }

        /*ESTA VARIABLE NOS AYUDA A SELECCIONAR EL MODO QUE TENDRA LA VIEW -> INSERT/UPDATE*/
        public bool FicModo { get; set; }
        #endregion

        public FicVmInventarioConteosItem(IFicSrvNavigationInventario IFicSrvNavigationInventario, IFicSrvInventariosConteosItem IFicSrvInventariosConteosItem)
        {
            this.IFicSrvNavigationInventario = IFicSrvNavigationInventario;
            this.IFicSrvInventariosConteosItem = IFicSrvInventariosConteosItem;
        }//CONSTRUCTOR

        #region VARIABLES DE CONEXION A LA VIEW
        /*HACEN REFERENCIA A LOS BIDING'S DE LA VIEW -> DEL ENCABEZADO*/
        public string FicLabelInventario { get { return _FicLabelInventario; } }

        public string FicLabelCEDI { get { return _FicLabelCEDI; } }

        public string FicLabelFecha { get { return _FicLabelFecha; } }

        /*HACEN REFERENCIA A LOS BIDING'S DE LA VIEW -> DEL CUERPO*/
        public string IdUbicacion
        {
            get { return _IdUbicacion; }
            set
            {
                if (value != null)
                {
                    _IdUbicacion = value;
                    RaisePropertyChanged("IdUbicacion");
                }
            }
        }

        public string CodigoBarra
        {
            get { return _CodigoBarra; }
            set
            {
                if (value != null)
                {
                    _CodigoBarra = value;
                    RaisePropertyChanged("CodigoBarra");
                }
            }
        }

        public string IdSKU
        {
            get { return _IdSKU; }
            set
            {
                if (value != null)
                {
                    _IdSKU = value;
                    RaisePropertyChanged("IdSKU");
                }
            }
        }

        public int CantidadFisica
        {
            get { return _CantidadFisica; }
            set
            {
                if (value != 0)
                {
                    _CantidadFisica = value;
                    RaisePropertyChanged("CantidadFisica");
                }
            }
        }

        public string IdUnidadMedida
        {
            get { return _IdUnidadMedida; }
            set
            {
                if (value != null)
                {
                    _IdUnidadMedida = value;
                    RaisePropertyChanged("IdUnidadMedida");
                }
            }
        }

        public string IdAlmacen
        {
            get { return _IdAlmacen; }
            set
            {
                if (value != null)
                {
                    _IdAlmacen = value;
                    RaisePropertyChanged("IdAlmacen");
                }
            }
        }

        public string Lote
        {
            get { return _Lote; }
            set
            {
                if (value != null)
                {
                    _Lote = value;
                    RaisePropertyChanged("Lote");
                }
            }
        }

        public string DesSKU
        {
            get { return _DesSKU; }
            set
            {
                if (value != null)
                {
                    _DesSKU = value;
                    RaisePropertyChanged("DesSKU");
                }
            }
        }

        public ObservableCollection<string> FicSourceAutoCompleteIdAlmacen { get { return _FicSourceStringIdAlmacen; } }

        public ObservableCollection<string> FicSourceAutoCompleteIdSKU { get { return _FicSourceStringIdSku; } }

        public ObservableCollection<string> FicSourceAutoCompleteIdUnidadMedida { get { return _FicSourceIdUnidadMedida; } }

        public ObservableCollection<string> FicSourceAutoCompleteCodigoBarras { get { return _FicSourceStringCodigoBarra; } }
        #endregion

        /*ESTE METODO ES EL QUE SE LLAMA EN LA VIEW EN EL METODO CON EL MISMO NOMBRE*/
        public async void OnAppearing()
        {
            try
            {
                #region LLENADO DEL ENCABEZADO
                /*ESTA VARIABLE TRAE EL ZT_INVENTARIOS, QUE TOMAMOS DE LA VIEW LIST*/
                var FicSource_zt_inventarios = FicNavigationContextC[0] as zt_inventarios;

                /*AQUI ASIGANMOS VALOR A NUESTRAS VARIABLES DE MANEJO LOCALES*/
                _FicLabelInventario = FicSource_zt_inventarios.IdInventario + "";
                _FicLabelCEDI = FicSource_zt_inventarios.IdCEDI + "";
                _FicLabelFecha = FicSource_zt_inventarios.FechaReg.Value.ToShortDateString();

                /*ACTUALIZAMOS EL ENCABEZADO*/
                RaisePropertyChanged("FicLabelInventario");
                RaisePropertyChanged("FicLabelCEDI");
                RaisePropertyChanged("FicLabelFecha");
                #endregion

                #region LLENADO DE LOS AUTOCOMPLETABLES
                /*LLENADO DE LOS SOURCES STRING PARA LOS AUTOCOMPLETABLES*/
                _FicSourceStringIdAlmacen = new ObservableCollection<string>();
                foreach (zt_cat_almacenes alm in await IFicSrvInventariosConteosItem.FicMetGetListAlmacenes())
                {
                    _FicSourceStringIdAlmacen.Add(alm.DesAlmacen);
                }

                _FicSourceIdUnidadMedida = new ObservableCollection<string>();
                foreach (zt_cat_unidad_medidas unm in await IFicSrvInventariosConteosItem.FicMetGetListCatUnidadMedida())
                {
                    _FicSourceIdUnidadMedida.Add(unm.DesUMedida);
                }

                FicSourceZt_cat_productos = new List<zt_cat_productos>();
                _FicSourceStringIdSku = new ObservableCollection<string>();
                foreach (zt_cat_productos unm in await IFicSrvInventariosConteosItem.FicMetGetListCatProductos())
                {
                    _FicSourceStringIdSku.Add(unm.IdSKU);
                    FicSourceZt_cat_productos.Add(unm);
                }

                _FicSourceStringCodigoBarra = new ObservableCollection<string>();
                foreach (zt_cat_productos unm in await IFicSrvInventariosConteosItem.FicMetGetListCatProductos())
                {
                    _FicSourceStringCodigoBarra.Add(unm.CodigoBarras);
                }

                /*ACTUALIZACION DE LOS AUTOCOMPLETABLES, PARA QUE SE REFLEJE EL LLENADO EN LA VIEW*/
                RaisePropertyChanged("FicSourceAutoCompleteIdAlmacen");
                RaisePropertyChanged("FicSourceAutoCompleteIdSKU");
                RaisePropertyChanged("FicSourceAutoCompleteIdSKU");
                RaisePropertyChanged("FicSourceAutoCompleteIdUnidadMedida");
                RaisePropertyChanged("FicSourceAutoCompleteCodigoBarras");
                #endregion

                #region LLENADO DEL CUERPO EN CASO QUE SEA MODO UPDATE
                /*EN ESTA VARIABLE TOMAMOS EL VALOR ZT_INVENTARIOS_CONTEOS QUE TOMAMOS DE LA VIEW LIST*/
                if (FicModo)
                {
                    var FicSourceZt_inventarios_conteo = FicNavigationContextC[1] as zt_inventarios_conteos;
                    if (FicSourceZt_inventarios_conteo != null)
                    {
                        /*AQUI LLENAMOS LAS VARIABLES LOCALES, PARA QUE SE VEAN LOS VALORES ATRAVEZ DEL BIDING*/
                        _CantidadFisica = (int)FicSourceZt_inventarios_conteo.CantidadFisica;
                        _CodigoBarra = FicSourceZt_inventarios_conteo.CodigoBarras;
                        _IdAlmacen = await IFicSrvInventariosConteosItem.FicMetIdAlmDes(FicSourceZt_inventarios_conteo.IdAlmacen);
                        _IdSKU = FicSourceZt_inventarios_conteo.IdSKU;
                        _IdUbicacion = FicSourceZt_inventarios_conteo.IdUbicacion;
                        _IdUnidadMedida = await IFicSrvInventariosConteosItem.FicMetIdUnmDes(FicSourceZt_inventarios_conteo.IdUnidadMedida);
                        _Lote = FicSourceZt_inventarios_conteo.Lote;

                        /*ACTUALIZACION DEL CUERPO EN LA VIEW*/
                        RaisePropertyChanged("CodigoBarra");
                        RaisePropertyChanged("IdAlmacen");
                        RaisePropertyChanged("IdSKU");
                        RaisePropertyChanged("CodigoBarra");
                        RaisePropertyChanged("IdUbicacion");
                        RaisePropertyChanged("IdUnidadMedida");
                        RaisePropertyChanged("CantidadFisica");
                        RaisePropertyChanged("Lote");
                    }
                }//ESTA EN MODO UPDATE?

                #endregion
            }
            catch (Exception e)
            {
                await new Page().DisplayAlert("ALERTA", e.Message.ToString(), "OK");
            }//USADO PARA VALIDAR ERRORES GLOBALES QUE OCURRAN EN EL PROCESO
        }//OnAppearing()

        /*ESTE METO HACE BUSQUEDA DE UN PRODUCTO QUE COINCIDA CON EL CODIGO DE BARRAS ENVIADO*/
        private zt_cat_productos FicExitsCodigoBarra(string codigo)
        {
            if(FicSourceZt_cat_productos != null)
            {
                return (from pr in FicSourceZt_cat_productos where pr.CodigoBarras == codigo select pr).SingleOrDefault();
            }

            return null;
        }//FicExitsCodigoBarra

        /*ESTE METODO SE MANDA A LLAMAR EN LA VIEW Y SIRVE PARA CAMBAIR VALORES A SELECCIONAR UN CODIGO DE BARRAS*/
        public void FicMetLoadInfoTomaCodigoBarra()
        {
            try
            {
                var temp = FicExitsCodigoBarra(_CodigoBarra);
                if(temp != null)
                {
                    _DesSKU = temp.DesSKU;
                    RaisePropertyChanged("DesSKU");
                    _IdSKU = temp.IdSKU;
                    RaisePropertyChanged("IdSKU");
                }
                else
                {
                    _DesSKU = "NO ENCONTRADO.";
                    RaisePropertyChanged("DesSKU");
                    _IdSKU = "";
                    RaisePropertyChanged("IdSKU");
                }
            }
            catch(Exception e)
            {
                _DesSKU = "NO ENCONTRADO.";
                RaisePropertyChanged("DesSKU");
                _IdSKU = "";
                RaisePropertyChanged("IdSKU");
            }
        }//FicMetLoadInfoTomaCodigoBarra()

        #region MANEJO DE COMANDOS
        public ICommand FicMetRegesarConteoListICommand
        {
            get
            {
                return _FicMetRegesarConteoListICommand = _FicMetRegesarConteoListICommand ??
                      new FicVmDelegateCommand(FicMetRegresaInventarioConteo);
            }
        }//ESTE VENTO AGREGA EL COMANDO AL BOTON EN LA VIEW

        private async void FicMetRegresaInventarioConteo()
        {
            try
            {
                IFicSrvNavigationInventario.FicMetNavigateTo<FicVmInventarioConteoList>(FicNavigationContextC[0]);
            }
            catch (Exception e)
            {
                await new Page().DisplayAlert("ALERTA", e.Message.ToString(), "OK");
            }
        }

        public ICommand SaveCommand
        {
            get { return _FicMetAddConteoItemIcommand = _FicMetAddConteoItemIcommand ?? new FicVmDelegateCommand(SaveCommandExecute); }
        }

        public async void SaveCommandExecute()
        {
            var t = FicNavigationContextC[0] as zt_inventarios;
            try
            {
                if (FicModo)
                {
                    /*ACTUALIZA CONTEO*/
                }
                else
                {
                    /*NUEVO CONTEO*/
                    var FicRespuestaInsert = await IFicSrvInventariosConteosItem.Insert_zt_inventarios_conteos(new zt_inventarios_conteos()
                    {
                        IdInventario = t.IdInventario,
                        IdAlmacen = 0,
                        NumConteo = 0,
                        IdSKU = _IdSKU,
                        CodigoBarras = _CodigoBarra,
                        IdUbicacion = _IdUbicacion,
                        CantidadFisica = _CantidadFisica,
                        IdUnidadMedida = 0,
                        CantidadPZA = 0,
                        Lote = _Lote,
                        FechaReg = DateTime.Now,
                        UsuarioReg = "BUAP",
                        Activo = "S",
                        Borrado = "N"
                    }, _IdAlmacen, _IdUnidadMedida, _IdSKU);

                    if (FicRespuestaInsert == "OK")
                    {
                        await new Page().DisplayAlert("ADD", "¡GUARDADO CON EXITO!", "OK");
                        IFicSrvNavigationInventario.FicMetNavigateTo<FicVmInventarioConteosItem>(FicNavigationContextC);
                    }
                    else
                    {
                        await new Page().DisplayAlert("ADD", FicRespuestaInsert.ToString(), "OK");
                    }//SE INSERTO EL CONTEO?
                }//VIENE EN MODO UPDATE?
            }
            catch (Exception e)
            {
                await new Page().DisplayAlert("ALERTA", e.Message.ToString(), "OK");
            }//MANEJO GLOBAL DE ERRORES
        }//SaveCommandExecute
        #endregion

        #region  INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }//CLASS
}//NAMESPACE
