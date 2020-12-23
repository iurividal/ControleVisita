using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ControleVisita.Models.Enum;

namespace ControleVisita.Models
{

    public class PessoaModelList : List<PessoaModel> { }
    public class PessoaModel
    {
        [Key]
        [Display(AutoGenerateField = true)]
        [ReadOnly(true)]
        [DisplayName("Id")]
        public int IdPessoa { get; set; }

        [Required(ErrorMessage = "Informe o nome do cliente")]
        [DisplayName("Nome")]
        public string NomeCompleto { get; set; }

        [DisplayName("Tipo de Pessoa")]
        public TipoPessoa TipoPessoa { get; set; }

        [DisplayName("CPF ou CNPJ")]
        public string Documento { get; set; }


        private string _tefoneFields = "";

        [DisplayName("Telefone")]
        public string Telefone { get => _tefoneFields; set => _tefoneFields = value; }


        private string _dddFoneFields = "";
        [DisplayName("DDD Tel.")]
        public string DDDFone { get => _dddFoneFields; set => _dddFoneFields = value; }

        [Required(ErrorMessage = "Informe o ddd celular")]
        [DisplayName("DDD Cel.")]
        public string DddCelular { get; set; } = "";

        [Required(ErrorMessage = "Informe o número do celular")]
        [DisplayName("Celular")]
        public string Celular { get; set; } = "";

        [DisplayName("WhatsApp")]
        public string WhatsApp { get; set; }

        [DisplayName("Nº Celular.")]
        public string NroCelularCompleto => DddCelular + "" + Celular;

        [DisplayName("Data de Nasc.")]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "O E-mail campo é obrigatório")]
        [DataType(DataType.EmailAddress, ErrorMessage = "informe um e-mail valído")]
        [EmailAddress(ErrorMessage = "e-email é inválido")]
        public string Email { get; set; }

        public EnderecoModel Endereco { get; set; } = new EnderecoModel();

        [DisplayName("Atividade")]
        public string Atividade { get; set; }

        [DisplayName("Informações")]
        public string Informacao { get; set; }
    }
}