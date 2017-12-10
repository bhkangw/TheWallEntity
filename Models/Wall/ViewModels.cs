using System.ComponentModel.DataAnnotations;

namespace TheWallEntity.Models
{
    public class ViewMessage : BaseEntity
    {
        [Required]
        [Display(Name = "Message")]
        [MinLength(2)]
        public string MContent { get; set; }

    }
    public class ViewComment : BaseEntity
    {
        [Required]
        [Display(Name = "Comment")]
        [MinLength(2)]
        public string CContent { get; set; }
    }
}