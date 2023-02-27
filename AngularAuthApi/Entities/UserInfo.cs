using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AngularAuthApi.Entities
{
    public record UserInfo
    {
        [Key]
        [ForeignKey("User")]
        public int Id { get; set; }
        [MaxLength(50)]
        public string? Name { get; set; }
        [MaxLength(100)]
        public string? Email { get; set; }
        [MaxLength(100)]
        public string? Linkedin { get; set; }
        [MaxLength(100)]
        public string? Facebook { get; set; }
        [MaxLength(100)]
        public string? YouTube { get; set; }
        [MaxLength(50)]
        public string? Website { get; set; }
        [MaxLength(200)]
        public string? AboutMe { get; set; }
        public virtual User User { get; set; }
    }
}
