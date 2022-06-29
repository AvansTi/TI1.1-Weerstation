using System;

namespace Shared.Domain
{
    public class WeatherDataPoint
    {
        public short AvgWindSpeed { get; set; }


        public short Barometer { get; set; }


        public short BattLevel { get; set; }


        public short InsideHum { get; set; }


        public short InsideTemp { get; set; }


        public short OutsideHum { get; set; }


        public short OutsideTemp { get; set; }


        public short RainRate { get; set; }


        public short SolarRad { get; set; }


        public WeatherStation Station { get; set; }


        public short Sunrise { get; set; }


        public short Sunset { get; set; }


        public DateTime Timestamp { get; set; }


        public short Ts { get; set; }

        public short UvLevel { get; set; }


        public short WindSpeed { get; set; }


        public short WindDir { get; set; }


        public short XmitBatt { get; set; }
    }
}