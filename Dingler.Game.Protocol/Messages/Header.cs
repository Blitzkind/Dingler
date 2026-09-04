using System.Text.Json;
using System.Text.Json.Serialization;

namespace Dingler.Game.Protocol.Messages
{
                        public sealed class Header
    {
        [JsonPropertyName("issuer")]
        public string? Issuer { get; set; } = null;

        [JsonPropertyName("target")]
        public string? Target { get; set; } = null;

        [JsonPropertyName("instance")]
        public string? Instance { get; set; } = null;

        [JsonPropertyName("sid")]
        public string? SessionId { get; set; } = null;

        [JsonPropertyName("scnt")]
        public long? ServerCount { get; set; } = 0;

        [JsonPropertyName("ccnt")]
        public long? ClientCount { get; set; } = 0;

        [JsonPropertyName("time")]
        public long? Time { get; set; } = 0;

        [JsonPropertyName("version")]
        public string? Version { get; set; } = null;

        [JsonPropertyName("tags")]
        public string? Tags { get; set; } = null;

        [JsonPropertyName("reqid")]
        public long? RequestId { get; set; }

        [JsonPropertyName("action")]
        public string? Action { get; set; } = null;

        [JsonPropertyName("c")]
        public byte Compressed { get; set; }


        public override bool Equals(object? obj)
        {
            if (obj is not Header header)
                return false;

            return (header.Issuer is null || header.Issuer == Issuer) && (header.Target is null || header.Target == Target) && (header.Instance is null || header.Instance == Instance);
        }

        public override int GetHashCode()
        {
            return $"{Issuer ?? ""}{Target ?? ""}{Instance ?? ""}".GetHashCode();
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
        }
    }
}
