namespace AngularAuthApi.Entities.Responses
{
    public class AuthUserResponse
    {
        public string ?AccessToken { get; set; } 
        public string? Email { get; set;}
        public string? Name { get; set;}
    }
}
