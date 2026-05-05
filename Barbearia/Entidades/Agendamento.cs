using System;

namespace Barbearia.Entidades
{
    public enum StatusAgendamento
    {
        Agendado,
        Concluido,
        Cancelado
    }

    /// <summary>Representa um agendamento na barbearia.</summary>
    public class Agendamento
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }
        public virtual Cliente Cliente { get; set; }

        public int BarbeiroId { get; set; }
        public virtual Barbeiro Barbeiro { get; set; }

        public int ServicoId { get; set; }
        public virtual Servico Servico { get; set; }

        public DateTime DataHora { get; set; }
        public StatusAgendamento Status { get; set; }
        public string Observacoes { get; set; }
    }
}
