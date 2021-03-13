using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using ApiConsorcioNet.Extensoes;
using OracleContext;
using Dapper;

namespace ControleVisita.Models
{
    public class VisitaViewModel
    {
        public static List<VisitaModel> Get(int codgrupo)
        {
            //new LoginData().GetUser().Empresa
            using (var db = new OracleDataContext())
            {
                // var user = (LoginModel)HttpContext.Current.Session["USERMODEL"];

                var response = db.PCURSORVISITA(codgrupo, null, null)
                    .Select(m => new VisitaModel
                    {
                        Id = m.CODVISITA.GetValueOrDefault(),
                        Cliente = new PessoaModel
                        {
                            NomeCompleto = m.VISITADO,
                            DddCelular = string.IsNullOrEmpty(m.CELULAR) ? m.CELULAR.Substring(0, 2) : "",
                            Celular = m.CELULAR,
                            Telefone = m.FONE,
                            WhatsApp = m.WHATSAPP,
                            Email = m.EMAIL,
                            Atividade = m.ATIVIDADE,
                            Endereco = new EnderecoModel
                            {

                                Logradouro = m.ENDERECO,
                                UF = m.UF,
                                Cidade = m.CIDADE
                            }
                        },
                        ValorBem = m.VALORBEM,
                        ValorBemAux = m.VALORBEM != null ? m.VALORBEM.Value.ToString("N2") : "",
                        DataVisita = m.DATAVISITA,
                        IsVendaRealizada = m.VENDEU == "1",
                        MotivoNaoVenda = m.MOTIVONAOVENDA,
                        BemViewModel = new BemViewModel
                        {
                            Modelo = m.MODELOBEM,
                            Marca = m.MARCABEM
                        },
                        Agendamento = new AgendaModel
                        {
                            DataAgendamento = m.DATAREAGENDAMENTO,

                        },
                        HistoricoVisita = m.OBSERVACOES,
                        Usuario = m.USUARIOATUALIZACAO,
                        DataInclusao = m.DATAINCLUSAO.GetValueOrDefault()

                    });

                return response.ToList();

            }
        }

        public VisitaModel GetVisita(int codvisita, string empresa)
        {
            using (var db = new OracleDataContext(ApiConsorcioNet.Conexao.ConnectionStrings.Acesso(empresa)))
            {
                var response = db.CNVISITAs.Where(a => a.CODVISITA == codvisita).ToList()
                    .Select(t => new VisitaModel
                    {
                        Id = t.CODVISITA,
                        CodGrupo = t.CODGRUPO.ToString(),
                        Cliente = new PessoaModel
                        {
                            NomeCompleto = t.VISITADO,
                            DDDFone = t.DDDFONE,
                            Telefone = t.FONE,
                            Email = t.EMAIL,
                            DddCelular = t.DDDCELULAR,
                            Celular = t.CELULAR,
                            WhatsApp = t.WHATSAPP,
                            Atividade = t.ATIVIDADE,


                            Endereco = new EnderecoModel
                            {
                                UF = t.UF,
                                Cep = "",
                                Cidade = t.CIDADE,
                                Logradouro = t.ENDERECO
                            }

                        },

                        BemViewModel = new BemViewModel
                        {
                            Modelo = t.MODELOBEM,
                            Marca = t.MARCABEM
                        },
                        ValorBemAux = t.VALORBEM != null ? t.VALORBEM.Value.ToString("N2") : "",
                        ValorBem = t.VALORBEM,
                        MotivoNaoVenda = t.MOTIVONAOVENDA,

                        Agendamento = new AgendaModel
                        {
                            DataAgendamento = t.DATAREAGENDAMENTO,
                            InformacaoAgendamento = t.OBSERVACOES,

                        },
                        HistoricoVisita = t.OBSERVACOES,
                        Usuario = t.USUARIOATUALIZACAO,
                        IsVendaRealizada = t.VENDEU == "1",
                        DataVisita = t.DATAVISITA,
                        DataInclusao = t.DATAINCLUSAO.GetValueOrDefault(),
                        MotivoVisita = t.MOTIVOVISITA,
                        Percepcao = t.PERCEPCAO
                        


                    }).ToList();

                return response.FirstOrDefault();
            }
        }

