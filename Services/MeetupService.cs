using Domain.Filters;
using Domain.Models;
using Domain.Requests;
using Domain.Responses;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class MeetupService : IMeetupService
    {
        private readonly Santander_TecnologiaContext context;
        private readonly IWeatherAPIService weatherAPIService;

        public MeetupService(Santander_TecnologiaContext context, IWeatherAPIService weatherAPIService)
        {
            this.context = context;
            this.weatherAPIService = weatherAPIService;
        }

        public List<Meetup> Get(MeetupSearchFilter filter)
        {
            var q = context.Meetup.Include(m => m.Organizer).Include("Attendees.User");

            if (!string.IsNullOrWhiteSpace(filter.Description))
                q = q.Where(x => x.Description.Contains(filter.Description));

            if (filter.DateFrom != null)
                q = q.Where(x => x.MeetupDate > filter.DateFrom);


            if (filter.DateTo != null)
                q = q.Where(x => x.MeetupDate < filter.DateTo);

            return q.ToList();
        }

        public int Create(MeetupRequest request)
        {
            try
            {
                var meetup = new Meetup()
                {
                    Description = request.Description,
                    MeetupDate = request.MeetupDate,
                    Temperature = weatherAPIService.GetDayTemperature(request.MeetupDate),
                    OrganizerId = 1
                };

                AddAttendees(meetup, request.Attendees);

                context.Add(meetup);
                context.SaveChanges();

                return meetup.Id;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(int id)
        {
            try
            {
                var meetup = context.Meetup.FirstOrDefault(x => x.Id == id);

                if (meetup == null)
                    throw new Exception("Not found");

                context.Meetup.Remove(meetup);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Update(MeetupRequest request)
        {
            try
            {
                var meetup = context.Meetup.First(x => x.Id == request.Id);

                meetup.Description = request.Description;
                meetup.MeetupDate = request.MeetupDate;
                meetup.Temperature = weatherAPIService.GetDayTemperature(request.MeetupDate);
                meetup.OrganizerId = 1;

                AddAttendees(meetup, request.Attendees);

                context.Update(meetup);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Private methods
        private void AddAttendees(Meetup meetup, List<int> attendees)
        {
            ClearAttendees(meetup);
            foreach (var id in attendees)
            {
                var user = context.User.FirstOrDefault(x => x.Id == id);
                meetup.Attendees.Add(new MeetupUsers() { Meetup = meetup, User = user });
            }
        }

        private void ClearAttendees(Meetup meetup)
        {
            if (meetup.Id > 0)
            {
                var toRemove = context.MeetupUsers.Where(x => x.MeetupId == meetup.Id);

                context.MeetupUsers.RemoveRange(toRemove);

            }
            meetup.Attendees = new List<MeetupUsers>();
        }
        #endregion
    }
}
