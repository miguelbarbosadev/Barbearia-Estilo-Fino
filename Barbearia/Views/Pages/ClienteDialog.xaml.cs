using System;
using System.Windows;
using Barbearia.Data;
using Barbearia.Entidades;
using Barbearia.Services;

namespace Barbearia.Views.Pages
{
    public partial class ClienteDialog : Window
    {
        private readonly ClienteService _service;
        private readonly Cliente _editando;

        public ClienteDialog(Cliente cliente = null)
        {
            InitializeComponent();
            _service  = new ClienteService(new BarbeariaContext());
            _editando = cliente;

            if (cliente != null)
            {
                TxtTitulo.Text   = "Editar Cliente";
                TxtNome.Text     = cliente.Nome;
                TxtTelefone.Text = cliente.Telefone;
                TxtEmail.Text    = cliente.Email;
            }
        }

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var c = _editando ?? new Cliente();
                c.Nome     = TxtNome.Text;
                c.Telefone = TxtTelefone.Text;
                c.Email    = TxtEmail.Text;
                _service.Salvar(c);
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
