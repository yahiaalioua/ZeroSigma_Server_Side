using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AngularAuthApi.Entities
{
    public class Auth
    {
        [Key]
        [ForeignKey("User")]
        public int Id { get; set; }
        [MaxLength(250)]
        public string? RefreshToken { get; set; }
        [MaxLength(150)]
        public DateTime? ExpiredTime  { get; set; }
        [MaxLength(10)]
        public bool IsExpired => DateTime.UtcNow >= ExpiredTime;
        [MaxLength(10)]
        public bool IsActive => !IsExpired;
        public virtual User User { get; set; }

    }
}
