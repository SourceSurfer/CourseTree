using Data.Models;
using Data.Repositories;

using Microsoft.AspNetCore.Mvc;

using Web.Models;

namespace Web.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICourseRepository _repo;

        public CoursesController(ICourseRepository repo) => _repo = repo;

        // Ступень 1 и 2: Отображение с фильтрацией
        public async Task<IActionResult> Index(string? subject, string? grade, string? genre)
        {
            var allCourses = await _repo.GetAllAsync();

            var viewModel = new CourseFilterViewModel
            {
                Courses = string.IsNullOrEmpty(subject) && string.IsNullOrEmpty(grade) && string.IsNullOrEmpty(genre)
                    ? allCourses
                    : await _repo.GetFilteredAsync(subject, grade, genre),

                SelectedSubject = subject,
                SelectedGrade = grade,
                SelectedGenre = genre,

                Subjects = allCourses.Select(c => c.Subject).Distinct().Where(s => !string.IsNullOrEmpty(s)).OrderBy(s => s).ToList(),
                Grades = allCourses.Select(c => c.Grade).Distinct().Where(g => !string.IsNullOrEmpty(g)).OrderBy(g => g).ToList(),
                Genres = allCourses.Select(c => c.Genre).Distinct().Where(g => !string.IsNullOrEmpty(g)).OrderBy(g => g).ToList()
            };

            return View(viewModel);
        }

        // Ступень 3: AJAX версия
        public IActionResult Ajax()
        {
            return View();
        }
    }
}
