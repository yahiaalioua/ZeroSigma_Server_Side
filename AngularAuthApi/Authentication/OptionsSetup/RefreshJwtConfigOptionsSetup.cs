using Microsoft.Extensions.Options;

namespace AngularAuthApi.Authentication.OptionsSetup
{
    public class RefreshJwtConfigOptionsSetup:IConfigureOptions<RefreshJwtConfigOptions>
    {
        private readonly IConfiguration _configuration;

        public RefreshJwtConfigOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void Configure(RefreshJwtConfigOptions options)
        {
            _configuration.GetSection("RefreshJwtConfigOptions").Bind(options);
        }
    }
}
