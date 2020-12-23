using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControleVisita.Models
{
    public class MarcaViewModel
    {
        public string Marca { get; set; }
        public string Empresa { get; set; }


        public MarcaViewModel(string marca, string empresa)
        {
            this.Marca = marca;
            this.Empresa = empresa;
        }

        public static IEnumerable<MarcaViewModel> GetMarca()
        {

            IEnumerable<MarcaViewModel> marcas = new List<MarcaViewModel>
            {
              
                new MarcaViewModel("MASSEY FERGUSON","CNMF"),
                new MarcaViewModel("VALTRA","CNV"),
                new MarcaViewModel("FIAT","CMC"),
                new MarcaViewModel("HARLEY DAVIDSON","CMC"),
                new MarcaViewModel("KTM","CMC"),
                new MarcaViewModel("KIA","CMC"),
                new MarcaViewModel("CITROEN","CMC"),
                new MarcaViewModel("VOLKSWAGEN","CMC"),
                new MarcaViewModel("LAND ROVER","CMC"),
                new MarcaViewModel("TOYOTA","CMC"),
                new MarcaViewModel("DAF","CMC"),
                new MarcaViewModel("MAN","CMC"),
                new MarcaViewModel("VOLVO","CMC"),

            };

            return marcas;

        }
    }
}