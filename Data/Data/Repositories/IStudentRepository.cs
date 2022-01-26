using Models.Entities;
using Models.StpModels;
using System.Collections.Generic;

namespace Data.Data.Repositories
{
    public interface IStudentRepository
    {
        void AddStudent(Student student);
        IEnumerable<Student> GetAllStudents();
        int GetStudentIdByEmail(string email);
        int GetStudentIdByName(string name);
        Student GetStudent(int id);
        void Save();
        void Edit(Student student);
        void Delete(Student student);
        Stp_GetStudentById GetStudentById(int studentId);
    }
}