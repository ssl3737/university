using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Data.Data
{
    public class UniversityContext : IdentityDbContext<ApplicationUser>
    {
        public UniversityContext(DbContextOptions options) : base(options) { }

        public DbSet<Student> Students { get; set; }   
        public DbSet<Course> Courses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<Book> Book { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StudentCourse>()
             .HasKey(sc => new { sc.CourseId, sc.StudentId });

            modelBuilder.Ignore<Person>();

            modelBuilder.Entity<Course>()
                .HasOne(i => i.Instructor).WithMany(c => c.Courses);
        }
    }
}
