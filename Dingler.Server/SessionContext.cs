extern alias HexGame;

using HexGame::Game.Shared.Domain;

namespace Dingler.Server
{

    public sealed class SessionContext
    {
        public Guid SessionId { get; init; }
        public DateTime CreationTime { get; init; }
        public bool IsAuthenticated { get; set; }
        public string? UserName { get; set; }
        public string? AuthToken { get; set; }
        public Dictionary<ulong, deck_bits> Decks { get; }
        public ulong AccountId { get; set; }
        public ulong ProfileId { get; set; }
        public ulong CurrentTournamentId { get; set; }
        public int CurrentMessageCount { get; set; }
        public DinglerEncoder Encoder { get; }
        
        public event Func<object, CancellationToken, Task>? SendMessageAsync;
        public event Func<object, bool>? TrySendMessage;

        public event Action? Disconnected;

        public SessionContext(Guid sessionId)
        {
            SessionId = sessionId;
            CreationTime = DateTime.UtcNow;
            Decks = new Dictionary<ulong, deck_bits>();
            Encoder = new DinglerEncoder();
        }

        public void NotifyDisconnected()
        {
            try
            {
                Disconnected?.Invoke();
            }
            catch
            {
                // Swallow; disconnect notifications must not break teardown.
            }
        }

        public async Task SendMessageToClientAsync(object message, CancellationToken token)
        {
            try
            {
                if (SendMessageAsync is not null)
                    await SendMessageAsync.Invoke(message, token);
            }
            catch
            {
                // Swallow
            }
        }

        public bool TrySendMessageToClient(object message)
        {
            try
            {
                if (TrySendMessage is not null)
                    return TrySendMessage.Invoke(message);
            }
            catch
            {
                // swallow
            }
            
            return false;
        }
    }
}
