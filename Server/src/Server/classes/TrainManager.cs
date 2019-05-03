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

        public void MakeTrain(Socket socket)
        {
            try
            {
                trains.Add(new Train(socket));
            }
            catch
            {
                //TODO:
            }
        }

        public List<string> GetMessages()
        {
            List<string> messages = new List<string>();
            foreach (Train s in trains)
            {
                s.Connection.ReceiveMessage();
                if (s.Connection.Message != null)
                {
                    messages.Add(s.Connection.Message);
                }
            }
            return messages;
        }

        public Train GetTrain(int rideNumber)
        {
            return null;
        }

        public void UpdateTrain(int rideNumber, List<int> occupation)
        {

        }
    }
}