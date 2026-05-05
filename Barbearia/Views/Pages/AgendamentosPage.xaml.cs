using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Barbearia.Data;
using Barbearia.Entidades;
using Barbearia.Services;

namespace Barbearia.Views.Pages
{
    public partial class AgendamentosPage : Page
    {
        private readonly AgendamentoService _service;
        private List<Agendamento> _todos = new List<Agendamento>();

        public AgendamentosPage()
        {
            InitializeComponent();
            _service = new AgendamentoService(new BarbeariaContext());
            FiltroDataInicio.SelectedDate = DateTime.Today.AddDays(-30);
            FiltroDataFim.SelectedDate    = DateTime.Today.AddDays(30);
            Loaded += (s, e) => Carregar();
        }

        private void Carregar()
        {
            _todos = _service.ListarTodos();
            AplicarFiltro();
        }

        private void AplicarFiltro()
        {
            IEnumerable<Agendamento> lista = _todos;

            if (FiltroDataInicio.SelectedDate.HasValue)
                lista = lista.Where(a => a.DataHora.Date >= FiltroDataInicio.SelectedDate.Value);

            if (FiltroDataFim.SelectedDate.HasValue)
                lista = lista.Where(a => a.DataHora.Date <= FiltroDataFim.SelectedDate.Value);

            var statusItem = FiltroStatus.SelectedItem as ComboBoxItem;
            var statusStr  = statusItem != null ? statusItem.Content.ToString() : "Todos";
            StatusAgendamento status;
            if (statusStr != "Todos" && Enum.TryParse(statusStr, out status))
                lista = lista.Where(a => a.Status == status);

            GridAgendamentos.ItemsSource = lista.OrderByDescending(a => a.DataHora).ToList();
        }

        private void BtnFiltrar_Click(object sender, RoutedEventArgs e) { AplicarFiltro(); }

        private void BtnNovo_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new AgendamentoDialog();
            if (dlg.ShowDialog() == true) Carregar();
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            var ag = GridAgendamentos.SelectedItem as Agendamento;
            if (ag == null) return;
            var dlg = new AgendamentoDialog(ag);
            if (dlg.ShowDialog() == true) Carregar();
        }

        private void BtnConcluir_Click(object sender, RoutedEventArgs e)
        {
            var ag = GridAgendamentos.SelectedItem as Agendamento;
            if (ag == null) return;
            _service.AlterarStatus(ag.Id, StatusAgendamento.Concluido);
            Carregar();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            var ag = GridAgendamentos.SelectedItem as Agendamento;
            if (ag == null) return;
            _service.AlterarStatus(ag.Id, StatusAgendamento.Cancelado);
            Carregar();
        }

        private void BtnExcluir_Click(object sender, RoutedEventArgs e)
        {
            var ag = GridAgendamentos.SelectedItem as Agendamento;
            if (ag == null) return;
            if (MessageBox.Show("Excluir este agendamento?", "Confirmar",
                MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;
            _service.Excluir(ag.Id);
            Carregar();
        }

        private void GridAgendamentos_SelectionChanged(object sender, SelectionChangedEventArgs e) { }

        private void FiltroStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
