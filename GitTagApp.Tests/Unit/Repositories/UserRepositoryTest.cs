using System.Linq;
using GitTagApp.Entities;
using GitTagApp.Repositories;
using GitTagApp.Repositories.Context;
using GitTagApp.Tests.Unit.Entities;
using GitTagApp.Tests.Unit.Repositories.Context;
using Xunit;

namespace GitTagApp.Tests.Unit.Repositories
{
    public class UserRepositoryTest
    {
        [Fact]
        public void Should_Return_All_Users_In_Db()
        {
            var fakeContext = new FakeContext("GetAllUsers");
            fakeContext.FillWith<User>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var userCountIndDb = context.Users.Count();
                var repository = new UserRepository(context);
                
                Assert.Equal(userCountIndDb, repository.GetAll().Count());
                repository.Dispose();
            }
        }
        
        [Theory]
        [InlineData(8916833)]
        [InlineData(216546)]
        [InlineData(121364)]
        public void Should_Return_Right_User_When_Find_By_Id_In_Db(long id)
        {
            var fakeContext = new FakeContext("UserById");
            fakeContext.FillWith<User>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var expected = fakeContext.GetFakeData<User>().Find(x => x.Id == id);
                var repository = new UserRepository(context);
                var actual = repository.GetById(id);
                
                Assert.Equal(expected, actual, new UserIdComparer());
                repository.Dispose();
            }
        }
        
        [Fact]
        public void Should_Save_New_User_To_Db()
        {
            var fakeContext = new FakeContext("AddNewUser");

            var fakeUser = new User();
            fakeUser.Name = "full name";
            fakeUser.Id = 1234;
            
            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new UserRepository(context);
                repository.Create(fakeUser);

                var createdUser = repository.GetById(1234);
                
                Assert.NotEqual(0, fakeUser.Id);
                Assert.Equal("full name", createdUser.Name);
                Assert.Equal(1234, createdUser.Id);
                repository.Dispose();
            }
        }
        
        [Theory]
        [InlineData(8916833)]
        [InlineData(216546)]
        [InlineData(121364)]
        public void Should_Update_User_In_Db(long id)
        {
            var fakeContext = new FakeContext("UpdateUser");
            fakeContext.FillWith<User>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new UserRepository(context);
                var currentUser = repository.GetById(id);

                currentUser.Name = "123abc";
                repository.Update(currentUser);
                Assert.Equal("123abc", repository.GetById(id).Name);
                repository.Dispose();
            }
        }
        
        [Fact]
        public void Should_Delete_User_In_Db()
        {
            var fakeContext = new FakeContext("DeleteUser");
            fakeContext.FillWith<User>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new UserRepository(context);
                var currentCount = context.Users.Count();
                var newUser = new User();
                newUser.Name = "Test User";
                newUser.Id = 123456798;
                repository.Create(newUser);
                var idToDelete = (from u in repository.GetAll()
                    where u.Id == newUser.Id
                    select u.Id).FirstOrDefault();

                Assert.Equal(currentCount + 1, repository.GetAll().ToList().Count);
                repository.Delete(idToDelete);
                Assert.Equal(currentCount, repository.GetAll().ToList().Count());
                repository.Dispose();
            }
        }
    }
}