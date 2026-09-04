using System.Text.RegularExpressions;

namespace Dingler.Game.Protocol;

public static class RegexExtensions
{
	public static bool TryMatch(this Regex regex, string input, out Match match)
	{
		match = regex.Match(input);
		return match.Success;
	}
}