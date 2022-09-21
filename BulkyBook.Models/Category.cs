using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyBook.Models
{
    public class Category
    {
        // Data annotations voor entity framework

        [Key] // Maakt hier een primary key van als de database gemaakt word
        public int Id { get; set; }
        [Required] // Kan niet null zijn in de database hier moet iets ingevuld worden in de row
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "Display Order must be between 1 and 100 only !!")]
        public int DisplayOrder { get; set; }
        public DateTime CreateDateTime { get; set; } = DateTime.Now;

    }
}
