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
        private readonly ApplicationDbContext context;
        private readonly IWeatherAPIService weatherAPIService;

        public MeetupService(ApplicationDbContext context, IWeatherAPIService weatherAPIService)
        {
            this.context = context;
            this.weatherAPIService = weatherAPIService;
        }

        public MeetupsSearchResponse Get(MeetupSearchFilter filter)
        {
            var q = context.Meetup.Include(m => m.Organizer).Include("Attendees.User");

            if (!string.IsNullOrWhiteSpace(filter.Description))
                q = q.Where(x => x.Description.Contains(filter.Description));

            if (filter.DateFrom.HasValue)
                q = q.Where(x => x.MeetupDate > filter.DateFrom.Value);

            if (filter.DateTo.HasValue)
                q = q.Where(x => x.MeetupDate < filter.DateTo.Value);

            if (filter.Id.HasValue)
                q = q.Where(x => x.Id == filter.Id.Value);

            return new MeetupsSearchResponse()
            {
                Meetups = q.ToList()
            };
            
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
            catch(Exception)
            {
                throw;
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
            catch (Exception)
            {
                throw;
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
            catch (Exception)
            {
                throw;
            }
        }

        public void Join(int userId, int meetupId)
        {
            var user = context.User.First(x => x.Id == userId);
            var meetup = context.Meetup.First(x => x.Id == meetupId);

            var attendee = new MeetupUser()
            {
                MeetupId = meetupId,
                UserId = user.Id
            };

            meetup.Attendees.Add(attendee);

            context.Update(meetup);
            context.SaveChanges();
        }

        #region Private methods
        private void AddAttendees(Meetup meetup, List<int> attendees)
        {
            ClearAttendees(meetup);
            foreach (var id in attendees)
            {
                var user = context.User.FirstOrDefault(x => x.Id == id);
                meetup.Attendees.Add(new MeetupUser() { Meetup = meetup, User = user });
            }
        }

        private void ClearAttendees(Meetup meetup)
        {
            if (meetup.Id > 0)
            {
                var toRemove = context.MeetupUser.Where(x => x.MeetupId == meetup.Id);

                context.MeetupUser.RemoveRange(toRemove);

            }
            meetup.Attendees = new List<MeetupUser>();
        }
        #endregion
    }
}
