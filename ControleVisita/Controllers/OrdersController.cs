using System.Net.Http;
using System.Web.Http;
using ControleVisita.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;

namespace ControleVisita.Controllers {
    public class OrdersController : ApiController {

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            return Request.CreateResponse(DataSourceLoader.Load(SampleData.Orders, loadOptions));
        }

    }
}