        public static IEnumerable<VisitaModel> GetVisitas(decimal codgrupo, string empresa, FiltroModelView filtro)
        {

            using (var db = new Oracle.ManagedDataAccess.Client.OracleConnection(ApiConsorcioNet.Conexao.ConnectionStrings.AcessoOracleODP(empresa)))
            {
                Dapper.Oracle.OracleDynamicParameters param = new Dapper.Oracle.OracleDynamicParameters();

                var vcursor = @"    select distinct CODVISITA,
                                            COD_GRUPO,
                                            DATA_VISITA,
                                            VISITADO,
                                            FONE,
                                            ENDERECO,
                                            VENDEU,
                                            MOTIVO_NAO_VENDA,
                                            OBSERVACOES,
                                            DATA_REAGENDAMENTO,
                                            DATA_ATUALIZACAO,
                                            USUARIO_ATUALIZACAO,
                                            REAGENDAMENTO_REALIZADO,
                                            EMAIL,
                                            VALORBEM,
                                            MARCABEM,
                                            MODELOBEM,
                                            UF,
                                            CIDADE,
                                            DDDCELULAR,
                                            CELULAR,
                                            DDDFONE,
                                            WHATSAPP,
                                            ATIVIDADE,
                                            NOME,
                                            TIPO_GRUPO,
                                            datainclusao,
                                            COD_GRUPOOUTRO,
                                            DATA_REAGENDAMENTO,
                                            PERCEPCAO,
                                            CHECK_RESULT
                              from (select distinct V.*,
                                                    G.COD_GRUPO RESPONSAVEL,
                                                    GRUPOS.NOME,
                                                    GRUPOS.TIPO_GRUPO,
                                                    -- G.SEQREVENDA,
                                                    VORTICE.f_responsavel_cod(G.SEQREVENDA, 1, 3) COORDENADOR_RESPONSAVEL,
                                                    VORTICE.f_responsavel_cod(G.SEQREVENDA, 4, 3) SUPERVISOR_RESPONSAVEL,
                                                    VORTICE.f_responsavel_cod(G.SEQREVENDA, 11, 3) GERENTE_RESPONSAVEL,
                                                    VORTICE.f_responsavel_cod(G.SEQREVENDA, 6, 3) ADM_RESPONSAVEL,
                                                    TB.cod_grupo COD_GRUPOOUTRO
                           
                                      from CN_VISITAS V
                                     inner join CN_GRUPOS_GEPESSOA G
                                        on G.COD_GRUPO = V.COD_GRUPO
                                      join (select distinct a.seqrevenda,
                                                           a.cod_grupo
                                             from cn_grupos_gepessoa a
                                            where a.seqrevenda in (select g.seqrevenda
                                                                     from cn_grupos_gepessoa g
                                                                    where g.cod_grupo = :CODGRUPOLOGIN
                                                                          and g.sit_grupo = 1)
                                                  and a.cod_grupo = :CODGRUPOLOGIN) TB
                                        on TB.SEQREVENDA = G.SEQREVENDA
                                     inner join CN_GRUPOS GRUPOS
                                        on GRUPOS.COD_GRUPO = V.COD_GRUPO 
                                        where 1 = 1 ";

                #region FILTROS DINAMICOS

                if (filtro.DataVisitaInicial != null)
                {
                    vcursor += @" and v.data_visita between :DTAVISITAINI and :DTAVISITAFIM ";
                    param.Add("DTAVISITAINI", filtro.DataVisitaInicial);
                    param.Add("DTAVISITAFIM", filtro.DataVisitaFinal);
                }

                if (filtro.DataReagentamentoInicial != null)
                {
                    vcursor += @" and v.Data_Reagendamento between :DTAREAGENDAMENTO and :DTAREAGENDAMENTOFIM ";
                    param.Add("DTAREAGENDAMENTO", filtro.DataReagentamentoInicial);
                    param.Add("DTAREAGENDAMENTOFIM", filtro.DataReagentamentoFinal);
                }

                if (!string.IsNullOrEmpty(filtro.Vendedor))
                {
                    vcursor += @" and V.COD_GRUPO = :VENDEDOR";
                    param.Add("VENDEDOR", filtro.Vendedor);

                }

                if (filtro.isVendaRealizada)
                {
                    vcursor += @" and v.vendeu = '1' ";
                }


                vcursor += @" and v.dtaexclusao is null)  UNPIVOT(CHECK_RESULT for COD_GRUPOS in(RESPONSAVEL,COORDENADOR_RESPONSAVEL, SUPERVISOR_RESPONSAVEL,
                                                                                                             GERENTE_RESPONSAVEL,
                                                                                                             ADM_RESPONSAVEL))
                             where CHECK_RESULT = :CODGRUPOLOGIN";


                param.Add("CODGRUPOLOGIN", codgrupo);
                #endregion


                //   var motivos = MotivoViewModel.GetAll();

                var response = db.Query(vcursor, param).Select(m => new VisitaModel
                {

                    Id = m.CODVISITA,
                    Cliente = new PessoaModel
                    {

                        NomeCompleto = m.VISITADO,
                        DddCelular = m.DDDCELULAR,
                        Celular = m.CELULAR,
                        Telefone = m.FONE,
                        WhatsApp = m.WHATSAPP,
                        Email = m.EMAIL,
                        Atividade = m.ATIVIDADE,

                        Endereco = new EnderecoModel
                        {

                            Logradouro = m.ENDERECO,
                            UF = m.UF,
                            Cidade = m.CIDADE
                        }
                    },

                    BemViewModel = new BemViewModel
                    {
                        Modelo = m.MODELOBEM,
                        Marca = m.MARCABEM
                    },
                    ValorBemAux = m.VALORBEM != null ? Convert.ToDecimal(m.VALORBEM).ToString("N2") : "",
                    ValorBem = m.VALORBEM,
                    DataVisita = m.DATA_VISITA,
                    IsVendaRealizada = m.VENDEU == "1",
                    MotivoNaoVenda = m.MOTIVO_NAO_VENDA,
                    Agendamento = new AgendaModel
                    {
                        DataAgendamento = m.DATA_REAGENDAMENTO,

                    },
                    NomeVendedor = m.NOME,
                    HistoricoVisita = m.OBSERVACOES,
                    Usuario = m.USUARIO_ATUALIZACAO,
                    DataInclusao = Convert.ToDateTime(m.DATAINCLUSAO),
                    MotivoVisita = m.MOTIVOVISITA,
                    Percepcao = m.PERCEPCAO
                    //IdMotivo = motivos.First(x => x.Motivo == m.MOTIVO_NAO_VENDA).IdMotivo

                }).ToList();


                return response;
            }

        }


