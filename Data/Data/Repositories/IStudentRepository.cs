using Models.Entities;
using Models.StpModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Data.Repositories
{
    public interface IStudentRepository
    {
        Task AddStudentAsync(Student student);
        IEnumerable<Student> GetAllStudents();
        int GetStudentIdByEmail(string email);
        Task<Student> GetStudentByName(string name);
        Task<Student> GetStudentAsync(int? id);
        Task SaveAsync();
        void Edit(Student student);
        void Delete(Student student);
        Stp_GetStudentById GetStudentById(int studentId);
    }
}