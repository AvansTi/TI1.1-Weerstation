using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Client.DomainServices;
using Shared.Domain;
using Shared.SerialConsole;
using static Client.SerialConsole.Globals;

namespace Client.SerialConsole
{
    public class ConnectionManager : IWeatherConsoleDAO
    {
        private readonly string comport;
        private readonly int baudrate;

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
        //Could take 1.5 to 2 minutes
        /// <summary>
        /// Requests a WeatherDataPoint from a weather console connected to a usb/serialport
        /// <remarks>Could take 1.5 to 2 minutes</remarks>
        /// </summary>
        /// <param name="a">Automapper class to map from raw data to domainobject</param>
        /// <returns></returns>
        public WeatherDataPoint Get(Mapper mapper)
        {
            WeatherStationConnection connection = new WeatherStationConnection();
            WeatherDataPoint dataPoint =  null;
            int result = connection.OpenSerialPort(this.comport, this.baudrate);

            //Could not open the com port, return null
            if(result == OK)
                result = connection.WakeUp();

            if(result == OK)
            {
                WeatherStationDataStruct dataStruct = connection.GetRealTimeData();
                if(dataStruct != null)
                    dataPoint = mapper.Map<WeatherDataPoint>(dataStruct);
            }

            result = connection.CloseSerialPort();
            
            if(result == NOK)
                Console.WriteLine("Could not close COM port!");
            
            return dataPoint;
        }
    }
}
