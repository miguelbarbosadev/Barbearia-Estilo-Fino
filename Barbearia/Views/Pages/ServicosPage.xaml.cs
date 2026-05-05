using System;
using System.Windows;
using System.Windows.Controls;
using Barbearia.Data;
using Barbearia.Entidades;
using Barbearia.Services;

namespace Barbearia.Views.Pages
{
    public partial class ServicosPage : Page
    {
        private readonly ServicoService _service;

        public ServicosPage()
        {
            InitializeComponent();
            _service = new ServicoService(new BarbeariaContext());
            Loaded  += (s, e) => Carregar();
        }

        private void Carregar()
        {
            GridServicos.ItemsSource = _service.ListarTodos();
        }

        private void BtnNovo_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new ServicoDialog();
            if (dlg.ShowDialog() == true) Carregar();
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            var s = GridServicos.SelectedItem as Servico;
            if (s == null) return;
            var dlg = new ServicoDialog(s);
            if (dlg.ShowDialog() == true) Carregar();
        }

        private void BtnExcluir_Click(object sender, RoutedEventArgs e)
        {
            var s = GridServicos.SelectedItem as Servico;
            if (s == null) return;
            if (MessageBox.Show("Excluir serviço '" + s.Nome + "'?", "Confirmar",
                MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;
            _service.Excluir(s.Id);
            Carregar();
        }
    }
}
