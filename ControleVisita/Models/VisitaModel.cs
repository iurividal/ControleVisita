using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.UI.WebControls;

namespace ControleVisita.Models
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


        [Required(ErrorMessage = "Informe o tipo contato")]
        [DisplayName("Forma de contato")]
        public string TipoContato { get; set; }


        [DataType(DataType.Currency)]
        [DisplayName("Valor de Bem (R$)")]
        public decimal? ValorBem { get; set; }


        [DisplayName("Valor de Bem (R$)")]
        public string ValorBemAux { get; set; }

        public bool IsVendaRealizada { get; set; }

        [DisplayName("Venda Realizada")]
        public string VendaRealizadaStr => IsVendaRealizada == true ? "SIM" : "NÃO";

        [DisplayName("Histórico")]
        public string HistoricoVisita { get; set; }

        [DisplayName("Por que não vendeu?")]
        public string MotivoNaoVenda { get; set; }

        public string Venda { get; set; }

        public decimal? ValorVenda { get; set; }

        public string ValorVendaStr { get;set; }

        public int QtdCotaVenda { get; set; }

        public int IdMotivo { get; set; }

        public AgendaModel Agendamento { get; set; } = new AgendaModel();

        [DisplayName("Usuário")]
        public string Usuario { get; set; }

        public string CodGrupo { get; set; }

        public BemViewModel BemViewModel { get; set; } = new BemViewModel();

        public DateTime DataInclusao { get; set; }

        [DisplayName("Percepção")]
        public string Percepcao { get; set; }

        public int IdPessoa { get; set; }

        public decimal QtdVisita { get; set; }
    }


}