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
    public class TagServiceTest
    {
        [Fact]
        public void Should_Return_All_Tags()
        {
            var fakeContext = new FakeContext("GetAllTags");
            fakeContext.FillWith<Tag>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var userCountIndDb = context.Tags.Count();
                var repository = new TagRepository(context);
                var service = new TagService(repository);
                
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
        public void Should_Return_Right_Tag_When_Find_By_Id(long id)
        {
            var fakeContext = new FakeContext("TagById");
            fakeContext.FillWith<Tag>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var expected = fakeContext.GetFakeData<Tag>().Find(x => x.Id == id);

                var repository = new TagRepository(context);
                var service = new TagService(repository);
                var actual = service.GetById(id);
                var tryZeroIdTag = service.GetById(0);

                Assert.Null(tryZeroIdTag);
                Assert.Equal(expected, actual, new TagIdComparer());
                repository.Dispose();
            }
        }

        [Fact]
        public void Should_Create_New_Tag()
        {
            var fakeContext = new FakeContext("CreateNewTag");

            var fakeTag = new Tag();
            fakeTag.Name = "tag name";
            fakeTag.Id = 1234;

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new TagRepository(context);
                
               var service = new TagService(repository);
                var actual = service.Create(fakeTag);

                Assert.Equal("Item successfully created", actual);
                repository.Dispose();
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void Should_Update_Existing_Tag(long id)
        {
            var fakeContext = new FakeContext("UpdateTag");
            fakeContext.FillWith<Tag>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new TagRepository(context);
                var service = new TagService(repository);
                var currentTag = service.GetById(id);

                currentTag.Id = 1234;
                service.Update(currentTag);
                Assert.Equal(1234, service.GetById(id).Id);
                repository.Dispose();
            }
        }

        [Fact]
        public void Should_Delete_Tag()
        {
            var fakeContext = new FakeContext("DeleteTag");
            fakeContext.FillWith<Tag>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new TagRepository(context);
                var service = new TagService(repository);
                var currentCount = context.Tags.Count();
                var newTag = new Tag();
                newTag.Name = "New Tag";
                newTag.Id = 1234;
                service.Create(newTag);
                var createdTag = (from u in service.GetAll()
                    where u.Id != 0
                    select u).FirstOrDefault();

                Assert.NotEqual(0, currentCount);
                service.Delete(createdTag.Id);
                Assert.Equal(currentCount,context.Tags.Count());
                repository.Dispose();
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void Should_Not_Create_Repeated_Tag(long id)
        {
            var fakeContext = new FakeContext("DeleteTag");
            fakeContext.FillWith<Tag>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new TagRepository(context);
                var service = new TagService(repository);

                var newTag = new Tag();
                newTag.Name = "Test Name";
                newTag.Id = 6;
                service.Create(newTag);
                var existingTag = (from u in service.GetAll()
                    where u.Id == id
                    select u).FirstOrDefault();

                var anotherTag = new Tag();
                anotherTag.Name = "Test Name 2";
                anotherTag.Id = 6;
                var anotherResult = service.Create(anotherTag);
                
                Assert.NotEqual(newTag.Id, existingTag.Id);
                Assert.NotEqual(newTag.Name, existingTag.Name);
                repository.Dispose();
            }
        }
    }
}