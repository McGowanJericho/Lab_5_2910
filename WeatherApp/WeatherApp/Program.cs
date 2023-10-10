///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	Solution/Project:  Lab 5 - WeatherApp
//	File Name:         Program.cs
//	Course:            CSCI-2910
//	Author:            Jericho McGowan, mcgowanj2@etsu.edu, East Tennessee State University
//	Created:           10/04/2023
//
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Text.Json;

namespace WeatherApp
{
    internal class Program
    {
        static async Task Main()
        {

            bool endProgram = false;
            int programRunCount = 0;
            Console.WriteLine("Welcome to the Weather App! Please input the exact latitude and longitude of the location you wish to view the weather info of.");
            while (endProgram == false)
            {
                bool yesOrNo = false;
                if (programRunCount > 0)
                {
                    while (yesOrNo == false)
                    {

                        string userInput = "";
                        Console.Write("Would you like to continue the program? (Y or N) : ");
                        userInput = Console.ReadLine().ToUpper();
                        if (userInput == "Y")
                        {
                            Console.WriteLine("\nPlease Input new coordinates:");
                            yesOrNo = true;
                        }
                        else if (userInput == "N")
                        {
                            Console.WriteLine("Program ended. Thanks for using the Weather App!");
                            endProgram = true;
                            yesOrNo = true;

                        }
                        else
                        {
                            Console.WriteLine("Error occured. Please try inputting if you would like to continue again.");
                        }


                    }
                }
                double latitude = 10.0;
                double longitude = 10.0;

                DateTime time = DateTime.Now;

                using (HttpClient client = new HttpClient())
                {
                    if (endProgram != true)
                    {
                        bool validInput = false;
                        while (validInput == false)
                        {
                            try
                            {
                                Console.Write("Longitude: ");
                                longitude = Math.Round(Convert.ToDouble(Console.ReadLine()), 4);
                                Console.Write("Latitude: ");
                                latitude = Math.Round(Convert.ToDouble(Console.ReadLine()), 4);
                   
                                validInput = true;
                            }
                            catch
                            {
                                Console.WriteLine("Invalid Coordinates, try again.");
                            }
                        }
                        //https://api.open-meteo.com/v1/forecast?latitude=-36&longitude=-82.453601&hourly=temperature_2m&daily=temperature_2m_max,temperature_2m_min,sunrise,sunset,uv_index_max&temperature_unit=fahrenheit&windspeed_unit=mph&precipitation_unit=inch&timezone=America%2FNew_York
                        HttpResponseMessage response = await client.GetAsync($"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&hourly=temperature_2m&daily=temperature_2m_max,temperature_2m_min,sunrise,sunset,uv_index_max&temperature_unit=fahrenheit&windspeed_unit=mph&precipitation_unit=inch&timezone=America%2FNew_York");

                        if (response.IsSuccessStatusCode)
                        {
                            string currentTimeString = time.ToString("HHmm");

                            string jsonResponse = await response.Content.ReadAsStringAsync();
                            WeatherInfo weatherInfo = JsonSerializer.Deserialize<WeatherInfo>(jsonResponse);



                            int currentTime = Convert.ToInt32(RoundToNearestHour(time));
                            string[] sunriseTimeSplit = weatherInfo.daily.sunrise[0].Split('T');
                            string[] sunsetTimeSplit = weatherInfo.daily.sunset[0].Split('T');

                            Console.WriteLine($"-------------------------------------------------------------------" +
                                $"\nInfo for Coordinates tomorrow at \n" +
                                $"Long: {weatherInfo.longitude} | Lat: {weatherInfo.latitude} | Elevation: {weatherInfo.elevation}ft\n" +
                                $"Timezone: {weatherInfo.timezone_abbreviation} | Time (HHmm):{currentTimeString}\n" +
                                $"Sunrise Time: {sunriseTimeSplit[1]} EST | Sunset Time: {sunsetTimeSplit[1]} EST | UV Index Max: {weatherInfo.daily.uv_index_max[0]}");




                            #region Display Temperature tomorrow based on current time
                            if (currentTime >= 0 && currentTime <= 2400)
                            {
                                int i = currentTime / 100;
                                Console.WriteLine($"Temperature at current time - Temp:{weatherInfo.hourly.temperature_2m[i]}{weatherInfo.hourly_units.temperature_2m}\n" +
                                    $"-------------------------------------------------------------------");
                                programRunCount++;
                            }
                            else
                            {
                                Console.WriteLine("Error occured.");
                                //Should never be able to run
                            }


                            #endregion

                            #region Viewing Temperature at a different time
                            validInput = false;
                            while (validInput == false)
                            {
                                try
                                {


                                    Console.WriteLine("Would you like to view the temperature at a different time less than 7 days from now?(Y or N)");
                                    string input = Console.ReadLine().ToUpper();
                                    if (input == "Y")
                                    {
                                        Console.WriteLine("How many days ahead would you like to see the temperature for? (1-6)");
                                        int dayInput = Convert.ToInt32(Console.ReadLine());

                                        if (dayInput >= 1 && dayInput <= 6)
                                        {
                                            bool validInputTime = false;
                                            while (validInputTime == false)
                                            {


                                                Console.WriteLine($"What time would you like to see the temperature for {dayInput} day(s) from now (in HHMM format)?");
                                                int timeInput = Convert.ToInt32(Console.ReadLine());


                                                if (timeInput >= 0 && timeInput <= 2400)
                                                {


                                                    timeInput = timeInput + dayInput * 2400;

                                                    if (timeInput >= 0 && timeInput <= 16800)
                                                    {

                                                        int i = timeInput / 100;
                                                        string[] hourlyTimeSplit = weatherInfo.hourly.time[i].Split('T');
                                                        Console.WriteLine($"Predicted Temperature on {hourlyTimeSplit[0]} at {hourlyTimeSplit[1]} - Temp: {weatherInfo.hourly.temperature_2m[i]}{weatherInfo.hourly_units.temperature_2m}\n" +
                                                            $"-------------------------------------------------------------------");
                                                        validInputTime = true;
                                                        validInput = true;

                                                    }
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Invalid Input, must be between 0000 and 2400. Please try again.");


                                                }
                                            }

                                        

                                        }
                                        else
                                        {
                                            Console.WriteLine("Invalid time input value.");

                                        }


                                    }
                                    else if (input == "N")
                                    {
                                        Console.WriteLine("Program ended. Thanks for using the Weather App!");

                                        endProgram = true;
                                        validInput = true;


                                    }
                                    else
                                    {
                                        Console.WriteLine("Try again...");
                                    }
                                }
                                catch
                                {
                                    Console.WriteLine("Oops invalid input try again");
                                }
                            }
                        }
                        #endregion





                        else
                        {
                            Console.WriteLine("Error fetching weather data. Please try again.");
                        }
                    }
                }
            }
        
            
        


        }
        static string RoundToNearestHour(DateTime dateTime)
        {
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            
            DateTime roundedDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0);
            string formattedTime = roundedDateTime.ToString("HHmm");
            return formattedTime;
        }
        #region Failed 
        /*
        if(currentTime == 0000)
        {
            Console.WriteLine($"PredictedTemperature at current time tomorrow: {weatherInfo.hourly.time[0]} - Temp: {weatherInfo.hourly.temperature_2m[0]}{weatherInfo.hourly_units.temperature_2m}");
        }
        else if (currentTime == 0100)
        {
            Console.WriteLine($"Predicted Temperature at current time tomorrow: {weatherInfo.hourly.time[1]} - Temp: {weatherInfo.hourly.temperature_2m[1]}{weatherInfo.hourly_units.temperature_2m}");
        }
        else if(currentTime == 0200)                               
        {                                                          
            Console.WriteLine($"Predicted Temperature at current time tomorrow: {weatherInfo.hourly.time[2]} - Temp: {weatherInfo.hourly.temperature_2m[2]} {weatherInfo.hourly_units.temperature_2m}");
        }                                                          
        else if (currentTime == 0300)                              
        {                                                          
            Console.WriteLine($"Predicted Temperature at current time tomorrow: {weatherInfo.hourly.time[3]} - Temp: {weatherInfo.hourly.temperature_2m[3]}");
        }                                                         
        else if (currentTime == 0400)                             
        {                                                         
            Console.WriteLine($"Predicted Temperature at current time tomorrow: {weatherInfo.hourly.time[4]} - Temp: {weatherInfo.hourly.temperature_2m[4]} {weatherInfo.hourly_units.temperature_2m}");
        }                                                        
        else if (currentTime == 0500)                            
        {                                                        
            Console.WriteLine($"Predicted Temperature at current time tomorrow: {weatherInfo.hourly.time[5]} - Temp: {weatherInfo.hourly.temperature_2m[5]} {weatherInfo.hourly_units.temperature_2m}");
        }                                                      
        else if (currentTime == 0600)                          
        {                                                      
            Console.WriteLine($"Predicted Temperature at current time tomorrow: {weatherInfo.hourly.time[6]} - Temp: {weatherInfo.hourly.temperature_2m[6]} {weatherInfo.hourly_units.temperature_2m}");
        }                                                       
        else if (currentTime == 0700)                           
        {                                                       
            Console.WriteLine($"Predicted Temperature at current time tomorrow: {weatherInfo.hourly.time[7]} - Temp: {weatherInfo.hourly.temperature_2m[7]}{weatherInfo.hourly_units.temperature_2m}");
        }                                                          
        else if (currentTime == 0800)                              
        {                                                          
            Console.WriteLine($"Predicted Temperature at current time tomorrow: {weatherInfo.hourly.time[8]} - Temp: {weatherInfo.hourly.temperature_2m[8]} {weatherInfo.hourly_units.temperature_2m}");
        }                                                            
        else if (currentTime == 0900)                                
        {                                                            
            Console.WriteLine($"Predicted Temperature at current time tomorrow: {weatherInfo.hourly.time[9]} - Temp: {weatherInfo.hourly.temperature_2m[9]} {weatherInfo.hourly_units.temperature_2m}");
        }                                                         
        else if (currentTime == 1000)                             
        {                                                         
            Console.WriteLine($"Predicted Temperature at current time tomorrow: {weatherInfo.hourly.time[10]} - Temp: {weatherInfo.hourly.temperature_2m[10]} {weatherInfo.hourly_units.temperature_2m}");
        }                                                            
        else if (currentTime == 1100)                                
        {                                                            
            Console.WriteLine($"Predicted Temperature at current time tomorrow: {weatherInfo.hourly.time[11]} - Temp: {weatherInfo.hourly.temperature_2m[11]} {weatherInfo.hourly_units.temperature_2m}");
        }                                                          
        else if (currentTime == 1200)                              
        {                                                          
            Console.WriteLine($"Predicted Temperature at current time tomorrow: {weatherInfo.hourly.time[12]} - Temp: {weatherInfo.hourly.temperature_2m[12]} {weatherInfo.hourly_units.temperature_2m}");
        }                                                         
        else if (currentTime == 1300)                             
        {                                                         
            Console.WriteLine($"Predicted Temperature at current time tomorrow: {weatherInfo.hourly.time[13]} - Temp: {weatherInfo.hourly.temperature_2m[13]} {weatherInfo.hourly_units.temperature_2m}");
        }                                                           
        else if (currentTime == 1400)                               
        {                                                           
            Console.WriteLine($"Predicted Temperature at current time tomorrow: {weatherInfo.hourly.time[14]} - Temp: {weatherInfo.hourly.temperature_2m[14]} {weatherInfo.hourly_units.temperature_2m}");
        }                                                            
        else if (currentTime == 1500)                                
        {                                                            
            Console.WriteLine($"Predicted Temperature at current time tomorrow: {weatherInfo.hourly.time[15]} - Temp: {weatherInfo.hourly.temperature_2m[15]} {weatherInfo.hourly_units.temperature_2m}");
        }                                                           
        else if (currentTime == 1600)                               
        {                                                           
            Console.WriteLine($"Predicted Temperature at current time tomorrow: {weatherInfo.hourly.time[16]} - Temp: {weatherInfo.hourly.temperature_2m[16]} {weatherInfo.hourly_units.temperature_2m}");
        }                                                            
        else if (currentTime == 1700)                                
        {                                                            
            Console.WriteLine($"Predicted Temperature at current time tomorrow: {weatherInfo.hourly.time[17]} - Temp: {weatherInfo.hourly.temperature_2m[17]} {weatherInfo.hourly_units.temperature_2m}");
        }                                                            
        else if (currentTime == 1800)                                
        {                                                            
            Console.WriteLine($"Predicted Temperature at current time tomorrow: {weatherInfo.hourly.time[18]} - Temp: {weatherInfo.hourly.temperature_2m[18]} {weatherInfo.hourly_units.temperature_2m}");
        }                                                        
        else if (currentTime == 1900)                            
        {                                                        
            Console.WriteLine($"Predicted Temperature at current time tomorrow: {weatherInfo.hourly.time[19]} - Temp: {weatherInfo.hourly.temperature_2m[19]} {weatherInfo.hourly_units.temperature_2m}");
        }                                                         
        else if (currentTime == 2000)                             
        {                                                         
            Console.WriteLine($"Predicted Temperature at current time tomorrow: {weatherInfo.hourly.time[20]} - Temp: {weatherInfo.hourly.temperature_2m[20]} {weatherInfo.hourly_units.temperature_2m}");
        }                                                           
        else if (currentTime == 2100)                               
        {                                                           
            Console.WriteLine(  $"Predicted Temperature at current time tomorrow: {weatherInfo.hourly.time[21]} - Temp: {weatherInfo.hourly.temperature_2m[21]} {weatherInfo.hourly_units.temperature_2m}");
        }                                                         
        else if (currentTime == 2200)                             
        {                                                         
            Console.WriteLine($"Predicted Temperature at current time tomorrow: {weatherInfo.hourly.time[22]} - Temp: {weatherInfo.hourly.temperature_2m[22]} {weatherInfo.hourly_units.temperature_2m}");
        }                                                             
        else if (currentTime == 2300)                                 
        {                                                             
            Console.WriteLine(  $"Predicted Temperature at current time tomorrow: {weatherInfo.hourly.time[23]} - Temp: {weatherInfo.hourly.temperature_2m[23]}{weatherInfo.hourly_units.temperature_2m}");

        }                                                          
        else if (currentTime == 2400)                              
        {                                                          
            Console.WriteLine($"Predicted Temperature at current time tomorrow: {weatherInfo.hourly.time[24]} - Temp: {weatherInfo.hourly.temperature_2m[24]}{weatherInfo.hourly_units.temperature_2m}");
        }
        */
        #endregion
        #region Waste of time because API does not seem to actually know the time zone

