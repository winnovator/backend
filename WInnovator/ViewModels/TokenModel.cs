namespace WInnovator.ViewModels
{
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
