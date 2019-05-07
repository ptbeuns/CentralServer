using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Collections.Generic;

namespace CentralServer
{
    public class RailwayManager
    {
        public TrackManager trackManager;
        public TrainManager trainManager;

        public RailwayManager()
        {
            trackManager = new TrackManager();
            trainManager = new TrainManager();
        }

        public bool CreateRailwayObjects(Connection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }


            connection.ReceiveMessage();

            if (connection.Message == "CONNECT:TRAIN")
            {
                trainManager.MakeTrain(connection);
                return true;
            }
            else if (connection.Message == "CONNECT:STATION")
            {
                trackManager.MakeStation(connection);
                return true;
            }
            else if(connection.Message != null)
            {
                connection.SendMessage("NACK");
                return false;
            }

            return false;
        }
}
}
