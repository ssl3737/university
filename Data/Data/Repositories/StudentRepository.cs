using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models.Entities;
using Models.StpModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;


namespace Data.Data.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly UniversityContext _context;
        private readonly IConfiguration _config;
        public static SqlConnection connection;

        public StudentRepository(UniversityContext context,
                                 IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public SqlConnection GetConnection()
        {
            string connectionString = _config.GetConnectionString("UniversityConnection");

            return new SqlConnection(connectionString);
        }

        public virtual async Task AddStudentAsync(Student student)
        {
            await _context.Students.AddAsync(student);
        }

        public virtual async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Student> GetAllStudents()
        {
            var result = _context.Students.ToList();

            return result;
        }

        public async Task<Student> GetStudentAsync(int? id)
        {
            return await _context.Students
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.StudentId == id);
        }

        public int GetStudentIdByEmail(string email)
        {
            var studentId = _context.Students.Where(a => a.Email == email).SingleOrDefault().StudentId;

            return studentId;
        }

        public async Task<Student> GetStudentByName(string name)
        {

            return await _context.Students
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.FullName == name);
        }

        public void Edit(Student student)
        {
            _context.Update(student);
        }

        public void Delete(Student student)
        {
            _context.Remove(student);
        }

        public Stp_GetStudentById GetStudentById(int studentId)
        {
            Stp_GetStudentById result = null;

            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();

                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Id", studentId);

                    var objDetails = SqlMapper.QueryMultiple(connection, "dbo.stp_GetStudentById", param, commandType: CommandType.StoredProcedure);
                    result = new Stp_GetStudentById() 
                    {
                       StudentById  = objDetails.Read<StudentById>().ToList()
                    };

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }
    }
}
