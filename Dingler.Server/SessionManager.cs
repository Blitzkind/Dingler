using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace Dingler.Server;

public sealed class SessionManager
{
	private readonly ConcurrentDictionary<Guid, SessionContext> _sessionsById = new();
	private readonly ConcurrentDictionary<string, SessionContext> _sessionsByUsername = new();

	public bool TryCreateSession(out SessionContext session)
	{
		var sessionId = Guid.NewGuid();

		session = new SessionContext(sessionId);

		return _sessionsById.TryAdd(sessionId, session);
	}

	public bool TryRemoveSession(SessionContext context)
	{
		return _sessionsById.Remove(context.SessionId, out _) |
		       (context.UserName is null || _sessionsByUsername.Remove(context.UserName, out _));
	}
	
	public bool TryLinkUserToSession(string username, SessionContext context)
	{
		return _sessionsByUsername.TryAdd(username, context);
	}

	public bool TryGetUserSession(string username, [MaybeNullWhen(false)] out SessionContext context)
	{
		return _sessionsByUsername.TryGetValue(username, out context);
	}
}