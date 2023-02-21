namespace AngularAuthApi.Entities.Responses
{
    public class AuthUserResponse
    {
        public string ?AccessToken { get; set; }
        public Payload? Payload { get; set; }
        public string? RefreshToken { get; set; }

    }
}
