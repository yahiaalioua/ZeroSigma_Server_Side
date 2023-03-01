using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AngularAuthApi.Entities
{
    public record UserInfo
    {
        [Key]
        
        public int Id { get; set; }
        [MaxLength(100)]
        public string? Linkedin { get; set; }
        [MaxLength(100)]
        public string? YouTube { get; set; }
        [MaxLength(50)]
        public string? Website { get; set; }
        [MaxLength(200)]
        public string? AboutMe { get; set; }
        public virtual User User { get; set; }
    }
}
