using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace CentralServer
{
    public class Station
    {
        public Connection Connection;
        public string stationName { get; set; }
        public Station(Connection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            Connection = connection;
            Connection.SendMessage("ACK");
        }
    }
}