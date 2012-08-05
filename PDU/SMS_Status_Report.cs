using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMSMCsharp.Interfaces;
using eSMSC;

namespace SMSMCsharp.PDU
{
    public class SMS_Status_Report : PDUbase, IBinarySerializable
    {

        //public enum Statu

        public byte[] BinaryForm
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }


    }
}
