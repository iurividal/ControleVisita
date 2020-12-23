using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControleVisita.Models
{
    public class CidadeViewModelModel
    {
        public decimal IdCidade { get; set; }
        public string Cidade { get; set; }

        public int IDEstado { get; set; }
    }

    public class EstadoViewModel
    {
        public decimal IdEstado { get; set; }

        public string Estado { get; set; }

        public string Sigla { get; set; }



    }
}