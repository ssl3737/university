using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.Entities
{
    public class Instructor : Person
    {
        public virtual ICollection<Course> Courses { get; set; }
    }
}
