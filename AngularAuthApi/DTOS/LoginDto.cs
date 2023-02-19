using System.ComponentModel.DataAnnotations;

namespace AngularAuthApi.DTOS
{
    public record LoginDto
    {
        public string Email { get; set; }
        
        public string Password { get; set; }
    }
}
