using System.Diagnostics.CodeAnalysis;
using Dingler.Server.Systems;
using Dingler.Game.Protocol.Messages;

namespace Dingler.Game.Protocol;

public static class MessageContextExtensions
{
	private const string HEADER = "header";
	private const string DATA_TYPE = "data_type";
	private const string REQUEST_ID = "request_id";
	private const string IS_CUSTOM_ENCODING = "Is_Custom_Encoding";
	
	extension(MessageContext context)
	{
		public void SetHeader(Header header)
		{
			context.AdditionalData[HEADER] = header;
		}

		public bool TryGetHeader([MaybeNullWhen(false)] out Header header)
		{
			if (!context.AdditionalData.TryGetValue(HEADER, out var obj))
			{
				header = null;
				return false;
			}
		
			header = (Header)obj;
			return true;
		}

		public void SetDataType(int dataType)
		{
			context.AdditionalData[DATA_TYPE] = dataType;
		}

		public bool TryGetDataType(out int dataType)
		{
			if (!context.AdditionalData.TryGetValue(DATA_TYPE, out var obj))
			{
				dataType = -1;
				return  false;
			}
		
			dataType = (int)obj;
			return true;
		}

		public void SetRequestId(long? requestId)
		{
			if (requestId is null)
				context.AdditionalData[REQUEST_ID] = (long)0;
			else
				context.AdditionalData[REQUEST_ID] = requestId;
		}

		public bool TryGetRequestId(out long requestId)
		{
			if (!context.AdditionalData.TryGetValue(REQUEST_ID, out var obj))
			{
				requestId = 0;
				return false;
			}
		
			requestId = (long)obj;
			return true;
		}

		public void SetCustomEncoding(bool isCustomEncoding = true)
		{
			context.AdditionalData[IS_CUSTOM_ENCODING] = isCustomEncoding;
		}

		public static bool IsRequestExpectingCustomEncoding(string issuer)
		{
			return issuer.Split('.').Length == 6;
		}
	}
}