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

        public void RemoveTrain(Train train)
        {
            if (train != null)
            {
                trains.Remove(train);
            }
        }

        public void UpdateTrains()
        {
            List<Train> toRemove = new List<Train>();

            foreach (Train train in trains)
            {
                try
                {
                    train.Update();
                }
                catch (ObjectDisposedException e)
                {
                    Console.WriteLine(train.RideNumber + " has closed the connection " + e.Message);
                    toRemove.Add(train);
                }
            }

            foreach(Train delet in toRemove)
            {
                RemoveTrain(delet);
            }
        }
    }
}