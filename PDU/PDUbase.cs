using System;

namespace eSMSC
{
	public class PDUbase
	{
	//	public bool MoreMessageToSend;
		//public byte MessageReference;
	//	public AddressField RecipientAddress;
		public virtual MessageTypeIndicator MessageType{get;set;}
		public bool IsFromMS{get;set;}
		public enum MessageTypeIndicator
		{
			Deliver,
			DeliverReport,
			StatusReport,
			Command,
			Submit,
			SumbitReport,
			Reserved
		}

		public PDUbase ()
		{
		}
	}
}

