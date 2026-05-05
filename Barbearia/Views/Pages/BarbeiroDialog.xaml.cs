using System;
using System.Windows;
using Barbearia.Data;
using Barbearia.Entidades;
using Barbearia.Services;

namespace Barbearia.Views.Pages
{
    public partial class BarbeiroDialog : Window
    {
        private readonly BarbeiroService _service;
        private readonly Barbeiro _editando;

        public BarbeiroDialog(Barbeiro barbeiro = null)
        {
            InitializeComponent();
            _service  = new BarbeiroService(new BarbeariaContext());
            _editando = barbeiro;

            if (barbeiro != null)
            {
                TxtTitulo.Text         = "Editar Barbeiro";
                TxtNome.Text           = barbeiro.Nome;
                TxtEspecialidades.Text = barbeiro.Especialidades;
                TxtHorario.Text        = barbeiro.HorarioTrabalho;
            }
        }

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var b = _editando ?? new Barbeiro();
                b.Nome            = TxtNome.Text;
                b.Especialidades  = TxtEspecialidades.Text;
                b.HorarioTrabalho = TxtHorario.Text;
                _service.Salvar(b);
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
