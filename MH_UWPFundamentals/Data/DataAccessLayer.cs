using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using MH_UWPFundamentals.Models;
using MH_UWPFundamentals.Extensions;

namespace MH_UWPFundamentals.Data
{
    internal static class DataAccessLayer
    {
        private static string horsesTableQuery = "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='Horse';";
        private static bool horsesTableExists;
        private static string studentsTableQuery = "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='Student';";
        private static bool studentsTableExists;
        private static string lessonsTableQuery = "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='Lesson';";
        private static bool lessonsTableExists;

        //DATABASE PATH
        private static string dbPath = string.Empty;
        private static string DbPath
        {
            get
            {
                if (string.IsNullOrEmpty(dbPath))
                {
                    dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Storage.sqlite");
                }

                return dbPath;
            }
        }

        //DATABASE CONNECTION
        private static SQLiteConnection DbConnection
        {
            get
            {
                return new SQLiteConnection(new SQLitePlatformWinRT(), DbPath);
            }
        }

        //CREATES DATABASE AND TABLE
        public static async Task CreateDatabase()
        {
            //create a new connection
            using (var db = DbConnection)
            {
                //activate tracing
                db.TraceListener = new DebugTraceListener();

                //check if tables exists
                horsesTableExists = db.ExecuteScalar<bool>(horsesTableQuery);
                studentsTableExists = db.ExecuteScalar<bool>(studentsTableQuery);
                lessonsTableExists = db.ExecuteScalar<bool>(lessonsTableQuery);

                if (TablesExist())
                {
                    List<string> myTables = new List<string> { "Horse", "Student", "Lesson" };

                    foreach (string tb in myTables)
                    {
                        SQLiteCommand command = db.CreateCommand(string.Format("DROP TABLE {0};", tb));
                        command.ExecuteNonQuery();
                    }
                }

                //HORSE TABLE
                //create the horse table if it does not exist
                var horseTable = db.CreateTable<Horse>();
                var horseTableInfo = db.GetMapping(typeof(Horse));
                //populate horse table with data
                Horse horse = new Horse();
                horse.Id = 1;
                horse.Name = "Sargent Pepper";
                horse.Age = 12;
                horse.Discipline = "Polocrosse";
                horse.Notes = "Some important and noteworthy information about this specific horse: A former police horse, he now struggles with arthritis in his front legs. This is believed to have been caused by the car that hit him.";
                StorageFile file = await StorageFile.GetFileFromPathAsync(Path.Combine(Package.Current.InstalledLocation.Path, @"Assets\Images\horse1.jpg"));
                horse.Picture = await file.AsByteArray();
                var i = db.InsertOrReplace(horse);

                horse = new Horse();
                horse.Id = 2;
                horse.Name = "Damper";
                horse.Age = 18;
                horse.Discipline = "Show Jumping";
                horse.Notes = "All you need to know about this horse: she loves to go fast.";
                file = await StorageFile.GetFileFromPathAsync(Path.Combine(Package.Current.InstalledLocation.Path, @"Assets\Images\horse2.jpg"));
                horse.Picture = await file.AsByteArray();
                i = db.InsertOrReplace(horse);

                horse = new Horse();
                horse.Id = 3;
                horse.Name = "Love Story";
                horse.Age = 15;
                horse.Discipline = "Show Jumping";
                horse.Notes = "This is the best horse to use for lessons. She is comfortable and well-trained. Never forget to give her a carrot. Ever.";
                file = await StorageFile.GetFileFromPathAsync(Path.Combine(Package.Current.InstalledLocation.Path, @"Assets\Images\horse3.jpg"));
                horse.Picture = await file.AsByteArray();
                i = db.InsertOrReplace(horse);
                
                //STUDENT TABLE
                //create the student table if it does not exist
                var studentTable = db.CreateTable<Student>();
                var studentTableInfo = db.GetMapping(typeof(Student));
                //populate horse table with data
                Student student = new Student();
                student.Id = 1;
                student.Name = "Phillip Anders";
                student.Number = "0132882413";
                student.Notes = "Enjoys show jumping, polocrosse and racing. A real dare devil...he will enjoy riding Damper.";
                i = db.InsertOrReplace(student);

                student = new Student();
                student.Id = 2;
                student.Name = "Roger Bollier";
                student.Number = "0789632544";
                student.Notes = "Have not met this student yet.";
                i = db.InsertOrReplace(student);

                student = new Student();
                student.Id = 3;
                student.Name = "Sandy Hersheem";
                student.Number = "0113937741";
                student.Notes = "Her focus is polocrosse, and she is not a fast learner, so patience is required.";
                i = db.InsertOrReplace(student);

                //LESSON TABLE
                //create the lesson table if it does not exist
                var lessonTable = db.CreateTable<Lesson>();
                var lessonTableInfo = db.GetMapping(typeof(Lesson));
                //populate horse table with data
                Lesson lesson = new Lesson();
                lesson.Id = 1;
                lesson.LessonDate = new DateTime(2017, 2, 5, 0, 0, 0, DateTimeKind.Utc);
                lesson.Cost = 240;
                lesson.Focus = "Jumping over poles";
                lesson.Notes = "Roger and Phillip will be riding together.";
                i = db.InsertOrReplace(lesson);

                lesson = new Lesson();
                lesson.Id = 2;
                lesson.LessonDate = new DateTime(2017, 2, 6, 0, 0, 0, DateTimeKind.Utc);
                lesson.Cost = 240;
                lesson.Focus = "Jump over course of jumps";
                lesson.Notes = "This will be Roger and Phillip's second riding lesson together.";
                i = db.InsertOrReplace(lesson);

                lesson = new Lesson();
                lesson.Id = 3;
                lesson.LessonDate = new DateTime(2017, 2, 8, 0, 0, 0, DateTimeKind.Utc);
                lesson.Cost = 120;
                lesson.Focus = "Catching the ball for polocrosse";
                lesson.Notes = "Sandy's third lesson with us. She will have learnt all the core components of polocrosse by the next month.";
                i = db.InsertOrReplace(lesson);
            }
        }

