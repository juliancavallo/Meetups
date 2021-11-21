using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Meetups.Domain.Interfaces.Services
{
    public interface IWeatherAPIService
    {
        decimal GetDayTemperature(DateTime dateTime);
    }
}
