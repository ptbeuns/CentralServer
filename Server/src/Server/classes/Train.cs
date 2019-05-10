using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace CentralServer
{
    public class Train
    {
        private Connection connection;
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
            this.connection = connection;
            occupation = new List<int>();

        }

        private void UpdateOccupation(List<int> newOccupation)
        {
            if (newOccupation != null)
            {
                occupation = newOccupation;
            }
        }

        public void Update()
        {
            List<Message> messages = connection.ReceiveMessage();

            if (messages != null)
            {
                foreach (Message msg in messages)
                {
                    if (msg.Command == "IAM")
                    {
                        int rideNumber = 0;
                        if (Int32.TryParse(msg.Values, out rideNumber))
                        {
                            RideNumber = rideNumber;
                            Console.WriteLine("Ride " + RideNumber + " has identified itself");
                            connection.SendACK();
                        }
                        else
                        {
                            connection.SendNACK();
                        }
                    }
                    else if (msg.Command == "OCCUPATION")
                    {
                        List<int> occu = new List<int>();
                        string[] values = msg.Values.Split(',');

                        int trainUnit = 0;

                        if (Int32.TryParse(values[0], out trainUnit))
                        {
                            TrainUnitNumber = trainUnit;

                            for (int i = 1; i < values.Length; i++)
                            {
                                int occupationInt = 0;
                                if (Int32.TryParse(values[i], out occupationInt))
                                {
                                    occu.Add(occupationInt);
                                }
                            }
                            Console.WriteLine("Ride " + RideNumber + " has updated its occupation");
                            connection.SendACK();
                        }
                        else
                        {
                            connection.SendNACK();
                        }

                        if (occu.Count > 0)
                        {
                            UpdateOccupation(occu);
                        }
                    }
                }
            }
        }
    }
}