using Microsoft.Extensions.Options;

namespace AngularAuthApi.Authentication.OptionsSetup
{
    public class RefreshJwtConfigOptionsSetup:IConfigureOptions<JwtConfigOptions>
    {
        private readonly IConfiguration _configuration;

        public RefreshJwtConfigOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void Configure(JwtConfigOptions options)
        {
            _configuration.GetSection(nameof(RefreshJwtConfigOptions)).Bind(options);
        }
    }
}
