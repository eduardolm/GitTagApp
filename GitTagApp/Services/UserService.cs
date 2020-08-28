using GitTagApp.Entities;
using GitTagApp.Interfaces;
using GitTagApp.Services.GenericService;

namespace GitTagApp.Services
{
    public class UserService : GenericService<User>, IUserService
    {
        public UserService(IGenericRepository<User> repository) : base(repository)
        {
        }
    }
}