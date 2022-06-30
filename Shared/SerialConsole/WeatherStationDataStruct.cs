using System;
using System.Collections.Generic;

namespace Shared.SerialConsole
{
    public class WeatherStationDataStruct
    {
		//	type		varnaam		offset	Omschrijving
		//      ----------------------------------------------------------------------
		public char c1;			// 0	karakter "L"
		public char c2;			// 1	karakter "O"
		public char c3;			// 2	karakter "O"
		public char c4;			// 3	karakter "P" (RevA) of actuele barometer trend als volgt
							//    	196 = Falling Rapidly
							//    	236 = Falling Slowly
							//    	0   = Steady
							//    	20  = Rising Slowly
							//    	60  = Rising Rapidly
							//	80  = Rev A firmware. ASCII "P"
		public byte PacketType;	// 4 	Altijd zero. Voor toekomstige uitbreiding
		public ushort NextRec;		// 5 	Next Record. Gebruiken we niet
		public ushort Barometer;   // 7 	Huide barometerwaarde (Hg/1000)
		public short InsideTemp;   // 9 	Temperatuur binnen (10den Farenheid)
		public byte InsideHum;		// 11 	Relatieve luchtvochtigheid binnen (percentage)
		public short OutsideTemp;  // 12 	Temperatuur buiten (10den Farenheid
		public byte WindSpeed;		// 14 	Windsnelheid (mph)
		public byte AvgWindSpeed;	// 15 	Windsnelheid gemiddelde afgelopen 10 minuten (mph)
		public ushort WindDir;		// 16 	Windrichting (graden)
		public byte[] XtraTemps;	// 18 	Temperaturen optionele opnemers (niet gebruikt) => byte XtraTemps[7]
		public byte[] SoilTemps;	// 25 	Temperaturen soils (niet gebruikt) => byte SoilTemps[4]
		public byte[] LeafTemps;	// 29 	Temperaturen bladeren (niet gebruikt) => byte LeafTemps[4]
		public byte OutsideHum;	// 33 	Relatieve luchtvochtigheid buiten (percentage)
		public byte[] XtraHums;	// 34 	Relatieve luchtvochtigheid optionele sensoren (percentage) => byte XtraHums[7]
		public ushort RainRate;    // 41 	Regenmeter (clicks/uur, 1 click = 0.2mm)
		public byte UVLevel;		// 43 	UV index (??)
		public ushort SolarRad;    // 44 	Zonlicht sterkte (watt/meter)
		public ushort StormRain;   // 46 	Storm rain (100ste inch) ??
		public ushort StormStart;  // 48 	Start datum storm (??)
		public ushort RainDay;		// 50 	Regen vandaag (clicks, 1 click = 0.2mm)
		public ushort RainMonth;   // 52 	Regen maand (clicks, 1 click = 0.2mm)
		public ushort RainYear;    // 54 	Regen jaar (clicks, 1 click = 0.2mm)
		public ushort ETDay;       // 56 	Day ET ??
		public ushort ETMonth;		// 58 	Month ET ??
		public ushort ETYear;      // 60 	Year ET ??
		public ulong SoilMoist;    // 62 	Soil Moistures (niet gebruikt)
		public ulong LeafWet;		// 66 	Leaf Wetness (niet gebruikt)
		public byte AlarmInside;	// 70 	Inside Alarm bits (niet gebruikt)
		public byte AlarmRain;		// 71 	Regen alarm (niet gebruikt)
		public ushort AlarmOut;    // 72 	Temperatuur buiten alarm (niet gebruikt)
		public byte[] AlarmXtra;	// 74 	Optione alarmen (niet gebruikt)		=> byte AlarmXtra[8]
		public ulong AlarmSL;		// 82 	Blad en grond alarm (niet gebruikt)
		public byte XmitBatt;		// 86 	Transmitter accu status
		public ushort BattLevel;   // 87 	Console accu spanning = ((wBattLevel * 300)/512)/100.0)
		public byte ForeIcon;		// 89 	Weersvoorspelling icoon (niet gebruikt)
		public byte Rule;			// 90 	Forecast rule number (??)
		public ushort Sunrise;		// 91 	Zonsopkomst (uur*100+min)
		public ushort Sunset;      // 93 	Zonsondergang (uur*100+min)
		public byte LF;			// 95 	(\n) 0x0a
		public byte CR;			// 96 	(\r) 0x0d
		public ushort CRC;         // 97 	CRC check bytes (CCITT-16 standard)

        public void Fill(byte[] bytes)
        {
			c1 = (char)bytes[0];
			c2 = (char)bytes[1];
			c3 = (char)bytes[2];
			c4 = (char)bytes[3];
			PacketType = bytes[4];
			NextRec = BitConverter.ToUInt16(bytes[5..7]);
			Barometer = BitConverter.ToUInt16(bytes[7..9]);
			InsideTemp = BitConverter.ToInt16(bytes[9..11]);
			InsideHum = bytes[11];
			OutsideTemp = BitConverter.ToInt16(bytes[12..14]);
			WindSpeed = bytes[14];
			AvgWindSpeed = bytes[15];
			WindDir = BitConverter.ToUInt16(bytes[16..18]);
			XtraTemps = bytes[18..25];
			SoilTemps = bytes[25..29];
			LeafTemps = bytes[29..33];
			OutsideHum = bytes[33];
			XtraHums = bytes[34..41];
			RainRate = BitConverter.ToUInt16(bytes[41..43]);
			UVLevel = bytes[43];
			SolarRad = BitConverter.ToUInt16(bytes[44..46]);
			StormRain = BitConverter.ToUInt16(bytes[46..48]);
			StormStart = BitConverter.ToUInt16(bytes[48..50]);
			RainDay = BitConverter.ToUInt16(bytes[50..52]);
			RainMonth = BitConverter.ToUInt16(bytes[52..54]);
			RainYear = BitConverter.ToUInt16(bytes[54..56]);
			ETDay = BitConverter.ToUInt16(bytes[56..58]);
			ETMonth = BitConverter.ToUInt16(bytes[58..60]);
			ETYear = BitConverter.ToUInt16(bytes[60..62]);
			SoilMoist = BitConverter.ToUInt32(bytes[62..66]);
			LeafWet = BitConverter.ToUInt32(bytes[66..70]);
			AlarmInside = bytes[70];
			AlarmRain = bytes[71];
			AlarmOut = BitConverter.ToUInt16(bytes[72..74]);
			AlarmXtra = bytes[74..82];
			AlarmSL = BitConverter.ToUInt32(bytes[82..86]);
			XmitBatt = bytes[86];
			BattLevel = BitConverter.ToUInt16(bytes[87..89]);
			ForeIcon = bytes[89];
			Rule = bytes[90];
			Sunrise = BitConverter.ToUInt16(bytes[91..93]);
			Sunset = BitConverter.ToUInt16(bytes[93..95]);
			LF = bytes[95];
			CR = bytes[96];
			CRC = BitConverter.ToUInt16(bytes[97..99]);
		}
    }
}
