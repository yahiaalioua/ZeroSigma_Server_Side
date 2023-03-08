using Microsoft.Extensions.Options;

namespace AngularAuthApi.Data_Access.Options
{
    public class DatabaseOptionsSetup:IConfigureOptions<DatabaseOptions>
    {
        private readonly IConfiguration _configuration;

        public DatabaseOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void Configure(DatabaseOptions options)
        {
            var connectionString = _configuration.GetConnectionString("ConnStr1");
            options.ConnectionStrings = connectionString;
            _configuration.GetSection("connectionString").Bind(options);
            
        }
    }
}
