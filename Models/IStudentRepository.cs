using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyManagement.Models
{
    public interface IStudentRepository
    {
        Student GetById(int id);

        Student Add(Student student);

        IEnumerable<Student> GetAll();

        Student Update(Student updatedStudent);

        Student Delete(int id);
    }
}