        public static List<VisitaModel> Get(decimal codgrupo, DateTime? dtaini, DateTime? dtafim)
        {
            //new LoginData().GetUser().Empresa
            using (var db = new OracleDataContext())
            {
                //   var user = (LoginModel)HttpContext.Current.Session["USERMODEL"];



                var response = db.PCURSORVISITA(codgrupo, dtaini, dtafim)
                    .Select(m => new VisitaModel
                    {
                        Id = m.CODVISITA.GetValueOrDefault(),
                        Cliente = new PessoaModel
                        {

                            NomeCompleto = m.VISITADO,
                            DddCelular = m.DDDCELULAR,
                            Celular = m.CELULAR,
                            Telefone = m.FONE,
                            WhatsApp = m.WHATSAPP,
                            Email = m.EMAIL,
                            Atividade = m.ATIVIDADE,
                            Endereco = new EnderecoModel
                            {

                                Logradouro = m.ENDERECO,
                                UF = m.UF,
                                Cidade = m.CIDADE
                            }
                        },

                        BemViewModel = new BemViewModel
                        {
                            Modelo = m.MODELOBEM,
                            Marca = m.MARCABEM
                        },
                        ValorBemAux = m.VALORBEM != null ? m.VALORBEM.Value.ToString("N2") : "",
                        ValorBem = m.VALORBEM,
                        DataVisita = m.DATAVISITA,
                        IsVendaRealizada = m.VENDEU == "1",
                        MotivoNaoVenda = m.MOTIVONAOVENDA,
                        Agendamento = new AgendaModel
                        {
                            DataAgendamento = m.DATAREAGENDAMENTO,

                        },
                        HistoricoVisita = m.OBSERVACOES,
                        Usuario = m.USUARIOATUALIZACAO,
                        DataInclusao = m.DATAINCLUSAO.GetValueOrDefault(),
                        MotivoVisita = m.MOTIVOVISITA,
                        Percepcao = m.PERCEPCAO

                    });

                return response.OrderByDescending(a => a.DataVisita).ToList();

            }
        }


