using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;
using ControleVisita.Models;
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

        [HttpGet]
        public async Task<HttpResponseMessage> Get(DataSourceLoadOptions loadOptions)
        {
            var reponse = DataSourceLoader.Load(await Models.ClienteData.Get(), loadOptions);

            return Request.CreateResponse(reponse);
        }


        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        //public async void Put(int id, [FromBody]PessoaModel value)
        //{
        //    await ClienteData.Update(value);
        //}

        [HttpPut]
        public async Task<HttpResponseMessage> Put(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var values = form.Get("values");

            var cliente = await ClienteData.Get(key);

            Newtonsoft.Json.JsonConvert.PopulateObject(values, cliente);

            Validate(cliente);

            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Falha");

            await ClienteData.Update(cliente);

            return Request.CreateResponse(HttpStatusCode.OK);

            // await ClienteData.Update(value);
        }


        // DELETE api/<controller>/5
        [HttpDelete]
        public async Task Delete(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));

            await ClienteData.Delete(key);

        }
    }
}