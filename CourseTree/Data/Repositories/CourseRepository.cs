using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

using Dapper;

using Data.Models;

namespace Data.Repositories
{
    public sealed class CourseRepository : ICourseRepository
    {
        private readonly IDbConnection _db;
        public CourseRepository(IDbConnection db) => _db = db;

        private const string SqlGetAll = @"
            SELECT 
                c.Id, c.Title, c.Subject, c.Grade, c.Genre,
                m.Id, m.CourseId, m.ParentId, m.Num, m.Title, m.[Order]
            FROM Courses c
            LEFT JOIN Modules m ON m.CourseId = c.Id
            ORDER BY c.Title ASC, m.[Order], m.Id";

        private const string SqlGetFiltered = @"
            SELECT 
                c.Id, c.Title, c.Subject, c.Grade, c.Genre,
                m.Id, m.CourseId, m.ParentId, m.Num, m.Title, m.[Order]
            FROM Courses c
            LEFT JOIN Modules m ON m.CourseId = c.Id
            WHERE (@subject IS NULL OR c.Subject = @subject)
              AND (@grade IS NULL OR c.Grade = @grade)
              AND (@genre IS NULL OR c.Genre = @genre)
            ORDER BY c.Title ASC, m.[Order], m.Id";

        public Task<IEnumerable<CourseDto>> GetAllAsync() =>
            ExecuteQueryAsync(SqlGetAll, null);

        public Task<IEnumerable<CourseDto>> GetFilteredAsync(
            string? subject,
            string? grade,
            string? genre)
        {
            return ExecuteQueryAsync(SqlGetFiltered, new { subject, grade, genre });
        }

        private async Task<IEnumerable<CourseDto>> ExecuteQueryAsync(string sql, object? param)
        {
            var courseLookup = new Dictionary<int, CourseDto>();
            var moduleLookup = new Dictionary<int, ModuleDto>();

            await _db.QueryAsync<CourseDto, ModuleDto, CourseDto>(
                sql,
                map: (course, mod) =>
                {
                    if (!courseLookup.TryGetValue(course.Id, out var courseEntry))
                    {
                        courseEntry = course;
                        courseLookup.Add(courseEntry.Id, courseEntry);
                    }

                    if (mod != null && mod.Id > 0)
                    {
                        if (!moduleLookup.TryGetValue(mod.Id, out var moduleEntry))
                        {
                            moduleEntry = mod;
                            moduleLookup.Add(moduleEntry.Id, moduleEntry);
                        }

                        if (mod.ParentId.HasValue && moduleLookup.TryGetValue(mod.ParentId.Value, out var parent))
                        {
                            if (!parent.Children.Any(c => c.Id == moduleEntry.Id))
                            {
                                parent.Children.Add(moduleEntry);
                            }
                        }
                        else if (!courseEntry.Modules.Any(m => m.Id == moduleEntry.Id))
                        {
                            courseEntry.Modules.Add(moduleEntry);
                        }
                    }

                    return courseEntry;
                },
                param: param,
                splitOn: "Id"
            );

            // Сортируем модули по Order
            foreach (var course in courseLookup.Values)
            {
                SortModules(course.Modules);
            }

            return courseLookup.Values.OrderBy(c => c.Title);
        }

        private void SortModules(List<ModuleDto> modules)
        {
            modules.Sort((a, b) => (a.Order ?? int.MaxValue).CompareTo(b.Order ?? int.MaxValue));
            foreach (var module in modules)
            {
                if (module.Children.Any())
                {
                    SortModules(module.Children);
                }
            }
        }
    }
}
