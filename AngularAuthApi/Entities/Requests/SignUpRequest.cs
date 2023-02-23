using System.ComponentModel.DataAnnotations;

namespace AngularAuthApi.Entities.Requests
{
    public record SignUpRequest
    {
        public string? Name { get; set; }
        
        public string? Email { get; set; }
        
        public string? Password { get; set; }
       
    }
}
