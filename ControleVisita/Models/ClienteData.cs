using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ApiConsorcioNet.Extensoes;
using OracleContext;

namespace ControleVisita.Models
{
    public class ClienteData
    {

        public static async Task<IEnumerable<PessoaModel>> Get(string empresa, string usuario, string subempresa)
        {
            using (var db = new OracleDataContext(ApiConsorcioNet.Conexao.ConnectionStrings.Acesso(empresa)))
            {
                var results = await Task.FromResult(db.CNVISITAPESSOAs);


                var usuarioPermissao = "";
                if (!string.IsNullOrEmpty(usuario))
                {
                    usuarioPermissao = db.WEBLOGINs.First(a => a.CODGRUPO == Convert.ToDecimal(usuario)).LOGIN;
                }

                var empresaPermissaoColletion = subempresa.Split(';').ToArray();

                var result = results
                    .Where(a => empresaPermissaoColletion.Contains(a.CODEMPRESA.ToString()))
                    .WhereIf(!string.IsNullOrEmpty(usuario), x => x.USUARIOINCLUSAO == usuarioPermissao);


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
                        TipoPessoa = a.TIPOPESSOA,
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

                return responseList.OrderBy(a => a.NomeCompleto);
            }

        }

        public static async Task<PessoaModel> Get(int id, string empresa)
        {
            using (var db = new OracleDataContext(ApiConsorcioNet.Conexao.ConnectionStrings.Acesso(empresa)))
            {
                var a = await Task.FromResult(db.CNVISITAPESSOAs.FirstOrDefault(b => b.IDPESSOA == id));


                if (a == null)
                    return new PessoaModel();


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
                    TipoPessoa = a.TIPOPESSOA,
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

       
        public static async Task AddOrUpdate(PessoaModel model, string empresa, int codsubempresa)
        {
            using (var db = new OracleDataContext(ApiConsorcioNet.Conexao.ConnectionStrings.Acesso(empresa)))
            {
                var dados = Newtonsoft.Json.JsonConvert.SerializeObject(model);

                var result = await Task.FromResult(db.CNVISITAPESSOAs.Where(a => a.IDPESSOA == model.IdPessoa));

                if (result.Any())
                {
                    result.ToList().ForEach(item =>
                    {
                        item.NOMECOMPLETO = model.NomeCompleto.Trim().ToUpper();
                        item.DDDFONE = model.DDDFone;
                        item.TELEFONE = model.Telefone;
                        item.TIPOPESSOA = model.TipoPessoa;
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
                        NOMECOMPLETO = model.NomeCompleto.ToUpper(),
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
                        USUARIOINCLUSAO = "",
                        CODEMPRESA = codsubempresa
                    };

                    db.CNVISITAPESSOAs.InsertOnSubmit(cNVISITAPESSOA);
                    db.SubmitChanges();

                }

            }
        }


        public static async Task Delete(int idpessoa, string empresa)
        {
            using (var db = new OracleDataContext(ApiConsorcioNet.Conexao.ConnectionStrings.Acesso(empresa)))
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
                new SelectListItem { Value = "F", Text = "Física" },
                new SelectListItem { Value = "J", Text = "Jurídica" }

            };

            return new SelectList(list, "Value", "Text");

        }
    }
}