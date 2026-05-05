using System;
using System.Windows;
using System.Windows.Controls;
using Barbearia.Data;
using Barbearia.Entidades;
using Barbearia.Services;

namespace Barbearia.Views.Pages
{
    public partial class BarbeirosPage : Page
    {
        private readonly BarbeiroService _service;

        public BarbeirosPage()
        {
            InitializeComponent();
            _service = new BarbeiroService(new BarbeariaContext());
            Loaded  += (s, e) => Carregar();
        }

        private void Carregar()
        {
            GridBarbeiros.ItemsSource = _service.ListarTodos();
        }

        private void BtnNovo_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new BarbeiroDialog();
            if (dlg.ShowDialog() == true) Carregar();
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            var b = GridBarbeiros.SelectedItem as Barbeiro;
            if (b == null) return;
            var dlg = new BarbeiroDialog(b);
            if (dlg.ShowDialog() == true) Carregar();
        }

        private void BtnExcluir_Click(object sender, RoutedEventArgs e)
        {
            var b = GridBarbeiros.SelectedItem as Barbeiro;
            if (b == null) return;
            if (MessageBox.Show("Excluir barbeiro '" + b.Nome + "'?", "Confirmar",
                MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;
            try
            {
                _service.Excluir(b.Id);
                Carregar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
