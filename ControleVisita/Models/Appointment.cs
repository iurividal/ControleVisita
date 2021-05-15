using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControleVisita.Models
{



    public class SchedulerResourcesModelView
    {
        public List<AgendaAppointment> AgendaAppointments { get; set; }

        public List<AgendaResource> AgendaResources { get; set; }
    }

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



    public class AgendaAppointment : Appointment
    {
        public int IdPercepcao { get; set; }
    }


    public class Appointment
    {
        public int AppointmentId { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public bool AllDay { get; set; }
        public string RecurrenceRule { get; set; }
        public string RecurrenceException { get; set; }


    }


    public class DisableDatesAppointment
    {
        public int AppointmentId { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool AllDay { get; set; }
        public string RecurrenceRule { get; set; }
        public string RecurrenceException { get; set; }
    }

    public class AdaptiveAppointmentsResource
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public string Color { get; set; }

        public static readonly IEnumerable<AdaptiveAppointmentsResource> AdaptiveAppointmentsResources = new[] {

            new AdaptiveAppointmentsResource {
                Id = 2,
                Text = "NÃO ATENDEU",
                Color = "#bbd806"
            },
            new AdaptiveAppointmentsResource
            {
                Id = 3,
                Text = "JÁ TEM COTA DE OUTRA ADMINISTRADORA",
                Color = "#bbd806"
            }
        };
    }
}