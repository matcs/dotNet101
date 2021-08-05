using dotNet101.Controllers;
using dotNet101.Data;
using dotNet101.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using dotNet101.Test.SetUp;

namespace dotNet101.Test.Controller
{
    [Trait("CATEGORY","Controller Crud")]
    public class TestStudentsController : IClassFixture<TestsFixture>
    {
        public TestsFixture _testsFixture { get; }

        public TestStudentsController(TestsFixture data)
        {
            this._testsFixture = data;
        }

        [Fact(DisplayName = "Get All Students")]
        public async void GetListOfStudens()
        {
            using (var context = new ApplicationDbContext(_testsFixture.optionsBuilder))
            {
                var controller = new StudentsController(context);
                var result = await controller.GetStudents();

                Assert.Equal(3, result.Value.Count());
            }
        }

        [Fact(DisplayName ="Get Student by Id")]
        public async void GetStudentById()
        {
            using (var context = new ApplicationDbContext(_testsFixture.optionsBuilder))
            {
                var controller = new StudentsController(context);
                var result = await controller.GetStudent(1);

                Assert.Equal(1, result.Value.StudentId);
            }
        }
    }
}
