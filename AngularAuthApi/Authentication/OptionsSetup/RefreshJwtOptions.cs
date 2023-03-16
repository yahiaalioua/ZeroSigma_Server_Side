namespace AngularAuthApi.Authentication.OptionsSetup
{
    public class RefreshJwtOptions
    {
        public string RefreshTokenSecretKey { get; init; }
        public double RefreshTokenExpirationHours { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
