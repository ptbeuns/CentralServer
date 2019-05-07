using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace CentralServer
{
    public class Station
    {
        private Connection Connection;
        public Station(Connection connection)
        {
            if(connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            Connection = connection;
            Connection.SendMessage("ACK");
        }

        public void SendTrainOccupation(int rideNumber, List<int> occupation)
        {
            //TODO make message protocol compliant
            string message = "";
            message += rideNumber.ToString();
            foreach(int o in occupation)
            {
                message += ',';
                message += o.ToString();
            }
            Connection.SendMessage(message);
        }

        public void UpdateStation()
        {
            Connection.ReceiveMessage();
        }
    }
}