using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace SMSMCsharp
{
    public static class Http
    {
        public static HttpListener Listener = new HttpListener();

        public static void Start()
        {
            Listener.Prefixes.Add("http://localhost/");
            Listener.Start();
            Listener.BeginGetContext(OnPost, null);
        }

        public static void SendSMS(string message, string phone, string to)
        {
            var client = new WebClient();
            client.UploadString(to, message);
        }

        private static void OnPost(IAsyncResult ar)
        {
            var listener = (HttpListener) ar.AsyncState;
            // Call EndGetContext to complete the asynchronous operation.
            var context = listener.EndGetContext(ar);
            var request = context.Request;
            // Obtain a response object.
            var response = context.Response;
            try
            {
                if (request.ContentLength64 > 160)
                {
                    response.StatusCode = 413;
                    var bufferb = System.Text.Encoding.UTF8.GetBytes("ENTITY TOO LARGE");
                    // Get a response stream and write the response to it.
                    response.ContentLength64 = bufferb.Length;
                    var outputr = response.OutputStream;
                    outputr.Write(bufferb, 0, bufferb.Length);
                    // You must close the output stream.
                    outputr.Close();
                }

                response.StatusCode = 202;

                var bytes = new byte[request.ContentLength64];
                request.InputStream.Read(bytes, 0, (int)request.ContentLength64);
                // Construct a response.
                UDP.SendSMS(bytes.ToString(), request.Url.AbsolutePath.TrimEnd('/').Split('/').Last(), "123456");
                var responseString = "ACCEPTED";
                var buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                // Get a response stream and write the response to it.
                response.ContentLength64 = buffer.Length;
                var output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                // You must close the output stream.
                output.Close();
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                response.StatusCode = 567;
                var buffer = System.Text.Encoding.UTF8.GetBytes("FUCKED UP REQUEST");
                // Get a response stream and write the response to it.
                response.ContentLength64 = buffer.Length;
                var outputr = response.OutputStream;
                outputr.Write(buffer, 0, buffer.Length);
                // You must close the output stream.
                outputr.Close();
            }

            listener.BeginGetContext(OnPost, null);
        }
    }
}
