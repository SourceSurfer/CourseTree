using Microsoft.Data.Sqlite;
using System.Data;
using Dapper;
using SQLitePCL;

namespace Tests.Helpers
{
    public static class InMemoryDatabase
    {

        static InMemoryDatabase()
        {
            // Инициализация SQLite
            Batteries.Init();
        }
        public static IDbConnection CreateConnection()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            // Создаем таблицы
            connection.Execute(@"
                CREATE TABLE Courses (
                    Id INTEGER PRIMARY KEY,
                    Title VARCHAR(1000),
                    Description TEXT,
                    Status VARCHAR(50),
                    ExternalId VARCHAR(255),
                    Hash VARCHAR(64),
                    Subject VARCHAR(255),
                    Grade VARCHAR(50),
                    Genre VARCHAR(50)
                )
            ");

            connection.Execute(@"
                CREATE TABLE Modules (
                    Id INTEGER PRIMARY KEY,
                    CourseId INTEGER,
                    Title VARCHAR(1000),
                    [Order] INTEGER,
                    Href VARCHAR(1000),
                    ParentId INTEGER,
                    ExternalId VARCHAR(255),
                    ContentType VARCHAR(50),
                    Num VARCHAR(50)
                )
            ");

            return connection;
        }

        public static void SeedTestData(IDbConnection connection)
        {
            // Вставляем тестовые курсы
            connection.Execute(@"
                INSERT INTO Courses (Id, Title, Subject, Grade, Genre) VALUES 
                (1, 'Математика 5 класс', 'Математика', '5', 'Учебник'),
                (2, 'Биология 7 класс', 'Биология', '7', 'Рабочая тетрадь'),
                (3, 'География 8 класс', 'География', '8', 'Рабочая тетрадь'),
                (4, 'Химия 9 класс', 'Химия', '9', 'Учебник')
            ");

            // Вставляем модули для курса 1
            connection.Execute(@"
                INSERT INTO Modules (Id, CourseId, Title, [Order], ParentId, Num) VALUES 
                (1, 1, 'Натуральные числа', 1, NULL, '1.'),
                (2, 1, 'Сложение и вычитание', 1, 1, '1.1.'),
                (3, 1, 'Умножение и деление', 2, 1, '1.2.'),
                (4, 1, 'Дроби', 2, NULL, '2.'),
                (5, 1, 'Обыкновенные дроби', 1, 4, '2.1.')
            ");

            // Вставляем модули для курса 2
            connection.Execute(@"
                INSERT INTO Modules (Id, CourseId, Title, [Order], ParentId, Num) VALUES 
                (6, 2, 'Строение животных', 1, NULL, '1.'),
                (7, 2, 'Клеточное строение', 1, 6, '1.1.'),
                (8, 2, 'Ткани', 2, 6, '1.2.')
            ");
        }
    }
}