        /*
                                if (weatherInfo.timezone_abbreviation == "GMT" || weatherInfo.timezone_abbreviation == "UTC")
                                {
                                    //Nothing time is already in GMT
                                    currentTimeString = currentTimeGMT.ToString();

                                }
                                else if(weatherInfo.timezone_abbreviation == "ECT")
                                {
                                    currentTimeGMT = currentTimeGMT + 0100;
                                    currentTimeString = currentTimeGMT.ToString();
                                }
                                else if (weatherInfo.timezone_abbreviation == "EET" || weatherInfo.timezone_abbreviation == "ART")
                                {
                                    currentTimeGMT = currentTimeGMT + 0200;
                                    currentTimeString = currentTimeGMT.ToString();
                                }
                                else if (weatherInfo.timezone_abbreviation == "EAT")
                                {
                                    currentTimeGMT = currentTimeGMT + 0300;
                                    currentTimeString = currentTimeGMT.ToString();
                                }
                                else if (weatherInfo.timezone_abbreviation == "MET")
                                {
                                    currentTimeGMT = currentTimeGMT + 0330;
                                    currentTimeString = currentTimeGMT.ToString();
                                }
                                else if (weatherInfo.timezone_abbreviation == "NET")
                                {
                                    currentTimeGMT = currentTimeGMT + 0400;
                                    currentTimeString = currentTimeGMT.ToString();
                                }
                                else if (weatherInfo.timezone_abbreviation == "PLT")
                                {
                                    currentTimeGMT = currentTimeGMT + 0500;
                                    currentTimeString = currentTimeGMT.ToString();
                                }
                                else if (weatherInfo.timezone_abbreviation == "IST")
                                {
                                    currentTimeGMT = currentTimeGMT + 0530;
                                    currentTimeString = currentTimeGMT.ToString();
                                }
                                else if (weatherInfo.timezone_abbreviation == "BST")
                                {
                                    currentTimeGMT = currentTimeGMT + 0600;
                                    currentTimeString = currentTimeGMT.ToString();
                                }
                                else if (weatherInfo.timezone_abbreviation == "VST")
                                {
                                    currentTimeGMT = currentTimeGMT + 0700;
                                    currentTimeString = currentTimeGMT.ToString();
                                }
                                else if (weatherInfo.timezone_abbreviation == "CTT")
                                {
                                    currentTimeGMT = currentTimeGMT + 0800;
                                    currentTimeString = currentTimeGMT.ToString();
                                }
                                else if (weatherInfo.timezone_abbreviation == "JST")
                                {
                                    currentTimeGMT = currentTimeGMT + 0900;
                                    currentTimeString = currentTimeGMT.ToString();
                                }
                                else if (weatherInfo.timezone_abbreviation == "ACT")
                                {
                                    currentTimeGMT = currentTimeGMT + 0930;
                                    currentTimeString = currentTimeGMT.ToString();
                                }
                                else if (weatherInfo.timezone_abbreviation == "AET")
                                {
                                    currentTimeGMT = currentTimeGMT + 1000;
                                    currentTimeString = currentTimeGMT.ToString();
                                }
                                else if (weatherInfo.timezone_abbreviation == "SST")
                                {
                                    currentTimeGMT = currentTimeGMT + 1100;
                                    currentTimeString = currentTimeGMT.ToString();
                                }
                                else if (weatherInfo.timezone_abbreviation == "NST")
                                {
                                    currentTimeGMT = currentTimeGMT + 1200;
                                    currentTimeString = currentTimeGMT.ToString();
                                }
                                else if (weatherInfo.timezone_abbreviation == "MIT")
                                {
                                    currentTimeGMT = currentTimeGMT - 1100;
                                    currentTimeString = currentTimeGMT.ToString();
                                }
                                else if (weatherInfo.timezone_abbreviation == "HST")
                                {
                                    currentTimeGMT = currentTimeGMT - 1000;
                                    currentTimeString = currentTimeGMT.ToString();
                                }
                                else if (weatherInfo.timezone_abbreviation == "AST")
                                {
                                    currentTimeGMT = currentTimeGMT - 0900;
                                    currentTimeString = currentTimeGMT.ToString();
                                }
                                else if (weatherInfo.timezone_abbreviation == "PST" || weatherInfo.timezone_abbreviation == "PNT")
                                {
                                    currentTimeGMT = currentTimeGMT - 0800;
                                    currentTimeString = currentTimeGMT.ToString();
                                }
                                else if (weatherInfo.timezone_abbreviation == "MST")
                                {
                                    currentTimeGMT = currentTimeGMT - 0700;
                                    currentTimeString = currentTimeGMT.ToString();
                                }
                                else if (weatherInfo.timezone_abbreviation == "CST")
                                {
                                    currentTimeGMT = currentTimeGMT - 0600;
                                    currentTimeString = currentTimeGMT.ToString();
                                }
                                else if (weatherInfo.timezone_abbreviation == "EST" || weatherInfo.timezone_abbreviation == "IET")
                                {
                                    currentTimeGMT = currentTimeGMT - 0500;
                                    currentTimeString = currentTimeGMT.ToString();
                                }
                                else if (weatherInfo.timezone_abbreviation == "PRT")
                                {
                                    currentTimeGMT = currentTimeGMT - 0400;
                                    currentTimeString = currentTimeGMT.ToString();
                                }
                                else if (weatherInfo.timezone_abbreviation == "CNT")
                                {
                                    currentTimeGMT = currentTimeGMT - 0330;
                                    currentTimeString = currentTimeGMT.ToString();
                                }
                                else if (weatherInfo.timezone_abbreviation == "AGT")
                                {
                                    currentTimeGMT = currentTimeGMT - 0300;
                                    currentTimeString = currentTimeGMT.ToString();
                                }
                                else if (weatherInfo.timezone_abbreviation == "BET")
                                {
                                    currentTimeGMT = currentTimeGMT - 0300;
                                    currentTimeString = currentTimeGMT.ToString();
                                }
                                else if (weatherInfo.timezone_abbreviation == "CAT")
                                {
                                    currentTimeGMT = currentTimeGMT - 0100;
                                    currentTimeString = currentTimeGMT.ToString();
                                }
                                else
                                {

                                    currentTimeString = "Unknown Timezone";
                                }
        */




        #endregion
    }
}