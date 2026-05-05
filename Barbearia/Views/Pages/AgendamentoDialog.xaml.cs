using System;
using System.Windows;
using Barbearia.Data;
using Barbearia.Entidades;
using Barbearia.Services;

namespace Barbearia.Views.Pages
{
    public partial class AgendamentoDialog : Window
    {
        private readonly AgendamentoService _agService;
        private readonly ClienteService     _cliService;
        private readonly BarbeiroService    _barService;
        private readonly ServicoService     _srvService;
        private readonly Agendamento        _editando;

        public AgendamentoDialog(Agendamento agendamento = null)
        {
            InitializeComponent();
            var ctx     = new BarbeariaContext();
            _agService  = new AgendamentoService(ctx);
            _cliService = new ClienteService(ctx);
            _barService = new BarbeiroService(ctx);
            _srvService = new ServicoService(ctx);
            _editando   = agendamento;

            if (agendamento != null)
                TxtTitulo.Text = "Editar Agendamento";

            Loaded += (s, e) => CarregarCombos();
        }

        private void CarregarCombos()
        {
            CbCliente.ItemsSource  = _cliService.ListarTodos();
            CbBarbeiro.ItemsSource = _barService.ListarTodos();
            CbServico.ItemsSource  = _srvService.ListarTodos();

            if (_editando != null)
            {
                foreach (Cliente c in CbCliente.Items)
                    if (c.Id == _editando.ClienteId) { CbCliente.SelectedItem = c; break; }

                foreach (Barbeiro b in CbBarbeiro.Items)
                    if (b.Id == _editando.BarbeiroId) { CbBarbeiro.SelectedItem = b; break; }

                foreach (Servico s in CbServico.Items)
                    if (s.Id == _editando.ServicoId) { CbServico.SelectedItem = s; break; }

                DpData.SelectedDate = _editando.DataHora.Date;
                TxtHorario.Text     = _editando.DataHora.ToString("HH:mm");
                TxtObs.Text         = _editando.Observacoes;
            }
            else
            {
                DpData.SelectedDate = DateTime.Today;
                TxtHorario.Text     = "09:00";
            }
        }

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TimeSpan hora;
                if (!TimeSpan.TryParse(TxtHorario.Text, out hora))
                    throw new ArgumentException("Horário inválido. Use o formato HH:mm.");

                if (DpData.SelectedDate == null)
                    throw new ArgumentException("Selecione uma data.");

                var dataHora = DpData.SelectedDate.Value.Add(hora);

                var ag = _editando ?? new Agendamento();

                var cliente  = CbCliente.SelectedItem  as Cliente;
                var barbeiro = CbBarbeiro.SelectedItem as Barbeiro;
                var servico  = CbServico.SelectedItem  as Servico;

                if (cliente  == null) throw new ArgumentException("Selecione um cliente.");
                if (barbeiro == null) throw new ArgumentException("Selecione um barbeiro.");
                if (servico  == null) throw new ArgumentException("Selecione um serviço.");

                ag.ClienteId  = cliente.Id;
                ag.BarbeiroId = barbeiro.Id;
                ag.ServicoId  = servico.Id;
                ag.DataHora   = dataHora;
                ag.Observacoes = TxtObs.Text;

                if (ag.Id == 0) ag.Status = StatusAgendamento.Agendado;

                _agService.Salvar(ag);
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
