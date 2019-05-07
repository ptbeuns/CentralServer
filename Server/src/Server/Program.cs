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

                server.ProcessSockets();

                server.RailwayManager.UpdateTrains();

                server.RailwayManager.UpdateStations();
            }
        }
    }
}