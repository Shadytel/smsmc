using System;
using SMSMCsharp.Interfaces;

namespace eSMSC
{
    public class SMSTimeStamp : IBinarySerializable
	{
		public SMSTimeStamp ()
		{
		}
		public byte[] BinaryForm 
		{get;set;}
	}
}

