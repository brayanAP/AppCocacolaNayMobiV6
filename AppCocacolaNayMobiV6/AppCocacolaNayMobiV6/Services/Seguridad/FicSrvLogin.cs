using AppCocacolaNayMobiV6.Data;
using AppCocacolaNayMobiV6.Interfaces.Seguridad;
using AppCocacolaNayMobiV6.Interfaces.SQlite;
using AppCocacolaNayMobiV6.Models.Eva;
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
using System.Security.Cryptography;

namespace AppCocacolaNayMobiV6.Services.Seguridad
{
    public class FicSrvLogin : IFicSrvLogin
    {
        private readonly FicBDContext FicLoBDContext;
        private readonly HttpClient FiClient;

        public FicSrvLogin(){
            FicLoBDContext = new FicBDContext(DependencyService.Get<IFicConfigSQLite>().FicGetDataBasePath());
            FiClient = new HttpClient();
            FiClient.MaxResponseContentBufferSize = 256000;
        }//CONSTRUCTOR

        public string FicMetEncripta(string texto)
        {
            try
            {
                string key = "elchico69eva78jGH5"; //llave para encriptar datos
                byte[] keyArray;
                byte[] Arreglo_a_Cifrar = UTF8Encoding.UTF8.GetBytes(texto);
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = tdes.CreateEncryptor();
                byte[] ArrayResultado = cTransform.TransformFinalBlock(Arreglo_a_Cifrar, 0, Arreglo_a_Cifrar.Length);
                tdes.Clear();
                return  Convert.ToBase64String(ArrayResultado, 0, ArrayResultado.Length);
            }
            catch (Exception)
            {
                return "ERROR";
            }
        }//Encriptar

        public Task<string> FicMetLoginUser(string user, string password)
        {
            throw new NotImplementedException();
        }

        private string FicMetDesencripta(string textoEncriptado)
        {
            try
            {
                string key = "elchico69eva78jGH5"; //llave para desencriptar datos
                byte[] keyArray;
                byte[] Array_a_Descifrar = Convert.FromBase64String(textoEncriptado);
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = tdes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(Array_a_Descifrar, 0, Array_a_Descifrar.Length);
                tdes.Clear();
                return UTF8Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception)
            {
                return "ERROR";
            }
        }//Desencriptar


        private async Task<temp_web_api_login> FicMetGetWebApiLogin(string user, string password,bool tipo)
        {
            string url = "";

            if (tipo) url = "";
            else url = "";

                try
                {
                    var response = await FiClient.GetAsync(url);
                    return response.IsSuccessStatusCode ? JsonConvert.DeserializeObject<temp_web_api_login>(await response.Content.ReadAsStringAsync()) : null;
                }
                catch (Exception e)
                {
                    await new Page().DisplayAlert("ALERTA", e.Message.ToString(), "OK");
                    return null;
                }
        }//ESTE METODO HACE LA PETICION A LA WEB API

        //public async Task<string> FicMetLoginUser(string user, string password)
        //{
        //    try
        //    {
        //        if (CrossConnectivity.Current.IsConnected)
        //        {
        //            var FicSourceWebApiLogin = await FicMetGetWebApiLogin(user, password, true);

        //            if(FicSourceWebApiLogin != null)
        //            {
        //                /*importar a local*/
        //            }
        //        }//ACTUALIZAR EN LOCAL

        //        var FicSourceLocalUser = await (
        //            from u in FicLoBDContext.cat_usuarios
        //            join c in FicLoBDContext.seg_expira_claves on u.IdUsuario equals c.IdUsuario
        //            join e in FicLoBDContext.seg_usuarios_estatus on u.IdUsuario equals e.IdUsuario
        //            where (u.Usuario == user && u.Expira == "N" && u.Activo == "S") &&
        //                  (c.Actual == "S" && ((DateTime.Now.Date >= c.FechaExpiraIni.Value.Date) && (DateTime.Now.Date <= c.FechaExpiraFin.Value.Date))) &&
        //                  (e.IdTipoEstatus == 1 && e.IdEstatus == 1)
        //            select new { u, c , e}).SingleOrDefaultAsync();

        //        if(FicSourceLocalUser == null || (FicSourceLocalUser.u == null || FicSourceLocalUser.c == null || FicSourceLocalUser.e == null))
        //        {
        //            if(FicSourceLocalUser.u == null)
        //            {
        //                var FicBuscaUsuario = await (from u in FicLoBDContext.cat_usuarios where u.Usuario == user select u).SingleOrDefaultAsync();

        //                if (FicBuscaUsuario == null) return "Usuario Invalido.";
        //            }

        //            if (FicSourceLocalUser.c == null)
        //            {
        //                var FicBuscaClave = await (from c in FicLoBDContext.seg_expira_claves where (c.Clave == password && c.Actual == "S") select c).SingleOrDefaultAsync();

        //                if (FicBuscaClave == null) return "Contraseña Invalida";
        //                else if (!((DateTime.Now.Date >= FicBuscaClave.FechaExpiraIni.Value.Date) && (DateTime.Now.Date <= FicBuscaClave.FechaExpiraFin.Value.Date))) return "Clave Expirada.";
        //            }

        //            if(FicSourceLocalUser.e == null)
        //            {
        //                var FicBuscaUsuario = await (from u in FicLoBDContext.cat_usuarios where u.Usuario == user select u).SingleOrDefaultAsync();
        //                var FicBuscaEstatus = await (from e in FicLoBDContext.seg_usuarios_estatus
        //                                         join t in FicLoBDContext.cat_tipos_estatus on e.IdTipoEstatus equals t.IdTipoEstatus
        //                                         join es in FicLoBDContext.cat_estatus on e.IdEstatus equals es.IdEstatus
        //                                         where e.IdUsuario == FicBuscaUsuario.IdUsuario 
        //                                         select es).SingleOrDefaultAsync();

        //                if (FicBuscaEstatus == null) return "Estatus del usuario no encontrado.";
        //                else if (FicBuscaEstatus.IdEstatus != 1 && FicBuscaEstatus.IdTipoEstatus != 1) return FicBuscaEstatus.DesEstatus;
        //            }

        //            return "Ocurrió algo inesperado.";
        //        }//BUSCAR POR QUE EL USUARIO NO FUE ENCONTRADO

        //        return (FicSourceLocalUser.u.Usuario == user && FicMetDesencripta(FicSourceLocalUser.c.Clave) == password) ? "OK" : "Ocurrió algo inesperado.";
        //    }
        //    catch(Exception e)
        //    {
        //        return e.Message.ToString();
        //    }
        //}//INICIAR SESION POR USUER


    }//CLASS
}//NAMESPACE
