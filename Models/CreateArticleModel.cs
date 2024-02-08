using System.ComponentModel.DataAnnotations.Schema;

namespace NewsApp.Models
{
    public class CreateArticleModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public IFormFile? Photo { get; set; }
        public DateTime DateTime { get; set; }
    }
}
