using System.Linq;
using GitTagApp.Entities;
using GitTagApp.Repositories;
using GitTagApp.Repositories.Context;
using GitTagApp.Services;
using GitTagApp.Tests.Unit.Entities;
using GitTagApp.Tests.Unit.Repositories.Context;
using Xunit;

namespace GitTagApp.Tests.Unit.Services
{
    public class GitRepoTagServiceTest
    {
        [Fact]
        public void Should_Return_All_GitRepoTags()
        {
            var fakeContext = new FakeContext("GetAllGitRepoTags");
            fakeContext.FillWith<GitRepoTag>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var userCountIndDb = context.GitRepoTags.Count();
                var repository = new GitRepoTagRepository(context);
                var service = new GitRepoTagService(repository);
                
                Assert.Equal(userCountIndDb, service.GetAll().Count());
                repository.Dispose();
            }
        }
        
        [Theory]
        [InlineData(23083156,1)]
        [InlineData(23083156,2)]
        [InlineData(283106159,3)]
        [InlineData(23083156,4)]
        [InlineData(23083156,5)]
        public void Should_Return_Right_GitRepoTag_When_Find_By_Id(long repoId, long tagId)
        {
            var fakeContext = new FakeContext("GitRepoTagById");
            fakeContext.FillWith<GitRepoTag>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                //var expected = fakeContext.GetFakeData<GitRepoTag>().Find(repoId,tagId );

                var repository = new GitRepoTagRepository(context);
                var service = new GitRepoTagService(repository);
                var actual = service.GetByIdRepoTag(repoId, tagId);

                Assert.NotNull(actual);
                repository.Dispose();
            }
        }

        [Fact]
        public void Should_Create_New_GitRepoTag()
        {
            var fakeContext = new FakeContext("CreateNewGitRepoTag");

            var fakeGitRepoTag = new GitRepoTag();
            fakeGitRepoTag.GitRepoId = 321456;
            fakeGitRepoTag.TagId = 1;

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new GitRepoTagRepository(context);
                
               var service = new GitRepoTagService(repository);
                var actual = service.CreateRepoTag(fakeGitRepoTag);

                Assert.Equal("Item successfully created", actual);
                Assert.Equal(1, service.GetByIdRepoTag(321456,1).TagId);
                Assert.Equal(321456, service.GetByIdRepoTag(321456,1).GitRepoId);
                repository.Dispose();
            }
        }

        [Theory]
        [InlineData(23083156,1)]
        [InlineData(23083156,2)]
        [InlineData(283106159,3)]
        [InlineData(23083156,4)]
        [InlineData(23083156,5)]
        public void Should_Update_Existing_GitRepoTag(long repoId, long tagId)
        {
            var fakeContext = new FakeContext("UpdateGitRepoTag");
            fakeContext.FillWith<GitRepoTag>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new GitRepoTagRepository(context);
                var service = new GitRepoTagService(repository);
                var currentGitRepoTag = service.GetByIdRepoTag(repoId, tagId);

                currentGitRepoTag.GitRepoId = 1234;
                currentGitRepoTag.TagId = 321;
                service.Update(currentGitRepoTag);
                Assert.Equal(1234, service.GetByIdRepoTag(repoId, tagId).GitRepoId);
                Assert.Equal(321, service.GetByIdRepoTag(repoId, tagId).TagId);
                repository.Dispose();
            }
        }

        [Fact]
        public void Should_Delete_GitRepoTag()
        {
            var fakeContext = new FakeContext("DeleteGitRepoTag");
            fakeContext.FillWith<GitRepoTag>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new GitRepoTagRepository(context);
                var service = new GitRepoTagService(repository);
                var currentCount = context.GitRepoTags.Count();
                var newGitRepoTag = new GitRepoTag();
                newGitRepoTag.GitRepoId = 1111;
                newGitRepoTag.TagId = 1234;
                service.CreateRepoTag(newGitRepoTag);
                var createdGitRepoTag = service.GetByIdRepoTag(1111, 1234);

                Assert.NotEqual(0, currentCount);
                service.DeleteRepoTag(createdGitRepoTag.GitRepoId, createdGitRepoTag.TagId);
                Assert.Equal(currentCount,context.GitRepoTags.Count());
                repository.Dispose();
            }
        }

        [Theory]
        [InlineData(23083156,1)]
        [InlineData(23083156,2)]
        [InlineData(283106159,3)]
        [InlineData(23083156,4)]
        [InlineData(23083156,5)]
        public void Should_Not_Create_Repeated_GitRepoTag(long repoId, long tagId)
        {
            var fakeContext = new FakeContext("DeleteGitRepoTag");
            fakeContext.FillWith<GitRepoTag>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new GitRepoTagRepository(context);
                var service = new GitRepoTagService(repository);

                var newGitRepoTag = new GitRepoTag();
                newGitRepoTag.GitRepoId = 123;
                newGitRepoTag.TagId = 12;
                service.CreateRepoTag(newGitRepoTag);
                var existingGitRepoTag = (from u in service.GetAll()
                    where u.GitRepoId == repoId
                    select u).FirstOrDefault();

                var anotherGitRepoTag = new GitRepoTag();
                anotherGitRepoTag.GitRepoId = 123;
                anotherGitRepoTag.TagId = 12;
                var anotherResult = service.CreateRepoTag(anotherGitRepoTag);
                
                Assert.NotEqual(newGitRepoTag.GitRepoId, existingGitRepoTag.GitRepoId);
                Assert.NotEqual(newGitRepoTag.TagId, existingGitRepoTag.TagId);
                repository.Dispose();
            }
        }
    }
}