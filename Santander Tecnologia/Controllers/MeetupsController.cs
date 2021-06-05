using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Santander_Tecnologia.Data;
using Santander_Tecnologia.Models;

namespace Santander_Tecnologia.Controllers
{
    public class MeetupsController : Controller
    {
        private readonly Santander_TecnologiaContext _context;
        HttpClientHandler _clientHandler = new HttpClientHandler();


        public MeetupsController(Santander_TecnologiaContext context)
        {
            _context = context;
            _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sllPolicy) => { return true; };
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Meetup.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meetup = await _context.Meetup
                .FirstOrDefaultAsync(m => m.Id == id);
            if (meetup == null)
            {
                return NotFound();
            }

            return View(meetup);
        }

        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MeetupDate,Attendees")] Meetup meetup)
        {
            if (ModelState.IsValid)
            {
                meetup.Temperature = await this.GetDayTemperature(meetup.MeetupDate);

                _context.Add(meetup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(meetup);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meetup = await _context.Meetup.FindAsync(id);
            if (meetup == null)
            {
                return NotFound();
            }
            return View(meetup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MeetupDate,Attendees")] Meetup meetup)
        {
            if (id != meetup.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    meetup.Temperature = await this.GetDayTemperature(meetup.MeetupDate);
                    _context.Update(meetup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MeetupExists(meetup.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(meetup);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meetup = await _context.Meetup
                .FirstOrDefaultAsync(m => m.Id == id);
            if (meetup == null)
            {
                return NotFound();
            }

            return View(meetup);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var meetup = await _context.Meetup.FindAsync(id);
            _context.Meetup.Remove(meetup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<decimal> GetDayTemperature(DateTime dateTime)
        {
            using (var httpClient = new HttpClient(_clientHandler))
            {
                using (var response = await httpClient.GetAsync("https://api.openweathermap.org/data/2.5/onecall?lat=-34.60&lon=-58.37&exclude=minutely,hourly&appid=d29d76177ad34ee7bd19d99bdd69e252&units=metric"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var objResponse = JsonConvert.DeserializeObject<WeatherAPIResponse.Root>(apiResponse);

                    var day = objResponse.daily.FirstOrDefault(x => x.Date.Day == dateTime.Day);

                    
                    return day != null ? Convert.ToDecimal((day.temp.max + day.temp.min)/2) : 23;

                }
            }

        }

        private bool MeetupExists(int id)
        {
            return _context.Meetup.Any(e => e.Id == id);
        }
    }
}
