using Meetups.Domain.Interfaces.Services;
using Meetups.Domain.Models.Responses;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Meetups.Services
{
    public class WeatherAPIService : IWeatherAPIService
    {
        public decimal GetDayTemperature(DateTime dateTime)
        {
            var restClient = new RestClient($"https://api.openweathermap.org/data/2.5/onecall?lat=-34.60&lon=-58.37&exclude=minutely,hourly&appid=d29d76177ad34ee7bd19d99bdd69e252&units=metric")
            {
                Timeout = -1,
            };
            var restRequestGet = new RestRequest(Method.GET);
            restRequestGet.AddHeader("Content-Type", "application/json");

            var apiResponse = restClient.Execute(restRequestGet);
            if (apiResponse.StatusCode == HttpStatusCode.OK)
            {
                var response = JsonConvert.DeserializeObject<WeatherAPIResponse.Root>(apiResponse.Content);
                var day = response.daily.FirstOrDefault(x => x.Date.Day == dateTime.Day);
                return day != null ? Convert.ToDecimal((day.temp.max + day.temp.min) / 2) : 23;
            }
            else
            {
                throw apiResponse.ErrorException;
            }
        }
    }
}
