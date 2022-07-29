using Message_App.Core.IConfiguration;
using Message_App.Core.IRepositories;
using Message_App.Core.Repositories;
using System;
using System.Threading.Tasks;

namespace Message_App.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _context;
        public IUserRepository Users { get; private set; }
        public IMessageRepository Messages { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
            Messages = new MessageRepository(_context);
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
