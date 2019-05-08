using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace CentralServer
{
    public class Server
    {
        public RailwayManager RailwayManager;
        private Socket listnerSocket;
        private List<Connection> acceptedSockets;
        public IReadOnlyList<Connection> AcceptedConnections
        {
            get => acceptedSockets.AsReadOnly();
        }

        public Server()
        {
            RailwayManager = new RailwayManager();
            acceptedSockets = new List<Connection>();

            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 4337);

            listnerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listnerSocket.Bind(localEndPoint);
                listnerSocket.Listen(20);
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
            }
        }

        public void AcceptClient()
        {
            if (!listnerSocket.IsBound)
            {
                return;
            }

            Socket socket = null;

            if (listnerSocket.Poll(1000, SelectMode.SelectRead))
            {
                try
                {
                    socket = listnerSocket.Accept();
                }
                catch
                {
                }
            }

            if (socket != null)
            {
                acceptedSockets.Add(new Connection(socket));
                Console.WriteLine("A new socket has been accepted");
            }
        }

        public void RemoveAcceptedConnection(Connection connection)
        {
            if (connection != null)
            {
                acceptedSockets.Remove(connection);
                Console.WriteLine("A socket has been removed");
            }
        }

        public void ProcessSockets()
        {
            foreach (Connection connection in AcceptedConnections)
            {
                try
                {
                    if (RailwayManager.CreateRailwayObjects(connection))
                    {
                        RemoveAcceptedConnection(connection);
                        return;
                    }
                }
                catch (ObjectDisposedException e)
                {
                    RemoveAcceptedConnection(connection);
                    return;
                }
            }
        }
    }
}