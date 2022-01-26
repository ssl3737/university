using Dapper;
using Microsoft.Extensions.Configuration;
using Models.Entities;
using Models.StpModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

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

        public void AddStudent(Student student)
        {
            _context.Students.Add(student);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public IEnumerable<Student> GetAllStudents()
        {
            var result = _context.Students.ToList();

            return result;
        }

        public Student GetStudent(int id)
        {
            var result = _context.Students.Find(id);

            return result;
        }

        public int GetStudentIdByEmail(string email)
        {
            var studentId = _context.Students.Where(a => a.Email == email).SingleOrDefault().StudentId;

            return studentId;
        }

        public int GetStudentIdByName(string name)
        {
            var studentId = _context.Students.Where(a => a.FullName == name).SingleOrDefault().StudentId;

            return studentId;
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
                throw ex;
            }

            return result;
        }
    }
}
