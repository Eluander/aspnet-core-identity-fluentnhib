using NHibernate;
using System.Threading.Tasks;

namespace Eluander.Shared.Core
{
    public interface IUow
    {
        ISession GetSession();
        void OpenTransaction();

        Task Commit();
        void Rollback();
    }
}