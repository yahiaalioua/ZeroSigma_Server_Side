using System.ComponentModel.DataAnnotations;

namespace AngularAuthApi.Authentication.DTOS
{
    public record LoginDto
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
