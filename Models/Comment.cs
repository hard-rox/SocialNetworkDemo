using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialNetDemo.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        [Required]
        public string CommentText { get; set; }
    }
}