using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMSMCsharp
{
    public static class Extensions
    {
        public static byte Byte(this bool byt)
        {
            return (byte)(byt ? 1 : 0);
        }
    }
}
