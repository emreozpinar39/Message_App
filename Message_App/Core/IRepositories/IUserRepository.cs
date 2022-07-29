using Message_App.Models;
using System.Threading.Tasks;

namespace Message_App.Core.IRepositories
{
    public interface IUserRepository : IGenericRepository<Users>
    {
        Task<Users> GetByNameAndPassword(string username, string password);
    }
}
