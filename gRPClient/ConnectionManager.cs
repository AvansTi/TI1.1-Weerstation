using gRPCClient;
using gRPClient.Repo_s;
using System;
using System.Collections.Generic;
using System.Text;
using static gRPClient.Globals;

namespace gRPClient
{
    public class ConnectionManager : IWeatherConsoleDAO
    {
        private string comport;

        /// <summary>
        /// TODO remove constructor as it is used as test code
        /// </summary>
        public ConnectionManager()
        {
            var serialPorts = System.IO.Ports.SerialPort.GetPortNames();
            Console.WriteLine("Found the following COM ports: ");
            
            foreach (var item in serialPorts)
            {
                Console.WriteLine(item);
            }
            comport = serialPorts[0];
        }

        public List<WeatherDataPoint> Get()
        {
            WeatherStationConnection connection = new WeatherStationConnection();
            int result = connection.OpenSerialPort(this.comport);

            //Could not open the com port, return null
            if(result == OK)
                result = connection.WakeUp();

            //if(result == OK)
                //result = connection.GetRealTimeData();

            result = connection.CloseSerialPort();

            //TODO implement return value
            return null;
        }
    }
}
