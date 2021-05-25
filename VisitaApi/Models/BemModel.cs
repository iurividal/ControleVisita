using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace VisitaApi.Models
{
    public class BemModel
    {
        public string CodBem { get; set; }

        [DisplayName("Marca")]
        public string Marca { get; set; }

        [DisplayName("Modelo")]
        public string Modelo { get; set; }

        public decimal? ValorBem { get; set; }

        public string ValorBemAux { get; set; }
    }
}