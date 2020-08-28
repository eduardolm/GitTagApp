using GitTagApp.Entities;
using GitTagApp.Interfaces;
using GitTagApp.Repositories.Context;
using GitTagApp.Repositories.GenericRepository;

namespace GitTagApp.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(MainContext dbContext) : base(dbContext)
        {
        }
    }
}