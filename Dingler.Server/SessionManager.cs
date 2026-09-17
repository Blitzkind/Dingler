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
		var removedById = _sessionsById.TryRemove(context.SessionId, out _);

		if (context.UserName is null)
			return removedById;

		if (_sessionsByUsername.TryGetValue(context.UserName, out var current) &&
		    ReferenceEquals(current, context))
		{
			return _sessionsByUsername.TryRemove(context.UserName, out _) || removedById;
		}

		return removedById;
	}

	public bool TryLinkUserToSession(string username, SessionContext context)
	{
		_sessionsByUsername[username] = context;
		return true;
	}

	public bool TryGetUserSession(string username, [MaybeNullWhen(false)] out SessionContext context)
	{
		return _sessionsByUsername.TryGetValue(username, out context);
	}
}