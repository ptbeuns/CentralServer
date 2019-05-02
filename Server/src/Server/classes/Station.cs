using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace CentralServer
{
    public class Station
    {
        public Connection Connection;
        public Station(Socket socket)
        {
            Connection = new Connection(socket);
        }

        public void SendTrainOccupation(int rideNumber, List<int> occupation)
        {
            //TODO
        }
    }
}