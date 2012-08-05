using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace SMSMCsharp
{
    public class Program
    {
        public static System.Configuration.AppSettingsReader AppReader = new System.Configuration.AppSettingsReader();
        static void Main(string[] args)
        {
            //var fjf = new BinaryReader(new FileStream("hi", FileMode.OpenOrCreate));
            UDP.Start();
            Http.Start();
            var testSend = false;
            while (true)
            {
                Thread.Sleep(1000);

                if(testSend)
                {
                    UDP.SendSMS("Hello World", "1234", "123456");
                    testSend = false;
                }
            }
        }
    }
}
