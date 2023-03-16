using Microsoft.Extensions.Options;

namespace AngularAuthApi.Authentication.OptionsSetup
{

    public class JwtConfigOptionsSetup : IConfigureOptions<JwtOptions>
    {
        private readonly IConfiguration _configuration;

        public JwtConfigOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(JwtOptions options)
        {
            _configuration.GetSection("JwtConfigOptions").Bind(options);
        }
    }
}
