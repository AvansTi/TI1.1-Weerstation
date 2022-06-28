using System;

namespace Server.Domain
{
    public class WeatherDataPoint
    {
        public int AvgWindSpeed { get; set; }


        public int Barometer { get; set; }


        public int BattLevel { get; set; }


        public int InsideHum { get; set; }


        public int InsideTemp { get; set; }


        public int OutsideHum { get; set; }


        public int OutsideTemp { get; set; }


        public int RainRate { get; set; }


        public int SolarRad { get; set; }


        public WeatherStation Station { get; set; }


        public int Sunrise { get; set; }


        public int Sunset { get; set; }


        public DateTime Timestamp { get; set; }


        public int Ts { get; set; }

        public int UvLevel { get; set; }


        public int WindSpeed { get; set; }


        public int WindDir { get; set; }


        public int XmitBatt { get; set; }
    }
}