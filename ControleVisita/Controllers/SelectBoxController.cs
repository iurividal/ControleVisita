using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using ControleVisita.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

namespace ControleVisita.Controllers
{

    [System.Web.Http.Route("api/selectbox/{action}", Name = "SelectBox")]
    public class SelectBoxController : ApiController
    {


        // GET api/<controller>
        public IEnumerable<MarcaViewModel> GetBems(string empresa)
        {
            return MarcaViewModel.GetMarca()
                .Where(a => a.Empresa == empresa).OrderBy(a => a.Marca);
        }


        public HttpResponseMessage GetModelos(string empresa, DataSourceLoadOptions loadOptions)
        {
            var response = BemViewModel.GetModelo(empresa);
            return Request.CreateResponse(DataSourceLoader.Load(response, loadOptions));
        }


        public HttpResponseMessage GetMotivoVisita(DataSourceLoadOptions loadOptions)
        {
            return Request.CreateResponse(DataSourceLoader.Load(MotivoViewModel.GetMotivoVisita(), loadOptions));
        }

        public HttpResponseMessage GetVendedores(string empresa, string codgrupo, DataSourceLoadOptions loadOptions)
        {
            return Request.CreateResponse(DataSourceLoader.Load(VendedorModel.GetVendedor(empresa, codgrupo), loadOptions));
        }


        // GET api/<controller>/5
        public IEnumerable<ProfissaoViewModel> GetAtividades()
        {

            return ProfissaoModel.GetProfissao();
        }

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