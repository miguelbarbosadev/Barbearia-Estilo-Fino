using System.Collections.Generic;

namespace Barbearia.Entidades
{
    /// <summary>Representa um cliente da barbearia.</summary>
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Agendamento> Agendamentos { get; set; }

        public Cliente()
        {
            Agendamentos = new List<Agendamento>();
        }
    }
}
