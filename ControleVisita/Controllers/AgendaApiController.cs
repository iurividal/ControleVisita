using ControleVisita.Models;
using ControleVisita.Models.IdentityExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        public HttpResponseMessage Get(decimal codgrupo, string empresa, DataSourceLoadOptions loadOptions)
        {
            FiltroModelView filtroModel = new FiltroModelView
            {
                DataReagentamentoInicial = DateTime.Now.Date,
                DataReagentamentoFinal =  DateTime.Now.AddMonths(3)


            };


            var response = VisitaViewModel
               .GetVisitas(codgrupo, empresa, filtroModel)
               .OrderByDescending(a => a.Agendamento.DataAgendamento).ToList();

            List<Appointment> agendaModels = new List<Appointment>();

            response.ForEach(item =>
            {
                agendaModels.Add(new Appointment
                {
                    StartDate = item.Agendamento.DataAgendamento.Value.AddHours(8).ToUniversalTime().ToString("o"),
                    EndDate = item.Agendamento.DataAgendamento.Value.AddHours(18).ToUniversalTime().ToString("o"),
                    Text = $"{item.Cliente.NomeCompleto}",
                    AppointmentId = Convert.ToInt32(item.Id),
                    AllDay = false,
                    Description = item.HistoricoVisita

                });
            });

            return Request.CreateResponse(DataSourceLoader.Load(agendaModels, loadOptions));
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