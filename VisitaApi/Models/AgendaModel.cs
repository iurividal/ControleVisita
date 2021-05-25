using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace VisitaApi.Models
{
    public class AgendaModel
    {

        public decimal Id { get; set; }

        [DisplayName("Data Reagendamento")]
        public DateTime? DataAgendamento { get; set; }

        public string InformacaoAgendamento { get; set; }

        public bool IsConcluido { get; set; }

        public string Texto { get; set; }
    }
}