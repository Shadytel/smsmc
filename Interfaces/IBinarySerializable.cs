using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMSMCsharp.Interfaces
{
    interface IBinarySerializable
    {
        byte[] BinaryForm { get; set; }
    }
}
