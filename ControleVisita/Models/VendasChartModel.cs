using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControleVisita.Models
{
    public class VendasChartModel
    {
        public static IEnumerable<VendasChartViewModel> GetVendaPeriodo(decimal codgrupo,string empresa, DateTime datainicial, DateTime datafinal)
        {
            var response = VisitaViewModel.Get(codgrupo,empresa,datainicial, datafinal);

            return response.Where(a => a.IsVendaRealizada)
                    .GroupBy(a => a.DataVisita)
                    .Select(v => new VendasChartViewModel
                    {
                        Mes = v.Key.Value.ToString("MMM/yyyy"),
                        Qtd = v.Count()
                    }).ToList();
        }

        public static IEnumerable<VendasChartViewModel> GetVendaPeriodoDiaria(decimal codgrupo,string empresa, DateTime datainicial, DateTime datafinal)
        {
            var response = VisitaViewModel.Get(codgrupo,empresa, datainicial, datafinal);

            return response.Where(a => a.IsVendaRealizada)
                    .GroupBy(a => a.DataVisita)
                    .Select(v => new VendasChartViewModel
                    {
                        Mes = v.Key.Value.ToString("dd/MM/yyyy"),
                        Qtd = v.Count()
                    }).ToList();
        }
    }
}