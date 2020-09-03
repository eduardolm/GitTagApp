using System.Linq;
using GitTagApp.Entities;
using GitTagApp.Repositories;
using GitTagApp.Repositories.Context;
using GitTagApp.Tests.Unit.Entities;
using GitTagApp.Tests.Unit.Repositories.Context;
using Xunit;

namespace GitTagApp.Tests.Unit.Repositories
{
    public class GitRepoRepositoryTest
    {
        [Fact]
        public void Should_Return_All_GitRepos_In_Db()
        {
            var fakeContext = new FakeContext("GetAllGitRepos");
            fakeContext.FillWith<GitRepo>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var userCountIndDb = context.GitRepos.Count();
                var repository = new GitRepoRepository(context);
                
                Assert.Equal(userCountIndDb, repository.GetAll().Count());
                repository.Dispose();
            }
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_Return_Right_GitRepo_When_Find_By_Id_In_Db(long id)
        {
            var fakeContext = new FakeContext("GitRepoById");
            fakeContext.FillWith<GitRepo>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var expected = fakeContext.GetFakeData<GitRepo>().Find(x => x.Id == id);
                var repository = new GitRepoRepository(context);
                var actual = repository.GetById(id);
                
                Assert.Equal(expected, actual, new GitRepoIdComparer());
                repository.Dispose();
            }
        }
        
        [Fact]
        public void Should_Save_New_GitRepo_To_Db()
        {
            var fakeContext = new FakeContext("AddNewGitRepo");

            var fakeGitRepo = new GitRepo();
            fakeGitRepo.Name = "full name";
            fakeGitRepo.Id = 1234;
            fakeGitRepo.Description = "Testing repo repository";
            fakeGitRepo.HttpUrl = "https://github.com/teste/testing";
            
            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new GitRepoRepository(context);
                repository.Create(fakeGitRepo);

                var createdGitRepo = repository.GetById(1234);
                
                Assert.NotEqual(0, fakeGitRepo.Id);
                Assert.Equal("full name", createdGitRepo.Name);
                Assert.Equal(1234, createdGitRepo.Id);
                Assert.Equal("Testing repo repository", createdGitRepo.Description);
                Assert.Equal("https://github.com/teste/testing", createdGitRepo.HttpUrl);
                repository.Dispose();
            }
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_Update_GitRepo_In_Db(long id)
        {
            var fakeContext = new FakeContext("UpdateGitRepo");
            fakeContext.FillWith<GitRepo>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new GitRepoRepository(context);
                var currentGitRepo = repository.GetById(id);

                currentGitRepo.Name = "123abc";
                repository.Update(currentGitRepo);
                Assert.Equal("123abc", repository.GetById(id).Name);
                repository.Dispose();
            }
        }
        
        [Fact]
        public void Should_Delete_GitRepo_In_Db()
        {
            var fakeContext = new FakeContext("DeleteGitRepo");
            fakeContext.FillWith<GitRepo>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new GitRepoRepository(context);
                var currentCount = context.GitRepos.Count();
                var newGitRepo = new GitRepo();
                newGitRepo.Name = "Test GitRepo";
                newGitRepo.Id = 123456798;
                repository.Create(newGitRepo);
                var idToDelete = (from u in repository.GetAll()
                    where u.Id == newGitRepo.Id
                    select u.Id).FirstOrDefault();

                Assert.Equal(currentCount + 1, repository.GetAll().ToList().Count);
                repository.Delete(idToDelete);
                Assert.Equal(currentCount, repository.GetAll().ToList().Count());
                repository.Dispose();
            }
        }
    }
}