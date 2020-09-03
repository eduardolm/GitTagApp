using GitTagApp.Entities;
using GitTagApp.Repositories;
using GitTagApp.Repositories.Context;
using GitTagApp.Services;
using GitTagApp.Tests.Unit.Entities;
using GitTagApp.Tests.Unit.Repositories.Context;
using Xunit;

namespace GitTagApp.Tests.Unit.Services
{
    public class RepoBuilderTest
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void Should_Build_Repo_Correctly(long id)
        {
            var fakeContext = new FakeContext("RepoBuilderTest");
            fakeContext.FillWithAll();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var expected = fakeContext.GetFakeData<GitRepo>().Find(x => x.Id == id);

                var repoRepository = new GitRepoRepository(context);
                var repoService = new GitRepoService(repoRepository);
                var actualRepo = repoService.GetById(id);
                
                var tagRepository = new TagRepository(context);
                var tagService = new TagService(tagRepository);
                var actualTag = tagService.GetById(id);
                
                var repoTagRepository = new GitRepoTagRepository(context);
                var repoTagService = new GitRepoTagService(repoTagRepository);
                
                var userRepository = new UserRepository(context);
                var userService = new UserService(userRepository);
                var actualUser = userService.GetById(id);
                
                var repoBuilder = new RepoBuilder(userService, tagService, repoTagService);
                var testRepo = repoBuilder.GetPayload(actualRepo);

                Assert.Equal(id, testRepo.Id);
                Assert.Equal(repoService.GetById(id), testRepo);
                Assert.NotNull(repoService.GetById(id).User);
                Assert.Equal(repoService.GetById(id).UserId, testRepo.UserId);
                Assert.Equal(repoService.GetById(id).Name, testRepo.Name);
                Assert.Equal(repoService.GetById(id).Description, testRepo.Description);
            }
        }
    }
}