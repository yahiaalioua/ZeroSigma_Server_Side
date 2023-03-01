using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AngularAuthApi.Entities
{
    public class Auth
    {
        [Key]
       
        public int Id { get; set; }
        [MaxLength(500)]
        public string? RefreshToken { get; set; }
        [MaxLength(50)]
        public DateTime? ExpiredDate  { get; set; }
        [MaxLength(50)]
        public DateTime? IssuedDate { get; set; }
        [MaxLength(10)]
        public bool IsExpired { get; set; }
        [MaxLength(10)]
        public bool IsActive { get; set; }
        public virtual User User { get; set; }

    }
}
