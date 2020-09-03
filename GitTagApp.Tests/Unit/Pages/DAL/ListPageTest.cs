using GitTagApp.Repositories.Context;
using Xunit;

namespace GitTagApp.Tests.Unit.Pages.DAL
{
    public class ListPageTest
    {
        [Fact]
        public void Should_Return_Correct_Page()
        {
            using (var context = new MainContext(Utilities.TestDbContextOptions()))
            {
                
            }

        } 
    }
}