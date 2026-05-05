using System.Linq;
using System.Windows;
using Barbearia.Data;
using Barbearia.Services;

namespace Barbearia.Views.Pages
{
    public partial class HistoricoClienteDialog : Window
    {
        public HistoricoClienteDialog(int clienteId, string nomeCliente)
        {
            InitializeComponent();
            Title          = "Histórico – " + nomeCliente;
            TxtTitulo.Text = "📋 Histórico de " + nomeCliente;

            Loaded += (s, e) =>
            {
                var service = new AgendamentoService(new BarbeariaContext());
                var todos   = service.ListarTodos();
                GridHistorico.ItemsSource = todos
                    .Where(a => a.ClienteId == clienteId)
                    .OrderByDescending(a => a.DataHora)
                    .ToList();
            };
        }
    }
}
