namespace AngularAuthApi.Authentication
{
    public interface IRefreshTokenValidate
    {
        bool Validate(string refreshToken);
    }
}