using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VisitaApi.Models
{
    public class SchedulerResourcesModelView
    {
        public List<AgendaAppointment> AgendaAppointments { get; set; }

        public List<AgendaResource> AgendaResources { get; set; }

    }
       
}