using System.Diagnostics.CodeAnalysis;

namespace WInnovator.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class TokenModel
    {
        public string Token { get; set; }
        public string Username { get; set; }

        public TokenModel(string token, string username)
        {
            Token = token;
            Username = username;
        }
    }
}
