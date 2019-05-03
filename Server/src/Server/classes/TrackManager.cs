using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace CentralServer
{
    public class TrackManager
    {
        private List<Station> stations;
        public IReadOnlyList<Station> Stations
        {
            get
            {
                return stations.AsReadOnly();
            }
        }

        public TrackManager()
        {
            stations = new List<Station>();
        }

        public void MakeStation(Socket socket)
        {
            try
            {
                stations.Add(new Station(socket));
            }
            catch
            {
                //TODO:
            }
        }

        public List<string> GetMessages()
        {
            List<string> messages = new List<string>();
            foreach (Station s in stations)
            {
                s.Connection.ReceiveMessage();
                if (s.Connection.Message != null)
                {
                    messages.Add(s.Connection.Message);
                }
            }
            return messages;
        }
    }
}