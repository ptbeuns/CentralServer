using System;

namespace CentralServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();

            while(true)
            {
                server.AcceptClient();

                foreach (string msg in server.RailwayManager.trainManager.GetMessages())
                {

                }

                foreach (string msg in server.RailwayManager.trackManager.GetMessages())
                {

                }
            }
        }
    }
}