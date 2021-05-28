using System;
using System.Linq;
using System.Security.Claims;
using System.Web;
using Microsoft.AspNet.Identity;
using MoreLinq.Extensions;
using OracleContext;

namespace ControleVisita.Models
{
    public class LoginData
    {

        public LoginModel Logar(LoginModel login)
        {
            using (var db = new OracleDataContext(ApiConsorcioNet.Conexao.ConnectionStrings.Acesso(login.Empresa)))
            {
                var response = db.WEBLOGINs
    .FirstOrDefault(a => a.LOGIN == login.Login && a.SENHA == login.Password);


                if (response != null)
                {

                    var grupo = db.CNGRUPOs.First(c => c.CODGRUPO == response.CODGRUPO);

                    var cookie = new HttpCookie("VP")
                    {
                        // Value = "CMC+9+LEANDRO.AMARAL+11",
                        Value = $"{login.Empresa}+{response.CODLOGIN}+{response.LOGIN}+{grupo.TIPOGRUPO}",
                        Expires = DateTime.Now.AddMinutes(20),
                        HttpOnly = true
                    };

                    HttpContext.Current.Response.AppendCookie(cookie);

                }

                return GetUser();
            }
        }

        public static LoginModel GetUser()
        {

            var response = getPropriedadeCookie("VP");


            return response;

        }

        public static void CreateCookie(string nomeCookie)
        {
            var cookie = HttpContext.Current.Request.Cookies[nomeCookie] ?? new HttpCookie(nomeCookie)
            {

                //Value = "CMC+6578+RUBEN.REIS+25",
                //Value = "CMC+9+LEANDRO.AMARAL+11",
                // Value = "CNMF+27403+FELIPE.LOPES+1",
                // Value = "CNMF+27514+JOAO.CUSTODIO+4",
                // Value = "CMC+6516+SILVANA.SILVA+6",
               // Value = "CMC+6596+JEAN.PARAGUAI+25",
                //Value = "CMC+6579+LEONARDO.GIMENEZ+26",
                Value = "CMC+6509+EDSON.RODRIGUES+26",
                // Value = "CMC+6466+FABYANE.MUNIZ+25",
                // Value = "CMC+6573+JOACAZ.MARTINS+25",

                Expires = DateTime.Now.AddMinutes(20),
                HttpOnly = true
            };

            HttpContext.Current.Response.AppendCookie(cookie);

        }

        private static LoginModel getPropriedadeCookie(string nomeCookie)
        {

            var cookie = OpenCookie(nomeCookie);

            if (cookie == null)
                return new LoginModel();


            var keys = cookie.Value.Split('+');

            string[] valores = cookie.Value.ToString().Split('+');

            LoginModel model = new LoginModel
            {
                Empresa = valores[0],
                CodLogin = valores[1],
                Login = valores[2],
                TipoGrupo = valores[3]
            };

            using (var db = new OracleDataContext(ApiConsorcioNet.Conexao.ConnectionStrings.Acesso(model.Empresa)))
            {
                model.CodGrupo = db.WEBLOGINs.First(a => a.CODLOGIN == Convert.ToDecimal(model.CodLogin)).CODGRUPO.ToString();

                model.SubEmpresaPermissao = string.Join(";", (from d in db.CNGRUPOSGEPESSOAs
                                                              where d.CODGRUPO == Convert.ToDecimal(model.CodGrupo)
                                                              && d.SITGRUPO == 1
                                                              select new
                                                              {
                                                                  d.SEQREVENDA

                                                              }).ToList()
                                             .Select(a => new
                                             {
                                                 MARCA = db.FBUSCAMARCACOD(model.Empresa, a.SEQREVENDA)

                                             }).GroupBy(a => a.MARCA).Select(a => a.Key));
            }

            return model;




        }

        private void RemoverCookie(string nomeCookie)
        {

            HttpContext.Current.Response.Cookies[nomeCookie].Expires = DateTime.Now.AddMinutes(-20);
        }

        private static HttpCookie OpenCookie(string nomeCookie)
        {
            try
            {
                return HttpContext.Current.Request.Cookies[nomeCookie];
            }
            catch
            {
                return null;
            }

        }


    }
}