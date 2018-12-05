using System.ComponentModel.DataAnnotations;

namespace AspBlog.Models.Article
{
    public class ArticleViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
        
        public string AuthorId { get; set; }
    }
}
