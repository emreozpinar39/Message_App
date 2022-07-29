using System.Threading.Tasks;

namespace Message_App.Core.IConfiguration
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}
