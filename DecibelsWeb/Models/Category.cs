using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DecibelsWeb.Models
{
    public class Category
    {
        // Key & Required are data annotations required for EF Core
        [Key]
        public int Id { get; set; }
        [Required]
        // DisplayName is a data annotation useful for client-side UI/validation
        [DisplayName("Category Name")]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        public int DisplayOrder { get; set; }
    }
}
