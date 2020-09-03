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
    public class GitRepoServiceTest
    {
        [Fact]
        public void Should_Return_All_GitRepos()
        {
            var fakeContext = new FakeContext("GetAllGitRepos");
            fakeContext.FillWith<GitRepo>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var userCountIndDb = context.GitRepos.Count();
                var repository = new GitRepoRepository(context);
                var service = new GitRepoService(repository);
                
                Assert.Equal(userCountIndDb, service.GetAll().Count());
                repository.Dispose();
            }
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void Should_Return_Right_GitRepo_When_Find_By_Id(long id)
        {
            var fakeContext = new FakeContext("GitRepoById");
            fakeContext.FillWith<GitRepo>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var expected = fakeContext.GetFakeData<GitRepo>().Find(x => x.Id == id);

                var repository = new GitRepoRepository(context);
                var service = new GitRepoService(repository);
                var actual = service.GetById(id);
                var tryZeroIdGitRepo = service.GetById(0);

                Assert.Null(tryZeroIdGitRepo);
                Assert.Equal(expected, actual, new GitRepoIdComparer());
                repository.Dispose();
            }
        }

        [Fact]
        public void Should_Create_New_GitRepo()
        {
            var fakeContext = new FakeContext("CreateNewGitRepo");

            var fakeGitRepo = new GitRepo();
            fakeGitRepo.Name = "GitRepo name";
            fakeGitRepo.Id = 1234;
            fakeGitRepo.Description = "New rpo for unit testing";
            fakeGitRepo.HttpUrl = "https://github.com/blob";
            fakeGitRepo.UserId = 321654;

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new GitRepoRepository(context);
                
               var service = new GitRepoService(repository);
                var actual = service.Create(fakeGitRepo);

                Assert.Equal("Item successfully created", actual);
                Assert.Equal(1234, service.GetById(1234).Id);
                Assert.Equal("https://github.com/blob", service.GetById(1234).HttpUrl);
                Assert.Equal("New rpo for unit testing", service.GetById(1234).Description);
                Assert.Equal("GitRepo name", service.GetById(1234).Name);
                repository.Dispose();
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void Should_Update_Existing_GitRepo(long id)
        {
            var fakeContext = new FakeContext("UpdateGitRepo");
            fakeContext.FillWith<GitRepo>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new GitRepoRepository(context);
                var service = new GitRepoService(repository);
                var currentGitRepo = service.GetById(id);

                currentGitRepo.Id = 1234;
                service.Update(currentGitRepo);
                Assert.Equal(1234, service.GetById(id).Id);
                repository.Dispose();
            }
        }

        [Fact]
        public void Should_Delete_GitRepo()
        {
            var fakeContext = new FakeContext("DeleteGitRepo");
            fakeContext.FillWith<GitRepo>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new GitRepoRepository(context);
                var service = new GitRepoService(repository);
                var currentCount = context.GitRepos.Count();
                var newGitRepo = new GitRepo();
                newGitRepo.Name = "New GitRepo";
                newGitRepo.Id = 1234;
                service.Create(newGitRepo);
                var createdGitRepo = (from u in service.GetAll()
                    where u.Id != 0
                    select u).FirstOrDefault();

                Assert.NotEqual(0, currentCount);
                service.Delete(createdGitRepo.Id);
                Assert.Equal(currentCount,context.GitRepos.Count());
                repository.Dispose();
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void Should_Not_Create_Repeated_GitRepo(long id)
        {
            var fakeContext = new FakeContext("DeleteGitRepo");
            fakeContext.FillWith<GitRepo>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new GitRepoRepository(context);
                var service = new GitRepoService(repository);

                var newGitRepo = new GitRepo();
                newGitRepo.Name = "GitRepo Name";
                newGitRepo.Id = 6;
                service.Create(newGitRepo);
                var existingGitRepo = (from u in service.GetAll()
                    where u.Id == id
                    select u).FirstOrDefault();

                var anotherGitRepo = new GitRepo();
                anotherGitRepo.Name = "GitRepo Name 2";
                anotherGitRepo.Id = 6;
                service.Create(anotherGitRepo);
                
                Assert.NotEqual(newGitRepo.Id, existingGitRepo.Id);
                Assert.NotEqual(newGitRepo.Name, existingGitRepo.Name);
                repository.Dispose();
            }
        }
    }
}