namespace AngularAuthApi.Authentication
{
    public class JwtConfigOptions
    {
        public string AccessTokenSecretKey { get; init; }
        public int AccessTokenExpirationHours { get; set; }
        public string ?Issuer { get; set; }
        public string ?Audience { get; set; } 

    }
}
