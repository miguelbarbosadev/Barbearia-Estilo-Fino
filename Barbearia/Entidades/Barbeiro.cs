using System.Collections.Generic;

namespace Barbearia.Entidades
{
    /// <summary>Representa um barbeiro da barbearia.</summary>
    public class Barbeiro
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Especialidades { get; set; }
        public string HorarioTrabalho { get; set; }

        public virtual ICollection<Agendamento> Agendamentos { get; set; }

        public Barbeiro()
        {
            Agendamentos = new List<Agendamento>();
        }
    }
}
