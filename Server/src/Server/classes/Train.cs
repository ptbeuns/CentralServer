using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace CentralServer
{
    public class Train
    {
        private Connection Connection;
        private List<int> occupation;
        public int RideNumber { get; private set; }
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

        public void UpdateTrain()
        {
            Connection.ReceiveMessage();

            if (Connection.Message != null)
            {
                if (Connection.Message.Contains("IAM:"))
                {
                    string value = MessageParser.GetValue(Connection.Message);
                    int rideNumber = 0;

                    if (Int32.TryParse(value, out rideNumber))
                    {
                        RideNumber = rideNumber;
                        Connection.SendMessage("ACK");
                    }
                    else
                    {
                        Connection.SendMessage("NACK");
                    }
                }
                else if (Connection.Message.Contains("OCCUPATION:"))
                {
                    string value = MessageParser.GetValue(Connection.Message);
                    List<int> newOccuaption = new List<int>();
                    string[] values = value.Split(",");

                    foreach (string val in values)
                    {
                        int num = 0;
                        if (Int32.TryParse(val, out num))
                        {
                            newOccuaption.Add(num);
                        }
                        else
                        {
                            newOccuaption = null;
                            Connection.SendMessage("NACK");
                            break;
                        }
                    }

                    if (newOccuaption != null)
                    {
                        occupation = newOccuaption;
                        Connection.SendMessage("ACK");
                    }
                }
                else
                {
                    Connection.SendMessage("NACK");
                }
            }
        }
    }
}