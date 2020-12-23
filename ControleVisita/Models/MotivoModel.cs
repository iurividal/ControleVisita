namespace ControleVisita.Models
{
    public class MotivoModel
    {
        public int IdMotivo { get; set; }
        public string Motivo { get; set; }
        public bool IsObrigaObservacao { get; set; }
        public bool IsObrigaReagendamento { get; set; }
        public int Qtd { get; set; }
    }
}