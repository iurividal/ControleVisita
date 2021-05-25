using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ApiConsorcioNet.Extensoes;
using ControleVisita.Models.Enum;

namespace ControleVisita.Models
{

    public class PessoaModelList : List<PessoaModel> { }
    public class PessoaModel
    {

        [DisplayName("Id")]
        public int IdPessoa { get; set; }

        [MinLength(10, ErrorMessage = "Informe o nome completo")]
        [Required(ErrorMessage = "Informe o nome razão completo do cliente")]
        [DisplayName("Nome Razão")]
        public string NomeCompleto { get; set; }

        [DisplayName("Sobrenome")]
        public string Sobrenome { get; set; }

        [DisplayName("Tipo de Pessoa")]
        public string TipoPessoa { get; set; }

        [DisplayName("CPF ou CNPJ")]
        public string Documento { get; set; }

        [DisplayName("Telefone")]
        public string Telefone { get; set; }

        [DisplayName("DDD Tel.")]
        public string DDDFone { get; set; }


        [DisplayName("DDD Cel.")]
        public string DddCelular { get; set; }


        [DisplayName("Celular")]
        public string Celular { get; set; }

        [DisplayName("WhatsApp")]
        public string WhatsApp { get; set; }

        [DisplayName("Nº Celular.")]
        public string NroCelularCompleto => DddCelular + "" + Celular;

        [DisplayName("Data de Nasc.")]
        public DateTime? DataNascimento { get; set; }

        [Required(ErrorMessage = "O E-mail campo é obrigatório")]
        [DataType(DataType.EmailAddress, ErrorMessage = "informe um e-mail valído")]
        [EmailAddress(ErrorMessage = "e-email é inválido")]
        public string Email { get; set; }

        public EnderecoModel Endereco { get; set; } = new EnderecoModel();

        [DisplayName("Atividade")]
        public string Atividade { get; set; }

        [DisplayName("Informações")]
        public string Informacao { get; set; }

        public string Grupo { get; set; }// NomeCompleto.ToUpper().Substring(0, 1);
    }
}