using System.Collections.Generic;

namespace ControleVisita.Models
{
    public class MotivoViewModel
    {

        public static IEnumerable<MotivoModel> GetAll()
        {

            IEnumerable<MotivoModel> M = new List<MotivoModel>
            {
                new MotivoModel{IdMotivo = 0,Motivo = "",IsObrigaObservacao = false,IsObrigaReagendamento = false},
                new MotivoModel{IdMotivo = 1,Motivo = "SEM INTERESSE",IsObrigaObservacao = false,IsObrigaReagendamento = false},
                new MotivoModel{IdMotivo = 2,Motivo = "NÃO ATENDEU",IsObrigaObservacao = true,IsObrigaReagendamento = true},
                new MotivoModel{IdMotivo = 3,Motivo = "JÁ TEM COTA DE OUTRA ADMINISTRADORA",IsObrigaObservacao = true,IsObrigaReagendamento = false},
                new MotivoModel{IdMotivo = 4,Motivo = "SOLICITOU CONTATO FUTURO",IsObrigaObservacao = false,IsObrigaReagendamento = true},
                new MotivoModel{IdMotivo = 5,Motivo = "AVALIANDO PROPOSTA",IsObrigaObservacao = false,IsObrigaReagendamento = true},
                //new MotivoModel{IdMotivo = 6,Motivo = "COMPARANDO COM A CONCORRÊNCIA",IsObrigaObservacao = false,IsObrigaReagendamento = true},
                new MotivoModel{IdMotivo = 7,Motivo = "OUTROS",IsObrigaObservacao = true,IsObrigaReagendamento = false},
            };

            return M;

        }

        public static IEnumerable<MotivoModel> GetMotivoVisita()
        {
            IEnumerable<MotivoModel> M = new List<MotivoModel>
            {
                new MotivoModel{IdMotivo = 1,Motivo = "VENDA",IsObrigaObservacao = false,IsObrigaReagendamento = false},
                new MotivoModel{IdMotivo = 2,Motivo = "COBRANÇA/JURÍDICO",IsObrigaObservacao = false,IsObrigaReagendamento = false},
                new MotivoModel{IdMotivo = 2,Motivo = "COLETA DOC. / RESOLUÇÃO PENDÊNCIAS",IsObrigaObservacao = false,IsObrigaReagendamento = false}
            };

            return M;

        }

    }
}