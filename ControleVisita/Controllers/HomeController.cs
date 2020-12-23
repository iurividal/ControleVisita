using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using ControleVisita.Extensions;
using ControleVisita.Models;
using ControleVisita.Models.IdentityExtensions;
using Microsoft.AspNet.Identity;

namespace ControleVisita.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        VisitaViewModel visita = new VisitaViewModel();

        public async Task<ActionResult> Index()
        {
            var user = (ClaimsIdentity)User.Identity;

            var userSession = (LoginModel)Session["USUARIOLOGADO"];

            ViewBag.Usuario = user.Name.Replace('.', ' ');
            await AddNotificacaoNextVisita();     
            return View();
        }

        private async Task AddNotificacaoNextVisita()
        {
            var response = await visita.GetNextVisita(User.Identity.Getcodgrupo(),  User.Identity.GetEmpresa());

            if (response.Any())
            {
                this.AddNotification($"Você tem <a href='{Url.Action("ListaVisita", "Visita")}?clickhome=true'> {response.Count()} visita(s) </a> para realizar amanhã dia {DateTime.Now.Date.AddDays(1).ToString("dd/MM/yyyy")}", NotificationType.INFO);
            }
        }

     

        public ActionResult CldrData()
        {
            return new DevExtreme.AspNet.Mvc.CldrDataScriptBuilder()
                .SetCldrPath("~/Content/cld")
                .SetInitialLocale("pt")
                .UseLocales("de", "es", "pt")
                .Build();
        }

    }
}