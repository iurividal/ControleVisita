using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ControleVisita.Models
{
    public class BemViewModel
    {
        public string CodBem { get; set; }


        [DisplayName("Marca")]
        public string Marca { get; set; }

        [DisplayName("Modelo")]
        public string Modelo { get; set; }

        public decimal? ValorBem { get; set; }

        public string BemModeloDisplayMember => "Marca/Modelo: " + (!string.IsNullOrEmpty(Marca) ? $"{Marca} " : "Não informada ")+"/" + (!string.IsNullOrEmpty(Modelo) ? $"{Modelo}" : "Não informado");

        public string ValorBemAux { get; set; }



        public static IEnumerable<BemViewModel> GetModelo(string empresa)
        {
            using (var db = new OracleContext.OracleDataContext(ApiConsorcioNet.Conexao.ConnectionStrings.Acesso(empresa)))
            {
                var response = db.GCBEMs.Where(a => a.FORALINHA == "N").ToList();


                return response.Select(x => new BemViewModel
                {
                    Marca = x.MARCA,
                    Modelo = x.DESCR,
                    CodBem = x.CODBEM
                });
            }
        }
    }
}