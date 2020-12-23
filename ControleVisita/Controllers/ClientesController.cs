using System.Web.Mvc;
using ControleVisita.Models;

namespace ControleVisita.Controllers
{
    [Authorize]
    public class ClientesController : Controller
    {
        // GET: Clientes
        public ActionResult Index()
        {
            return View(ClienteData.Get());
        }

        public ActionResult Cadastrar()
        {
            ViewBag.ListTipoPessoa = Models.ClienteData.GetTipoPessoa();

            var model = new PessoaModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(PessoaModel model)
        {

            if (ModelState.IsValid)
            {
                ClienteData.AddOrUpdateCliente(model);

                return RedirectToAction("Index");
            }

            ViewBag.ListTipoPessoa = Models.ClienteData.GetTipoPessoa();

            return View(model);




        }
    }
}