using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;
using ControleVisita.Models;
using ControleVisita.Models.IdentityExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;

namespace ControleVisita.Controllers
{
    public class ClienteController : ApiController
    {
        // GET api/<controller>
        //public async Task<IEnumerable<PessoaModel>> Get()
        //{
        //    return await Models.ClienteData.Get();
        //}

        [Route("api/clientes")]
        [HttpGet]
        public async Task<HttpResponseMessage> Get(string empresa, string usuario, DataSourceLoadOptions loadOptions)
        {
            var reponse = DataSourceLoader.Load(await Models.ClienteData.Get(empresa, usuario, User.Identity.GetEmpresaPermissao()), loadOptions);

            return Request.CreateResponse(reponse);
        }


        [Route("api/clientes")]
        public string Get(int id)
        {
            return "value";
        }

        [Route("api/clientes")]
        public async Task<HttpResponseMessage> Get(string nomerazao, string empresa)
        {

            if (string.IsNullOrEmpty(nomerazao))
                return Request.CreateResponse(HttpStatusCode.OK);

            var cliente = await ClienteData
                                    .Get(empresa, "", User.Identity.GetEmpresaPermissao());

            if (cliente.Any(a => a.NomeCompleto == nomerazao.Trim().ToUpper()))
            {
                return Request.CreateResponse(HttpStatusCode.OK, "<i class='fas fa-exclamation-triangle'></i> Existe um cliente cadastrado com esse mesmo nome");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }

        }


        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        //public async void Put(int id, [FromBody]PessoaModel value)
        //{
        //    await ClienteData.Update(value);
        //}

        [Route("api/clientes")]
        [HttpPut]
        public async Task<HttpResponseMessage> Put(FormDataCollection form, string empresa)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var values = form.Get("values");

            var cliente = await ClienteData.Get(key, empresa);

            Newtonsoft.Json.JsonConvert.PopulateObject(values, cliente);

            Validate(cliente);

            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Falha");

            await ClienteData.AddOrUpdate(cliente, empresa, Convert.ToInt32(User.Identity.GetEmpresaPermissao().Split(';')[0]));

            return Request.CreateResponse(HttpStatusCode.OK);

            // await ClienteData.Update(value);
        }


        [Route("api/clientes/Update")]
        [HttpPut]
        public async Task<HttpResponseMessage> Put_Update(PessoaModel pessoa, string empresa)
        {
            var key = pessoa.IdPessoa;
            var values = Newtonsoft.Json.JsonConvert.SerializeObject(pessoa);

            var cliente = await ClienteData.Get(key, empresa);

            Newtonsoft.Json.JsonConvert.PopulateObject(values, cliente);

            Validate(cliente);

            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Falha");

            await ClienteData.AddOrUpdate(cliente, empresa, Convert.ToInt32(User.Identity.GetEmpresaPermissao().Split(';')[0]));

            return Request.CreateResponse(HttpStatusCode.OK);

            // await ClienteData.Update(value);
        }

        // DELETE api/<controller>/5
        [HttpDelete]
        public async Task Delete(FormDataCollection form, string empresa)
        {
            var key = Convert.ToInt32(form.Get("key"));

            await ClienteData.Delete(key, empresa);

        }
    }
}