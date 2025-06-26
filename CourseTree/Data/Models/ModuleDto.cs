using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public sealed class ModuleDto
    {
        public int Id { get; init; }
        public int CourseId { get; init; }
        public int? ParentId { get; init; }
        public string? Num { get; init; }
        public string? Title { get; init; }
        public int? Order { get; init; }

        public List<ModuleDto> Children { get; set; } = new();
    }
}
