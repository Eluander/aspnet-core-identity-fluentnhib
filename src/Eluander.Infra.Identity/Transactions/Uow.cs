using NHibernate;
using System.Threading.Tasks;

namespace Eluander.Infra.Identity.Transactions
{
    public class Uow : IUow
    {
        #region Repositorios e Construtores.
        private readonly ISessionFactory sessionFactory;
        private ISession _session;
        public Uow(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }
        #endregion

        public ISession GetSession()
        {
            if (this._session == null)
            {
                this._session = sessionFactory.OpenSession();
            }
            return _session;
        }

        public void OpenTransaction()
        {
            if (this._session == null) return;

            if (!_session.Transaction.IsActive)
                _session.BeginTransaction();

        }
        public async Task Commit()
        {
            if (this._session == null) return;

            await _session?.FlushAsync();

            if (_session.Transaction.IsActive)
                _session.Transaction.Commit();
        }
        public void Rollback()
        {
            if (this._session == null) return;

            if (_session.Transaction.IsActive)
                _session.Transaction.Rollback();
        }
    }
}
