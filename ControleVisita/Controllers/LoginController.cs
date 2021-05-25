using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using ControleVisita.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using OracleContext;

namespace ControleVisita.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login

        public ActionResult Index()
        {



#if DEBUG
            LoginData.CreateCookie("VP");
#endif

            var user = LoginData.GetUser();

            if (string.IsNullOrEmpty(user.Login))
                return RedirectToAction("Logar", "Login");

            AutoLogin(user);
            //return RedirectToAction("Index", "Home");

            //if (User.Identity.IsAuthenticated)
            //{
            Session["USERMODEL"] = user;
            return RedirectToAction("Index", "Home");
            //}

            //return View(new LoginModel());
        }


        private void AutoLogin(LoginModel user)
        {

            Session["USUARIOLOGADO"] = user;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Login.Replace("%2E",".")),
                new Claim(ClaimTypes.Name, user.Login.Replace("%2E",".")),
                new Claim(ClaimTypes.Role, ""),
                new Claim(ClaimTypes.GroupSid,user.CodGrupo.ToString()),
                new Claim(ClaimTypes.UserData,user.Empresa),
                new Claim(type: "SubEmpresa", value: user.SubEmpresaPermissao)
            };


            var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

            var ctx = Request.GetOwinContext();
            var auth = ctx.Authentication;

            auth.SignIn(new AuthenticationProperties { IsPersistent = true }, identity);
            auth.SignIn();
        }


        public ActionResult Logar(LoginModel model, string code)
        {
            var host = Request.Url.Host;
            var loginModel = new LoginModel();

            //host = "restrito.cnmf.com.br";

            switch (host)
            {
                case "restrito.cnmf.com.br":
                    loginModel.Empresa = "CNMF";
                    break;
                case "restrito2.consorciomaggi.com.br":
                    loginModel.Empresa = "CMC";
                    break;
                default:
                    loginModel.Empresa = "CMC";
                    break;

            }


            return View(loginModel);

            //return PartialView("_login", model);
        }


        [HttpPost]
        public ActionResult Logar(FormCollection form)
        {

            var usuario = Request.Form["Login"].ToUpper();
            var senha = Request.Form["Password"].ToUpper();
            var empresa = Request.Form["Empresa"].ToUpper();


            LoginData l = new LoginData();


            var user = l.Logar(new LoginModel
            {
                Login = usuario,
                Password = senha,
                Empresa = empresa

            });


            if (user.CodGrupo == null)
            {

                ModelState.AddModelError("", "Usuário ou senha incorretos");
                ViewBag.erro = "Usuário ou senha inválidos";
                return View(new LoginModel { Login = usuario, Password = senha, Empresa = empresa });
            }

            Session["USUARIOLOGADO"] = user;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Login),
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Role, ""),
                new Claim(ClaimTypes.GroupSid,user.CodGrupo.ToString()),
                new Claim(ClaimTypes.UserData,user.Empresa),
                new Claim(type: "SubEmpresa", value: user.SubEmpresaPermissao)
            };

            var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

            var ctx = Request.GetOwinContext();
            var auth = ctx.Authentication;

            auth.SignIn(new AuthenticationProperties { IsPersistent = false }, identity);

            return RedirectToAction("Index", "Home");

        }

        public ActionResult Sair()
        {
            if (Request.Cookies["VP"] != null)
            {
                var c = new HttpCookie("VP");
                c.Expires = DateTime.Now.AddSeconds(1);
                Response.Cookies.Add(c);

                Thread.Sleep(3000);
            }

            var ctx = Request.GetOwinContext();
            var auth = ctx.Authentication;
            auth.SignOut();
            return RedirectToAction("Index");
        }
    }
}