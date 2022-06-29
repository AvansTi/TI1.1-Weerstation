using System;
using System.Collections.Generic;
using System.Text;
using Client.Repos;
using static Client.SerialConsole.Globals;

namespace Client.SerialConsole
{
    public class ConnectionManager : IWeatherConsoleDAO
    {
        private string comport;
        private int baudrate;

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
            baudrate = 19200;
        }

        public List<WeatherDataPoint> Get()
        {
            WeatherStationConnection connection = new WeatherStationConnection();
            int result = connection.OpenSerialPort(this.comport, this.baudrate);

            //Could not open the com port, return null
            if(result == OK)
                result = connection.WakeUp();

            if(result == OK)
                result = connection.GetRealTimeData();

            result = connection.CloseSerialPort();

            //TODO implement return value
            return null;
        }
    }
}
