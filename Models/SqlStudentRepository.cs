using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace StudyManagement.Models
{
    public class SqlStudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;

        public SqlStudentRepository(AppDbContext context)
        {
            _context = context;
        }


        public Student Add(Student student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
            return student;
        }
        public Student Delete(int id)
        {
            var student = _context.Students.Find(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                _context.SaveChanges();
            }
            return student;

        }
        public IEnumerable<Student> GetAll() => _context.Students;
        public Student GetById(int id) => _context.Students.Find(id);
        public Student Update(Student updatedStudent)
        {
            var student = _context.Students.Attach(updatedStudent);
            student.State = EntityState.Modified;
            _context.SaveChanges();
            return updatedStudent;
        }
    }
}
