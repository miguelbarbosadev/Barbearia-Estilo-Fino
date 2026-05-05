using System;
using System.Windows;
using System.Windows.Controls;
using Barbearia.Data;
using Barbearia.Entidades;
using Barbearia.Services;

namespace Barbearia.Views.Pages
{
    public partial class ClientesPage : Page
    {
        private readonly ClienteService _service;

        public ClientesPage()
        {
            InitializeComponent();
            _service = new ClienteService(new BarbeariaContext());
            Loaded  += (s, e) => Carregar();
        }

        private void Carregar()
        {
            GridClientes.ItemsSource = _service.ListarTodos();
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtBusca.Text))
                Carregar();
            else
                GridClientes.ItemsSource = _service.BuscarPorNome(TxtBusca.Text);
        }

        private void BtnNovo_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new ClienteDialog();
            if (dlg.ShowDialog() == true) Carregar();
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            var c = GridClientes.SelectedItem as Cliente;
            if (c == null) return;
            var dlg = new ClienteDialog(c);
            if (dlg.ShowDialog() == true) Carregar();
        }

        private void BtnHistorico_Click(object sender, RoutedEventArgs e)
        {
            var c = GridClientes.SelectedItem as Cliente;
            if (c == null) return;
            var dlg = new HistoricoClienteDialog(c.Id, c.Nome);
            dlg.ShowDialog();
        }

        private void BtnExcluir_Click(object sender, RoutedEventArgs e)
        {
            var c = GridClientes.SelectedItem as Cliente;
            if (c == null) return;
            if (MessageBox.Show("Excluir cliente '" + c.Nome + "'?", "Confirmar",
                MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;
            try
            {
                _service.Excluir(c.Id);
                Carregar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
