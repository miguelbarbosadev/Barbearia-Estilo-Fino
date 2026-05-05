using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Barbearia.Views.Pages;

namespace Barbearia.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Navegar("Calendario");
        }

        private void NavBtn_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn != null && btn.Tag is string pagina)
                Navegar(pagina);
        }

        private void Navegar(string pagina)
        {
            Page page = null;
            switch (pagina)
            {
                case "Calendario":   page = new CalendarioPage();   break;
                case "Agendamentos": page = new AgendamentosPage(); break;
                case "Clientes":     page = new ClientesPage();     break;
                case "Barbeiros":    page = new BarbeirosPage();    break;
                case "Servicos":     page = new ServicosPage();     break;
            }

            if (page != null)
                MainFrame.Navigate(page);

            var accentBrush   = new SolidColorBrush(Color.FromRgb(233, 69, 96));
            var inactiveBrush = new SolidColorBrush(Color.FromRgb(160, 160, 176));

            foreach (var child in NavPanel.Children)
            {
                var btn = child as Button;
                if (btn == null) continue;

                bool ativo = btn.Tag != null && btn.Tag.ToString() == pagina;
                btn.Background = ativo ? accentBrush : Brushes.Transparent;
                btn.Foreground = ativo ? Brushes.White : inactiveBrush;
            }
        }
    }
}
