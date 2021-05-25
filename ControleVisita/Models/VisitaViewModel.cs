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
using System.Globalization;

namespace ControleVisita.Models
{
    public class VisitaViewModel
    {
        public static List<VisitaModel> Get(int codgrupo, string empresa)
        {
            //new LoginData().GetUser().Empresa
            using (var db = new OracleDataContext(ApiConsorcioNet.Conexao.ConnectionStrings.Acesso(empresa)))
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
                        IdPessoa = t.IDPESSOA == null ? 0 : Convert.ToInt32(t.IDPESSOA),
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
                            IdPessoa = t.IDPESSOA == null ? 0 : Convert.ToInt32(t.IDPESSOA),


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

        public IEnumerable<VisitaHistoricoModel> GetHistoricoVisita(int codvisita, int idpessoa, string empresa)
        {
            using (var db = new Oracle.ManagedDataAccess.Client.OracleConnection(ApiConsorcioNet.Conexao.ConnectionStrings.AcessoOracleODP(empresa)))
            {
                var query = @"SELECT DISTINCT VW.* FROM (
                            select T.DATA_VISITA,
                                            T.OBSERVACOES,
                                            T.MOTIVOVISITA,
                                            T.PERCEPCAO,
                                            T.DATA_REAGENDAMENTO,
                                            T.USUARIOINCLUSAO,
                                            T.MOTIVO_NAO_VENDA,
                                            T.MARCABEM,
                                            T.MODELOBEM,
                                            T.VALORBEM
                              from CN_VISITAS T
                              where t.codvisita = :CODVISITA and t.DTAEXCLUSAO IS NULL
                              union all 
                              select T.DATA_VISITA,
                                            T.OBSERVACOES,
                                            T.MOTIVOVISITA,
                                            T.PERCEPCAO,
                                            T.DATA_REAGENDAMENTO,
                                            T.USUARIOINCLUSAO,
                                            T.MOTIVO_NAO_VENDA,
                                            T.MARCABEM,
                                            T.MODELOBEM,
                                            T.VALORBEM
                              from CN_VISITAS T  
                             where t.idpessoa = :IDPESSOA AND t.DTAEXCLUSAO IS NULL
                             order by DATA_VISITA desc) VW";


                var response = db.Query(query, new { CODVISITA = codvisita, IDPESSOA = idpessoa });

                List<VisitaHistoricoModel> historicoModels = new List<VisitaHistoricoModel>();
                foreach (var item in response)
                {
                    var visita = new VisitaHistoricoModel();

                    visita.DataVisita = item.DATA_VISITA;
                    visita.HistoricoVisita = item.OBSERVACOES;
                    visita.MotivoVisita = item.MOTIVOVISITA;
                    visita.Percepcao = item.PERCEPCAO;
                    visita.Usuario = item.USUARIOINCLUSAO;
                    visita.MotivoNaoVenda = item.MOTIVO_NAO_VENDA;
                    visita.BemViewModel = new BemViewModel { ValorBem = Convert.ToDecimal(item.VALORBEM), Marca = item.MARCABEM, Modelo = item.MODELOBEM };

                    visita.ValorBem = Convert.ToDecimal(item.VALORBEM);
                    visita.ValorBemAux = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", visita.ValorBem);


                    visita.Agendamento = new AgendaModel
                    {
                        DataAgendamento = item.DATA_REAGENDAMENTO
                    };

                    historicoModels.Add(visita);

                }

                return historicoModels.OrderByDescending(a => a.DataVisita).ToList();
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
                                            NOMECOMPLETO AS VISITADO,
                                           TELEFONE as FONE,
                                          logradouro as ENDERECO,
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
                                            CHECK_RESULT,
                                            TIPOCONTATO,
                                            NVL(IDPESSOA,0)IDPESSOA
                              from (select distinct V.CODVISITA,
                                                   V.COD_GRUPO,
                                                   V.DATA_VISITA,   
                                                   V.VENDEU,
                                                   V.MOTIVO_NAO_VENDA,
                                                   V.OBSERVACOES,
                                                   V.DATA_REAGENDAMENTO,
                                                   V.DATA_ATUALIZACAO,
                                                   V.USUARIO_ATUALIZACAO,
                                                   V.REAGENDAMENTO_REALIZADO,     
                                                   V.VALORBEM,     
                                                   V.DTAEXCLUSAO,
                                                   V.USUARIOEXCLUSAO,
                                                   V.MARCABEM,
                                                   V.MODELOBEM,
                                                   V.DATAINCLUSAO,
                                                   V.USUARIOINCLUSAO,
                                                   V.MOTIVOVISITA,
                                                   V.PERCEPCAO,                                                  
                                                   V.TIPOCONTATO,
                                                   vp.idpessoa,
                                                   vp.nomecompleto,
                                                   vp.tipopessoa,
                                                   vp.documento,
                                                   vp.dddfone,
                                                   vp.telefone,
                                                   vp.dddcelular,
                                                   vp.celular,
                                                   vp.whatsapp,
                                                   vp.datanascimento,
                                                   vp.email,
                                                   vp.cep,
                                                   vp.logradouro,
                                                   vp.uf,
                                                   vp.cidade,
                                                   vp.bairro,
                                                   vp.atividade,
                                                   vp.informacao,     
       
                                                    G.COD_GRUPO RESPONSAVEL,
                                                    GRUPOS.NOME,
                                                    GRUPOS.TIPO_GRUPO,
                                                    -- G.SEQREVENDA,
                                                    VORTICE.f_responsavel_cod(G.SEQREVENDA, 1, decode(USER,'CNMF',1,'CNV',2, 3)) COORDENADOR_RESPONSAVEL,
                                                    VORTICE.f_responsavel_cod(G.SEQREVENDA, 4, decode(USER,'CNMF',1,'CNV',2, 3)) SUPERVISOR_RESPONSAVEL,
                                                    VORTICE.f_responsavel_cod(G.SEQREVENDA, 11, decode(USER,'CNMF',1,'CNV',2, 3)) GERENTE_RESPONSAVEL,
                                                    VORTICE.f_responsavel_cod(G.SEQREVENDA, 6, decode(USER,'CNMF',1,'CNV',2, 3)) ADM_RESPONSAVEL,
                                                    TB.cod_grupo COD_GRUPOOUTRO
                           
                                      from CN_VISITAS V
                                       INNER JOIN   CN_VISITA_PESSOA VP ON V.IDPESSOA = VP.IDPESSOA
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
                                                                                                             ADM_RESPONSAVEL))VW
                             where CHECK_RESULT = :CODGRUPOLOGIN
                             AND DATA_VISITA = (SELECT MAX(VT.DATA_VISITA) FROM CN_VISITAS VT WHERE VT.IDPESSOA = VW.IDPESSOA)";


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
                        IdPessoa = Convert.ToInt32(m.IDPESSOA),

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

                }).OrderByDescending(a=>a.DataVisita).ToList();


