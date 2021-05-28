using System;
using System.Web.Mvc;
using ControleVisita.Models;
using ControleVisita.Models.IdentityExtensions;

namespace ControleVisita.Controllers
{
    [Authorize]
    public class ClientesController : Controller
    {
        // GET: Clientes
        public ActionResult Index()
        {
            return View(ClienteData.Get(User.Identity.GetEmpresa(), User.Identity.Getcodgrupo().ToString(), User.Identity.GetEmpresaPermissao()));
        }

        public ActionResult Cadastrar(int idpessoa, string returnUrl)
        {

            ViewBag.ListTipoPessoa = Models.ClienteData.GetTipoPessoa();

            var model = new PessoaModel();

            model.Status = "Ativo";
            if (idpessoa != 0)
            {
                model = ClienteData.Get(idpessoa, User.Identity.GetEmpresa()).Result;
            }

            TempData["ReturnUrl"] = returnUrl;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(PessoaModel model)
        {

            if (ModelState.IsValid)
            {

                var codsubempresa = Convert.ToInt32(User.Identity.GetEmpresaPermissao().Split(';')[0]);
                ClienteData.AddOrUpdate(model, User.Identity.GetEmpresa(), codsubempresa).Wait();

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