using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OracleContext;

namespace ControleVisita.Models
{
    public class ProfissaoModel
    {


        public static IEnumerable<ProfissaoViewModel> GetProfissao()
        {
            using (var db = new OracleDataContext(ApiConsorcioNet.Conexao.ConnectionStrings.Acesso("cmglobal")))
            {

                return db.CNPROFISSAOs.Select(p => new ProfissaoViewModel {Id = p.ID, Profissao = p.PROFISSAO})
                    .ToList();


            }
        }

    }
}