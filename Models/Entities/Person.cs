using System.ComponentModel.DataAnnotations;

namespace Models.Entities
{
    public abstract class Person
    {
        [Key]
        public int PersonId { get; set; }
        
        public string FullName { get; set; }
    }
}
