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

        public ActionResult Cadastrar(string returnUrl)
        {

            ViewBag.ListTipoPessoa = Models.ClienteData.GetTipoPessoa();

            TempData["ReturnUrl"] = returnUrl;


            var model = new PessoaModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(PessoaModel model)
        {

            if (ModelState.IsValid)
            {
                ClienteData.AddOrUpdate(model).Wait();

                if (TempData["ReturnUrl"] != null)
                    return Redirect(TempData["ReturnUrl"].ToString());


                ViewBag.Msg = "Cadastro com Sucesso";
            }

            ViewBag.ListTipoPessoa = Models.ClienteData.GetTipoPessoa();

            ViewBag.Msg = "Falha, não foi possível cadastrar o cliente";

            return View(model);




        }
    }
}