using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oracle.ManagedDataAccess.Client;
using Dapper;

namespace ControleVisita.Models
{
    public class VendedorModel
    {
        public string CodGrupo { get; set; }

        public string Nome { get; set; }


        public static IEnumerable<VendedorModel> GetVendedor(string empresa, string codgrupoLogin)
        {
            using (var db = new OracleConnection(ApiConsorcioNet.Conexao.ConnectionStrings.AcessoOracleODP(empresa)))
            {
                var query = @"SELECT DISTINCT NOME,COD_GRUPO AS CODGRUPO FROM (
                                select 
                                       G.COD_GRUPO RESPONSAVEL,     
                                       G.SIT_GRUPO,       
                                       DECODE(G.COD_GRUPO,:CODGRUPOLOGIN, '#'||G.NOME,G.NOME)NOME,
                                        VORTICE.f_responsavel_cod(R.SEQREVENDA, 1, 3) COORDENADOR_RESPONSAVEL,
                                                            VORTICE.f_responsavel_cod(R.SEQREVENDA, 4, 3) SUPERVISOR_RESPONSAVEL,
                                                            VORTICE.f_responsavel_cod(R.SEQREVENDA, 11, 3) GERENTE_RESPONSAVEL,
                                                            VORTICE.f_responsavel_cod(R.SEQREVENDA, 6, 3) ADM_RESPONSAVEL,
                                       R.COD_GRUPO
                                  from CN_GRUPOS G
                                 inner join CN_GRUPOS_GEPESSOA R
                                    on G.COD_GRUPO = R.COD_GRUPO
                                       and G.TIPO_GRUPO = R.TIPO_GRUPO
                                 where R.SEQREVENDA in (select R2.SEQREVENDA from CN_GRUPOS_GEPESSOA R2 where R2.COD_GRUPO = :CODGRUPOLOGIN)
                                 group by G.NOME,
                                          G.COD_GRUPO,
                                          G.TIPO_GRUPO,
                                          G.SIT_GRUPO,
                                           R.COD_GRUPO,
                                          R.SEQREVENDA) UNPIVOT(CHECK_RESULT for COD_GRUPOS in(RESPONSAVEL, COORDENADOR_RESPONSAVEL,
                                                                                                                     SUPERVISOR_RESPONSAVEL,
                                                                                                                     GERENTE_RESPONSAVEL,
                                                                                                                     ADM_RESPONSAVEL))
                                                                                                                     WHERE CHECK_RESULT= :CODGRUPOLOGIN";

                return db.Query<VendedorModel>(query, new { CODGRUPOLOGIN = codgrupoLogin }).OrderBy(a=>a.Nome);

            }
        }
    }
}