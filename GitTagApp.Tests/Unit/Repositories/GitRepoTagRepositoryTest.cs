using System.Linq;
using GitTagApp.Entities;
using GitTagApp.Repositories;
using GitTagApp.Repositories.Context;
using GitTagApp.Tests.Unit.Repositories.Context;
using Xunit;

namespace GitTagApp.Tests.Unit.Repositories
{
    public class GitRepoTagRepositoryTest
    {
        [Fact]
        public void Should_Return_All_GitRepoTags_In_Db()
        {
            var fakeContext = new FakeContext("GetAllGitRepoTag");
            fakeContext.FillWith<GitRepoTag>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var userCountIndDb = context.GitRepoTags.Count();
                var repository = new GitRepoTagRepository(context);
                
                Assert.Equal(userCountIndDb, repository.GetAll().Count());
                repository.Dispose();
            }
        }
        
        // [Theory]
        // [InlineData(23083156,1)]
        // [InlineData(23083156,2)]
        // [InlineData(283106159,3)]
        // public void Should_Return_Right_GitRepoTag_When_Find_By_Id_In_Db(long repoId, long tagId)
        // {
        //     var fakeContext = new FakeContext("GitRepoTagById");
        //     fakeContext.FillWith<GitRepoTag>();
        //
        //     using (var context = new MainContext(fakeContext.FakeOptions))
        //     {
        //         var expected = fakeContext.GetFakeData<GitRepoTag>().Find(repoId, tagId);
        //         var repository = new GitRepoTagRepository(context);
        //         var actual = repository.GetByIdRepoTag(repoId, tagId);
        //         
        //         Assert.Equal(expected, actual, new GitRepoTagIdComparer());
        //         repository.Dispose();
        //     }
        // }
        
        [Fact]
        public void Should_Save_New_GitRepoTag_To_Db()
        {
            var fakeContext = new FakeContext("AddNewGitRepoTag");

            var fakeGitRepoTag = new GitRepoTag();
            fakeGitRepoTag.TagId = 1111;
            fakeGitRepoTag.GitRepoId = 1234;

            
            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new GitRepoTagRepository(context);
                repository.CreateRepoTag(fakeGitRepoTag);

                var createdGitRepoTag = repository.GetByIdRepoTag(1234, 1111);
                
                Assert.Equal(0, fakeGitRepoTag.Id);
                Assert.Equal(1111, createdGitRepoTag.TagId);
                Assert.Equal(1234, createdGitRepoTag.GitRepoId);
                repository.Dispose();
            }
        }

        [Fact]
        public void Should_Delete_GitRepo_In_Db()
        {
            var fakeContext = new FakeContext("DeleteGitRepoTag");
            fakeContext.FillWith<GitRepoTag>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new GitRepoTagRepository(context);
                var currentCount = context.GitRepoTags.Count();
                var newGitRepoTag = new GitRepoTag();
                newGitRepoTag.GitRepoId = 13212;
                newGitRepoTag.TagId = 123456798;
                repository.CreateRepoTag(newGitRepoTag);
                var idToDelete = (from u in repository.GetAll()
                    where u.GitRepoId == newGitRepoTag.GitRepoId
                    where u.TagId == newGitRepoTag.TagId
                    select new {u.GitRepoId, u.TagId}).FirstOrDefault();

                Assert.Equal(currentCount + 1, repository.GetAll().ToList().Count);
                repository.DeleteRepoTag(idToDelete.GitRepoId, idToDelete.TagId);
                Assert.Equal(currentCount, repository.GetAll().ToList().Count());
                repository.Dispose();
            }
        }
    }
}