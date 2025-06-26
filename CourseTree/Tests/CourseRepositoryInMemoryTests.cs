using Data.Repositories;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tests.Helpers;

namespace Tests
{
    public class CourseRepositoryInMemoryTests : IDisposable
    {
        private readonly IDbConnection _connection;
        private readonly CourseRepository _repository;

        public CourseRepositoryInMemoryTests()
        {
            _connection = InMemoryDatabase.CreateConnection();
            InMemoryDatabase.SeedTestData(_connection);
            _repository = new CourseRepository(_connection);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllCourses()
        {
            // Act
            var courses = await _repository.GetAllAsync();

            // Assert
            Assert.NotNull(courses);
            Assert.Equal(4, courses.Count());
        }

        [Fact]
        public async Task GetAllAsync_CoursesSortedByTitle()
        {
            // Act
            var courses = (await _repository.GetAllAsync()).ToList();

            // Assert
            Assert.Equal("Биология 7 класс", courses[0].Title);
            Assert.Equal("География 8 класс", courses[1].Title);
            Assert.Equal("Математика 5 класс", courses[2].Title);
            Assert.Equal("Химия 9 класс", courses[3].Title);
        }       

        [Fact]
        public async Task GetFilteredAsync_FiltersBySubject()
        {
            // Act
            var courses = await _repository.GetFilteredAsync("Биология", null, null);

            // Assert
            Assert.Single(courses);
            Assert.Equal("Биология 7 класс", courses.First().Title);
        }

        [Fact]
        public async Task GetFilteredAsync_FiltersByGrade()
        {
            // Act
            var courses = await _repository.GetFilteredAsync(null, "8", null);

            // Assert
            Assert.Single(courses);
            Assert.Equal("География 8 класс", courses.First().Title);
        }

        [Fact]
        public async Task GetFilteredAsync_FiltersByGenre()
        {
            // Act
            var courses = await _repository.GetFilteredAsync(null, null, "Учебник");

            // Assert
            Assert.Equal(2, courses.Count());
            Assert.All(courses, c => Assert.Equal("Учебник", c.Genre));
        }

        [Fact]
        public async Task GetFilteredAsync_CombinedFilters()
        {
            // Act
            var courses = await _repository.GetFilteredAsync("Биология", "7", "Рабочая тетрадь");

            // Assert
            Assert.Single(courses);
            var course = courses.First();
            Assert.Equal("Биология 7 класс", course.Title);
            Assert.Equal("Биология", course.Subject);
            Assert.Equal("7", course.Grade);
            Assert.Equal("Рабочая тетрадь", course.Genre);
        }

        [Fact]
        public async Task GetFilteredAsync_NoMatchingCourses()
        {
            // Act
            var courses = await _repository.GetFilteredAsync("Физика", null, null);

            // Assert
            Assert.Empty(courses);
        }

        [Fact]
        public async Task ModulesWithNumAndTitle_ConcatenatedCorrectly()
        {
            // Act
            var courses = await _repository.GetAllAsync();
            var mathCourse = courses.First(c => c.Subject == "Математика");
            var module = mathCourse.Modules.First();

            // Assert
            Assert.Equal("1.", module.Num);
            Assert.Equal("Натуральные числа", module.Title);
            // В представлении они должны отображаться как "1. Натуральные числа"
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}

