using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;
using static gRPClient.Globals;

namespace gRPClient
{
    public class WeatherStationConnection
    {
        private SerialPort Conn { get; set; }

        /// <summary>
        /// Opens the serial port for communications
        /// </summary>
        /// <returns></returns>
        public int OpenSerialPort(string comport)
        {
            if (comport == null)
            {
                Console.WriteLine("OpenSerialPort: Comport name is null!");
                return NOK;
            }

            Conn = new SerialPort();
            Conn.PortName = comport;
            Conn.BaudRate = 19200;
            Conn.Open();
            
            return OK;
        }

        /// <summary>
        /// This function will try to wake the weather station 3 times.
        /// Every attempt will send a newline character '\n' or hex 0x0A
        /// and wait 1.2 seconds for a character response.
        /// When an awnser is given (characters '\n' and '\r') the device is
        /// awake and standing by.
        /// </summary>
        /// <returns></returns>
        public int WakeUp()
        {
            int result = NOK;
            int idx;
            char[] ch = new char[1] { (char) 0x0A };
            char[] resp = new char[2];

            for (idx = 0; idx < 3 && NOK == result; idx++)
            {
                Conn.Write(ch, 0, 1);

                Thread.Sleep(1200);

                if (Conn.Read(resp, 0, Conn.BytesToRead) > 0)
                {
                    if (resp[0] == '\n' && resp[1] == '\r')
                    {
                        result = OK;
                    }
                    else
                    {
                        Console.WriteLine($"WakeUp: Waking up weather station. Attempt: {idx}");
                        Thread.Sleep(1200);
                    }
                }
            }

            if (NOK == result)
                Console.WriteLine("WakeUp: Could not wake up the weather station!");

            return result;
        }

        public int CloseSerialPort()
        {
            if(Conn != null)
            {
                this.Conn.Close();
                return OK;
            }
            else
            {
                Console.WriteLine("CloseSerialPort: Conn is null. Could not close the serial port!");
                return NOK;
            }
        }
    }
}
