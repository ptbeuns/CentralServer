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

        public void MakeStation(Connection connection)
        {
            try
            {
                stations.Add(new Station(connection));
            }
            catch
            {
                //TODO:
            }
        }

        public void RemoveStation(Station station)
        {
            if(station != null)
            {
                stations.Remove(station);
            }
        }
    }
}