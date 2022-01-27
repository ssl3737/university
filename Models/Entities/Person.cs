using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.Entities
{
    public abstract class Person
    {
        [Key]
        public int PersonId { get; set; }

        public string FullName { get; set; }
    }
}
