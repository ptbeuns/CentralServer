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

            Console.WriteLine("BEUNS central server has been started");

            while (true)
            {
                server.AcceptClient();

                server.ProcessSockets();

                server.RailwayManager.Update();
            }
        }
    }
}