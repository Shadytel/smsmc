using System;
using System.Collections.Generic;
using SMSMCsharp;
using SMSMCsharp.Interfaces;

namespace eSMSC
{
    public class SMS_Deliver : PDUbase, IBinarySerializable
	{
		public override MessageTypeIndicator MessageType 
		{
			get{return MessageTypeIndicator.Deliver;}
			set{throw new AccessViolationException("Message type already set");}
		}
		public bool MoreMessages;
		public bool ReplyPathExists;
		public bool HasUserDataHeader;
		public bool StatusReportIndication;
		public AddressField OrigninatingAddress;
		public byte Protocol;
		public byte DataCodingScheme;
		public SMSTimeStamp ServiceCenterTimeStamp;
        public byte UserDataLength { get { return (byte) UserData.Length; } }
		public byte[] UserData;

		public byte[] BinaryForm 
		{
			get
			{
                var header = (byte) (
                             ((byte)MessageType << 6) + 
							(MoreMessages.Byte() << 5) +
                            (StatusReportIndication.Byte() << 4) +
                            (HasUserDataHeader.Byte() << 3) +
                            (ReplyPathExists.Byte() << 2));
			    var toReturn = new List<byte> {header};
                toReturn.AddRange(OrigninatingAddress.BinaryForm);
                toReturn.Add(Protocol);
                toReturn.Add(DataCodingScheme);
                toReturn.AddRange(ServiceCenterTimeStamp.BinaryForm);
                toReturn.Add(UserDataLength);
                toReturn.AddRange(UserData);
			    return toReturn.ToArray();
			}
			set
			{
				var header = value[0];
				if((header >> 6) != (decimal) MessageTypeIndicator.Deliver)
					throw new ArgumentOutOfRangeException("Message type != "+MessageTypeIndicator.Deliver+". (Found type: " + (header >> 6) + ")");
				MoreMessages = (header & 0x20) == 0x20;
				StatusReportIndication = (header & 0x10) == 0x10;
				HasUserDataHeader = (header & 0x08) == 0x08;
				ReplyPathExists = (header & 0x04) == 0x04;
			}
		}
	}
}

