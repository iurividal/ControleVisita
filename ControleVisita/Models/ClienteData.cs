using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using OracleContext;

namespace ControleVisita.Models
{
    public class ClienteData
    {

        public static async Task<IEnumerable<PessoaModel>> Get()
        {
            using (var db = new OracleDataContext())
            {
                var result = await Task.FromResult(db.CNVISITAPESSOAs);

                List<PessoaModel> responseList = new List<PessoaModel>();

                result.ToList().ForEach(a =>
                {
                    var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<PessoaModel>(a.DADOS);
                    obj.IdPessoa = Convert.ToInt32(a.IDPESSOA);
                    responseList.Add(obj);

                });

                return responseList;
            }

        }

        public static async Task<PessoaModel> Get(int id)
        {
            using (var db = new OracleDataContext())
            {
                var result = await Task.FromResult(db.CNVISITAPESSOAs.First(a => a.IDPESSOA == id));

                var pessoa = Newtonsoft.Json.JsonConvert.DeserializeObject<PessoaModel>(result.DADOS);
                pessoa.IdPessoa = id;
                return pessoa;
            }

        }

        public static async Task Update(PessoaModel model)
        {
            using (var db = new OracleDataContext())
            {

                var dados = Newtonsoft.Json.JsonConvert.SerializeObject(model);

                var result = await Task.FromResult(db.CNVISITAPESSOAs.Where(a => a.IDPESSOA == model.IdPessoa));

                if (result.Any())
                {
                    result.ToList().ForEach(item =>
                    {
                        item.DADOS = dados;
                    });

                    db.SubmitChanges();
                }

            }
        }


        public static async Task Delete(int idpessoa)
        {
            using (var db = new OracleDataContext())
            {
                var response = db.CNVISITAPESSOAs.First(e => e.IDPESSOA == idpessoa);

                db.CNVISITAPESSOAs.DeleteOnSubmit(response);
                db.SubmitChanges();

            }
        }

        public static IEnumerable<SelectListItem> GetTipoPessoa()
        {
            var list = new[]
            {
                new SelectListItem { Value = "1", Text = "Pessoa Física" },
                new SelectListItem { Value = "2", Text = "Pessoa Jurídica" }

            };

            return new SelectList(list, "Value", "Text");

        }


        public static void AddOrUpdateCliente(PessoaModel clienteModel)
        {

            using (var db = new OracleContext.OracleDataContext())
            {

                string dados = Newtonsoft.Json.JsonConvert.SerializeObject(clienteModel);

                CNVISITAPESSOA cliente = new CNVISITAPESSOA
                {
                    DADOS = dados
                };

                db.CNVISITAPESSOAs.InsertOnSubmit(cliente);
                db.SubmitChanges();
            }

        }
    }
}