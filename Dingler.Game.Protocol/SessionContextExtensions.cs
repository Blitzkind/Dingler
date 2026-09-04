using Dingler.Server;

namespace Dingler.Game.Protocol;

public static class SessionContextExtensions
{
	private const string ENCODER = "encoder";
	private const string SERVER_MESSAGE_COUNT = "serverMessageCount";
	
	public static DinglerEncoder GetOrCreateEncoder(this SessionContext sessionContext)
	{
		if (sessionContext.AdditionalData.TryGetValue(ENCODER, out var boxedEncoder) &&
		    boxedEncoder is DinglerEncoder encoder) 
			return encoder;
            
		encoder = new DinglerEncoder();
		sessionContext.AdditionalData[ENCODER] = encoder;

		return encoder;
	}
	
	internal static int GetCurrentServerMessageCount(this SessionContext sessionContext)
	{
		var count = sessionContext.AdditionalData.AddOrUpdate(SERVER_MESSAGE_COUNT, 1, (_, value) =>
		{
			var count = (int)value;

			return count + 1;
		});

		return (int)count;
	}
}