using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace CentralServer
{
    public class TrainManager
    {
        private List<Train> trains;
        public IReadOnlyList<Train> Trains
        {
            get
            {
                return trains.AsReadOnly();
            }
        }

        public TrainManager()
        {
            trains = new List<Train>();
        }

        public void MakeTrain(Connection connection)
        {
            try
            {
                trains.Add(new Train(connection));
            }
            catch
            {
                //TODO:
            }
        }

        public List<Train> GetTrains(int rideNumber)
        {
            List<Train> rides = new List<Train>();

            foreach (Train t in trains)
            {
                if (t.RideNumber == rideNumber)
                {
                    rides.Add(t);
                }
            }

            return rides;
        }
    }
}