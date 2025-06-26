using Data.Models;

namespace Web.Models
{
    public class CourseFilterViewModel
    {
        public IEnumerable<CourseDto> Courses { get; set; }
        public string? SelectedSubject { get; set; }
        public string? SelectedGrade { get; set; }
        public string? SelectedGenre { get; set; }

        public List<string> Subjects { get; set; }
        public List<string> Grades { get; set; }
        public List<string> Genres { get; set; }
    }
}
