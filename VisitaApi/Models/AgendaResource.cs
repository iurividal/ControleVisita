using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VisitaApi.Models
{
    public class AgendaResource
    {
        public int IdPercepcao { get; set; }

        public string Texto { get; set; }

        public string Color { get; set; }

        public static readonly IEnumerable<AgendaResource> AgendaResources = new[]{
            new AgendaResource
            {
                IdPercepcao = 1,
                Texto = "Frio",
                Color = "#33B2FF"
            },
            new AgendaResource
            {
                IdPercepcao = 2,
                Texto = "Morno",
                Color = "#FFBB33"
            },
            new AgendaResource
            {
                IdPercepcao = 3,
                Texto = "Quente",
                Color = "#FF3F33"
            }

            };

    }
}