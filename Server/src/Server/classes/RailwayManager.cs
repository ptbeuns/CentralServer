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
                Console.WriteLine("A socket has registerd itself as a train");
                return true;
            }
            else if (connection.Message == "CONNECT:STATION")
            {
                trackManager.MakeStation(connection);
                Console.WriteLine("A socket has registerd itself as a station");
                return true;
            }
            else if (connection.Message != null)
            {
                connection.SendMessage("NACK");
                return false;
            }

            return false;
        }

        public void UpdateStations()
        {
            foreach (Station station in trackManager.Stations)
            {
                try
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
                                Console.WriteLine("Station " + value + " has identified itself");
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
                                List<Train> trains = trainManager.GetTrains(rideNumber);
                                string occupation = "OCCUPATION:";
                                foreach (Train t in trains)
                                {
                                    occupation += "@" + t.TrainUnitNumber;
                                    foreach (int occu in t.Occupation)
                                    {

                                        occupation += "," + occu.ToString();
                                    }
                                }
                                Console.WriteLine("Station " + station.stationName + " has requested ride: " + rideNumber);
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
                catch (ObjectDisposedException e)
                {
                    Console.WriteLine("Communication to station " + station.stationName + " has been terminated");
                    trackManager.RemoveStation(station);
                    return;
                }
            }
        }

        public void UpdateTrains()
        {
            foreach (Train train in trainManager.Trains)
            {
                try
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
                                Console.WriteLine("Ride " + value + " has identified itself");
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

                            if(train.TrainUnitNumber == 0)
                            {
                                int trainUnitNumber = 0;
                                if(Int32.TryParse(values[0], out trainUnitNumber))
                                {
                                    train.TrainUnitNumber = trainUnitNumber;
                                }
                            }

                            for (int i = 1; i < values.Length; i++)
                            {
                                int num = 0;
                                if (Int32.TryParse(values[i], out num))
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
                                Console.WriteLine("Train " + train.TrainUnitNumber + " has updated its occupation of ride " + train.RideNumber);
                                train.Connection.SendMessage("ACK");
                            }
                        }
                        else
                        {
                            train.Connection.SendMessage("NACK");
                        }
                    }
                }
                catch (ObjectDisposedException e)
                {
                    Console.WriteLine("Communication to train " + train.TrainUnitNumber + " of ride " + train.RideNumber + " has been terminated");
                    trainManager.RemoveTrain(train);
                    return;
                }
            }
        }
    }
}
