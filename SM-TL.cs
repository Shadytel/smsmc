using System;
using System.Collections.Generic;
using SMSMCsharp.Interfaces;

namespace eSMSC
{
	public class SM_TL
	{

		public SM_TL ()
		{
		}
	}

    public class AddressField : IBinarySerializable
	{
		public byte[] BinaryForm 
		{
			get{
				var toReturn = new List<byte> {AddressLength};
			    toReturn.AddRange(TypeOfAddress.BinaryForm);
				toReturn.AddRange(AddressValue);
				return toReturn.ToArray();
			}
			set{
				AddressLength = value[0];
				TypeOfAddress = new AddressType(value[1]);
				AddressValue = new byte[AddressLength];
				for(var i=0; i< AddressLength; i++)
					AddressValue[i] = value[AddressLength+2];

			}
		}

		public byte AddressLength {get;set;}

		public class AddressType
		{
			public AddressType(byte toSet){
				BinaryForm = new byte[]{toSet};
			}
            public AddressType(){}
			public byte[] BinaryForm 
			{
				get
				{
					if(NumberType == TypeOfNumber.Alphanumeric)
						return new byte[]{(byte)NumberType};
					return new byte[]{ (byte)((byte)NumberType + (byte)NumberPlan)};
				}
				set
				{
					NumberType = (TypeOfNumber)(value[0] & 0x70);
					NumberPlan = (NumberingPlanIdentification) (value[0] & 0x0F);
				}
			}
			public TypeOfNumber NumberType {get;set;}
			public NumberingPlanIdentification NumberPlan {get;set;}

			public enum TypeOfNumber :byte{
				Unknown = 0,
				Internation = 0x10,
				National = 0x20,
				NetworkSpecific = 0x30,
				Subscriber = 0x40,
				/// <summary>
				/// GSM TS 3.38 7bit Alphanumeric.
				/// </summary>
				Alphanumeric = 0x50,
				Abbreviated =0x60,
				Reserved = 0x70
			}
			//Note: The gaps in this enum are intentional.
			public enum NumberingPlanIdentification:byte{
				Unknown = 0,
				ISDN = 1,
				Data = 3,
				Telex = 4,
				National = 8,
				Private = 9,
				ERMES = 10,
				Reserved = 11
			}
		}

		public AddressType TypeOfAddress{get;set;}

		public byte[] AddressValue{get;set;}
        public override string ToString()
        {
            return AddressValue.ToString();
        }
	}
}

