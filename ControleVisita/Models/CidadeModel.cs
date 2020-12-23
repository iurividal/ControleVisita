using DevExtreme.AspNet.Data;
using OracleContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControleVisita.Models
{
    public class CidadeModel
    {
        public static IEnumerable<EstadoViewModel> GetEstado()
        {
            using (var db = new OracleDataContext())
            {
                var respose = db.ESTADOs.ToList();
                return respose.Select(c => new EstadoViewModel
                {
                    Estado = c.NOME,
                    IdEstado = c.ID,
                    Sigla = c.UF
                }).ToList();
            }
        }

        public static IEnumerable<CidadeViewModelModel> GetCidade(int idEstado)
        {
            using (var db = new OracleDataContext())
            {
                var response = db.CIDADEs.Where(a => a.ESTADO == idEstado);

                return response.Select(c => new CidadeViewModelModel
                {
                    Cidade = c.NOME,
                    IdCidade = c.ID

                }).ToList();
            }
        }

        public static SelectList ToSectList(IEnumerable<EstadoViewModel> estado)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (var row in estado)
            {
                list.Add(new SelectListItem()
                {
                    Text = row.Estado,
                    Value = row.IdEstado.ToString()
                });
            }

            return new SelectList(list, "Value", "Text");
        }
    }
}