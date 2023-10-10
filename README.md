# Lab_5_2910
##Problems Encountered

1. First Major problem is relating to the JSON deserializer. I had no problems using the API link and receiving a response, but whenever it came to breaking down the response, and storing it into an object it is having issues storing some variables. It turns out this issue stems from me not having the exact same naming convention for each one of my variables as they are in the API. To fix this, I went through each JSON element and created variable to match it.
  
2. I have confusion on the time variable in the daily_units class. It is not actually a display of the local time, I am unsure the usage. To get past this I used the DateTime enum to create a local time here. From there I will create code to manipulate the time based on the timezone_abbreviation variable. Previous attempt did not work. The variable timezone_abbreviation does not collect the local timezone of the coordinates, it instead goes based off of a standard pre determined standard for. It assume GMT if not specified. SO I cannot make a local time at coordinates vs my local time comparison. Settled for displaying my local time.

3. Problem with loops for entire program. It keeps looping back through the using(HttpClient client = new HttpClient()). Unsure as to why even though whole program is encompassed in a while loop that is ended before it is reached. To fix this I set an if statement to only allow the content within using(HttpClient client = new HttpClient()) to be ran whenever endProgram is not set to true. This prevents any code within that section to be ran.
