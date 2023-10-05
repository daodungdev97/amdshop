using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Models.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Name must be at least 3 characters")]      
        [DisplayName("Author Name")]
        public string Name { get; set; }
      
    }
}
