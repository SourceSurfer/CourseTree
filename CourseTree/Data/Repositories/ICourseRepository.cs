using Data.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    // <summary>
    /// Абстракция доступа к таблицам Courses / Modules.
    /// </summary>
    public interface ICourseRepository
    {
        Task<IEnumerable<CourseDto>> GetAllAsync();
        Task<IEnumerable<CourseDto>> GetFilteredAsync(
            string? subject,
            string? grade,
            string? genre);
    }
}
