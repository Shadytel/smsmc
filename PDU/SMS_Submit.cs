using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMSMCsharp.Interfaces;
using eSMSC;

namespace SMSMCsharp.PDU
{
    public class SMS_Submit : PDUbase, IBinarySerializable
    {
        public override MessageTypeIndicator MessageType
        {
            get { return MessageTypeIndicator.Deliver; }
            set { throw new AccessViolationException("Message type already set"); }
        }
        public bool RejectDuplicates;
        public bool ValidityPeriodFormat;
        public bool HasUserDataHeader;
        public bool StatusReportRequest;
        public AddressField DestinationAddress;
        public byte Protocol;
        public byte DataCodingScheme;
        public SMSTimeStamp ValidityPeriod;
        public byte UserDataLength { get { return (byte)UserData.Length; } }
        public byte[] UserData;

        public byte[] BinaryForm
        {
            get
            {
                var header = (byte)(
                             ((byte)MessageType << 6) +
                            (RejectDuplicates.Byte() << 5) +
                            (ValidityPeriodFormat.Byte() << 4) +
                            (ValidityPeriodFormat.Byte() << 3) +
                            (HasUserDataHeader.Byte() << 3) +
                            (StatusReportRequest.Byte() << 2));
                var toReturn = new List<byte> { header, MessageReference };
                toReturn.AddRange(DestinationAddress.BinaryForm);
                toReturn.Add(Protocol);
                toReturn.Add(DataCodingScheme);
                toReturn.AddRange(ValidityPeriod.BinaryForm);
                toReturn.Add(UserDataLength);
                toReturn.AddRange(UserData);
                return toReturn.ToArray();
            }
            set
            {
                var header = value[0];
                if ((header >> 6) != (decimal)MessageTypeIndicator.Deliver)
                    throw new ArgumentOutOfRangeException("Message type != " + MessageTypeIndicator.Deliver + ". (Found type: " + (header >> 6) + ")");
                RejectDuplicates = (header & 0x20) == 0x20;
                ValidityPeriodFormat = (header & 0x10) == 0x10;
                HasUserDataHeader = (header & 0x04) == 0x08;
                StatusReportRequest = (header & 0x02) == 0x04;

                var userdat = new List<byte>();
                var userlength = value[24];
                for(var i=0; i< userlength; i++)
                {
                    userdat.Add(value[i+24]);
                }
                UserData = userdat.ToArray();
            }
        }

        public byte MessageReference
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }
}
