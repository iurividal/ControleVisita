using ControleVisita.Models;
using ControleVisita.Models.IdentityExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ControleVisita.Controllers
{
    public class AgendaApiController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public async Task<HttpResponseMessage> Get(decimal codgrupo, string empresa, DataSourceLoadOptions loadOptions)
        {
            FiltroModelView filtroModel = new FiltroModelView
            {
                DataReagentamentoInicial = DateTime.Now.Date.AddDays(-20),
                DataReagentamentoFinal = DateTime.Now.AddMonths(2)


            };


            var response = await Task.FromResult(VisitaViewModel.GetVisitas(codgrupo, empresa, filtroModel)
                                     .OrderByDescending(a => a.Agendamento.DataAgendamento).ToList());



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

        [HttpGet]
        [Route("api/agendaResources")]
        public HttpResponseMessage GetResoucer(DataSourceLoadOptions loadOptions)
        {
            return Request.CreateResponse(DataSourceLoader.Load(AgendaResource.AgendaResources.ToList(), loadOptions));
        }

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}