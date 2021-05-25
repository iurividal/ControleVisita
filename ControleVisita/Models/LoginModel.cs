using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.UI.WebControls;

namespace ControleVisita.Models
{
    public class LoginModel
    {
        [DisplayName("Usuário")]
        public string Login { get; set; }

        public string Usuario => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Login.Replace('.', ' '));

        
        [DisplayName("Senha")]
        public string Password { get; set; }

        public string CodLogin { get; set; }

        public string CodGrupo { get; set; }

        public string TipoGrupo { get; set; }

        public string Empresa { get; set; }

        public string SubEmpresaPermissao { get; set; }
    }
}