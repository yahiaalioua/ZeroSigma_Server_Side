namespace AngularAuthApi.Utilities
{
    public interface IDecodeJwt
    {
        DateTime GetJwtExpiration(string token);
    }
}