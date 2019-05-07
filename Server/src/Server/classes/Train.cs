using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace CentralServer
{
    public class Train
    {
        public Connection Connection;
        private List<int> occupation;
        public int RideNumber { get; set; }
        public int TrainUnitNumber { get; set; }
        public IReadOnlyList<int> Occupation
        {
            get
            {
                return occupation.AsReadOnly();
            }
        }

        public Train(Connection connection)
        {
            if (connection == null)
            {
                throw new NullReferenceException("connection");
            }
            Connection = connection;
            occupation = new List<int>();

            Connection.SendMessage("ACK");
        }

        public void UpdateOccupation(List<int> newOccupation)
        {
            if (newOccupation != null)
            {
                occupation = newOccupation;
            }
        }
    }
}