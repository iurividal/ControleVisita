using System.Net.Http;
using System.Web.Http;
using ControleVisita.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;

namespace ControleVisita.Controllers
{

    public class MotivoApiController : ApiController
    {
        // GET api/<controller>
        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions)
        {
            return Request.CreateResponse(DataSourceLoader.Load(MotivoViewModel.GetAll(), loadOptions));
        }



    }
}