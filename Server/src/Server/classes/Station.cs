using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace CentralServer
{
    public class Station
    {
        private Connection connection;
        public string StationName { get; set; }
        public Station(Connection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            this.connection = connection;
        }

        public List<(string, int)> Update()
        {
            List<Message> messages = connection.ReceiveMessage();
            List<(string, int)> requests = new List<(string, int)>();

            if (messages != null)
            {
                foreach (Message msg in messages)
                {
                    if (msg.Command == "GETOCCUPATION")
                    {
                        int trainId = 0;
                        if (Int32.TryParse(msg.Values, out trainId))
                        {
                            requests.Add((StationName, trainId));
                            Console.WriteLine("Station " + StationName + " asked for the occupation of " + trainId);
                            connection.SendACK();
                        }
                        else
                        {
                            connection.SendNACK();
                        }
                    }
                    else if (msg.Command == "IAM")
                    {
                        StationName = msg.Values;
                        Console.WriteLine("Station " + StationName + " has identified itself");
                        connection.SendACK();
                    }
                }
            }

            return requests;
        }

        public void SendOccupation(string message)
        {
            connection.SendMessage("OCCUPATION:" + message);
        }
    }
}