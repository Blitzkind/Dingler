using Microsoft.IdentityModel.Tokens;

namespace Dingler.Auth.Endpoints;

public static class OpenIdEndpoints
{
	public static IEndpointRouteBuilder MapOpenIdEndpoints(this IEndpointRouteBuilder builder, RsaSecurityKey signingKey)
	{
		builder.MapGet("/.well-known/openid-configuration", (HttpContext context) =>
		{
			var baseUrl = $"{context.Request.Host}";
			return Results.Json(new
			{
				issuer = "dingler-auth",
				jwks_uri = $"{baseUrl}/.well-known/jwks.json"
			});
		});
		
		builder.MapGet("/.well-known/jwks.json", () =>
		{
			var publicParameters = signingKey.Rsa.ExportParameters(includePrivateParameters: false);
			var publicOnlyKey = new RsaSecurityKey(publicParameters);
			
			var jwk = JsonWebKeyConverter.ConvertFromRSASecurityKey(publicOnlyKey);
			jwk.Use = "sig";
			jwk.Alg = SecurityAlgorithms.RsaSha256;
			
			return Results.Json(new { keys = new[] { jwk } });
		});

		return builder;
	}
}