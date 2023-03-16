using Microsoft.Extensions.Options;

namespace AngularAuthApi.Authentication.OptionsSetup
{
    public class RefreshJwtConfigOptionsSetup:IConfigureOptions<RefreshJwtOptions>
    {
        private readonly IConfiguration _configuration;

        public RefreshJwtConfigOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void Configure(RefreshJwtOptions options)
        {
            _configuration.GetSection("RefreshJwtConfigOptions").Bind(options);
        }
    }
}
