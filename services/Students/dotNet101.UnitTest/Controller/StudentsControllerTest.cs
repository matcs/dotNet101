using System.Threading.Tasks;
using dotNet101.Controllers;
using dotNet101.Model;
using dotNet101.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace dotNet101.UnitTest.Controller
{
    [Trait("CATEGORY","Controller Crud")]
    public class StudentsControllerTest
    {
        [Fact]
        public async Task GetStudentbyId()
        {
            var mockContext = new Mock<IStudentService>();
            mockContext.Setup(c => c.GetStudentById(It.IsAny<int>()))
                               .ReturnsAsync(() => new Student() { StudentId = 1, Name = "Itadori", Grade = "A"});
            
            var controller = new StudentsController(mockContext.Object);

            var result = await controller.GetStudent(1);

            Assert.Equal("Itadori", result.Value.Name);

            mockContext.Verify(m => m.GetStudentById(1), Times.Once());
        }

        [Fact]
        public async Task GetShouldReturnNotFoundForMissingStudent()
        {
            var mockContext = new Mock<IStudentService>();
            mockContext.Setup(c => c.GetStudentById(It.IsAny<int>()))
                               .ReturnsAsync(() => null);
            var controller = new StudentsController(mockContext.Object);
            int id = -1;

            var result = await controller.GetStudent(id);

            Assert.IsType<NotFoundResult> (result.Result);
        }

        [Fact]
        public async Task PostNewStudentReturnCreated()
        {
            var mockContext = new Mock<IStudentService>();
            mockContext.Setup(c => c.AddStudent(It.IsAny<Student>()))
                               .ReturnsAsync(new Student() { StudentId = 4, Name = "Name", Grade = "Special"});
            var controller = new StudentsController(mockContext.Object);

            var result = await controller.PostStudent(new Student());

            Assert.IsType<CreatedAtActionResult>(result.Result);
        }


        [Fact]
        public async Task PostNewStudentReturnShouldBeBadRequest()
        {
            var mockContext = new Mock<IStudentService>();
            Student student = null;
            mockContext.Setup(c => c.AddStudent(It.IsAny<Student>()))
                               .ReturnsAsync(student);

            var controller = new StudentsController(mockContext.Object);

            var result = await controller.PostStudent(new Student());

            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task PutStudentReturnShouldBeNoContent()
        {
            var mockContext = new Mock<IStudentService>();
            mockContext.Setup(c => c.UpdateStudent(It.IsAny<Student>()))
                               .ReturnsAsync(new Student() { StudentId = 1, Name = "Itadori", Grade = "Special" });

            var controller = new StudentsController(mockContext.Object);

            var result = await controller.PutStudent(1,new Student() { StudentId = 1, Name = "Itadori", Grade = "Special"});

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutStudentWithNotEqualIdsReturnShouldBeBadRequest()
        {
            var mockContext = new Mock<IStudentService>();
            mockContext.Setup(c => c.UpdateStudent(It.IsAny<Student>()))
                               .ReturnsAsync(new Student() { StudentId = 2, Name = "Itadori", Grade = "Special" });

            var controller = new StudentsController(mockContext.Object);

            var result = await controller.PutStudent(2, new Student() { StudentId = 1, Name = "Itadori", Grade = "Special" });

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteStudenReturnShouldBeNoContent()
        {
            var mockContext = new Mock<IStudentService>();
            mockContext.Setup(c => c.GetStudentById(It.IsAny<int>()))
                                .ReturnsAsync(new Student() { StudentId = 1, Name = "Itadori", Grade = "B" });
            mockContext.Setup(c => c.DeleteStudent(It.IsAny<int>()))
                               .ReturnsAsync(new Student() { StudentId = 1, Name = "Itadori", Grade = "A" });

            var controller = new StudentsController(mockContext.Object);

            var result = await controller.DeleteStudent(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteStudentReturnShouldBeNotFound()
        {
            var mockContext = new Mock<IStudentService>();
            Student student = null;
            mockContext.Setup(c => c.GetStudentById(It.IsAny<int>()))
                              .ReturnsAsync(student);
           
            var controller = new StudentsController(mockContext.Object);

            var result = await controller.DeleteStudent(5);

            Assert.IsType<NotFoundResult>(result);
        }

    }
}
