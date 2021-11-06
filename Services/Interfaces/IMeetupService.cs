using Domain.Requests;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.Filters;
using Domain.Responses;

namespace Services
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
