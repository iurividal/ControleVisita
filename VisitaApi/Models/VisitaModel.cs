using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VisitaApi.Models
{
    public class VisitaModel
    {
        [DisplayName("Código")]
        public decimal Id { get; set; }

        public PessoaModel Cliente { get; set; } = new PessoaModel();

        [DisplayName("Responsável")]
        public string NomeVendedor { get; set; }

        [Required(ErrorMessage = "Informe a data da visita")]
        [DataType(DataType.Date)]
        [DisplayName("Data visita")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DataVisita { get; set; }

        public DateTime? DataVisitaFinal { get; set; }

        //public EnderecoModel Endereco { get; set; } = new EnderecoModel();

        [Required(ErrorMessage = "Informe o motivo da visita")]
        [DisplayName("Motivo da Visita")]
        public string MotivoVisita { get; set; }


        [DataType(DataType.Currency)]
        [DisplayName("Valor de Bem (R$)")]
        public decimal? ValorBem { get; set; }


        [DisplayName("Valor de Bem (R$)")]
        public string ValorBemAux { get; set; }

        public bool IsVendaRealizada { get; set; }

        [DisplayName("Venda Realizada")]
        public string VendaRealizadaStr => IsVendaRealizada == true ? "SIM" : "NÃO";

        [DisplayName("Histórico da Visita")]
        public string HistoricoVisita { get; set; }

        [DisplayName("Por que não vendeu?")]
        public string MotivoNaoVenda { get; set; }

        public int IdMotivo { get; set; }

        public AgendaModel Agendamento { get; set; } = new AgendaModel();

        [DisplayName("Usuário")]
        public string Usuario { get; set; }

        public string CodGrupo { get; set; }

        public BemModel BemModel { get; set; } = new BemModel();

        public DateTime DataInclusao { get; set; }

        [DisplayName("Percepção")]
        public string Percepcao { get; set; }

        public int IdPessoa { get; set; }
    }
}