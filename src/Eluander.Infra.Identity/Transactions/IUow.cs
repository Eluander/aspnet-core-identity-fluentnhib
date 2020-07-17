using NHibernate;
using System.Threading.Tasks;

namespace Eluander.Infra.Identity.Transactions
{
    public interface IUow
    {
        ISession GetSession();

        void OpenTransaction();
        Task Commit();
        void Rollback();
    }
}
