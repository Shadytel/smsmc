using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMSMCsharp.Interfaces;
using eSMSC;

namespace SMSMCsharp.PDU
{
    public class SMS_Submit_Report : PDUbase, IBinarySerializable
    {
        public override PDUbase.MessageTypeIndicator MessageType
        {
            get { return PDUbase.MessageTypeIndicator.SumbitReport; }
            set { throw new AccessViolationException("Message type already set"); }
        }

        public byte FailureCause;

        public byte[] BinaryForm
        {
            get
            {
                return new byte[]
                           {
                               (byte) MessageType,
                               FailureCause
                           };
            }
            set { FailureCause = value[1]; }
        }
    }
}
