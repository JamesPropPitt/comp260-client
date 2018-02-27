using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Client
{
    class client
    {
        static void Main(string[] args)
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint ipLocal = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8221);

			bool connected = false;

			while (connected == false) 
			{
				try 
				{
					s.Connect (ipLocal);
					connected = true;
                    
				} 
				catch (Exception) 
				{
					Thread.Sleep (1000);
				}
			}

            int ID = 0;

            while (true)
            {
                String Msg = Console.ReadLine();
                ASCIIEncoding encoder = new ASCIIEncoding();
                byte[] buffer = encoder.GetBytes(Msg);
                
                String DunMessage = encoder.GetString(buffer, 0, ID);
                Console.WriteLine(DunMessage);

                try
                {
                    Console.WriteLine("Writing to server: " + Msg);
                    int bytesSent = s.Send(buffer);
                    s.Receive(buffer);
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex);	
                }
                

                Thread.Sleep(1000);
            }
        }
    }
}
