using System.Linq;
using GitTagApp.Entities;
using GitTagApp.Repositories;
using GitTagApp.Repositories.Context;
using GitTagApp.Tests.Unit.Entities;
using GitTagApp.Tests.Unit.Repositories.Context;
using Xunit;

namespace GitTagApp.Tests.Unit.Repositories
{
    public class TagRepositoryTest
    {
        [Fact]
        public void Should_Return_All_Tags_In_Db()
        {
            var fakeContext = new FakeContext("GetAllTags");
            fakeContext.FillWith<Tag>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var userCountIndDb = context.Tags.Count();
                var repository = new TagRepository(context);
                
                Assert.Equal(userCountIndDb, repository.GetAll().Count());
                repository.Dispose();
            }
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_Return_Right_Tags_When_Find_By_Id_In_Db(long id)
        {
            var fakeContext = new FakeContext("TagById");
            fakeContext.FillWith<Tag>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var expected = fakeContext.GetFakeData<Tag>().Find(x => x.Id == id);
                var repository = new TagRepository(context);
                var actual = repository.GetById(id);
                
                Assert.Equal(expected, actual, new TagIdComparer());
                repository.Dispose();
            }
        }
        
        [Fact]
        public void Should_Save_New_Tag_To_Db()
        {
            var fakeContext = new FakeContext("AddNewTag");

            var fakeTag = new Tag();
            fakeTag.Name = "full name";
            //fakeTag.Id = 1;
            
            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new TagRepository(context);
                repository.Create(fakeTag);

                var createdTag = repository.GetById(1);
                
                Assert.NotEqual(0, fakeTag.Id);
                Assert.Equal("full name", createdTag.Name);
                Assert.Equal(1, createdTag.Id);
                repository.Dispose();
            }
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_Update_Tag_In_Db(long id)
        {
            var fakeContext = new FakeContext("UpdateTag");
            fakeContext.FillWith<Tag>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new TagRepository(context);
                var currentTag = repository.GetById(id);

                currentTag.Name = "123abc";
                repository.Update(currentTag);
                Assert.Equal("123abc", repository.GetById(id).Name);
                repository.Dispose();
            }
        }
        
        [Fact]
        public void Should_Delete_Tag_In_Db()
        {
            var fakeContext = new FakeContext("DeleteTag");
            fakeContext.FillWith<Tag>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new TagRepository(context);
                var currentCount = context.Tags.Count();
                var newTag = new Tag();
                newTag.Name = "Test Tag";
                // newTag.Id = 123456798;
                repository.Create(newTag);
                var idToDelete = (from u in repository.GetAll()
                    where u.Id == newTag.Id
                    select u.Id).FirstOrDefault();

                Assert.Equal(currentCount + 1, repository.GetAll().ToList().Count);
                repository.Delete(idToDelete);
                Assert.Equal(currentCount, repository.GetAll().ToList().Count());
                repository.Dispose();
            }
        }
    }
}