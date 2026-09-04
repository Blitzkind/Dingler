using System.Text.Json.Serialization;

namespace Dingler.Game.Protocol.Messages.Requests
{
    public sealed class AuthenticationRequestArg
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
        [JsonPropertyName("user")]
        public string UserName { get; set; }


        public AuthenticationRequestArg()
        {
            Token = string.Empty;
            UserName = string.Empty;
        }

        public byte[] Serialize()
        {
            throw new NotImplementedException();
        }
    }
}
