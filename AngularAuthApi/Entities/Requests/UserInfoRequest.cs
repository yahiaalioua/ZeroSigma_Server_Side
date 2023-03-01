using System.ComponentModel.DataAnnotations;

namespace AngularAuthApi.Entities.Requests
{
    public class UserInfoRequest
    {
        public int Id { get; set; }
        
        public string? Linkedin { get; set; }      
               
        public string? YouTube { get; set; }
       
        public string? Website { get; set; }
        
        public string? AboutMe { get; set; }
    }
}
