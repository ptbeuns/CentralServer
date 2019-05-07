using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CentralServer
{
    public class Connection
    {
        private Socket connectionSocket;
        public string Message;


        public Connection(Socket socket)
        {
            if (socket == null)
            {
                throw new NullReferenceException("socket");
            }

            connectionSocket = socket;
        }

        public void ReceiveMessage()
        {
            byte[] data = new byte[1024];

            try
            {
                if (connectionSocket.Poll(1000, SelectMode.SelectRead))
                {
                    int bytesRec = connectionSocket.Receive(data);
                    Message += Encoding.ASCII.GetString(data, 0, bytesRec);
                    Message = MessageParser.Parse(Message);
                }
                else
                {
                    Message = null;
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.Message);
                connectionSocket.Close();
                connectionSocket.Dispose();
            }
        }

        public void SendMessage(string msg)
        {
            byte[] message = Encoding.ASCII.GetBytes(MessageParser.Encode(msg));

            try
            {
                connectionSocket.Send(message);
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.Message);
                connectionSocket.Close();
                connectionSocket.Dispose();
            }
        }
    }
}