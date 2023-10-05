using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Ecom.Models.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters")]
        [MaxLength(30, ErrorMessage = "Name must be no more than 30 characters")]
        [DisplayName("Category Name")]
        public string Name { get; set; }



        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "Display Order must be between 1-100")]
        public int DisplayOrder { get; set; }
        //DisplayOrder trong trường hợp này là một thuộc tính số nguyên được sử dụng để xác định thứ tự
        //hiển thị hoặc sắp xếp của một danh mục hoặc một mục trong ứng dụng của bạn.
    }
}
