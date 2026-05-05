using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Barbearia.Data;
using Barbearia.Entidades;

namespace Barbearia.Repositories
{
    public class AgendamentoRepository : Repository<Agendamento>
    {
        public AgendamentoRepository(BarbeariaContext context) : base(context) { }

        public List<Agendamento> ObterTodosComDetalhes()
        {
            return _dbSet
                .Include(a => a.Cliente)
                .Include(a => a.Barbeiro)
                .Include(a => a.Servico)
                .OrderByDescending(a => a.DataHora)
                .ToList();
        }

        public List<Agendamento> ObterPorMes(int ano, int mes)
        {
            return _dbSet
                .Include(a => a.Cliente)
                .Include(a => a.Barbeiro)
                .Include(a => a.Servico)
                .Where(a => a.DataHora.Year == ano && a.DataHora.Month == mes)
                .OrderBy(a => a.DataHora)
                .ToList();
        }

        public List<Agendamento> ObterPorDia(DateTime dia)
        {
            return _dbSet
                .Include(a => a.Cliente)
                .Include(a => a.Barbeiro)
                .Include(a => a.Servico)
                .Where(a => DbFunctions.TruncateTime(a.DataHora) == dia.Date)
                .OrderBy(a => a.DataHora)
                .ToList();
        }

        public int ContarAtendimentosMes(int ano, int mes)
        {
            return _dbSet.Count(a =>
                a.DataHora.Year == ano &&
                a.DataHora.Month == mes &&
                a.Status == StatusAgendamento.Concluido);
        }

        public decimal FaturamentoMes(int ano, int mes)
        {
            var lista = _dbSet
                .Include(a => a.Servico)
                .Where(a => a.DataHora.Year == ano &&
                            a.DataHora.Month == mes &&
                            a.Status == StatusAgendamento.Concluido)
                .ToList();

            return lista.Sum(a => a.Servico != null ? a.Servico.Preco : 0);
        }

        public List<ServicoRanking> ServicosMaisRealizados(int ano, int mes)
        {
            return _dbSet
                .Include(a => a.Servico)
                .Where(a => a.DataHora.Year == ano &&
                            a.DataHora.Month == mes &&
                            a.Status == StatusAgendamento.Concluido)
                .GroupBy(a => a.Servico.Nome)
                .Select(g => new ServicoRanking { Nome = g.Key, Quantidade = g.Count() })
                .OrderByDescending(x => x.Quantidade)
                .Take(5)
                .ToList();
        }
    }

    public class ServicoRanking
    {
        public string Nome { get; set; }
        public int Quantidade { get; set; }
    }
}
