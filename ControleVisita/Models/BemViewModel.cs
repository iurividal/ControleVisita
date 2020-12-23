using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ControleVisita.Models
{
    public class BemViewModel
    {
        [DisplayName("Marca")]
        public string Marca { get; set; }

        [DisplayName("Modelo")]
        public string Modelo { get; set; }

        public decimal? ValorBem { get; set; }

        public string ValorBemAux { get; set; }


    }
}