namespace Dingler.Server.Systems;

public abstract class MessageContext
{
	public Dictionary<string, object> AdditionalData { get; init; }

	protected MessageContext()
	{
		AdditionalData = new Dictionary<string, object>();
	}

	protected MessageContext(Dictionary<string, object> additionalData)
	{
		AdditionalData = additionalData;
	}
}