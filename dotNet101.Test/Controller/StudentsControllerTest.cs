using dotNet101.Model;
using Xunit;
using dotNet101.UnitTest.SharedDatabase;
using dotNet101.Controllers;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using dotNet101.Service.IService;
using Moq;
using System.Collections.Generic;

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
        public async Task DetailsShouldReturnNotFoundForMissingStudent()
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

    }
}
