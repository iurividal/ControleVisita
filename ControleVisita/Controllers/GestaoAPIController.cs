using ControleVisita.Models;
using ControleVisita.Models.IdentityExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace ControleVisita.Controllers
{
    public class GestaoAPIController : ApiController
    {
        [HttpGet]
        public async Task<HttpResponseMessage> GetAsync(DataSourceLoadOptions loadOptions)
        {

            var user = (ClaimsIdentity)User.Identity;
            var response = await Task.FromResult(VisitaViewModel.Get(user.Getcodgrupo(), DateTime.Now.AddMonths(-2), DateTime.Now).Where(a => !a.IsVendaRealizada).ToList());


            var lista = response
                        .GroupBy(a => a.MotivoNaoVenda)
                        .Select(a => new MotivoModel
                        {

                            Motivo = a.Key,
                            Qtd = a.Count()

                        }).ToList();

            return Request.CreateResponse(DataSourceLoader.Load(lista, loadOptions));
        }


       

   

    }
}