        //DATABASE FUNCTIONS
        private static bool TablesExist()
        {
            return horsesTableExists && studentsTableExists && lessonsTableExists;
        }

        //HORSE FUNCTIONS
        //RETRIEVE ALL HORSES FROM TABLE
        public static List<Horse> GetAllHorses()
        {
            List<Horse> models;

            //create a new connection
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                //activate tracing
                db.TraceListener = new DebugTraceListener();

                models = (from p in db.Table<Horse>()
                          select p).ToList();
            }

            return models;
        }
        //RETRIEVE A SPECIFIC HORSE BY ID
        public static Horse GetHorseById(int Id)
        {
            //create a new connection
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                //activate tracing
                db.TraceListener = new DebugTraceListener();

                Horse m = (from p in db.Table<Horse>()
                            where p.Id == Id
                            select p).FirstOrDefault();
                return m;
            }
        }
        //SAVE A SPECIFIC HORSE TO TABLE
        public static void SaveHorse(Horse horse)
        {
            //create a new connection
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                //activate tracing
                db.TraceListener = new DebugTraceListener();

                if (horse.Id == 0)
                {
                    //new horse
                    db.Insert(horse);
                }
                else
                {
                    //update horse
                    db.Update(horse);
                }
            }
        }
        //REMOVE A SPECIFIC HORSE FROM TABLE
        public static void DeleteHorse(Horse horse)
        {
            // Create a new connection
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                // Activate Tracing
                db.TraceListener = new DebugTraceListener();

                // Object model:
                //db.Delete(horse);

                // SQL Syntax:
                db.Execute("DELETE FROM Horse WHERE Id = ?", horse.Id);
            }
        }

        //STUDENT FUNCTIONS
        //RETRIEVE ALL STUDENTS FROM TABLE
        public static List<Student> GetAllStudents()
        {
            List<Student> models;

            //create a new connection
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                //activate tracing
                db.TraceListener = new DebugTraceListener();

                models = (from p in db.Table<Student>()
                          select p).ToList();
            }

            return models;
        }
        //RETRIEVE A SPECIFIC STUDENT BY ID
        public static Student GetStudentById(int Id)
        {
            //create a new connection
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                //activate tracing
                db.TraceListener = new DebugTraceListener();

                Student m = (from p in db.Table<Student>()
                           where p.Id == Id
                           select p).FirstOrDefault();
                return m;
            }
        }
        //SAVE A SPECIFIC STUDENT TO TABLE
        public static void SaveStudent(Student student)
        {
            //create a new connection
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                //activate tracing
                db.TraceListener = new DebugTraceListener();

                if (student.Id == 0)
                {
                    //new student
                    db.Insert(student);
                }
                else
                {
                    //update student
                    db.Update(student);
                }
            }
        }
        //REMOVE A SPECIFIC STUDENT FROM TABLE
        public static void DeleteStudent(Student student)
        {
            // Create a new connection
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                // Activate Tracing
                db.TraceListener = new DebugTraceListener();

                // Object model:
                //db.Delete(student);

                // SQL Syntax:
                db.Execute("DELETE FROM Student WHERE Id = ?", student.Id);
            }
        }

        //LESSON FUNCTIONS
        //RETRIEVE ALL LESSONS FROM TABLE
        public static List<Lesson> GetAllLessons()
        {
            List<Lesson> models;

            //create a new connection
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                //activate tracing
                db.TraceListener = new DebugTraceListener();

                models = (from p in db.Table<Lesson>()
                          select p).ToList();
            }

            return models;
        }
        //RETRIEVE A SPECIFIC LESSON BY ID
        public static Lesson GetLessonById(int Id)
        {
            //create a new connection
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                //activate tracing
                db.TraceListener = new DebugTraceListener();

                Lesson m = (from p in db.Table<Lesson>()
                             where p.Id == Id
                             select p).FirstOrDefault();
                return m;
            }
        }
        //SAVE A SPECIFIC LESSON TO TABLE
        public static void SaveLesson(Lesson lesson)
        {
            //create a new connection
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                //activate tracing
                db.TraceListener = new DebugTraceListener();

                if (lesson.Id == 0)
                {
                    //new lesson
                    db.Insert(lesson);
                }
                else
                {
                    //update lesson
                    db.Update(lesson);
                }
            }
        }
        //REMOVE A SPECIFIC LESSON FROM TABLE
        public static void DeleteLesson(Lesson lesson)
        {
            // Create a new connection
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                // Activate Tracing
                db.TraceListener = new DebugTraceListener();

                // Object model:
                //db.Delete(lesson);

                // SQL Syntax:
                db.Execute("DELETE FROM Lesson WHERE Id = ?", lesson.Id);
            }
        }
    }
}
