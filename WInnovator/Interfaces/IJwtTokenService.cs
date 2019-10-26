namespace WInnovator.Interfaces
{
    public interface IJwtTokenService
    {
        string BuildToken(string email);
    }
}