        public async Task<IEnumerable<VisitaModel>> GetNextVisita(decimal codgrupo, string empresa)
        {
            using (var db = new OracleDataContext(ApiConsorcioNet.Conexao.ConnectionStrings.Acesso(empresa)))
            {

                var result = db.CNVISITAs
                    .Where(a => a.CODGRUPO == codgrupo && a.DATAREAGENDAMENTO == DateTime.Now.AddDays(1).Date /*&& a.DATAREAGENDAMENTO <= DateTime.Now.AddDays(6).Date*/).ToList()
                    .Select(m => new VisitaModel
                    {

                        Id = m.CODVISITA,
                        Cliente = new PessoaModel
                        {

                            NomeCompleto = m.VISITADO,
                            DddCelular = m.DDDCELULAR,
                            Celular = m.CELULAR,
                            Telefone = m.FONE,
                            WhatsApp = m.WHATSAPP,
                            Email = m.EMAIL,
                            Atividade = m.ATIVIDADE,

                            Endereco = new EnderecoModel
                            {

                                Logradouro = m.ENDERECO,
                                UF = m.UF,
                                Cidade = m.CIDADE
                            }
                        },

                        BemViewModel = new BemViewModel
                        {
                            Modelo = m.MODELOBEM,
                            Marca = m.MARCABEM
                        },
                        ValorBemAux = m.VALORBEM != null ? m.VALORBEM.Value.ToString("N2") : "",
                        ValorBem = m.VALORBEM,
                        DataVisita = m.DATAVISITA,
                        IsVendaRealizada = m.VENDEU == "1",
                        MotivoNaoVenda = m.MOTIVONAOVENDA,
                        Agendamento = new AgendaModel
                        {
                            DataAgendamento = m.DATAREAGENDAMENTO,

                        },
                        HistoricoVisita = m.OBSERVACOES,
                        Usuario = m.USUARIOATUALIZACAO,
                        DataInclusao = m.DATAINCLUSAO.GetValueOrDefault(),
                        MotivoVisita = m.MOTIVOVISITA

                    });

                return await Task.FromResult(result.ToList());



            }
        }


