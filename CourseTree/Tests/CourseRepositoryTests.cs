using Data.Repositories;

using FluentAssertions;

using Microsoft.Data.SqlClient;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class CourseRepositoryTests
    {
        private readonly ICourseRepository _repo;

        public CourseRepositoryTests()
        {
            // строку подключения возьмите из appsettings.json или задайте явно
            IDbConnection conn = new SqlConnection(
                "Data Source=DESKTOP-6JTHA04\\SQLEXPRESS;Initial Catalog=Test;Integrated Security=True;Trust Server Certificate=True");
            _repo = new CourseRepository(conn);
        }

        [Fact]
        public async Task GetAllAsync_Returns_Courses_With_Modules()
        {
            var courses = await _repo.GetAllAsync();

            courses.Should().NotBeEmpty();
            courses.First().Modules.Should().NotBeNull();
        }
    }
}
