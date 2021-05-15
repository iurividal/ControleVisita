using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ControleVisita.Models
{
    public class EnderecoModel
    {

        [DisplayName("Cep")]
        public string Cep { get; set; }

      //  [Required(ErrorMessage = "Informe o Estado")]
        [DisplayName("UF")]
        public string UF { get; set; }

        //[Required(ErrorMessage = "Informe a Cidade")]
        [DisplayName("Cidade")]
        public string Cidade { get; set; }

        [DisplayName("Logradouro")]
        public string Logradouro { get; set; }

        public string Bairro { get; set; }
    }
}