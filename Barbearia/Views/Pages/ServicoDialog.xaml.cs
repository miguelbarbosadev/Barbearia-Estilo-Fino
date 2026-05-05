using System;
using System.Globalization;
using System.Windows;
using Barbearia.Data;
using Barbearia.Entidades;
using Barbearia.Services;

namespace Barbearia.Views.Pages
{
    public partial class ServicoDialog : Window
    {
        private readonly ServicoService _service;
        private readonly Servico _editando;

        public ServicoDialog(Servico servico = null)
        {
            InitializeComponent();
            _service  = new ServicoService(new BarbeariaContext());
            _editando = servico;

            if (servico != null)
            {
                TxtTitulo.Text  = "Editar Serviço";
                TxtNome.Text    = servico.Nome;
                TxtPreco.Text   = servico.Preco.ToString("F2");
                TxtDuracao.Text = servico.DuracaoMinutos.ToString();
            }
        }

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                decimal preco;
                if (!decimal.TryParse(TxtPreco.Text.Replace(",", "."),
                    NumberStyles.Any, CultureInfo.InvariantCulture, out preco))
                    throw new ArgumentException("Preço inválido.");

                int duracao;
                if (!int.TryParse(TxtDuracao.Text, out duracao))
                    throw new ArgumentException("Duração inválida.");

                var s = _editando ?? new Servico();
                s.Nome           = TxtNome.Text;
                s.Preco          = preco;
                s.DuracaoMinutos = duracao;
                _service.Salvar(s);
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
