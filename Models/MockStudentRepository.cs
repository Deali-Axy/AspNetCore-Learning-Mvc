using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyManagement.Models
{
    public class MockStudentRepository : IStudentRepository
    {
        private List<Student> _students;

        public MockStudentRepository()
        {
            _students = new List<Student>
            {
                new Student { Id=1, Name="小米", ClassName=ClassNameEnum.FirstGrade, Email="hello1@deali.cn" },
                new Student { Id=2, Name="华为", ClassName=ClassNameEnum.SecondGrade, Email="hello2@deali.cn" },
                new Student { Id=3, Name="oppo", ClassName=ClassNameEnum.ThirdGrade, Email="hello3@deali.cn" },
            };
        }

        public IEnumerable<Student> GetAll() => _students;

        public Student GetById(int id)
        {
            return _students.FirstOrDefault(a => a.Id == id);
        }

        public Student Add(Student student)
        {
            student.Id = _students.Max(s => s.Id) + 1;
            _students.Add(student);
            return student;
        }

        public Student Update(Student student)
        {
            var index = _students.FindIndex(s => s.Id == student.Id);
            if (index > -1)
            {
                _students[index] = student;
            }

            return student;
        }
        public Student Delete(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            if (student != null)
            {
                _students.Remove(student);
            }

            return student;
        }
    }
}
