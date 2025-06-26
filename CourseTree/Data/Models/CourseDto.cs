using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public sealed class CourseDto
    {
        public int Id { get; init; }
        public string? Title { get; init; }
        public string? Subject { get; init; }
        public string? Grade { get; init; }
        public string? Genre { get; init; }

        public List<ModuleDto> Modules { get; set; } = new();
    }
}
