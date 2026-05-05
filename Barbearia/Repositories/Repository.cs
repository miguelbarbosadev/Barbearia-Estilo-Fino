using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Barbearia.Data;

namespace Barbearia.Repositories
{
    public class Repository<T> where T : class
    {
        protected readonly BarbeariaContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(BarbeariaContext context)
        {
            _context = context;
            _dbSet   = context.Set<T>();
        }

        public virtual List<T> ObterTodos()
        {
            return _dbSet.ToList();
        }

        public virtual T ObterPorId(int id)
        {
            return _dbSet.Find(id);
        }

        public virtual void Adicionar(T entidade)
        {
            _dbSet.Add(entidade);
            _context.SaveChanges();
        }

        public virtual void Atualizar(T entidade)
        {
            _context.Entry(entidade).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public virtual void Remover(int id)
        {
            var entidade = ObterPorId(id);
            if (entidade != null)
            {
                _dbSet.Remove(entidade);
                _context.SaveChanges();
            }
        }
    }
}
