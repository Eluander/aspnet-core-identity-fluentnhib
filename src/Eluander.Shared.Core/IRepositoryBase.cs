using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Eluander.Shared.Core
{
    public interface IRepositoryBase<T> : IDisposable where T : BaseEntity
    {
        /// <summary>
        /// Adicionar uma entidade e retornar a entidade.
        /// </summary>
        /// <param name="entidade"></param>
        /// <returns></returns>
        T Adicionar(T entidade);
        /// <summary>
        /// Atualizar uma entidade e retornar a entidade atualizada.
        /// </summary>
        /// <param name="entidade"></param>
        /// <returns></returns>
        T Atualizar(T entidade);
        /// <summary>
        /// Deletar uma entidade sem retorno.
        /// </summary>
        /// <param name="entidade"></param>
        void Deletar(T entidade);

        /// <summary>
        /// Async adicionar uma entidade sem retorno.
        /// </summary>
        /// <param name="entidade"></param>
        /// <returns></returns>
        Task AdicionarAsync(T entidade);
        /// <summary>
        /// Async atualizar uma entidade sem retorno.
        /// </summary>
        /// <param name="entidade"></param>
        /// <returns></returns>
        Task AtualizarAsync(T entidade);
        /// <summary>
        /// Deletar uma entidade sem retorno
        /// </summary>
        /// <param name="entidade"></param>
        /// <returns></returns>
        Task DeletarAsync(T entidade);

        /// <summary>
        /// Obter uma entidade passando uma expressão lambda.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        T ObterPor(Expression<Func<T, bool>> expression);
        /// <summary>
        /// Obter lista de entidade pagda passando uma expressão lambda.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        IEnumerable<T> ObterListaPor(Expression<Func<T, bool>> expression, int pag, int total);

        /// <summary>
        /// Async obter uma entidade passando uma expressão lambda.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<T> ObterPorAsync(Expression<Func<T, bool>> expression);
        /// <summary>
        /// Obter lista de entidade paginda passando uma expressão lambda.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> ObterListaPorAsync(Expression<Func<T, bool>> expression, int pag, int total);

        /// <summary>
        /// Listar todas entidades.
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> ObterTodos();
        /// <summary>
        /// Listar todas entidades pagdas.
        /// </summary>
        /// <param name="pag"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        IEnumerable<T> ObterTodos(int pag, int total);

        /// <summary>
        /// Async listar todas entidades.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> ObterTodosAsync();
        /// <summary>
        /// Async listar todas entidades pagdas.
        /// </summary>
        /// <param name="pag"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> ObterTodosAsync(int pag, int total);
    }
}
