using Message_App.Core.IRepositories;
using Message_App.Data;
using Message_App.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Message_App.Core.Repositories
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        public MessageRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Message> GetByMessageId(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<bool> DeleteMessage(int id)
        {
            var message = await dbSet.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (message != null)
            {
                dbSet.Remove(message);
                return true;
            }
            return false;
        }

        public override async Task<bool> Update(Message entity)
        {
            var existingUser = await dbSet.Where(x => x.Id == entity.Id).FirstOrDefaultAsync();

            if (existingUser == null)
            {
                return await Add(entity);
            }

            existingUser.Content = entity.Content;
            existingUser.MessageDate = DateTime.Now.ToString("dd/MM/yyyy");

            return true;

        }

    }
}
