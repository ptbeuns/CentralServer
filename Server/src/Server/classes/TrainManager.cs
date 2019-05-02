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
                trains.Add(new Train(socket))
            }
            catch
            {
                //TODO:
            }
        }
    }
}