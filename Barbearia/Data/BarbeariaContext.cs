using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Barbearia.Entidades;

namespace Barbearia.Data
{
    public class BarbeariaContext : DbContext
    {
        public BarbeariaContext()
            : base("name=BarbeariaContext")
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Barbeiro> Barbeiros { get; set; }
        public DbSet<Servico> Servicos { get; set; }
        public DbSet<Agendamento> Agendamentos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Cliente>().ToTable("clientes");
            modelBuilder.Entity<Cliente>().Property(c => c.Nome).IsRequired().HasMaxLength(150);
            modelBuilder.Entity<Cliente>().Property(c => c.Telefone).HasMaxLength(20);
            modelBuilder.Entity<Cliente>().Property(c => c.Email).HasMaxLength(150);

            modelBuilder.Entity<Barbeiro>().ToTable("barbeiros");
            modelBuilder.Entity<Barbeiro>().Property(b => b.Nome).IsRequired().HasMaxLength(150);
            modelBuilder.Entity<Barbeiro>().Property(b => b.Especialidades).HasMaxLength(300);
            modelBuilder.Entity<Barbeiro>().Property(b => b.HorarioTrabalho).HasMaxLength(200);

            modelBuilder.Entity<Servico>().ToTable("servicos");
            modelBuilder.Entity<Servico>().Property(s => s.Nome).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Servico>().Property(s => s.Preco).HasPrecision(10, 2);

            modelBuilder.Entity<Agendamento>().ToTable("agendamentos");
            modelBuilder.Entity<Agendamento>().Property(a => a.Observacoes).HasMaxLength(500);

            modelBuilder.Entity<Agendamento>()
                .HasRequired(a => a.Cliente)
                .WithMany(c => c.Agendamentos)
                .HasForeignKey(a => a.ClienteId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Agendamento>()
                .HasRequired(a => a.Barbeiro)
                .WithMany(b => b.Agendamentos)
                .HasForeignKey(a => a.BarbeiroId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Agendamento>()
                .HasRequired(a => a.Servico)
                .WithMany()
                .HasForeignKey(a => a.ServicoId)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}