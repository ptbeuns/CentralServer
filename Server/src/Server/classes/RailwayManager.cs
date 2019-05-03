using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

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

        public void CreateRailwayObject(Socket socket)
        {
            string railwayObjectType = null;
            if (socket == null)
            {
                throw new NullReferenceException("socket");
            }

            byte[] data = new byte[1024];

            while (socket.Available > 0)
            {
                int receivedData = socket.Receive(data);
                railwayObjectType += Encoding.ASCII.GetString(data, 0, receivedData);
                //TODO: change delimiter
                if (railwayObjectType.IndexOf("<EOF>") > -1)
                {
                    break;
                }
            }

            if (railwayObjectType == "Train")
            {
                trainManager.MakeTrain(socket);
            }
            else if (railwayObjectType == "Station")
            {
                trackManager.MakeStation(socket);
            }
            else
            {
                return;
            }

        }
    }
}