                //AGRUPAMENTO

                //var result = response.GroupBy(a => a.Cliente.IdPessoa)                    
                //    .Select(m => new VisitaModel
                //    {
                //        Cliente = new PessoaModel
                //        {

                //            NomeCompleto = m.First().Cliente.NomeCompleto,
                //            DddCelular = m.First().Cliente.DDDFone,
                //            Celular = m.First().Cliente.Celular,
                //            Telefone = m.First().Cliente.Telefone,
                //            WhatsApp = m.First().Cliente.WhatsApp,
                //            Email = m.First().Cliente.Email,
                //            Atividade = m.First().Cliente.Atividade,
                //            IdPessoa = m.Key,
                //            Endereco = new EnderecoModel
                //            {

                //                Logradouro = m.First().Cliente.Endereco.Logradouro,
                //                UF = m.First().Cliente.Endereco.UF,
                //                Cidade = m.First().Cliente.Endereco.Cidade
                //            }
                //        },

                //        Id = m.Max(a => a.Id),
                //        CodGrupo = m.OrderByDescending(a => a.DataInclusao).First().CodGrupo,
                //        DataInclusao = m.Max(a => a.DataInclusao),
                //        DataVisita = m.Max(a => a.DataVisita),
                //        HistoricoVisita = m.OrderByDescending(a => a.DataInclusao).First().HistoricoVisita,
                //        NomeVendedor = m.OrderByDescending(a => a.DataInclusao).First().NomeVendedor,
                //        IsVendaRealizada = m.OrderByDescending(a => a.DataInclusao).First().IsVendaRealizada,
                //        MotivoNaoVenda = m.OrderByDescending(a => a.DataInclusao).First().MotivoNaoVenda,
                //        IdPessoa = m.Key,
                //        Agendamento = new AgendaModel
                //        {
                //            DataAgendamento = m.OrderByDescending(a => a.DataVisita).First().Agendamento.DataAgendamento,
                //        },
                //        Percepcao = m.OrderByDescending(a => a.DataInclusao).First().Percepcao,
                //        QtdVisita = m.Count(),
                //        MotivoVisita = m.OrderByDescending(a => a.DataInclusao).First().MotivoVisita,
                //        Usuario = m.OrderByDescending(a => a.DataInclusao).First().Usuario,
                //        BemViewModel = m.OrderByDescending(a => a.DataInclusao).First().BemViewModel,


