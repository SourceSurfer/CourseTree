using Data.Models;
using Data.Repositories;

using Microsoft.AspNetCore.Mvc;

using Moq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Web.Controllers.Api;

namespace Tests
{
    public class ApiControllerTests
    {
        private readonly Mock<ICourseRepository> _mockRepo;
        private readonly CoursesApiController _controller;

        public ApiControllerTests()
        {
            _mockRepo = new Mock<ICourseRepository>();
            _controller = new CoursesApiController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetCourses_ReturnsOkResult()
        {
            // Arrange
            var courses = new List<CourseDto>
            {
                new CourseDto { Id = 1, Title = "Test Course" }
            };
            _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(courses);

            // Act
            var result = await _controller.GetCourses(null, null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCourses = Assert.IsAssignableFrom<IEnumerable<CourseDto>>(okResult.Value);
            Assert.Single(returnedCourses);
        }       
    }
}
