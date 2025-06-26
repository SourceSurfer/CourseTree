using Data.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    internal interface ICourseRepository
    {
        Task<IEnumerable<CourseDto>> GetAllAsync();
        Task<IEnumerable<CourseDto>> GetFilteredAsync(string? subject, string? grade, string? genre);

    }
}
