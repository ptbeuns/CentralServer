using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Collections.Generic;

namespace CentralServer
{
    public class RailwayManager
    {
        public StationManager StationManager;
        public TrainManager TrainManager;

        public RailwayManager()
        {
            StationManager = new StationManager();
            TrainManager = new TrainManager();
        }

        public bool CreateRailwayObjects(Connection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            List<Message> messages = connection.ReceiveMessage();

            foreach (Message msg in messages)
            {
                if (msg.Command == "CONNECT" && msg.Values == "TRAIN")
                {
                    TrainManager.MakeTrain(connection);
                    connection.SendACK();
                    Console.WriteLine("A socket has registerd itself as a train");
                    return true;
                }
                else if (msg.Command == "CONNECT" && msg.Values == "STATION")
                {
                    StationManager.MakeStation(connection);
                    connection.SendACK();
                    Console.WriteLine("A socket has registerd itself as a station");
                    return true;
                }
                else if (msg != null)
                {
                    connection.SendNACK();
                    return false;
                }
            }

            return false;
        }

        public void Update()
        {
            TrainManager.UpdateTrains();
            List<(string, int)> requests = StationManager.UpdateStations();

            foreach ((string, int) request in requests)
            {
                List<Train> trains = TrainManager.GetTrains(request.Item2);
                string msg = "@" + request.Item2;

                foreach (Train train in trains)
                {
                    msg += "@" + train.TrainUnitNumber;

                    foreach (int i in train.Occupation)
                    {
                        msg += "," + i;
                    }
                }

                StationManager.SendOccupation(request.Item1, msg);
            }
        }
    }
}
