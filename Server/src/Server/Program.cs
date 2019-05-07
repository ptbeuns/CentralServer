using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace CentralServer
{
    class Program
    {

        static void Main(string[] args)
        {
            Server server = new Server();

            while (true)
            {
                server.AcceptClient();

                foreach (Connection connection in server.AcceptedConnections)
                {
                    if (server.RailwayManager.CreateRailwayObjects(connection))
                    {
                        server.RemoveAcceptedConnection(connection);
                        break;
                    }
                }

                server.RailwayManager.UpdateTrains();

                server.RailwayManager.UpdateStations();
            }
        }
    }
}