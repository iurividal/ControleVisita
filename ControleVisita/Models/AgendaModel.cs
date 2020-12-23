using System;
using System.ComponentModel;

namespace ControleVisita.Models
{
    public class AgendaModel
    {
        [DisplayName("Data Reagendamento")]
        public DateTime? DataAgendamento { get; set; }

        public string InformacaoAgendamento { get; set; }

        public bool IsConcluido { get; set; }
    }
}