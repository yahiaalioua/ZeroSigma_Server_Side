namespace AngularAuthApi.Authentication.OptionsSetup
{
    public class RefreshJwtConfigOptions
    {
        public string RefreshTokenSecretKey { get; init; }
        public int RefreshTokenExpirationHours { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
