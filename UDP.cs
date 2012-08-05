using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SMSMCsharp.PDU;
using eSMSC;

namespace SMSMCsharp
{
    public static class UDP
    {
        public static System.Net.Sockets.UdpClient Socket = new System.Net.Sockets.UdpClient();
        public static void SendMessage(byte[] toSend)
        {
            Socket.Send(toSend, toSend.Length, (string)Program.AppReader.GetValue("hostname", typeof(string)), (int)Program.AppReader.GetValue("port", typeof(int)));
        }

        public static void Start()
        {
            Socket.Client.Bind(new IPEndPoint(IPAddress.Any, 1337));
            Socket.BeginReceive(ReceiveUDP, null);
        }

        public static void SendSMS(string message, string to, string from)
        {
            var toSend = new SMS_Deliver();
            toSend.UserData = Encoding.ASCII.GetBytes(message);

            var addressType = new AddressField.AddressType
                              {
                                  NumberPlan = AddressField.AddressType.NumberingPlanIdentification.National,
                                  NumberType = AddressField.AddressType.TypeOfNumber.Subscriber
                              };

            toSend.OrigninatingAddress = new AddressField()
                                             {
                                                 AddressValue = Encoding.ASCII.GetBytes(from),
                                                 TypeOfAddress = addressType
                                             };

            var bMessage = toSend.BinaryForm;

            var bTo = Encoding.ASCII.GetBytes(to);
            var bytesToSend = new List<byte>();

            bytesToSend.Add((byte)bTo.Length);
            bytesToSend.AddRange(bTo);
            bytesToSend.AddRange(bMessage);

            SendMessage(bytesToSend.ToArray());
        }

        private static void ReceiveUDP(IAsyncResult ar)
        {
            var u = (UdpClient)((UdpState)(ar.AsyncState)).u;
            var e = (IPEndPoint)((UdpState)(ar.AsyncState)).e;
            var rawBytes = new List<byte>();
            rawBytes.AddRange(u.EndReceive(ar, ref e));
            var FromAddress = rawBytes.GetRange(1, rawBytes[0]);

            var receiveBytes = rawBytes.GetRange(rawBytes[0] + 1, rawBytes.Count - 1 - rawBytes[0]).ToArray();

            switch ((PDUbase.MessageTypeIndicator)receiveBytes[0])
            {
                case PDUbase.MessageTypeIndicator.DeliverReport:
                case PDUbase.MessageTypeIndicator.Command:
                    break;
                case PDUbase.MessageTypeIndicator.Submit:
                    var packet = new SMS_Submit {BinaryForm = receiveBytes};
                    var hairpin = packet.DestinationAddress.ToString().StartsWith("21") ||
                                  packet.DestinationAddress.ToString().StartsWith("11");
                    if (hairpin)
                    {
                        UDP.SendSMS(packet.UserData.ToString(), packet.DestinationAddress.ToString(), FromAddress.ToString());
                        break;
                    }
                    Http.SendSMS(packet.UserData.ToString(), packet.DestinationAddress.ToString(), "");
                    break;
                default:
                    break;
            }
        }
    }
    public class UdpState
    {
        public IPEndPoint e;
        public UdpClient u;
    }
}
