using Org.BouncyCastle.Utilities.Encoders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AngularAuthApi.Entities
{
    public record User
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string ?Name { get; set; }
        [MaxLength(100)]
        [Required]
        public string ?Email { get; set; }
        [MaxLength(128)]
        [Required]
        public string ?Password { get; set; }
        public string ?Token { get; set; }

        [Column(TypeName = "varbinary")]
        [MaxLength(128)]
        public string ?Salt { get; set; }
        public virtual Auth Auth { get; set; }

    }
}
