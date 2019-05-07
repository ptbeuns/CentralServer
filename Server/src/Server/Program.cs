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

                foreach (Connection connection in server.AcceptedSockets)
                {
                    if (server.RailwayManager.CreateRailwayObjects(connection))
                    {
                        server.RemoveAcceptedConnection(connection);
                        break;
                    }
                }

                foreach (Train train in server.RailwayManager.trainManager.Trains)
                {
                    train.UpdateTrain();
                }

                foreach (Station station in server.RailwayManager.trackManager.Stations)
                {

                }
            }
        }
    }
}