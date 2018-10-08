using AppCocacolaNayMobiV6.Interfaces.Inventarios;
using AppCocacolaNayMobiV6.Interfaces.Navegacion;
using AppCocacolaNayMobiV6.Services.Inventarios;
using AppCocacolaNayMobiV6.Services.Navegacion;
using AppCocacolaNayMobiV6.ViewModels.Inventarios;
using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppCocacolaNayMobiV6.ViewModels.Base
{
    public class FicViewModelLocator
    {
        private static IContainer FicIContainer;

        public FicViewModelLocator()
        {
            //FIC: ContainerBuilder es una clase de la libreria de Autofac para poder ejecutar la interfaz en las diferentes plataformas 
            var FicContainerBuilder = new ContainerBuilder();

            //-------------------------------- VIEW MODELS ------------------------------------------------------
            //FIC: se procede a registrar las ViewModels para que se puedan mandar llamar en cualquier plataforma
            //---------------------------------------------------------------------------------------------------
            FicContainerBuilder.RegisterType<FicVmInventariosList>();
            FicContainerBuilder.RegisterType<FicVmInventarioConteoList>();
            FicContainerBuilder.RegisterType<FicVmInventarioConteosItem>();
            FicContainerBuilder.RegisterType<FicVmInventarioAcumuladoList>();
            FicContainerBuilder.RegisterType<FicVmImportarWebApi>();
            //------------------------- INTERFACE SERVICES OF THE VIEW MODELS -----------------------------------
            //FIC: se procede a registrar la interface con la que se comunican las ViewModels con los Servicios 
            //para poder ejecutar las tareas (metodos o funciones, etc) del servicio en cuestion.
            //---------------------------------------------------------------------------------------------------
            FicContainerBuilder.RegisterType<FicSrvNavigationInventario>().As<IFicSrvNavigationInventario>().SingleInstance();
            FicContainerBuilder.RegisterType<FicSrvInventariosList>().As<IFicSrvInventariosList>().SingleInstance();
            FicContainerBuilder.RegisterType<FicSrvInventariosConteosItem>().As<IFicSrvInventariosConteosItem>().SingleInstance();
            FicContainerBuilder.RegisterType<FicSrvInventariosConteoList>().As<IFicSrvInventariosConteoList>().SingleInstance();
            FicContainerBuilder.RegisterType<FicSrvInventarioAcumuladoList>().As<IFicSrvInventarioAcumuladoList>().SingleInstance();
            FicContainerBuilder.RegisterType<FicSrvImportarWebApi>().As<IFicSrvImportarWebApi>().SingleInstance();

            //FIC: se asigna o se libera el contenedor
            //-------------------------------------------
            if (FicIContainer != null)
            {
                FicIContainer.Dispose();
            }

            FicIContainer = FicContainerBuilder.Build();
        }//CONSTRUCTOR

        //-------------------- CONTROL DE INVENTARIOS ------------------------
        //FIC: se manda llamar desde el backend de la View de List
        public FicVmInventariosList FicVmInventariosList
        {
            get { return FicIContainer.Resolve<FicVmInventariosList>(); }
        }

        public FicVmInventarioConteoList FicVmInventarioConteoList
        {
            get { return FicIContainer.Resolve<FicVmInventarioConteoList>(); }
        }
        
        public FicVmInventarioConteosItem FicVmInventarioConteosItem
        {
            get { return FicIContainer.Resolve<FicVmInventarioConteosItem>(); }
        }
        
        public FicVmInventarioAcumuladoList FicVmInventarioAcumuladoList
        {
            get { return FicIContainer.Resolve<FicVmInventarioAcumuladoList>(); }
        }

        public FicVmImportarWebApi FicVmImportarWebApi
        {
            get { return FicIContainer.Resolve<FicVmImportarWebApi>(); }
        }

    }//CLASS
}//NAMESPACE
