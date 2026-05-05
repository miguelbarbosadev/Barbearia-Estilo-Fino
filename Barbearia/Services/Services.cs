using System;
using System.Collections.Generic;
using System.Linq;
using Barbearia.Data;
using Barbearia.Entidades;
using Barbearia.Repositories;

namespace Barbearia.Services
{
    public class ClienteService
    {
        private readonly Repository<Cliente> _repo;
        private readonly BarbeariaContext _context;

        public ClienteService(BarbeariaContext context)
        {
            _context = context;
            _repo    = new Repository<Cliente>(context);
        }

        public List<Cliente> ListarTodos()
        {
            return _repo.ObterTodos();
        }

        public Cliente ObterPorId(int id)
        {
            return _repo.ObterPorId(id);
        }

        public List<Cliente> BuscarPorNome(string termo)
        {
            return _context.Clientes
                .Where(c => c.Nome.Contains(termo))
                .ToList();
        }

        public void Salvar(Cliente cliente)
        {
            if (string.IsNullOrWhiteSpace(cliente.Nome))
                throw new ArgumentException("Nome do cliente é obrigatório.");

            if (cliente.Id == 0)
                _repo.Adicionar(cliente);
            else
                _repo.Atualizar(cliente);
        }

        public void Excluir(int id)
        {
            bool temAgendamentos = _context.Agendamentos.Any(a => a.ClienteId == id);
            if (temAgendamentos)
                throw new InvalidOperationException("Não é possível excluir: cliente possui agendamentos.");
            _repo.Remover(id);
        }
    }

    public class BarbeiroService
    {
        private readonly Repository<Barbeiro> _repo;
        private readonly BarbeariaContext _context;

        public BarbeiroService(BarbeariaContext context)
        {
            _context = context;
            _repo    = new Repository<Barbeiro>(context);
        }

        public List<Barbeiro> ListarTodos()
        {
            return _repo.ObterTodos();
        }

        public Barbeiro ObterPorId(int id)
        {
            return _repo.ObterPorId(id);
        }

        public void Salvar(Barbeiro barbeiro)
        {
            if (string.IsNullOrWhiteSpace(barbeiro.Nome))
                throw new ArgumentException("Nome do barbeiro é obrigatório.");

            if (barbeiro.Id == 0)
                _repo.Adicionar(barbeiro);
            else
                _repo.Atualizar(barbeiro);
        }

        public void Excluir(int id)
        {
            bool temAgendamentos = _context.Agendamentos.Any(a => a.BarbeiroId == id);
            if (temAgendamentos)
                throw new InvalidOperationException("Não é possível excluir: barbeiro possui agendamentos.");
            _repo.Remover(id);
        }
    }

    public class ServicoService
    {
        private readonly Repository<Servico> _repo;

        public ServicoService(BarbeariaContext context)
        {
            _repo = new Repository<Servico>(context);
        }

        public List<Servico> ListarTodos()
        {
            return _repo.ObterTodos();
        }

        public Servico ObterPorId(int id)
        {
            return _repo.ObterPorId(id);
        }

        public void Salvar(Servico servico)
        {
            if (string.IsNullOrWhiteSpace(servico.Nome))
                throw new ArgumentException("Nome do serviço é obrigatório.");
            if (servico.Preco <= 0)
                throw new ArgumentException("Preço deve ser maior que zero.");
            if (servico.DuracaoMinutos <= 0)
                throw new ArgumentException("Duração deve ser maior que zero.");

            if (servico.Id == 0)
                _repo.Adicionar(servico);
            else
                _repo.Atualizar(servico);
        }

        public void Excluir(int id)
        {
            _repo.Remover(id);
        }
    }

    public class AgendamentoService
    {
        private readonly AgendamentoRepository _repo;

        public AgendamentoService(BarbeariaContext context)
        {
            _repo = new AgendamentoRepository(context);
        }

        public List<Agendamento> ListarTodos()
        {
            return _repo.ObterTodosComDetalhes();
        }

        public List<Agendamento> ListarPorMes(int ano, int mes)
        {
            return _repo.ObterPorMes(ano, mes);
        }

        public List<Agendamento> ListarPorDia(DateTime dia)
        {
            return _repo.ObterPorDia(dia);
        }

        public void Salvar(Agendamento agendamento)
        {
            if (agendamento.ClienteId == 0)  throw new ArgumentException("Selecione um cliente.");
            if (agendamento.BarbeiroId == 0) throw new ArgumentException("Selecione um barbeiro.");
            if (agendamento.ServicoId == 0)  throw new ArgumentException("Selecione um serviço.");

            if (agendamento.Id == 0)
                _repo.Adicionar(agendamento);
            else
                _repo.Atualizar(agendamento);
        }

        public void Excluir(int id)
        {
            _repo.Remover(id);
        }

        public void AlterarStatus(int id, StatusAgendamento novoStatus)
        {
            var ag = _repo.ObterPorId(id);
            if (ag == null) throw new InvalidOperationException("Agendamento não encontrado.");
            ag.Status = novoStatus;
            _repo.Atualizar(ag);
        }

        public int ContarAtendimentosMes(int ano, int mes)
        {
            return _repo.ContarAtendimentosMes(ano, mes);
        }

        public decimal FaturamentoMes(int ano, int mes)
        {
            return _repo.FaturamentoMes(ano, mes);
        }

        public List<ServicoRanking> ServicosMaisRealizados(int ano, int mes)
        {
            return _repo.ServicosMaisRealizados(ano, mes);
        }
    }
}
