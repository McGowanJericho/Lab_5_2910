﻿///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	Solution/Project:  Lab 5 - WeatherApp
//	File Name:         Daily.cs
//	Course:            CSCI-2910
//	Author:            Jericho McGowan, mcgowanj2@etsu.edu, East Tennessee State University
//	Created:           10/04/2023
//
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp
{
    internal class Daily
    {
        public List<string> time { get; set; }
        public List<double> temperature_2m_max { get; set; }
        public List<double> temperature_2m_min { get; set; }
        public List<string> sunrise { get; set; }
        public List<string> sunset { get; set; }
        public List<double> uv_index_max { get; set; }
    }
}
