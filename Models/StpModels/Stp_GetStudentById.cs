using System.Collections.Generic;

namespace Models.StpModels
{
    public class Stp_GetStudentById
    {
        public List<StudentById> StudentById { get; set; }
    }

    public class StudentById
    {
        public int StudentId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
    }
}
