using System;
using System.Data.Entity;
using System.Windows;
using Barbearia.Data;

namespace Barbearia
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Database.SetInitializer(new BarbeariaInitializer());
            try
            {
                using (var ctx = new BarbeariaContext())
                {
                    ctx.Database.Initialize(force: false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao conectar ao banco de dados:\n\n" + ex.Message,
                    "Erro de Conexão", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}