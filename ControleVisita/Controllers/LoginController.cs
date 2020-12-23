using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
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
                new Claim(ClaimTypes.UserData,user.Empresa)
            };

            var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

            var ctx = Request.GetOwinContext();
            var auth = ctx.Authentication;

            auth.SignIn(new AuthenticationProperties { IsPersistent = true }, identity);
            auth.SignIn();
        }



        [HttpPost]
        public ActionResult Logar(FormCollection form)
        {

            var usuario = Request.Form["Login"].ToUpper();
            var senha = Request.Form["Password"].ToUpper();

            LoginData l = new LoginData();
            
            var user = LoginData.GetUser();

            //var user = l.Logar(new LoginModel
            //{
            //    Login = usuario,
            //    Password = senha,

            //});

            if (user.CodGrupo == null)
            {

                ModelState.AddModelError("", "Usuario ou senha incorretos");
                return View("Index", new LoginModel());
            }

            Session["USUARIOLOGADO"] = user;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Login),
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Role, ""),
                new Claim(ClaimTypes.GroupSid,user.CodGrupo.ToString()),
                new Claim(ClaimTypes.UserData,user.Empresa)
            };

            var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

            var ctx = Request.GetOwinContext();
            var auth = ctx.Authentication;

            auth.SignIn(new AuthenticationProperties { IsPersistent = false }, identity);

            return RedirectToAction("Index");

        }

        public ActionResult Sair()
        {
            var ctx = Request.GetOwinContext();
            var auth = ctx.Authentication;
            auth.SignOut();
            return RedirectToAction("Index");
        }
    }
}