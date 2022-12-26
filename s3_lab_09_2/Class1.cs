using System;
using System.Net.Http;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;
using System.Windows.Forms;

namespace s3_lab_09_2
{
    public class Forecast
    {
        public struct Weather
        {
            public string Country, Name, Description;
            public int Temperature;

            public Weather(string country, string name, string description, int temp) => (Country, Name, Description, Temperature) = (country, name, description, temp);

            override public string ToString() => $"Country: {Country}, Name: {Name}, Description: {Description}, Temp: {Temperature}";
        }

        async Task<Weather> getWeatherAsync(string city, double lat, double lon)
        {

            using (var client = new HttpClient())
            {
                    Random rand = new Random();
                    string appID = "cd805fd448ea5625ff9dd992e8c6dc6e";

                    string Data = await client.GetStringAsync($"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={appID}");
                    
                    Regex rxCountry = new Regex("(?<=\"country\":\")[^\"]+");
                    Regex rxName = new Regex("(?<=\"name\":\")[^\"]+");
                    Regex rxDescription = new Regex("(?<=\"description\":\")[^\"]+");
                    Regex rxTemp = new Regex("(?<=\"temp\":).{3}");

                    MatchCollection country = rxCountry.Matches(Data);
                    MatchCollection name = rxName.Matches(Data);
                    MatchCollection description = rxDescription.Matches(Data);
                    MatchCollection temp = rxTemp.Matches(Data);
                    //Console.WriteLine("Add");
                    return (new Weather(country[0].ToString(), name[0].ToString(), description[0].ToString(), (int.Parse(temp[0].ToString()) - 273)));
            }
        }

        async public Task<Weather[]> getForecastAsync()
        {
            
            var WeatherLineTask = File.ReadLines("city.txt").Select(async line =>
            {
                var data = line.Split(new char[] { ' ', ',', '\t' }).ToList();
                data = data.Where(val => val != "").ToList();
                while (data.Count() > 3)
                    for (int i = 0; i < data.Count() - 3; i++)
                    {
                        data[0] += " " + data[1];
                        data.RemoveAt(1);
                    }

                Weather wthr = await Task.Run(() =>
                {
                    //Console.WriteLine($"{data[0]}, {data[1]}, {data[2]}");
                    return getWeatherAsync(data[0],
                        Convert.ToDouble(data[1].Replace(".", ",")),
                        Convert.ToDouble(data[2].Replace(".", ",")));
                });
                return wthr;
            });
            Weather[] results = await Task.WhenAll(WeatherLineTask);
            //
            return results;
              /**/ 
        }
    }

}
