using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace CentralServer
{
    public class StationManager
    {
        private List<Station> stations;
        public IReadOnlyList<Station> Stations
        {
            get
            {
                return stations.AsReadOnly();
            }
        }

        public StationManager()
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
            if (station != null)
            {
                stations.Remove(station);
            }
        }

        public void SendOccupation(string stationName, string occupation)
        {
            foreach (Station station in stations)
            {
                if (station.StationName == stationName)
                {
                    station.SendOccupation(occupation);
                }
            }
        }

        public List<(string, int)> UpdateStations()
        {
            List<(string, int)> requests = new List<(string, int)>();

            foreach (Station station in stations)
            {
                foreach ((string, int) request in station.Update())
                {
                    requests.Add(request);
                }
            }

            return requests;
        }
    }
}