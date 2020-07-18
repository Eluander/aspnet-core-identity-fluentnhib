using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Eluander.Shared.Core
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : BaseEntity
    {
        protected IUow UoW { get; private set; }
        protected RepositoryBase(IUow uow)
        {
            this.UoW = uow;
        }

        public T Adicionar(T entidade)
        {
            UoW.GetSession().Save(entidade);
            UoW.GetSession().Flush();

            return entidade;
        }
        public T Atualizar(T entidade)
        {
            UoW.GetSession().UpdateAsync(entidade);
            UoW.GetSession().FlushAsync();

            return entidade;
        }
        public void Deletar(T entidade)
        {
            UoW.GetSession().DeleteAsync(entidade);
            UoW.GetSession().FlushAsync();
        }

        public async Task AdicionarAsync(T entidade)
        {
            await UoW.GetSession().SaveAsync(entidade);
            await UoW.GetSession().FlushAsync();
        }
        public async Task AtualizarAsync(T entidade)
        {
            await UoW.GetSession().UpdateAsync(entidade);
            await UoW.GetSession().FlushAsync();
        }
        public async Task DeletarAsync(T entidade)
        {
            await UoW.GetSession().DeleteAsync(entidade);
            await UoW.GetSession().FlushAsync();
        }


        public T ObterPor(Expression<Func<T, bool>> expression)
        {
            return UoW.GetSession().QueryOver<T>().Where(expression).SingleOrDefault();
        }
        public IEnumerable<T> ObterListaPor(Expression<Func<T, bool>> expression, int pagina, int tamanhoPagina)
        {
            return UoW.GetSession().QueryOver<T>().Where(expression).Skip(pagina).Take(tamanhoPagina).List();
        }
        public IEnumerable<T> ObterTodos()
        {
            return UoW.GetSession().QueryOver<T>().List<T>();
        }
        public IEnumerable<T> ObterTodos(int page, int pageSize)
        {
            return UoW.GetSession().QueryOver<T>().Skip(page).Take(pageSize).List<T>();
        }

        public async Task<T> ObterPorAsync(Expression<Func<T, bool>> expression)
        {
            return await UoW.GetSession().QueryOver<T>().Where(expression).SingleOrDefaultAsync();
        }
        public async Task<IEnumerable<T>> ObterListaPorAsync(Expression<Func<T, bool>> expression, int pagina, int tamanhoPagina)
        {
            return await UoW.GetSession().QueryOver<T>().Where(expression).Skip(pagina).Take(tamanhoPagina).ListAsync();
        }
        public async Task<IEnumerable<T>> ObterTodosAsync()
        {
            return await UoW.GetSession().QueryOver<T>().ListAsync<T>();
        }
        public async Task<IEnumerable<T>> ObterTodosAsync(int page, int pageSize)
        {
            return await UoW.GetSession().QueryOver<T>().Skip(page).Take(pageSize).ListAsync<T>();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
