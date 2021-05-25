using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VisitaApi.Models
{
    public class FiltroModelView
    {
        public DateTime? DataVisitaInicial { get; set; }

        public DateTime? DataVisitaFinal { get; set; }

        public DateTime? DataReagentamentoInicial { get; set; }

        public DateTime? DataReagentamentoFinal { get; set; }

        public string Vendedor { get; set; }

        public string MotivoVisita { get; set; }

        public bool isVendaRealizada { get; set; }
    }
}