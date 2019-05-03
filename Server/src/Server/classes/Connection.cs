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

            while (connectionSocket.Available > 0)
            {
                int bytesRec = connectionSocket.Receive(data);
                Message += Encoding.ASCII.GetString(data, 0, bytesRec);
                //TODO implement protocol End and Start chars
                if (Message.IndexOf("<EOF>") > -1)
                {
                    break;
                }
            }
        }

        public void SendMessage(string msg)
        {
            byte[] message = Encoding.ASCII.GetBytes(msg);

            connectionSocket.Send(message);
        }
    }
}