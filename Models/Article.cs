using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsApp.Models
{
    public class Article
    {
        [Key]
        public int ID { get; set; }
        
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateTime { get; set; }
        public int UID { get; set; }
        [ForeignKey("UID")]
        public virtual User User { get; set; }
        public int? PID { get; set; }
        [ForeignKey("PID")]
        public virtual Photo? Photo { get; set; }

    }
    public class Photo
    {
        [Key]
        public int PID { get; set; }        
        public string? Title { get; set; }
        public byte[] ImageData { get; set; }
    }
}
