using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace CentralServer
{
    public class Train
    {
        public Connection Connection;
        private List<Coupe> coupes;
        public int RideNumber { get; private set; }
        public IReadOnlyList<Coupe> Coupes
        {
            get
            {
                return coupes.AsReadOnly();
            }
        }

        public Train(Socket socket)
        {
            if (socket == null)
            {
                throw new NullReferenceException("socket");
            }
            Connection = new Connection(socket);
            coupes = new List<Coupe>();
        }
    }
}