using System.ComponentModel.DataAnnotations;

namespace Models.Entities
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Author { get; set; }

        [Required]
        public float Price { get; set; }
    }
}
