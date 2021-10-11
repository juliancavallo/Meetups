using Domain.Requests;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IMeetupsService
    {
        List<Meetup> Get();

        int Create(MeetupRequest request);

        void Delete(int id);

        void Update(MeetupRequest request);
    }
}
