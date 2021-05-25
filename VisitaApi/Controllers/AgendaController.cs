using DevExtreme.AspNet.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VisitaApi.Models;

namespace VisitaApi.Controllers
{
    public class AgendaController : ApiController
    {
        public HttpResponseMessage Get(decimal codgrupo, string empresa, DataSourceLoadOptions loadOptions)
        {
            FiltroModelView filtroModel = new FiltroModelView
            {
                DataReagentamentoInicial = DateTime.Now.Date.AddDays(-20),
                DataReagentamentoFinal = DateTime.Now.AddMonths(2)


            };


            var response = VisitaViewModel
               .GetVisitas(codgrupo, empresa, filtroModel)
               .OrderByDescending(a => a.Agendamento.DataAgendamento).ToList();



            List<AgendaAppointment> agendaAppointments = new List<AgendaAppointment>();
            response.ForEach(item =>
            {
                agendaAppointments.Add(new AgendaAppointment
                {
                    StartDate = item.Agendamento.DataAgendamento.Value.AddHours(8).ToUniversalTime().ToString("o"),
                    EndDate = item.Agendamento.DataAgendamento.Value.AddHours(18).ToUniversalTime().ToString("o"),
                    Text = $"{item.Cliente.NomeCompleto}",
                    AppointmentId = Convert.ToInt32(item.Id),
                    AllDay = false,
                    Description = item.HistoricoVisita,
                    IdPercepcao = string.IsNullOrEmpty(item.Percepcao) || item.Percepcao == "Frio" ? 1 :
                        item.Percepcao == "Morno" ? 2 : 3

                });
            });

            List<SchedulerResourcesModelView> SchedulerResourcesList = new List<SchedulerResourcesModelView>();

            SchedulerResourcesList.Add(new SchedulerResourcesModelView
            {
                AgendaAppointments = agendaAppointments,
                AgendaResources = AgendaResource.AgendaResources.ToList()
            });

            return Request.CreateResponse(DataSourceLoader.Load(agendaAppointments, loadOptions));
        }

    }
}