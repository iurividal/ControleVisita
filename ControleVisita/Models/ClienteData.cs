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
                    //  var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<PessoaModel>(a.DADOS);

                    responseList.Add(new PessoaModel
                    {
                        NomeCompleto = a.NOMECOMPLETO,
                        Atividade = a.ATIVIDADE,
                        DataNascimento = a.DATANASCIMENTO,
                        DddCelular = a.DDDCELULAR,
                        Celular = a.CELULAR,
                        DDDFone = a.DDDFONE,
                        Telefone = a.TELEFONE,
                        Documento = a.DOCUMENTO,
                        TipoPessoa = a.TIPOPESSOA == "F" ? Enum.TipoPessoa.Fisica : Enum.TipoPessoa.Juridica,
                        Email = a.EMAIL,
                        WhatsApp = a.WHATSAPP,
                        Endereco = new EnderecoModel
                        {
                            Cep = a.CEP,
                            Logradouro = a.LOGRADOURO,
                            Cidade = a.CIDADE,
                            UF = a.UF
                        },
                        IdPessoa = Convert.ToInt32(a.IDPESSOA),
                        Informacao = a.INFORMACAO
                    });
                });

                return responseList;
            }

        }

        public static async Task<PessoaModel> Get(int id)
        {
            using (var db = new OracleDataContext())
            {
                var a = await Task.FromResult(db.CNVISITAPESSOAs.First(b => b.IDPESSOA == id));

                var pessoa = new PessoaModel
                {
                    NomeCompleto = a.NOMECOMPLETO,
                    Atividade = a.ATIVIDADE,
                    DataNascimento = a.DATANASCIMENTO,
                    DddCelular = a.DDDCELULAR,
                    Celular = a.CELULAR,
                    DDDFone = a.DDDFONE,
                    Telefone = a.TELEFONE,
                    Documento = a.DOCUMENTO,
                    TipoPessoa = a.TIPOPESSOA == "F" ? Enum.TipoPessoa.Fisica : Enum.TipoPessoa.Juridica,
                    Email = a.EMAIL,
                    WhatsApp = a.WHATSAPP,
                    Endereco = new EnderecoModel
                    {
                        Cep = a.CEP,
                        Logradouro = a.LOGRADOURO,
                        Cidade = a.CIDADE,
                        UF = a.UF
                    },
                    IdPessoa = Convert.ToInt32(a.IDPESSOA),
                    Informacao = a.INFORMACAO
                };

                return pessoa;
            }

        }

        public static async Task AddOrUpdate(PessoaModel model)
        {
            using (var db = new OracleDataContext())
            {

                var dados = Newtonsoft.Json.JsonConvert.SerializeObject(model);

                var result = await Task.FromResult(db.CNVISITAPESSOAs.Where(a => a.IDPESSOA == model.IdPessoa));

                if (result.Any())
                {
                    result.ToList().ForEach(item =>
                    {
                        item.NOMECOMPLETO = model.NomeCompleto;
                        item.DDDFONE = model.DDDFone;
                        item.TELEFONE = model.Telefone;
                        item.LOGRADOURO = model.Endereco.Logradouro;
                        item.CIDADE = model.Endereco.Cidade;
                        item.DATANASCIMENTO = model.DataNascimento;
                        item.DOCUMENTO = model.Documento;
                        item.EMAIL = model.Email;
                        item.CEP = model.Endereco.Cep;
                        item.ATIVIDADE = model.Atividade;
                        item.INFORMACAO = model.Informacao;

                    });

                    db.SubmitChanges();
                }
                else
                {
                    CNVISITAPESSOA cNVISITAPESSOA = new CNVISITAPESSOA
                    {
                        NOMECOMPLETO = model.NomeCompleto,
                        DOCUMENTO = model.Documento,
                        TIPOPESSOA = model.TipoPessoa.ToString(),
                        DDDFONE = model.DDDFone,
                        TELEFONE = model.Telefone,
                        DDDCELULAR = model.DDDFone,
                        CELULAR = model.Celular,
                        WHATSAPP = model.WhatsApp,
                        DATANASCIMENTO = model.DataNascimento,
                        EMAIL = model.Email,
                        CEP = model.Endereco.Cep,
                        LOGRADOURO = model.Endereco.Logradouro,
                        UF = model.Endereco.UF,
                        CIDADE = model.Endereco.Cidade,
                        BAIRRO = model.Endereco.Bairro,
                        ATIVIDADE = model.Atividade,
                        INFORMACAO = model.Informacao,
                        DTAINCLUSAO = DateTime.Now,
                        USUARIOINCLUSAO = ""
                    };

                    db.CNVISITAPESSOAs.InsertOnSubmit(cNVISITAPESSOA);
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

        public static IDictionary<string, string> GetTypePessoa()
        {

            IDictionary<string, string> dictionary = new Dictionary<string, string>();

            dictionary.Add("F", "FÍSICA");
            dictionary.Add("J", "JURÍDICA");

            return dictionary;
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
    }
}