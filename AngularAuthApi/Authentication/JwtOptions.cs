namespace AngularAuthApi.Authentication
{
    public class JwtOptions
    {
        public string AccessTokenSecretKey { get; init; }
        public double AccessTokenExpirationMinutes { get; set; }
        public string ?Issuer { get; set; }
        public string ?Audience { get; set; } 

    }
}
