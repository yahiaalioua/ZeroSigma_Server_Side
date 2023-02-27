using System.ComponentModel.DataAnnotations;

namespace AngularAuthApi.Authentication.DTOS
{
    public record RefreshTokenDto
    {
        public int Id { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? ExpiredTime { get; set; }
    }
}
