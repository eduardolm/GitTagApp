using GitTagApp.Entities;
using GitTagApp.Interfaces;
using GitTagApp.Repositories.Context;
using GitTagApp.Repositories.GenericRepository;

namespace GitTagApp.Repositories
{
    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        public TagRepository(MainContext dbContext) : base(dbContext)
        {
        }
    }
}