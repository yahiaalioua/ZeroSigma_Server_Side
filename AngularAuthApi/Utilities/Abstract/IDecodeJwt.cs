namespace AngularAuthApi.Utilities
{
    public interface IDecodeJwt
    {
        DateTime GetJwtExpiration(string token);
        DateTime GetJwtIssued(string token);
        bool IsExpired(string token);
        bool IsActive(string token);
    }
}