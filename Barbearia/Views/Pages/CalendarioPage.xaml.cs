using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Barbearia.Data;
using Barbearia.Entidades;
using Barbearia.Services;

namespace Barbearia.Views.Pages
{
    public partial class CalendarioPage : Page
    {
        private readonly AgendamentoService _service;
        private DateTime _mesAtual;
        private List<Agendamento> _agendamentosMes = new List<Agendamento>();

        public CalendarioPage()
        {
            InitializeComponent();
            _service   = new AgendamentoService(new BarbeariaContext());
            _mesAtual  = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            Loaded    += (s, e) => CarregarMes();
        }

        private void CarregarMes()
        {
            TxtMesAtual.Text    = _mesAtual.ToString("MMMM yyyy").ToUpper();
            _agendamentosMes    = _service.ListarPorMes(_mesAtual.Year, _mesAtual.Month);
            DesenharCalendario();
        }

        private void DesenharCalendario()
        {
            GridCalendario.Children.Clear();

            int primeiroDia = (int)new DateTime(_mesAtual.Year, _mesAtual.Month, 1).DayOfWeek;
            int diasNoMes   = DateTime.DaysInMonth(_mesAtual.Year, _mesAtual.Month);
            var hoje        = DateTime.Today;

            for (int i = 0; i < primeiroDia; i++)
                GridCalendario.Children.Add(CriarCelulaVazia());

            for (int dia = 1; dia <= diasNoMes; dia++)
            {
                var data    = new DateTime(_mesAtual.Year, _mesAtual.Month, dia);
                var agsDia  = new List<Agendamento>();
                foreach (var a in _agendamentosMes)
                    if (a.DataHora.Date == data.Date) agsDia.Add(a);

                bool ehHoje = data.Date == hoje;
                GridCalendario.Children.Add(CriarCelulaDia(dia, agsDia, ehHoje, data));
            }

            int total = primeiroDia + diasNoMes;
            int resto = total % 7;
            if (resto != 0)
                for (int i = 0; i < 7 - resto; i++)
                    GridCalendario.Children.Add(CriarCelulaVazia());
        }

        private Border CriarCelulaDia(int dia, List<Agendamento> ags, bool ehHoje, DateTime data)
        {
            var border = new Border
            {
                Margin       = new Thickness(2),
                CornerRadius = new CornerRadius(8),
                Background   = ehHoje
                    ? new SolidColorBrush(Color.FromRgb(233, 69, 96))
                    : ags.Count > 0
                        ? new SolidColorBrush(Color.FromRgb(15, 52, 96))
                        : new SolidColorBrush(Color.FromRgb(30, 42, 74)),
                Cursor  = Cursors.Hand,
                Padding = new Thickness(6),
                MinHeight = 60
            };

            var stack = new StackPanel();
            stack.Children.Add(new TextBlock
            {
                Text       = dia.ToString(),
                FontWeight = FontWeights.Bold,
                Foreground = ehHoje ? Brushes.White : Brushes.LightGray,
                FontSize   = 13
            });

            if (ags.Count > 0)
            {
                stack.Children.Add(new TextBlock
                {
                    Text       = ags.Count + " ag.",
                    Foreground = ehHoje
                        ? Brushes.White
                        : new SolidColorBrush(Color.FromRgb(233, 69, 96)),
                    FontSize = 10
                });
            }

            border.Child = stack;

            var capturedData = data;
            border.MouseLeftButtonDown += (s, e) => MostrarAgendamentosDia(capturedData);

            return border;
        }

        private Border CriarCelulaVazia()
        {
            return new Border
            {
                Margin    = new Thickness(2),
                Background = new SolidColorBrush(Color.FromRgb(24, 32, 56)),
                CornerRadius = new CornerRadius(8),
                MinHeight = 60
            };
        }

        private void MostrarAgendamentosDia(DateTime data)
        {
            TxtDiaSelecionado.Text    = "📅 " + data.ToString("dddd, dd 'de' MMMM");
            ListAgendamentosDia.ItemsSource = _service.ListarPorDia(data);
        }

        private void BtnMesAnterior_Click(object sender, RoutedEventArgs e)
        {
            _mesAtual = _mesAtual.AddMonths(-1);
            CarregarMes();
        }

        private void BtnProximoMes_Click(object sender, RoutedEventArgs e)
        {
            _mesAtual = _mesAtual.AddMonths(1);
            CarregarMes();
        }
    }
}
