using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsApp.Models
{
    public class User
    {
        [Key]
        public int UID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

    }
}
