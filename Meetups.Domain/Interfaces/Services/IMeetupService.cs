using Meetups.Domain.Models.Requests;
using Meetups.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Meetups.Domain.Filters;
using Meetups.Domain.Models.Responses;

namespace Meetups.Domain.Interfaces.Services
{
    public interface IMeetupService
    {
        MeetupsSearchResponse Get(MeetupSearchFilter filter);

        int Create(MeetupRequest request);

        void Delete(int id);

        void Update(MeetupRequest request);

        void Join(int userId, int meetupId);
    }
}
