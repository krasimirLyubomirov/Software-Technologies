using System.ComponentModel.DataAnnotations;

namespace AspBlog.Models.Article
{
    public class ArticleCreateFormModel
    {
        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        [MinLength(1)]
        public string Content { get; set; }
    }
}