                //    }).ToList().Where(a => a.Cliente.IdPessoa == 1703);

                return response;
            }

        }


        public static List<VisitaModel> Get(decimal codgrupo, string empresa, DateTime? dtaini, DateTime? dtafim)
        {
            //new LoginData().GetUser().Empresa
            using (var db = new OracleDataContext(ApiConsorcioNet.Conexao.ConnectionStrings.Acesso(empresa)))
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

            using (var db = new OracleDataContext(ApiConsorcioNet.Conexao.ConnectionStrings.Acesso(user.Empresa)))
            {
                try
                {


                    var visitas = db.CNVISITAs.Where(a => a.CODVISITA == model.Id && a.IDPESSOA == null);
                    foreach (var item in visitas)
                    {
                        item.IDPESSOA = model.Cliente.IdPessoa;
                        db.SubmitChanges();
                    }


                    //if (visita == null)
                    //{
                    var entity = new CNVISITA();
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
                    entity.IDPESSOA = model.Cliente.IdPessoa;
                    entity.TIPOCONTATO = model.TipoContato;
                    entity.VENDAQTD = model.QtdCotaVenda;
                    entity.VENDAVALOR = model.ValorVenda;
                    db.CNVISITAs.InsertOnSubmit(entity);

                    db.SubmitChanges();
                    //}
                    //else
                    //{

                    //    visita.VISITADO = model.Cliente.NomeCompleto;
                    //    visita.VENDEU = !string.IsNullOrEmpty(model.MotivoNaoVenda) ? "0" : "1";
                    //    visita.DATAREAGENDAMENTO = model.Agendamento.DataAgendamento;
                    //    visita.ENDERECO = model.Cliente.Endereco.Logradouro;
                    //    visita.CIDADE = model.Cliente.Endereco.Cidade;
                    //    visita.UF = model.Cliente.Endereco.UF;
                    //    visita.OBSERVACOES = model.HistoricoVisita;
                    //    visita.DDDFONE = model.Cliente.DDDFone;
                    //    visita.FONE = model.Cliente.Telefone;
                    //    visita.DDDCELULAR = model.Cliente.DddCelular;
                    //    visita.CELULAR = model.Cliente.Celular.OnlyNumbers();
                    //    visita.WHATSAPP = model.Cliente.WhatsApp.OnlyNumbers();
                    //    //visita.CODGRUPO = Convert.ToInt32(user.CodGrupo);
                    //    visita.VALORBEM = model.ValorBem;
                    //    visita.MOTIVONAOVENDA = string.IsNullOrEmpty(model.MotivoNaoVenda) ? "" : model.MotivoNaoVenda.ToUpper();
                    //    visita.EMAIL = model.Cliente.Email;
                    //    visita.DATAVISITA = model.DataVisita.GetValueOrDefault();
                    //    // visita.USUARIOATUALIZACAO = user.Login.Replace("%2E", ".");
                    //    visita.REAGENDAMENTOREALIZADO = model.Agendamento.DataAgendamento == null ? true : false;
                    //    visita.DATAATUALIZACAO = DateTime.Now.ToString("dd/MM/yyyy");
                    //    visita.MODELOBEM = model.BemViewModel.Modelo;
                    //    visita.MARCABEM = model.BemViewModel.Marca;
                    //    visita.ATIVIDADE = model.Cliente.Atividade;
                    //    visita.MOTIVOVISITA = model.MotivoVisita;
                    //    visita.PERCEPCAO = model.Percepcao;
                    //    db.SubmitChanges();
                    //}


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