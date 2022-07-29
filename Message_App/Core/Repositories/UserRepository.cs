using Message_App.Core.IRepositories;
using Message_App.Data;
using Message_App.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Message_App.Core.Repositories
{
    public class UserRepository : GenericRepository<Users>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Users>> All()
        {
            return await dbSet.ToListAsync();
        }

        public override async Task<bool> Update(Users entity)
        {
            var existingUser = await dbSet.Where(x => x.Id == entity.Id).FirstOrDefaultAsync();

            if (existingUser == null)
            {
                return await Add(entity);
            }

            existingUser.FirstName = entity.FirstName;
            existingUser.LastName = entity.LastName;
            existingUser.Password = entity.Password;
            existingUser.Email = entity.Email;

            return true;
        }

        public override async Task<bool> Delete(Guid id)
        {
            var exist = await dbSet.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (exist != null)
            {
                dbSet.Remove(exist);
                return true;
            }
            return false;
        }

        /*Defined In The IUserRepository, Did not come from IGenericRepsitory */
        public async Task<Users> GetByNameAndPassword(string username, string password)
        {
            var user = await dbSet.Where(x => x.FirstName == username & x.Password == password).FirstOrDefaultAsync();
            return user;
        }
    }
}
