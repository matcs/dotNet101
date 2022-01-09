using dotNet101.Model;
using dotNet101.Service;
using Xunit;
using dotNet101.Unit.Test.SharedDatabase;

namespace dotNet101.Unit.Test.Services
{
    public class StudentServiceTest : IClassFixture<SharedDatabaseFixture>
    {
        public StudentServiceTest(SharedDatabaseFixture fixture) => Fixture = fixture;

        public SharedDatabaseFixture Fixture { get; }

        [Fact]
        public async void AddStudentTest()
        {
            using (var transaction = Fixture.Connection.BeginTransaction())
            {
                using (var context = Fixture.CreateContext(transaction))
                {
                    var service = new StudentsService(context);

                    var student = await service.AddStudent(new Student() { Name = "Yuta", Grade = "Special" });

                    Assert.Equal("Yuta", student.Name);
                    //Assert.(m => m.Add(It.IsAny<Livro>()), Times.Once());
                    //mockContext.Verify(m => m.SaveChanges(), Times.Once());
                }
            }
        }
    }
}
