using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ControleVisita.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using OracleContext;

namespace ControleVisita.Controllers
{
    public class CidadeApiController : ApiController
    {
        // GET api/<controller>

        [Route("api/UFApi/")]
        public HttpResponseMessage GetUF(DataSourceLoadOptions loadOptions)
        {
            using (var db = new OracleDataContext())
            {
                var respose = db.ESTADOs.ToList();
                return Request.CreateResponse(DataSourceLoader.Load(respose, loadOptions));
            }


        }

        [HttpGet]
        public HttpResponseMessage GetCidade(DataSourceLoadOptions loadOptions)
        {
            using (var db = new OracleDataContext())
            {
                var respose = (from c in db.CIDADEs
                               join e in db.ESTADOs on c.ESTADO equals e.ID
                               select new
                               {
                                   c.NOME,
                                   ESTADO = e.NOME

                               }).ToList();


                return Request.CreateResponse(DataSourceLoader.Load(respose, loadOptions));
            }

        }

        public IEnumerable<CIDADE> Get(int id)
        {
            using (var db = new OracleDataContext())
            {
                return db.CIDADEs.Where(a => a.ESTADO == id);

            }
        }
        // GET api/<controller>/5

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}