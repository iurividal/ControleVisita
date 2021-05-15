using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using Antlr.Runtime.Tree;
using ApiConsorcioNet.Extensoes;
using ControleVisita.Models;
using ControleVisita.Models.IdentityExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using FluentDateTime;
using Newtonsoft.Json;

namespace ControleVisita.Controllers
{
    public class VisitaApiController : ApiController
    {
        // GET api/<controller>



        [HttpGet]
        public HttpResponseMessage Get(string usuario, string empresa, DataSourceLoadOptions loadOptions)
        {

            var response = VisitaViewModel
                .Get(User.Identity.Getcodgrupo(), empresa, DateTime.Now.Date.Date.FirstDayOfMonth(), DateTime.Now.Date.LastDayOfMonth())
                .Where(a => a.Usuario == usuario && a.DataInclusao.Date == DateTime.Now.Date)
                .ToList().OrderByDescending(a => a.DataVisita);

            return Request.CreateResponse(DataSourceLoader.Load(response, loadOptions));
        }


        // PUT api/<controller>/5
        //[HttpPut]
        public HttpResponseMessage Put(int id, string empresa, string codgrupo, string login, FormDataCollection form)
        {
            try
            {
                // var key = Convert.ToInt32(form.Get("Key"));
                var values = form.Get("values");

                var visita = new Models.VisitaViewModel().GetVisita(id, empresa);

                JsonConvert.PopulateObject(values, visita);

                Validate(visita);


                Models.VisitaViewModel.AddOrUpdate(visita, new LoginModel { CodGrupo = codgrupo, Login = login });


                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, $"Não foi possível atualizar a visita \n{e.Message}");
            }


        }

        // DELETE api/<controller>/5
        [HttpDelete]
        public HttpResponseMessage Delete(int id, string usuario, string empresa, FormDataCollection form)
        {
            try
            {
                VisitaViewModel.Delete(id, empresa, usuario);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, $"Não foi possível delete a visita \n{e.Message}");
            }

        }

        [HttpDelete]
        public void DeleteOrder(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));

            var usuario = User.Identity.GetUsuario();
            var empresa = User.Identity.GetEmpresa();
            VisitaViewModel.Delete(key, empresa, usuario);


        }


    }
}