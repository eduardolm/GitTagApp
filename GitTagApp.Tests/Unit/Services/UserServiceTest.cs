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
    public class UserServiceTest
    {
        [Fact]
        public void Should_Return_All_Users()
        {
            var fakeContext = new FakeContext("GetAllUsers");
            fakeContext.FillWith<User>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var userCountIndDb = context.Users.Count();
                var repository = new UserRepository(context);
                var service = new UserService(repository);
                
                Assert.Equal(userCountIndDb, service.GetAll().Count());
                repository.Dispose();
            }
        }
        
        [Theory]
        [InlineData(8916833)]
        [InlineData(216546)]
        [InlineData(121364)]
        [InlineData(4563214)]
        [InlineData(7896541)]
        public void Should_Return_Right_User_When_Find_By_Id(long id)
        {
            var fakeContext = new FakeContext("UserById");
            fakeContext.FillWith<User>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var expected = fakeContext.GetFakeData<User>().Find(x => x.Id == id);

                var repository = new UserRepository(context);
                var service = new UserService(repository);
                var actual = service.GetById(id);
                var tryZeroIdUser = service.GetById(0);

                Assert.Null(tryZeroIdUser);
                Assert.Equal(expected, actual, new UserIdComparer());
                repository.Dispose();
            }
        }

        [Fact]
        public void Should_Create_New_User()
        {
            var fakeContext = new FakeContext("CreateNewUser");

            var fakeUser = new User();
            fakeUser.Name = "full name";
            fakeUser.Id = 1234;

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new UserRepository(context);
                
               var service = new UserService(repository);
                var actual = service.Create(fakeUser);

                Assert.Equal("Item successfully created", actual);
                repository.Dispose();
            }
        }

        [Theory]
        [InlineData(8916833)]
        [InlineData(216546)]
        [InlineData(121364)]
        public void Should_Update_Existing_User(long id)
        {
            var fakeContext = new FakeContext("UpdateUser");
            fakeContext.FillWith<User>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new UserRepository(context);
                var service = new UserService(repository);
                var currentUser = service.GetById(id);

                currentUser.Id = 1234;
                service.Update(currentUser);
                Assert.Equal(1234, service.GetById(id).Id);
                Assert.NotNull(service.GetById(id));
                repository.Dispose();
            }
        }

        [Fact]
        public void Should_Delete_User()
        {
            var fakeContext = new FakeContext("DeleteUser");
            fakeContext.FillWith<User>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new UserRepository(context);
                var service = new UserService(repository);
                var currentCount = context.Users.Count();
                var newUser = new User();
                newUser.Name = "New user";
                newUser.Id = 1234;
                service.Create(newUser);
                var createdUser = (from u in service.GetAll()
                    where u.Id != 0
                    select u).FirstOrDefault();

                Assert.NotEqual(0, currentCount);
                service.Delete(createdUser.Id);
                Assert.Equal(currentCount,context.Users.Count());
                repository.Dispose();
            }
        }

        [Theory]
        [InlineData(8916833)]
        [InlineData(216546)]
        [InlineData(121364)]
        [InlineData(4563214)]
        [InlineData(7896541)]
        public void Should_Not_Create_Repeated_User(long id)
        {
            var fakeContext = new FakeContext("DeleteUser");
            fakeContext.FillWith<User>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new UserRepository(context);
                var service = new UserService(repository);

                var newUser = new User();
                newUser.Name = "Test Name";
                newUser.Id = 7;
                service.Create(newUser);
                var existingUser = (from u in service.GetAll()
                    where u.Id == id
                    select u).FirstOrDefault();

                var anotherUser = new User();
                anotherUser.Name = "Test Name 2";
                anotherUser.Id = 7;
                service.Create(anotherUser);
                
                Assert.NotEqual(newUser.Id, existingUser.Id);
                Assert.NotEqual(newUser.Name, existingUser.Name);
                repository.Dispose();
            }
        }
    }
}