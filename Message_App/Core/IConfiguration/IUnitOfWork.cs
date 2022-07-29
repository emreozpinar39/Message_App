using Message_App.Core.IRepositories;
using System.Threading.Tasks;

namespace Message_App.Core.IConfiguration
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IMessageRepository Messages { get; }
        Task CompleteAsync();
    }
}
