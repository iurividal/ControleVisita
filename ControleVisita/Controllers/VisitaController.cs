using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using ApiConsorcioNet.Extensoes;
using ControleVisita.Models;
using ControleVisita.Models.IdentityExtensions;

namespace ControleVisita.Controllers
{

    [Authorize]
    public class VisitaController : Controller
    {
        VisitaViewModel visita = new VisitaViewModel();
        // GET: Visita
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Cadastrar()
        {
            // ViewBag.Clientes = await Models.ClienteData.Get();
            var visita = new VisitaModel();

            if (Request.QueryString["p"] != null)
                visita = new VisitaViewModel().GetVisita(Convert.ToInt32(Request.QueryString["p"]), User.Identity.GetEmpresa());

            ViewBag.Motivos = ToSelectList(MotivoViewModel.GetAll());
            ViewBag.Estados = CidadeModel.ToSectList(CidadeModel.GetEstado());

            return View(visita);
        }



        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Cadastrar(VisitaModel model)
        {

            model.Cliente = ClienteData.Get(model.IdPessoa).Result;

            if (ModelState.IsValid)
            {



                var user = (LoginModel)Session["USUARIOLOGADO"];



                if (!string.IsNullOrEmpty(model.ValorBemAux))
                {

                    var remove = (model.ValorBemAux.Replace(".", "")).Replace(",", ".");
                    var value = Decimal.Parse(remove, CultureInfo.InvariantCulture);
                    model.ValorBem = value;
                }



                VisitaViewModel.AddOrUpdate(model, user);

                ViewBag.Error = model.Id == 0 ? "0|success" : "Cadastro Atualizado com sucesso|success";

                return View(new VisitaModel());
            }

            ViewBag.Error = "1|error";

            return View(model);
        }

        [NonAction]
        public SelectList ToSelectList(IEnumerable<MotivoModel> dados)
        {
            List<SelectListItem> list = new List<SelectListItem>();


            foreach (var row in dados)
            {
                list.Add(new SelectListItem()
                {
                    Text = row.Motivo,
                    Value = row.IdMotivo.ToString()
                });
            }

            return new SelectList(list, "Value", "Text");
        }

        public async Task<ActionResult> ListaVisita()
        {
            var listaView = new List<VisitaModel>();
            var request = Request.QueryString["clickhome"];

            if (!string.IsNullOrEmpty(request))
            {
                listaView = (List<VisitaModel>)await visita.GetNextVisita(User.Identity.Getcodgrupo(), User.Identity.GetEmpresa());
            }

            return View(listaView.OrderByDescending(a => a.DataVisita).ToList());
        }


        //[HttpPost]
        //public ActionResult ListaVisita(DateTime datainicial, DateTime datafinal, bool isVendaRealizada)
        //{


        //    var response = VisitaViewModel
        //        .Get(User.Identity.Getcodgrupo(), datainicial, datafinal)
        //        .WhereIf(isVendaRealizada, model => model.IsVendaRealizada)
        //        .ToList()
        //        .OrderByDescending(a => a.DataVisita);

        //    ViewBag.TitlePesquisa = $"Período {datainicial.Date:dd/MM/yyyy} até {datafinal:dd/MM/yyyy}";


        //    return View(response);
        //}

        [HttpPost]
        public ActionResult ListaVisita(FiltroModelView filtro)
        {


            var response = VisitaViewModel
                .GetVisitas(User.Identity.Getcodgrupo(), User.Identity.GetEmpresa(), filtro)
                .ToList()
                .OrderByDescending(a => a.DataVisita);


            //  ViewBag.TitlePesquisa = $"Período {datainicial.Date:dd/MM/yyyy} até {datafinal:dd/MM/yyyy}";

            return View(response);
        }

        public ActionResult Agenda()
        {
            return View();
        }


    }
}