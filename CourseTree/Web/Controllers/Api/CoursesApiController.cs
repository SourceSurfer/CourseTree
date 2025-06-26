using Data.Repositories;

using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesApiController : ControllerBase
    {
        private readonly ICourseRepository _repo;

        public CoursesApiController(ICourseRepository repo) => _repo = repo;

        [HttpGet]
        public async Task<IActionResult> GetCourses(string? subject, string? grade, string? genre)
        {
            var courses = string.IsNullOrEmpty(subject) && string.IsNullOrEmpty(grade) && string.IsNullOrEmpty(genre)
                ? await _repo.GetAllAsync()
                : await _repo.GetFilteredAsync(subject, grade, genre);

            return Ok(courses.OrderBy(c => c.Title));
        }

        [HttpGet("filters")]
        public async Task<IActionResult> GetFilters()
        {
            var courses = await _repo.GetAllAsync();

            return Ok(new
            {
                subjects = courses.Select(c => c.Subject).Distinct().Where(s => !string.IsNullOrEmpty(s)).OrderBy(s => s),
                grades = courses.Select(c => c.Grade).Distinct().Where(g => !string.IsNullOrEmpty(g)).OrderBy(g => g),
                genres = courses.Select(c => c.Genre).Distinct().Where(g => !string.IsNullOrEmpty(g)).OrderBy(g => g)
            });
        }
    }
}
