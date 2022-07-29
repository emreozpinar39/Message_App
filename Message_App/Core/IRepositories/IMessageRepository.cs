using Message_App.Models;
using System.Threading.Tasks;

namespace Message_App.Core.IRepositories
{
    public interface IMessageRepository : IGenericRepository<Message>
    {
        Task<Message> GetByMessageId(int id);
        Task<bool> DeleteMessage(int id);
    }
}
