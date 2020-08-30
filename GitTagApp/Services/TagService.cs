using GitTagApp.Entities;
using GitTagApp.Interfaces;
using GitTagApp.Services.GenericService;

namespace GitTagApp.Services
{
    public class TagService : GenericService<Tag>, ITagService
    {
        public TagService(IGenericRepository<Tag> repository) : base(repository)
        {
        }
    }
}