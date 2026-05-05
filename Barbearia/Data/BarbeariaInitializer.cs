using System.Data.Entity;
using Barbearia.Entidades;

namespace Barbearia.Data
{
    /// <summary>
    /// Inicializa o banco de dados e insere dados de exemplo (seed).
    /// Estratégia: cria o banco se não existir; se existir, não recria.
    /// </summary>
    public class BarbeariaInitializer
        : CreateDatabaseIfNotExists<BarbeariaContext>
    {
        protected override void Seed(BarbeariaContext context)
        {
            // Serviços iniciais
            context.Servicos.Add(new Servico { Nome = "Corte de Cabelo",  Preco = 35m,  DuracaoMinutos = 30 });
            context.Servicos.Add(new Servico { Nome = "Barba",            Preco = 25m,  DuracaoMinutos = 20 });
            context.Servicos.Add(new Servico { Nome = "Corte + Barba",    Preco = 55m,  DuracaoMinutos = 50 });
            context.Servicos.Add(new Servico { Nome = "Hidratação",       Preco = 45m,  DuracaoMinutos = 40 });

            context.SaveChanges();
            base.Seed(context);
        }
    }
}
