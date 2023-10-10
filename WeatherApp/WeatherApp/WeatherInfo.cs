using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	Solution/Project:  Lab 5 - WeatherApp
//	File Name:         WeatherInfo.cs
//	Course:            CSCI-2910
//	Author:            Jericho McGowan, mcgowanj2@etsu.edu, East Tennessee State University
//	Created:           10/04/2023
//
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
namespace WeatherApp
{
    internal class WeatherInfo
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
        public double elevation { get; set; }
        public double generationtime_ms { get; set; }
        public int utc_offset_seconds { get; set; }
        public string timezone { get; set; }
        public string timezone_abbreviation { get; set; }
        public HourlyData hourly { get; set; }
        public HourlyUnits hourly_units { get; set; }
        public DailyUnits daily_units { get; set; }
        public Daily daily { get; set; }

    }
}
