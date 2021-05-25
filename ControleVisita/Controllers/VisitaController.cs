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

            ViewData["HISTORICOVISITA"] = new List<VisitaHistoricoModel>();

            if (Request.QueryString["p"] != null)
            {
                int codvisita = Convert.ToInt32(Request.QueryString["p"]);
                var v = new VisitaViewModel().GetVisita(codvisita, User.Identity.GetEmpresa());

                visita.IdPessoa = v.IdPessoa;
                visita.BemViewModel = v.BemViewModel;
                visita.ValorBem = v.ValorBem;
                visita.ValorBemAux = v.ValorBemAux;
                visita.MotivoVisita = v.MotivoVisita;


                ViewData["HISTORICOVISITA"] = new VisitaViewModel().GetHistoricoVisita(codvisita, visita.IdPessoa, User.Identity.GetEmpresa());

                ViewData["DADOS_CLIENTE"] = ClienteData.Get(v.IdPessoa, User.Identity.GetEmpresa()).Result;

            }


            ViewBag.Motivos = ToSelectList(MotivoViewModel.GetAll());
            ViewBag.Estados = CidadeModel.ToSectList(CidadeModel.GetEstado());



            return View(visita);
        }



        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Cadastrar(VisitaModel model)
        {

          

            if (model.IdPessoa == 0)
            {
                ModelState.AddModelError(Guid.NewGuid().ToString(), "Selecione um cliente");
                return View(model);
            }

            if (model.Venda == "Não Realizada" && string.IsNullOrEmpty(model.MotivoNaoVenda))
            {
                ModelState.AddModelError(Guid.NewGuid().ToString(), "Selecione um motivo da não venda");
                return View(model);
            }


            model.Cliente = ClienteData.Get(model.IdPessoa, User.Identity.GetEmpresa()).Result;

            if (ModelState.IsValid)
            {
                var user = (LoginModel)Session["USUARIOLOGADO"];


                model.ValorBem = CorrigiValorMoeda(model.ValorBemAux);
                model.ValorVenda = CorrigiValorMoeda(model.ValorVendaStr);


                VisitaViewModel.AddOrUpdate(model, user);

                ViewBag.Error = model.Id == 0 ? "0|success" : "Cadastro Atualizado com sucesso|success";

                //ViewData["HISTORICOVISITA"] = new VisitaViewModel().GetHistoricoVisita(Convert.ToInt32(model.Id), model.Cliente.IdPessoa, User.Identity.GetEmpresa());

                return View(new VisitaModel());
            }


            ViewData["HISTORICOVISITA"] = new VisitaViewModel();
            ViewBag.Error = "1|error";

            return View(model);
        }
        private decimal CorrigiValorMoeda(string valor)
        {
            if (string.IsNullOrEmpty(valor))
                return 0;

            var remove = valor.Replace(".", "").Replace(",", ".");
            var value = Decimal.Parse(remove, CultureInfo.InvariantCulture);
            return value;
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

        [HttpGet]
        public PartialViewResult Historico(int idpessoa)
        {
            ViewData["DADOS_CLIENTE"] = ClienteData.Get(idpessoa, User.Identity.GetEmpresa());

            ViewData["HISTORICOVISITA"] = new VisitaViewModel().GetHistoricoVisita(0, idpessoa, User.Identity.GetEmpresa());
            return PartialView("_Historico");
        }

        public ActionResult TransferenciaLead()
        {
            return View();
        }


    }
}