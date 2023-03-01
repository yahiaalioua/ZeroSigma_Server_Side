using System.ComponentModel.DataAnnotations;

namespace AngularAuthApi.Entities.Responses
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Linkedin { get; set; }         
        public string? YouTube { get; set; }     
        public string? Website { get; set; }      
        public string? AboutMe { get; set; }
    }
}
