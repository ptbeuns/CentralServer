using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace CentralServer
{
    class Program
    {

        static void Main(string[] args)
        {
            Server server = new Server();

            while (true)
            {
                server.AcceptClient();

                foreach (Connection connection in server.AcceptedSockets)
                {
                    if (server.RailwayManager.CreateRailwayObjects(connection))
                    {
                        server.RemoveAcceptedConnection(connection);
                        break;
                    }
                }

                foreach (Train train in server.RailwayManager.trainManager.Trains)
                {
                    train.Connection.ReceiveMessage();

                    if (train.Connection.Message != null)
                    {
                        if (train.Connection.Message.Contains("IAM:"))
                        {
                            string value = MessageParser.GetValue(train.Connection.Message);
                            int rideNumber = 0;

                            if (Int32.TryParse(value, out rideNumber))
                            {
                                train.RideNumber = rideNumber;
                                train.Connection.SendMessage("ACK");
                            }
                            else
                            {
                                train.Connection.SendMessage("NACK");
                            }
                        }
                        else if (train.Connection.Message.Contains("OCCUPATION:"))
                        {
                            string value = MessageParser.GetValue(train.Connection.Message);
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
                                    train.Connection.SendMessage("NACK");
                                    break;
                                }
                            }

                            if (newOccuaption != null)
                            {
                                train.UpdateOccupation(newOccuaption);
                                train.Connection.SendMessage("ACK");
                            }
                        }
                        else
                        {
                            train.Connection.SendMessage("NACK");
                        }
                    }
                }

                foreach (Station station in server.RailwayManager.trackManager.Stations)
                {
                    station.Connection.ReceiveMessage();

                    if (station.Connection.Message != null)
                    {
                        if (station.Connection.Message.Contains("IAM:"))
                        {
                            string value = MessageParser.GetValue(station.Connection.Message);

                            if (value != null)
                            {
                                station.stationName = value;
                                station.Connection.SendMessage("ACK");
                            }
                            else
                            {
                                station.Connection.SendMessage("NACK");
                            }
                        }
                        else if (station.Connection.Message.Contains("GETOCCUPATION:"))
                        {
                            string value = MessageParser.GetValue(station.Connection.Message);
                            int rideNumber = 0;

                            if (Int32.TryParse(value, out rideNumber))
                            {
                                List<Train> trains = server.RailwayManager.trainManager.GetTrains(rideNumber);
                                string occupation = "OCCUPATION:";
                                foreach (Train t in trains)
                                {
                                    foreach(int occu in t.Occupation)
                                    {
                                        occupation += occu.ToString() + ",";
                                    }

                                    occupation = occupation.Remove(occupation.Length - 1);
                                }
                                station.Connection.SendMessage(occupation);
                            }
                            else
                            {
                                station.Connection.SendMessage("NACK");
                            }
                        }
                        else
                        {
                            station.Connection.SendMessage("NACK");
                        }
                    }
                }
            }
        }
    }
}