        public static void AddOrUpdate(VisitaModel model, LoginModel user)
        {

            using (var db = new OracleDataContext())
            {
                try
                {


                    var entity = new CNVISITA();

                    var visita = db.CNVISITAs.FirstOrDefault(a => a.CODVISITA == model.Id);

                    if (visita == null)
                    {


                        entity.CODVISITA = db.CNVISITAs.Max(a => a.CODVISITA) + 1;
                        entity.VISITADO = model.Cliente.NomeCompleto;
                        entity.VENDEU = !string.IsNullOrEmpty(model.MotivoNaoVenda) ? "0" : "1";
                        entity.DATAREAGENDAMENTO = model.Agendamento.DataAgendamento;
                        entity.ENDERECO = model.Cliente.Endereco.Logradouro;
                        entity.CIDADE = model.Cliente.Endereco.Cidade;
                        entity.UF = model.Cliente.Endereco.UF;
                        entity.OBSERVACOES = model.HistoricoVisita;
                        entity.FONE = model.Cliente.Telefone;
                        entity.DDDFONE = model.Cliente.DDDFone;
                        entity.DDDCELULAR = model.Cliente.DddCelular;
                        entity.CELULAR = model.Cliente.Celular;
                        entity.WHATSAPP = model.Cliente.WhatsApp.OnlyNumbers();
                        entity.CODGRUPO = Convert.ToInt32(user.CodGrupo);
                        entity.VALORBEM = model.ValorBem;
                        entity.MOTIVONAOVENDA = string.IsNullOrEmpty(model.MotivoNaoVenda) ? "" : model.MotivoNaoVenda.ToUpper();
                        entity.EMAIL = model.Cliente.Email;
                        entity.DATAVISITA = model.DataVisita.GetValueOrDefault();
                        entity.USUARIOATUALIZACAO = user.Login.Replace("%2E", ".");
                        entity.REAGENDAMENTOREALIZADO = model.Agendamento.DataAgendamento == null ? true : false;
                        entity.DATAATUALIZACAO = DateTime.Now.ToString("dd/MM/yyyy");
                        entity.MARCABEM = model.BemViewModel.Marca;
                        entity.MODELOBEM = model.BemViewModel.Modelo;
                        entity.DATAINCLUSAO = DateTime.Now;
                        entity.USUARIOINCLUSAO = user.Login.Replace("%2E", ".");
                        entity.ATIVIDADE = model.Cliente.Atividade;
                        entity.MOTIVOVISITA = model.MotivoVisita;
                        entity.PERCEPCAO = model.Percepcao;
                        db.CNVISITAs.InsertOnSubmit(entity);

                        db.SubmitChanges();
                    }
                    else
                    {

                        visita.VISITADO = model.Cliente.NomeCompleto;
                        visita.VENDEU = !string.IsNullOrEmpty(model.MotivoNaoVenda) ? "0" : "1";
                        visita.DATAREAGENDAMENTO = model.Agendamento.DataAgendamento;
                        visita.ENDERECO = model.Cliente.Endereco.Logradouro;
                        visita.CIDADE = model.Cliente.Endereco.Cidade;
                        visita.UF = model.Cliente.Endereco.UF;
                        visita.OBSERVACOES = model.HistoricoVisita;
                        visita.DDDFONE = model.Cliente.DDDFone;
                        visita.FONE = model.Cliente.Telefone;
                        visita.DDDCELULAR = model.Cliente.DddCelular;
                        visita.CELULAR = model.Cliente.Celular.OnlyNumbers();
                        visita.WHATSAPP = model.Cliente.WhatsApp.OnlyNumbers();
                        //visita.CODGRUPO = Convert.ToInt32(user.CodGrupo);
                        visita.VALORBEM = model.ValorBem;
                        visita.MOTIVONAOVENDA = string.IsNullOrEmpty(model.MotivoNaoVenda) ? "" : model.MotivoNaoVenda.ToUpper();
                        visita.EMAIL = model.Cliente.Email;
                        visita.DATAVISITA = model.DataVisita.GetValueOrDefault();
                        // visita.USUARIOATUALIZACAO = user.Login.Replace("%2E", ".");
                        visita.REAGENDAMENTOREALIZADO = model.Agendamento.DataAgendamento == null ? true : false;
                        visita.DATAATUALIZACAO = DateTime.Now.ToString("dd/MM/yyyy");
                        visita.MODELOBEM = model.BemViewModel.Modelo;
                        visita.MARCABEM = model.BemViewModel.Marca;
                        visita.ATIVIDADE = model.Cliente.Atividade;
                        visita.MOTIVOVISITA = model.MotivoVisita;
                        visita.PERCEPCAO = model.Percepcao;
                        db.SubmitChanges();
                    }


                }
                catch (Exception ex)
                {
                    throw new Exception(" " + ex);
                }
            }

        }

        public static void Delete(int codvisita, string empresa, string usuario)
        {

            using (var db = new OracleDataContext(ApiConsorcioNet.Conexao.ConnectionStrings.Acesso(empresa)))
            {

                var visita = db.CNVISITAs.First(a => a.CODVISITA == codvisita);


                visita.DTAEXCLUSAO = DateTime.Now;
                visita.USUARIOEXCLUSAO = usuario;
                db.SubmitChanges();

            }
        }
    }
}