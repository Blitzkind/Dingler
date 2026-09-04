namespace Dingler.Game.Configuration;

public sealed class AuthOptions
{
	public static string SectionName => "Auth";
	public string BaseUrl { get; set; } = "https://localhost:5000";
}