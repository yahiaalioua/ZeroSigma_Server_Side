using System.Security.Permissions;

namespace AngularAuthApi.Entities.Requests
{
    public record RefreshAuthRequest
    {
        public string? RefreshToken { get; set; }

    }
}
