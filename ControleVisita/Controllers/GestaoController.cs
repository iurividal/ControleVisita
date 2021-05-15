using ControleVisita.Models;
using ControleVisita.Models.IdentityExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ControleVisita.Controllers
{
    [Authorize]
    public class GestaoController : Controller
    {
        // GET: Gestao


        public async Task<ActionResult> Vendas()
        {


            return View(new List<VendasChartViewModel>());
        }

        [HttpPost]
        public async Task<ActionResult> Vendas(DateTime datainicial, DateTime datafinal)
        {

            var visita = new VisitaModel();

            visita.DataVisita = datainicial;
            visita.DataVisitaFinal = datafinal;

            var user = (ClaimsIdentity)User.Identity;

            var response = VendasChartModel.GetVendaPeriodo(user.Getcodgrupo(),user.GetEmpresa(), datainicial, datafinal).ToList();

            return View(response);
        }



        public ActionResult MotivoNaoVenda()
        {
            ViewBag.TituloGrafico = "Motivos não venda";
            return View(new List<MotivoNaoVendaChartViewModel>());
        }

        [HttpPost]
        public async Task<ActionResult> MotivoNaoVenda(DateTime datainicial, DateTime datafinal)
        {

            var user = (ClaimsIdentity)User.Identity;

            var response = await Task.FromResult(VisitaViewModel.Get(user.Getcodgrupo(),user.GetEmpresa(), datainicial, datafinal).Where(a => !a.IsVendaRealizada).ToList());
            ViewBag.TituloGrafico =
                $"Motivos não venda {datainicial.Date.ToString("dd/MM/yyyy")} até {datafinal.Date.ToString("dd/MM/yyyy")}";

           // var total = response.Count();

            var lista = response
                .GroupBy(a => a.MotivoNaoVenda)
                .Select(a => new MotivoNaoVendaChartViewModel
                {

                    MotivoNaoVenda = a.Key,
                    Valor = a.Count(), //(a.Count() / total) * 100,
                    Cliente = new PessoaModel
                    {
                        Endereco = new EnderecoModel
                        {
                            UF = a.First().Cliente.Endereco.UF
                        }
                    }

                }).ToList();

            return View(lista);
        }


        public async Task<ActionResult> Get(DataSourceLoadOptions loadOptions)
        {

            var user = (ClaimsIdentity)User.Identity;
            var response = await Task.FromResult(VisitaViewModel.Get(user.Getcodgrupo(),user.GetEmpresa(), DateTime.Now.AddMonths(-1), DateTime.Now).Where(a => !a.IsVendaRealizada).ToList());


            var lista = response
                        .GroupBy(a => a.MotivoNaoVenda)
                        .Select(a => new MotivoModel
                        {

                            Motivo = a.Key,
                            Qtd = a.Count()

                        }).ToList();


            var result = DataSourceLoader.Load(lista, loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }

        private async Task<IEnumerable<VisitaModel>> ListaVendaMesal(VisitaModel visitaModel)
        {

            if (visitaModel == null)
                return new List<VisitaModel>();

            var user = (ClaimsIdentity)User.Identity;
            var response = await Task.FromResult(VisitaViewModel.Get(user.Getcodgrupo(),user.GetEmpresa(), visitaModel.DataVisita, visitaModel.DataVisitaFinal)
                .Where(a => a.IsVendaRealizada).ToList());

            return response;
        }

        public async Task<ActionResult> GetVendaMensal(DataSourceLoadOptions loadOptions)
        {

            var user = (ClaimsIdentity)User.Identity;
            var response = await Task.FromResult(VisitaViewModel.Get(user.Getcodgrupo(),user.GetEmpresa(), DateTime.Now.AddMonths(-1), DateTime.Now).Where(a => a.IsVendaRealizada).ToList());


            var lista = response
                        .GroupBy(a => a.DataVisita.Value.ToString("MMM"))
                        .Select(a => new
                        {

                            MES = a.Key,
                            QTD = a.Count()

                        }).ToList();


            var result = DataSourceLoader.Load(lista, loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }





    }
}