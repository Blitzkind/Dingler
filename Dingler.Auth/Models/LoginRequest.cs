namespace Dingler.Auth.Models
{
    public sealed class LoginRequest
    {
        public string User { get; set; }
        public string Pass { get; set; }
        public string Region { get; set; }
        public string Lang { get; set; }
        public string Totp { get; set; } = "";

        public LoginRequest(string user, string pass, string region, string lang, string totp = "")
        {
            User = user;
            Pass = pass;
            Region = region;
            Lang = lang;
            Totp = totp;
        }

        public LoginRequest()
        {
            User = "";
            Pass = "";
            Region = "";
            Lang = "";
            Totp = "";
        }
    }
}
