using System.ComponentModel.DataAnnotations.Schema;

namespace SocialNetDemo.Models
{
    public class Like
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}